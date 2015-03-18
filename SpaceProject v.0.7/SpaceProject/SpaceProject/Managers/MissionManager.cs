using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public enum MissionID
    {
        //Main
        Main1_1_RebelsInTheAsteroids,
        Main1_2_ToHighfence,
        Main2_1_TheConvoy,
        Main2_2_ToFortrun,
        Main3_DefendColony,
        Main4_Infiltration,
        Main5_Retribution,
        Main6_InTheNameOfScience,
        Main7_Information,
        Main8_1_BeginningOfTheEnd,
        Main8_2_TheEnd,
        Main9_A_RebelArc,
        Main9_B_AllianceArc,
        Main9_C_OnYourOwnArc,

        //Side missions
        Side_DebtCollection,
        Side_AstroDodger,
        Side_AstroScan,
        Side_DeathByMeteor,
        Side_FlightTraining,
        Side_ColonyAid
    }

    public class MissionManager
    {
        public static readonly Color MainMissionColor = Color.Green;
        public static readonly Color SideMissionColor = Color.Yellow;

        private Game1 game;

        //Lists
        private static List<Mission> missions;
        private static List<Mission> activeMissions;
        private static List<Mission> removedActiveMissions;

        // Missions

        // Main Missions
        private static Main1_1_RebelsInTheAsteroids mainNewFirstMission;
        private static Main1_2_ToHighfence mainHighfence;
        private static Main2_1_TheConvoy mainRebels;
        private static Main2_2_ToFortrun mainToPhaseTwo;
        private static Main3_DefendColony mainDefendColony;
        private static Main4_Infiltration mainInfiltration;
        private static Main5_Retribution mainRetaliation;
        private static Main6_InTheNameOfScience mainInTheNameOfScience;
        private static Main7_Information mainInformation;
        private static Main8_1_BeginningOfTheEnd mainBeginningOfTheEnd;
        private static Main8_2_TheEnd mainTheEnd;
        private static Main9_A_RebelArc mainRebelArc;
        private static Main9_B_AllianceArc mainAllianceArc;
        private static Main9_C_OnYourOwnArc mainOnYourOwnArc;

        // Side Missions
        private static Side_DebtCollection debtCollection;
        private static Side_AstroDodger astroDodger;
        private static Side_AstroScan astroScan;
        private static Side_DeathByMeteor deathByMeteor;
        private static Side_FlightTraining flightTraining;
        private static Side_ColonyAid colonyAid;

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

            // Main 1-1 - Rebels in the Asteroids
            mainNewFirstMission = new Main1_1_RebelsInTheAsteroids(game, "Main1_1_RebelsInTheAsteroids",
                missionObjectSpriteSheet, MissionID.Main1_1_RebelsInTheAsteroids);

            mainNewFirstMission.Initialize();
            missions.Add(mainNewFirstMission);

            // Main 1-2 - To Highfence
            mainHighfence = new Main1_2_ToHighfence(game, "Main1_2_ToHighfence", null,
                MissionID.Main1_2_ToHighfence);

            mainHighfence.Initialize();
            missions.Add(mainHighfence);

            // Main 2-1 - The Convoy
            mainRebels = new Main2_1_TheConvoy(game, "Main2_1_TheConvoy", null,
                MissionID.Main2_1_TheConvoy);

            mainRebels.Initialize();
            missions.Add(mainRebels);

            // Main 2-2 - To Fortrun
            mainToPhaseTwo = new Main2_2_ToFortrun(game, "Main2_2_ToFortrun", null,
                MissionID.Main2_2_ToFortrun);

            mainToPhaseTwo.Initialize();
            missions.Add(mainToPhaseTwo);

            //Main 3 - Defend Colony
            mainDefendColony = new Main3_DefendColony(game, "Main3_DefendColony", null,
                MissionID.Main3_DefendColony);

            mainDefendColony.Initialize();
            missions.Add(mainDefendColony);

            //Main 4 - Infiltration
            mainInfiltration = new Main4_Infiltration(game, "Main4_Infiltration", null,
                MissionID.Main4_Infiltration);

            mainInfiltration.Initialize();
            missions.Add(mainInfiltration);

            // Main 5 - Retribution
            mainRetaliation = new Main5_Retribution(game, "Main5_Retribution", missionObjectSpriteSheet,
                MissionID.Main5_Retribution);

            mainRetaliation.Initialize();
            missions.Add(mainRetaliation);

            // Main 6 - In The Name Of Science
            mainInTheNameOfScience = new Main6_InTheNameOfScience(game, "Main6_InTheNameOfScience", null,
                MissionID.Main6_InTheNameOfScience);

            mainInTheNameOfScience.Initialize();
            missions.Add(mainInTheNameOfScience);

            // Main 7 - Information
            mainInformation = new Main7_Information(game, "Main7_Information", null,
                MissionID.Main7_Information);

            mainInformation.Initialize();
            missions.Add(mainInformation);

            // Main 8-1 - Beginning Of The End
            mainBeginningOfTheEnd = new Main8_1_BeginningOfTheEnd(game, "Main8_1_BeginningOfTheEnd", null,
                MissionID.Main8_1_BeginningOfTheEnd);

            mainBeginningOfTheEnd.Initialize();
            missions.Add(mainBeginningOfTheEnd);

            // Main 8-2 - The End
            mainTheEnd = new Main8_2_TheEnd(game, "Main8_2_TheEnd", null,
                MissionID.Main8_2_TheEnd);

            mainTheEnd.Initialize();
            missions.Add(mainTheEnd);

            // Main 9-A - Rebel Arc
            mainRebelArc = new Main9_A_RebelArc(game, "Main9_A_RebelArc", null,
                MissionID.Main9_A_RebelArc);

            mainRebelArc.Initialize();
            missions.Add(mainRebelArc);

            // Main 9-B - Alliance Arc
            mainAllianceArc = new Main9_B_AllianceArc(game, "Main9_B_AllianceArc", null,
                MissionID.Main9_B_AllianceArc);

            mainAllianceArc.Initialize();
            missions.Add(mainAllianceArc);

            // Main 9-C - On Your Own Arc
            mainOnYourOwnArc = new Main9_C_OnYourOwnArc(game, "Main9_C_OnYourOwnArc", null,
                MissionID.Main9_C_OnYourOwnArc);

            mainOnYourOwnArc.Initialize();
            missions.Add(mainOnYourOwnArc);

            // Side Missions

            //DebtCollection
            debtCollection = new Side_DebtCollection(game, "Side_DebtCollection", null,
                MissionID.Side_DebtCollection);

            debtCollection.Initialize();
            missions.Add(debtCollection);

            //Astro Dodger
            astroDodger = new Side_AstroDodger(game, "Side_AstroDodger", missionObjectSpriteSheet,
                MissionID.Side_AstroDodger);

            astroDodger.Initialize();
            missions.Add(astroDodger);

            // Astro Scan
            astroScan = new Side_AstroScan(game, "Side_AstroScan", null,
                MissionID.Side_AstroScan);

            astroScan.Initialize();
            missions.Add(astroScan);

            // Death by Meteor
            deathByMeteor = new Side_DeathByMeteor(game, "Side_DeathByMeteor", null,
                MissionID.Side_DeathByMeteor);

            deathByMeteor.Initialize();
            missions.Add(deathByMeteor);

            // Flight training
            flightTraining = new Side_FlightTraining(game, "Side_FlightTraining", null,
                MissionID.Side_FlightTraining);

            flightTraining.Initialize();
            missions.Add(flightTraining);

            // Colony Aid
            colonyAid = new Side_ColonyAid(game, "Side_ColonyAid", null,
                MissionID.Side_ColonyAid);

            colonyAid.Initialize();
            missions.Add(colonyAid);

            RefreshLists();
        }

        public static void UnlockMission(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

            if (tempMission.MissionState.Equals(StateOfMission.Available))
                return;

            else
                tempMission.MissionState = StateOfMission.Available;

            RefreshLists();
        }

        public static void ResetMission(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

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

        public static void RemoveAvailableMission(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

            if (tempMission.MissionState.Equals(StateOfMission.Available))
            {
                tempMission.MissionState = StateOfMission.Unavailable;
            }

            RefreshLists();
        }

        //Sets the state of the mission sent in to ACTIVE
        public static void MarkMissionAsActive(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

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
        public static void MarkMissionAsCompleted(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

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

        public static void MarkCompletedMissionAsDead(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);
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
        public static void MarkMissionAsFailed(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);
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

        public static void MarkFailedMissionAsDead(MissionID missionID)
        {
            Mission tempMission = GetMission(missionID);

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
                    ResetMission(tempMission.MissionID);
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
                PopupHandler.DisplayMessage("Hyper speed unlocked! Hold down '" + ControlManager.GetKeyName(RebindableKeys.Action3) + "' to use.");
            }

            // Screening off player from certain locations
            if (mainInfiltration.MissionState != StateOfMission.CompletedDead
                && mainInfiltration.ObjectiveIndex < 9)
            {
                if (StatsManager.gameMode != GameMode.develop
                    && !game.player.HyperspeedOn)
                {
                    if (CollisionDetection.IsRectInRect(game.player.Bounds,
                        game.stateManager.overworldState.GetRebelOutpost.SpaceRegionArea) &&
                        PopupHandler.TextBufferEmpty)
                    {
                        PopupHandler.DisplayMessage("A large group of rebels prevents you from entering this area.");
                        game.player.InitializeHyperSpeedJump(new Vector2(game.player.position.X + (100 * -game.player.Direction.GetDirectionAsVector().X),
                            game.player.position.Y + (100 * -game.player.Direction.GetDirectionAsVector().Y)), false);
                    }
                }
            }

            // Unlock missions
            if (mainNewFirstMission.MissionState == StateOfMission.CompletedDead &&
                mainHighfence.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main1_2_ToHighfence);
                UnlockMission(MissionID.Side_FlightTraining);
            }

            if (mainHighfence.MissionState == StateOfMission.CompletedDead &&
                mainRebels.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main2_1_TheConvoy);
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
                UnlockMission(MissionID.Main2_2_ToFortrun);
                MarkMissionAsActive(MissionID.Main2_2_ToFortrun);
            }

            if (mainToPhaseTwo.MissionState == StateOfMission.CompletedDead
                && mainDefendColony.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main3_DefendColony);
                MarkMissionAsActive(MissionID.Main3_DefendColony);
            }

            if (mainDefendColony.MissionState == StateOfMission.CompletedDead
                && mainInfiltration.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main4_Infiltration);
            }

            if (mainInfiltration.MissionState == StateOfMission.CompletedDead
                && mainRetaliation.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main5_Retribution);
                MarkMissionAsActive(MissionID.Main5_Retribution);
            }

            if (mainRetaliation.MissionState == StateOfMission.CompletedDead
                && mainInTheNameOfScience.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main6_InTheNameOfScience);
            }

            if (mainInTheNameOfScience.MissionState == StateOfMission.CompletedDead
                && mainInformation.MissionState == StateOfMission.Unavailable)
            {
                UnlockMission(MissionID.Main7_Information);
                MarkMissionAsActive(MissionID.Main7_Information);
            }

            if (mainInformation.MissionState == StateOfMission.Completed
                && mainBeginningOfTheEnd.MissionState == StateOfMission.Unavailable)
            {
                mainInformation.MissionState = StateOfMission.CompletedDead;
                UnlockMission(MissionID.Main8_1_BeginningOfTheEnd);
                MarkMissionAsActive(MissionID.Main8_1_BeginningOfTheEnd);
            }

            // Start second mission after first is completed
            if (mainNewFirstMission.MissionState == StateOfMission.CompletedDead &&
                mainHighfence.MissionState == StateOfMission.Available &&
                mainHighfence.MissionHelper.IsPlayerOnStation("Border Station"))
            {
                MarkMissionAsActive(MissionID.Main1_2_ToHighfence);
            }

            // Start part 2 of final mission
            if (mainBeginningOfTheEnd.MissionState == StateOfMission.Completed
                && mainTheEnd.MissionState == StateOfMission.Unavailable)
            {
                mainBeginningOfTheEnd.MissionState = StateOfMission.CompletedDead;
                UnlockMission(MissionID.Main8_2_TheEnd);
                MarkMissionAsActive(MissionID.Main8_2_TheEnd);
            }

            // Final mission stuff
            if ((mainTheEnd.MissionState == StateOfMission.Completed
                    || (mainTheEnd.MissionState == StateOfMission.CompletedDead 
                        && !RebelFleet.IsShown))
                && GameStateManager.currentState.Equals("OverworldState"))
            {
                UnlockMission(MissionID.Main9_C_OnYourOwnArc);
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
                mainRebelArc.MissionState = StateOfMission.CompletedDead;
                game.stateManager.outroState.SetOutroType(OutroType.RebelEnd);
                game.stateManager.ChangeState("OutroState");
            }

            else if (mainAllianceArc.MissionState == StateOfMission.Completed)
            {
                mainAllianceArc.MissionState = StateOfMission.CompletedDead;
                game.stateManager.outroState.SetOutroType(OutroType.AllianceEnd);
                game.stateManager.ChangeState("OutroState");
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

        public static Mission GetMission(MissionID missionID)
        {
            foreach (Mission mission in missions)
            {
                if (mission.MissionID.Equals(missionID))
                    return mission;
            }

            throw new ArgumentException("Mission not found");
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

        public static List<String> GetAvailableMainMissionLocationNames()
        {
            List<String> missionLocationNames = new List<String>();

            foreach (Mission mission in MissionManager.missions)
            {
                if (mission.MissionState == StateOfMission.Available
                    && mission.IsMainMission)
                {
                    missionLocationNames.Add(mission.LocationName);
                }
            }

            return missionLocationNames;
        }

        public static bool IsNoMainMissionActive()
        {
            foreach (Mission mission in activeMissions)
            {
                if (mission.IsMainMission)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
