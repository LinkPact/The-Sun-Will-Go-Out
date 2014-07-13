﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class Bullet : CombatGameObject
    {
        protected Boolean collidesOtherBullets;
        public Boolean CollidesOtherBullets { get { return collidesOtherBullets; } private set { } }

        public float duration;
        
        protected Bullet(Game1 Game, Sprite spriteSheet)
            : base (Game, spriteSheet)
        { 
            
        }

        //public override void Update(GameTime gameTime)
        //{
        //    base.Update(gameTime);
        //
        //    duration -= gameTime.ElapsedGameTime.Milliseconds;
        //
        //    if (duration <= 0)
        //        IsKilled = true;
        //
        //}

        public override void InflictDamage(GameObjectVertical obj)
        {
            Game.stateManager.shooterState.backgroundObjects.Add(
               ExplosionGenerator.GenerateBulletExplosion(Game, spriteSheet, this));
        }

        public void SetSpreadSpeed(Random random)
        {
            Speed += ((float)random.NextDouble()) * 1.0f * Speed - 0.5f * Speed;
        }
    }
}
