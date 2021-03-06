﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject_Linux
{
    public abstract class Bullet : CombatGameObject
    {
        protected Boolean collidesOtherBullets;
        public Boolean CollidesOtherBullets { get { return collidesOtherBullets; } private set { } }

        public Bullet(Game1 Game, Sprite spriteSheet)
            : base (Game, spriteSheet)
        {
            DrawLayer = 0.4f;
            deathSoundID = SoundEffects.MuffledExplosion;
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            Game.stateManager.shooterState.backgroundObjects.Add(
               ExplosionGenerator.GenerateBulletExplosion(Game, spriteSheet, this));
        }

        public void SetSpreadSpeed(Random random)
        {
            Speed += ((float)random.NextDouble()) * 1.0f * Speed - 0.5f * Speed;
        }

        public void SetDirectionAgainstTarget(GameObjectVertical shooter, GameObjectVertical target)
        {
            Direction = MathFunctions.ScaleDirection(target.Position - shooter.Position);
        }

        public override void OnKilled()
        { }
    }
}
