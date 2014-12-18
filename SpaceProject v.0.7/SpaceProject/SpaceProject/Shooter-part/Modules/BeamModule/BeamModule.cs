using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /**
     * The purpose with beam modules is to provide an easy way to access full beam functionality
     * in both playerweapons and in enemies.
     * 
     * It is extended by HostileBeamModule and FriendlyBeamModule which adds different targets to the
     * list "viableTargetTypes" found in this class. They also specify the "targetingUpwards"-boolean
     * which is used to decide if the beam is targeting, drawn and damaging upwards or downwards.
     * 
     * "Activate" is the main function, and the only function reached from outside. 
     * This is called either from a weapons activation logic (called when it is time for the PlayerWeapon to fire)
     * or from a ships ShootingPattern (which is called when the ship is to fire).
     * 
     * When this is called following happens:
     * (1) It checks if there is a beam drawing present. If not, create a new one through calling "InitBeam".
     * (2) Seach for a target using "LocateTarget".
     * (3) If target is present, inflict damage.
     * (4) Update the size and position of the BeamDrawing-object.
     */
    public abstract class BeamModule
    {
        #region variables
        private Game1 game;
        private Sprite spriteSheet;

        private Boolean targetingUpwards;

        private float damage;

        private BeamDrawing beamDrawing;
        
        private GameObjectVertical target;

        protected List<String> viableTargetTypes = new List<String>();

        protected Color color;
        #endregion

        public BeamModule(Game1 game, Sprite spriteSheet, Boolean targetingUpwards, float damage)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;

            this.targetingUpwards = targetingUpwards;
            this.damage = damage;
        }

        public void Activate(Vector2 shooterPosition, GameTime gameTime)
        {
            if (beamDrawing == null)
            {
                InitBeam(shooterPosition);
            }
            else if (beamDrawing.IsKilled)
            {
                InitBeam(shooterPosition);
            }

            target = LocateTarget(shooterPosition);

            if (target != null)
                InflictDamage(target);

            UpdateBeamDrawing(shooterPosition, target, gameTime);
        }

        public Boolean HasTargetInLineOfSight(Vector2 shooterPosition)
        {
            return (LocateTarget(shooterPosition) != null);
        }

        //------------------------

        /**
         * The functions LocateTarget, FindTargetsWithSameX and CheckYRelation checks if there is a 
         * suitable target present. A suitable target has the same x-position as the shooting object, and a y-position
         * that either is higher or lower depending on the variable "targetingUpwards"
         */
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

        private void UpdateBeamDrawing(Vector2 shooterPosition, GameObjectVertical beamTarget, GameTime gameTime)
        {
            if (beamTarget != null)
            {
                beamDrawing.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, beamTarget.PositionY);
            }
            else
            {
                if (targetingUpwards)
                {
                    beamDrawing.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, 0);
                }
                else
                {
                    beamDrawing.UpdateLocation(gameTime, shooterPosition.X, shooterPosition.Y, game.Window.ClientBounds.Height);
                }
            }
        }

        private void InflictDamage(GameObjectVertical obj)
        {
            Bullet dummyBullet = new YellowBullet(game, spriteSheet);
            dummyBullet.Damage = damage;
            ((CombatGameObject)obj).InflictDamage(dummyBullet);
        }

        /**
         * Creates a new beam object and adds it to background objects.
         */
        private void InitBeam(Vector2 shooterPosition)
        {
            beamDrawing = new BeamDrawing(game, spriteSheet, targetingUpwards);
            beamDrawing.PositionX = shooterPosition.X;
            beamDrawing.PositionY = shooterPosition.Y;
            beamDrawing.Initialize();

            if (color != null)
                beamDrawing.Color = color;

            game.stateManager.shooterState.backgroundObjects.Add(beamDrawing);
        }

        private bool IntervalInsideInterval(float start1, float end1, float start2, float end2)
        {
            if (end1 < start2 || end2 < start1) return false;

            return true;
        }
    }
}
