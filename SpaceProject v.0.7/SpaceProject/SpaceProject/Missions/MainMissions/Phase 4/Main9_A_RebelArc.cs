using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main9_A_RebelArc : Mission
    {
        private readonly string FirstAllianceLevel = "RebelBranch_1";
        private readonly string SecondAllianceLevel = "RebelBranch_2";
        //private readonly string ThirdAllianceLevel = "AlliancePirate3";

        private enum EventID
        {
            CloseToMurt,
            AfterFirstLevel,
            ArriveAtMurt,
            KilledOnAllianceLevel,
            AfterAllianceAttacks
        }
        public Main9_A_RebelArc(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
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
            MissionManager.RemoveAvailableMission(MissionID.Main9_B_AllianceArc);
            MissionManager.RemoveAvailableMission(MissionID.Main9_C_OnYourOwnArc);
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

            PopupHandler.DisplayMessage("You could not defend the Murt from the Alliance, return to the rebel fleet outside Telmun to try again.");
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

            GameObjectOverworld telmun = Game.stateManager.overworldState.GetPlanet("Murt Asteroid");

            AddDestination(telmun, 5);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], 300, new EventTextCapsule(
                GetEvent((int)EventID.CloseToMurt), null, EventTextCanvas.MessageBox, PortraitID.Rok)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                FirstAllianceLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterFirstLevel),
                    null, EventTextCanvas.MessageBox, PortraitID.Rok)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], new EventTextCapsule(
                GetEvent((int)EventID.ArriveAtMurt), null, EventTextCanvas.MessageBox, PortraitID.Rok)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[2],
                SecondAllianceLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAllianceAttacks), null,
                    EventTextCanvas.MessageBox, PortraitID.Rok)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate
                {
                    return PopupHandler.TextBufferEmpty;
                },
                delegate { return false; }));

            //objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[3], destinations[3],
            //    ThirdAllianceLevel, LevelStartCondition.TextCleared));
        }
    }
}
