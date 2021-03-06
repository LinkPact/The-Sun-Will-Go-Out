﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    public class Main9_C_OnYourOwnArc : Mission
    {
        private readonly string AvoidAllianceLevel = "OnYourOwn_1";
        private readonly string AvoidRebelsLevel = "OnYourOwn_2";
        //private readonly string FinalBattle = "flightTraining_3";

        private enum EventID
        {
            AfterAllianceAttack,
            KilledOnLevel,
            SettingExplosions
        }
        public Main9_C_OnYourOwnArc(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDestinations();
            SetupObjectives();
            RestartAfterFail();
        }

        public override void StartMission()
        {
            MissionManager.RemoveAvailableMission(MissionID.Main9_A_RebelArc);
            MissionManager.RemoveAvailableMission(MissionID.Main9_B_AllianceArc);
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            PopupHandler.DisplayMessage("You could not reach the Murt before the Alliance and the rebels. Go to Telmun to try again.");
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            AddDestination(telmun, 4);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                AvoidAllianceLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAllianceAttack), null,
                    EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                AvoidRebelsLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.SettingExplosions), null,
                    EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate
                {
                    return PopupHandler.TextBufferEmpty;
                },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate 
                {
                    Game.stateManager.ChangeState("OverworldState");
                    Game.stateManager.overworldState.ActivateBurnOutEnding();
                },
                delegate { },
                delegate { return false; },
                delegate { return false; }));

            //objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
            //    destinations[3],
            //    delegate
            //    {
            //        Game.stateManager.shooterState.BeginLevel(FinalBattle);
            //    },
            //    delegate { },
            //    delegate { return Game.stateManager.shooterState.GetLevel(FinalBattle).IsObjectiveCompleted; },
            //    delegate { return false; }));
        }
    }
}
