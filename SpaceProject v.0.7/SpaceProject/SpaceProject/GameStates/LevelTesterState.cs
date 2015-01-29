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
     * A place to easily access and test added levels
     * 
     * Contains list with instances of "LevelTesterEntry"
     * Those entries contains a brief comment about the level, a file-path to the level
     *  and a button which is linked to the level.
     * 
     * When state is running, displayed buttons are used to choose level and equipment.
     * Then, the user can simply run the chosen level with chosen equipment.
     */

    public class LevelTesterState : GameState
    {
        #region declaration

        private SpriteFont smallFont;

        private String chosenLevel;
        private String equipInfo;

        private List<String> display1 = new List<String>();
        private List<LevelTesterEntry> jakobsLevelEntries = new List<LevelTesterEntry>();
        private List<LevelTesterEntry> dannesLevelEntries = new List<LevelTesterEntry>();
        private List<LevelTesterEntry> johansLevelEntries = new List<LevelTesterEntry>();

        private float lifeFactor;
        private float initialLife;

        private List<LevelTesterEntry> GetAllEntries()
        {
            List<LevelTesterEntry> combined = new List<LevelTesterEntry>();

            combined.AddRange(jakobsLevelEntries);
            combined.AddRange(dannesLevelEntries);
            combined.AddRange(johansLevelEntries);

            return combined;
        }

        #endregion

        public LevelTesterState(Game1 game, string name) :
            base(game, name)
        {
            lifeFactor = 1;
            initialLife = StatsManager.Armor();

            smallFont = game.Content.Load<SpriteFont>("Fonts/Iceland_12");

            display1.Add("Press Enter to start chosen level");
            display1.Add("Press Escape to return to main menu");
            display1.Add("Use number keys (0-9) to switch equipment");

            var missionPathDict = GetMissionPathDict();

            johansLevelEntries.Add(new LevelTesterEntry(missionPathDict["1_1"], "Main 1 - Rebels in Asteroids", Keys.Z, standardEquip: 1));
            johansLevelEntries.Add(new LevelTesterEntry(missionPathDict["2_1"], "Main 2 - Defend ship, first ", Keys.X, standardEquip: 2));
            johansLevelEntries.Add(new LevelTesterEntry(missionPathDict["2_2"], "Main 2 - Defend ship, second ", Keys.C, standardEquip: 2));
            johansLevelEntries.Add(new LevelTesterEntry(missionPathDict["3_1"], "Main 3 - Break the Rebels defence", Keys.V, standardEquip: 3));
            johansLevelEntries.Add(new LevelTesterEntry(missionPathDict["3_2"], "Main 3 - Hold against Rebels", Keys.B, standardEquip: 3));

            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["4_1"], "4 - Infiltration (1)", Keys.A, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["4_2"], "4 - Infiltration (2)", Keys.S, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["5_1"], "5 - Retribution (1)", Keys.D, standardEquip: 5));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["5_2"], "5 - Retribution (2)", Keys.F, standardEquip: 5));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["6_1"], "6 - ITNOS (1)", Keys.G, standardEquip: 6));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["6_2"], "6 - ITNOS (2)", Keys.H, standardEquip: 6));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["7_1"], "7 - Information", Keys.J, standardEquip: 6));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8o_1"], "8 - On Your Own End (1)", Keys.K, standardEquip: 7));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8o_2"], "8 - On Your Own End (2)", Keys.L, standardEquip: 7));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8r_1"], "8 - Rebels End (1)", Keys.Q, standardEquip: 7));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8r_2"], "8 - Rebels End (2)", Keys.W, standardEquip: 7));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8a_1"], "8 - Alliance End (1)", Keys.E, standardEquip: 7));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["8a_2"], "8 - Alliance End (2)", Keys.R, standardEquip: 7));

            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["rp1"], "RebPir1", Keys.F1, standardEquip: 2));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["rp2"], "RebPir2", Keys.F2, standardEquip: 2));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["rp3"], "RebPir3", Keys.F3, standardEquip: 2));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["rp4"], "RebPir4", Keys.F4, standardEquip: 2));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["rp5"], "RebPir5", Keys.F5, standardEquip: 2));

            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["ap1"], "AllPir1", Keys.F6, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["ap2"], "AllPir2", Keys.F7, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["ap3"], "AllPir3", Keys.F8, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["ap4"], "AllPir4", Keys.F9, standardEquip: 4));
            jakobsLevelEntries.Add(new LevelTesterEntry(missionPathDict["ap5"], "AllPir5", Keys.F10, standardEquip: 4));

            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP1", "J_RP1", Keys.T, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP2", "J_RP2", Keys.Y, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\rebel\\J_RP3", "J_RP3", Keys.U, standardEquip: 3));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP1", "J_AP1", Keys.I, standardEquip: 5));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP2", "J_AP2", Keys.O, standardEquip: 5));
            //jakobsLevelEntries.Add(new LevelTesterEntry("jakob_pirate\\alliance\\J_AP3", "J_AP3", Keys.P, standardEquip: 5));

            chosenLevel = jakobsLevelEntries[0].GetPath();
        }

        // Creates and returns a dictionary linking level names to string paths
        public static Dictionary<String, String> GetMissionPathDict()
        {
            var pathDict = new Dictionary<String, String>();

            pathDict.Add("1_1", "johan_main\\RebelsAsteroids");
            pathDict.Add("2_1", "johan_main\\FreighterEscortlvl1");
            pathDict.Add("2_2", "johan_main\\FreighterEscortlvl2");
            pathDict.Add("3_1", "johan_main\\DefendColonyBreak");
            pathDict.Add("3_2", "johan_main\\DefendColonyHold");

            pathDict.Add("4_1", "jakob_main\\4_infiltration\\m4_infiltration_lv1_v1");
            pathDict.Add("4_2", "jakob_main\\4_infiltration\\m4_infiltration_lv2_v2");

            pathDict.Add("5_1", "jakob_main\\5_retribution\\m5_retribution_lv1_v2");
            pathDict.Add("5_2", "jakob_main\\5_retribution\\m5_retribution_lv2_v2");

            pathDict.Add("6_1", "jakob_main\\6_itnos\\m6_itnos_lv1_v2");
            pathDict.Add("6_2", "jakob_main\\6_itnos\\m6_itnos_lv2_v1");

            pathDict.Add("7_1", "jakob_main\\7_infiltration\\m7_infiltration_lv1_v2");

            pathDict.Add("8o_1", "jakob_main\\8a_oyo\\m10a_OYO_lv1_v2");
            pathDict.Add("8o_2", "jakob_main\\8a_oyo\\m10a_OYO_lv2_v2");
            pathDict.Add("8r_1", "jakob_main\\8b_rebels\\m10b_rebels_lv1_v2");
            pathDict.Add("8r_2", "jakob_main\\8b_rebels\\m10b_rebels_lv2_v2");
            pathDict.Add("8a_1", "jakob_main\\8c_alliance\\m10c_alliance_lv1_v2");
            pathDict.Add("8a_2", "jakob_main\\8c_alliance\\m10c_alliance_lv2_v2");

            pathDict.Add("rp1", "all_pirates\\RebelPirate1");
            pathDict.Add("rp2", "all_pirates\\RebelPirate2");
            pathDict.Add("rp3", "all_pirates\\RebelPirate3");
            pathDict.Add("rp4", "all_pirates\\RebelPirate4");
            pathDict.Add("rp5", "all_pirates\\RebelPirate5");
            
            pathDict.Add("ap1", "all_pirates\\AlliancePirate1");
            pathDict.Add("ap2", "all_pirates\\AlliancePirate2");
            pathDict.Add("ap3", "all_pirates\\AlliancePirate3");
            pathDict.Add("ap4", "all_pirates\\AlliancePirate4");
            pathDict.Add("ap5", "all_pirates\\AlliancePirate5");

            return pathDict;
        }

        public override void Initialize()
        {
            base.Initialize();
            equipInfo = ShipInventoryManager.MapCreatorEquip(1);
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
           base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            UpdateControls();
            ChooseLevel();
            ApplyEquipments();
        }

        float lifeFactorInterval = 0.5f;
        private void UpdateControls()
        {
            if (ControlManager.CheckKeypress(Keys.Enter))
            {
                int startTime = 0;

                Game.stateManager.shooterState.SetupTestLevelRun(chosenLevel, startTime);
                StatsManager.SetCustomDamageFactor_DEVELOPONLY(lifeFactor);
                Game.stateManager.shooterState.BeginLevel("testRun");
            }

            if (ControlManager.CheckKeypress(Keys.Escape))
            {
                Game.stateManager.ChangeState("MainMenuState");
            }

            if (ControlManager.CheckKeypress(Keys.Up))
            {
                if (lifeFactor < lifeFactorInterval)
                    lifeFactor = lifeFactorInterval;
                else
                    lifeFactor += lifeFactorInterval;
            }

            if (ControlManager.CheckKeypress(Keys.Down))
            {
                if (lifeFactor > lifeFactorInterval)
                    lifeFactor -= lifeFactorInterval;
                else
                    lifeFactor = 0.01f;
            }
        }

        private void ChooseLevel()
        {
            // Checks if one of the level-keys is pressed. If so, assign that as chosenLevel

            List<LevelTesterEntry> combined = GetAllEntries();

            foreach (LevelTesterEntry entry in combined)
            {
                Keys entryKey = entry.GetKey();

                if (ControlManager.CheckKeypress(entryKey))
                {
                    chosenLevel = entry.GetPath();
                    CheckStandardEquip(entry);
                    break;
                }
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

        // Evaluates if a number key is pressed. If so, tells the ShipInventoryManager
        // to equip corresponding equipment set.
        private void ApplyEquipments()
        {
            if (ControlManager.CheckKeypress(Keys.D1))
                equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            else if (ControlManager.CheckKeypress(Keys.D2))
                equipInfo = ShipInventoryManager.MapCreatorEquip(2);
            else if (ControlManager.CheckKeypress(Keys.D3))
                equipInfo = ShipInventoryManager.MapCreatorEquip(3);
            else if (ControlManager.CheckKeypress(Keys.D4))
                equipInfo = ShipInventoryManager.MapCreatorEquip(4);
            else if (ControlManager.CheckKeypress(Keys.D5))
                equipInfo = ShipInventoryManager.MapCreatorEquip(5);
            else if (ControlManager.CheckKeypress(Keys.D6))
                equipInfo = ShipInventoryManager.MapCreatorEquip(6);
            else if (ControlManager.CheckKeypress(Keys.D7))
                equipInfo = ShipInventoryManager.MapCreatorEquip(7);
            else if (ControlManager.CheckKeypress(Keys.D8))
                equipInfo = ShipInventoryManager.MapCreatorEquip(8);
            else if (ControlManager.CheckKeypress(Keys.D9))
                equipInfo = ShipInventoryManager.MapCreatorEquip(9);
            else if (ControlManager.CheckKeypress(Keys.D0))
                equipInfo = ShipInventoryManager.MapCreatorEquip(0);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            float xLeft = 50;
            float xRight = 450;
            float yBase = 50;
            float yInterval = 20;

            // Left column
            for (int n = 0; n < display1.Count; n++)
            {
                spriteBatch.DrawString(smallFont, display1[n], new Vector2(xLeft, yBase + n * yInterval), Color.White);
            }

            spriteBatch.DrawString(smallFont, "Equip: " + equipInfo, new Vector2(xLeft, yBase + (display1.Count + 2) * yInterval), Color.Green);
            spriteBatch.DrawString(smallFont, "Lifefactor (up/down arrow): " + lifeFactor + "x", new Vector2(xLeft, yBase + (display1.Count + 8) * yInterval), Color.Red);

            // Right column
            int posCounter = 0;

            posCounter += 2;
            spriteBatch.DrawString(smallFont, "Jakobs entries", new Vector2(xRight, yBase + (posCounter) * yInterval), Color.Green);
            posCounter++;

            for (int n = 0; n < jakobsLevelEntries.Count; n++)
            {
                spriteBatch.DrawString(smallFont, jakobsLevelEntries[n].GetDescriptionWithKey(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            posCounter += 1;
            spriteBatch.DrawString(smallFont, "Johans entries", new Vector2(xRight, yBase + (posCounter) * yInterval), Color.Green);
            posCounter++;

            for (int n = 0; n < johansLevelEntries.Count; n++)
            {
                spriteBatch.DrawString(smallFont, johansLevelEntries[n].GetDescriptionWithKey(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            posCounter += 1;
            spriteBatch.DrawString(smallFont, "Dannes entries", new Vector2(xRight, yBase + (posCounter) * yInterval), Color.Green);
            posCounter++;

            for (int n = 0; n < dannesLevelEntries.Count; n++)
            {
                spriteBatch.DrawString(smallFont, dannesLevelEntries[n].GetDescriptionWithKey(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            spriteBatch.DrawString(smallFont, "Chosen level: " + chosenLevel, new Vector2(xRight, yBase), Color.White);
        }
    }
}
