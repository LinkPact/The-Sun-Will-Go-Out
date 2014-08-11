﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class AstroDodger: Mission
    {
        private enum EventID
        {
            ShipFound = 0,
            Return = 1
        }

        private DestroyedShip destroyedShip;

        public AstroDodger(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            destroyedShip = new DestroyedShip(this.Game, spriteSheet);
            destroyedShip.Initialize();

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destroyedShip,
                new EventTextCapsule(
                    GetEvent((int)EventID.ShipFound),
                    null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destroyedShip,
                "AstroDodger",
                LevelStartCondition.Immediately,
                new EventTextCapsule(
                    GetEvent((int)EventID.Return),
                    new EventText("You decide it's best to abandon the ship and return to Fotrun Station. No reward is worth getting crushed by asteroids."),
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Fotrun Station I")));
        }

        public override void StartMission()
        {
            progress = 0;
            ObjectiveIndex = 0;

            destroyedShip.IsUsed = true;
            Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
        }

        public override void OnLoad()
        {
            if (!Game.stateManager.overworldState.ContainsOverworldObject(destroyedShip) &&
                (MissionState == StateOfMission.Active || MissionState == StateOfMission.Completed ||
                MissionState == StateOfMission.Failed))
            {
                destroyedShip.IsUsed = true;
                Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }
    }
}