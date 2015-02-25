﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class Main6_Infiltration : Mission
    {
        enum EventID
        {
            Introduction,
            OutsideFortrun,
            ToMeetingPoint,
            AtMeeting,
            AfterMeeting1,
            AfterMeeting2,
            ToLavis,
            Level1Begins,
            DuringLevel1,
            Level2Begins,
            DuringLevel2,
            AfterLevel2,
            ToRebelBase
        }

        private List<OverworldShip> rebelShips;
        private List<OverworldShip> allianceShips;
        private readonly int numberOfRebelShips = 3;
        private readonly int numberOfAllianceShips = 3;
        private float time;

        public Main6_Infiltration(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            rebelShips = new List<OverworldShip>();
            allianceShips = new List<OverworldShip>();

            InitializeOverworldShips();

            SetDestinations();
            SetupObjectives();
            RestartAfterFail();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
            AddOverworldShips();
        }

        public override void OnLoad()
        { }

        public override void OnReset()
        {
            InitializeOverworldShips();
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            RemoveAllianceShips();
            RemoveRebelShips();

            Game.messageBox.DisplayMessage("You failed to dispatch the treacherous alliance attack fleet leader and gain the trust of the rebels. Go back to Fortrun Station 1 to try again.", false);
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

        private void AddOverworldShips()
        {
            foreach (OverworldShip ship in rebelShips)
            {
                Game.stateManager.overworldState.AddOverworldObject(ship);
            }

            foreach (OverworldShip ship in allianceShips)
            {
                Game.stateManager.overworldState.AddOverworldObject(ship);
            }
        }

        private void InitializeOverworldShips()
        {
            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips[i].Initialize();
                rebelShips[i].position = new Vector2(98500 + (i * 50), 77000);
                rebelShips[i].RemoveOnStationEnter = false;
                rebelShips[i].collisionEvent = new RemoveOnCollisionEvent(Game, rebelShips[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base"));

                actions.Add(new WaitAction(rebelShips[i],
                    delegate
                    {
                        return ObjectiveIndex >= 3;
                    }));

                actions.Add(new TravelAction(rebelShips[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base")));

                rebelShips[i].AIManager = actions;
            }

            for (int i = 0; i < numberOfAllianceShips; i++)
            {
                allianceShips.Add(new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet));
                allianceShips[i].Initialize();
                allianceShips[i].RemoveOnStationEnter = false;
                allianceShips[i].position = new Vector2(
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.X - 500 + (i * 50),
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.Y - 200);
                allianceShips[i].AIManager = new WaitAction(allianceShips[i], delegate { return false; });
            }
        }

        private void RemoveAllianceShips()
        {
            foreach (OverworldShip ship in allianceShips)
            {
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
                ship.IsDead = true;
            }
        }

        private void RemoveRebelShips()
        {
            foreach (OverworldShip ship in allianceShips)
            {
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
                ship.IsDead = true;
            }
        }

        private void AddRebelShips()
        {
            rebelShips.Clear();

            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips[i].Initialize();
                rebelShips[i].RemoveOnStationEnter = false;
                rebelShips[i].position = new Vector2(
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.X - 500 + (i * 50),
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.Y - 200);
                rebelShips[i].collisionEvent = new RemoveOnCollisionEvent(Game, rebelShips[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base"));

                actions.Add(new WaitAction(rebelShips[i],
                    delegate
                    {
                        return ObjectiveIndex >= 8;
                    }));

                actions.Add(new TravelAction(rebelShips[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base")));

                rebelShips[i].AIManager = actions;
            }

            foreach (OverworldShip ship in rebelShips)
            {
                Game.stateManager.overworldState.AddOverworldObject(ship);
            }
        }

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld rebelBase = Game.stateManager.overworldState.GetStation("Rebel Base");

            destinations.Add(rebelShips[1]);
            destinations.Add(rebelShips[1]);
            destinations.Add(rebelShips[1]);
            destinations.Add(allianceShips[1]);
            destinations.Add(allianceShips[1]);
            destinations.Add(allianceShips[1]);
            destinations.Add(allianceShips[1]);
            destinations.Add(allianceShips[1]);
            destinations.Add(rebelBase);
            destinations.Add(rebelBase);
            destinations.Add(rebelBase);
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[0],
                new EventTextCapsule(GetEvent((int)EventID.OutsideFortrun), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return GameStateManager.currentState.ToLower().Equals("overworldstate"); },
                delegate { return false; }));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], destinations[1],
                GetEvent((int)EventID.ToMeetingPoint).Text, 3000, 6000));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], destinations[2],
                300, new EventTextCapsule(GetEvent((int)EventID.AtMeeting), null, EventTextCanvas.MessageBox)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1], destinations[3],
                GetEvent((int)EventID.AfterMeeting1).Text, 3000, 5000,
                new EventTextCapsule(GetEvent((int)EventID.AfterMeeting2), null, EventTextCanvas.MessageBox)));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[1], destinations[4],
                GetEvent((int)EventID.ToLavis).Text, 3000, 5000));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[1], destinations[5],
                300, new EventTextCapsule(GetEvent((int)EventID.Level1Begins), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1], destinations[6],
                "Infiltration1", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.Level2Begins), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[1], destinations[7],
                "Infiltration2", LevelStartCondition.Immediately));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[2], destinations[8],
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel2), null, EventTextCanvas.MessageBox),
                delegate
                {
                    RemoveAllianceShips();
                    AddRebelShips();
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], destinations[9],
                new EventTextCapsule(GetEvent((int)EventID.ToRebelBase), null, EventTextCanvas.MessageBox),
                delegate
                {
                    time = StatsManager.PlayTime.GetFutureOverworldTime(2000);
                },
                delegate { },
                delegate { return StatsManager.PlayTime.HasOverworldTimePassed(time); },
                delegate { return false; }));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2], destinations[10]));
        }
    }
}
