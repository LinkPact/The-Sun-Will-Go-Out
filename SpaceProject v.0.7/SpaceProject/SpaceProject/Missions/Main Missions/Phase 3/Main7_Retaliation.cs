using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main7_Retaliation : Mission
    {
        private enum EventID
        {
            Introduction, 
            ToMeetingPoint,
            AtMeetingPoint1,
            AtMeetingPoint2,
            AtMeetingPoint3,
            AtMeetingPoint4,
            Level,
            AfterLevel,
            BackToRebelBase
        }

        private List<RebelShip> rebelShips;
        private readonly int numberOfRebelShips = 3;
        
        private AllianceShip alliance1;

        private readonly Vector2 destination = new Vector2(94600, 100000);

        private FreighterShip freighter;

        public Main7_Retaliation(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            isMainMission = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();
            InitializeShips();
            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));

            StatsManager.reputation = -100;
        }

        public override void OnLoad()
        {
            switch (ObjectiveIndex)
            {
                case 0:
                case 1:
                case 2:
                case 3:
                case 4:
                case 5:
                case 6:
                    ObjectiveIndex = 1;
                    break;

                case 7:
                case 9:
                    ObjectiveIndex = 8;
                    break;

                default:
                    break;
            }
        }

        public override void OnReset()
        {
            InitializeShips();
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            RemoveShips();
            Game.messageBox.DisplayMessage("The attack on the alliance freighter failed. Return to the rebel base to try again.", false);
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

            GameObjectOverworld rebelBase = Game.stateManager.overworldState.GetStation("Rebel Base");

            destinations.Add(rebelShips[0]);
            destinations.Add(rebelShips[0]);
            destinations.Add(rebelShips[0]);
            destinations.Add(rebelShips[0]);
            destinations.Add(rebelShips[0]);
            destinations.Add(freighter);
            destinations.Add(freighter);
            destinations.Add(rebelBase);
            destinations.Add(rebelBase);
            destinations.Add(rebelBase);
            destinations.Add(rebelBase);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                new EventTextCapsule(GetEvent((int)EventID.ToMeetingPoint), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[1],
                delegate
                {
                    foreach (OverworldShip ship in rebelShips)
                    {
                        Game.stateManager.overworldState.AddOverworldObject(ship);
                    }
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[2], 400));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[3],
                delegate
                {
                    OverworldShip.FollowPlayer = false;
                    StartFreighter();
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[4],
                new List<string> { GetEvent((int)EventID.AtMeetingPoint1).Text, GetEvent((int)EventID.AtMeetingPoint2).Text,
                    GetEvent((int)EventID.AtMeetingPoint3).Text, GetEvent((int)EventID.AtMeetingPoint4).Text },
                2000, 1000));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[5],
                delegate { },
                delegate { },
                delegate
                {
                    return Vector2.Distance(rebelShips[0].position, freighter.position) < 500;
                },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[6],
                delegate { },
                delegate { },
                delegate
                {
                    return CollisionDetection.IsRectInRect(rebelShips[0].Bounds, freighter.Bounds);
                },
                delegate { return false; }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], destinations[7],
                "Retribution1", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel), null, EventTextCanvas.MessageBox)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1], destinations[8],
                delegate
                {
                    freighter.Destroy();
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        alliance1, Game.player.position + new Vector2(-300, 0), "Retribution2", Game.player);
                    OverworldShip.FollowPlayer = true;
                },
                delegate { }, delegate { return true; }, delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1], destinations[9],
                delegate { },
                delegate { },
                delegate
                {
                    if (Game.stateManager.shooterState.CurrentLevel != null
                        && Game.stateManager.shooterState.CurrentLevel.Identifier.Equals("Retribution2"))
                    {
                        return (Game.stateManager.shooterState.CurrentLevel.IsObjectiveCompleted
                            && GameStateManager.currentState.Equals("OverworldState"));
                    }

                    return (missionHelper.IsPlayerOnStation("Rebel Base"));
                },
                delegate
                {
                    if (Game.stateManager.shooterState.CurrentLevel != null
                        && Game.stateManager.shooterState.CurrentLevel.Identifier.Equals("Retribution2"))
                    {
                        return (Game.stateManager.shooterState.CurrentLevel.IsObjectiveFailed
                            && GameStateManager.currentState.Equals("OverworldState"));
                    }

                    return false;
                }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1], destinations[10]));
        }

        private void StartFreighter()
        {
            Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                freighter, Game.stateManager.overworldState.GetStation("Soelara Station").position);
        }

        private void InitializeShips()
        {
            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetStation("Soelara Station"),
                Game.stateManager.overworldState.GetStation("Fortrun Station I"));

            alliance1 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance1.SaveShip = false;
            alliance1.Initialize(Game.stateManager.overworldState.GetSectorX);

            rebelShips = new List<RebelShip>();

            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips[i].Initialize();
                rebelShips[i].RemoveOnStationEnter = false;
                rebelShips[i].position = new Vector2(destination.X + (i * 50), destination.Y);
                rebelShips[i].collisionEvent = null;
                rebelShips[i].SaveShip = false;

                actions.Add(new WaitAction(rebelShips[i],
                    delegate
                    {
                        return ObjectiveIndex >= 6;
                    }));

                actions.Add(new TravelAction(rebelShips[i], freighter));
                switch (i)
                {
                    case 0:
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetPlanet("Lavis")));
                        break;

                    case 1:
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetStation("Rebel Base")));
                        break;

                    case 2:
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetPlanet("New Norrland")));
                        break;
                }

                rebelShips[i].AIManager = actions;
            }
        }

        private void RemoveShips()
        {
            alliance1.Remove();
            freighter.Remove();
            foreach (RebelShip rebel in rebelShips)
            {
                rebel.Remove();
            }
        }
    }
}
