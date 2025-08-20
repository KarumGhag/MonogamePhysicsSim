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
    private int ropeNum;

    private List<VerletObject> rope = new List<VerletObject>();


    public int currentSelected = 0;

    public bool isSelected = false;

    private KeyboardState newKbState;
    private KeyboardState oldKbState;

    public RopeEditor(List<VerletObject> rope)
    {
        numRopes++;
        ropeNum = numRopes;

        this.rope = rope;
        oldKbState = Keyboard.GetState();

    }


    public void Update()
    {
        newKbState = Keyboard.GetState();

        if (newKbState.IsKeyDown(Keys.Right) && !oldKbState.IsKeyDown(Keys.Right))
        {
            currentSelected = GetNext();
        }

        if (newKbState.IsKeyDown(Keys.Down) && !oldKbState.IsKeyDown(Keys.Down))
        {
            currentSelected = GetNext();
        }



        if (newKbState.IsKeyDown(Keys.Left) && !oldKbState.IsKeyDown(Keys.Left))
        {
            currentSelected = GetLast();
        }

        if (newKbState.IsKeyDown(Keys.Up) && !oldKbState.IsKeyDown(Keys.Up))
        {
            currentSelected = GetLast();
        }


        rope[currentSelected].renderBall = true;

        oldKbState = Keyboard.GetState();
    }


    private int GetNext()
    {

        int nextPoint = currentSelected;

        if (nextPoint + 1 == rope.Count) nextPoint = 0;
        else nextPoint++;

        rope[currentSelected].renderBall = false;

        return nextPoint;
    }


    private int GetLast()
    {

        int lastPoint = currentSelected;

        if (lastPoint - 1 < 0) lastPoint = rope.Count - 1;
        else lastPoint--;

        rope[currentSelected].renderBall = false;

        return lastPoint;
    }
}