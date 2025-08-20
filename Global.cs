using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
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
    public static float deltaTime;
    public static Texture2D _circle;

    public static int width;
    public static int height;

    public static Color backgroundColour = Color.Black;
    public static Color defaultRopeColour = Color.White;
    public static Color selectedRopeColour = Color.Lime;
    public static Color defaultPointColour = Color.Blue;
    public static Color stationaryPointColour = Color.Red;
    public static Color editingPointColour = Color.SkyBlue;

    public static float GetVectorLen(Vector2 vector)
    {
        double xLen = Math.Pow(vector.X, 2);
        double yLen = Math.Pow(vector.Y, 2);

        return (float)Math.Sqrt(xLen + yLen);

    }

}