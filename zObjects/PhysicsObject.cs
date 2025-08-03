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

namespace PhysicsClass;

public class PhysicsObject : Entity
{
    public Vector2 velocity;

    public float gravity { get; set; } = 9;

    private bool stationary { get; set; } = false;

    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities, bool stationary = false, float gravity = 100) : base(sprite, position, entities)
    {
        this.gravity = gravity;
        this.stationary = stationary;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        velocity.Y += gravity * Global.deltaTime;
        
        if (!stationary)
        {
            position += velocity * Global.deltaTime;

        }
    }

    public virtual void ApplyForce(Vector2 force)
    {
        if (!stationary)
        {
            velocity += force;
        }
    }

}