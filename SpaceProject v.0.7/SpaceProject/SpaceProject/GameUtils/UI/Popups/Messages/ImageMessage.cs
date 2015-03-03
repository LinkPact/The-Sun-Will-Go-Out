﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ImageMessage : ImagePopup
    {
        TextContainer textContainer;

        public ImageMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 245, 400, 400));
        }

        public override void Initialize()
        {
            base.Initialize();

            textContainer = new TextContainer(game, canvas.SourceRectangle.Value);
            textContainer.Initialize();
            textContainer.UseScrolling = true;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            textContainer.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            textContainer.Draw(spriteBatch);
        }

        public void SetMessage(params string[] messages)
        {
            textContainer.SetMessage(messages);
        }
    }
}
