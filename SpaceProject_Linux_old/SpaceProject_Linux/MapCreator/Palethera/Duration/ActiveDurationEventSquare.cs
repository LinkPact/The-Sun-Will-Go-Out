using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class ActiveDurationEventSquare : ActiveSquare
    {
        private Sprite overlaySprite;

        public ActiveDurationEventSquare(Sprite spriteSheet, Vector2 position)
            : base(position)
        {
            this.displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
            this.position = position;
            color = Color.White;
        }

        public void SetDisplay(DurationEventType newEventType)
        {
            ActiveData.durationEventState = newEventType;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            overlaySprite = DurationSquare.GetEventSprite(ActiveData.durationEventState);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (overlaySprite != null)
                spriteBatch.Draw(overlaySprite.Texture, position, overlaySprite.SourceRectangle, Color.White);
        }
    }
}
