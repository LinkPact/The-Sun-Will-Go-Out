using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main2_Rebels : Mission
    {
        int messageTimer = 0;

        private int rebelShipSpawnerDelay;
        private int numberOfRebelShips = 3;

        int levelProgression = 0;

        FreighterShip freighter;
        private float freighterHP;
        public float GetFreighterHP { get { return freighterHP; } }

        public Main2_Rebels(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
            EventArray = new string[7, 1];

            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText1", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText2", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText3", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText4", ""));
            Objectives.Add(configFile.GetPropertyAsString(section, "ObjectiveText5", ""));

            EventArray[0, 0] = configFile.GetPropertyAsString(section, "EventText1", "");
            EventArray[1, 0] = configFile.GetPropertyAsString(section, "EventText2", "");
            EventArray[2, 0] = configFile.GetPropertyAsString(section, "EventText3", "");
            EventArray[3, 0] = configFile.GetPropertyAsString(section, "EventText4", "");
            EventArray[4, 0] = configFile.GetPropertyAsString(section, "EventText5", "");
            EventArray[5, 0] = configFile.GetPropertyAsString(section, "EventText6", "");
            EventArray[6, 0] = configFile.GetPropertyAsString(section, "EventText7", "");
        }

        public override void Initialize()
        {
            base.Initialize();

            RestartAfterFail();

            freighterHP = 2000;

            freighter = new FreighterShip(Game, Game.stateManager.shooterState.spriteSheet);
            freighter.Initialize(Game.stateManager.overworldState.GetSectorX,
                Game.stateManager.overworldState.getPlanet("Highfence"),
                Game.stateManager.overworldState.getStation("Soelara Station"));
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;
        }

        public override void OnLoad()
        {
            // Player returns to overworld after speaking to Kamali
            if (progress == 2 || progress == 3)
            {
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    freighter, Game.stateManager.overworldState.getPlanet("Highfence").position);

                freighter.position = Game.stateManager.overworldState.getPlanet("Highfence").position - new Vector2(200, 0);

                freighter.Wait();

                ObjectiveIndex = 2;
                progress = 2;
            }
        }

        public override void OnReset()
        {
            base.OnReset();

            MissionManager.MarkMissionAsFailed(this.MissionName);
            ObjectiveIndex = 0;
            Game.stateManager.overworldState.RemoveOverworldObject(freighter);
            levelProgression = 0;
            progress = 0;
            numberOfRebelShips = 3;
            freighterHP = 1000;
            PirateShip.FollowPlayer = true;
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            // Player arrives at Highfence
            if (progress == 0 && missionHelper.IsPlayerOnPlanet("Highfence"))
            {
                missionHelper.ShowEvent(new List<int> { 0, 1, 2, 3 });
                ObjectiveIndex = 1;
                progress = 1;
            }

            // Player returns to overworld after speaking to Kamali
            if (progress == 1 && GameStateManager.currentState.Equals("OverworldState"))
            {
                Game.stateManager.overworldState.GetSectorX.shipSpawner.AddFreighterToSector(
                    freighter, Game.stateManager.overworldState.getPlanet("Highfence").position);

                freighter.position = Game.stateManager.overworldState.getPlanet("Highfence").position - new Vector2(200, 0);
                
                freighter.Wait();

                progress = 2;
            }

            // Turn on/off pirates following player during escort
            if (progress == 3 && PirateShip.FollowPlayer)
            {
                PirateShip.FollowPlayer = false;
            }

            else if (progress != 3 && !PirateShip.FollowPlayer)
            {
                PirateShip.FollowPlayer = true;
            }

            // Player talks to freighter to begin escort
            if (progress == 2 && GameStateManager.currentState.Equals("OverworldState") &&
                CollisionDetection.IsPointInsideRectangle(Game.player.position, freighter.Bounds) &&
                ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                Game.messageBox.DisplayMessage(EventArray[4, 0]);
                rebelShipSpawnerDelay = 1000;

                freighter.Start();

                ObjectiveIndex = 2;
                progress = 3;
            }

            // Escort mission begins
            if (progress == 3 && GameStateManager.currentState.Equals("OverworldState") &&
                numberOfRebelShips > 0)
            {
                rebelShipSpawnerDelay--;

                // Ready to spawn a new rebel ship
                if (rebelShipSpawnerDelay < 0)
                {
                    if (numberOfRebelShips == 3)
                    {
                        Game.messageBox.DisplayMessage(EventArray[5, 0]);
                        ObjectiveIndex = 3;
                    }

                    if (numberOfRebelShips == 3)
                    {
                        Game.stateManager.overworldState.GetSectorX.shipSpawner.AddRebelShip(
                            new Vector2(freighter.position.X - 650,
                                freighter.position.Y + 650), "SecondMissionlvl1", freighter);
                    }

                    else if (numberOfRebelShips == 2)
                    {
                        Game.stateManager.overworldState.GetSectorX.shipSpawner.AddRebelShip(
                            new Vector2(freighter.position.X - 650,
                                freighter.position.Y + 650), "SecondMissionlvl2", freighter);
                    }

                    else if (numberOfRebelShips == 1)
                    {
                        Game.stateManager.overworldState.GetSectorX.shipSpawner.AddRebelShip(
                            new Vector2(freighter.position.X - 650,
                                freighter.position.Y + 650), "SecondMissionlvl3", freighter);
                    }

                    numberOfRebelShips--;

                    if (numberOfRebelShips > 0)
                    {
                        rebelShipSpawnerDelay = 500;
                    }
                }
            }

            // Transfers freigter hp between levels
            if (progress == 3 && levelProgression == 0 &&
                GameStateManager.currentState.Equals("ShooterState") &&
                Game.stateManager.shooterState.CurrentLevel.Name == "SecondMissionlvl1")
            {
                ((SecondMissionLevel)Game.stateManager.shooterState.CurrentLevel).SetFreighterHP(freighterHP);
                levelProgression = 1;
            }

            if (progress == 3 && GameStateManager.currentState == "ShooterState" &&
                Game.stateManager.shooterState.GetLevel("SecondMissionlvl1").IsObjectiveCompleted)
            {
                freighterHP = ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl1")).GetFreighterHP();
            }

            if (progress == 3 && levelProgression == 1 &&
                GameStateManager.currentState.Equals("ShooterState") &&
                Game.stateManager.shooterState.CurrentLevel.Name == "SecondMissionlvl2")
            {
                ((SecondMissionLevel)Game.stateManager.shooterState.CurrentLevel).SetFreighterHP(freighterHP);
                levelProgression = 2;
            }

            if (progress == 3 && GameStateManager.currentState == "ShooterState" &&
                Game.stateManager.shooterState.GetLevel("SecondMissionlvl2").IsObjectiveCompleted)
            {
                freighterHP = ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl2")).GetFreighterHP();
            }

            if (progress == 3 && levelProgression == 2 &&
                GameStateManager.currentState.Equals("ShooterState") &&
                Game.stateManager.shooterState.CurrentLevel.Name == "SecondMissionlvl3")
            {
                ((SecondMissionLevel)Game.stateManager.shooterState.CurrentLevel).SetFreighterHP(freighterHP);
                levelProgression = 3;
            }

            if (progress == 3 && GameStateManager.currentState == "ShooterState" &&
                Game.stateManager.shooterState.GetLevel("SecondMissionlvl3").IsObjectiveCompleted)
            {
                freighterHP = ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl3")).GetFreighterHP();
            }

            // Freighter is destroyed
            if (GameStateManager.currentState.Equals("ShooterState") && 
                ((Game.stateManager.shooterState.CurrentLevel.Name.Equals("SecondMissionlvl1") &&
                ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl1")).GetFreighterHP() <= 0) || 
                (Game.stateManager.shooterState.CurrentLevel.Name.Equals("SecondMissionlvl2") &&
                ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl2")).GetFreighterHP() <= 0) ||
                (Game.stateManager.shooterState.CurrentLevel.Name.Equals("SecondMissionlvl3") &&
                ((SecondMissionLevel)Game.stateManager.shooterState.GetLevel("SecondMissionlvl3")).GetFreighterHP() <= 0)))
            {
                OnReset();
                Game.messageBox.DisplayMessage("Noooo! The freighter was destroyed. We failed.");
                Game.stateManager.ChangeState("OverworldState");
            }

            // Player arrives at Soelara with freighter
            if (progress == 3 && freighter.HasArrived)
            {
                missionHelper.ShowEvent(6);
                progress = 4;
                Game.stateManager.GotoStationSubScreen("Soelara Station", "Overview");
            }

            // Player returns to overworld after visiting Soelara Station
            if (progress == 4 && GameStateManager.currentState.Equals("OverworldState"))
            {
                ObjectiveIndex = 4;
                progress = 5;
            }

            // Player returns to colony on Highfence
            if (progress == 5 && missionHelper.IsPlayerOnPlanet("Highfence"))
            {
                MissionManager.MarkMissionAsCompleted(this.MissionName);
            }

            if (GameStateManager.currentState == "OverworldState")
            {
                Collision();
            }
        }

        private void Collision()
        {
            messageTimer--;

            List<GameObjectOverworld> gameObjs = Game.stateManager.overworldState.GetDeepSpaceGameObjects;

            for (int i = 0; i < gameObjs.Count; i++)
            {
                if (CollisionDetection.IsRectInRect(freighter.Bounds, gameObjs[i].Bounds))
                {
                    if (gameObjs[i] is RebelShip)
                    {
                        Game.stateManager.overworldState.RemoveOverworldObject(gameObjs[i]);
                        Game.messageBox.DisplayMessage("Death to the Alliance!");
                        Game.stateManager.shooterState.BeginLevel(((RebelShip)gameObjs[i]).GetLevel);
                    }
                }
            }

            if (progress == 3)
            {
                if (!CollisionDetection.IsPointInsideCircle(Game.player.position, freighter.position, 600) &&
                    messageTimer <= 0)
                {
                    Game.messageBox.DisplayMessage("\"Don't stray too far from the freighter, get back here!\"");
                    messageTimer = 200;
                }

                if (CollisionDetection.IsPointInsideCircle(Game.player.position, freighter.position, 600) &&
                    messageTimer > 0)
                {
                    Game.messageBox.DisplayMessage("\"Good! Now keep the freighter in sight at all times!\"");
                    messageTimer = 0;
                }
            }

            if (messageTimer == 1)
            {
                OnReset();
                Game.messageBox.DisplayMessage("\"What are you doing?! You compromised our entire mission. Get out of my sight, you moron!\"");
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (progress == 2 && GameStateManager.currentState.Equals("OverworldState") &&
                CollisionDetection.IsRectInRect(Game.player.Bounds, freighter.Bounds))
            {
                CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, freighter, new Rectangle(2, 374, 0, 0));
                Game.helper.DisplayText("Press 'Enter' to talk to freighter captain..");
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
