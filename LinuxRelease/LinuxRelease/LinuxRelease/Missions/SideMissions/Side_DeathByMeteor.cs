using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class Side_DeathByMeteor : Mission
    {
        private enum EventID
        {
            LevelCleared = 0,
        }

        public Side_DeathByMeteor(Game1 Game, string section, Sprite spriteSheet, MissionID missionID) :
            base(Game, section, spriteSheet, missionID)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            AdvancedBeamWeapon advancedBeam = new AdvancedBeamWeapon(Game, ItemVariety.Regular);
            RewardItems.Add(advancedBeam);

            RestartAfterFail();

            SetDestinations();
            SetupObjectives();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        { }

        public override void OnFailed()
        {
            base.OnFailed();

            PopupHandler.DisplayPortraitMessage(PortraitID.Ente, "[Crazy scientist] \"Haha! Oh, I mean, sorry. Look what a mess you are. Of course you weren't able to make it through the level. But you are welcome to try again. If you do, maybe you too will be a legend one day.\"");
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

        protected override void SetDestinations()
        {
            destinations = new List<GameObjectOverworld>();

            destinations.Add(Game.stateManager.overworldState.GetPlanet("Peye"));
        }

        protected override void SetupObjectives()
        {
            objectives.Clear();

            objectives.Add(new ShootingLevelObjective(Game, this, ObjectiveDescriptions[0], "DeathByMeteor", LevelStartCondition.TextCleared,
                new EventTextCapsule(GetEvent((int)EventID.LevelCleared), null, EventTextCanvas.BaseState)));
        }
    }
}
