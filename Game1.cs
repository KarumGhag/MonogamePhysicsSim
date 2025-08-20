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
using Simulation;
using RopeSimulation;

namespace MonogamePhysicsSim;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private Texture2D _pixel;

    private int _screenWidth = 1920;
    private int _screenHeight = 1080;

    public SpriteBatch _spriteBatch;

    private Texture2D _circle;
    private Vector2 _center;


    public SimulationClass currentSimulation;

    private RopeSim ropeSim;

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

        // Instantiates simulations
        ropeSim = new RopeSim(this);

        currentSimulation = ropeSim;


    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here
        // updates global states
        Global.mouseState = Mouse.GetState();
        Global.gameTime = gameTime;
        Global.deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

        // Updates the currently in use simulation
        currentSimulation.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.Black);

        // Draws the currently in use simulation
        currentSimulation.Draw(gameTime);

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
