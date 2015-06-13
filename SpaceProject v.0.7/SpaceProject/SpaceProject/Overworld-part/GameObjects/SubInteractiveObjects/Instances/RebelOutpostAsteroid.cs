using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class RebelOutpostAsteroid : Asteroid
    {
        private bool activated;
        public bool Activated { get { return activated; } set { activated = value; } }

        private static readonly Vector2 coordinate = new Vector2(-728, -1180);

        public RebelOutpostAsteroid(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet, coordinate)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1130, 54, 55));
            
            //position = MathFunctions.CoordinateToPosition(new Vector2(-728, -1180));
            name = "RebelOutpostAsteroid";

            color = Color.Orange;

            base.Initialize();

            //overworldEvent = new DisplayTextOE("The Rebel Outpost Asteroid!");

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
            Vector2 test = position;
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            activated = true;
            base.Interact();
        }

        protected override void SetClearedText()
        {
            clearedText = "Not applicable?";
        }
    }
}
