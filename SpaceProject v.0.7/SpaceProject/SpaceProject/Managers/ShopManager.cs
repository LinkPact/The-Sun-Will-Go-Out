using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ShopManager
    {
        //Shop update timer
        public const int PRESET_SHOPTIME = 120;             //Sets how often the shop is updated (in seconds)
        public static int ShopUpdateTime;

        public static void SetShopUpdateTime(int seconds)
        {
            ShopUpdateTime = seconds * 1000;
        }

        public void Update(GameTime gameTime)
        {
            if(GameStateManager.currentState != "MainMenuState" && GameStateManager.currentState != "PauseMenuState" &&
                GameStateManager.currentState != "OptionsMenuState" && GameStateManager.currentState != "LoadMenuState")
            ShopUpdateTime -= gameTime.ElapsedGameTime.Milliseconds;
        }

        public static void UpdateShops(List<GameObjectOverworld> objects)
        {
            if (ShopUpdateTime <= 0)
            {
                if (objects != null)
                {
                    foreach (GameObjectOverworld obj in objects)
                    {
                        if (obj is Station)
                        {
                            Station station = (Station)obj;
                            
                            if (station.ShopFilling != ShopFilling.none)
                                station.UpdateShopInventory();
                        }

                        else if (obj is Planet)
                        {
                            Planet planet = (Planet)obj;

                            if (planet.ShopFilling != ShopFilling.none)
                                planet.UpdateShopInventory();
                        }
                    }
                }

                SetShopUpdateTime(PRESET_SHOPTIME);
            }
        }
    }
}
