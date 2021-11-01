using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Entropy
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        Texture2D tankTexture;
        Point tankPosition = new Point(100, 100);
        Point tankSize = new Point(50, 50);
        int tankSpeed = 3;

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
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here

            Keys[] pressedKeys = Keyboard.GetState().GetPressedKeys();

            for (int i = 0; i < pressedKeys.Length; ++i)
            {
                if (pressedKeys[i].Equals(Keys.W))
                    tankPosition.Y -= tankSpeed;
                else if (pressedKeys[i].Equals(Keys.S))
                    tankPosition.Y += tankSpeed;
                else if (pressedKeys[i].Equals(Keys.A))
                    tankPosition.X -= tankSpeed;
                else if (pressedKeys[i].Equals(Keys.D))
                    tankPosition.X += tankSpeed;
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: Add your drawing code here

            _spriteBatch.Begin(SpriteSortMode.Immediate, null, null, null, null, null, null);

            Rectangle tankRect = new Rectangle(tankPosition.X, tankPosition.Y, tankSize.X, tankSize.Y);

            _spriteBatch.Draw(tankTexture, tankPosition, null, Color.White, (float)Math.PI, new Vector2(0, 0), 1, SpriteEffects.None, 0);
            //_spriteBatch.Draw(tankTexture, new Rectangle(tankPosition.X, tankPosition.Y, tankSize.X, tankSize.Y), Color.White);

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
