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
                    //Game.messageBox.DisplayMessage(EventList[0].Value);
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
                    //Game.messageBox.DisplayMessage(EventList[6].Value);
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
                missionHelper.ShowEvent(EventList[0].Value);
                progress = 5;
                Game.stateManager.overworldState.getStation("Fotrun Station II").Abandoned = true;
            }
            
            // Player drops of man at Fotrun Station I
            if (progress == 5 && missionHelper.IsPlayerOnStation("Fotrun Station I"))
            {
                missionHelper.ShowEvent(EventList[1].Value);
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
                //Game.messageBox.DisplayMessage(EventList[2].Value);
            }
            
            if (progress == 7 && missionHelper.IsPlayerOnPlanet("Highfence"))
            {
                missionHelper.ShowEvent(EventList[3].Value);
                //missionHelper.ShowResponse(8, new List<int> { 1, 2 });
            }
            
            if (MissionResponse != 0)
            {
                Game.stateManager.planetState.SubStateManager.MissionMenuState.ActiveMission = this;
            
                switch (MissionResponse)
                {
                    case 1:
                        {
                            missionHelper.ShowEvent(EventList[4].Value);
                            MissionManager.MarkMissionAsCompleted(this.MissionName);
                            missionHelper.ClearResponseText();
                            break;
                        }
            
                    case 2:
                        {
                            missionHelper.ShowEvent(EventList[5].Value);
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
