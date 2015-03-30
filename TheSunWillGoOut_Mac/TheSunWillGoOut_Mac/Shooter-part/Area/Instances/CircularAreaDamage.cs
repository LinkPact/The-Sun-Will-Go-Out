using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class CircularAreaDamage : AreaDamage
    {
        private Boolean visualizeCircle;
        private float scaleFactor;
        private float radius;
        private Color color;

        public CircularAreaDamage(Game1 game, AreaDamageType type, Vector2 position, float damage, float radius)
            : base(game, type, position, damage)
        {
            this.radius = radius;
            visualizeCircle = true;
            transparency = 0.25f;
            this.color = Color.Orange;
        }

        public override void Initialize()
        {
            base.Initialize();

            anim.LoopTime = 500;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(220, 180, 100, 100)));
            CenterPoint = new Vector2(Width / 2, Height / 2);
            scaleFactor = radius / (Width / 2);
        }

        public override Boolean IsOverlapping(AnimatedGameObject obj)
        {
            Rectangle rect = new Rectangle((int)obj.PositionX, (int)obj.PositionY, obj.BoundingWidth, obj.BoundingHeight);

            if (CollisionDetection.IsCircleInRectangle(Position, radius, rect))
            {
                return true;
            }

            return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (visualizeCircle)
            {
                spriteBatch.Draw(CurrentAnim.CurrentFrame.Texture, Position, CurrentAnim.CurrentFrame.SourceRectangle,
                    color * transparency, 0f, CenterPoint, scaleFactor, SpriteEffects.None, 0.65f);
            }
        }
    }
}
