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

namespace MonogamePhysicsSim;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;

    private int _screenWidth = 1280;
    private int _screenHeight = 720;

    private SpriteBatch _spriteBatch;

    private Texture2D _circle;
    private Vector2 _center;


    public List<Entity> entities;
    public Entity _player;

    public PhysicsObject point1;
    public PhysicsObject point2;
    public Spring spring;

    public List<Spring> shape;



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

        // Initalises the entity list
        entities = new List<Entity>();
        shape = new List<Spring>();


        // Instantiates objects
        //_player = new FollowObject(_circle, _center, entities);


        point1 = new PhysicsObject(_circle, _center - new Vector2(500, 0), entities, (float)0);
        point2 = new PhysicsObject(_circle, _center + new Vector2(500, 0), entities, (float)0);

        spring = new Spring(point1, point2, (float)0.5, (float)0.05, 200);

        shape.Add(spring);
        
        
        

    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

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
        }

        Console.WriteLine(_center.X < point1.position.X);
        
        
        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Update(gameTime);
            _spriteBatch.Draw(entities[i].sprite, entities[i].position, Color.White);
        }


        _spriteBatch.End();

        // Cleans the screen
        base.Draw(gameTime);
    }
}
