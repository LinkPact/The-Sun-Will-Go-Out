using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main10_3c_OnYourOwnArc : Mission
    {
        private readonly string AvoidAllianceLevel = "OnYourOwn_1";
        private readonly string AvoidRebelsLevel = "OnYourOwn_2";
        private readonly string FinalBattle = "flightTraining_3";

        private enum EventID
        {
            AfterAllianceAttack,
            AfterRebelAttack,
            KilledOnLevel,
            SettingExplosions
        }
        public Main10_3c_OnYourOwnArc(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            MissionManager.RemoveAvailableMission("Main - Rebel Arc");
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            destinations.Add(telmun);
            destinations.Add(telmun);
            destinations.Add(telmun);
            destinations.Add(telmun);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                destinations[0], AvoidAllianceLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAllianceAttack), GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                destinations[1], AvoidRebelsLevel, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterRebelAttack), GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                destinations[2],
                new EventTextCapsule(GetEvent((int)EventID.SettingExplosions), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                destinations[3],
                delegate
                {
                    Game.stateManager.shooterState.BeginLevel(FinalBattle);
                },
                delegate { },
                delegate { return Game.stateManager.shooterState.GetLevel(FinalBattle).IsObjectiveCompleted; },
                delegate { return false; }));
        }
    }
}
