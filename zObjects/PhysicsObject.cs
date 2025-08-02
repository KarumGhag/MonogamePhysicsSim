using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using EntityClass;
using GlobalInfo;
using System.Threading;

namespace PhysicsClass;

public class PhysicsObject : Entity
{
    public Vector2 velocity;

    public float gravity = 1;

    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities, float gravity = 1) : base(sprite, position, entities)
    {
        this.gravity = gravity;
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        velocity.Y += gravity * Global.deltaTime;
        position += velocity * Global.deltaTime;
    }

    public virtual void ApplyForce(Vector2 force)
    {
        this.velocity += force;
    }

}