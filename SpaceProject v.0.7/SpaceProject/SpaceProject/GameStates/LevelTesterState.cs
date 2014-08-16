using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
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
            smallFont = game.Content.Load<SpriteFont>("Fonts/Iceland_14");

            display1.Add("Press Enter to start chosen level");
            display1.Add("Press Escape to return to main menu");
            display1.Add("Use number keys (0-9) to switch equipment");

            jakobsLevelEntries.Add(new LevelTesterEntry("P3_Science_1", "Phase 3 scientist-level", Keys.A));

            jakobsLevelEntries.Add(new LevelTesterEntry("P4_rebel1", "Phase 4 one rebel scout", Keys.S));
            jakobsLevelEntries.Add(new LevelTesterEntry("P4_rebel2", "Phase 4 other rebel scout", Keys.D));
            jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted1", "Phase 4 hunted to Soelara1", Keys.F));
            jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted2", "Phase 4 hunted to Soelara2", Keys.G));
            jakobsLevelEntries.Add(new LevelTesterEntry("P4_hunted3", "Phase 4 hunted to Soelara3", Keys.H));
            jakobsLevelEntries.Add(new LevelTesterEntry("P4_recognizedByAlliance", "Phase 4 Fortrun guard", Keys.J));

            johansLevelEntries.Add(new LevelTesterEntry("XDefendColony", "Johans defend colony mission", Keys.V));
            johansLevelEntries.Add(new LevelTesterEntry("P2AttackOnRebelStation", "Phase 2 - Attack on station", Keys.B));
            johansLevelEntries.Add(new LevelTesterEntry("P2RebelAmbush", "Phase 2 - Ambush", Keys.N));

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
            StartLevel();
            ApplyEquipments();
        }

        private void UpdateControls()
        {
            if (ControlManager.CheckKeypress(Keys.Enter))
            {
                int startTime = 0;

                Game.stateManager.shooterState.SetupTestLevelRun(chosenLevel, startTime);
                Game.stateManager.shooterState.BeginLevel("testRun");
            }

            if (ControlManager.CheckKeypress(Keys.Escape))
            {
                Game.stateManager.ChangeState("MainMenuState");
            }        
        }

        private void StartLevel()
        {
            List<LevelTesterEntry> combined = GetAllEntries();

            foreach (LevelTesterEntry entry in combined)
            {
                Keys entryKey = entry.GetKey();

                if (ControlManager.CheckKeypress(entryKey))
                {
                    chosenLevel = entry.GetPath();
                    break;
                }
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
            float yInterval = 25;

            // Left column
            for (int n = 0; n < display1.Count; n++)
            {
                spriteBatch.DrawString(smallFont, display1[n], new Vector2(xLeft, yBase + n * yInterval), Color.White);
            }

            spriteBatch.DrawString(smallFont, "Equip: " + equipInfo, new Vector2(xLeft, yBase + (display1.Count + 2) * yInterval), Color.Green);

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

            public LevelTesterEntry(String filepath, String description, Keys entryKey)
            {
                this.filepath = filepath;
                this.description = description;
                this.entryKey = entryKey;
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
        }
    }
}
