using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class AstroDodger: Mission
    {
        private DestroyedShip destroyedShip;

        public AstroDodger(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            destroyedShip = new DestroyedShip(this.Game, spriteSheet);
            destroyedShip.Initialize();

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destroyedShip,
                new EventTextCapsule(
                    new List<String> { EventArray[0, 0], EventArray[1, 0] },
                    null,
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destroyedShip,
                "AstroDodger",
                LevelStartCondition.Immediately,
                new EventTextCapsule(
                    new List<String> { EventArray[2, 0] },
                    new List<String> { "You decide it's best to abandon the ship and return to Soelara Station. No reward is worth getting crushed by asteroids." },
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[1],
                Game.stateManager.overworldState.getStation("Soelara Station")));
        }

        public override void StartMission()
        {
            progress = 0;
            ObjectiveIndex = 0;

            destroyedShip.IsUsed = true;
            Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
        }

        public override void OnLoad()
        {
            if (!Game.stateManager.overworldState.ContainsOverworldObject(destroyedShip) &&
                (MissionState == StateOfMission.Active || MissionState == StateOfMission.Completed ||
                MissionState == StateOfMission.Failed))
            {
                destroyedShip.IsUsed = true;
                Game.stateManager.overworldState.AddOverworldObject(destroyedShip);
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
