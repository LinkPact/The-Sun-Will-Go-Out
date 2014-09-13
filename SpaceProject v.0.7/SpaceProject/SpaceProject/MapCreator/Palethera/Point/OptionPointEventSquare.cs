using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class OptionPointEventSquare : OptionSquare
    {
        private PointEventType optionState;
        private Sprite overlaySprite;

        public OptionPointEventSquare(Sprite spriteSheet, Vector2 position, PointEventType state) 
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
            optionState = state;
            overlaySprite = PointSquare.GetEventSprite(state);
            readyToSetDisplay = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLeftClicked())
            {
                readyToSetDisplay = true;
                ActiveData.isPointActive = true;
            }
        }

        public override void SetDisplay(ActiveSquare display)
        {
            if (readyToSetDisplay == true)
            {
                ((ActivePointEventSquare)display).SetDisplay(optionState);
                readyToSetDisplay = false; 
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.Draw(overlaySprite.Texture, position, overlaySprite.SourceRectangle, Color.White);
        }
    }
}
