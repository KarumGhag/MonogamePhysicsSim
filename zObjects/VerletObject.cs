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

namespace VerletClass;

public class VerletObject : Entity
{

    private Vector2 oldPosition;

    public VerletObject(Texture2D sprite, Vector2 position, List<Entity> entities, bool stationary = false) : base(sprite, position, entities)
    {
        oldPosition = position;
    }
}