using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class OptionDurationEventSquare : OptionSquare
    {
        private DurationEventType optionState;
        private Sprite overlaySprite;

        public OptionDurationEventSquare(Sprite spriteSheet, Vector2 position, DurationEventType state) 
            : base(spriteSheet, position)
        {
            displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
            optionState = state;
            overlaySprite = DurationSquare.GetEventSprite(state);
            readyToSetDisplay = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (IsLeftClicked())
            {
                readyToSetDisplay = true;
                ActiveData.isPointActive = false;
            }
        }

        public override void SetDisplay(ActiveSquare display)
        {
            if (readyToSetDisplay == true)
            {
                ((ActiveDurationEventSquare)display).SetDisplay(optionState);
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
