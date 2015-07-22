using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class LonelyAsteroid : SubInteractiveObject
    {
        public LonelyAsteroid(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            
        }

        public override void Initialize()
        {
            Item item = new BasicLaserWeapon(Game, ItemVariety.High);
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1130, 54, 55));
            position = new Vector2(110000, 102000);
            name = "Lonely Asteroid";

            base.Initialize();

            overworldEvent = new GetItemOE(item,
                String.Format("Holy crap! A weapon is just floating in space!\n\nYou found {0}!", item.Name), 
                "..But unfortunately, your inventory is full.",
                "..no more weapons here.");
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
            clearedText = "No more weapons here!";
        }
    }
}
