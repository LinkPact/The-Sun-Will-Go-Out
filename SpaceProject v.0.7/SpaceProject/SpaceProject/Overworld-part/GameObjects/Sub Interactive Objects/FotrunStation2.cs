using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class FotrunStation2 : SubInteractiveObject
    {
        public FotrunStation2(Game1 Game, Sprite spriteSheet, MessageBox messageBox) :
            base(Game, spriteSheet, messageBox)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            position = new Vector2(95300, 97000);

            base.Initialize();

            text.Add("Do you want to refuel? It costs 100 rupees!");
            text.Add("Thank you! Your fuel has been refilled!");
            text.Add("Have a pleasent day!");
            text.Add("I'm afraid you do not have enough money, sir.");

            options.Add("Yes");
            options.Add("No");
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
            messageBox.DisplaySelectionMenu(text[0], options, new List<System.Action>()
            {
                delegate 
                {
                    if (StatsManager.Rupees >= 100)
                    {
                        messageBox.DisplayMessage(text[1], 50);
                        StatsManager.Rupees -= 100;
                        StatsManager.Fuel = StatsManager.MaxFuel;
                    }

                    else
                    {
                        messageBox.DisplayMessage(text[3], 50);
                    }
                }, 

                delegate 
                {
                    messageBox.DisplayMessage(text[2], 50);
                }
            });
        }
    }
}
