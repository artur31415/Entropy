using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Entropy
{
    public class Game1 : Game
    {
        Random r = new Random();

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Tank tank;
        Texture2D TankTexture;
        Texture2D TankTexture2;


        List<Tank> EnemyTanks = new List<Tank>();


        List<Bullet> Bullets = new List<Bullet>();
        Texture2D BulletTexture;

        float BulletBaseSpeed = 5f;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            TankTexture = Content.Load<Texture2D>("Tiles/Tanks/tank1_body");
            TankTexture2 = Content.Load<Texture2D>("Tiles/Tanks/tank2_body");
            BulletTexture = Content.Load<Texture2D>("Tiles/Tanks/tank1_gun");

            tank = new Tank(TankTexture, new Vector2(100, 100), 0f);

            
            int enemyCount = r.Next(3, 10);

            for(int i = 0; i < enemyCount; ++i)
            {
                EnemyTanks.Add(new Tank(TankTexture2, new Vector2(r.Next(100, 500), r.Next(100, 500)), (float)(r.NextDouble() * 2 * Math.PI)));
            }

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            for (int i = 0; i < pressedKeys.Length; ++i)
            {
                Vector2 linearSpeed = new Vector2(0, 0);
                float angularDirection = 0f;
                float linearDirection= 0;


                if (pressedKeys[i].Equals(Keys.W))
                    linearDirection= 1;
                else if (pressedKeys[i].Equals(Keys.S))
                    linearDirection= -1;

                if (pressedKeys[i].Equals(Keys.E))
                    angularDirection = 1;
                else if (pressedKeys[i].Equals(Keys.Q))
                    angularDirection = - 1;

                if (pressedKeys[i].Equals(Keys.Space))
                {
                    //then shoot!
                    Bullets.Add(new Bullet(BulletTexture, tank.Position, BulletBaseSpeed * tank.GetRotationVector2(), tank.Orientation));
                }

                if (!GraphicsDevice.Viewport.Bounds.Contains(tank.GetNewPosition(linearDirection, false)))
                    linearDirection = 0;

                tank.ApplySpeed(linearDirection, angularDirection);
            }


            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach(Bullet bullet in Bullets)
            {
                bullet.ApplyCurrentVelocity();

                if (bullet.Position.X > GraphicsDevice.Viewport.Width || bullet.Position.X < 0 || bullet.Position.Y > GraphicsDevice.Viewport.Height || bullet.Position.Y < 0)
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            foreach(Bullet bullet in bulletsToRemove)
            {
                Bullets.Remove(bullet);
            }


            //TODO: ENEMY MOTION LOGIC HERE!
            foreach(Tank enemyTank in EnemyTanks)
            {
                float linearMult = r.Next(3) - 1;
                float angularMult = r.Next(3) - 1;
                if (!GraphicsDevice.Viewport.Bounds.Contains(enemyTank.GetNewPosition(linearMult, false)))
                    linearMult = 0;
                enemyTank.ApplySpeed(linearMult * r.Next(10), angularMult);
            }

            //DAMAGE LOGIC HERE!

            List<Tank> deadTanks = new List<Tank>();

            foreach (Tank enemyTank in EnemyTanks)
            {
                foreach (Bullet bullet in Bullets)
                {
                    if(enemyTank.DetectHit(bullet.Position, 101))
                    {
                        Debug.WriteLine("Enemy Hit!");
                        if(!enemyTank.IsAlive())
                        {
                            deadTanks.Add(enemyTank);
                            Debug.WriteLine("Enemy DEAD!");
                        }
                    }
                }
            }

            foreach (Tank deadTank in deadTanks)
            {
                EnemyTanks.Remove(deadTank);
            }

                base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);

            tank.Draw(_spriteBatch);

            for (int i = 0; i < Bullets.Count; ++i)
            {
                Bullets[i].Draw(_spriteBatch);
            }

            foreach(Tank enemyTank in EnemyTanks)
            {
                enemyTank.Draw(_spriteBatch);
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
