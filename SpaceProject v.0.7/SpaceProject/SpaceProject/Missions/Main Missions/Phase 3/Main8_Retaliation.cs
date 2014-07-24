using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main8_Retaliation : Mission
    {
        private enum EventID
        {

        }

        private AllyShip rebel1;
        private AllyShip rebel2;
        private AllyShip rebel3;

        private AmbushSpot ambushSpot;

        public Main8_Retaliation(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            ambushSpot = new AmbushSpot(Game, spriteSheet);
            ambushSpot.Initialize();
            Game.stateManager.overworldState.AddOverworldObject(ambushSpot);

            rebel1 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel1.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1"),
                ambushSpot);

            rebel2 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel2.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1"),
                ambushSpot);

            rebel3 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel3.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1"),
                ambushSpot);

            objectives.Add(new FollowObjective(Game, this, ObjectiveDescriptions[0],
                ambushSpot,
                new EventTextCapsule(new EventText("Wait for the freighter to arrive."), null, EventTextCanvas.MessageBox),
                "",
                Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                new Vector2(50, 50),
                rebel1, rebel2, rebel3));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
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
