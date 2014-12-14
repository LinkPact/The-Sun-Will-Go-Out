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

        private readonly float autoPilotSpeed = 0.3f;

        private readonly int numberOfAllies = 4;
        private List<OverworldShip> allyShips1;
        private List<OverworldShip> allyShips2;

        public Main5_DefendColony(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            Planet newNorrland = Game.stateManager.overworldState.GetPlanet("New Norrland");
            Station fortrunStation = Game.stateManager.overworldState.GetStation("Fortrun Station I");

            allyShips1 = CreateAllyShips(numberOfAllies);
            allyShips2 = CreateAllyShips(numberOfAllies);

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], fortrunStation,
                delegate { SetupAllyShips(allyShips1, fortrunStation.position, newNorrland); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], allyShips1[2],
                200, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun1), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], allyShips1[2],
                100, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun2), null, EventTextCanvas.MessageBox)));

            objectives.Add(new AutoPilotObjective(Game, this, ObjectiveDescriptions[0], newNorrland, autoPilotSpeed,
                allyShips1, fortrunStation.position,
                new Dictionary<string, List<float>>
                { 
                    { GetEvent((int)EventID.ToNewNorrland1).Text, new List<float> { 5000, 3000 } },
                    { GetEvent((int)EventID.ToNewNorrland2).Text, new List<float> { 8000, 3000 } },
                    { GetEvent((int)EventID.ToNewNorrland3).Text, new List<float> { 20000, 3000 } }
                },
                new EventTextCapsule(GetEvent((int)EventID.OutsideNewNorrland), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                newNorrland, "DefendColonyBreak", LevelStartCondition.TextCleared));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                new EventTextCapsule(GetEvent((int)EventID.FirstLevelCompleted), null, EventTextCanvas.BaseState),
                delegate { Game.stateManager.GotoPlanetSubScreen("New Norrland", "Mission"); },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
                newNorrland, "DefendColonyHold", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.SecondLevelCompleted), null, EventTextCanvas.BaseState)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
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

            objectives.Add(new AutoPilotObjective(Game, this, ObjectiveDescriptions[0], fortrunStation, autoPilotSpeed,
                allyShips2, newNorrland.position,
                new Dictionary<string, List<float>>
                { 
                    { GetEvent((int)EventID.ToFortrun1).Text, new List<float> { 5000, 3000 } },
                    { GetEvent((int)EventID.ToFortrun2).Text, new List<float> { 8000, 3000 } }
                }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                delegate 
                {
                    RemoveShips(allyShips2);
                    Game.stateManager.GotoStationSubScreen("Fortrun Station I", "Mission"); 
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], fortrunStation));

            //objectives.Add(new ArSecondLevelCompletedive(Game, this, ObjectiveDescriptions[2],
            //    Game.stateManager.overworldState.GetStation("Fortrun Station I")));
            //
            //objectives.Add(new ResponseObjective(Game, this, ObjectiveDescriptions[3],
            //    Game.stateManager.overworldState.GetStation("Fortrun Station I"),
            //    new ResponseTextCapsule(GetEvent(2), GetAllResponses(2),
            //        new List<System.Action> 
            //                    {
            //                        delegate 
            //                        {
            //                            missionHelper.ShowEvent(GetEvent(3));
            //                        },
            //                        delegate 
            //                        {
            //                            missionHelper.ShowEvent(GetEvent(4));
            //                        }
            //                    }, EventTextCanvas.BaseState)));

            RestartAfterFail();
        }

        public override void StartMission()
        { 
            ObjectiveIndex = 0;
            progress = 0;
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
        }

        public override void OnLoad()
        { }

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
