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
        private AllianceShip alliance2;

        private readonly Vector2 destination = new Vector2(94600, 100000);

        private FreighterShip freighter;

        public Main7_Retaliation(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.GetStation("Soelara Station"),
                Game.stateManager.overworldState.GetStation("Fortrun Station I"));

            alliance1 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance1.Initialize(Game.stateManager.overworldState.GetSectorX);

            alliance2 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance2.Initialize(Game.stateManager.overworldState.GetSectorX);

            rebelShips = new List<RebelShip>();

            for (int i = 0; i < numberOfRebelShips; i++)
            {
                CompositeAction actions = new SequentialAction();

                rebelShips.Add(new RebelShip(Game, Game.stateManager.shooterState.spriteSheet));
                rebelShips[i].Initialize();
                rebelShips[i].position = new Vector2(destination.X + (i * 50), destination.Y);
                rebelShips[i].collisionEvent = null;

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
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetStation("Rebel Station 2")));
                        break;

                    case 2:
                        actions.Add(new TravelAction(rebelShips[i], Game.stateManager.overworldState.GetPlanet("New Norrland")));
                        break;
                }
                
                rebelShips[i].AIManager = actions;
            }

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebelShips[0],
                new EventTextCapsule(GetEvent((int)EventID.ToMeetingPoint), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebelShips[0],
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

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], rebelShips[0], 400));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebelShips[0],
                delegate
                {
                    StartFreighter();
                },
                delegate { },
                delegate { return true; },
                delegate { return false; }));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], rebelShips[0],
                new List<string> { GetEvent((int)EventID.AtMeetingPoint1).Text, GetEvent((int)EventID.AtMeetingPoint2).Text,
                    GetEvent((int)EventID.AtMeetingPoint3).Text, GetEvent((int)EventID.AtMeetingPoint4).Text },
                3000, 1000));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], freighter,
                delegate { },
                delegate { },
                delegate
                {
                    return Vector2.Distance(rebelShips[0].position, freighter.position) < 500;
                },
                delegate { return false; }));

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], freighter,
                delegate { },
                delegate { },
                delegate 
                { 
                    return CollisionDetection.IsRectInRect(rebelShips[0].Bounds, freighter.Bounds);
                },
                delegate { return false; }));
            
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], freighter,
                "PirateAmbush", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel), null, EventTextCanvas.MessageBox)));
            
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Rebel Station 1"),
                delegate
                {
                    freighter.Destroy();
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        alliance1, destination + new Vector2(-600, 0), "PirateLevel2", Game.player);
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddOverworldShip(
                        alliance2, destination + new Vector2(600, 0), "PirateLevel3", Game.player);
                },
                delegate
                {
            
                },
                delegate
                {
                    return (CollisionDetection.IsPointInsideCircle(Game.player.position, 
                        Game.stateManager.overworldState.GetStation("Rebel Station 1").position, 1000));
                },
                delegate
                {
                    return false;
                }));
            
            objectives.Add(new ArriveAtLocationObjective(Game, this,
                ObjectiveDescriptions[0], Game.stateManager.overworldState.GetStation("Rebel Station 1")));
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
            
            if (ObjectiveIndex == 5)
            {
                //PirateShip.FollowPlayer = true;
            }
            else
            {
                //PirateShip.FollowPlayer = false;
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

        private void StartFreighter()
        {
            Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                freighter, Game.stateManager.overworldState.GetStation("Soelara Station").position);
        }
    }
}
