using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using EntityClass;
using PhysicsClass;
using GlobalInfo;

namespace FollowClass;

public class FollowObject : PhysicsObject
{

    private Vector2 direction;
    private Vector2 mousePosition;
    private float speed;

    public FollowObject(Texture2D sprite, Vector2 position, List<Entity> entities, float speed = 500) : base(sprite, position, entities)
    {
        this.speed = speed;
    }

    public override void Update(GameTime gameTime)
    {
        mousePosition = new Vector2(Global.mouseState.X, Global.mouseState.Y);

        direction = mousePosition - position;
        direction = Vector2.Normalize(direction);

        velocity = direction * speed * Global.deltaTime;

        base.Update(gameTime);

    }
}