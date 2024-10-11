using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders;

public class Player : Sprite
{
    public Rectangle destinationRectangle;
    public bool isAlive = true;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle) : base(texture, destinationRectangle, sourceRectangle)
    {
        this.destinationRectangle = destinationRectangle;
    }

    public virtual void Update(GameTime gameTime, List<Meteorite> meteorites)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            destinationRectangle.X -= 3;
        }
        else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            destinationRectangle.X += 3;
        }

        foreach (var meteorite in meteorites)
        {
            if (meteorite.destinationRectangle.Intersects(this.destinationRectangle))
            {
                // GAME OVER
                isAlive = false;
            }
        }
    }
}