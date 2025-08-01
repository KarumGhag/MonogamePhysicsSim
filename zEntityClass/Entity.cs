using System.Collections.Generic;
using System.Xml.Serialization;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonogamePhysicsSim;

namespace EntityClass;

public class Entity
{
    public Texture2D sprite;
    public Vector2 position;

    public Entity(Texture2D sprite, Vector2 position, List<Entity> entities)
    {
        this.sprite = sprite;
        this.position = position;
        entities.Add(this);
    }

    public virtual void Update(GameTime gameTime)
    {

    }

}
