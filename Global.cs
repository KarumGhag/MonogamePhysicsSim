using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;

namespace GlobalInfo;

//this is a centeralised global game state file that allows all classes and files to get reference to the same data that will always be consistant
public static class Global
{
    public static MouseState mouseState;
    public static GameTime gameTime;
    public static float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

}