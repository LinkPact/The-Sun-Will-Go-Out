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
            //String encounterText = "Pirates! Let's root em out!";
            //String winText = "We scared them of, for now..";
            //String loseText = "They are too strong. Let's come back later.";
            //List<Item> rewards = new List<Item>();
            //rewards.Add(new BasicLaserWeapon(Game, ItemVariety.Regular));
            //overworldEvent = new LevelOE(encounterText, "sub1", 100, rewards, winText, loseText);

            overworldEvent = new PirateEncounterOE();
        }

        public override void Update(GameTime gameTime)
        {
            overworldEvent.Update(Game, gameTime);
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
