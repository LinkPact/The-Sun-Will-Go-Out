using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class ShootingLevelObjective : Objective
    {
        private string level;
        private bool levelStarted;
        private LevelStartCondition levelStartCondition;

        public ShootingLevelObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, String level, LevelStartCondition levelStartCondition) :
            base(game, mission, description, destination)
        {
            this.level = level;
            this.levelStartCondition = levelStartCondition;
            mission.MissionHelper.StartLevelAfterCondition(level, levelStartCondition);
        }

        public ShootingLevelObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, String level, LevelStartCondition levelStartCondition, List<int> endEventIndices) :
            base(game, mission, description, destination)
        {
            this.level = level;
            this.levelStartCondition = levelStartCondition;

            foreach (Int32 i in endEventIndices)
            {
                this.endEventIndices.Add(i);
            }
        }

        public ShootingLevelObjective(Game1 game, Mission mission, String description,
            GameObjectOverworld destination, String level, LevelStartCondition levelStartCondition,
            List<int> endEventIndices, EventTextFormat eventTextFormat) :
            base(game, mission, description, destination)
        {
            this.level = level;
            this.levelStartCondition = levelStartCondition;
            this.eventTextFormat = eventTextFormat;

            foreach (Int32 i in endEventIndices)
            {
                this.endEventIndices.Add(i);
            }
        }

        public override void OnActivate()
        {
            base.OnActivate();

            mission.MissionHelper.StartLevelAfterCondition(level, levelStartCondition);
        }

        public override void Reset()
        {
            base.Reset();

            //game.stateManager.shooterState.Initialize();
            //game.stateManager.shooterState.GetLevel(level).ResetLevel();
            game.stateManager.shooterState.GetLevel(level).Initialize();
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update()
        {
            base.Update();

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

            return (game.stateManager.shooterState.GetLevel(level).IsObjectiveCompleted);
        }

        public override void OnCompleted()
        {
            base.OnCompleted();
        }

        public override bool Failed()
        {
            if (game.stateManager.shooterState.GetLevel(level).IsGameOver
                && GameStateManager.currentState != "ShooterState")
            {
                OnFailed();
                return true;
            }

            return false;
        }
    }
}
