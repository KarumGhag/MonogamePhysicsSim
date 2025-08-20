using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

using MonogamePhysicsSim;
using GlobalInfo;

namespace EntityClass;

public class Entity
{
    public Texture2D sprite;
    public Vector2 position;
    public float deltaTime;

    public Color colour;

    public bool renderBall = false;

    public Entity(Texture2D sprite, Vector2 position, List<Entity> entities, Color? colour = null)
    {
        this.sprite = sprite;
        this.position = position;
        this.colour = colour ?? Global.defaultPointColour;
        entities.Add(this);

    }

    public virtual void Update(GameTime gameTime)
    {
        deltaTime = Global.deltaTime;
    }

}
