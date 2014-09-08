using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX5_1_OnYourOwnArc : Mission
    {
        private readonly string AVOID_ALLIANCE = "flightTraining_1";
        private readonly string AVOID_REBELS = "flightTraining_2";

        private enum EventID
        {
            AfterAllianceAttack,
            AfterRebelAttack,
            KilledOnLevel
        }
        public MainX5_1_OnYourOwnArc(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"), AVOID_ALLIANCE, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterAllianceAttack), GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.BaseState)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetPlanet("Telmun"), AVOID_REBELS, LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterRebelAttack), GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.BaseState)));
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
    }
}
