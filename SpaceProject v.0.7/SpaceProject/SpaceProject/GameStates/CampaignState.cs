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
        private String debugText = "debugtext";
        private Boolean firstRunStarted = false;

        private int attemptNbr;
        private int totalAttemptCount;

        private int experimentCredits;

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

            var jakobMissionPathDict = GetMissionPathDict();

            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["4_1"], "4 - Infiltration (1)", Keys.A, standardEquip: 4));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["4_2"], "4 - Infiltration (2)", Keys.S, standardEquip: 4));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["5_1"], "5 - Retribution (1)", Keys.D, standardEquip: 5));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["5_2"], "5 - Retribution (2)", Keys.F, standardEquip: 5));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["6_1"], "6 - ITNOS (1)", Keys.G, standardEquip: 6));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["6_2"], "6 - ITNOS (2)", Keys.H, standardEquip: 6));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["7_1"], "7 - Information", Keys.J, standardEquip: 6));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8o_1"], "8 - On Your Own End (1)", Keys.K, standardEquip: 7));
            campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8o_2"], "8 - On Your Own End (2)", Keys.L, standardEquip: 7));
            
            //campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8r_1"], "8 - Rebels End (1)", Keys.Q, standardEquip: 7));
            //campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8r_2"], "8 - Rebels End (2)", Keys.W, standardEquip: 7));
            //campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8a_1"], "8 - Alliance End (1)", Keys.E, standardEquip: 7));
            //campaignEntries.Add(new LevelTesterEntry(jakobMissionPathDict["8a_2"], "8 - Alliance End (2)", Keys.R, standardEquip: 7));
            
            //
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP1", "J_RP1", Keys.T, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP2", "J_RP2", Keys.Y, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP3", "J_RP3", Keys.U, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP1", "J_AP1", Keys.I, standardEquip: 5));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP2", "J_AP2", Keys.O, standardEquip: 5));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP3", "J_AP3", Keys.P, standardEquip: 5));
            //
            //johansLevelEntries.Add(new LevelTesterEntry("johan_main\\RebelsAsteroids", "Main 1 - Rebels in Asteroids", Keys.Z, standardEquip: 1));
            //johansLevelEntries.Add(new LevelTesterEntry("johan_main\\FreighterEscortlvl1", "Main 2 - Defend ship, first ", Keys.X, standardEquip: 2));
            //johansLevelEntries.Add(new LevelTesterEntry("johan_main\\FreighterEscortlvl2", "Main 2 - Defend ship, second ", Keys.C, standardEquip: 2));
            //johansLevelEntries.Add(new LevelTesterEntry("johan_main\\DefendColonyBreak", "Main 3 - Break the Rebels defence", Keys.V, standardEquip: 3));
            //johansLevelEntries.Add(new LevelTesterEntry("johan_main\\DefendColonyHold", "Main 3 - Hold against Rebels", Keys.B, standardEquip: 3));

            //chosenLevel = campaignEntries[0].GetPath(); ;
        }

        // Creates and returns a dictionary linking level names to string paths
        private Dictionary<String, String> GetMissionPathDict()
        {
            var pathDict = new Dictionary<String, String>();

            pathDict.Add("4_1", "jakob_main\\4_infiltration\\m4_infiltration_lv1_v1");
            pathDict.Add("4_2", "jakob_main\\4_infiltration\\m4_infiltration_lv2_v2");

            pathDict.Add("5_1", "jakob_main\\5_retribution\\m5_retribution_lv1_v2");
            pathDict.Add("5_2", "jakob_main\\5_retribution\\m5_retribution_lv2_v2");

            pathDict.Add("6_1", "jakob_main\\6_itnos\\m6_itnos_lv1_v1");
            pathDict.Add("6_2", "jakob_main\\6_itnos\\m6_itnos_lv2_v1");

            pathDict.Add("7_1", "jakob_main\\7_infiltration\\m7_infiltration_lv1_v1");

            pathDict.Add("8o_1", "jakob_main\\8a_oyo\\m10a_OYO_lv1_v1");
            pathDict.Add("8o_2", "jakob_main\\8a_oyo\\m10a_OYO_lv1_v1");
            pathDict.Add("8r_1", "jakob_main\\8b_rebels\\m10b_rebels_lv1_v1");
            pathDict.Add("8r_2", "jakob_main\\8b_rebels\\m10b_rebels_lv1_v1");
            pathDict.Add("8a_1", "jakob_main\\8c_alliance\\m10c_alliance_lv1_v1");
            pathDict.Add("8a_2", "jakob_main\\8c_alliance\\m10c_alliance_lv1_v1");

            return pathDict;
        }

        public override void Initialize()
        {
            base.Initialize();
            equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            currentLevel = 1;
            attemptNbr = 1;
            totalAttemptCount = 0;
            experimentCredits = 5;
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (firstRunStarted)
            {
                totalAttemptCount++;

                bool levelCompleted = WasLevelCompleted();

                if (levelCompleted)
                {
                    attemptNbr = 1;
                    currentLevel++;
                    lifeFactor = 1;
                }
                else
                {
                    attemptNbr++;
                    lifeFactor++;
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
            CheckStandardEquip(campaignEntries[currentLevel-1]);
        }

        private void UpdateControls()
        {
            if (ControlManager.CheckKeypress(Keys.Enter))
            {
                int startTime = 0;

                Game.stateManager.shooterState.SetupTestLevelRun(chosenLevelEntry.GetPath(), startTime);
                StatsManager.SetCustomLife_DEVELOPONLY(lifeFactor * initialLife);
                Game.stateManager.shooterState.BeginLevel("testRun");

                firstRunStarted = true;
            }

            if (ControlManager.CheckKeypress(Keys.Escape))
            {
                Game.stateManager.ChangeState("MainMenuState");
            }

            if (ControlManager.CheckKeypress(Keys.Space))
            {
                currentLevel++;
            }

            //if (ControlManager.CheckKeypress(Keys.Up))
            //{
            //    if (lifeFactor < lifeFactorInterval)
            //        lifeFactor = lifeFactorInterval;
            //    else
            //        lifeFactor += lifeFactorInterval;
            //}
            //
            //if (ControlManager.CheckKeypress(Keys.Down))
            //{
            //    if (lifeFactor > lifeFactorInterval)
            //        lifeFactor -= lifeFactorInterval;
            //    else
            //        lifeFactor = 0.01f;
            //}
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
            float xRight = 450;
            float yBase = 50;
            float yInterval = 40;

            var displayStrings = GetDisplayStrings();

            int pos = 0;
            // Title
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

            // Right column
            int posCounter = 0;

            posCounter += 2;
            posCounter++;
        }

        private List<String> GetDisplayStrings()
        {
            var strings = new List<String>();
            strings.Add("Level: " + currentLevel + "/" + campaignEntries.Count);
            strings.Add("Description: " + chosenLevelEntry.GetDescription());
            strings.Add("Lifefactor: " + lifeFactor + "x");
            strings.Add("Attempt: " + attemptNbr);
            strings.Add("Total number of attempts: " + totalAttemptCount);
            strings.Add("Experiment credits: " + experimentCredits + " Rupees");
            return strings;
        }
    }
}
