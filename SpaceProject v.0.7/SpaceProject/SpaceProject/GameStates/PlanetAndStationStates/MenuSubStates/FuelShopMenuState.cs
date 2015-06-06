using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class FuelShopMenuState: MenuState
    {
        private int totalCost;
        private int fuelCost;
        private float boughtFuel;

        float counter = 0;
        float counter2 = 0;

        private Bar fuelBar;
        private List<string> fuelStrings;
        private List<string> helpStrings;

        public FuelShopMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
        }

        public override void Initialize()
        {
            fuelStrings = new List<string>();
            helpStrings = new List<string>();
            helpStrings.Add("'Up'/'Down' to buy fuel");
            helpStrings.Add("'Enter' to confirm");
            helpStrings.Add("'Escape' to cancel");

            fuelCost = 1;
            fuelBar = new Bar(Game, SpriteSheet, Color.Olive, false, new Rectangle(644, 1, 64, 256), new Rectangle(709, 2, 64, 256));
            fuelBar.Initialize();
        }

        public override void OnEnter()
        {
            confirmString = "";

            BaseStateManager.ActiveButton = null;
        }

        public override void Update(GameTime gameTime)
        {
            counter--;

            if (ControlManager.CheckHold(RebindableKeys.Up) &&
                counter <= 0)
            {
                if(counter2 < 7)
                    counter2+= 0.75f;

                if (StatsManager.Fuel + boughtFuel < StatsManager.MaxFuel)
                {
                    boughtFuel++;
                    totalCost += fuelCost;
                    counter = 7 - counter2;
                }
            }

            else if (ControlManager.CheckHold(RebindableKeys.Down) &&
                counter <= 0)
            {
                if (counter2 < 7)
                    counter2 += 0.75f;

                if (boughtFuel > 0)
                {
                    boughtFuel--;
                    totalCost -= fuelCost;
                    counter = 7 - counter2;
                }
            }

            else if (!ControlManager.CheckHold(RebindableKeys.Up) &&
                !ControlManager.CheckHold(RebindableKeys.Down))
            {
                counter2 = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Action2) || ControlManager.CheckKeyPress(Keys.Escape))
            {
                boughtFuel = 0;
                totalCost = 0;
                BaseStateManager.ChangeMenuSubState("Overview");                    
            }

            fuelStrings.Clear();
            fuelStrings.Add("Current Fuel: " + (int)(StatsManager.Fuel + boughtFuel) + " / "
                    + StatsManager.MaxFuel + " l");
            fuelStrings.Add("Fuel Bought: " + boughtFuel + "l");
            fuelStrings.Add("Cost: " + totalCost);
            fuelStrings.Add("Rupees: " + StatsManager.Rupees);

            fuelBar.Update(gameTime, StatsManager.Fuel + boughtFuel, StatsManager.MaxFuel,
                new Vector2(Game.Window.ClientBounds.Width * 1 / 3 + 20, Game.Window.ClientBounds.Height / 2 + 24));
        }

        public override void ButtonActions()
        {
            if (totalCost <= (int)StatsManager.Rupees)
            {
                if (boughtFuel > 0)
                {
                    PopupHandler.DisplayMessage("Transaction complete! \nYou bought " + boughtFuel + " l for " + totalCost + " rupees.");

                    StatsManager.Fuel += boughtFuel;
                    StatsManager.Rupees -= totalCost;
                }

                totalCost = 0;
                boughtFuel = 0;
                BaseStateManager.ChangeMenuSubState("Overview");
            }

            else
            {
                PopupHandler.DisplayMessage("You do not have enough rupees!");
            }
        }

        public override void CursorActions()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < fuelStrings.Count; i++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(16), fuelStrings[i],
                    new Vector2(Game.Window.ClientBounds.Width * 2 / 3,
                                Game.Window.ClientBounds.Height * 2 / 4 + 60 + (i * 20)) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor,
                    0.0f, new Vector2(Game.fontManager.GetFont(16).MeasureString(fuelStrings[i]).X / 2, 0),
                    1f, SpriteEffects.None, 0.5f);
            }

            for (int i = 0; i < helpStrings.Count; i++)
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(16), helpStrings[i],
                    new Vector2(Game.Window.ClientBounds.Width * 2 / 3,
                                Game.Window.ClientBounds.Height * 3 / 4 + 20 + (i * 20)) + Game.fontManager.FontOffset,
                    Game.fontManager.FontColor,
                    0.0f, new Vector2(Game.fontManager.GetFont(16).MeasureString(helpStrings[i]).X / 2, 0),
                    1f, SpriteEffects.None, 0.5f);
            }

            fuelBar.Draw(spriteBatch);
        }
    }
}
