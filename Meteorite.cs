using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders;

public class Meteorite : Sprite
{
    public Rectangle destinationRectangle;
    public int direction = 1;
    public bool isAlive = true;
    
    public Meteorite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle) :
        base(texture, destinationRectangle, sourceRectangle)
    {
        this.destinationRectangle = destinationRectangle;   
    }

    public virtual void Update(GameTime gameTime, List<Meteorite> meteorites)
    {
        destinationRectangle.X += 1 * direction;
        if (destinationRectangle.X >= 750 || destinationRectangle.X <= 25)
        {
            foreach (var meteorite in meteorites)
            {
                meteorite.destinationRectangle.Y += 35;
                meteorite.direction *= -1;   
            }
        }
    }
}