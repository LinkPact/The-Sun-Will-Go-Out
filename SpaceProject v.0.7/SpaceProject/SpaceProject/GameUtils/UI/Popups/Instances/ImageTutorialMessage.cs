using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class ImageTutorialMessage : ImageMessage
    {
        private Sprite uncheckedTutorialButton;
        private Sprite checkedTutorialButton;
        private Sprite uncheckedSelectedTutorialButton;
        private Sprite checkedSelectedTutorialButton;

        public ImageTutorialMessage(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            uncheckedTutorialButton = spriteSheet.GetSubSprite(new Rectangle(271, 0, 78, 22));
            checkedTutorialButton = spriteSheet.GetSubSprite(new Rectangle(271, 48, 78, 22));
            uncheckedSelectedTutorialButton = spriteSheet.GetSubSprite(new Rectangle(271, 24, 78, 22));
            checkedSelectedTutorialButton = spriteSheet.GetSubSprite(new Rectangle(271, 72, 78, 22));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
