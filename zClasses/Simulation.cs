using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

using EntityClass;
using PhysicsClass;
using GlobalInfo;
using FollowClass;
using SpringClass;
using VerletClass;
using VerletRope;
using MonogamePhysicsSim;
using System.Security.Cryptography;

namespace Simulation;

public class SimulationClass
{

    public Game1 game1;
    public SpriteBatch _spriteBatch;

    public int _screenWidth;
    public int _screenHeight;

    public KeyboardState newKbState;
    public KeyboardState oldKbState;


    public string SimulationName;

    public SimulationClass(Game1 game1)
    {
        this.game1 = game1;
        _spriteBatch = game1._spriteBatch;

        _screenWidth = Global.width;
        _screenHeight = Global.height;

        oldKbState = Keyboard.GetState();

    }


    public virtual void Update(GameTime gameTime)
    {
        newKbState = Keyboard.GetState();
    }

    public virtual void Draw(GameTime gameTime)
    {

    }

}