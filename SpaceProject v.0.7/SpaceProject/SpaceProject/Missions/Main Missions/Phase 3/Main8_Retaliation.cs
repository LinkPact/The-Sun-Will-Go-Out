using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main8_Retaliation : Mission
    {
        private enum EventID
        {
            StartTraveling,
            WaitForFreighter,
            OutOfRange,
            FreighterLeft,
            FlavorText1,
            FlavorText2,
            FlavorText3,
            AttackFreighter,
            FreighterLevelStart,
            ReturnToRebelBase
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

        public Main8_Retaliation(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            rebel1 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel1.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                destination + new Vector2(-50, 0));

            rebel2 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel2.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                destination);

            rebel3 = new AllyShip(Game, Game.stateManager.shooterState.spriteSheet, ShipType.Rebel);
            rebel3.Initialize(null, Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                destination + new Vector2(50, 0));

            alliance1 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance1.Initialize(Game.stateManager.overworldState.GetSectorX);

            alliance2 = new AllianceShip(Game, Game.stateManager.shooterState.spriteSheet);
            alliance2.Initialize(Game.stateManager.overworldState.GetSectorX);

            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);

            objectives.Add(new FollowObjective(Game, this, ObjectiveDescriptions[0],
                rebel2,
                new EventTextCapsule(GetEvent((int)EventID.WaitForFreighter), null, EventTextCanvas.MessageBox),
                GetEvent((int)EventID.StartTraveling).Text,
                Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                new Vector2(50, 50),
                600,
                GetEvent((int)EventID.OutOfRange).Text,
                rebel1, rebel2, rebel3));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], freighter,
                new EventTextCapsule(GetEvent((int)EventID.AttackFreighter), null, EventTextCanvas.MessageBox),
                delegate
                {
                    Game.messageBox.DisplayMessage(GetEvent((int)EventID.FreighterLeft).Text);
                    
                    freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                                Game.stateManager.overworldState.GetPlanet("Soelara"),
                                Game.stateManager.overworldState.GetStation("Fotrun Station I"));
                    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                        freighter, Game.stateManager.overworldState.GetPlanet("Soelara").position);

                    message1Time = StatsManager.PlayTime.GetFutureOverworldTime(10000);
                    message2Time = StatsManager.PlayTime.GetFutureOverworldTime(20000);
                    message3Time = StatsManager.PlayTime.GetFutureOverworldTime(30000);
                },
                delegate
                {
                    if (message1Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message1Time))
                    {
                        message1Time = -1;
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.FlavorText1).Text);
                    }

                    if (message2Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message2Time))
                    {
                        message2Time = -1;
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.FlavorText2).Text);
                    }

                    if (message3Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message3Time))
                    {
                        message3Time = -1;
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.FlavorText3).Text);
                    }

                    if (!CollisionDetection.IsPointInsideCircle(Game.player.position, rebel1.position, 600)
                        && outOfRangeTimer <= 0)
                    {
                        outOfRangeTimer = 150;
                        Game.messageBox.DisplayMessage(GetEvent((int)EventID.OutOfRange).Text);
                    }

                    if (outOfRangeTimer > 0)
                    {
                        outOfRangeTimer--;

                        if (outOfRangeTimer == 149)
                        {
                            Game.player.InitializeHyperSpeedJump(rebel1.position, false);
                        }

                        if (outOfRangeTimer < 1)
                        {
                            outOfRangeTimer = -100;
                        }
                    }
                },
                delegate
                {
                    return CollisionDetection.IsPointInsideCircle(rebel1.position, freighter.position, 300);
                },
                delegate
                {
                    return false;
                }));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], freighter,
                new EventTextCapsule(GetEvent((int)EventID.FreighterLevelStart), null, EventTextCanvas.MessageBox),
                delegate
                {
                    rebel1.ResetArrived();
                    rebel2.ResetArrived();
                    rebel3.ResetArrived();

                    rebel1.destinationPlanet = freighter;
                    rebel2.destinationPlanet = freighter;
                    rebel3.destinationPlanet = freighter;

                    rebel1.Start();
                    rebel2.Start();
                    rebel3.Start();
                },
                delegate
                {
                },
                delegate
                {
                    return (CollisionDetection.IsRectInRect(rebel1.Bounds, freighter.Bounds)
                        || CollisionDetection.IsRectInRect(rebel2.Bounds, freighter.Bounds)
                        || CollisionDetection.IsRectInRect(rebel3.Bounds, freighter.Bounds));
                },
                delegate
                {
                    return false;
                }));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], freighter,
                "PirateLevel1", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.ReturnToRebelBase), null, EventTextCanvas.MessageBox)));

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

            freighterStartTime = StatsManager.PlayTime.GetFutureOverworldTime(freighterStartDelay);
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

            //if (freighterStartTime > 0
            //    && StatsManager.PlayTime.HasOverworldTimePassed(freighterStartTime))
            //{
            //    Game.messageBox.DisplayMessage("The freighter has left Soelara, we have to hurry!");
            //
            //    freighterStartTime = -1;
            //
            //    freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
            //                Game.stateManager.overworldState.GetPlanet("Soelara"),
            //                Game.stateManager.overworldState.GetStation("Fotrun Station I"));
            //    Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
            //        freighter, Game.stateManager.overworldState.GetPlanet("Soelara").position);
            //}

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
                PirateShip.FollowPlayer = true;
            }
            else
            {
                PirateShip.FollowPlayer = false;
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
