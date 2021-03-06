﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class Main5_Retribution : Mission
    {
        private enum EventID
        {
            Introduction, 
            ToMeetingPoint,
            AtMeetingPoint1,
            AtMeetingPoint2,
            AtMeetingPoint3,
            AtMeetingPoint4,
            AttackFreighter,
            DuringLevel_1,
            DuringLevel_2,
            AfterLevel,
            BackToRebelBase
        }

        private readonly Vector2 SkipWaitFreighterOffset = new Vector2(500, 500);

        private List<RebelShip> rebelShips;
        private readonly int numberOfRebelShips = 3;
        
        private AllianceShip alliance1;

        private readonly Vector2 destination = new Vector2(94600, 100000);

        private FreighterShip freighter;

        public Main5_Retribution(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
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
            OverworldShip.FollowPlayer = true;
            PopupHandler.DisplayMessage("The attack on the alliance freighter failed. Return to the rebel base to try again.");
        }

        public override void OnCompleted()
        {
            RemoveShips();
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (ObjectiveIndex > 2
                && ObjectiveIndex <= 6
                && !Game.player.HyperspeedOn)
            {
                if (Vector2.Distance(Game.player.position, rebelShips[1].position) > 500)
                {
                    PopupHandler.DisplayPortraitMessage(PortraitID.RebelPilot, "Hey! Where are you going? The freighter will be here any minute now.");
                    Game.player.BounceBack();
                }

                if (ControlManager.CheckPress(RebindableKeys.Pause)
                    && GameStateManager.currentState.Equals("OverworldState"))
                {
                    PopupHandler.DisplaySelectionMenu("Do you want to skip waiting?",
                        new List<string>() { "Yes", "No" },
                        new List<System.Action>(){
                            delegate 
                            {
                                SkipForward();
                            },
                            delegate {}
                        });
                }
            }
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

            AddDestination(rebelShips[0], 6);
            AddDestination(freighter, 2);
            AddDestination(rebelBase, 4);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.ToMeetingPoint), null, EventTextCanvas.MessageBox, PortraitID.Ai),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
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

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], 400));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.AtMeetingPoint1), null, EventTextCanvas.MessageBox, PortraitID.RebelPilot),
                delegate
                {
                    OverworldShip.FollowPlayer = false;
                    StartFreighter();
                },
                delegate 
                { },
                delegate { return true; },
                delegate { return false; }));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 10000, PortraitID.RebelPilot, GetEvent((int)EventID.AtMeetingPoint2).Text));

            TimedMessageObjective timedMessageObjective = new TimedMessageObjective(Game, this, ObjectiveDescriptions[0],
                3000, 10000, PortraitID.RebelPilot, GetEvent((int)EventID.AtMeetingPoint3).Text);
            timedMessageObjective.SetEventText(
                new EventTextCapsule(GetEvent((int)EventID.AtMeetingPoint4), null, EventTextCanvas.MessageBox, PortraitID.RebelPilot));
            Objectives.Add(timedMessageObjective);

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                new EventTextCapsule(GetEvent((int)EventID.AttackFreighter), null, EventTextCanvas.MessageBox, PortraitID.RebelPilot),
                delegate { },
                delegate { },
                delegate
                {
                    return Vector2.Distance(rebelShips[0].position, freighter.position) < 500;
                },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                delegate { },
                delegate { },
                delegate
                {
                    return CollisionDetection.IsRectInRect(rebelShips[0].Bounds, freighter.Bounds);
                },
                delegate { return false; }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                "Retribution1", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel), null, EventTextCanvas.MessageBox, PortraitID.RebelPilot)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[1],
                delegate
                {
                    freighter.Explode();
                    freighter.Destroy();
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        alliance1, Game.player.position + new Vector2(-900, 0), "Retribution2", Game.player);
                    OverworldShip.FollowPlayer = true;
                },
                delegate { }, delegate { return true; }, delegate { return false; }));

            CustomObjective chaseObjective = new CustomObjective(Game, this, ObjectiveDescriptions[1],
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

                    return Game.stateManager.overworldState.GetCurrentSpaceRegion is RebelOutpost;
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
                });

            chaseObjective.SetCompletedLogic(
                delegate
                {
                    alliance1.AIManager = new PatrolAction(alliance1, Game.stateManager.overworldState.GetSectorX);
                });

            objectives.Add(chaseObjective);

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1]));
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
                Game.stateManager.overworldState.GetStation("Fortrun Station"));

            alliance1 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance1.SaveShip = false;
            alliance1.Initialize(Game.stateManager.overworldState.GetSectorX);
            alliance1.rotationSpeed = 2.5f;
            alliance1.speed = 0.7f;

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
                        return ObjectiveIndex >= 7;
                    }));

                actions.Add(new TravelAction(rebelShips[i], freighter));
                switch (i)
                {
                    case 0:
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetSectorX.GetGameObject("Lavis")));
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

        private void SkipForward()
        {
            PopupHandler.SkipRealTimeMessages();
            ObjectiveIndex = 6;
            freighter.position = rebelShips[0].position + SkipWaitFreighterOffset;
        }
    }
}
