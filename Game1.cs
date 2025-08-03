using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EntityClass;
using PhysicsClass;
using GlobalInfo;
using FollowClass;
using SpringClass;
using System;
using System.Security.Authentication;
using RopeClass;

namespace MonogamePhysicsSim;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private int _screenWidth = 1920;
    private int _screenHeight = 1080;

    private SpriteBatch _spriteBatch;

    private Texture2D _circle;
    private Vector2 _center;

    private Texture2D _pixel;
    public List<Entity> entities;
    public Entity _player;

    public PhysicsObject point1;
    public PhysicsObject point2;
    public PhysicsObject point3;
    public PhysicsObject point4;
    public PhysicsObject point5;
    public PhysicsObject point6;
    public PhysicsObject point7;


    public Spring spring;
    public Spring spring2;
    public Spring spring3;
    public Spring spring4;
    public Spring spring5;
    public Spring spring6;
    public Spring spring7;

    public Rope rope;

    public List<Spring> shape;
    public List<Rope> ropes;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
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
        Global._circle = _circle;

        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        // Initalises the entity list
        entities = new List<Entity>();
        shape = new List<Spring>();
        ropes = new List<Rope>();

        Global.entities = entities;


        // Instantiates objects
        //_player = new FollowObject(_circle, _center, entities);


        point1 = new PhysicsObject(_circle, _center + new Vector2(0, 0), entities, 1, true);
        point2 = new PhysicsObject(_circle, _center + new Vector2(200, 0), entities, 10f, false);
        point3 = new PhysicsObject(_circle, _center + new Vector2(300, -100), entities, 4f, false);
        point4 = new PhysicsObject(_circle, _center + new Vector2(500, 0), entities, 1f, true);
        point5 = new PhysicsObject(_circle, _center + new Vector2(250, 0), entities, 2f);
        point6 = new PhysicsObject(_circle, _center + new Vector2(250, 0), entities, 1f);
        point7 = new PhysicsObject(_circle, _center + new Vector2(200, 0), entities, 0.5f);

        point2.velocity = new Vector2(0, -100);
        point3.velocity = new Vector2(0, -200);
        point6.velocity = new Vector2(0, 2000);
        point7.velocity = new Vector2(200, -200);

        spring = new Spring(point1, point2, 100f, 50f, 50f, shape);
        //spring2 = new Spring(point2, point3, 90f, 10f, 200f, shape);
        spring3 = new Spring(point3, point7, 60f, 1f, 50f, shape);
        spring4 = new Spring(point3, point5, 5f, 10f, 500f, shape);
        spring5 = new Spring(point4, point5, 50f, 10f, 200f, shape);
        spring6 = new Spring(point2, point6, 60f, 10f, 200f, shape);
        spring7 = new Spring(point2, point7, 50f, 1f, 100f, shape);


        rope = new Rope(5, 1, _center, ropes);

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        for (int i = 0; i < ropes.Count; i++)
        {

            ropes[i].ApplyForces();

            for (int j = 0; j < ropes[i].points.Count; j++)
            {
                if (j == ropes[i].points.Count - 1) break;
                DrawLine(_spriteBatch, ropes[i].points[j].position, ropes[i].points[j + 1].position, Color.White, 2f);
            }
        }

        for (int i = 0; i < entities.Count; i++)
            {
                entities[i].Update(gameTime);
                _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, Color.White, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 1f, SpriteEffects.None, 0f);
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
            shape[i].ApplyForce();
            DrawLine(_spriteBatch, shape[i].pointA.position, shape[i].pointB.position, Color.White, 2.5f);
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


}
