using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entropy
{
    class Bullet
    {

        public Vector2 Position = new Vector2();
        public Vector2 Velocity = new Vector2();
        public Texture2D Texture;
        public float Orientation = 0f;

        public Bullet()
        { 
        }

        public Bullet(Texture2D texture, Vector2 position, Vector2 velocity, float orientation)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Orientation = orientation;
        }

        public void ApplyCurrentVelocity()
        {
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.Red, Orientation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
