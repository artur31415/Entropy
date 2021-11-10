using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entropy
{
    class Bullet
    {

        Vector2 Position = new Vector2();
        Vector2 Velocity = new Vector2();
        public Texture2D Texture;
        float Orientation = 0f;

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

        }
    }
}
