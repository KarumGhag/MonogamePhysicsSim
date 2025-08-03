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

    public float fieldStrength { get; } = 650; // Pixels per second²
    private bool stationary;
    public float mass;

    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities, float mass, bool stationary = false)
        : base(sprite, position, entities)
    {
        this.stationary = stationary;
        this.mass = mass;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);

        // Apply gravity as constant acceleration (per second²)
        ApplyAcceleration(new Vector2(0, fieldStrength));

        if (!stationary)
        {
            velocity += acceleration * Global.deltaTime;
            position += velocity * Global.deltaTime;
        }

        // Reset acceleration at end of frame
        acceleration = Vector2.Zero;
    }

    public void ApplyForce(Vector2 force)
    {
        acceleration += force / mass;
    
    }

    public void ApplyAcceleration(Vector2 accel)
    {
        acceleration += accel;
    }
}


    /* Newtons equations:

        Force = Mass * Accelartion
        Acceleration = Force / Mass
        velocity += acceleration * deltaTime


    */


