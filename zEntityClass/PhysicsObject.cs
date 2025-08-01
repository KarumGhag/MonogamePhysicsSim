using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogamePhysicsSim;

using EntityClass;

namespace PhysicsClass;

class PhysicsObject : Entity
{

    public Vector2 velocity;

    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities) : base(sprite, position, entities)
    {

    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        this.position += velocity;
    }

}