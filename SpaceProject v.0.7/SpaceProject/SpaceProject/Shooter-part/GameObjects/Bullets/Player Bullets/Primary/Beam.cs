using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Beam : PlayerBullet
    {
        #region decl
        float posX;
        float startY;
        float endY;
        #endregion
        public Beam(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }
        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 1.0f;
            IsKilled = false;
            Damage = 0;
            ObjectClass = "beam";
            Duration = 10;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(18, 26, 4, 1)));
            
            Bounding = new Rectangle(18, 26, 4, 1);
            
            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }
        public void Update_(GameTime gameTime, float posX, float startY, float endY)
        {
            this.posX = posX;
            this.startY = startY;
            this.endY = endY;
        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ControlManager.CurrentKeyboardState.IsKeyUp(ControlManager.KeyboardAction))
            {
                IsKilled = true;
            }

        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int m = (int)startY; m > endY; m--)
            {
                spriteBatch.Draw(anim.CurrentFrame.Texture, new Vector2(posX, m), anim.CurrentFrame.SourceRectangle, Color.White, 0, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
            }
        }
    }
}
