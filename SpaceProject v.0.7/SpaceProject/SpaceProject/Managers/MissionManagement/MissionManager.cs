using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MissionManager
    {
        private Game1 game;

        //Lists
        private static List<Mission> missions;
        private static List<Mission> activeMissions;
        private static List<Mission> removedActiveMissions;

        //Missions
        // Sector X
        private static DebtCollection debtCollection;
        private static AstroDodger astroDodger;
        private static DefendColony defendColony;
        private static Main1_AColdWelcome aColdWelcome;
        private static Main2_Rebels rebels;
        private static Main3_TheAlliance theAlliance;
        private static AstroScan astroScan;
        private static MidMissionRebel midMissionRebel;
        private static DeathByMeteorMission deathByMeteor;
        private static FlightTraining flightTraining;

        // Sector Y
        private static ColonyAid colonyAid;
        private static MainMissionOne mainMission1;
        private static MidMissionAlliance midMissionAlliance;

        // Sector Z
        private static FinalMission finalMission;

        private static List<string> missionEventBuffer = new List<string>();
        private static List<string> missionResponseBuffer = new List<string>();

        public static List<string> MissionEventBuffer { get { return missionEventBuffer; } set { missionEventBuffer = value; } }
        public static List<string> MissionResponseBuffer { get { return missionResponseBuffer; } set { missionResponseBuffer = value; } }

        // Mission spritesheet
        private Sprite missionObjectSpriteSheet;

        // Bools used to determine if text has been displayed.
        private bool gameCompleted = false;
        
        public MissionManager(Game1 Game)
        {
            this.game = Game;
        }

        public void Initialize()
        {
            missionObjectSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/MissionObjectSpriteSheet"), null);

            activeMissions = new List<Mission>();
            removedActiveMissions = new List<Mission>();
            missions = new List<Mission>();

            //Initialize Missions

            #region Sector X

            //DebtCollection
            debtCollection = new DebtCollection(game, "SX_DebtCollection", null);
            debtCollection.Initialize();
            missions.Add(debtCollection);

            //Astro Dodger
            astroDodger = new AstroDodger(game, "SX_AstroDodger", missionObjectSpriteSheet);
            astroDodger.Initialize();
            missions.Add(astroDodger);

            //Defend Colony
            defendColony = new DefendColony(game, "SX_DefendColony", null);
            defendColony.Initialize();
            missions.Add(defendColony);

            // First Mission
            aColdWelcome = new Main1_AColdWelcome(game, "SX_AColdWelcome", missionObjectSpriteSheet);
            aColdWelcome.Initialize();
            missions.Add(aColdWelcome);

            // Second Mission
            rebels = new Main2_Rebels(game, "SX_Rebels", null);
            rebels.Initialize();
            missions.Add(rebels);

            // Third Mission
            theAlliance = new Main3_TheAlliance(game, "SX_TheAlliance", null);
            theAlliance.Initialize();
            missions.Add(theAlliance);

            // Astro Scan
            astroScan = new AstroScan(game, "SX_AstroScan", null);
            astroScan.Initialize();
            missions.Add(astroScan);

            // Mid Mission Rebel
            midMissionRebel = new MidMissionRebel(game, "SX_MidMission_Rebel", null);
            midMissionRebel.Initialize();
            missions.Add(midMissionRebel);

            // Jakobs first test mission
            deathByMeteor = new DeathByMeteorMission(game, "SX_JakobTest1", null);
            deathByMeteor.Initialize();
            missions.Add(deathByMeteor);

            // Flight training
            flightTraining = new FlightTraining(game, "SX_FlightTraining", null);
            flightTraining.Initialize();
            missions.Add(flightTraining);

            // ColonyAid
            colonyAid = new ColonyAid(game, "SY_ColonyAid", null);
            colonyAid.Initialize();
            missions.Add(colonyAid);

            #endregion

            #region Sector Y

            //Main1
            mainMission1 = new MainMissionOne(game, "SY_Main1", null);
            mainMission1.Initialize();
            missions.Add(mainMission1);

            // MidMission Alliance
            midMissionAlliance = new MidMissionAlliance(game, "SY_MidMission_Alliance", missionObjectSpriteSheet);
            midMissionAlliance.Initialize();
            missions.Add(midMissionAlliance);
            
            #endregion

            #region Sector Z

            // Final Mission
            finalMission = new FinalMission(game, "SZ_FinalMission", null);
            finalMission.Initialize();
            missions.Add(finalMission);

            #endregion

            RefreshLists();
        }

        public static void UnlockMission(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Available))
                return;

            else
                tempMission.MissionState = StateOfMission.Available;

            RefreshLists();
        }

        public static void ResetMission(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Active))
            {
                removedActiveMissions.Add(tempMission);
                tempMission.MissionState = StateOfMission.Available;
            }
            else if (tempMission.MissionState.Equals(StateOfMission.Failed) 
                && tempMission.IsRestartAfterFailSet())
            {
                removedActiveMissions.Add(tempMission);
                tempMission.MissionState = StateOfMission.Available;
            }

            else
            {
                return;
            }

            RefreshLists();
        }

        //Sets the state of the mission sent in to ACTIVE
        public static void MarkMissionAsActive(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Active))
                return;

            else if (tempMission.MissionState.Equals(StateOfMission.Available))
            {
                tempMission.MissionState = StateOfMission.Active;
                tempMission.StartMission();
            }

            RefreshLists();
        }

        //Sets the state of the mission sent in to COMPLETED
        public static void MarkMissionAsCompleted(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Completed))
                return;

            else if (activeMissions.Contains(tempMission))
            {
                tempMission.MissionState = StateOfMission.Completed;
                removedActiveMissions.Add(tempMission);
                tempMission.CurrentObjective = tempMission.ObjectiveCompleted;
            }

            RefreshLists();
        }

        public static void MarkCompletedMissionAsDead(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);
            tempMission.CurrentObjective = tempMission.ObjectiveCompleted;

            if (tempMission.MissionState.Equals(StateOfMission.CompletedDead))
                return;

            else if (tempMission.MissionState.Equals(StateOfMission.Completed))
            {
                foreach (Item reward in tempMission.RewardItems)
                {
                    ShipInventoryManager.AddItem(reward);
                }

                StatsManager.progress += tempMission.ProgressReward;
                StatsManager.reputation += tempMission.ReputationReward;
                StatsManager.Rupees += tempMission.MoneyReward;

                tempMission.MissionState = StateOfMission.CompletedDead;
            }

            RefreshLists();
        }

        //Sets the state of the mission sent in to FAILED
        public static void MarkMissionAsFailed(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);
            tempMission.CurrentObjective = tempMission.ObjectiveFailed;

            if (tempMission.MissionState.Equals(StateOfMission.Failed))
                return;

            else if (activeMissions.Contains(tempMission))
            {
                    tempMission.MissionState = StateOfMission.Failed;
                    removedActiveMissions.Add(tempMission);
                    tempMission.CurrentObjective = tempMission.ObjectiveFailed;
            }

            RefreshLists();
        }

        public static void MarkFailedMissionAsDead(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.FailedDead))
                return;

            else if (tempMission.MissionState.Equals(StateOfMission.Failed))
            {
                if (!tempMission.IsRestartAfterFailSet())
                    tempMission.MissionState = StateOfMission.FailedDead;
                else
                {
                    ResetMission(tempMission.MissionName);
                }
            }

            //RefreshLists();
        }

        //Returns a list of Missions of active missions
        public static List<Mission> ReturnActiveMissions()
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in activeMissions)
            {
                tempList.Add(mission);
            }

            return tempList;
        }

        // Checks if parameter is current objective of any active mission
        public static bool IsCurrentObjective(GameObjectOverworld obj)
        {
            for (int i = 0; i < activeMissions.Count; i++)
            {
                if (activeMissions[i].ObjectiveDestination != null &&
                    activeMissions[i].ObjectiveDestination.Equals(obj))
                {
                    return true;
                }
            }

            return false;
        }

        //Returns a list of Missions of completed missions on a specified planet or station
        public static List<Mission> ReturnCompletedMissions(string planetOrStationName)
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if (mission.PlanetName == planetOrStationName &&
                    mission.MissionState.Equals(StateOfMission.Completed))
                    tempList.Add(mission);
            }

            return tempList;
        }

        //Returns a list of all completed missions
        public static List<Mission> ReturnCompletedMissions()
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if (mission.MissionState.Equals(StateOfMission.Completed))
                    tempList.Add(mission);
            }

            return tempList;
        }

        public static List<Mission> ReturnFailedMissions(string planetOrStationName)
        { 
           List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if (mission.PlanetName == planetOrStationName &&
                    mission.MissionState.Equals(StateOfMission.Failed))
                        tempList.Add(mission);
            }

            return tempList;
        }

        //Returns a list of Missions of completed dead missions
        public static List<Mission> ReturnCompletedDeadMissions()
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if(mission.MissionState.Equals(StateOfMission.CompletedDead))
                    tempList.Add(mission);
            }

            return tempList;
        }

        //Returns a list of Missions of failed dead missions
        public static List<Mission> ReturnFailedDeadMissions()
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if(mission.MissionState.Equals(StateOfMission.FailedDead))
                    tempList.Add(mission);
            }

            return tempList;
        }

        //Returns a list of Missions of available missions
        public static List<Mission> ReturnAvailableMissions(string planetOrStationName)
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if (planetOrStationName == mission.PlanetName &&
                    mission.MissionState.Equals(StateOfMission.Available))
                    tempList.Add(mission);
            }

            if (tempList.Count.Equals(0))
            {
                return new List<Mission>();
            }

            return tempList;
        }

        public static void CheckMissionLogic(Game1 Game)
        {
            if (missions != null)
            {
                foreach (Mission mission in activeMissions)
                {
                    if (mission.MissionState == StateOfMission.Active)
                        mission.MissionLogic();
                }
            }
        }

        //Set the missions in the lists to their appropriate state
        private static void RefreshLists()
        {
            foreach (Mission mission in missions)
            {
                if (mission.MissionState.Equals(StateOfMission.Active) &&
                    !activeMissions.Contains(mission))
                {
                    activeMissions.Add(mission);
                }
            }
        }

        private static Mission ReturnSpecifiedMission(string missionName)
        {
            foreach (Mission mission in missions)
            {
                if (mission.MissionName.Equals(missionName))
                    return mission;
            }

            return null;
        }

        public void Update(GameTime gameTime)
        {
            foreach (Mission mission in activeMissions)
            {
                if (mission.MissionState == StateOfMission.Active)
                    mission.MissionLogic();
            }

            foreach (Mission mission in removedActiveMissions)
            {
                activeMissions.Remove(mission);
            }
            removedActiveMissions.Clear();

            UpdateProgressionConditions();
        }

        public void UpdateProgressionConditions()
        {
            //// DEBUG
            //if (ControlManager.CheckKeypress(Microsoft.Xna.Framework.Input.Keys.End))
            //{
            //    MarkMissionAsCompleted("Main - A Cold Welcome");
            //}

            // Unlock hyperspeed
            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                ControlManager.CheckKeypress(Keys.Y) && !game.player.IsHyperSpeedUnlocked)
            {
                game.player.UnlockHyperSpeed();
                game.messageBox.DisplayMessage("Hyper speed unlocked! Hold down '" + ControlManager.GetKeyName(RebindableKeys.Action3) + "' to use.");
            }

            if (aColdWelcome.MissionState != StateOfMission.CompletedDead)
            {
                if (StatsManager.gameMode != GameMode.develop)
                {
                    if (CollisionDetection.IsRectInRect(game.player.Bounds,
                        game.stateManager.overworldState.GetSectorX.SpaceRegionArea) &&
                        game.messageBox.MessageState == MessageState.Invisible)
                    {
                        game.messageBox.DisplayMessage("You do not have the proper papers to enter Sector X. Please finish mission 'A Cold Welcome' first.");
                        game.player.Direction.SetDirection(new Vector2(game.player.position.X - game.stateManager.overworldState.GetSectorX.SectorXStar.position.X,
                            game.player.position.Y - game.stateManager.overworldState.GetSectorX.SectorXStar.position.Y));
                    }
                }
            }

            if (MissionManager.theAlliance.MissionState == StateOfMission.CompletedDead &&
                GameStateManager.currentState.Equals("OverworldState") &&
                !gameCompleted)
            {
                game.messageBox.DisplayMessage(new List<string> { "Congratulations! You have completed all the story missions we have managed to make so far! You are now free to explore the star system however you like! Look for side-missions you might have missed or just see if you can find any secrets! ;)",
                    "You can now use the hyper speed button! Hold down 'Space' when moving your ship around in the overworld to go much faster! If you restart the game, you can unlock it by holding down 'Alt' and pressing 'Y'."});
                gameCompleted = true;
                game.player.UnlockHyperSpeed();
            }

            // Unlock missions
            if (MissionManager.aColdWelcome.MissionState == StateOfMission.CompletedDead &&
                MissionManager.rebels.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Rebels");
                UnlockMission("Flight Training");
            }

            if (MissionManager.rebels.MissionState == StateOfMission.CompletedDead &&
                MissionManager.theAlliance.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - The Alliance");
            }

            // Start second mission after first is completed
            if (MissionManager.aColdWelcome.MissionState == StateOfMission.CompletedDead &&
                MissionManager.rebels.MissionState == StateOfMission.Available &&
                GameStateManager.currentState == "OverworldState")
            {
                MissionManager.MarkMissionAsActive("Main - Rebels");
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Mission mission in activeMissions)
            {
                if (mission.MissionState == StateOfMission.Active)
                    mission.Draw(spriteBatch);
            }
        }

        public static Mission GetMission(String missionName)
        {
            for (int i = 0; i < missions.Count; i++)
            {
                if (missions[i].MissionName.ToLower().Equals(missionName.ToLower()))
                {
                    return missions[i];
                }
            }

            throw new ArgumentException(String.Format("No mission by the name of '%s'.", missionName)); 
        }

        //Loops through the missions-list and adds the name and state of each mission into the sorted dictonary "StateOfMissions"
        public void Save()
        {
            SortedDictionary<string, string> StateOfMissions = new SortedDictionary<string, string>();
            SortedDictionary<string, string> CurrentMissionObjectives = new SortedDictionary<string, string>();
            SortedDictionary<string, string> ProgressOfMissions = new SortedDictionary<string,string>();

            foreach (Mission mission in missions)
            {
                int foo = (int)mission.MissionState;
                int objectiveIndex = 0;

                StateOfMissions.Add(mission.MissionName, foo.ToString());

                if (mission.MissionState == StateOfMission.Active || mission.MissionState == StateOfMission.Completed ||
                    mission.MissionState == StateOfMission.Failed)
                {
                    objectiveIndex = mission.ObjectiveIndex;
                    CurrentMissionObjectives.Add(mission.MissionName + " obj", objectiveIndex.ToString());

                    ProgressOfMissions.Add(mission.MissionName, mission.GetProgress().ToString());
                }

            }

            game.saveFile.Save("save.ini", "missions", StateOfMissions);
            game.saveFile.Save("save.ini", "missionprog", ProgressOfMissions);
            game.saveFile.Save("save.ini", "missionobjs", CurrentMissionObjectives);

            SortedDictionary<string, string> temp = new SortedDictionary<string, string>();
            temp.Add("isGameCompleted", gameCompleted.ToString());
            game.saveFile.Save("save.ini", "generalprogress", temp);
        }

        //Loops through the missions-list and keys in StateOfMission-dictonary, checks if name and key match and then
        //sets the the state of that mission to the state saved in StateOfMissions
        public void Load()
        {
            for (int i = 0; i < missions.Count; i++)
            {
                missions[i].MissionState = (StateOfMission)game.saveFile.GetPropertyAsInt("missions", missions[i].MissionName, 0);

                if (missions[i].MissionState == StateOfMission.CompletedDead)
                    missions[i].CurrentObjective = missions[i].ObjectiveCompleted;

                else if (missions[i].MissionState == StateOfMission.FailedDead)
                    missions[i].CurrentObjective = missions[i].ObjectiveFailed;

                else
                    missions[i].ObjectiveIndex = game.saveFile.GetPropertyAsInt("missionobjs", missions[i].MissionName + " obj", 0);

                missions[i].SetProgress(game.saveFile.GetPropertyAsInt("missionprog", missions[i].MissionName, 0));

                /*
                for (int j = 0; j < StateOfMissions.Keys.Count; j++)
                {
                    if (StateOfMissions.Keys.ToList<string>()[j] == missions[i].MissionName)
                    {
                        missions[i].MissionState = (StateOfMission)StateOfMissions[missions[i].MissionName];
                        break;
                    }
                }*/

                missions[i].OnLoad();
            }

            gameCompleted = game.saveFile.GetPropertyAsBool("generalprogress", "isgamecompleted", false);

            RefreshLists();
        }
    }
}
