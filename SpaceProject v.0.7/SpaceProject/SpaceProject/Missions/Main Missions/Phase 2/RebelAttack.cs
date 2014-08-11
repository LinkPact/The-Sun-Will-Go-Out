﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class RebelAttack : Mission
    {
        public RebelAttack(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Rebel Station 2"),
                new EventTextCapsule(GetEvent(0), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Rebel Station 2"), "AttackOnRebelStation", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent(1), null, EventTextCanvas.BaseState)));

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