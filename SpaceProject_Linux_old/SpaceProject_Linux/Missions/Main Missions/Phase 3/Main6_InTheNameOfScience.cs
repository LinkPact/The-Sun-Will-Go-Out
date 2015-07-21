using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    public class Main6_InTheNameOfScience : Mission
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
            InsideScienceStation1,
            InsideScienceStation2,
            InsideScienceStation3,
            OutsideScienceStation,
            BreakThroughLevel,
            DuringLevel2,
            AfterLevel2,
            OutsideRebelStation2
        }

        private readonly float autoPilotSpeed = 0.5f;
        private float hangarAttackTime;

        private readonly int numberOfAllies = 4;
        private List<OverworldShip> allyShips1;

        public Main6_InTheNameOfScience(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            allyShips1 = CreateAllyShips(numberOfAllies);

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
        }

        public override void OnLoad()
        {
            switch (ObjectiveIndex)
            {
                case 1:
                case 2:
                case 3:
                    ObjectiveIndex = 0;
                    break;

            }
        }

        public override void OnReset()
        {
            allyShips1 = CreateAllyShips(numberOfAllies);
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            RemoveShips();
            PopupHandler.DisplayMessage("You were unsuccessful in retrieving Ente. Return to the rebel base to try again.");
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

            Station rebelBase = Game.stateManager.overworldState.GetStation("Rebel Base");
            Station peyeScienceStation = Game.stateManager.overworldState.GetStation("Peye Science Station");

            AddDestination(rebelBase);
            AddDestination(peyeScienceStation);
            AddDestination(allyShips1[1]);
            AddDestination(peyeScienceStation, 5);
            AddDestination(rebelBase, 5);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { SetupAllyShips(allyShips1, destinations[0].position, destinations[1]); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.OutsideRebelStation1), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0],
                200, new EventTextCapsule(GetEvent((int)EventID.AtRebelRendezvous), null, EventTextCanvas.MessageBox, PortraitID.RebelTroopLeader)));

            AutoPilotObjective autoPilotObjective = new AutoPilotObjective(Game, this, ObjectiveDescriptions[0], autoPilotSpeed,
                allyShips1, destinations[3].position,
                new EventTextCapsule(GetEvent((int)EventID.ArriveAtScienceStation),
                    null,
                    EventTextCanvas.MessageBox, PortraitID.RebelTroopLeader), false);

            autoPilotObjective.Initialize();
            autoPilotObjective.SetTimedMessages(new Dictionary<string, List<float>>
                { 
                    { GetEvent((int)EventID.TravelToScienceStation1).Text, new List<float> { 5000, 3000 } },
                    { GetEvent((int)EventID.TravelToScienceStation2).Text, new List<float> { 1000, 3000 } },
                    { GetEvent((int)EventID.TravelToScienceStation3).Text, new List<float> { 13000, 3000 } }
                }
                , new List<List<PortraitID>>()
                {
                    new List<PortraitID> {PortraitID.Ai},
                    new List<PortraitID> {PortraitID.Sair},
                    new List<PortraitID> {PortraitID.Ai}
                }
                , new List<List<int>> { new List<int> { }, new List<int> { }, new List<int> { } });

            objectives.Add(autoPilotObjective);

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "Itnos_1", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel1),
                    null, EventTextCanvas.MessageBox, PortraitID.RebelTroopLeader)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.InsideScienceStation1), null, EventTextCanvas.MessageBox, PortraitID.Sair)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.InsideScienceStation2), null, EventTextCanvas.MessageBox),
                delegate { }, delegate { }, delegate { return true; }, delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.InsideScienceStation3), null, EventTextCanvas.MessageBox, PortraitID.Ente),
                delegate { }, delegate { }, delegate { return true; }, delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1],
                new EventTextCapsule(GetEvent((int)EventID.OutsideScienceStation), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate 
                {
                    RemoveShips();
                }, delegate { },
                delegate
                {
                    return GameStateManager.currentState == "OverworldState";
                },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1],
                new EventTextCapsule(GetEvent((int)EventID.BreakThroughLevel), null, EventTextCanvas.MessageBox, PortraitID.Sair),
                delegate
                {
                    hangarAttackTime = StatsManager.PlayTime.GetFutureOverworldTime(2000);
                },
                delegate { },
                delegate
                {
                    return StatsManager.PlayTime.HasOverworldTimePassed(hangarAttackTime);
                },
                delegate { return false; }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                "Itnos_2", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel2), null, EventTextCanvas.MessageBox, PortraitID.RebelTroopLeader)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[1], 200,
                new EventTextCapsule(GetEvent((int)EventID.OutsideRebelStation2), null, EventTextCanvas.MessageBox, PortraitID.Ente)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1]));
        }

        private void RemoveShips()
        {
            foreach (OverworldShip ship in allyShips1)
            {
                ship.IsDead = true;
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
            }
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
