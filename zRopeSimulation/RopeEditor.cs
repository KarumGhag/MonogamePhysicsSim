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

namespace RopeEdit;

class RopeEditor
{
    public static int numRopes;

    private List<VerletObject> points = new List<VerletObject>();

    private List<Rope> ropes = new List<Rope>();


    public int currentSelected = 0;
    public int currentRope = 0;

    public bool isSelected = false;

    private KeyboardState newKbState;
    private KeyboardState oldKbState;

    public RopeEditor(List<VerletObject> points, List<Rope> ropes)
    {
        numRopes++;

        this.points = points;
        this.ropes = ropes;

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

        if (newKbState.IsKeyDown(Keys.Right) && !oldKbState.IsKeyDown(Keys.Right)) currentSelected = GetNext();
        if (newKbState.IsKeyDown(Keys.Down) && !oldKbState.IsKeyDown(Keys.Down)) currentSelected = GetNext();
        if (newKbState.IsKeyDown(Keys.Left) && !oldKbState.IsKeyDown(Keys.Left)) currentSelected = GetLast();
        if (newKbState.IsKeyDown(Keys.Up) && !oldKbState.IsKeyDown(Keys.Up)) currentSelected = GetLast();
        if (newKbState.IsKeyDown(Keys.Z) && !oldKbState.IsKeyDown(Keys.Z)) currentRope = GetNextRope();
        if (newKbState.IsKeyDown(Keys.C) && !oldKbState.IsKeyDown(Keys.C)) CutRope();
        if (newKbState.IsKeyDown(Keys.G) && !oldKbState.IsKeyDown(Keys.G)) points[currentSelected].stationary = !points[currentSelected].stationary;
        if (newKbState.IsKeyDown(Keys.J) && !oldKbState.IsKeyDown(Keys.J)) points[currentSelected].grabbed = !points[currentSelected].grabbed;

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].stationary) points[i].colour = Global.stationaryPointColour;
            else points[i].colour = Global.defaultPointColour;
        }

        points[currentSelected].colour = Global.editingPointColour;

        points[currentSelected].renderBall = true;
        if (ropes[currentRope].active) ropes[currentRope].colour = Color.Red;
        for (int i = 0; i < ropes.Count; i++)
        {
            if (!ropes[i].active && i == currentRope) ropes[i].colour = Color.Gray;
            else if (!ropes[i].active) ropes[i].colour = Global.backgroundColour;
            else if (i != currentRope) ropes[i].colour = Global.selectedRopeColour;
        }




        oldKbState = Keyboard.GetState();
    }

    private void CutRope()
    {
        ropes[currentRope].active = !ropes[currentRope].active;
    }
    private void Deselect()
    {
        points[currentSelected].renderBall = false;
        for (int i = 0; i < ropes.Count; i++)
        {
            if (ropes[i].active) ropes[i].colour = Global.defaultRopeColour;
            else ropes[i].colour = Global.backgroundColour;
        }

        for (int i = 0; i < points.Count; i++)
        {
            if (points[i].stationary) points[i].colour = Global.stationaryPointColour;
            else points[i].colour = Global.defaultPointColour;
        }


    }

    // Cycles forwards through the points list
    private int GetNext()
    {

        int nextPoint = currentSelected;

        if (nextPoint + 1 == points.Count) nextPoint = 0;
        else nextPoint++;

        points[currentSelected].renderBall = false;
        points[currentSelected].grabbed = false;

        ropes[currentRope].colour = Global.selectedRopeColour;
        currentRope++;

        SolveRope(nextPoint);
        return nextPoint;
    }


    // Cycles backwards through the points list
    private int GetLast()
    {

        int lastPoint = currentSelected;

        if (lastPoint - 1 < 0) lastPoint = points.Count - 1;
        else lastPoint--;

        points[currentSelected].renderBall = false;
        points[currentSelected].grabbed = false;

        ropes[currentRope].colour = Global.selectedRopeColour;
        currentRope--;

        SolveRope(lastPoint);
        return lastPoint;
    }


    private int GetNextRope()
    {
        int nextRope = currentSelected;

        ropes[currentRope].colour = Global.selectedRopeColour;

        if (currentRope == currentSelected && currentSelected != 0) nextRope--;
        else if (currentRope == currentSelected - 1 && currentSelected > ropes.Count) nextRope++;

        return nextRope;
    }

    private void SolveRope(int pos)
    {
        if (pos == 0) currentRope = 0;
        if (pos == points.Count - 1) currentRope = pos - 1;
    }

}