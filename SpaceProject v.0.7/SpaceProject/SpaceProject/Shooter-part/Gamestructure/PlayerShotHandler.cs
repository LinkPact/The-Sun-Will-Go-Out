using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class PlayerShotHandler
    {
        #region decl
        private PlayerVerticalShooter player;
        private Game1 Game;
        private Sprite spriteSheet;
        public ShotHandlerHelper shotHandlerHelper;

        private Random random = new Random();

        public PlayerWeapon currentPrimaryWeapon;
        public List<PlayerWeapon> equippedWeapons;

        public List<int> ownedWeapons;

        private static bool primaryOn;
        private static bool secondaryOn;
        public static bool SecondaryOn { get { return secondaryOn; } }

        private bool switchMode;

        #endregion

        public PlayerShotHandler(PlayerVerticalShooter player, Game1 Game, Sprite spriteSheet)
        {
            this.player = player;
            this.Game = Game;
            this.spriteSheet = spriteSheet;

            shotHandlerHelper = new ShotHandlerHelper(Game);
        }
        
        public void Initialize()
        {
            shotHandlerHelper.Initialize();
            primaryOn = true;
            secondaryOn = true;

            switchMode = false;
        }
        
        public void Update(GameTime gameTime)
        {
            if (primaryOn)
            {
                ShipInventoryManager.currentPrimaryWeapon.Update(player, gameTime);
            }

            if (secondaryOn)
            {
                ShipInventoryManager.equippedSecondary.Update(player, gameTime);
            }

            if (switchMode)
            {
                if (ControlManager.CheckPress(RebindableKeys.Action3))
                {
                    if (primaryOn && secondaryOn)
                    {
                        secondaryOn = false;
                    }
                    else if (primaryOn && !secondaryOn)
                    {
                        primaryOn = false;
                        secondaryOn = true;
                    }
                    else if (!primaryOn && secondaryOn)
                    {
                        primaryOn = true;
                        secondaryOn = true;
                    }
                }
            }
            else
            {
                if (ControlManager.CheckPress(RebindableKeys.Action3))
                {
                    secondaryOn = !secondaryOn;
                }
            }

            if (ControlManager.CheckHold(RebindableKeys.Action1))
            {
                UseWeapon((PlayerWeapon)ShipInventoryManager.currentPrimaryWeapon, gameTime);
                UseWeapon((PlayerWeapon)ShipInventoryManager.equippedSecondary, gameTime);
            }

            shotHandlerHelper.Update();

            //Byter magi till nasta man kan, Q for vanster, E for hoger.
            if (ControlManager.CheckPress(RebindableKeys.Action2))
                shotHandlerHelper.ChangePrimary();

            //Lagrar de forandringar som gjorts i aktuellt magival till statsManagern.
            ShipInventoryManager.currentPrimaryWeapon = (PlayerWeapon)shotHandlerHelper.currentPrimary;

        }

        //Delegerar vidare vilket vapen som ska anropas baserat pa en int
        private void UseWeapon(PlayerWeapon weapon, GameTime gameTime)
        {
            if (primaryOn && weapon.Kind.Equals("Primary") && weapon.IsReadyToUse && weapon.EnergyCost < player.MP)
            {
                weapon.Use(player, gameTime);
                //weapon.PlaySound();
            }

            if (secondaryOn && weapon.Kind.Equals("Secondary") && weapon.IsReadyToUse && weapon.EnergyCost < player.MP)
            {
                weapon.Use(player, gameTime);
            }
        }    
 
        public void Draw(SpriteBatch spriteBatch)
        { 
            
        }

    }
}
