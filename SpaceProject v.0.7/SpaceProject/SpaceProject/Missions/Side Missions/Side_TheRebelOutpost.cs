using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Side_TheRebelOutpost: Mission
    {
        private enum EventID
        {
            ShipFound = 0,
            Return = 1
        }

        private RebelOutpostAsteroid rebelOutpost;
        private Sprite overworldSpriteSheet;

        public Side_TheRebelOutpost(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            overworldSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/SectorXSpriteSheet"), null);
        }

        public override void Initialize()
        {
            base.Initialize();

            rebelOutpost = new RebelOutpostAsteroid(Game, overworldSpriteSheet);
            rebelOutpost.Initialize();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            progress = 0;
            ObjectiveIndex = 0;

            rebelOutpost.IsUsed = true;
            Game.stateManager.overworldState.AddOverworldObject(rebelOutpost);
        }

        public override void OnLoad()
        {
            if (!Game.stateManager.overworldState.ContainsOverworldObject(rebelOutpost) &&
                (MissionState == StateOfMission.Active || MissionState == StateOfMission.Completed ||
                MissionState == StateOfMission.Failed))
            {
                rebelOutpost.IsUsed = true;
                Game.stateManager.overworldState.AddOverworldObject(rebelOutpost);
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

            GameObjectOverworld borderStation =
                Game.stateManager.overworldState.GetStation("Border Station");

            AddDestination(rebelOutpost, 2);
            AddDestination(borderStation);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                new EventTextCapsule(
                    GetEvent((int)EventID.ShipFound),
                    null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(
                Game,
                this,
                ObjectiveDescriptions[0],
                "AstroDodger",
                LevelStartCondition.Immediately,
                new EventTextCapsule(
                    GetEvent((int)EventID.Return),
                    new EventText("You decide it's best to abandon the ship and return to Fortrun Station. No reward is worth getting crushed by asteroids."),
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(
                Game,
                this,
                ObjectiveDescriptions[1]));
        }
    }
}
