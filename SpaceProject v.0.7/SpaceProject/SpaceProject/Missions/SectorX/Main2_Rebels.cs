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
        private enum EventID
        {
            Introduction,
            FreighterStart,
            AllianceAttack,
            ArrivalAtSoelara
        }

        FreighterShip freighter1;
        FreighterShip freighter2;
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

            freighter1 = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter1.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                Game.stateManager.overworldState.GetStation("Soelara Station"));

            freighter2 = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter2.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetStation("Soelara Station"),
                Game.stateManager.overworldState.GetPlanet("Highfence"));

            //OBJECTIVES
            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetPlanet("Highfence"),
                new EventTextCapsule(GetEvent((int)EventID.Introduction), null, EventTextCanvas.BaseState)));

            // Lots-of-paramaters-version of EscortObjective
            objectives.Add(new EscortObjective(Game,
                this,
                ObjectiveDescriptions[1], 
                Game.stateManager.overworldState.GetStation("Soelara Station"),
                new EscortDataCapsule(freighter1,
                    GetEvent((int)EventID.FreighterStart).Text, 
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(3, "rebel"),
                    new List<String> { "Death to the Alliance 1!", "Death to the Alliance 2!", "Death to the Alliance 3!" },
                    null,
                    Game.stateManager.overworldState.GetPlanet("Highfence").position + new Vector2(-200, 0),
                    GetEvent((int)EventID.AllianceAttack).Text,
                    22500,
                    7500,
                    2000,
                    new List<String> { "SecondMissionlvl1", "SecondMissionlvl2", "SecondMissionlvl3" }),
                new EventTextCapsule(GetEvent((int)EventID.ArrivalAtSoelara),
                    null,
                    EventTextCanvas.BaseState)));

            // A-few-less-parameters-version of Escort Objective (just an example, will not be used in finished version of mission)
            //objectives.Add(new EscortObjective(Game,
            //    this,
            //    ObjectiveDescriptions[2],
            //    Game.stateManager.overworldState.GetPlanet("Highfence"),
            //    new EscortDataCapsule(freighter2,
            //        "Hello! Let's go back!",
            //        Game.stateManager.overworldState.GetSectorX.shipSpawner.GetOverworldShips(5, "rebel"),
            //        Game.stateManager.overworldState.GetStation("Soelara Station").position + new Vector2(200, -200),
            //        new List<String> { "SecondMissionlvl3", "SecondMissionlvl2", "SecondMissionlvl1",
            //            "SecondMissionlvl2", "SecondMissionlvl3" },
            //        17500,
            //        7500)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
                Game.stateManager.overworldState.GetPlanet("Highfence")));
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
