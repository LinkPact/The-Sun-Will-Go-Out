using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main1_AColdWelcome: Mission
    {
        private enum EventID
        {
            Introduction,
            LevelCleared,
            Bonus
        }

        private Battlefield battlefield;
        private bool died;

        public Main1_AColdWelcome(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            battlefield = new Battlefield(Game, spriteSheet);
            battlefield.Initialize();

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], battlefield,
                new EventTextCapsule(new EventText("Avoid the astroids and collect resources."),
                null, EventTextCanvas.MessageBox)));

            objectives.Add(new CustomObjective(Game, this, ObjectiveDescriptions[0], battlefield,
                new EventTextCapsule(GetEvent((int)EventID.LevelCleared), null, EventTextCanvas.MessageBox),
                delegate
                {
                    missionHelper.StartLevel("FirstMissionLevel");
                },
                delegate
                {
                    if (GameStateManager.currentState.Equals("ShooterState") &&
                        Game.stateManager.shooterState.CurrentLevel.Name.Equals("FirstMissionLevel"))
                    {
                        if (Game.stateManager.shooterState.CurrentLevel.IsGameOver)
                        {
                            died = true;
                            ((FirstMissionLevel)Game.stateManager.shooterState.GetLevel("FirstMissionLevel")).SuppliesCount = 0;
                        }
                    }

                    if (died && GameStateManager.currentState.Equals("OverworldState"))
                    {
                        Game.messageBox.DisplayMessage("Fei Yan: Too bad. Try again! Return to Border Station if you need to repair your ship.", false);
                        died = false;
                        ObjectiveIndex = 0;
                        objectives[0].Reset();
                    }
                },
                new Func<Boolean>(delegate()
                {
                    return (missionHelper.IsLevelCompleted("FirstMissionLevel")
                        && GameStateManager.currentState.Equals("OverworldState"));
                }),
                new Func<Boolean>(delegate()
                {
                    return false;
                })));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Border Station"), new EventTextCapsule(
                null, null, EventTextCanvas.BaseState)));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(EventList[0].Key);
            //Game.stateManager.GotoStationSubScreen("Border Station", "Overview");

            Game.stateManager.overworldState.AddOverworldObject(battlefield);
        }

        public override void OnLoad()
        {
            Game.stateManager.overworldState.AddOverworldObject(battlefield);

            if (progress == 2)
            {
                objectiveDestination = battlefield;
            }

            else if (progress == 3)
            {
                objectiveDestination = Game.stateManager.overworldState.GetStation("Border Station");
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            //if (progress == 3 
            //    && missionHelper.IsPlayerOnStation("Border Station"))
            //{
            //    if (((FirstMissionLevel)Game.stateManager.shooterState.GetLevel("FirstMissionLevel")).SuppliesCount >= 6)
            //    {
            //        TitaniumResource titanium = new TitaniumResource(Game, 100);
            //        RewardItems.Add(titanium);
            //
            //        missionHelper.ShowEvent(EventArray[4, 0]);
            //    }
            //
            //    MissionManager.MarkMissionAsCompleted(this.MissionName);
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (ObjectiveIndex == 0
                && CollisionDetection.IsRectInRect(Game.player.Bounds, battlefield.Bounds))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, battlefield);
                Game.helper.DisplayText("Press 'Enter' to investigate battlefield..");
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
