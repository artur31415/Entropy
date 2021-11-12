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


        Texture2D BulletTexture;

        float BulletBaseSpeed = 5f;

        SpriteFont MainFont;
        SpriteFont GameOverFont;

        int WinCounter = 0;

        Rectangle ArenaRectangle;
        int ArenaMargin = 50;

        public void ResetGame()
        {
            tank = new Tank(TankTexture, new Vector2(100, 100), 0f);

            int enemyCount = r.Next(3, 10);

            SpawnEnemies(enemyCount);
        }

        public void SpawnEnemies(int counter)
        {
            for (int i = 0; i < counter; ++i)
            {
                EnemyTanks.Add(new Tank(TankTexture2, new Vector2(r.Next(150, ArenaRectangle.Width), r.Next(150, ArenaRectangle.Height)), (float)(r.NextDouble() * 2 * Math.PI)));
            }
        }

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

            MainFont = Content.Load<SpriteFont>("MainFont");
            GameOverFont = Content.Load<SpriteFont>("GameOverFont");

            ArenaRectangle = new Rectangle(ArenaMargin, ArenaMargin, GraphicsDevice.Viewport.Bounds.Width - ArenaMargin * 2, GraphicsDevice.Viewport.Bounds.Height - ArenaMargin * 2);
            ResetGame();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            if (!tank.IsAlive())
                return;

            if(EnemyTanks.Count == 0)
            {
                WinCounter++;
                ResetGame();
            }

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
                    tank.Shoot(BulletTexture, Color.Red, BulletBaseSpeed);
                }

                if (!ArenaRectangle.Contains(tank.GetNewPosition(linearDirection, false)))
                    linearDirection = 0;

                tank.ApplySpeed(linearDirection, angularDirection);
            }


            tank.UpdateBullets(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);


            //TODO: ENEMY MOTION LOGIC HERE!
            foreach(Tank enemyTank in EnemyTanks)
            {
                float linearMult = r.Next(3) - 1;
                float angularMult = r.Next(3) - 1;
                if (!ArenaRectangle.Contains(enemyTank.GetNewPosition(linearMult, false)))
                    linearMult = 0;
                enemyTank.ApplySpeed(linearMult * r.Next(10), angularMult);


                //TODO: ENEMY ACTION HERE!
                if(r.Next(100) < 10)
                {
                    enemyTank.Shoot(BulletTexture, Color.Green, BulletBaseSpeed);
                }

                enemyTank.UpdateBullets(GraphicsDevice.Viewport.Width, GraphicsDevice.Viewport.Height);
            }


            foreach (Tank enemyTank in EnemyTanks)
            {
                Bullet usedBullet = null;

                foreach (Bullet enemyBullet in enemyTank.Bullets)
                {
                    if (tank.DetectHit(enemyBullet.Position, 2))
                    {
                        Debug.WriteLine("Player Hit!");
                        if (!tank.IsAlive())
                        {
                            Debug.WriteLine("Player DEAD!");
                        }
                        usedBullet = enemyBullet;
                        break;
                    }
                }

                if (usedBullet != null)
                    enemyTank.Bullets.Remove(usedBullet);
            }

            //DAMAGE LOGIC HERE!

            List<Tank> deadTanks = new List<Tank>();

            foreach (Tank enemyTank in EnemyTanks)
            {
                Bullet usedBullet = null;

                foreach (Bullet bullet in tank.Bullets)
                {
                    if(enemyTank.DetectHit(bullet.Position, 10))
                    {
                        Debug.WriteLine("Enemy Hit!");
                        if(!enemyTank.IsAlive())
                        {
                            deadTanks.Add(enemyTank);
                            Debug.WriteLine("Enemy DEAD! ");
                            //TODO: REWARD MECHANICS HERE!
                            tank.Fuel += r.Next(20);
                        }
                        usedBullet = bullet;
                        break;
                    }
                }

                if (usedBullet != null)
                    tank.Bullets.Remove(usedBullet);
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

            for (int i = 0; i < tank.Bullets.Count; ++i)
            {
                tank.Bullets[i].Draw(_spriteBatch);
            }

            foreach(Tank enemyTank in EnemyTanks)
            {
                enemyTank.Draw(_spriteBatch);
                foreach (Bullet enemyBullet in enemyTank.Bullets)
                {
                    enemyBullet.Draw(_spriteBatch);
                }
            }


            _spriteBatch.DrawString(MainFont, "HP: " + tank.HP.ToString() + "\nFuel: " + tank.Fuel.ToString(), new Vector2(10, 10), Color.White);


            _spriteBatch.DrawString(MainFont, "Wave: " + WinCounter.ToString() + "  Enemies: " + EnemyTanks.Count.ToString(), new Vector2(GraphicsDevice.Viewport.Width / 2, 10), Color.White);


            if(!tank.IsAlive())
            {
                _spriteBatch.DrawString(MainFont, "GAME OVER", new Vector2(GraphicsDevice.Viewport.Width / 2, GraphicsDevice.Viewport.Height / 2), Color.White);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
