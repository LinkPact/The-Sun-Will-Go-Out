using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Change class-name to the name of the mission 
    class MainMissionOne : Mission
    {
        private bool startLevel;

        public MainMissionOne(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            AcceptText = new string[3];
        }

        public override void Initialize()
        {
            base.Initialize();

            //AcceptText[1] = configFile.GetPropertyAsString(ConfigSection, "Accept2", "");
            //AcceptText[2] = configFile.GetPropertyAsString(ConfigSection, "Accept3", "");
        }

        //If you want anything specific to happen when you start the mission, 
        //for example add an item to the ship inventory, add it here
        public override void StartMission()
        {
            ObjectiveIndex = 0;
        }

        public override void OnLoad()
        { }

        //Add logic for the mission here
        public override void MissionLogic()
        {
            base.MissionLogic();

            //if (GameStateManager.currentState == "PlanetState" &&
            //    Game.stateManager.planetState.Planet is Telmun)
            //{
            //    if (ObjectiveIndex == 0)
            //    {
            //        MissionMenuState.ActiveMission = this;
            //
            //        missionHelper.ShowEvent(new List<int>() { 0, 1 });
            //        missionHelper.ShowResponse(1, new List<int>() { 1, 2 });
            //    }
            //
            //    if (MissionResponse != 0)
            //    {
            //        switch (MissionResponse)
            //        {
            //            case 1:
            //                missionHelper.ShowEvent(3);
            //                break;
            //
            //            case 2:
            //                missionHelper.ShowEvent(2);
            //                break;
            //        }
            //
            //        ResponseBuffer.Clear();
            //    }
            //
            //    if (startLevel && EventBuffer.Count <= 0 &&
            //        Game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            //    {
            //        Game.stateManager.shooterState.BeginLevel("EscortLevel");
            //        startLevel = false;
            //    }
            //
            //    if (Game.stateManager.shooterState.GetLevel("EscortLevel").IsObjectiveCompleted)
            //    {
            //        missionHelper.ShowEvent(4);
            //        progress = 1;
            //
            //        MissionManager.MarkMissionAsCompleted(this.MissionName);
            //    }
            //
            //    if (Game.stateManager.shooterState.GetLevel("EscortLevel").IsGameOver)
            //    {
            //        EventBuffer.Clear();
            //        ResponseBuffer.Clear();
            //
            //        CurrentObjectiveDescription = ObjectiveDescriptions[0];
            //
            //        missionHelper.ShowEvent(new List<int>() { 0, 1 });
            //        missionHelper.ShowResponse(1, new List<int>() { 1, 2 });
            //    }
            //}
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
