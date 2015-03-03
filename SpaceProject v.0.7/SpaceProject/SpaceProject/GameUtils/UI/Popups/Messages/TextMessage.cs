using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class TextMessage : Popup
    {
        private readonly float TextLayerDepth = 1f;

        public TextMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 56, 269, 184));
        }

        public override void Initialize()
        {
            base.Initialize();

            useScrolling = true;
            usePause = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (textBuffer.Count > 0)
            {
                text = TextUtils.WordWrap(game.fontManager.GetFont(14),
                                          TextUtils.ScrollText(textBuffer[0],
                                                               flushScrollingText,
                                                               out textScrollingFinished),
                                          (int)Math.Round(((float)canvas.SourceRectangle.Value.Width),
                                          0));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawString(game.fontManager.GetFont(14),
                                   text,
                                   new Vector2(textPosition.X,
                                               textPosition.Y) + game.fontManager.FontOffset,
                                   game.fontManager.FontColor,
                                   0f,
                                   Vector2.Zero,
                                   1f,
                                   SpriteEffects.None,
                                   TextLayerDepth);
        }
    }
}
