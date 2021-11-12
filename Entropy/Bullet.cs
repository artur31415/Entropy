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

        public float BaseSpeed = 5f;

        public Color color;

        public Bullet()
        { 
        }

        public Bullet(Texture2D texture, Color color, Vector2 position, Vector2 velocity, float orientation)
        {
            Texture = texture;
            Position = position;
            Velocity = velocity;
            Orientation = orientation;
            this.color = color;
        }

        public void ApplyCurrentVelocity()
        {
            Position += Velocity;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, color, Orientation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1, SpriteEffects.None, 0);
        }
    }
}
