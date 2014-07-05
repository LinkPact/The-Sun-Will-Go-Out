﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
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
            if (ControlManager.PreviousKeyboardState.IsKeyUp(ControlManager.KeyboardAction))
            {
                beamOn = false;
            }

            GameObjectVertical beamTarget = LocateTarget(player.Position);

            if (!beamOn) InitBeam(player.Position);
            
            if (!player.IsKilled) BeamUpdate(player.Position, beamTarget, gameTime);

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
        private void BeamUpdate(Vector2 playerPosition, GameObjectVertical beamTarget, GameTime gameTime)
        {
            if (beamTarget != null)
            {
                beamTarget.HP -= Damage;
                beam.Update_(gameTime, playerPosition.X, playerPosition.Y, beamTarget.PositionY);
            }
            else
                beam.Update_(gameTime, playerPosition.X, playerPosition.Y, 0);
        }
        private void InitBeam(Vector2 playerPosition)
        {
            beamOn = true;

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