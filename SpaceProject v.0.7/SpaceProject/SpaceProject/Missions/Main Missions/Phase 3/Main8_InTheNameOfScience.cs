using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main8_InTheNameOfScience : Mission
    {
        private enum EventID
        {
            Introduction,
            OutsideRebelStation1,
            AtRebelRendezvous,
            TravelToScienceStation1,
            TravelToScienceStation2,
            TravelToScienceStation3,
            ArriveAtScienceStation,
            AfterLevel1,
            InsideScienceStation,
            OutsideScienceStation,
            BreakThroughLevel,
            AfterLevel2,
            OutsideRebelStation2
        }

        private readonly float autoPilotSpeed = 0.3f;

        private readonly int numberOfAllies = 4;
        private List<OverworldShip> allyShips1;

        public Main8_InTheNameOfScience(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Station rebelStation3 = Game.stateManager.overworldState.GetStation("Rebel Station 3");
            Station peyeScienceStation = Game.stateManager.overworldState.GetStation("Peye Science Station");

            RestartAfterFail();

            allyShips1 = CreateAllyShips(numberOfAllies);

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebelStation3,
                delegate { SetupAllyShips(allyShips1, rebelStation3.position, peyeScienceStation); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], peyeScienceStation,
                new EventTextCapsule(GetEvent((int)EventID.OutsideRebelStation1), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], peyeScienceStation, 1000,
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtScienceStation), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], peyeScienceStation,
                "Main10_AllianceDefence", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel1),
                    null, EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], peyeScienceStation,
                new EventTextCapsule(GetEvent((int)EventID.InsideScienceStation), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebelStation3,
                new EventTextCapsule(GetEvent((int)EventID.OutsideScienceStation), null, EventTextCanvas.MessageBox),
                delegate { }, delegate { },
                delegate 
                {
                    return GameStateManager.currentState == "OverworldState";
                },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                rebelStation3));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
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

        private void RemoveShips(List<OverworldShip> ships)
        {
            foreach (OverworldShip ship in ships)
            {
                ship.IsDead = true;
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
            }

            ships.Clear();
        }

        private List<OverworldShip> CreateAllyShips(int count)
        {
            List<OverworldShip> ships = new List<OverworldShip>();

            for (int i = 0; i < count; i++)
            {
                ships.Add(new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel));
            }

            return ships;
        }

        private void SetupAllyShips(List<OverworldShip> ships, Vector2 startingPoint, GameObjectOverworld destination)
        {
            // Adds companion ships
            int modifier = 1;
            for (int i = 0; i < ships.Count; i++)
            {
                ships[i].Initialize();
                ships[i].AIManager = new TravelAction(ships[i], destination);
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                    ships[i], new Vector2(startingPoint.X - 50 + (50 * i),
                                              startingPoint.Y + 325 - (25 * modifier)),
                    "", destination);

                ships[i].Wait();

                modifier *= -1;
            }
        }

    }
}
