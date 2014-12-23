using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    /**
     * This state lets the player run a sequence of levels
     */

    public class CampaignState : GameState
    {
        #region declaration

        private const int checkpoint1 = 4;
        private const int checkpoint2 = 8;
        private const int checkpoint3 = 10;

        private SpriteFont smallFont;
        private SpriteFont bigFont;

        private String equipInfo;

        private List<LevelTesterEntry> campaignEntries = new List<LevelTesterEntry>();
        private LevelTesterEntry chosenLevelEntry
        {
            get
            {
                if (currentLevel < campaignEntries.Count)
                    return campaignEntries[currentLevel - 1];
                else
                    return campaignEntries[campaignEntries.Count - 1];
            }
        }

        private float lifeFactor;
        private float initialLife;
        private int currentLevel;
        private Boolean isGameCompleted;

        private int attemptNbr;
        private int totalAttemptCount;

        private int rupees { get { return StatsManager.Rupees; } set { StatsManager.Rupees = value; }}

        private int prizeMoney { get { return (150 + (currentLevel - 1) * 50) * (int)lifeFactor; } }

        private Boolean standardEquipEnabled = false;

        private List<LevelTesterEntry> GetAllEntries()
        {
            List<LevelTesterEntry> combined = new List<LevelTesterEntry>();
            combined.AddRange(campaignEntries);
            return combined;
        }

        #endregion

        public CampaignState(Game1 game, string name) :
            base(game, name)
        {
            lifeFactor = 1;
            initialLife = StatsManager.Armor();

            smallFont = game.Content.Load<SpriteFont>("Fonts/Iceland_12");
            bigFont = game.Content.Load<SpriteFont>("Fonts/ISL_Jupiter_24");

            //display1.Add("Press Enter to start chosen level");
            //display1.Add("Press Escape to return to main menu");
            //display1.Add("Use number keys (0-9) to switch equipment");

            var jakobMissionPathDict = LevelTesterState.GetMissionPathDict();

            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["1_1"], "1 - Rebels in Asteroids (1)", Keys.A, standardEquip: 1));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["2_1"], "2 - Defend Ship (1)", Keys.A, standardEquip: 2));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["2_2"], "2 - Defend Ship (2)", Keys.A, standardEquip: 2));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["3_1"], "3 - Break the Rebels defence (1)", Keys.A, standardEquip: 3));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["3_2"], "3 - Break the Rebels defence (2)", Keys.A, standardEquip: 3));

            //campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["4_1"], "4 - Infiltration (1)", Keys.A, standardEquip: 4));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["4_2"], "4 - Infiltration (2)", Keys.S, standardEquip: 4));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["5_1"], "5 - Retribution (1)", Keys.D, standardEquip: 5));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["5_2"], "5 - Retribution (2)", Keys.F, standardEquip: 5));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["6_1"], "6 - ITNOS (1)", Keys.G, standardEquip: 6));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["6_2"], "6 - ITNOS (2)", Keys.H, standardEquip: 6));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["7_1"], "7 - Information", Keys.J, standardEquip: 6));
            
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8o_1"], "8 - On Your Own End (1)", Keys.K, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8o_2"], "8 - On Your Own End (2)", Keys.L, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8r_1"], "8 - Rebels End (1)", Keys.K, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8r_2"], "8 - Rebels End (2)", Keys.L, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8a_1"], "8 - Alliance End (1)", Keys.K, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8a_2"], "8 - Alliance End (2)", Keys.L, standardEquip: 7));
            
        }

        public override void Initialize()
        {
            base.Initialize();
            equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            currentLevel = 1;
            attemptNbr = 1;
            totalAttemptCount = 0;
            rupees = 300;
            isGameCompleted = false;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            StatsManager.gameMode = GameMode.campaign;

            Boolean previousWasShooter = (GameStateManager.previousState == "ShooterState");

            if (previousWasShooter)
            {
                totalAttemptCount++;

                bool levelCompleted = WasLevelCompleted();

                if (levelCompleted)
                {
                    rupees += prizeMoney;
                    attemptNbr = 1;
                    currentLevel++;
                    lifeFactor = 1;

                    if (currentLevel >= campaignEntries.Count)
                        isGameCompleted = true;
                }
                else
                {
                    rupees += prizeMoney;
                    //lifeFactor++;
                    attemptNbr++;
                }
            }
        }

        private bool WasLevelCompleted()
        {
            return Game.stateManager.shooterState.GetLevel("testRun").IsObjectiveCompleted;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateControls();
            //ChooseLevel();
            //ApplyEquipments();

            if (standardEquipEnabled && currentLevel <= campaignEntries.Count)
                CheckStandardEquip(campaignEntries[currentLevel-1]);
        }

        private void UpdateControls()
        {
            if (ControlManager.CheckKeypress(Keys.Enter))
            {
                int startTime = 0;

                Game.stateManager.shooterState.SetupTestLevelRun(chosenLevelEntry.GetPath(), startTime);
                StatsManager.SetCustomDamageFactor_DEVELOPONLY(lifeFactor);
                Game.stateManager.shooterState.BeginLevel("testRun");
            }

            if (ControlManager.CheckKeypress(Keys.Escape))
            {
                Game.stateManager.ChangeState("MainMenuState");
            }

            if (ControlManager.CheckKeypress(Keys.D1))
            {
                Game.stateManager.planetState.LoadPlanetData(Game.stateManager.overworldState.GetPlanet("Highfence"));
                Game.stateManager.ChangeState("PlanetState");
            }

            if (ControlManager.CheckKeypress(Keys.D2) && currentLevel >= checkpoint1)
            {
                Game.stateManager.stationState.LoadStationData(Game.stateManager.overworldState.GetStation("Fortrun Station I"));
                Game.stateManager.ChangeState("StationState");
            }

            if (ControlManager.CheckKeypress(Keys.D3) && currentLevel >= checkpoint2)
            {
                Game.stateManager.stationState.LoadStationData(Game.stateManager.overworldState.GetStation("Rebel Station 1"));
                Game.stateManager.ChangeState("StationState");
            }

            if (ControlManager.CheckKeypress(Keys.D4) && currentLevel >= checkpoint3)
            {
                Game.stateManager.planetState.LoadPlanetData(Game.stateManager.overworldState.GetPlanet("Peye"));
                Game.stateManager.ChangeState("PlanetState");
            }

            if (ControlManager.CheckKeypress(Keys.Space))
            {
                standardEquipEnabled = true;
            }

            if (ControlManager.CheckKeypress(Keys.Z))
            {
                currentLevel = checkpoint3;
                rupees = 10000;
            }
        }

        private void CheckStandardEquip(LevelTesterEntry entry)
        {
            // Evaluate if chosen entry has standard equipment
            // If so, assign it to standard equip

            int standardEq = entry.GetStandardEquip();
            if (standardEq != -1)
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(standardEq);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            float xLeft = 50;
            float yBase = 50;
            float yInterval = 30;

            var displayStrings = GetDisplayStrings();
            var availableShops = GetAvailableShops();

            int pos = 0;
            // Title
            String titleString = "CAMPAIGN MODE";
            if (isGameCompleted)
                titleString += " - Game Completed! Congratulations!";

            spriteBatch.DrawString(bigFont, "CAMPAIGN MODE",
                new Vector2(xLeft, yBase + pos * yInterval), Color.Blue);
            pos++;

            // State
            for (int n = 0; n < displayStrings.Count; n++)
            {
                spriteBatch.DrawString(bigFont, displayStrings[n], 
                    new Vector2(xLeft, yBase + pos * yInterval), Color.Green);
                pos++;
            }

            pos++;

            // Shops
            if (!standardEquipEnabled)
            {
                spriteBatch.DrawString(bigFont, "Press space to disable shop and enable default equipment",
                    new Vector2(xLeft, yBase + pos * yInterval), Color.Red);
                pos++;

                spriteBatch.DrawString(bigFont, "Available shops (access with shown key)",
                    new Vector2(xLeft, yBase + pos * yInterval), Color.Orange);
                pos++;
                for (int n = 0; n < availableShops.Count; n++)
                {
                    spriteBatch.DrawString(bigFont, availableShops[n],
                        new Vector2(xLeft, yBase + pos * yInterval), GetShopColor(n + 1));
                    pos++;
                }
            }

            // Right column
            int posCounter = 0;

            posCounter += 2;
            posCounter++;
        }

        private Color GetShopColor(int shopNumber)
        {
            switch (shopNumber)
            { 
                case 1:
                    return Color.Orange;
                case 2:
                    return GetShopColor(currentLevel >= checkpoint1);
                case 3:
                    return GetShopColor(currentLevel >= checkpoint2);
                case 4:
                    return GetShopColor(currentLevel >= checkpoint3);
                default:
                    throw new ArgumentException("Non-existing shopnumber!");
            }
        }

        private Color GetShopColor(Boolean isCheckpointReached)
        {
            if (isCheckpointReached)
                return Color.Orange;
            else
                return Color.Gray;
        }

        private List<String> GetDisplayStrings()
        {
            var strings = new List<String>();
            strings.Add("Level: " + currentLevel + "/" + campaignEntries.Count);
            strings.Add("Description: " + chosenLevelEntry.GetDescription());
            strings.Add("Prize money: " + prizeMoney);
            strings.Add("Lifefactor: " + lifeFactor + "x");
            strings.Add("Attempt: " + attemptNbr);
            strings.Add("Total number of attempts: " + totalAttemptCount);
            strings.Add("Money: " + rupees + " Rupees");
            strings.Add("Total vertical time: " + (int)(StatsManager.PlayTime.ShooterPartTime / 1000) + " seconds");
            return strings;
        }

        private List<String> GetAvailableShops()
        {
            var strings = new List<String>();
            strings.Add("Highfence shop (NumKey 1)");
            strings.Add("Fortrun shop   (NumKey 2)");
            strings.Add("Rebel shop     (NumKey 3)");
            strings.Add("Peye shop      (NumKey 4)");
            return strings;
        }
    }
}
