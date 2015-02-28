using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class Main4_Infiltration : Mission
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

        private List<OverworldShip> rebelShips1;
        private List<OverworldShip> rebelShips2;
        private List<OverworldShip> allianceShips;
        private readonly int numberOfRebelShips = 3;
        private readonly int numberOfAllianceShips = 3;
        private float time;

        public Main4_Infiltration(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
            isMainMission = true; 
        }

        public override void Initialize()
        {
            base.Initialize();

            rebelShips1 = new List<OverworldShip>();
            rebelShips2 = new List<OverworldShip>();
            allianceShips = new List<OverworldShip>();

            InitializeRebelShips1();
            InitializeRebelShips2();
            InitializeAllianceShips();

            SetDestinations();
            SetupObjectives();
            RestartAfterFail();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
            AddShips(rebelShips1);
            AddShips(allianceShips);
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
                    AddShips(rebelShips1);
                    AddShips(allianceShips);
                    break;

                default:
                    break;
            }
        }

        public override void OnReset()
        {
            InitializeRebelShips1();
            InitializeRebelShips2();
            InitializeAllianceShips();
            SetDestinations();
            SetupObjectives();

            base.OnReset();
        }

        public override void OnFailed()
        {
            base.OnFailed();

            RemoveShips(allianceShips);
            RemoveShips(rebelShips1);
            RemoveShips(rebelShips2);

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

        private void InitializeAllianceShips()
        {
            for (int i = 0; i < numberOfAllianceShips; i++)
            {
                allianceShips.Add(new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet));
                allianceShips[i].Initialize();
                allianceShips[i].RemoveOnStationEnter = false;
                allianceShips[i].position = new Vector2(
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.X - 500 + (i * 50),
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.Y - 200);
                allianceShips[i].AIManager = new WaitAction(allianceShips[i], delegate { return false; });
                allianceShips[i].SaveShip = false;
            }
        }

        private void InitializeRebelShips1()
        {
            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips1.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips1[i].Initialize();
                rebelShips1[i].position = new Vector2(98500 + (i * 50), 77000);
                rebelShips1[i].RemoveOnStationEnter = false;
                rebelShips1[i].collisionEvent = new RemoveOnCollisionEvent(Game, rebelShips1[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base"));
                rebelShips1[i].SaveShip = false;

                actions.Add(new WaitAction(rebelShips1[i],
                    delegate
                    {
                        return ObjectiveIndex >= 3;
                    }));

                actions.Add(new TravelAction(rebelShips1[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base")));

                rebelShips1[i].AIManager = actions;
            }
        }

        private void InitializeRebelShips2()
        {
            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips2.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips2[i].Initialize();
                rebelShips2[i].RemoveOnStationEnter = false;
                rebelShips2[i].position = new Vector2(
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.X - 500 + (i * 50),
                    Game.stateManager.overworldState.GetStation("Lavis Station").position.Y - 200);
                rebelShips2[i].collisionEvent = new RemoveOnCollisionEvent(Game, rebelShips2[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base"));
                rebelShips2[i].SaveShip = false;

                actions.Add(new WaitAction(rebelShips2[i],
                    delegate
                    {
                        return ObjectiveIndex >= 8;
                    }));

                actions.Add(new TravelAction(rebelShips2[i],
                    Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Base")));

                rebelShips2[i].AIManager = actions;
            }
        }

        private void AddShips(List<OverworldShip> ships)
        {
            foreach (OverworldShip ship in ships)
            {
                Game.stateManager.overworldState.AddOverworldObject(ship);
            }
        }

        private void RemoveShips(List<OverworldShip> ships)
        {
            foreach (OverworldShip ship in ships)
            {
                Game.stateManager.overworldState.RemoveOverworldObject(ship);
                ship.IsDead = true;
            }
        }

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            GameObjectOverworld rebelBase = Game.stateManager.overworldState.GetStation("Rebel Base");

            destinations.Add(rebelShips1[1]);
            destinations.Add(rebelShips1[1]);
            destinations.Add(rebelShips1[1]);
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
                    RemoveShips(allianceShips);
                    AddShips(rebelShips2);
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
