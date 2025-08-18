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

namespace RopeSimulation;

public class RopeSim : SimulationClass
{
    public List<Entity> entities = new List<Entity>();
    public List<Spring> springs = new List<Spring>();
    public List<VerletObject> verletObjects = new List<VerletObject>();
    public List<Rope> ropes = new List<Rope>();

    public RopeSim(Game1 game1) : base(game1)
    {
        generateRope(30, new Vector2(_screenWidth / 2, -_screenHeight), 5);


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
            game1.DrawLine(_spriteBatch, ropes[i].point1.position, ropes[i].point2.position, Color.White, 2.5f);
        }


        for (int i = 0; i < entities.Count; i++)
        {
            _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, Color.White, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 1f, SpriteEffects.None, 0f);

        }

        _spriteBatch.End();
    }


    void generateRope(int numPoint, Vector2 anchorPos, float distance)
    {
        List<Rope> generatedRope = new List<Rope>();
        List<VerletObject> generatedPoints = new List<VerletObject>();

        VerletObject anchor = new VerletObject(Global._circle, anchorPos, entities, verletObjects, new Vector2(0, 0), true);

        generatedPoints.Add(anchor);

        VerletObject point1;

        Vector2 lastPointPos = anchorPos;


        for (int i = 0; i < numPoint; i++)
        {
            point1 = new VerletObject(Global._circle, lastPointPos + new Vector2(distance, distance), entities, verletObjects);
            lastPointPos += new Vector2(distance, distance);

            generatedPoints.Add(point1);
            generatedRope.Add(new Rope(generatedPoints[i], generatedPoints[i + 1], distance, ropes));
        }
        

    }



}