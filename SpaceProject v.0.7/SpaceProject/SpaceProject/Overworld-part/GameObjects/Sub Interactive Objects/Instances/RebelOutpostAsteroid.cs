using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RebelOutpostAsteroid : SubInteractiveObject
    {
        public RebelOutpostAsteroid(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1075, 54, 55));
            
            
            position = MathFunctions.CoordinateToPosition(new Vector2(1917, 135));
            name = "RebelOutpostAsteroid";

            base.Initialize();

            SetupText("The Rebel Outpost Asteroid!");

            //SetupText("I think Jakob is experimenting with Space Objects!");
            //String encounterText = "Rebels! Let's root em out!";
            //String winText = "We scared them of, for now..";
            //String loseText = "They are too strong. Let's come back later.";
            //List<Item> rewards = new List<Item>();
            //rewards.Add(new BasicLaserWeapon(Game, ItemVariety.regular));
            //SetupLevel(encounterText, "sub1", 100, rewards, winText, loseText);
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
