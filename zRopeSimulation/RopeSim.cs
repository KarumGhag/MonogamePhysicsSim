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

namespace RopeSimulation;

public class RopeSim : SimulationClass
{
    public List<Entity> entities = new List<Entity>();
    public List<Spring> springs = new List<Spring>();
    public List<VerletObject> verletObjects = new List<VerletObject>();
    public List<Rope> ropes = new List<Rope>();

    private bool drawPoints = false;
    public RopeSim(Game1 game1) : base(game1)
    {
        addRope();
    }

    public override void Update(GameTime gameTime)
    {

        base.Update(gameTime);


        for (int i = 0; i < springs.Count; i++)
        {
            springs[i].ApplyForce();
        }

        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Update(gameTime);
        }

        for (int i = 0; i < verletObjects.Count; i++)
        {
            verletObjects[i].Update();
        }

        for (int i = 0; i < ropes.Count; i++)
        {
            ropes[i].ConstrainPoints();

        }


        // Delete all ropes
        if (newKbState.IsKeyDown(Keys.Space))
        {
            resetRope();
        }

        // Add many ropes
        if (newKbState.IsKeyDown(Keys.A))
        {
            addRope();
        }

        // Add Single rope
        if (newKbState.IsKeyDown(Keys.D) && !oldKbState.IsKeyDown(Keys.D))
        {
            addRope();
        }

        // Toggle drawing of points
        if (newKbState.IsKeyDown(Keys.W) && !oldKbState.IsKeyDown(Keys.W))
        {
            drawPoints = !drawPoints;
        }

        // Generate lattice
        if (newKbState.IsKeyDown(Keys.S) && !oldKbState.IsKeyDown(Keys.S))
        {
            generateSheet(20, new Vector2(_screenWidth / 2, -_screenHeight), 20, 5, 150);
        }


        oldKbState = newKbState;

       // generateLattice();

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
            game1.DrawLine(_spriteBatch, ropes[i].point1.position, ropes[i].point2.position, Color.White, 5f);
        }

        if (drawPoints)
        {

            for (int i = 0; i < entities.Count; i++)
            {
                _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, Color.White, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 1f, SpriteEffects.None, 0f);
            }

        }

        _spriteBatch.End();
    }


    private void generateRope(int numPoint, Vector2 anchorPos, float distance, bool anchorStationary = true)
    {
        List<Rope> generatedRope = new List<Rope>();
        List<VerletObject> generatedPoints = new List<VerletObject>();

        VerletObject anchor = new VerletObject(Global._circle, anchorPos, entities, verletObjects, new Vector2(0, 0), anchorStationary);

        generatedPoints.Add(anchor);

        VerletObject point;

        Vector2 lastPointPos = anchorPos;


        for (int i = 0; i < numPoint; i++)
        {
            point = new VerletObject(Global._circle, lastPointPos + new Vector2(distance * 1.5f, distance * 1.5f), entities, verletObjects);
            lastPointPos += new Vector2(distance, distance);

            generatedPoints.Add(point);
            generatedRope.Add(new Rope(generatedPoints[i], generatedPoints[i + 1], distance, ropes));

        }

        //generatedPoints[numPoint].position += new Vector2(500, 0);


    }



    private void generateSheet(int numPoint, Vector2 anchorPos, float distance, int numRope, float sheetDistance, bool anchorStationary = true)
    {
        List<Rope> sheetRopes = new List<Rope>();
        List<List<VerletObject>> allPoints = new List<List<VerletObject>>();

        List<VerletObject> currentPoints = new List<VerletObject>();

        Vector2 nextPointPos;


        for (int j = 0; j < numRope; j++)
        {
            
            nextPointPos = anchorPos;
            currentPoints = new List<VerletObject>();
            // Generate points
            for (int i = 0; i < numPoint; i++)
            {
                if (i == 0 && j == 0) // First point is anchor
                {
                    currentPoints.Add(new VerletObject(Global._circle, anchorPos, entities, verletObjects, new Vector2(0, 0), true));
                    continue;
                }

                nextPointPos += new Vector2(distance * 1.5f, distance * 1.5f);
                currentPoints.Add(new VerletObject(Global._circle, nextPointPos, entities, verletObjects));


            }
            allPoints.Add(currentPoints);
            // Makes ropes between each point just generated
            for (int i = 1; i < numPoint; i++)
            {
                sheetRopes.Add(new Rope(currentPoints[i - 1], currentPoints[i], distance, ropes));
            }
        }



        for (int i = 0; i < numRope - 1; i++)
        {
            for (int j = 0; j < numPoint; j++)
            {
                sheetRopes.Add(new Rope(allPoints[i][j], allPoints[i + 1][j], sheetDistance, ropes));
            }
        }



    }


    private void resetRope()
    {
        ropes = new List<Rope>();
        entities = new List<Entity>();
    }

    private void addRope()
    {
        generateRope(50, new Vector2(_screenWidth / 2, -_screenHeight), 15);
    }


}