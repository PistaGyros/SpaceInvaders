using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceInvaders;

public class Sprite
{
    public Texture2D texture;
    public Rectangle destinationRectangle;
    public Rectangle sourceRectangle;
    
    public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle)
    {
        this.texture = texture;
        this.destinationRectangle = destinationRectangle;
        this.sourceRectangle = sourceRectangle;
    }
    
    public virtual void Update(GameTime gameTime){}
}