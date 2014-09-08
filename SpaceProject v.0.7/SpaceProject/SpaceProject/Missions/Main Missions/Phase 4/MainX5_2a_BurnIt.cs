using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX5_2a_BurnIt : Mission
    {
        private readonly string FINAL_BATTLE = "PirateLevel1";

        private enum EventID
        {
            SettingExplosions,
            KilledOnLevel
        }
        public MainX5_2a_BurnIt(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"), FINAL_BATTLE, LevelStartCondition.TextCleared,
                new EventTextCapsule(null, GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.BaseState)));
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.SettingExplosions));
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
