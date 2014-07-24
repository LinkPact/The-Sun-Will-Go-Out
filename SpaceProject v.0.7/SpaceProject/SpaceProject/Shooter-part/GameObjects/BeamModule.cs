using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class BeamModule
    {
        private Game1 game;
        private Sprite spriteSheet;

        private Boolean belongsToAlly;
        private Boolean targetingUpwards;

        private Beam beam;
        public PlayerBullet GetBullet() 
        {
            PlayerBullet dummyBullet = new PlayerBullet(game, spriteSheet);
            dummyBullet.Damage = 3.0f;

            return dummyBullet; 
        }
        
        private GameObjectVertical target;

        List<String> viableTargetTypes = new List<String>();

        public Boolean HasTarget() { return target != null; }
        public GameObjectVertical GetTarget() { return target; }

        public BeamModule(Game1 game, Sprite spriteSheet, Boolean belongsToAlly, Boolean targetingUpwards)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;

            this.belongsToAlly = belongsToAlly;
            this.targetingUpwards = targetingUpwards;

            if (belongsToAlly)
            {
                viableTargetTypes.Add("enemy");
            }
            else
            {
                viableTargetTypes.Add("player");
                viableTargetTypes.Add("ally");
            }
        }

        public void Activate(GameObjectVertical shooter, GameTime gameTime)
        {
            if (beam == null)
            {
                InitBeam(shooter.CenterPoint);
            }
            else if (beam.IsKilled)
            {
                InitBeam(shooter.CenterPoint);
            }

            target = LocateTarget(shooter.Position);

            BeamUpdate(shooter.Position, target, gameTime);
        }

        //
        private GameObjectVertical LocateTarget(Vector2 shooterPos)
        {
            GameObjectVertical newTarget = null;
            List<GameObjectVertical> sameXList = FindTargetsWithSameX(shooterPos);

            if (sameXList.Count > 0)
            {
                newTarget = sameXList[0];
                foreach (GameObjectVertical obj in sameXList)
                {
                    if (shooterPos.Y - obj.PositionY < shooterPos.Y - newTarget.PositionY)
                    {
                        newTarget = obj;
                    }
                }
            }

            if (newTarget != null) return newTarget;
            else return null;
        }

        private List<GameObjectVertical> FindTargetsWithSameX(Vector2 shooterPos)
        {
            List<GameObjectVertical> sameXTargets = new List<GameObjectVertical>();

            foreach (GameObjectVertical obj in game.stateManager.shooterState.gameObjects)
            {
                if (viableTargetTypes.Contains(obj.ObjectClass))
                {
                    if (IntervalInsideInterval(obj.PositionX - obj.BoundingWidth / 2, obj.PositionX + obj.BoundingWidth / 2,
                                shooterPos.X - 2, shooterPos.X + 2)
                                && CheckYRelation(shooterPos, obj.Position, targetingUpwards))
                    {
                        sameXTargets.Add(obj);
                    }
                }
            }
            return sameXTargets;
        }

        private Boolean CheckYRelation(Vector2 shooterPos, Vector2 otherPos, Boolean targetingForward)
        {
            if (targetingForward)
            {
                return shooterPos.Y > otherPos.Y;
            }
            else
            {
                return shooterPos.Y < otherPos.Y;
            }
        }
        //

        private void BeamUpdate(Vector2 shooterPosition, GameObjectVertical beamTarget, GameTime gameTime)
        {
            if (beamTarget != null)
            {
                //beamTarget.HP -= Damage;
                beam.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, beamTarget.PositionY);
            }
            else
            {
                if (targetingUpwards)
                {
                    beam.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, 0);
                }
                else
                {
                    beam.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, game.Window.ClientBounds.Height);
                }
            }
        }

        private void InitBeam(Vector2 shooterPosition)
        {
            beam = new Beam(game, spriteSheet, targetingUpwards);
            beam.PositionX = shooterPosition.X;
            beam.PositionY = shooterPosition.Y;
            beam.Initialize();

            game.stateManager.shooterState.gameObjects.Add(beam);
        }

        private bool IntervalInsideInterval(float start1, float end1, float start2, float end2)
        {
            if (end1 < start2 || end2 < start1) return false;

            return true;
        }
    }
}
