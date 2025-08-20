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

namespace ClothEdit;

class ClothEditor
{

    private KeyboardState newKbState;
    private KeyboardState oldKbState;


    private List<List<VerletObject>> points = new List<List<VerletObject>>(); // points[x][y]
    private List<Rope> verticalRopes = new List<Rope>();
    private List<Rope> horizontalRopes = new List<Rope>();

    private int currentPointX = 0;
    private int currentPointY = 0;

    private int maxX = 0;
    private int maxY = 0;
    private int currentWholeRope = 0;


    public bool isSelected = false;

    public ClothEditor(List<List<VerletObject>> points, List<Rope> verticalRopes, List<Rope> horizontalRopes)
    {
        this.points = points;
        this.verticalRopes = verticalRopes;
        this.horizontalRopes = horizontalRopes;

        maxX = points.Count;
        maxY = points[0].Count;

        oldKbState = Keyboard.GetState();
    }

    public void Update()
    {
        if (!isSelected)
        {
            Deselect();
            return;
        }

        newKbState = Keyboard.GetState();


        if (newKbState.IsKeyDown(Keys.Down) && !oldKbState.IsKeyDown(Keys.Down)) currentPointY = DownPoint();
        if (newKbState.IsKeyDown(Keys.Up) && !oldKbState.IsKeyDown(Keys.Up)) currentPointY = UpPoint();

        if (newKbState.IsKeyDown(Keys.Right) && !oldKbState.IsKeyDown(Keys.Right)) currentPointX = RightPoint();
        if (newKbState.IsKeyDown(Keys.Left) && !oldKbState.IsKeyDown(Keys.Left)) currentPointX = LeftPoint();


        points[currentPointX][currentPointY].renderBall = true;

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

        return belowPoint;

    }

    private int UpPoint()
    {
        int abovePoint = currentPointY;

        if (abovePoint - 1 < 0) abovePoint = maxY - 1;
        else abovePoint--;

        points[currentPointX][currentPointY].renderBall = false;

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

}