using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entropy
{
    class Tank
    {
        public Texture2D Texture;
        public Vector2 Position;

        public float Orientation = 0f;
        public float LinearSpeed = 2f;
        public float RotationSpeed = (float)Math.PI / 100;

        public int Fuel = 500;
        public float HP = 100f;

        public List<Bullet> Bullets = new List<Bullet>();

        public Tank()
        {
            
        }

        public Tank(Texture2D texture, Vector2 position, float orientation)
        {
            Texture = texture;
            Position = position;
            Orientation = orientation;
        }

        public Vector2 GetNewPosition(float newLinearSpeedMult, Boolean isCentered = true)
        {
            return Position + newLinearSpeedMult * LinearSpeed * GetRotationVector2() - (isCentered ? new Vector2(0, 0) : new Vector2(Texture.Width / 2, Texture.Height / 2));
        }

        public void ApplySpeed(float newLinearSpeedMult, float newAngularSpeedMult, Boolean consumeFuel = true)
        {
            Position += newLinearSpeedMult * LinearSpeed * GetRotationVector2();
            Orientation += newAngularSpeedMult * RotationSpeed;

            //TODO: MAKE FUEL CONSUMPTION BASED ON LINEAR SPEED!
            int fuelConsumptionTick = 0;

            if (Math.Abs(newLinearSpeedMult) > 0 || Math.Abs(newAngularSpeedMult) > 0)
                fuelConsumptionTick = 1;

            Fuel -= 1 * fuelConsumptionTick;
        }

        public Vector2 GetRotationVector2()
        {
            return new Vector2((float)Math.Sin(Orientation), - (float)Math.Cos(Orientation));
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Position, null, Color.White, Orientation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1, SpriteEffects.None, 0);
        }

        public Rectangle CollisionRectangle()
        {
            return new Rectangle((int)Position.X, (int)Position.Y, Texture.Width, Texture.Height);
        }

        public Boolean DetectHit(Vector2 hitPosition, float Damage)
        {
            if(CollisionRectangle().Contains(hitPosition))
            {
                HP -= Damage;
                return true;
            }

            return false;
        }

        public Boolean IsAlive()
        {
            if (HP > 0)
                return true;
            else
                return false;
        }

        public void UpdateBullets(int WorldWidth, int WorldHeight)
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();

            foreach (Bullet bullet in Bullets)
            {
                bullet.ApplyCurrentVelocity();

                if (bullet.Position.X > WorldWidth || bullet.Position.X < 0 || bullet.Position.Y > WorldHeight || bullet.Position.Y < 0)
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            foreach (Bullet bullet in bulletsToRemove)
            {
                Bullets.Remove(bullet);
            }
        }

        public Boolean Shoot(Texture2D bulletTexture, Color bulletColor, float bulletBaseSpeed)
        {
            Bullets.Add(new Bullet(bulletTexture, bulletColor, Position, bulletBaseSpeed * GetRotationVector2(), Orientation));

            //FIXME: COOLDOWN MECHANICS?
            return true;
        }
    }
}
