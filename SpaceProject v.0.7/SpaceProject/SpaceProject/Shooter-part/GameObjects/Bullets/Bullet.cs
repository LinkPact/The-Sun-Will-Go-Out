using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public abstract class Bullet : CombatGameObject
    {
        protected Boolean collidesOtherBullets;
        public Boolean CollidesOtherBullets { get { return collidesOtherBullets; } private set { } }

        protected Bullet(Game1 Game, Sprite spriteSheet)
            : base (Game, spriteSheet)
        { 
            
        }

        public override void InflictDamage(GameObjectVertical obj)
        {
            Game.stateManager.shooterState.backgroundObjects.Add(
               ExplosionGenerator.GenerateBulletExplosion(Game, spriteSheet, this));

            //Game.stateManager.shooterState.backgroundObjects.Add(
              //   ExplosionGenerator.GenerateBombExplosion(Game, spriteSheet, this));
        }

    }
}
