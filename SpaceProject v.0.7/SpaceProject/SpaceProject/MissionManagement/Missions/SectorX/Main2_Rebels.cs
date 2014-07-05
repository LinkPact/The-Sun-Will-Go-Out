using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main2_Rebels : Mission
    {
        int messageTimer = 0;

        private int rebelShipSpawnerDelay;
        private int numberOfRebelShips = 3;

        int levelProgression = 0;

        FreighterShip freighter;
        private float freighterHP;
        public float GetFreighterHP { get { return freighterHP; } }

        public Main2_Rebels(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            freighterHP = 2000;

            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.getPlanet("Highfence"),
                Game.stateManager.overworldState.getStation("Soelara Station"));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.getPlanet("Highfence"),
                new EventTextCapsule( new List<String> { EventArray[0, 0], EventArray[1, 0], EventArray[2, 0], EventArray[3, 0] },
                    null, EventTextCanvas.BaseState)));

            objectives.Add(new EscortObjective(Game, this, ObjectiveDescriptions[1], 
                Game.stateManager.overworldState.getPlanet("Soelara"),
                new EscortDataCapsule(freighter, new List<String> { EventArray[4, 0] }, 
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(3, "rebel"), 
                    new List<String> { "Death to the Alliance!" }, null,
                    Game.stateManager.overworldState.getPlanet("Highfence").position + new Vector2(-200, 0),
                    200, 100, 2000, new List<String> { "SecondMissionlvl1", "SecondMissionlvl2", "SecondMissionlvl3" })));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        {
            // Player returns to overworld after speaking to Kamali
            if (progress == 2 || progress == 3)
            {
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    freighter, Game.stateManager.overworldState.getPlanet("Highfence").position);

                freighter.position = Game.stateManager.overworldState.getPlanet("Highfence").position - new Vector2(200, 0);

                freighter.Wait();

                ObjectiveIndex = 2;
                progress = 2;
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            MissionManager.MarkMissionAsFailed(this.MissionName);
            ObjectiveIndex = 0;
            Game.stateManager.overworldState.RemoveOverworldObject(freighter);
            levelProgression = 0;
            progress = 0;
            numberOfRebelShips = 3;
            freighterHP = 1000;
            PirateShip.FollowPlayer = true;
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
            
            //// Player returns to overworld after visiting Soelara Station
            //if (progress == 4 && GameStateManager.currentState.Equals("OverworldState"))
            //{
            //    ObjectiveIndex = 4;
            //    progress = 5;
            //}
            //
            //// Player returns to colony on Highfence
            //if (progress == 5 && missionHelper.IsPlayerOnPlanet("Highfence"))
            //{
            //    MissionManager.MarkMissionAsCompleted(this.MissionName);
            //}
            //
            //if (GameStateManager.currentState == "OverworldState")
            //{
            //    Collision();
            //}
        }

        private void Collision()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (progress == 2 && GameStateManager.currentState.Equals("OverworldState") &&
                CollisionDetection.IsRectInRect(Game.player.Bounds, freighter.Bounds))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, freighter, new Rectangle(2, 374, 0, 0));
                Game.helper.DisplayText("Press 'Enter' to talk to freighter captain..");
            }
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
