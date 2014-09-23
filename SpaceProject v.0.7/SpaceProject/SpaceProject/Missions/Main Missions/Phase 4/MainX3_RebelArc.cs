﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX3_RebelArc : Mission
    {
        private readonly string FIRST_ALLIANCE_LEVEL = "flightTraining_1";
        private readonly string SECOND_ALLIANCE_LEVEL = "flightTraining_2";
        private readonly string FINAL_BATTLE = "flightTraining_3";

        private enum EventID
        {
            ArriveAtTelmun,
            BetweenAllianceAttacks,
            KilledOnAllianceLevel,
            AfterAllianceAttacks
        }
        public MainX3_RebelArc(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"), new EventTextCapsule(
                    GetEvent((int)EventID.ArriveAtTelmun), null, EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetPlanet("Telmun"), FIRST_ALLIANCE_LEVEL,
                LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.BetweenAllianceAttacks),
                    GetEvent((int)EventID.KilledOnAllianceLevel), EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[2],
                Game.stateManager.overworldState.GetPlanet("Telmun"), SECOND_ALLIANCE_LEVEL,
                LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAllianceAttacks), GetEvent((int)EventID.KilledOnAllianceLevel),
                    EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[3],
                Game.stateManager.overworldState.GetPlanet("Telmun"), FINAL_BATTLE,
                LevelStartCondition.TextCleared));
        }

        public override void StartMission()
        {
            MissionManager.RemoveAvailableMission("Main - Alliance Arc");
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            base.OnReset();

            ObjectiveIndex = 0;

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Reset();
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