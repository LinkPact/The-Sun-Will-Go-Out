using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class NewFirstMission: Mission
    {
        private enum EventID
        {
            Introduction,
            Scouting,
            PirateAttack,
            ReturnToBorder,
            Allies,
            EngagePirates,
            ReturnToBorderAgain
        }

        public NewFirstMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                new EventTextCapsule(GetEvent((int)EventID.Scouting),
                    null,
                    EventTextCanvas.MessageBox)));

            Objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                "FirstMissionLevel", LevelStartCondition.Immediately,
                new EventTextCapsule(GetEvent((int)EventID.PirateAttack),
                    null,
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                "PirateLevel7", LevelStartCondition.Immediately,
                new EventTextCapsule(
                    GetEvent((int)EventID.ReturnToBorder),
                    null,
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[1],
                Game.stateManager.overworldState.GetStation("Border Station"),
                new EventTextCapsule(
                    GetEvent((int)EventID.Allies),
                    null,
                    EventTextCanvas.BaseState)));

            Objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                new EventTextCapsule(
                    GetEvent((int)EventID.EngagePirates),
                    null,
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0],
                Game.stateManager.overworldState.GetMiningOutpost.GetGameObject("Mining Asteroids"),
                "PirateLevel8", LevelStartCondition.Immediately,
                new EventTextCapsule(
                    GetEvent((int)EventID.ReturnToBorderAgain),
                    null,
                    EventTextCanvas.MessageBox)));

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[2],
                Game.stateManager.overworldState.GetStation("Border Station")));
                
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

        public override void MissionLogic()
        {
            base.MissionLogic();

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
