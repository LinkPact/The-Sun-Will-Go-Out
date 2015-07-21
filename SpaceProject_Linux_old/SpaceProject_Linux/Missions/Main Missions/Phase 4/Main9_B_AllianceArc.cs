using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    public class Main9_B_AllianceArc : Mission
    {
        private readonly string FirstAttack = "AllianceBranch_1";
        private readonly string SecondAttack = "AllianceBranch_2";
        //private readonly string FinalBattle = "flightTraining_3";

        private enum EventID
        {
            ArriveAtTelmun,
            BetweenAttacks,
            KilledOnLevel,
            AfterAttacks
        }
        public Main9_B_AllianceArc(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
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

            PopupHandler.DisplayMessage("You could not gain control of the Murt, return to the Alliance fleet outside Telmun to try again.");
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

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                 new EventTextCapsule(
                    GetEvent((int)EventID.ArriveAtTelmun), null, EventTextCanvas.MessageBox, PortraitID.Ai)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                     FirstAttack,
                LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.BetweenAttacks),
                    null, EventTextCanvas.MessageBox, PortraitID.Ai)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[2],
                SecondAttack,
                LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAttacks), null,
                    EventTextCanvas.MessageBox, PortraitID.Ai)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate
                {
                    return PopupHandler.TextBufferEmpty;
                },
                delegate { return false; }));

            //objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[3],
            //    destinations[3], FinalBattle,
            //    LevelStartCondition.TextCleared));
        }
    }
}
