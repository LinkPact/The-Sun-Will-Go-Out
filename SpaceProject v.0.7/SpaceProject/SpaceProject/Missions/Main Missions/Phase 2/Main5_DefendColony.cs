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

        private Planet newNorrland;
        private bool autoPilot;
        private readonly float autoPilotSpeed = 0.3f;

        private readonly int numberOfAllies = 4;
        private List<AllyShip> allyShips;

        public Main5_DefendColony(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            newNorrland = Game.stateManager.overworldState.GetPlanet("New Norrland");

            allyShips = new List<AllyShip>();

            for (int i = 0; i < numberOfAllies; i++)
            {
                allyShips.Add(new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Alliance));
                allyShips[i].Initialize();
                allyShips[i].AIManager = new TravelAction(allyShips[i], newNorrland);
            }

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], allyShips[2],
                200, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun1), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], allyShips[2],
                100, new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun2), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                delegate
                {
                    foreach (AllyShip ship in allyShips)
                    {
                        ship.Start();
                        ship.speed = autoPilotSpeed;
                    }

                    autoPilot = true;
                    ControlManager.DisableControls();
                },
                delegate
                {
                },
                delegate
                {
                    return true;
                },
                delegate
                {
                    return false;
                }));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                GetEvent((int)EventID.ToNewNorrland1).Text, 4000, 5000));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                GetEvent((int)EventID.ToNewNorrland2).Text, 4000, 3000));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], newNorrland,
                GetEvent((int)EventID.ToNewNorrland3).Text, 4000, 5000));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
                newNorrland));

            //objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1],
            //    Game.stateManager.overworldState.GetPlanet("New Norrland"), "DefendColony", LevelStartCondition.TextCleared,
            //    new EventTextCapsule(GetEvent(1), null, EventTextCanvas.BaseState)));
            //
            //objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
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
            Vector2 stationPos = Game.stateManager.overworldState.GetStation("Fortrun Station I").position;

            ObjectiveIndex = 0;
            progress = 0;
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));

            int modifier = 1;

            for (int i = 0; i < allyShips.Count; i++)
            {
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                    allyShips[i], new Vector2(stationPos.X - 50 + (50 * i),
                                              stationPos.Y + 325 - (25 * modifier)),
                    "", newNorrland);

                allyShips[i].Wait();

                modifier *= -1;
            }
        }

        public override void OnLoad()
        { }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (autoPilot)
            {
                Game.player.Direction.RotateTowardsPoint(Game.player.position, newNorrland.position, 0.2f);
                Game.player.speed = autoPilotSpeed + 0.013f;
            }

            if (Vector2.Distance(Game.player.position, newNorrland.position) < 500)
            {
                autoPilot = false;
                ControlManager.EnableControls();
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
