using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class FortrunStation2 : SubInteractiveObject
    {
        public FortrunStation2(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            position = new Vector2(95300, 97000);
            name = "Fortrun Station II";

            base.Initialize();

            SetupFuelShop("Do you want to refuel? It costs 100 rupees!",
                "Thanks anyway, sir!",
                "Thank you! Your fuel has been refilled!",
                "I'm afraid you do not have enough money, sir.",
                "Your tank is already full! How silly of you, sir!", 100);
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
