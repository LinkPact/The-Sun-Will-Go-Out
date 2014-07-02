﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class DeathByMeteorMission : Mission
    {

        public DeathByMeteorMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            FlameShotWeapon flameShot = new FlameShotWeapon(Game, ItemVariety.high);
            RewardItems.Add(flameShot);

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("Peye"), "DeathByMeteor", LevelStartCondition.TextCleared,
                new EventTextCapsule(new List<String>{EventArray[0, 0], EventArray[1, 0]}, 
                    null, EventTextCanvas.BaseState)));

            RestartAfterFail();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();
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
