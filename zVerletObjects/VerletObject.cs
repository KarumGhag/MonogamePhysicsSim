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
    private Vector2 velocity;

    private float bounceDamping;

    private float gravity = 0.3f;
    public float firction = 1f;

    public bool stationary = false;

    public bool grabbed = false;

    public VerletObject(Texture2D sprite, Vector2 position, List<Entity> entities, List<VerletObject> verletObjects, Vector2 startVelocity = new Vector2(), Color? colour = null, bool stationary = false, float bounceDamping = 0.5f) : base(sprite, position, entities, colour)
    {
        oldPosition = position - startVelocity;
        velocity = position - oldPosition;

        this.bounceDamping = bounceDamping;
        this.stationary = stationary;

        verletObjects.Add(this);
    }

    public virtual void Update()
    {

        if (grabbed) position = new Vector2(Global.mouseState.X, Global.mouseState.Y);

        if (!stationary && !grabbed)
        {
            velocity = (position - oldPosition) * firction;

            oldPosition = position;

            position += velocity;
            position.Y += gravity;
        }


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