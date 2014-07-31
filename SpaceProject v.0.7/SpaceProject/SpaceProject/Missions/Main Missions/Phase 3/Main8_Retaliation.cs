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

        }

        private AllyShip rebel1;
        private AllyShip rebel2;
        private AllyShip rebel3;

        private readonly Vector2 destination = new Vector2(94600, 100000);
        private readonly int freighterStartDelay = 20000;

        private FreighterShip freighter;
        private float freighterStartTime;

        private float message1Time;
        private float message2Time;
        private float message3Time;

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

            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);

            objectives.Add(new FollowObjective(Game, this, ObjectiveDescriptions[0],
                rebel2,
                new EventTextCapsule(new EventText("Wait for the freighter to arrive."), null, EventTextCanvas.MessageBox),
                "",
                Game.stateManager.overworldState.GetRebelOutpost.GetGameObject("Rebel Station 1").position,
                new Vector2(50, 50),
                rebel1, rebel2, rebel3));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], freighter,
                new EventTextCapsule(new EventText("ATTACK"), null, EventTextCanvas.MessageBox),
                delegate
                {
                    message1Time = StatsManager.PlayTime.GetFutureOverworldTime(3000);
                    message2Time = StatsManager.PlayTime.GetFutureOverworldTime(6000);
                    message3Time = StatsManager.PlayTime.GetFutureOverworldTime(9000);
                },
                delegate
                {
                    if (message1Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message1Time))
                    {
                        message1Time = -1;
                        Game.messageBox.DisplayMessage("TEXT 1");
                    }

                    if (message2Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message2Time))
                    {
                        message2Time = -1;
                        Game.messageBox.DisplayMessage("TEXT 2");
                    }

                    if (message3Time > 0
                        && StatsManager.PlayTime.HasOverworldTimePassed(message3Time))
                    {
                        message3Time = -1;
                        Game.messageBox.DisplayMessage("TEXT 3");
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
                new EventTextCapsule(new EventText("KILL THE FREIGHTER"), null, EventTextCanvas.MessageBox),
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
                new EventTextCapsule(new EventText("FREIGHTER CAPTURED! Go back to rebel base."), null, EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], Game.stateManager.overworldState.GetStation("Rebel Station 1"),
                new EventTextCapsule(new EventText("Good job!"), null, EventTextCanvas.BaseState)));
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

            if (freighterStartTime > 0
                && StatsManager.PlayTime.HasOverworldTimePassed(freighterStartTime))
            {
                Game.messageBox.DisplayMessage("The freighter has left Soelara, we have to hurry!");

                freighterStartTime = -1;

                freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                            Game.stateManager.overworldState.GetPlanet("Soelara"),
                            Game.stateManager.overworldState.GetStation("Fotrun Station I"));
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
