using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    /**
     * Advanced weapon class containing all logic for beam weapons.
     * 
     * TODO: Generalize through moving out the beam logic to a separate class.
     */
    public class BeamWeapon : PlayerWeapon
    {
        #region decl
        Boolean beamOn;
        Beam beam;
        #endregion

        public BeamWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public BeamWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a single solid beam at great range";
        }

        private void Setup()
        {
            Name = "Beam";
            Kind = "Primary";
            energyCostPerSecond = 0.15f;
            delay = 10;
            Weight = 200;

            damage = 6.0f;

            Value = 700;
        }

        public override void Initialize()
        {
            beamOn = false;
        }
        
        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            if (StatsManager.gameMode == GameMode.develop)
                Game.Window.Title = "Beam On: " + beamOn.ToString();

            if (beam == null)
            {
                InitBeam(player.CenterPoint);
            }
            else if (beam.IsKilled)
            {
                InitBeam(player.CenterPoint);
            }

            GameObjectVertical beamTarget = LocateTarget(player.Position);

            BeamUpdate(player.Position, beamTarget, gameTime);

            return true;
        }

        private GameObjectVertical LocateTarget(Vector2 playerPosition)
        {
            Boolean targetingForward = true;

            GameObjectVertical target = null;
            List<GameObjectVertical> sameXList = new List<GameObjectVertical>();

            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj.ObjectClass == "enemy")
                {
                    if (IntervalInsideInterval(obj.PositionX - obj.BoundingWidth/2, obj.PositionX + obj.BoundingWidth/2,
                                playerPosition.X - 2, playerPosition.X + 2)
                                && CheckYRelation(playerPosition, obj.Position, targetingForward))
                    {
                        sameXList.Add(obj);
                    }
                }
            }

            if (sameXList.Count > 0)
            {
                target = sameXList[0];
                foreach (GameObjectVertical obj in sameXList)
                {
                    if (playerPosition.Y - obj.PositionY < playerPosition.Y - target.PositionY)
                    {
                        target = obj;
                    }
                }
            }

            if (target != null) return target;
            else return null;
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

        private void BeamUpdate(Vector2 playerPosition, GameObjectVertical beamTarget, GameTime gameTime)
        {
            if (beamTarget != null)
            {
                beamTarget.HP -= Damage;
                beam.UpdateLocation(gameTime, playerPosition.X, playerPosition.Y, beamTarget.PositionY);
            }
            else
                beam.UpdateLocation(gameTime, playerPosition.X, playerPosition.Y, 0);
        }
        
        private void InitBeam(Vector2 playerPosition)
        {
            beam = new Beam(Game, spriteSheet);
            beam.PositionX = playerPosition.X;
            beam.PositionY = playerPosition.Y;
            beam.Initialize();

            Game.stateManager.shooterState.gameObjects.Add(beam);
        }
        
        private bool IntervalInsideInterval(float start1, float end1, float start2, float end2)
        {
            if (end1 < start2 || end2 < start1) return false;

            return true;
        }

        public override void PlaySound()
        {
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.Test4, 0f);
        }
    }
}
