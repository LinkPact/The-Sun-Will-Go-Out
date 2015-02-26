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
        private enum EventID
        {
            ShipFound = 0,
            Return = 1
        }

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

            SetDestinations();
            SetupObjectives();
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld fortrunStation1 =
                Game.stateManager.overworldState.GetStation("Fortrun Station I");

            destinations.Add(destroyedShip);
            destinations.Add(destroyedShip);
            destinations.Add(fortrunStation1);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destinations[0],
                new EventTextCapsule(
                    GetEvent((int)EventID.ShipFound),
                    null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                destinations[1],
                "AstroDodger",
                LevelStartCondition.Immediately,
                new EventTextCapsule(
                    GetEvent((int)EventID.Return),
                    new EventText("You decide it's best to abandon the ship and return to Fortrun Station. No reward is worth getting crushed by asteroids."),
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[1],
                destinations[2]));
        }
    }
}
