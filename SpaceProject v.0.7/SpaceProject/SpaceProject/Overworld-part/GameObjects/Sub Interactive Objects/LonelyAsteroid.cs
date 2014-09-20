using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class LonelyAsteroid : SubInteractiveObject
    {
        public LonelyAsteroid(Game1 Game, Sprite spriteSheet, MessageBox messageBox) :
            base(Game, spriteSheet, messageBox)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(724, 1075, 54, 55));
            position = new Vector2(110000, 102000);
            name = "Lonely Asteroid";

            base.Initialize();

            SetupLevel("A group of pirates attack!", "LonelyAsteroidEncounter", 50,
                new List<Item>(){ new DualLaserWeapon(Game, ItemVariety.low), 
                    new RegularShield(Game, ItemVariety.low)},
                "Good job!", "Too bad!");
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
    }
}
