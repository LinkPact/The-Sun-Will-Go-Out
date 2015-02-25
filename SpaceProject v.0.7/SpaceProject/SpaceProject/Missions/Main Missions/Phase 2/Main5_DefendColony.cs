using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Main5_DefendColony : Mission
    {
        private enum EventID
        {
            Introduction,
            OutsideFortrun1,
            OutsideFortrun2,
            ToNewNorrland1,
            ToNewNorrland2,
            ToNewNorrland3,
            OutsideNewNorrland,
            FirstLevelCompleted,
            SecondLevelCompleted,
            ToFortrun1,
            ToFortrun2
        }

        private readonly float AutoPilotSpeed = 0.5f;

        private readonly int numberOfAllies = 4;
        private List<OverworldShip> allyShips1;
        private List<OverworldShip> allyShips2;
        Planet newNorrland;
        Station fortrunStation;

        public Main5_DefendColony(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            newNorrland = Game.stateManager.overworldState.GetPlanet("New Norrland");
            fortrunStation = Game.stateManager.overworldState.GetStation("Fortrun Station I");

            allyShips1 = CreateAllyShips(numberOfAllies);
            allyShips2 = CreateAllyShips(numberOfAllies);

            SetDestinations();
            SetupObjectives();
            RestartAfterFail();
        }

        public override void StartMission()
        { 
            ObjectiveIndex = 0;
            progress = 0;
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
        }

        public override void OnReset()
        {
            RemoveShips(allyShips1);
            RemoveShips(allyShips2);

            allyShips1 = CreateAllyShips(numberOfAllies);
            allyShips2 = CreateAllyShips(numberOfAllies);

            SetDestinations();
            SetupObjectives();

            base.OnReset();

            ObjectiveIndex = 0;
        }

        public override void OnLoad()
        { }

        public override void OnFailed()
        {
            base.OnFailed();

            Game.messageBox.DisplayMessage("The operation was a failure. Return to Fortrun Station 1 to try again.", false);
        }

        public override void MissionLogic()
        {
            base.MissionLogic();
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

            destinations.Add(fortrunStation);
            destinations.Add(allyShips1[2]);
            destinations.Add(allyShips1[2]);
            destinations.Add(newNorrland);
            destinations.Add(newNorrland);
            destinations.Add(newNorrland);
            destinations.Add(newNorrland);
            destinations.Add(newNorrland);
            destinations.Add(fortrunStation);
            destinations.Add(fortrunStation);
            destinations.Add(fortrunStation);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                delegate { SetupAllyShips(allyShips1, fortrunStation.position, newNorrland); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[1],
                200, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun1), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[2],
                100, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun2), null, EventTextCanvas.MessageBox)));

            objectives.Add(new AutoPilotObjective(Game, this, ObjectiveDescriptions[1], destinations[3], AutoPilotSpeed,
                allyShips1, fortrunStation.position,
                new Dictionary<string, List<float>>
                { 
                    { GetEvent((int)EventID.ToNewNorrland1).Text, new List<float> { 5000, 3000 } },
                    { GetEvent((int)EventID.ToNewNorrland2).Text, new List<float> { 8000, 3000 } },
                    { GetEvent((int)EventID.ToNewNorrland3).Text, new List<float> { 20000, 3000 } }
                },
                new EventTextCapsule(GetEvent((int)EventID.OutsideNewNorrland), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                destinations[4], "DefendColonyBreak", LevelStartCondition.TextCleared));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[5],
                new EventTextCapsule(GetEvent((int)EventID.FirstLevelCompleted), null, EventTextCanvas.BaseState),
                delegate { Game.stateManager.GotoPlanetSubScreen("New Norrland", "Mission"); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                destinations[6], "DefendColonyHold", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.SecondLevelCompleted), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1], destinations[7],
                new EventTextCapsule(new EventText("[Ai] \"Let's go\""), null, EventTextCanvas.MessageBox),
                delegate
                {
                    RemoveShips(allyShips1);
                    SetupAllyShips(allyShips2,
                        new Vector2(newNorrland.position.X + 50, newNorrland.position.Y - 325),
                        fortrunStation);
                },
                delegate { },
                delegate { return GameStateManager.currentState == "OverworldState"; },
                delegate { return false; }));

            objectives.Add(new AutoPilotObjective(Game, this, ObjectiveDescriptions[2], destinations[8], AutoPilotSpeed,
                allyShips2, newNorrland.position,
                new Dictionary<string, List<float>>
                { 
                    { GetEvent((int)EventID.ToFortrun1).Text, new List<float> { 5000, 3000 } },
                    { GetEvent((int)EventID.ToFortrun2).Text, new List<float> { 8000, 3000 } }
                }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[2], destinations[9],
                delegate
                {
                    RemoveShips(allyShips2);
                    Game.stateManager.GotoStationSubScreen("Fortrun Station I", "Mission");
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2], destinations[10]));
        }

        private void RemoveShips(List<OverworldShip> ships)
        {
            foreach (OverworldShip ship in ships)
            {
                ship.IsDead = true;
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
            }

            ships.Clear();
            destinations.Clear();
        }

        private List<OverworldShip> CreateAllyShips(int count)
        {
            List<OverworldShip> ships = new List<OverworldShip>();

            for (int i = 0; i < count; i++)
            {
                ships.Add(new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Alliance));
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
