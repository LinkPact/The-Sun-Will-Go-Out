using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Fortrun : SubInteractiveObject
    {
        public Fortrun(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(1071, 1075, 512, 513));
            position = new Vector2(85000 + 7000, 85000 + 10000);
            name = "Fortrun";

            base.Initialize();

            overworldEvent = new DisplayTextOE("A big planet rich with valuable resources. Heavy exploitation over many years has left the planet's atmosphere extremely hazardous.");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            base.Interact();
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
