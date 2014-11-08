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

            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m4_infiltration_lv1_v1", "m4_1", Keys.A, standardEquip: 8));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m4_infiltration_lv2_v1", "m4_2", Keys.S));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m5_retribution_lv1_v1", "m5_1", Keys.D));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m5_retribution_lv2_v1", "m5_2", Keys.F));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m6_itnos_lv1_v1", "m6_1", Keys.G));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m6_itnos_lv2_v1", "m6_2", Keys.H));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m7_infiltration_lv1_v1", "m7_1", Keys.J));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10a_OYO_lv1_v1", "m10a_1", Keys.K));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10a_OYO_lv2_v1", "m10a_2", Keys.L));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10b_rebels_lv1_v1", "m10b_1", Keys.Q));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10b_rebels_lv2_v1", "m10b_2", Keys.W));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10c_alliance_lv1_v1", "m10c_1", Keys.E));
            jakobsLevelEntries.Add(new LevelTesterEntry("jakob_main\\m10c_alliance_lv2_v1", "m10c_2", Keys.R));

            //jakobsLevelEntries.Add(new LevelTesterEntry("P3_Science_1", "Phase 3 scientist-level", Keys.Q));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_rebel1", "Phase 4 one rebel scout", Keys.W));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_rebel2", "Phase 4 other rebel scout", Keys.E));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted1", "Phase 4 hunted to Soelara1", Keys.R));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted2", "Phase 4 hunted to Soelara2", Keys.T));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted3", "Phase 4 hunted to Soelara3", Keys.Y));
            //jakobsLevelEntries.Add(new LevelTesterEntry("P4_recognizedByAlliance", "Phase 4 Fortrun guard", Keys.U));

            johansLevelEntries.Add(new LevelTesterEntry("XDefendColony", "Johans defend colony mission", Keys.V));
            johansLevelEntries.Add(new LevelTesterEntry("P2AttackOnRebelStation", "Phase 2 - Attack on station", Keys.B));
            johansLevelEntries.Add(new LevelTesterEntry("johan_main\\RebelsAsteroids", "Main 1 - Rebels in Asteroids", Keys.Z));
            johansLevelEntries.Add(new LevelTesterEntry("johan_main\\FreighterEscortlvl1", "Main 2 - Defend ship, first ", Keys.X, standardEquip: 2));
            johansLevelEntries.Add(new LevelTesterEntry("johan_main\\DefendColonyBreak", "Main 3 - Break the Rebels defence", Keys.V));
            johansLevelEntries.Add(new LevelTesterEntry("johan_main\\DefendColonyHold", "Main 3 - Hold against Rebels", Keys.B));

            chosenLevel = jakobsLevelEntries[0].GetPath(); ;
        }

        public override void Initialize()
        {
            base.Initialize();
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
                StatsManager.SetCustomLife_DEVELOPONLY(lifeFactor * initialLife);
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

        private void ApplyEquipments()
        {
            if (ControlManager.CheckKeypress(Keys.D1))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            }

            if (ControlManager.CheckKeypress(Keys.D2))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(2);
            }

            if (ControlManager.CheckKeypress(Keys.D3))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(3);
            }

            if (ControlManager.CheckKeypress(Keys.D4))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(4);
            }

            if (ControlManager.CheckKeypress(Keys.D5))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(5);
            }

            if (ControlManager.CheckKeypress(Keys.D6))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(6);
            }

            if (ControlManager.CheckKeypress(Keys.D7))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(7);
            }

            if (ControlManager.CheckKeypress(Keys.D8))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(8);
            }

            if (ControlManager.CheckKeypress(Keys.D9))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(9);
            }
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
                spriteBatch.DrawString(smallFont, jakobsLevelEntries[n].GetDescription(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            posCounter += 1;
            spriteBatch.DrawString(smallFont, "Johans entries", new Vector2(xRight, yBase + (posCounter) * yInterval), Color.Green);
            posCounter++;

            for (int n = 0; n < johansLevelEntries.Count; n++)
            {
                spriteBatch.DrawString(smallFont, johansLevelEntries[n].GetDescription(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            posCounter += 1;
            spriteBatch.DrawString(smallFont, "Dannes entries", new Vector2(xRight, yBase + (posCounter) * yInterval), Color.Green);
            posCounter++;

            for (int n = 0; n < dannesLevelEntries.Count; n++)
            {
                spriteBatch.DrawString(smallFont, dannesLevelEntries[n].GetDescription(), new Vector2(xRight, yBase + (posCounter) * yInterval), Color.White);
                posCounter++;
            }

            spriteBatch.DrawString(smallFont, "Chosen level: " + chosenLevel, new Vector2(xRight, yBase), Color.White);
        }

        private class LevelTesterEntry
        {
            private String filepath;
            private String description;
            private Keys entryKey;
            private int standardEquip;

            public LevelTesterEntry(String filepath, String description, Keys entryKey, int standardEquip = -1)
            {
                this.filepath = filepath;
                this.description = description;
                this.entryKey = entryKey;
                this.standardEquip = standardEquip;
            }

            public String GetPath()
            {
                return "testlevels\\" + filepath;
            }

            public Keys GetKey()
            {
                return entryKey;
            }

            public String GetDescription()
            {
                return filepath + ", " + description + ", " + entryKey.ToString();
            }

            public int GetStandardEquip()
            {
                return standardEquip;
            }
        }
    }
}
