using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Handles the switching of weapons and so on.

    public class ShotHandlerHelper
    {

        private Game1 Game;
        public Item currentPrimary;
        
        public List<PlayerWeapon> equippedPrimaryWeapons;

        public ShotHandlerHelper(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        {
            equippedPrimaryWeapons = ShipInventoryManager.equippedPrimaryWeapons;
            currentPrimary = equippedPrimaryWeapons[0];
        }

        public void Update()
        {
            int pressedNumber = CheckKeyboardInput();
        }

        private int CheckKeyboardInput()
        {
            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D1))
                return 1;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D2))
                return 2;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D3))
                return 3;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D4))
                return 4;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D5))
                return 5;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D6))
                return 6;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D7))
                return 7;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D8))
                return 8;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D9))
                return 9;

            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.D0))
                return 10;

            return -1;
        }

        public void ChangePrimary()
        {
            int initialPos = equippedPrimaryWeapons.IndexOf(ShipInventoryManager.currentPrimaryWeapon);

            int currentPos = initialPos + 1;

            if (currentPos >= equippedPrimaryWeapons.Count)
            {
                currentPos = 0;
            }

            var targetWeapon = equippedPrimaryWeapons[currentPos];
            if (!(targetWeapon is EmptyWeapon))
            {
                currentPrimary = equippedPrimaryWeapons[currentPos];
            }

        }
    }
}
