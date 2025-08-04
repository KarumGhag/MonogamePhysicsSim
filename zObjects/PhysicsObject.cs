using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using EntityClass;
using GlobalInfo;
using System.Threading;
using System.Dynamic;
using System.Data;

namespace PhysicsClass;

public class PhysicsObject : Entity
{
    public Vector2 velocity;
    public Vector2 acceleration;

    public float fieldStrength { get; } = 10; // Pixels per second²
    public bool stationary;
    public float mass;
    private Vector2 spawnPos;


    public Vector2 currentPosition;
    public Vector2 previousPosition;



    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities, float mass, bool stationary = false)
        : base(sprite, position, entities)
    {
        this.stationary = stationary;
        this.mass = mass;
        spawnPos = position;

        currentPosition = previousPosition = position;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.Space))
        {
            position = spawnPos;
        }


        ApplyForce(new Vector2(0, fieldStrength));

        currentPosition = position;

        if (!stationary)
        {
            position += (currentPosition - previousPosition) + acceleration * deltaTime;
        }

        previousPosition = position;
        acceleration = Vector2.Zero;

    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    }
}


    /* Newtons equations:

        Force = Mass * Accelartion
        Acceleration = Force / Mass
        velocity += acceleration * deltaTime


    */


