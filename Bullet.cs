using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceInvaders;

public class Bullet : Sprite
{
    public bool IsAlive = true;
    public Rectangle destinationRectangle;
    
    public Bullet(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle) :
        base(texture, destinationRectangle, sourceRectangle)
    {
        this.destinationRectangle = destinationRectangle;
    }

    public virtual void Update(GameTime gameTime, List<Meteorite> meteorites)
    {
        if (IsAlive)
        {
            // check for collision between meteorite and bullet
            List <Meteorite> destroyedMeteorites = new List<Meteorite>();
            foreach (Meteorite meteorite in meteorites)
            {
                if (meteorite.destinationRectangle.Intersects(this.destinationRectangle))
                {
                    meteorite.isAlive = false;
                    destroyedMeteorites.Add(meteorite);
                    this.IsAlive = false;
                }
            }
            foreach (var item in destroyedMeteorites)
            {
                meteorites.Remove(item);
            }
            
            
            destinationRectangle.Y -= 5;
            if (destinationRectangle.Y <= -50)
            {
                IsAlive = false;
            }
        }
    }
}