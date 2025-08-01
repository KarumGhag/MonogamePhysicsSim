using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EntityClass;
using PhysicsClass;
using GlobalInfo;
using FollowClass;

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
    public static Entity _player;



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

        // Instantiates objects
        _player = new FollowObject(_circle, _center, entities);



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
