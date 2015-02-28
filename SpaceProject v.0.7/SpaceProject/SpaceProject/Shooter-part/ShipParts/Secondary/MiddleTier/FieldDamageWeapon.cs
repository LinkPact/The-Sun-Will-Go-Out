﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class FieldDamageWeapon : PlayerWeapon
    {
        private float blastRadius = 200;
        private float fieldDamage = 75;

        public FieldDamageWeapon(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Emits a field disabling weapons, shields and stealth-technology";
        }

        private void Setup()
        {
            Name = "Field Damage";
            Kind = "Secondary";
            energyCostPerSecond = 1f;
            delay = 800;
            Weight = 400;
            ActivatedSoundID = SoundEffects.MuffledExplosion;
            displaySprite = Game.spriteSheetItemDisplay.GetSubSprite(new Rectangle(200, 100, 100, 100));

            bullet = new Mine(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 500;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            CircularAreaDamage areaExpl = new CircularAreaDamage(Game, AreaDamageType.player, player.Position, fieldDamage, blastRadius);
            areaExpl.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(areaExpl);
                        
            return true;
        }
    }
}
