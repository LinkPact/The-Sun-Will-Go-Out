using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RealTimeMessage : Popup
    {
        private readonly float TextLayerDepth = 1f;

        public RealTimeMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(354, 0, 400, 128));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            canvasPosition = new Vector2(game.camera.cameraPos.X,
                                         game.camera.cameraPos.Y + game.Window.ClientBounds.Height / 4);

            textPosition = new Vector2(canvasPosition.X - canvas.Width / 2 + 30,
                                       canvasPosition.Y - canvas.Height / 2 + 30);

            text = TextUtils.WordWrap(game.fontManager.GetFont(14),
                                      TextUtils.ScrollText(textBuffer[0],
                                                           flushScrollingText,
                                                           out textScrollingFinished),
                                      (int)Math.Round(((float)canvas.SourceRectangle.Value.Width),
                                      0));
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
