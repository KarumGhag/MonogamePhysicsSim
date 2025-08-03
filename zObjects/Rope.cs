using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using EntityClass;
using GlobalInfo;
using PhysicsClass;
using System.Net.NetworkInformation;
using System.Runtime.CompilerServices;
using System;
using System.Reflection.Metadata;
using System.Linq;
using System.Xml.Schema;
using Microsoft.VisualBasic;

namespace RopeClass;

public class Rope
{
    public List<PhysicsObject> points;
    private int numPoints;
    private int pointMass;
    private Vector2 anchorPos;
    public Rope(int numPoints, int pointMass, Vector2 anchorPos, List<Rope> ropes)
    {
        ropes.Add(this);
        
        this.numPoints = numPoints;
        this.pointMass = pointMass;
        this.anchorPos = anchorPos;

        points = new List<PhysicsObject>();

        for (int i = 0; i < numPoints; i++)
        {
            points.Add(new PhysicsObject(Global._circle, anchorPos + new Vector2(i * 50, 0), Global.entities, pointMass, true));
        }


    }

}