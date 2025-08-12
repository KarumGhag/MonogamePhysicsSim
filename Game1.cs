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

namespace MonogamePhysicsSim;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Texture2D _pixel;

    private int _screenWidth = 1280;
    private int _screenHeight = 720;

    private SpriteBatch _spriteBatch;

    private Texture2D _circle;
    private Vector2 _center;


    public List<Entity> entities;
    public Entity _player;

    public PhysicsObject point1;
    public PhysicsObject point2;
    public PhysicsObject point3;
    public Spring spring;
    public Spring spring2;

    public List<Spring> shape;
    public List<VerletObject> verletObjects;
    public List<Rope> ropes;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;

        IsFixedTimeStep = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here


        // Resizes
        _graphics.PreferredBackBufferWidth = _screenWidth;  // set width
        _graphics.PreferredBackBufferHeight = _screenHeight;  // set height
        _graphics.ApplyChanges();                  // apply the change


        _center = new Vector2(_screenWidth / 2, _screenHeight / 2);



        base.Initialize();
    }






    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Loads all textures
        _circle = Content.Load<Texture2D>("circle");
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        // Sets global variables
        Global._circle = _circle;
        Global.width = _screenWidth;
        Global.height = _screenHeight;

        // Initalises the entity list
        entities = new List<Entity>();
        shape = new List<Spring>();
        verletObjects = new List<VerletObject>();
        ropes = new List<Rope>();


        // Instantiates objects

        //_player = new FollowObject(_circle, _center, entities);

        /*
        point1 = new PhysicsObject(_circle, _center - new Vector2(0, 0), entities, true, 0);
        point2 = new PhysicsObject(_circle, _center + new Vector2(300, 0), entities, false, 100);
        point3 = new PhysicsObject(_circle, _center + new Vector2(300, 100), entities, false, 200);

        spring = new Spring(point1, point2, 5f, 0.5f, 200f, shape);
        spring2 = new Spring(point2, point3, 5f, 0.5f, 100f, shape);
        */

        generateRope(30, new Vector2(_screenWidth / 2, -_screenHeight), 20);


    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        for (int i = 0; i < shape.Count; i++)
        {
            shape[i].ApplyForce();
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

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // updates global states
        Global.mouseState = Mouse.GetState();
        Global.gameTime = gameTime;
        Global.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


        _spriteBatch.Begin();

        // Loops over all _entites updates then draws them
        for (int i = 0; i < shape.Count; i++)
        {
            DrawLine(_spriteBatch, shape[i].point1.position, shape[i].point2.position, Color.White, 2.5f);
        }

        for (int i = 0; i < ropes.Count; i++)
        {
            DrawLine(_spriteBatch, ropes[i].point1.position, ropes[i].point2.position, Color.White, 2.5f);
        }


        for (int i = 0; i < entities.Count; i++)
        {
            _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, Color.White, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 1f, SpriteEffects.None, 0f);

        }



        _spriteBatch.End();

        // Cleans the screen
        base.Draw(gameTime);
    }

    public void DrawLine(SpriteBatch spriteBatch, Vector2 pointA, Vector2 pointB, Color color, float thickness = 1f)
    {
        Vector2 delta = pointB - pointA;
        float length = delta.Length();
        float angle = (float)Math.Atan2(delta.Y, delta.X);

        spriteBatch.Draw(_pixel, pointA, null, color, angle, Vector2.Zero, new Vector2(length, thickness), SpriteEffects.None, 0f);
    }


    public List<Rope> generatedRope;
    void generateRope(int numPoint, Vector2 anchorPos, float distance)
    {

        List<VerletObject> generatedPoints = new List<VerletObject>();
        generatedRope = new List<Rope>();

        VerletObject anchor = new VerletObject(_circle, anchorPos, entities, verletObjects, new Vector2(0, 0), true);

        generatedPoints.Add(anchor);

        VerletObject point1;
        VerletObject point2;

        Vector2 lastPointPos = anchorPos;


        for (int i = 0; i < numPoint; i++)
        {
            point1 = new VerletObject(_circle, lastPointPos + new Vector2(distance, distance), entities, verletObjects);
            lastPointPos += new Vector2(distance, distance);

            generatedPoints.Add(point1);
            generatedRope.Add(new Rope(generatedPoints[i], generatedPoints[i + 1], distance, ropes));
        }

    }

}
