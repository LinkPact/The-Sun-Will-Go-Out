﻿using System;
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

        private SoundEffects energyLockSound = SoundEffects.EnergyLock;
        private float remainingEnergyLockTime = 0;
        private readonly float ENERGY_LOCK_TIME = 1000;
        private Boolean IsEnergyLocked
        {
            get { return remainingEnergyLockTime > 0; }
        }

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
        }
        
        public void Update(GameTime gameTime)
        {
            if (IsEnergyLocked)
            {
                remainingEnergyLockTime -= gameTime.ElapsedGameTime.Milliseconds;
            }

            if (primaryOn)
            {
                ShipInventoryManager.currentPrimaryWeapon.Update(player, gameTime);
            }

            if (secondaryOn)
            {
                ShipInventoryManager.equippedSecondary.Update(player, gameTime);
            }

            if (ControlManager.CheckHold(RebindableKeys.Action1))
            {
                UseWeapon((PlayerWeapon)ShipInventoryManager.currentPrimaryWeapon, gameTime);
                UseWeapon((PlayerWeapon)ShipInventoryManager.equippedSecondary, gameTime);
            }

            shotHandlerHelper.Update();

            if (ControlManager.CheckPress(RebindableKeys.Action2))
            {
                shotHandlerHelper.ChangePrimary();
            }

            ShipInventoryManager.currentPrimaryWeapon = (PlayerWeapon)shotHandlerHelper.currentPrimary;
        }

        private void UseWeapon(PlayerWeapon weapon, GameTime gameTime)
        {
            if (weapon.IsReadyToUse && !IsEnergyLocked)
            {
                if (weapon.EnergyCost < player.MP)
                {
                    weapon.Use(player, gameTime);
                }
                else
                {
                    remainingEnergyLockTime = ENERGY_LOCK_TIME;
                    Game.soundEffectsManager.PlaySoundEffect(energyLockSound);
                }            
            }

            //if (primaryOn && weapon.Kind.Equals("Primary") && weapon.IsReadyToUse && !IsEnergyLocked)
            //{
            //    if (weapon.EnergyCost < player.MP)
            //    {
            //        weapon.Use(player, gameTime);
            //    }
            //    else
            //    {
            //        remainingEnergyLockTime = ENERGY_LOCK_TIME;
            //        Game.soundEffectsManager.PlaySoundEffect(SoundEffects.EnergyLock);
            //    }
            //}
            //
            //if (secondaryOn && weapon.Kind.Equals("Secondary") && weapon.IsReadyToUse && weapon.EnergyCost < player.MP)
            //{
            //    weapon.Use(player, gameTime);
            //}
        }

        private void ExecuteWeapon(PlayerWeapon weapon, GameTime gameTime)
        {
            if (weapon.EnergyCost < player.MP)
            {
                weapon.Use(player, gameTime);
            }
            else
            {
                remainingEnergyLockTime = ENERGY_LOCK_TIME;
                Game.soundEffectsManager.PlaySoundEffect(SoundEffects.EnergyLock);
            }
        }
    }
}
