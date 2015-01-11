using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class DrillBeamWeapon : PlayerWeapon
    {
        #region decl
        
        Boolean beamOn;
        DrillBeam beam;

        float miningSpeedCopper, miningSpeedGold, miningSpeedTitanium;

        #endregion
        public DrillBeamWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public DrillBeamWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots a solid beam which is strong against meteors and is able to extract minerals from mineral rocks";
        }

        private void Setup()
        {
            Name = "Drill Beam";
            Kind = "Primary";
            energyCostPerSecond = 0.15f;
            delay = 10;
            Weight = 100;
            ActivatedSoundID = SoundEffects.SmallLaser;

            damage = 0.5f;
            miningSpeedCopper = 0.2f;
            miningSpeedGold = 0.1f;
            miningSpeedTitanium = 0.05f;

            Value = 700;
        }

        public override void Initialize()
        {
            beamOn = false;
        }
        
        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            if (ControlManager.PreviousKeyboardState.IsKeyUp(ControlManager.KeyboardAction))
            {
                beamOn = false;
            }

            GameObjectVertical beamTarget = LocateTarget(player.Position);

            if (!beamOn) InitBeam(player.Position);
            
            BeamUpdate(player, beamTarget, gameTime);
            return true;

        }

        private GameObjectVertical LocateTarget(Vector2 playerPosition)
        {   
            GameObjectVertical target = null;
            
            List<GameObjectVertical> sameXList = new List<GameObjectVertical>();

            foreach (GameObjectVertical obj in Game.stateManager.shooterState.gameObjects)
            {
                if (obj.ObjectClass == "enemy")
                {
                    if (IntervalInsideInterval(obj.PositionX - obj.BoundingWidth/2, obj.PositionX + obj.BoundingWidth/2,
                        playerPosition.X - 2, playerPosition.X + 2))
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
                    if (playerPosition.Y - obj.PositionY < playerPosition.Y - target.PositionY 
                        && playerPosition.Y - obj.PositionY >= 0)
                    {
                        target = obj;
                    }
                }
            }

            if (target != null) return target;
            else return null;
        }

        private void BeamUpdate(GameObjectVertical player, GameObjectVertical beamTarget, GameTime gameTime)
        {
            if (beamTarget != null)
            {
                if (beamTarget.ObjectSubClass == "meteorite")
                    beamTarget.HP -= Damage * 20;
                else if (beamTarget.ObjectSubClass == "resource")
                {
                    ExtractResources((PlayerVerticalShooter)player, beamTarget);
                }
                else
                    beamTarget.HP -= Damage;

                beam.Update_(gameTime, player.PositionX, player.PositionY, beamTarget.PositionY);
            }
            else
                beam.Update_(gameTime, player.PositionX, player.PositionY, 0);
        }

        private void InitBeam(Vector2 playerPosition)
        {
            beamOn = true;

            beam = new DrillBeam(Game, spriteSheet);
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

        private void ExtractResources(PlayerVerticalShooter player, GameObjectVertical beamTarget)
        {
            float miningDamageFactor = 20;

            if (beamTarget.ObjectName == "ResourceMeteoriteCopper")
            {
                player.AmassedCopper += miningSpeedCopper;
                beamTarget.HP -= miningSpeedCopper * miningDamageFactor;
            }

            else if (beamTarget.ObjectName == "ResourceMeteoriteGold")
            {
                player.AmassedGold += miningSpeedGold;
                beamTarget.HP -= miningSpeedGold * miningDamageFactor;
            }

            else if (beamTarget.ObjectName == "ResourceMeteoriteTitanium")
            {
                player.AmassedTitanium += miningSpeedTitanium;
                beamTarget.HP -= miningSpeedTitanium * miningDamageFactor;
            }
        }
    }
}
