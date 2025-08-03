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
    public Spring spring;
    public Spring spring2;

    public Spring spring3;

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
        _pixel = new Texture2D(GraphicsDevice, 1, 1);
        _pixel.SetData(new[] { Color.White });

        // Initalises the entity list
        entities = new List<Entity>();
        shape = new List<Spring>();


        // Instantiates objects
        //_player = new FollowObject(_circle, _center, entities);


        point1 = new PhysicsObject(_circle, _center - new Vector2(0, 0), entities, 1, true);
        point2 = new PhysicsObject(_circle, _center + new Vector2(200, 0), entities, 1f, false);
        point3 = new PhysicsObject(_circle, _center + new Vector2(300, -100), entities, 4f, false);
        point4 = new PhysicsObject(_circle, _center + new Vector2(500, 0), entities, 1f, true);

        point2.velocity = new Vector2(0, -100);
        point3.velocity = new Vector2(1000, -200);

        spring = new Spring(point1, point2, 100f, 3f, 50f);
        spring2 = new Spring(point2, point3, 90f, 10f, 200f);
        spring3 = new Spring(point3, point4, 30f, 1f, 50f);

        shape.Add(spring);
        // shape.Add(spring2);
        shape.Add(spring3);



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

        Console.WriteLine(point2.position);
        DrawLine(_spriteBatch, _pixel, point3.position, point4.position, Color.Red, 2f);
        DrawLine(_spriteBatch, _pixel, point1.position, point2.position, Color.Red, 2f);


        for (int i = 0; i < entities.Count; i++)
        {
            entities[i].Update(gameTime);
            _spriteBatch.Draw(entities[i].sprite, entities[i].position, null, Color.White, 0f, new Vector2(entities[i].sprite.Width / 2f, entities[i].sprite.Height / 2f), 1f, SpriteEffects.None, 0f);

        }


        _spriteBatch.End();

        // Cleans the screen
        base.Draw(gameTime);
    }

    public void DrawLine(SpriteBatch spriteBatch, Texture2D texture, Vector2 start, Vector2 end, Color color, float thickness = 1f)
    {
        Vector2 edge = end - start;
        float angle = (float)Math.Atan2(edge.Y, edge.X);
        float length = edge.Length();

        spriteBatch.Draw(texture,
            position: start,
            sourceRectangle: null,
            color: color,
            rotation: angle,
            origin: new Vector2(0f, 0.5f), // this centers the texture vertically
            scale: new Vector2(length, thickness),
            effects: SpriteEffects.None,
            layerDepth: 0f);
    }

}
