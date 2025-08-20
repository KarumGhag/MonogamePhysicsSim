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
using VerletClass;
using System.Runtime.CompilerServices;

namespace VerletRope;

public class Rope
{
    public VerletObject point1;
    public VerletObject point2;
    private float restLen;
    private Vector2 distanceVec;
    private Vector2 midPoint;

    public bool active = true;

    public Color colour = Global.defaultRopeColour;

    public Rope(VerletObject point1, VerletObject point2, float restLen, List<Rope> ropes)
    {
        this.point1 = point1;
        this.point2 = point2;
        this.restLen = restLen;

        ropes.Add(this);
    }



    private float getCurrentAngle()
    {
        float angle;

        midPoint = (point1.position + point2.position) / 2;
        Vector2 midToP1 = point1.position - midPoint;

        float opposite = midToP1.Y;
        float adjacent = midToP1.X;

        angle = (float)Math.Atan2(opposite, adjacent);

        return angle;
    }

    private Vector2[] getCorrectedPos()
    {
        Vector2 correctedPos1;
        Vector2 correctedPos2;

        float currentAngle = getCurrentAngle();
        float adjacent = (float)Math.Cos(currentAngle) * (restLen / 2);
        float opposite = (float)Math.Sin(currentAngle) * (restLen / 2);


        midPoint = (point1.position + point2.position) / 2;
        correctedPos1 = midPoint + new Vector2(adjacent, opposite);
        correctedPos2 = midPoint - new Vector2(adjacent, opposite);

        Vector2[] correctedPositions = { correctedPos1, correctedPos2 };

        return correctedPositions;
    }


    public void ConstrainPoints()
    {

        if (!active) return;

        distanceVec = point1.position - point2.position;

        Vector2 desiredPoint1 = getCorrectedPos()[0];
        Vector2 desiredPoint2 = getCorrectedPos()[1];

        Vector2 point1Offset = desiredPoint1 - point1.position;
        Vector2 point2Offset = desiredPoint2 - point2.position;


        if (!point1.stationary) point1.position += point1Offset;
        if (!point2.stationary) point2.position += point2Offset;

    }


}