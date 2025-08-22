using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using EntityClass;
using PhysicsClass;
using GlobalInfo;
using FollowClass;
using SpringClass;
using VerletClass;
using VerletRope;
using MonogamePhysicsSim;
using Simulation;
using System.Text;
using Microsoft.Xna.Framework.Audio;
using System.Threading;
using RopeSimulation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;
using System.Data;
using System.Linq;

namespace ClothEdit;

class ClothEditor
{

    private KeyboardState newKbState;
    private KeyboardState oldKbState;


    private List<List<VerletObject>> points = new List<List<VerletObject>>(); // points[x][y]
    private List<List<Rope>> verticalRopes = new List<List<Rope>>();
    private List<List<Rope>> horizontalRopes = new List<List<Rope>>();


    Rope[] nearestFourRopes = new Rope[4];
    private int selectedPosInArr = 1;
    private Rope selectedRope;


    private int currentPointX = 0;
    private int currentPointY = 0;

    private int maxX = 0;
    private int maxY = 0;


    public bool isSelected = false;

    private Color nearestFourColour = Color.Coral;


    public ClothEditor(List<List<VerletObject>> points, List<List<Rope>> verticalRopes, List<List<Rope>> horizontalRopes)
    {
        this.points = points;
        this.verticalRopes = verticalRopes;
        this.horizontalRopes = horizontalRopes;

        maxX = points.Count;
        maxY = points[0].Count;

        selectedRope = verticalRopes[currentPointX][currentPointY];

        oldKbState = Keyboard.GetState();
    }

    public void Update()
    {
        if (!isSelected)
        {
            Deselect();
            return;
        }
        SolveRope();

        newKbState = Keyboard.GetState();


        if (newKbState.IsKeyDown(Keys.Down) && !oldKbState.IsKeyDown(Keys.Down)) currentPointY = DownPoint();
        if (newKbState.IsKeyDown(Keys.Up) && !oldKbState.IsKeyDown(Keys.Up)) currentPointY = UpPoint();

        if (newKbState.IsKeyDown(Keys.Right) && !oldKbState.IsKeyDown(Keys.Right)) currentPointX = RightPoint();
        if (newKbState.IsKeyDown(Keys.Left) && !oldKbState.IsKeyDown(Keys.Left)) currentPointX = LeftPoint();


        points[currentPointX][currentPointY].renderBall = true;

        if (newKbState.IsKeyDown(Keys.Z) && !oldKbState.IsKeyDown(Keys.Z)) SelectLastRope();
        if (newKbState.IsKeyDown(Keys.X) && !oldKbState.IsKeyDown(Keys.X)) SelectNextRope();
        if (newKbState.IsKeyDown(Keys.C) && !oldKbState.IsKeyDown(Keys.C)) CutRope();
        if (newKbState.IsKeyDown(Keys.N)) CutAll();
        if (newKbState.IsKeyDown(Keys.M)) RepairCloth();
        if (newKbState.IsKeyDown(Keys.G) && !oldKbState.IsKeyDown(Keys.G)) points[currentPointX][currentPointY].stationary = !points[currentPointX][currentPointY].stationary;
        if (newKbState.IsKeyDown(Keys.H)) RepairNearest();
        if (newKbState.IsKeyDown(Keys.J) && !oldKbState.IsKeyDown(Keys.J)) points[currentPointX][currentPointY].grabbed = !points[currentPointX][currentPointY].grabbed;
    

        // Makes stationary points stationary colour, the rest to default colour
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points[i].Count; j++)
            {
                if (points[i][j].stationary) points[i][j].colour = Global.stationaryPointColour;
                else points[i][j].colour = Global.defaultPointColour;
            }
        }

        // Makes selected point the editing point colour and sets the selected rope to the correct rope
        points[currentPointX][currentPointY].colour = Global.editingPointColour;
        selectedRope = nearestFourRopes[selectedPosInArr];

        // Next 2 set vertical and horizontal active and inactive ropes to either render or not
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                if (!verticalRopes[i][j].active) verticalRopes[i][j].renderLine = false;
                else verticalRopes[i][j].renderLine = true;
            }
        }

        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                if (!horizontalRopes[i][j].active) horizontalRopes[i][j].renderLine = false;
                else horizontalRopes[i][j].renderLine = true;
            }
        }

        // Makes nearest 4 always render
        for (int i = 0; i < 4; i++)
        {
            if (nearestFourRopes[i] != null) nearestFourRopes[i].renderLine = true;
        }


        oldKbState = Keyboard.GetState();

    }

    private void Deselect()
    {
        points[currentPointX][currentPointY].renderBall = false;

        // Set all vertical ropes to the default colour, if they are not active then dont render them
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                verticalRopes[i][j].colour = Global.defaultRopeColour;
                if (!verticalRopes[i][j].active) verticalRopes[i][j].colour = Global.backgroundColour;
            }
        }

        // Same as above but horizontal;
        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                horizontalRopes[i][j].colour = Global.defaultRopeColour;
                if (!horizontalRopes[i][j].active) horizontalRopes[i][j].colour = Global.backgroundColour;
            }
        }

        // Stops rendering inactive ropes
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                if (!verticalRopes[i][j].active) verticalRopes[i][j].renderLine = false;
            }
        }

        // Same as above
        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                if (!horizontalRopes[i][j].active) horizontalRopes[i][j].renderLine = false;
            }
        }

        // Sets stationary points to stationary colours, the rest to default colour
        for (int i = 0; i < points.Count; i++)
        {
            for (int j = 0; j < points[i].Count; j++)
            {
                if (points[i][j].stationary) points[i][j].colour = Global.stationaryPointColour;
                else points[i][j].colour = Global.defaultPointColour;
            }
        }


    }


    // Next 4 functions i just array looping
    private int DownPoint()
    {
        int belowPoint = currentPointY;

        if (belowPoint + 1 == maxY) belowPoint = 0;
        else belowPoint++;

        points[currentPointX][currentPointY].renderBall = false;
        points[currentPointX][currentPointY].grabbed = false;

        SolveRope();
        return belowPoint;

    }

    private int UpPoint()
    {
        int abovePoint = currentPointY;

        if (abovePoint - 1 < 0) abovePoint = maxY - 1;
        else abovePoint--;

        points[currentPointX][currentPointY].renderBall = false;
        points[currentPointX][currentPointY].grabbed = false;

        SolveRope();
        return abovePoint;
    }

    private int RightPoint()
    {
        int rightPoint = currentPointX;

        if (rightPoint + 1 == maxX) rightPoint = 0;
        else rightPoint++;

        points[currentPointX][currentPointY].renderBall = false;
        points[currentPointX][currentPointY].grabbed = false;

        SolveRope();
        return rightPoint;

    }

    private int LeftPoint()
    {
        int leftPoint = currentPointX;

        if (leftPoint - 1 < 0) leftPoint = maxX - 1;
        else leftPoint--;

        points[currentPointX][currentPointY].renderBall = false;
        points[currentPointX][currentPointY].grabbed = false;

        SolveRope();
        return leftPoint;
    }

    // Bu default sets them to the colour that they are when part of the selected object, if they are not active it gets set to background colour - while it should set them to not render this does not cause any issues like it did previously, easier to just leave like this
    private void ColourRopes()
    {
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                verticalRopes[i][j].colour = Global.selectedRopeColour;
                if (!verticalRopes[i][j].active) verticalRopes[i][j].colour = Global.backgroundColour;
            }
        }

        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                horizontalRopes[i][j].colour = Global.selectedRopeColour;
                if (!horizontalRopes[i][j].active) horizontalRopes[i][j].colour = Global.backgroundColour;
            }
        }


    }

    private void SolveRope()
    {

        ColourRopes();

        for (int i = 0; i < 4; i++)
        {
            nearestFourRopes[i] = null;
        }


        bool[] offClothRopes = new bool[4] { false, false, false, false };

        //Checks in a direction as to whether it is off cloth or not, if it is then it will skip over this point when setting what the nearest four ropes are

        if (currentPointY == 0) offClothRopes[0] = true; // Above
        if (currentPointY == maxY - 1) offClothRopes[2] = true; // Below

        if (currentPointX == 0) offClothRopes[3] = true; // Left
        if (currentPointX == maxX - 1) offClothRopes[1] = true; // Right 



        //checks each direction known to have a rope and sets the correct spot in nearest four ropes, if there is not a rope there and it is in the position of the selected rope then it will push it to the next possible position
        if (!offClothRopes[0]) nearestFourRopes[0] = verticalRopes[currentPointX][currentPointY - 1]; // Above
        else if (selectedPosInArr == 0) selectedPosInArr++;


        if (!offClothRopes[2]) nearestFourRopes[2] = verticalRopes[currentPointX][currentPointY]; // Below
        else if (selectedPosInArr == 2) selectedPosInArr++;



        if (!offClothRopes[1]) nearestFourRopes[1] = horizontalRopes[currentPointX][currentPointY]; // Right
        else if (selectedPosInArr == 1) selectedPosInArr++;

        if (!offClothRopes[3]) nearestFourRopes[3] = horizontalRopes[currentPointX - 1][currentPointY]; // Left
        else if (selectedPosInArr == 3) selectedPosInArr = 0;



        // Loops over non null nearest four ropes. Sets to coral by default, if its the selected rope then it becomes red, if it is the selected rope but inactive sets to grey if it is in the nearest four not selected but is inactive then sets to light grey
        for (int i = 0; i < nearestFourRopes.Length; i++)
        {
            if (nearestFourRopes[i] == null) continue;
            nearestFourRopes[i].colour = nearestFourColour;

            if (nearestFourRopes[i] == selectedRope) nearestFourRopes[i].colour = Color.Red;

            if (!nearestFourRopes[i].active && nearestFourRopes[i] == selectedRope) nearestFourRopes[i].colour = Color.Gray;
            else if (!nearestFourRopes[i].active) nearestFourRopes[i].colour = Color.LightGray;


        }

    }

    // Next 2 functions are basic array looping
    private void SelectNextRope()
    {
        selectedPosInArr++;
        if (selectedPosInArr > 3) selectedPosInArr = 0;
    }

    private void SelectLastRope()
    {
        selectedPosInArr--;
        if (selectedPosInArr < 0) selectedPosInArr = 3;
    }

    // Sets selected rope activeness toggled
    private void CutRope()
    {
        selectedRope.active = !selectedRope.active;
    }

    // Goes over nearest 4 ropes, if not null then they are set to inactive
    private void CutAll()
    {
        for (int i = 0; i < nearestFourRopes.Length; i++)
        {
            if (nearestFourRopes[i] == null) continue;
            nearestFourRopes[i].active = false;
        }
    }

    // Loops over all ropes and sets them to active, does cause knots
    private void RepairCloth()
    {
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                verticalRopes[i][j].active = true;
            }
        }

        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                horizontalRopes[i][j].active = true;
            }
        }
    }

    private void RepairNearest()
    {
        for (int i = 0; i < 4;   i++)
        {
            if (nearestFourRopes[i] != null) nearestFourRopes[i].active = true;
        }
    }

}