using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class TestAsteroid1 : SubInteractiveObject
    {
        public TestAsteroid1(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1075, 54, 55));
            
            
            position = MathFunctions.CoordinateToPosition(new Vector2(1817, 135));
            name = "TestAsteroid1";

            base.Initialize();
            SetupText("I think Jakob is experimenting with Space Objects!");
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
            clearedText = "Not applicable?";
        }
    }
}
