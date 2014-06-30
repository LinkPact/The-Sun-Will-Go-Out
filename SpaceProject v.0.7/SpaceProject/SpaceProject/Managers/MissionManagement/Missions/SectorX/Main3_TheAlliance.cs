using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main3_TheAlliance : Mission
    {
        private int tempTimer = 0;

        private bool died;
        private int tempTimer2;

        private Vector2 savedPos = Vector2.Zero;

        public Main3_TheAlliance(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[13, 3];

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText5", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText6", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText7", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText8", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText9", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");
            EventArray[4, 0] = configFile.GetPropertyAsString(section, "EventText5", "");
            EventArray[5, 0] = configFile.GetPropertyAsString(section, "EventText6", "");
            EventArray[6, 0] = configFile.GetPropertyAsString(section, "EventText7", "");
            EventArray[7, 0] = configFile.GetPropertyAsString(section, "EventText8", "");
            
            EventArray[8, 0] = configFile.GetPropertyAsString(section, "EventText9", "");
            EventArray[8, 1] = configFile.GetPropertyAsString(section, "EventText9Response1", "");
            EventArray[8, 2] = configFile.GetPropertyAsString(section, "EventText9Response2", "");

            EventArray[9, 0] = configFile.GetPropertyAsString(section, "EventText10", "");
            EventArray[10, 0] = configFile.GetPropertyAsString(section, "EventText11", "");
            EventArray[11, 0] = configFile.GetPropertyAsString(section, "EventText12", "");
            EventArray[12, 0] = configFile.GetPropertyAsString(section, "EventText13", "");
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        {
        }

        public override void OnReset()
        {
            base.OnReset();
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            tempTimer--;

            if (progress == 0 
                && GameStateManager.currentState.Equals("PlanetState") 
                && missionHelper.IsTextCleared())
            {
                ObjectiveIndex = 1;
                progress = 1;
            }

            if (progress == 1)
            {
                if (CollisionDetection.IsPointInsideCircle(Game.player.position,
                    Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                    400) && savedPos == Vector2.Zero)
                {
                    savedPos = Game.player.position;
                }

                else if (!CollisionDetection.IsPointInsideCircle(Game.player.position,
                    Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                    400))
                {
                    savedPos = Vector2.Zero;
                }

                if (CollisionDetection.IsPointInsideCircle(Game.player.position,
                    Game.stateManager.overworldState.getStation("Fotrun Station II").position,
                    300))
                {
                    Game.messageBox.DisplayMessage(EventArray[0, 0]);
                    missionHelper.StartLevel("Main_TheAlliancelvl");
                    ObjectiveIndex = 2;
                    progress = 2;
                }
            }

            if (progress == 2 && missionHelper.IsLevelCompleted("Main_TheAlliancelvl"))
            {
                ObjectiveIndex = 3;
                progress = 3;
            }

            else if (progress == 2  
                && !died 
                && missionHelper.IsLevelFailed("Main_TheAlliancelvl"))
            {
                died = true;
                tempTimer2 = 5;

                if (savedPos != Vector2.Zero)
                {
                    Game.player.position = savedPos;
                }

                savedPos = Vector2.Zero;
                Game.player.speed = 0;
            }

            if (died && GameStateManager.currentState.Equals("OverworldState"))
            {
                tempTimer2--;

                if (tempTimer2 < 0)
                {
                    died = false;

                    Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                    Game.messageBox.DisplayMessage(EventArray[12, 0]);
                    ObjectiveIndex = 1;
                    progress = 1;
                }
            }

            if (progress == 3 && GameStateManager.currentState == "OverworldState")
            {
                Game.messageBox.DisplayMessage("You fight off the last pirates. You better enter the station and see what damage they caused.");
                ObjectiveIndex = 4;
                progress = 4;
            }

            // Player enters Fotrun station II
            if (progress == 4 && missionHelper.IsPlayerOnStation("Fotrun Station II"))
            {
                missionHelper.ShowEvent(new List<int> { 1, 2, 3 });
                progress = 5;
                Game.stateManager.overworldState.getStation("Fotrun Station II").Abandoned = true;
            }

            // Player drops of man at Fotrun Station I
            if (progress == 5 && missionHelper.IsPlayerOnStation("Fotrun Station I"))
            {
                missionHelper.ShowEvent(4);
                progress = 6;
            }

            if (progress == 6 && GameStateManager.currentState == "OverworldState")
            {
                tempTimer = 100;
                ObjectiveIndex = 7;
                progress = 7;
            }

            if (tempTimer == 1)
            {
                Game.messageBox.DisplayMessage(EventArray[5, 0]);
            }

            if (progress == 7 && missionHelper.IsPlayerOnPlanet("Highfence"))
            {
                missionHelper.ShowEvent(new List<int> { 6, 7, 8 });
                missionHelper.ShowResponse(8, new List<int> { 1, 2 });
            }

            if (MissionResponse != 0)
            {
                Game.stateManager.planetState.SubStateManager.MissionMenuState.ActiveMission = this;

                switch (MissionResponse)
                {
                    case 1:
                        {
                            missionHelper.ShowEvent(new List<int> { 9, 10 });
                            MissionManager.MarkMissionAsCompleted(this.MissionName);
                            missionHelper.ClearResponseText();
                            break;
                        }

                    case 2:
                        {
                            missionHelper.ShowEvent(11);
                            MissionManager.MarkMissionAsCompleted(this.MissionName);
                            missionHelper.ClearResponseText();
                            break;
                        }
                }
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
