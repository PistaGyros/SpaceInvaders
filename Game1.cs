using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceInvaders;

public class Game1 : Game
{
    private GraphicsDeviceManager _graphics;
    private SpriteBatch _spriteBatch;

    private Player player;
    private Bullet bullet;
    private Meteorite meteorite;
    private SpriteFont pixelFont;
    
    private Vector2 scorePosition;
    public int numberOfMeteorites;
    public int actualNumOfMeteorites;
    public int score;
    private float bulletAliveTime;
    private bool gameOver;
    private bool playerWin;

    private List<Sprite> sprites;
    private List<Bullet> bullets;
    private List<Meteorite> meteorites;
    
    private Texture2D bulletTexture;
    private Texture2D meteoriteTexture;
    private Texture2D bg;
    private Song bgMusic;
    private SoundEffect fireBulletSound;
    private SoundEffect destroyMeteoriteSound;

    public Game1()
    {
        _graphics = new GraphicsDeviceManager(this);
        _graphics.PreferredBackBufferWidth = 800;
        _graphics.PreferredBackBufferHeight = 800;
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
        scorePosition = new Vector2(10, 10);
        gameOver = false;
        
        base.Initialize();
    }

    protected override void LoadContent()
    {
        _spriteBatch = new SpriteBatch(GraphicsDevice);

        // TODO: use this.Content to load your game content here

        fireBulletSound = Content.Load<SoundEffect>("Space Invaders_laser");
        destroyMeteoriteSound = Content.Load<SoundEffect>("Space Invaders_explosion");
        bgMusic = Content.Load<Song>("Cinematic-electronic-track");
        MediaPlayer.Play(bgMusic);
        MediaPlayer.IsRepeating = true;
        
        pixelFont = Content.Load<SpriteFont>("pixel_font");

        bg = Content.Load<Texture2D>("space_invaders_background");
        meteoriteTexture = Content.Load<Texture2D>("invader_v1");
        bulletTexture = Content.Load<Texture2D>("bullet");
        
        Texture2D playerTexture = Content.Load<Texture2D>("player_v2");
        player = new Player(playerTexture,
            new Rectangle(400 - (playerTexture.Width / 2), 700, playerTexture.Width * 2, playerTexture.Height * 2),
            new Rectangle(0, 0, playerTexture.Width, playerTexture.Height));
        player.OnDeath += PlayerOnOnDeath;
        
        SpawnMeteorites();
    }

    private void PlayerOnOnDeath(object sender, EventArgs e)
    {
        gameOver = true;
        playerWin = false;
    }

    protected override void Update(GameTime gameTime)
    {
        if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            Exit();

        // TODO: Add your update logic here

        if (!gameOver)
        {
            bulletAliveTime -= 0.025f;
        
            // fire bullet
            if (bulletAliveTime <= 0 && Keyboard.GetState().IsKeyDown(Keys.Space))
            {
                FireBullet();
            }

            foreach (Bullet bullet in bullets)
            {
                if (bullet.IsAlive)
                {
                    bullet.Update(gameTime, meteorites, destroyMeteoriteSound);
                }
            }
        
            foreach (var meteorite in meteorites)
            {
                if (meteorite.isAlive)
                {
                    meteorite.Update(gameTime, meteorites);
                }
            }
        
            // calculate score
            score = (numberOfMeteorites - meteorites.Count) * 10;
            if (score >= numberOfMeteorites * 10)
            {
                // player win
                gameOver = true;
                playerWin = true;
            }
        
        
            foreach (var sprite in sprites)
            {
                sprite.Update(gameTime);
            }
        
            player.Update(gameTime, meteorites, destroyMeteoriteSound);
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(Color.CornflowerBlue);

        // TODO: Add your drawing code here
        
        _spriteBatch.Begin(samplerState: SamplerState.PointClamp);

        _spriteBatch.Draw(bg, new Rectangle(0, 0, 800, 800), Color.White);

        if (!gameOver)
        {
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
        }
        else
        {
            string endGameText = playerWin ? "GALAXY IS SAVED" : "GAME OVER";
            Vector2 endGameTxtPos = new Vector2
            (_graphics.PreferredBackBufferWidth / 2 - pixelFont.MeasureString(endGameText).X / 2,
                _graphics.PreferredBackBufferHeight / 2 - 20);
            _spriteBatch.DrawString(pixelFont, endGameText, endGameTxtPos, Color.Red);
        }
        
        
        _spriteBatch.DrawString(pixelFont, "Score: " + score, scorePosition, Color.White);

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

    private void FireBullet()
    {
        fireBulletSound.Play();
        bulletAliveTime = 2f;
        int bulletX = player.destinationRectangle.X + player.destinationRectangle.Width / 2;
        bullets.Add(bullet = new Bullet(bulletTexture,
            new Rectangle(bulletX, player.destinationRectangle.Y + 5, bulletTexture.Width, bulletTexture.Height),
            new Rectangle(0, 0, bulletTexture.Width, bulletTexture.Height)));
    }
}
