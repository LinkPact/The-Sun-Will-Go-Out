﻿using System;
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

        // Missions

        // Main Missions
        private static Main1_NewFirstMission mainNewFirstMission;
        private static Main2_Highfence mainHighfence;
        private static Main3_Rebels mainRebels;
        private static Main4_ToPhaseTwo mainToPhaseTwo;
        private static Main5_DefendColony mainDefendColony;
        private static Main6_Infiltration mainInfiltration;
        private static Main7_Retaliation mainRetaliation;
        private static Main8_InTheNameOfScience mainInTheNameOfScience;
        private static Main9_Information mainInformation;
        private static Main10_1_BeginningOfTheEnd mainBeginningOfTheEnd;
        private static Main10_2_TheEnd mainTheEnd;
        private static Main10_3a_RebelArc mainRebelArc;
        private static Main10_3b_AllianceArc mainAllianceArc;
        private static Main10_3c_OnYourOwnArc mainOnYourOwnArc;

        // Side Missions
        private static DebtCollection debtCollection;
        private static AstroDodger astroDodger;
        private static AstroScan astroScan;
        private static DeathByMeteorMission deathByMeteor;
        private static FlightTraining flightTraining;
        private static ColonyAid colonyAid;

        private static List<string> missionEventBuffer = new List<string>();
        private static List<string> missionResponseBuffer = new List<string>();
        private static List<string> missionStartBuffer = new List<string>();

        public static List<string> MissionEventBuffer { get { return missionEventBuffer; } set { missionEventBuffer = value; } }
        public static List<string> MissionResponseBuffer { get { return missionResponseBuffer; } set { missionResponseBuffer = value; } }
        public static List<string> MissionStartBuffer { get { return missionStartBuffer; } set { missionStartBuffer = value; } }

        private Sprite missionObjectSpriteSheet;
        private bool gameCompleted = false;
        private bool askRebelMission = false;
        
        public MissionManager(Game1 Game)
        {
            this.game = Game;
        }

        public void Initialize()
        {
            RebelFleet.IsShown = false;

            missionObjectSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/MissionObjectSpriteSheet"), null);

            activeMissions = new List<Mission>();
            removedActiveMissions = new List<Mission>();
            missions = new List<Mission>();

            // Main Missions

            // Main 1 - New First Mission
            mainNewFirstMission = new Main1_NewFirstMission(game, "Main1_NewFirstMission", missionObjectSpriteSheet);
            mainNewFirstMission.Initialize();
            missions.Add(mainNewFirstMission);

            // Main 2 - Highfence
            mainHighfence = new Main2_Highfence(game, "Main2_Highfence", null);
            mainHighfence.Initialize();
            missions.Add(mainHighfence);

            // Main 3 - Rebels
            mainRebels = new Main3_Rebels(game, "Main3_Rebels", null);
            mainRebels.Initialize();
            missions.Add(mainRebels);

            // Main 4 - To Phase Two
            mainToPhaseTwo = new Main4_ToPhaseTwo(game, "Main4_ToPhaseTwo", null);
            mainToPhaseTwo.Initialize();
            missions.Add(mainToPhaseTwo);

            //Main 5 - Defend Colony
            mainDefendColony = new Main5_DefendColony(game, "Main5_DefendColony", null);
            mainDefendColony.Initialize();
            missions.Add(mainDefendColony);

            //Main 6 - Infiltration
            mainInfiltration = new Main6_Infiltration(game, "Main6_Infiltration", null);
            mainInfiltration.Initialize();
            missions.Add(mainInfiltration);

            // Main 7 - Retaliation
            mainRetaliation = new Main7_Retaliation(game, "Main7_Retaliation", missionObjectSpriteSheet);
            mainRetaliation.Initialize();
            missions.Add(mainRetaliation);

            // Main 8 - In The Name Of Science
            mainInTheNameOfScience = new Main8_InTheNameOfScience(game, "Main8_InTheNameOfScience", null);
            mainInTheNameOfScience.Initialize();
            missions.Add(mainInTheNameOfScience);

            // Main 9 - Information
            mainInformation = new Main9_Information(game, "Main9_Information", null);
            mainInformation.Initialize();
            missions.Add(mainInformation);

            // Main 10-1 - Beginning Of The End
            mainBeginningOfTheEnd = new Main10_1_BeginningOfTheEnd(game, "Main10_1_BeginningOfTheEnd", null);
            mainBeginningOfTheEnd.Initialize();
            missions.Add(mainBeginningOfTheEnd);

            // Main 10-2 - Continuation Of The End
            mainTheEnd = new Main10_2_TheEnd(game, "Main10_2_TheEnd", null);
            mainTheEnd.Initialize();
            missions.Add(mainTheEnd);

            // Main X3 - Rebel Arc
            mainRebelArc = new Main10_3a_RebelArc(game, "Main10_3a_RebelArc", null);
            mainRebelArc.Initialize();
            missions.Add(mainRebelArc);

            // Main X4 - Alliance Arc
            mainAllianceArc = new Main10_3b_AllianceArc(game, "Main10_3b_AllianceArc", null);
            mainAllianceArc.Initialize();
            missions.Add(mainAllianceArc);

            // Main X5_1 - On Your Own Arc
            mainOnYourOwnArc = new Main10_3c_OnYourOwnArc(game, "Main10_3c_OnYourOwnArc", null);
            mainOnYourOwnArc.Initialize();
            missions.Add(mainOnYourOwnArc);

            // Side Missions

            //DebtCollection
            debtCollection = new DebtCollection(game, "SX_DebtCollection", null);
            debtCollection.Initialize();
            missions.Add(debtCollection);

            //Astro Dodger
            astroDodger = new AstroDodger(game, "SX_AstroDodger", missionObjectSpriteSheet);
            astroDodger.Initialize();
            missions.Add(astroDodger);

            // Astro Scan
            astroScan = new AstroScan(game, "SX_AstroScan", null);
            astroScan.Initialize();
            missions.Add(astroScan);

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

            // Old missions

            // Final Mission
            //finalMission = new FinalMission(game, "SZ_FinalMission", null);
            //finalMission.Initialize();
            //missions.Add(finalMission);

            //Main1
            //mainMission1 = new MainMissionOne(game, "SY_Main1", null);
            //mainMission1.Initialize();
            //missions.Add(mainMission1);

            // MidMission Alliance
            //midMissionAlliance = new MidMissionAlliance(game, "SY_MidMission_Alliance", missionObjectSpriteSheet);
            //midMissionAlliance.Initialize();
            //missions.Add(midMissionAlliance);

            // Mid Mission Rebel
            //midMissionRebel = new MidMissionRebel(game, "SX_MidMission_Rebel", null);
            //midMissionRebel.Initialize();
            //missions.Add(midMissionRebel);

            // First Mission
            //aColdWelcome = new Main1_AColdWelcome(game, "SX_AColdWelcome", missionObjectSpriteSheet);
            //aColdWelcome.Initialize();
            //missions.Add(aColdWelcome);

            // Third Mission
            //theAlliance = new Main3_TheAlliance(game, "SX_TheAlliance", null);
            //theAlliance.Initialize();
            //missions.Add(theAlliance);

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

            if (tempMission.MissionState.Equals(StateOfMission.Active)
                || tempMission.MissionState.Equals(StateOfMission.Failed))
            {
                tempMission.OnReset();
                tempMission.MissionState = StateOfMission.Available;

                if (tempMission.MissionState.Equals(StateOfMission.Active))
                {
                    removedActiveMissions.Add(tempMission);
                }
            }

            else
            {
                return;
            }

            RefreshLists();
        }

        public static void RemoveAvailableMission(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Available))
            {
                tempMission.MissionState = StateOfMission.Unavailable;
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

                foreach (Objective obj in tempMission.Objectives)
                {
                    obj.OnMissionStart();
                }
            }

            RefreshLists();
        }

        //Sets the state of the mission sent in to COMPLETED
        public static void MarkMissionAsCompleted(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);

            if (tempMission.MissionState.Equals(StateOfMission.Completed)
                || tempMission.MissionState.Equals(StateOfMission.CompletedDead))
                return;

            else if (activeMissions.Contains(tempMission))
            {
                tempMission.MissionState = StateOfMission.Completed;
                removedActiveMissions.Add(tempMission);
                tempMission.CurrentObjectiveDescription = tempMission.ObjectiveCompleted;
            }

            RefreshLists();
        }

        public static void MarkCompletedMissionAsDead(string missionName)
        {
            Mission tempMission = ReturnSpecifiedMission(missionName);
            tempMission.CurrentObjectiveDescription = tempMission.ObjectiveCompleted;

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
            tempMission.CurrentObjectiveDescription = tempMission.ObjectiveFailed;

            if (tempMission.MissionState.Equals(StateOfMission.Failed))
                return;

            else if (activeMissions.Contains(tempMission))
            {
                    tempMission.MissionState = StateOfMission.Failed;
                    removedActiveMissions.Add(tempMission);
                    tempMission.CurrentObjectiveDescription = tempMission.ObjectiveFailed;
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
                if (!tempMission.IsRestartAfterFail())
                {
                    tempMission.MissionState = StateOfMission.FailedDead;
                }
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

        // Checks if parameter is current objective destination of any active mission
        public static bool IsCurrentObjectiveDestination(GameObjectOverworld obj)
        {
            return GetActiveMissionsAtDestination(obj).Count != 0;
        }

        // Checks if gameobject is target for main mission objective
        public static Boolean IsMainMissionDestination(GameObjectOverworld obj)
        {
            List<Mission> currentMissions = GetActiveMissionsAtDestination(obj);

            foreach (Mission mission in currentMissions)
            {
                if (mission.IsMainMission)
                {
                    return true;
                }
            }
            return false;
        }

        private static List<Mission> GetActiveMissionsAtDestination(GameObjectOverworld obj)
        {
            List<Mission> currentMissions = new List<Mission>();
            
            for (int i = 0; i < activeMissions.Count; i++)
            {
                if (activeMissions[i].ObjectiveDestination != null)
                {
                    if ((activeMissions[i].ObjectiveDestination is Planet
                        || activeMissions[i].ObjectiveDestination is Station
                        || activeMissions[i].ObjectiveDestination is SubInteractiveObject)
                        && (obj.name.ToLower() == activeMissions[i].ObjectiveDestination.name.ToLower()))
                    {
                        currentMissions.Add(activeMissions[i]);
                    }

                    else if (activeMissions[i].ObjectiveDestination == obj)
                    {
                        currentMissions.Add(activeMissions[i]);
                    }
                }
            }

            return currentMissions;
        }

        /////////////////

        //Returns a list of Missions of completed missions on a specified planet or station
        public static List<Mission> ReturnCompletedMissions(string planetOrStationName)
        {
            List<Mission> tempList = new List<Mission>();

            foreach (Mission mission in missions)
            {
                if (mission.EndLocationName == planetOrStationName &&
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
                if (mission.LocationName == planetOrStationName &&
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
                if (planetOrStationName == mission.LocationName &&
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
                if (mission.MissionName.ToLower().Equals(missionName.ToLower()))
                    return mission;
            }

            throw new ArgumentException("Mission not found");
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

            // Unlock hyperspeed
            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                ControlManager.CheckKeyPress(Keys.Y) && !game.player.IsHyperSpeedUnlocked)
            {
                game.player.UnlockHyperSpeed();
                game.messageBox.DisplayMessage("Hyper speed unlocked! Hold down '" + ControlManager.GetKeyName(RebindableKeys.Action3) + "' to use.", false);
            }

            if (mainNewFirstMission.MissionState != StateOfMission.CompletedDead)
            {
                if (StatsManager.gameMode != GameMode.develop)
                {
                    if (CollisionDetection.IsRectInRect(game.player.Bounds,
                        game.stateManager.overworldState.GetSectorX.SpaceRegionArea) &&
                        game.messageBox.MessageState == MessageState.Invisible)
                    {
                        game.messageBox.DisplayMessage("You do not have the proper papers to enter Sector X. Please finish mission 'A Cold Welcome' first.", false);
                        game.player.Direction.SetDirection(new Vector2(game.player.position.X - game.stateManager.overworldState.GetSectorX.SectorXStar.position.X,
                            game.player.position.Y - game.stateManager.overworldState.GetSectorX.SectorXStar.position.Y));
                    }
                }
            }

            // Unlock missions
            if (mainNewFirstMission.MissionState == StateOfMission.CompletedDead &&
                mainHighfence.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Highfence");
                UnlockMission("Flight Training");
            }

            if (mainHighfence.MissionState == StateOfMission.CompletedDead &&
                mainRebels.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Rebels");
                askRebelMission = true;
            }

            if (mainRebels.MissionState == StateOfMission.Available
                && game.stateManager.planetState.SubStateManager.ButtonControl == ButtonControl.Menu
                && askRebelMission)
            {
                askRebelMission = false;
                game.stateManager.planetState.SubStateManager.ChangeMenuSubState("Mission");
                game.stateManager.planetState.SubStateManager.MissionMenuState.SelectedMission = mainRebels;
                game.stateManager.planetState.SubStateManager.MissionMenuState.DisplayMissionIntroduction();
            }

            if (mainRebels.MissionState == StateOfMission.CompletedDead
                && mainToPhaseTwo.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - To Phase Two");
                MarkMissionAsActive("Main - To Phase Two");
            }

            if (mainToPhaseTwo.MissionState == StateOfMission.CompletedDead
                && mainDefendColony.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Defend Colony");
                MarkMissionAsActive("Main - Defend Colony");
            }

            if (mainDefendColony.MissionState == StateOfMission.CompletedDead
                && mainInfiltration.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Infiltration");
            }

            if (mainInfiltration.MissionState == StateOfMission.CompletedDead
                && mainRetaliation.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Retaliation");
                MarkMissionAsActive("Main - Retaliation");
            }

            if (mainRetaliation.MissionState == StateOfMission.CompletedDead
                && mainInTheNameOfScience.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - In the name of Science");
            }

            if (mainInTheNameOfScience.MissionState == StateOfMission.CompletedDead
                && mainInformation.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission("Main - Information");
                MarkMissionAsActive("Main - Information");
            }

            if (mainInformation.MissionState == StateOfMission.Completed
                && mainBeginningOfTheEnd.MissionState == StateOfMission.Unavailable)
            {
                mainInformation.MissionState = StateOfMission.CompletedDead;
                UnlockMission("Main - Beginning of the End");
                MarkMissionAsActive("Main - Beginning of the End");
            }

            // Start second mission after first is completed
            if (mainNewFirstMission.MissionState == StateOfMission.CompletedDead &&
                mainHighfence.MissionState == StateOfMission.Available &&
                mainHighfence.MissionHelper.IsPlayerOnStation("Border Station"))
            {
                MarkMissionAsActive("Main - Highfence");
            }

            // Start part 2 of final mission
            if (mainBeginningOfTheEnd.MissionState == StateOfMission.Completed
                && mainTheEnd.MissionState == StateOfMission.Unavailable)
            {
                mainBeginningOfTheEnd.MissionState = StateOfMission.CompletedDead;
                UnlockMission(mainTheEnd.MissionName);
                MarkMissionAsActive(mainTheEnd.MissionName);
            }

            // Final mission stuff
            if ((mainTheEnd.MissionState == StateOfMission.Completed
                    || (mainTheEnd.MissionState == StateOfMission.CompletedDead 
                        && !RebelFleet.IsShown))
                && GameStateManager.currentState.Equals("OverworldState"))
            {
                UnlockMission(mainOnYourOwnArc.MissionName);
                mainTheEnd.MissionState = StateOfMission.CompletedDead;

                RebelFleet rebelFleet = new RebelFleet(game,
                    game.stateManager.overworldState.GetSectorX.GetSpriteSheet(), Vector2.Zero);
                AllianceFleet allianceFleet = new AllianceFleet(game,
                    game.stateManager.overworldState.GetSectorX.GetSpriteSheet(), Vector2.Zero);
                RebelFleet.IsShown = true;

                rebelFleet.Initialize();
                allianceFleet.Initialize();

                game.stateManager.overworldState.AddOverworldObject(rebelFleet);
                game.stateManager.overworldState.AddOverworldObject(allianceFleet);
            }

            if (mainRebelArc.MissionState == StateOfMission.Completed)
            {
                game.stateManager.ChangeState("OutroState");
                // TODO: Ending 1
            }

            else if (mainAllianceArc.MissionState == StateOfMission.Completed)
            {
                game.stateManager.ChangeState("OutroState");
                // TODO: Ending 2
            }

            else if (mainOnYourOwnArc.MissionState == StateOfMission.Completed)
            {
                mainOnYourOwnArc.MissionState = StateOfMission.CompletedDead;
                game.stateManager.ChangeState("OverworldState");
                game.stateManager.overworldState.ActivateBurnOutEnding();
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
                    missions[i].CurrentObjectiveDescription = missions[i].ObjectiveCompleted;

                else if (missions[i].MissionState == StateOfMission.FailedDead)
                    missions[i].CurrentObjectiveDescription = missions[i].ObjectiveFailed;

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

        public static Mission GetActiveMission(string stationOrPlanetName)
        {
            List<Mission> m = new List<Mission>();

            foreach (Mission ms in ReturnActiveMissions())
            {
                if (ms.CurrentObjective != null)
                {
                    if (ms.CurrentObjective.Destination.name.ToLower().Equals(stationOrPlanetName.ToLower()))
                    {
                        m.Add(ms);
                    }
                }
            }

            if (m.Count > 0)
            {
                return m[0];
            }

            return null;
        }
    }
}
