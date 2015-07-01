using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Lavis : SubInteractiveObject
    {
        public Lavis(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 1080, 310, 309));
            position = new Vector2(85000 + 28000, 85000 + 1000);
            name = "Lavis";

            base.Initialize();

            overworldEvent = new DisplayTextOE("The surface is completly covered in ice. The main source of income is water harvesting.");
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
