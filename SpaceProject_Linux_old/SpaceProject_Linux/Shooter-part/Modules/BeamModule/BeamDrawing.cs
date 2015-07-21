using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    /**
     * Purely visual class and is handled as a background object in the shooterState.
     * Is used to display a beam shot by any weapon.
     * 
     * UpdateLocation is used to update both location and size of the beam.
     */
    public class BeamDrawing : AnimatedGameObject
    {
        private float posX;
        private float startY;
        private float endY;
        private int maxDur = 20;
        private Boolean shootingUpwards;

        private Color color;
        public Color Color { set { color = value; } }

        public BeamDrawing(Game1 Game, Sprite spriteSheet, Boolean shootingUpwards)
            : base(Game, spriteSheet)
        {
            this.shootingUpwards = shootingUpwards;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            IsKilled = false;
            Damage = 0f;
            ObjectClass = "beam";
            Duration = maxDur;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(17, 26, 5, 1)));
            
            Bounding = new Rectangle(18, 26, 4, 1);
            
            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            DrawLayer = 0.4f; // Same as bullets

            color = Color.White;
        }

        public void UpdateLocation(GameTime gameTime, float posX, float startY, float endY)
        {
            this.posX = posX;
            this.startY = startY;
            this.endY = endY;

            Duration = maxDur;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Duration -= gameTime.ElapsedGameTime.Milliseconds;

            if (Duration <= 0)
            {
                IsKilled = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (shootingUpwards)
            {
                for (int m = (int)startY; m > endY; m--)
                {
                    spriteBatch.Draw(anim.CurrentFrame.Texture, new Vector2(posX, m), anim.CurrentFrame.SourceRectangle, color, 0, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
                }
            }
            else
            {
                for (int m = (int)startY; m < endY; m++)
                {
                    spriteBatch.Draw(anim.CurrentFrame.Texture, new Vector2(posX, m), anim.CurrentFrame.SourceRectangle, color, 0, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
                }
            }
        }

        public override void OnKilled()
        { }
    }
}
