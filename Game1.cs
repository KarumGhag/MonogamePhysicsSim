using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using EntityClass;
using PhysicsClass;

namespace MonogamePhysicsSim;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Texture2D _circle;

    private Vector2 _center;


    public List<Entity> Entities;
    public Entity player;



    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        _graphics.PreferredBackBufferWidth = 1280;  // set width
        _graphics.PreferredBackBufferHeight = 720;  // set height
        _graphics.ApplyChanges();                  // apply the change



        int screenWidth = GraphicsDevice.Viewport.Width;
        int screenHeight = GraphicsDevice.Viewport.Height;

        _center = new Vector2(screenWidth / 2, screenHeight / 2);





        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        _circle = Content.Load<Texture2D>("circle");

        player = new PhysicsObject(_circle, _center);

        Entities = new List<Entity>();
        Entities.Add(player);

        // TODO: use this.Content to load your game content here
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


        _spriteBatch.Begin();

        // _spriteBatch.Draw(_circle, _center, Color.White);

        for (int i = 0; i < Entities.Count; i++)
        {
            Entities[i].Update(gameTime);
            _spriteBatch.Draw(Entities[i].sprite, Entities[i].position, Color.White);
        }

        _spriteBatch.End();


        base.Draw(gameTime);
    }
}
