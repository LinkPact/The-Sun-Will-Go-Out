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

        // Missions

        // Main Missions
        private static Main0_Tutorial mainTutorialMission;
        private static Main1_NewFirstMission mainNewFirstMission;
        private static Main2_Highfence mainHighfence;
        private static Main3_Rebels mainRebels;
        private static Main4_ToPhaseTwo mainToPhaseTwo;
        private static DefendColony defendColony;
        private static RebelAttack rebelAttack;
        private static Main8_Retaliation mainRetaliation;
        private static Main10_InTheNameOfScience mainInTheNameOfScience;
        private static MainX1_BeginningOfTheEnd mainBeginningOfTheEnd;
        private static MainX2_ContinuationOfTheEnd mainContinuationOfTheEnd;
        private static MainX3_RebelArc mainRebelArc;
        private static MainX4_AllianceArc mainAllianceArc;
        private static MainX5_1_OnYourOwnArc mainOnYourOwnArc;
        private static MainX5_2a_BurnIt mainBurnIt;
        private static MainX5_2b_Coward mainCoward;

        // Side Missions
        private static DebtCollection debtCollection;
        private static AstroDodger astroDodger;
        private static AstroScan astroScan;
        private static DeathByMeteorMission deathByMeteor;
        private static FlightTraining flightTraining;
        private static ColonyAid colonyAid;

        //Old Missions
        //private static MidMissionRebel midMissionRebel;
        //private static FinalMission finalMission;
        //private static MainMissionOne mainMission1;
        //private static MidMissionAlliance midMissionAlliance;
        //private static Main1_AColdWelcome aColdWelcome;
        //private static Main3_TheAlliance theAlliance;

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
            missionObjectSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/MissionObjectSpriteSheet"), null);

            activeMissions = new List<Mission>();
            removedActiveMissions = new List<Mission>();
            missions = new List<Mission>();

            // Main Missions

            // Main 0 - Tutorial Mission
            mainTutorialMission = new Main0_Tutorial(game, "SX_Main0_TutorialMission", null);
            mainTutorialMission.Initialize();
            missions.Add(mainTutorialMission);

            // Main 1 - New First Mission
            mainNewFirstMission = new Main1_NewFirstMission(game, "SX_Main1_NewFirstMission", missionObjectSpriteSheet);
            mainNewFirstMission.Initialize();
            missions.Add(mainNewFirstMission);

            // Main 2 - Highfence
            mainHighfence = new Main2_Highfence(game, "SX_Main2_Highfence", null);
            mainHighfence.Initialize();
            missions.Add(mainHighfence);

            // Main 3 - Rebels
            mainRebels = new Main3_Rebels(game, "SX_Main3_Rebels", null);
            mainRebels.Initialize();
            missions.Add(mainRebels);

            // Main 4 - To Phase Two
            mainToPhaseTwo = new Main4_ToPhaseTwo(game, "SX_Main4_ToPhaseTwo", null);
            mainToPhaseTwo.Initialize();
            missions.Add(mainToPhaseTwo);

            //Defend Colony
            defendColony = new DefendColony(game, "SX_DefendColony", null);
            defendColony.Initialize();
            missions.Add(defendColony);

            //Rebel Attack
            rebelAttack = new RebelAttack(game, "P2_RebelAttack", null);
            rebelAttack.Initialize();
            missions.Add(rebelAttack);

            // Main 8 - Retaliation
            mainRetaliation = new Main8_Retaliation(game, "RO_Main8_Retaliation", missionObjectSpriteSheet);
            mainRetaliation.Initialize();
            missions.Add(mainRetaliation);

            // Main 10 - In The Name Of Science
            mainInTheNameOfScience = new Main10_InTheNameOfScience(game, "RO_Main10_InTheNameOfScience", null);
            mainInTheNameOfScience.Initialize();
            missions.Add(mainInTheNameOfScience);

            // Main X1 - Beginning Of The End
            mainBeginningOfTheEnd = new MainX1_BeginningOfTheEnd(game, "P4_BeginningOfTheEnd", null);
            mainBeginningOfTheEnd.Initialize();
            missions.Add(mainBeginningOfTheEnd);

            // Main X2 - Continuation Of The End
            mainContinuationOfTheEnd = new MainX2_ContinuationOfTheEnd(game, "P4_ContinuationOfTheEnd", null);
            mainContinuationOfTheEnd.Initialize();
            missions.Add(mainContinuationOfTheEnd);

            // Main X3 - Rebel Arc
            mainRebelArc = new MainX3_RebelArc(game, "MainX3_RebelArc", null);
            mainRebelArc.Initialize();
            missions.Add(mainRebelArc);

            // Main X4 - Alliance Arc
            mainAllianceArc = new MainX4_AllianceArc(game, "MainX4_AllianceArc", null);
            mainAllianceArc.Initialize();
            missions.Add(mainAllianceArc);

            // Main X5_1 - On Your Own Arc
            mainOnYourOwnArc = new MainX5_1_OnYourOwnArc(game, "MainX5_1_OnYourOwnArc", null);
            mainOnYourOwnArc.Initialize();
            missions.Add(mainOnYourOwnArc);

            // Main X5_2a - Burn It
            mainBurnIt = new MainX5_2a_BurnIt(game, "MainX5_2a_BurnIt", null);
            mainBurnIt.Initialize();
            missions.Add(mainBurnIt);

            // Main X5_2b - Coward
            mainCoward = new MainX5_2b_Coward(game, "MainX5_2b_Coward", null);
            mainCoward.Initialize();
            missions.Add(mainCoward);

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

            if (tempMission.MissionState.Equals(StateOfMission.Active))
            {
                removedActiveMissions.Add(tempMission);
                tempMission.MissionState = StateOfMission.Available;
                tempMission.OnReset();
            }
            else if (tempMission.MissionState.Equals(StateOfMission.Failed) 
                && tempMission.IsRestartAfterFail())
            {
                removedActiveMissions.Add(tempMission);
                tempMission.MissionState = StateOfMission.Available;
                tempMission.OnReset();
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

        // Checks if parameter is current objective destination of any active mission
        public static bool IsCurrentObjective(GameObjectOverworld obj)
        {
            for (int i = 0; i < activeMissions.Count; i++)
            {
                if (activeMissions[i].ObjectiveDestination != null)
                {
                    if ((activeMissions[i].ObjectiveDestination is Planet
                        || activeMissions[i].ObjectiveDestination is Station
                        || activeMissions[i].ObjectiveDestination is SubInteractiveObject)
                        && (obj.name.ToLower() == activeMissions[i].ObjectiveDestination.name.ToLower()))
                    {
                        return true; 
                    }

                    else if (activeMissions[i].ObjectiveDestination == obj)
                    {
                        return true;
                    }
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
                if (mission.LocationName == planetOrStationName &&
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
                if (mission.MissionName.Equals(missionName))
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
            //// DEBUG
            //if (ControlManager.CheckKeypress(Microsoft.Xna.Framework.Input.Keys.End))
            //{
            //    MarkMissionAsCompleted("Main - A Cold Welcome");
            //}

            //if (mainTutorialMission.MissionState == StateOfMission.CompletedDead
            //    && mainNewFirstMission.MissionState == StateOfMission.Unavailable)
            //{
            //    UnlockMission("Main - New First Mission");
            //    MarkMissionAsActive("Main - New First Mission");
            //}

            // Unlock hyperspeed
            if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.LeftAlt) &&
                ControlManager.CheckKeypress(Keys.Y) && !game.player.IsHyperSpeedUnlocked)
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

            //if (MissionManager.theAlliance.MissionState == StateOfMission.CompletedDead &&
            //    GameStateManager.currentState.Equals("OverworldState") &&
            //    !gameCompleted)
            //{
            //    game.messageBox.DisplayMessage(new List<string> { "Congratulations! You have completed all the story missions we have managed to make so far! You are now free to explore the star system however you like! Look for side-missions you might have missed or just see if you can find any secrets! ;)",
            //        "You can now use the hyper speed button! Hold down 'Space' when moving your ship around in the overworld to go much faster! If you restart the game, you can unlock it by holding down 'Alt' and pressing 'Y'."});
            //    gameCompleted = true;
            //    game.player.UnlockHyperSpeed();
            //}

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
                && mainContinuationOfTheEnd.MissionState == StateOfMission.Unavailable)
            {
                mainBeginningOfTheEnd.MissionState = StateOfMission.CompletedDead;
                UnlockMission(mainContinuationOfTheEnd.MissionName);
                MarkMissionAsActive(mainContinuationOfTheEnd.MissionName);
            }

            // Final mission stuff
            if (mainContinuationOfTheEnd.MissionState == StateOfMission.Completed
                && GameStateManager.currentState.Equals("OverworldState"))
            {
                mainContinuationOfTheEnd.MissionState = StateOfMission.CompletedDead;

                RebelFleet rebelFleet = new RebelFleet(game,
                    game.stateManager.overworldState.GetSectorX.GetSpriteSheet(), Vector2.Zero);
                AllianceFleet allianceFleet = new AllianceFleet(game,
                    game.stateManager.overworldState.GetSectorX.GetSpriteSheet(), Vector2.Zero);

                rebelFleet.Initialize();
                allianceFleet.Initialize();

                game.stateManager.overworldState.AddOverworldObject(rebelFleet);
                game.stateManager.overworldState.AddOverworldObject(allianceFleet);

                game.messageBox.DisplayMessage("Time to make your choice! Go to Telmun!", false);
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
                game.messageBox.DisplaySelectionMenu("Try to escape?",
                    new List<String>() { "Yes", "No"},
                    new List<System.Action>() { 
                        delegate 
                        {
                            UnlockMission("Main - Coward");
                            MarkMissionAsActive("Main - Coward");
                            game.stateManager.planetState.OnEnter();
                        },
                        delegate 
                        {
                            UnlockMission("Main - Burn It");
                            MarkMissionAsActive("Main - Burn It");
                            game.stateManager.planetState.OnEnter();
                        }});

                mainOnYourOwnArc.MissionState = StateOfMission.CompletedDead;
            }

            if (mainBurnIt.MissionState == StateOfMission.CompletedDead)
            {
                game.stateManager.ChangeState("OutroState");
                // TODO: Ending 3
            }

            else if (mainCoward.MissionState == StateOfMission.Completed
                && game.messageBox.MessageState == MessageState.Invisible)
            {
                game.stateManager.ChangeState("OutroState");
                // TODO: Ending 4
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
