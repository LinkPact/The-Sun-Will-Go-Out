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
        private readonly string AVOID_ALLIANCE = "OnYourOwn_1";
        private readonly string AVOID_REBELS = "OnYourOwn_2";
        private readonly string FINAL_BATTLE = "flightTraining_3";

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
            isMainMission = true;
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

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"),
                new EventTextCapsule(GetEvent((int)EventID.SettingExplosions), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            //objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
            //    Game.stateManager.overworldState.GetPlanet("Telmun"), FINAL_BATTLE, LevelStartCondition.TextCleared,
            //    new EventTextCapsule(null, GetEvent((int)EventID.KilledOnLevel),
            //        EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"),
                delegate 
                { 
                    Game.stateManager.shooterState.BeginLevel(FINAL_BATTLE); 
                },
                delegate { },
                delegate { return Game.stateManager.shooterState.GetLevel(FINAL_BATTLE).IsObjectiveCompleted; },
                delegate { return false; }));
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
