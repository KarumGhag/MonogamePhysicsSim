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

        selectedRope = nearestFourRopes[selectedPosInArr];


        selectedRope.colour = Color.Red;

        oldKbState = Keyboard.GetState();

    }

    private void Deselect()
    {
        points[currentPointX][currentPointY].renderBall = false;
    }


    private int DownPoint()
    {
        int belowPoint = currentPointY;

        if (belowPoint + 1 == maxY) belowPoint = 0;
        else belowPoint++;

        points[currentPointX][currentPointY].renderBall = false;

        SolveRope();
        return belowPoint;

    }

    private int UpPoint()
    {
        int abovePoint = currentPointY;

        if (abovePoint - 1 < 0) abovePoint = maxY - 1;
        else abovePoint--;

        points[currentPointX][currentPointY].renderBall = false;

        SolveRope();
        return abovePoint;
    }

    private int RightPoint()
    {
        int rightPoint = currentPointX;

        if (rightPoint + 1 == maxX) rightPoint = 0;
        else rightPoint++;

        points[currentPointX][currentPointY].renderBall = false;

        return rightPoint;

    }

    private int LeftPoint()
    {
        int leftPoint = currentPointX;

        if (leftPoint - 1 < 0) leftPoint = maxX - 1;
        else leftPoint--;

        points[currentPointX][currentPointY].renderBall = false;

        return leftPoint;
    }

    private void AllRopesGreen()
    {
        for (int i = 0; i < verticalRopes.Count; i++)
        {
            for (int j = 0; j < verticalRopes[i].Count; j++)
            {
                verticalRopes[i][j].colour = Global.selectedRopeColour;
            }
        }

        for (int i = 0; i < horizontalRopes.Count; i++)
        {
            for (int j = 0; j < horizontalRopes[i].Count; j++)
            {
                horizontalRopes[i][j].colour = Global.selectedRopeColour;
            }
        }


    }

    private void SolveRope()
    {

        AllRopesGreen();


        bool[] offClothRopes = new bool[4] { false, false, false, false };

        if (currentPointY == 0) offClothRopes[0] = true; // Above
        if (currentPointY == maxY - 1) offClothRopes[2] = true; // Below

        if (currentPointX == 0) offClothRopes[3] = true; // Left
        if (currentPointX == maxX - 1) offClothRopes[1] = true; // Right 



        if (!offClothRopes[0]) // Above
        {
            verticalRopes[currentPointX][currentPointY - 1].colour = Color.Pink;
            nearestFourRopes[0] = verticalRopes[currentPointX][currentPointY - 1];
        }
        else if (selectedPosInArr == 0) selectedPosInArr++;

        if (!offClothRopes[2]) // Below
        {
            verticalRopes[currentPointX][currentPointY].colour = Color.Pink;
            nearestFourRopes[2] = verticalRopes[currentPointX][currentPointY];
        }
        else if (selectedPosInArr == 2) selectedPosInArr++;

        if (!offClothRopes[3]) // Left
        {
            horizontalRopes[currentPointX - 1][currentPointY].colour = Color.Pink;
            nearestFourRopes[3] = horizontalRopes[currentPointX - 1][currentPointY];
        }
        else if (selectedPosInArr == 3) selectedPosInArr = 0;

        if (!offClothRopes[1]) // Right
        {
            horizontalRopes[currentPointX][currentPointY].colour = Color.Pink;
            nearestFourRopes[1] = horizontalRopes[currentPointX][currentPointY];
        }
        else if (selectedPosInArr == 1) selectedPosInArr++;

    }

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



}