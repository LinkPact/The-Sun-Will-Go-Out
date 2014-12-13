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

        private AllyShip rebel1;
        private AllyShip rebel2;
        private AllyShip rebel3;
        
        private AllianceShip alliance1;
        private AllianceShip alliance2;

        private readonly Vector2 destination = new Vector2(94600, 100000);
        private readonly int freighterStartDelay = 20000;

        private FreighterShip freighter;
        private float freighterStartTime;
        
        private float message1Time;
        private float message2Time;
        private float message3Time;
        
        private int outOfRangeTimer;

        public Main7_Retaliation(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();
            
            rebel1 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel1.Initialize(null, destination + new Vector2(-50, 0), destination + new Vector2(-50, 0));
            
            rebel2 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel2.Initialize(null, destination, destination);
            
            rebel3 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel3.Initialize(null, destination + new Vector2(50, 0), destination + new Vector2(50, 0));
            
            alliance1 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance1.Initialize(Game.stateManager.overworldState.GetSectorX);
            
            alliance2 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance2.Initialize(Game.stateManager.overworldState.GetSectorX);
            
            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);

            Objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], rebel1,
                new EventTextCapsule(GetEvent((int)EventID.ToMeetingPoint), null, EventTextCanvas.MessageBox),
                delegate { },
                delegate { },
                delegate { return (GameStateManager.currentState.ToLower().Equals("overworldstate")); },
                delegate { return false; }));

            Objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], rebel1, 400));

            Objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], rebel1,
                new List<string> { GetEvent((int)EventID.AtMeetingPoint1).Text, GetEvent((int)EventID.AtMeetingPoint2).Text,
                    GetEvent((int)EventID.AtMeetingPoint3).Text, GetEvent((int)EventID.AtMeetingPoint4).Text },
                3000, 1000));
            
            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], freighter,
                "PirateLevel1", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.AfterLevel), null, EventTextCanvas.MessageBox)));
            
            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetStation("Rebel Station 1"),
                delegate
                {
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

            if (freighterStartTime > 0
                && StatsManager.PlayTime.HasOverworldTimePassed(freighterStartTime))
            {
                //Game.messageBox.DisplayMessage("The freighter has left Soelara, we have to hurry!");
            
                freighterStartTime = -1;
            
                freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                            Game.stateManager.overworldState.GetPlanet("Soelara"),
                            Game.stateManager.overworldState.GetStation("Fortrun Station I"));
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    freighter, Game.stateManager.overworldState.GetPlanet("Soelara").position);
            }

            if (ObjectiveIndex == 4
                && !freighter.IsDead)
            {
                freighter.Destroy();
                rebel1.Remove();
                rebel2.Remove();
                rebel3.Remove();
            }
            
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
    }
}
