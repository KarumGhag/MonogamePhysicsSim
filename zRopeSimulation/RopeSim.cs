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
using RopeEdit;
using ClothEdit;
using System.Collections.Concurrent;
using Microsoft.Xna.Framework.Media;
using System.Net.Sockets;

namespace RopeSimulation;





/*
===== KEYBINDS =====
P = Pause
B = Single frame forward
V = Hold for many frame forward

W = Toggle all point visibility
D = Add single rope
A = Add many rope
S = Add cloth

1 = Toggle whether editing ropes or not:
    Arrow keys to go up or down across rope
    C = Cut red rope and repair grey ropes
    X = Swap whether above rope or below rope selected
    F = Toggle anchor
    G = Grab

2 = Toggle whether editing cloths or not:
    Arrow keys to select point
    X = go forward through brown/light grey ropes (red/dark grey indicates the currently selected one)
    Z = same as X but backwards
    C = cut red rope and repair dark grey rope
    V = cut all ropes around the point, does not repair
    B = Repair all of the cloth (a bit buggy but does work if you repair some then break some then repair again)
    N = Repair nearest 4 (for some reason doesnt do the one on the left)
    F = Toggle anchor
    G = Grab

Q/E = cycle through the ropes/clothes to edit

*/



public class RopeSim : SimulationClass
{

    // Controls

    public static Keys makeRope = Keys.D;
    public static Keys makeManyRope = Keys.A;
    public static Keys makeCloth = Keys.S;
    public static Keys drawPointToggle = Keys.W;
    public static Keys clearSim = Keys.Space;
    public static Keys editRope = Keys.D1;
    public static Keys editCloth = Keys.D2;
    public static Keys nextEdit = Keys.E;
    public static Keys lastEdit = Keys.Q;


    public static Keys pause = Keys.P;
    public static Keys stepOne = Keys.D0;
    public static Keys stepMany = Keys.D9;


    public static Keys cutRope = Keys.C;
    public static Keys cutNearest = Keys.V;
    public static Keys repairCloth = Keys.B;
    public static Keys repairNearest = Keys.N;
    public static Keys grabPoint = Keys.G;
    public static Keys anchorPoint = Keys.F;
    public static Keys ropeCycleForward = Keys.X;
    public static Keys ropeCycleBackward = Keys.Z;



    

    private bool pauseMotion = false;
    public List<Entity> entities = new List<Entity>();
    public List<Spring> springs = new List<Spring>();
    public List<VerletObject> verletObjects = new List<VerletObject>();
    public List<Rope> ropes = new List<Rope>();

    private bool drawPoints = false;


    private List<RopeEditor> ropeEditors = new List<RopeEditor>();
    private bool editingRope = false;
    private int currentRopeEditor = 0;

    private List<ClothEditor> clothEditors = new List<ClothEditor>();
    private bool editingCloth = false;
    private int currentClothEditor = 0;


    private float stepThrough = 1f;

    public RopeSim(Game1 game1) : base(game1)
    {
        addRope();
    }

    public override void Update(GameTime gameTime)
    {

        base.Update(gameTime);


        stepThrough += 1f;

        if (!pauseMotion || Global.CheckTap(stepOne) || (Global.newKb.IsKeyDown(stepMany) && stepThrough % 2 == 0))
        {

            for (int i = 0; i < springs.Count; i++) springs[i].ApplyForce();

            for (int i = 0; i < entities.Count; i++) entities[i].Update(gameTime);

            for (int i = 0; i < verletObjects.Count; i++) verletObjects[i].Update();

            for (int i = 0; i < ropes.Count; i++) ropes[i].ConstrainPoints();
        }
        else if (pauseMotion)
        {
            for (int i = 0; i < verletObjects.Count; i++) if (verletObjects[i].grabbed) verletObjects[i].Update(); // Updates points even when paused if they are grabbed
        }



        if (Global.CheckTap(editRope) && ropeEditors.Count > 0)
        {
            if (editingRope) ropeEditors[currentRopeEditor].isSelected = false;
            ropeEditors[currentRopeEditor].Update();

            editingRope = !editingRope;

            if (editingRope)
            {
                editingCloth = false;
                for (int i = 0; i < clothEditors.Count; i++)
                {
                    clothEditors[i].isSelected = false;
                    clothEditors[i].Update();
                }
            }
        }

        if (Global.CheckTap(editCloth) && clothEditors.Count > 0)
        {

            if (editingCloth) clothEditors[currentClothEditor].isSelected = false;
            clothEditors[currentClothEditor].Update();

            editingCloth = !editingCloth;

            if (editingCloth)
            {
                editingRope = false;
                for (int i = 0; i < ropeEditors.Count; i++)
                {
                    ropeEditors[i].isSelected = false;
                    ropeEditors[i].Update();
                }
            }
        }


        if (editingRope)
        {
            ropeEditors[currentRopeEditor].isSelected = true;

            if (Global.CheckTap(nextEdit))
            {
                int nextRope = currentRopeEditor;

                if (nextRope + 1 == ropeEditors.Count) nextRope = 0;
                else nextRope++;

                ropeEditors[currentRopeEditor].isSelected = false;
                ropeEditors[currentRopeEditor].Update();

                currentRopeEditor = nextRope;
                ropeEditors[currentRopeEditor].isSelected = true;
            }

            if (Global.CheckTap(lastEdit))
            {
                int lastRope = currentRopeEditor;

                if (lastRope - 1 < 0) lastRope = ropeEditors.Count - 1;
                else lastRope--;

                ropeEditors[currentRopeEditor].isSelected = false;
                ropeEditors[currentRopeEditor].Update();

                currentRopeEditor = lastRope;
                ropeEditors[currentRopeEditor].isSelected = true;
            }


            ropeEditors[currentRopeEditor].Update();
        }

        if (editingCloth)
        {
            clothEditors[currentClothEditor].isSelected = true;

            if (Global.CheckTap(nextEdit))
            {
                int nextCloth = currentClothEditor;

                if (nextCloth + 1 == clothEditors.Count) nextCloth = 0;
                else nextCloth++;

                clothEditors[currentClothEditor].isSelected = false;
                clothEditors[currentClothEditor].Update();

                currentClothEditor = nextCloth;
                clothEditors[currentClothEditor].isSelected = true;
            }

            if (Global.CheckTap(lastEdit))
            {
                int lastCloth = currentClothEditor;

                if (lastCloth - 1 < 0) lastCloth = clothEditors.Count - 1;
                else lastCloth--;

                clothEditors[currentClothEditor].isSelected = false;
                clothEditors[currentClothEditor].Update();

                currentClothEditor = lastCloth;
                clothEditors[currentClothEditor].isSelected = true;
            }

            clothEditors[currentClothEditor].Update();
        }

        // Delete all ropes
        if (Global.CheckTap(clearSim)) resetRope();

        // Add many ropes
        if (Global.CheckTap(makeManyRope)) addRope();

        // Add Single rope
        if (Global.CheckTap(makeRope)) addRope();

        // Toggle drawing of points
        if (Global.CheckTap(drawPointToggle)) drawPoints = !drawPoints;

        // Pause
        if (Global.CheckTap(pause)) pauseMotion = !pauseMotion;


        // Generate sheet
        if (Global.CheckTap(makeCloth))
        {
            int sheetDistance = 25;
            int ropeNum = 15;
            int pointNum = 20;
            int distance = 25;
            generateSheet(pointNum, new Vector2(_screenWidth / 2 - (sheetDistance * ropeNum) / 2, 50), distance, ropeNum, sheetDistance, true);
        }


    }


    public override void Draw(GameTime gameTime)
    {

        base.Draw(gameTime);
        _spriteBatch.Begin();

        // Loops over all _entites updates then draws them
        for (int i = 0; i < springs.Count; i++)
        {
            game1.DrawLine(_spriteBatch, springs[i].point1.position, springs[i].point2.position, Color.White, 2.5f);
        }

        for (int i = 0; i < ropes.Count; i++)
        {
            if(ropes[i].renderLine) game1.DrawLine(_spriteBatch, ropes[i].point1.position, ropes[i].point2.position, ropes[i].colour, 5f);
        }


        for (int i = 0; i < entities.Count; i++)
        {
            if (drawPoints || entities[i].renderBall) _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, entities[i].colour, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 0.5f, SpriteEffects.None, 0f);
        }






        _spriteBatch.End();
    }


    private void generateRope(int numPoint, Vector2 anchorPos, float distance, bool anchorStationary = true)
    {
        List<Rope> generatedRope = new List<Rope>();
        List<VerletObject> generatedPoints = new List<VerletObject>();

        VerletObject anchor = new VerletObject(Global._circle, anchorPos, entities, verletObjects, new Vector2(0, 0), Color.Red, anchorStationary);

        generatedPoints.Add(anchor);

        VerletObject point;

        Vector2 lastPointPos = anchorPos;

        for (int i = 0; i < numPoint; i++)
        {
            point = new VerletObject(Global._circle, lastPointPos + new Vector2(distance * 1.5f, 0), entities, verletObjects);
            lastPointPos += new Vector2(distance, 0);

            generatedPoints.Add(point);
            generatedRope.Add(new Rope(generatedPoints[i], generatedPoints[i + 1], distance, ropes));

        }


        ropeEditors.Add(new RopeEditor(generatedPoints, generatedRope));

    }



    private void generateSheet(int numPoint, Vector2 anchorPos, float distance, int numRope, float sheetDistance, bool allAnchored = false, bool firstLastAnchor = false)
    {
        List<List<Rope>> verticalRopes = new List<List<Rope>>();
        List<List<Rope>> horizontalRopes = new List<List<Rope>>();
        List<List<VerletObject>> allPoints = new List<List<VerletObject>>();

        List<VerletObject> currentPoints;

        Vector2 nextPointPos;

        if (firstLastAnchor)
        {
            allAnchored = false;
        }

        for (int j = 0; j < numRope; j++)
        {

            nextPointPos = anchorPos;
            currentPoints = new List<VerletObject>();

            List<Rope> currentVertical = new List<Rope>();

            // Generate points
            for (int i = 0; i < numPoint; i++)
            {


                // If its the first point and (the first rope OR i want the first point of all ropes to anchor) OR i want the first and last to anchor AND its the last point AND the first point of that rope

                // First point will always anchor. if i want all first points to anchor then it does not care for j. if i want first and last to anchor then it will do this as long as it is the first point in the rope
                if (i == 0 && (j == 0 || allAnchored) || ((firstLastAnchor && j == numRope - 1) && i == 0))
                {
                    currentPoints.Add(new VerletObject(Global._circle, new Vector2(anchorPos.X + (j * sheetDistance), anchorPos.Y), entities, verletObjects, new Vector2(0, 0), Color.Red, true));
                    continue;
                }




                nextPointPos += new Vector2(distance * 1.5f, distance * 1.5f);
                currentPoints.Add(new VerletObject(Global._circle, nextPointPos, entities, verletObjects));


            }
            allPoints.Add(currentPoints);
            // Makes ropes between each point just generated
            for (int i = 1; i < numPoint; i++)
            {
                currentVertical.Add(new Rope(currentPoints[i - 1], currentPoints[i], distance, ropes));
            }

            verticalRopes.Add(currentVertical);

        }

        // Must be -1 so it doesnt not try connect the last rope to another non existant rope
        // i == the larger list holding each set of already connected points, j is each individual point in this larger list
        // this will connect point j of rope i to point j of rope i + 1
        for (int i = 0; i < numRope - 1; i++)
        {
            List<Rope> currentHorizontal = new List<Rope>();
            for (int j = 0; j < numPoint; j++)
            {
                currentHorizontal.Add(new Rope(allPoints[i][j], allPoints[i + 1][j], sheetDistance, ropes));
            }

            horizontalRopes.Add(currentHorizontal);
        }

        clothEditors.Add(new ClothEditor(allPoints, verticalRopes, horizontalRopes));

    }


    private void resetRope()
    {
        ropes = new List<Rope>();
        entities = new List<Entity>();

        for (int i = 0; i < ropeEditors.Count; i++)
        {
            ropeEditors[i].isSelected = false;
            ropeEditors[i].Update();
        }

        for (int i = 0; i < clothEditors.Count; i++)
        {
            clothEditors[i].isSelected = false;
            clothEditors[i].Update();
        }

        editingRope = false;
        editingCloth = false;
        currentClothEditor = 0;
        currentRopeEditor = 0;
        ropeEditors = new List<RopeEditor>();
        clothEditors = new List<ClothEditor>();
    }

    private void addRope()
    {
        generateRope(50, new Vector2(_screenWidth / 2, -_screenHeight), 15);
    }


}