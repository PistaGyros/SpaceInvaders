using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player player;
    private Bullet bullet;
    private Meteorite meteorite;
    private int numberOfMeteorites;

    private float bulletAliveTime;

    private List<Sprite> sprites;
    private List<Bullet> bullets;
    private List<Meteorite> meteorites;
    
    private Texture2D bulletTexture;
    private Texture2D meteoriteTexture;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 1000;
        _graphics.ApplyChanges();
        Content.RootDirectory = "Content";
        IsMouseVisible = true;
    }

    protected override void Initialize()
    {
        // TODO: Add your initialization logic here

        sprites = new List<Sprite>();
        bullets = new List<Bullet>();
        meteorites = new List<Meteorite>();
        numberOfMeteorites = 30;
        bulletAliveTime = 0;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        Texture2D playerTexture = Content.Load<Texture2D>("player_v2");
        meteoriteTexture = Content.Load<Texture2D>("invader_v1");
        bulletTexture = Content.Load<Texture2D>("bullet");
        
        player = new Player(playerTexture,
            new Rectangle(400 - (playerTexture.Width / 2), 900, playerTexture.Width * 2, playerTexture.Height * 2),
            new Rectangle(0, 0, playerTexture.Width, playerTexture.Height));
        
        SpawnMeteorites();
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        bulletAliveTime -= 0.025f;
        
        // fire bullet
        if (bulletAliveTime <= 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
        {
            bulletAliveTime = 2f;
            int bulletX = player.destinationRectangle.X + player.destinationRectangle.Width / 2;
            bullets.Add(bullet = new Bullet(bulletTexture,
                new Rectangle(bulletX, player.destinationRectangle.Y + 5, bulletTexture.Width, bulletTexture.Height),
                new Rectangle(0, 0, bulletTexture.Width, bulletTexture.Height)));
        }

        foreach (Bullet bullet in bullets)
        {
            if (bullet.IsAlive)
            {
                bullet.Update(gameTime, meteorites);   
            }
        }
        
        foreach (var meteorite in meteorites)
        {
            if (meteorite.isAlive)
            {
                meteorite.Update(gameTime, meteorites);
            }
        }
        
        
        foreach (var sprite in sprites)
        {
            sprite.Update(gameTime);
        }
        
        player.Update(gameTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        foreach (var bullet in bullets)
        {
            if (bullet.IsAlive)
            {
                _spriteBatch.Draw(bullet.texture, bullet.destinationRectangle, bullet.sourceRectangle, Color.White);
            }
        }
        
        foreach (var meteorite in meteorites) 
        {
            if (meteorite.isAlive)
                _spriteBatch.Draw(meteorite.texture, meteorite.destinationRectangle, meteorite.sourceRectangle, Color.White);
        }   

        foreach (var sprite in sprites)
        {
            _spriteBatch.Draw(sprite.texture, sprite.destinationRectangle, sprite.sourceRectangle, Color.White);
        }
        
        _spriteBatch.Draw(player.texture, player.destinationRectangle, player.sourceRectangle, Color.White);

        _spriteBatch.End();
        
        base.Draw(gameTime);
    }

    private void SpawnMeteorites()
    {
        int meteoriteX = 70;
        int meteoriteY = 70;
        int meteoritesInRow = 0;
        for (int i = 0; i < numberOfMeteorites; i++)
        {
            int newMeteoriteX = meteoriteX + meteoriteX * meteoritesInRow;
            meteorites.Add(meteorite = new Meteorite(meteoriteTexture,
                new Rectangle(newMeteoriteX, meteoriteY, meteoriteTexture.Width, meteoriteTexture.Height),
                new Rectangle(0, 0, meteoriteTexture.Width, meteoriteTexture.Height)));
            meteoritesInRow++;
            
            // end of row, move to next one
            if (meteoritesInRow == 10 || meteoritesInRow == 20 || meteoritesInRow == 30)
            {
                meteoriteY += 70;
                meteoritesInRow = 0;
            }
        }
    }
}
