using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public static class CollisionHandlingVerticalShooter
    {
        public const float TEMP_INVINCIBILITY = 50;

        public static void GameObjectsCollision(GameObjectVertical gameObject1, GameObjectVertical gameObject2)
        {
            if (GlobalMathFunctions.AreObjectsOfTypes<PlayerBullet, EnemyShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is PlayerBullet)
                    CollideBulletEnemy((PlayerBullet)gameObject1, (EnemyShip)gameObject2);
                else
                    CollideBulletEnemy((PlayerBullet)gameObject2, (EnemyShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<EnemyBullet, AlliedShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is EnemyBullet)
                    CollideBulletAlly((EnemyBullet)gameObject1, (AlliedShip)gameObject2);
                else
                    CollideBulletAlly((EnemyBullet)gameObject2, (AlliedShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<PlayerVerticalShooter, EnemyShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is PlayerVerticalShooter)
                    CollidePlayerEnemy((PlayerVerticalShooter)gameObject1, (EnemyShip)gameObject2);
                else
                    CollidePlayerEnemy((PlayerVerticalShooter)gameObject2, (EnemyShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<PlayerVerticalShooter, EnemyBullet>(gameObject1, gameObject2))
            {
                if (gameObject1 is PlayerVerticalShooter)
                    CollidePlayerBullet((PlayerVerticalShooter)gameObject1, (EnemyBullet)gameObject2);
                else
                    CollidePlayerBullet((PlayerVerticalShooter)gameObject2, (EnemyBullet)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<EnemyShip, AlliedShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is EnemyShip)
                    CollideEnemyAlly((EnemyShip)gameObject1, (AlliedShip)gameObject2);
                else
                    CollideEnemyAlly((EnemyShip)gameObject2, (AlliedShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<AreaDamage, PlayerVerticalShooter>(gameObject1, gameObject2))
            {
                if (gameObject1 is AreaDamage)
                    CollideAreaDamage((AreaDamage)gameObject1, (PlayerVerticalShooter)gameObject2);
                else
                    CollideAreaDamage((AreaDamage)gameObject2, (PlayerVerticalShooter)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<AreaDamage, AlliedShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is AreaDamage)
                    CollideAreaDamage((AreaDamage)gameObject1, (AlliedShip)gameObject2);
                else
                    CollideAreaDamage((AreaDamage)gameObject2, (AlliedShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<AreaDamage, EnemyShip>(gameObject1, gameObject2))
            {
                if (gameObject1 is AreaDamage)
                    CollideAreaDamage((AreaDamage)gameObject1, (EnemyShip)gameObject2);
                else
                    CollideAreaDamage((AreaDamage)gameObject2, (EnemyShip)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<AreaShieldCollision, PlayerBullet>(gameObject1, gameObject2))
            {
                if (gameObject1 is AreaShieldCollision)
                    CollideAreaCollision((AreaShieldCollision)gameObject1, (PlayerBullet)gameObject2);
                else
                    CollideAreaCollision((AreaShieldCollision)gameObject2, (PlayerBullet)gameObject1);
            }

            else if (GlobalMathFunctions.AreObjectsOfTypes<Bullet, Bullet>(gameObject1, gameObject2))
            {
                CollideBulletBullet((Bullet)gameObject1, (Bullet)gameObject2);
            }

            // Neutral - friend/enemy/bullet/enemybullet
            // Is this ever used?

            else if ((gameObject1 is AlliedShip || gameObject1 is EnemyShip
                || gameObject1 is PlayerBullet || gameObject1 is EnemyBullet)
                && gameObject2.ObjectClass == "neutral")
            {
                gameObject1.HP -= gameObject2.Damage;
                gameObject2.HP -= gameObject1.Damage;
                gameObject1.Direction = -1 * new Vector2(gameObject2.PositionX - gameObject1.PositionX, gameObject2.PositionY - gameObject1.PositionY);
                gameObject1.Direction = GlobalMathFunctions.ScaleDirection(gameObject1.Direction);

                gameObject1.DisableFollowObject = 500;

                if (gameObject1.HP <= 0)
                    gameObject1.IsKilled = true;

                if (gameObject2.HP <= 0)
                    gameObject2.IsKilled = true;
            }

            else if (gameObject1.ObjectClass == "neutral" &&
                (gameObject2 is AlliedShip || gameObject2 is EnemyShip ||
                gameObject2 is PlayerBullet || gameObject2 is EnemyBullet))
            {
                gameObject1.HP -= gameObject2.Damage;
                gameObject2.HP -= gameObject1.Damage;
                gameObject1.Direction *= -1;
                gameObject2.Direction *= -1;

                gameObject2.DisableFollowObject = 500;

                if (gameObject1.HP <= 0)
                    gameObject1.IsKilled = true;

                if (gameObject2.HP <= 0)
                    gameObject2.IsKilled = true;
            }
        }

        private static void CollideBulletEnemy(PlayerBullet bullet, EnemyShip enemy)
        {
            bullet.InflictDamage(enemy);
            enemy.InflictDamage(bullet);
        }

        private static void CollideBulletAlly(EnemyBullet bullet, AlliedShip ally)
        {
            bullet.InflictDamage(ally);
            ally.InflictDamage(bullet);
        }

        private static void CollidePlayerEnemy(PlayerVerticalShooter player, EnemyShip enemy)
        {
            player.InflictDamage(enemy);
            enemy.InflictDamage(player);
        }

        private static void CollidePlayerBullet(PlayerVerticalShooter player, EnemyBullet bullet)
        {
            player.InflictDamage(bullet);
            bullet.InflictDamage(player);
        }

        private static void CollideEnemyAlly(EnemyShip enemy, AlliedShip ally)
        {
            enemy.InflictDamage(ally);
            ally.InflictDamage(enemy);
        }

        private static void CollideBulletBullet(Bullet bullet1, Bullet bullet2)
        {
            if (bullet1.CollidesOtherBullets || bullet2.CollidesOtherBullets)
            {
                if ((bullet1 is PlayerBullet && bullet2 is EnemyBullet)
                    || (bullet2 is PlayerBullet && bullet1 is EnemyBullet))
                {
                    bullet1.InflictDamage(bullet2);
                    bullet2.InflictDamage(bullet1);
                }
            }
        }

        private static void CollideAreaDamage(AreaDamage area, CombatGameObject obj)
        {
            if (area.Type == AreaDamageType.enemy && (obj is PlayerVerticalShooter || obj is AlliedShip))
            {
                obj.InflictDamage(area);
            }
            else if (area.Type == AreaDamageType.player && obj is EnemyShip)
            {
                obj.InflictDamage(area);
            }
        }

        private static void CollideAreaCollision(AreaShieldCollision area, PlayerBullet obj)
        {
            if (area.SourceObject.ShieldCanTakeHit(obj.Damage))
            {
                area.InflictDamage(obj);
                obj.InflictDamage(area);
            }
        }
    }
}
