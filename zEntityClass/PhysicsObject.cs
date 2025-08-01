using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using EntityClass;
using GlobalInfo;

namespace PhysicsClass;

class PhysicsObject : Entity
{

    private Vector2 mousePos;
    public Vector2 velocity;
    private float speed = 500;
    private Vector2 direction;

    public PhysicsObject(Texture2D sprite, Vector2 position, List<Entity> entities) : base(sprite, position, entities)
    {

    }

    public override void Update(GameTime gameTime)
    {

        float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;


        base.Update(gameTime);

        position += velocity;

        mousePos = new Vector2(Global.mouseState.X, Global.mouseState.Y);

        direction = mousePos - position;
        direction = Vector2.Normalize(direction);

        velocity = direction * speed * deltaTime;

    }

}