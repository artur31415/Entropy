using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;

namespace Entropy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D tankTexture;
        Vector2 tankPosition = new Vector2(200, 200);
        Point tankSize = new Point(50, 50);
        int tankSpeed = 3;

        float tankOrientation = 0f;
        float rotationSpeed = (float)Math.PI / 100;

        Boolean IsNewShoot = false;

        ArrayList Bullets = new ArrayList();
        Texture2D BulletTexture;

        float BulletBaseSpeed = 5f;

        public void ApplySpeed(Vector2 newLinearSpeed, float newAngularSpeed)
        {
            tankPosition = tankPosition + newLinearSpeed;
            tankOrientation += newAngularSpeed;
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
            tankTexture = Content.Load<Texture2D>("Tiles/Tanks/tank1_body");
            BulletTexture = Content.Load<Texture2D>("Tiles/Tanks/tank1_gun");
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
                float angularSpeed = 0f;
                float dir = 0;


                if (pressedKeys[i].Equals(Keys.W))
                    dir = 1;
                else if (pressedKeys[i].Equals(Keys.S))
                    dir = -1;

                if (pressedKeys[i].Equals(Keys.E))
                    angularSpeed = rotationSpeed;
                else if (pressedKeys[i].Equals(Keys.Q))
                    angularSpeed = - rotationSpeed;

                if (!IsNewShoot && pressedKeys[i].Equals(Keys.Space))
                {
                    //then shoot!
                    IsNewShoot = true;

                    Bullets.Add(new Bullet(BulletTexture, tankPosition, new Vector2(BulletBaseSpeed * (float)Math.Sin(tankOrientation), -BulletBaseSpeed * (float)Math.Cos(tankOrientation)), tankOrientation));
                }

                ApplySpeed(new Vector2(dir * (float)Math.Sin(tankOrientation), - dir * (float)Math.Cos(tankOrientation)), angularSpeed);
            }

            if(IsNewShoot)
            {
                BulletPosition += BulletVelocity;

                if(BulletPosition.X > GraphicsDevice.Viewport.Width || BulletPosition.X < 0 || BulletPosition.Y > GraphicsDevice.Viewport.Height || BulletPosition.Y < 0)
                {
                    IsNewShoot = false;
                }
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);

            

            Rectangle tankRect = new Rectangle((int)tankPosition.X, (int)tankPosition.Y, tankSize.X, tankSize.Y);

            _spriteBatch.Draw(tankTexture, tankPosition, null, Color.White, tankOrientation, new Vector2(tankTexture.Width / 2, tankTexture.Height / 2), 1, SpriteEffects.None, 0);
            //_spriteBatch.Draw(tankTexture, new Rectangle(tankPosition.X, tankPosition.Y, tankSize.X, tankSize.Y), Color.White);

            if(Bullets.Count > 0)
            {
                for (int i = 0; i < Bullets.Count; ++i)
                {
                    _spriteBatch.Draw(Bullets[i].Texture, BulletPosition, null, Color.Red, BulletOrientation, new Vector2(BulletTexture.Width / 2, BulletTexture.Height / 2), 1, SpriteEffects.None, 0);
                }
            }


            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
