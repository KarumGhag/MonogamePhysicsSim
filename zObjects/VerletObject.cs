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
using System;

namespace VerletClass;

public class VerletObject : Entity
{

    private Vector2 oldPosition;
    private Vector2 startOffset = new Vector2(5, 5);

    private Vector2 velocity;

    private float bounceDamping;

    public float gravity = 0.3f;
    public float firction = 0.999f;

    public VerletObject(Texture2D sprite, Vector2 position, List<Entity> entities, List<VerletObject> verletObjects, Vector2 startVelocity = new Vector2(), float bounceDamping = 0.5f, bool stationary = false) : base(sprite, position, entities)
    {
        oldPosition = position - startOffset;
        velocity = position - oldPosition;
        this.bounceDamping = bounceDamping;

        verletObjects.Add(this);
    }

    public virtual void Update()
    {
        velocity = (position - oldPosition) * firction;

        oldPosition = position;

        position += velocity;
        position.Y += gravity;

        EdgeCheck();


    }


    private void EdgeCheck()
    {

        if (position.X > Global.width)
        {
            position.X = Global.width;
            oldPosition.X = position.X + velocity.X * bounceDamping;
        }
        else if (position.X < 0)
        {
            position.X = 0;
            oldPosition.X = position.X + velocity.X * bounceDamping;
        }

        if (position.Y > Global.height)
        {
            position.Y = Global.height;
            oldPosition.Y = position.Y + velocity.Y * bounceDamping;
        }
        else if (position.Y < 0)
        {
            position.Y = 0;
            oldPosition.Y = position.Y + velocity.Y * bounceDamping;
        }

    }

}