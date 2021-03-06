﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class ShootingLevelObjective : Objective
    {
        private string level;
        private bool levelStarted;
        private LevelStartCondition levelStartCondition;

        public ShootingLevelObjective(Game1 game, Mission mission, String description,
            String level, LevelStartCondition levelStartCondition) :
            base(game, mission, description)
        {
            Setup(level, levelStartCondition);
        }

        public ShootingLevelObjective(Game1 game, Mission mission, String description,
            String level, LevelStartCondition levelStartCondition,
            EventTextCapsule eventTextCapsule) :
            base(game, mission, description)
        {
            Setup(level, levelStartCondition);

            eventTextCanvas = eventTextCapsule.EventTextCanvas;
            objectiveCompletedEventText = eventTextCapsule.CompletedText;
            objectiveFailedEventText = eventTextCapsule.FailedText;

            if (eventTextCapsule.Portraits.Count > 0)
            {
                SetupPortraits(eventTextCapsule.Portraits, eventTextCapsule.PortraitTriggers);
            }
        }

        private void Setup(String level, LevelStartCondition levelStartCondition)
        {
            this.level = level;
            this.levelStartCondition = levelStartCondition;
        }

        public override void OnActivate()
        {
            base.OnActivate();

            mission.MissionHelper.StartLevelAfterCondition(level, levelStartCondition);
        }

        public override void Reset()
        {
            base.Reset();

            game.stateManager.shooterState.GetLevel(level).Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(PlayTime playTime)
        {
            base.Update(playTime);

            if (!levelStarted)
            {
                mission.MissionHelper.StartLevelAfterCondition(level, levelStartCondition);
                levelStarted = true;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override bool Completed()
        {
            if (isOnCompletedCalled)
            {
                return true;
            }

            return (game.stateManager.shooterState.GetLevel(level).IsObjectiveCompleted && !GameStateManager.currentState.Equals("ShooterState"));
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override bool Failed()
        {
            return (game.stateManager.shooterState.GetLevel(level).IsObjectiveFailed
                && GameStateManager.currentState != "ShooterState");
        }

        public override void OnFailed()
        {
            base.OnFailed();
        }
    }
}
