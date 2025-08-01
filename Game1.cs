using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EntityClass;
using PhysicsClass;
using GlobalInfo;

namespace MonogamePhysicsSim;


public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private int screenWidth = 1280;
    private int screenHeight = 720;
    private SpriteBatch _spriteBatch;

    private Texture2D _circle;

    private Vector2 _center;


    public List<Entity> Entities;
    public static Entity player;



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
        _graphics.PreferredBackBufferWidth = screenWidth;  // set width
        _graphics.PreferredBackBufferHeight = screenHeight;  // set height
        _graphics.ApplyChanges();                  // apply the change


        _center = new Vector2(screenWidth / 2, screenHeight / 2);



        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // Loads all textures
        _circle = Content.Load<Texture2D>("circle");

        // Initalises the entity list
        Entities = new List<Entity>();

        // Instantiates objects
        player = new PhysicsObject(_circle, _center, Entities);



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



        _spriteBatch.Begin();

        // Loops over all entites updates then draws them
        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].Update(gameTime);
            _spriteBatch.Draw(Entities[i].sprite, Entities[i].position, Color.White);
        }

        _spriteBatch.End();

        // Cleans the screen
        base.Draw(gameTime);
    }
}
