using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX5_2b_Coward : Mission
    {
        private readonly string ESCAPE_LEVEL = "PirateLevel1";

        private enum EventID
        {
            Fleeing,
            AfterEnemyAttack,
            AfterExplosion,
            KilledOnLevel
        }
        public MainX5_2b_Coward(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Telmun"), ESCAPE_LEVEL,
                LevelStartCondition.EnteringOverworld,
                new EventTextCapsule(GetEvent((int)EventID.AfterEnemyAttack), GetEvent((int)EventID.KilledOnLevel),
                    EventTextCanvas.MessageBox)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], null,
                new EventTextCapsule(GetEvent((int)EventID.AfterExplosion), null, EventTextCanvas.MessageBox),
                delegate
                {

                },
                delegate
                {

                },
                delegate
                {
                    return !CollisionDetection.IsPointInsideCircle(
                        Game.player.position, Game.stateManager.overworldState.GetPlanet("Telmun").position, 1000);
                },
                delegate
                {
                    return false;
                }));
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.Fleeing));
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
