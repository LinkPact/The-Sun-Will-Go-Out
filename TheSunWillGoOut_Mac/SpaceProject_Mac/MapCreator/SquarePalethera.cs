using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class SquarePalethera
    {
        #region declaration
        private Sprite spriteSheet;
        private Vector2 position;

        private EventEditor eventEditor;

        private List<OptionEnemyEntry> enemyOptions;
        private ActiveEnemySquare displayEnemy;

        private List<OptionPointEntry> pointOptions;
        private ActivePointEventSquare displayPointEvent;

        private List<OptionDurationEntry> durationOptions;
        private ActiveDurationEventSquare displayDurationEvent;

        private List<OptionMovementEntry> movementOptions;
        private ActiveMovementSquare displayMovement;

        private List<OptionObjectiveEntry> objectiveOptions;
        private ActiveObjectiveSquare displayObjective;

        #endregion

        public SquarePalethera(Sprite spriteSheet, Vector2 position)
        {
            this.spriteSheet = spriteSheet;
            this.position = position;

            enemyOptions = new List<OptionEnemyEntry>();    
            pointOptions = new List<OptionPointEntry>();
            durationOptions = new List<OptionDurationEntry>();
            movementOptions = new List<OptionMovementEntry>();
            objectiveOptions = new List<OptionObjectiveEntry>();

            eventEditor = new EventEditor();
        }

        public void Initialize()
        {
            SetupEnemyEntries();

            int counter = 0;

            foreach (PointEventType type in Enum.GetValues(typeof(PointEventType)))
            {
                OptionPointEntry entry = new OptionPointEntry(spriteSheet, GetEntryPosColumnTwo(counter), type);
                pointOptions.Add(entry);
                counter++;
            }

            displayPointEvent = new ActivePointEventSquare(spriteSheet, GetEntryPosColumnTwo(counter));
            counter++;

            foreach (DurationEventType type in Enum.GetValues(typeof(DurationEventType)))
            {
                OptionDurationEntry entry = new OptionDurationEntry(spriteSheet, GetEntryPosColumnTwo(counter), type);
                durationOptions.Add(entry);
                counter++;
            }

            displayDurationEvent = new ActiveDurationEventSquare(spriteSheet, GetEntryPosColumnTwo(counter));
            counter++;

            //Created here to give it the right starting position
            displayMovement = new ActiveMovementSquare(spriteSheet, GetEntryPosColumnTwo(counter));

            foreach (Movement mov in Enum.GetValues(typeof(Movement)))
            {
                OptionMovementEntry entry = new OptionMovementEntry(spriteSheet, GetEntryPosColumnTwo(counter), mov);
                movementOptions.Add(entry);
                counter++;
            }

            counter++;
            displayObjective = new ActiveObjectiveSquare(spriteSheet, GetEntryPosColumnTwo(counter));

            foreach (LevelObjective mov in Enum.GetValues(typeof(LevelObjective)))
            {
                OptionObjectiveEntry entry = new OptionObjectiveEntry(spriteSheet, GetEntryPosColumnTwo(counter), mov);
                objectiveOptions.Add(entry);
                counter++;
            }
        }

        private void SetupEnemyEntries()
        {
            Dictionary<String, List<Vector2>> positions = CalculateEnemyPositions();
            List<Vector2> unclassified = positions["unclassified"];
            List<Vector2> rebels = positions["rebels"];
            List<Vector2> alliance = positions["alliance"];

            int unclassifiedPos = 1;
            int rebelsPos = 1;
            int alliancePos = 1;

            enemyOptions.Add(new OptionEnemyEntry(spriteSheet, unclassified[0], "Unclassified"));
            enemyOptions.Add(new OptionEnemyEntry(spriteSheet, rebels[0], "Rebels"));
            enemyOptions.Add(new OptionEnemyEntry(spriteSheet, alliance[0], "Alliance"));

            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            {
                OptionEnemyEntry entry;

                if ((int)type < 100)
                {
                    entry = new OptionEnemyEntry(spriteSheet, unclassified[unclassifiedPos], type);
                    unclassifiedPos++;
                }
                else if ((int)type < 200)
                {
                    entry = new OptionEnemyEntry(spriteSheet, rebels[rebelsPos], type);
                    rebelsPos++;
                }
                else if ((int)type < 300)
                {
                    entry = new OptionEnemyEntry(spriteSheet, alliance[alliancePos], type);
                    alliancePos++;
                }
                else
                {
                    throw new ArgumentException("Unknown enum number encountered");
                }

                enemyOptions.Add(entry);
            }

            displayEnemy = new ActiveEnemySquare(spriteSheet,
                new Vector2(alliance[alliance.Count - 1].X, alliance[alliance.Count - 1].Y + 40));
            
         
        }

        private Dictionary<String, List<Vector2>> CalculateEnemyPositions()
        {
            //One to make space for header entry
            int unclassified = 1;
            int rebels = 1;
            int alliance = 1;

            foreach (EnemyType type in Enum.GetValues(typeof(EnemyType)))
            { 
                int nbr = (int)type;
                if (nbr < 100)
                {
                    unclassified++;
                }
                else if (nbr < 200)
                {
                    rebels++;
                }
                else if (nbr < 300)
                {
                    alliance++;
                }
            }

            Dictionary<String, List<Vector2>> positions = new Dictionary<string,List<Vector2>>();
            positions.Add("unclassified", new List<Vector2>());
            positions.Add("rebels", new List<Vector2>());
            positions.Add("alliance", new List<Vector2>());

            int n = 0;
            int group = 0;
            for (; n < unclassified; n++)
            {
                positions["unclassified"].Add(GetEntryPos(n,group));
            }

            group = 1;
            for (; n < unclassified + rebels; n++)
            {
                positions["rebels"].Add(GetEntryPos(n, group));
            }

            group = 2;
            for (; n < unclassified + rebels + alliance; n++)
            {
                positions["alliance"].Add(GetEntryPos(n, group));
            }

            return positions;
        }

        private Vector2 GetEntryPos(int entryNumber, int group)
        {
            return new Vector2(position.X - 15, entryNumber * 15 + group * 25 + position.Y - 50);
        }

        private Vector2 GetEntryPosColumnTwo(int entryNumber)
        {
            return new Vector2(position.X + 150, entryNumber * 15 + position.Y - 50);
        }

        public void Update(GameTime gameTime)
        {
            UpdateEnemyEntries(gameTime);
            displayEnemy.Update(gameTime);

            UpdatePointEntries(gameTime);
            displayPointEvent.Update(gameTime);
            
            UpdateDurationEntries(gameTime);
            displayDurationEvent.Update(gameTime);

            UpdateMovementEntries(gameTime);
            displayMovement.Update(gameTime);

            UpdateObjectiveEntries(gameTime);
            displayObjective.Update(gameTime);

            eventEditor.Update(gameTime);
        }

        #region update
        private void UpdateEnemyEntries(GameTime gameTime)
        {
            foreach (OptionEnemyEntry enemyEntry in enemyOptions)
            {
                enemyEntry.Update(gameTime);

                if (enemyEntry.IsReadyToSetDisplay())
                {
                    enemyEntry.SetDisplay(displayEnemy);
                }
            }
        }

        private void UpdatePointEntries(GameTime gameTime)
        {
            foreach (OptionPointEntry pointEntry in pointOptions)
            {
                pointEntry.Update(gameTime);

                if (pointEntry.IsReadyToSetDisplay())
                {
                    pointEntry.SetDisplay(displayPointEvent);
                }
            }
        }

        private void UpdateDurationEntries(GameTime gameTime)
        {
            foreach (OptionDurationEntry durationEntry in durationOptions)
            {
                durationEntry.Update(gameTime);

                if (durationEntry.IsReadyToSetDisplay())
                {
                    durationEntry.SetDisplay(displayDurationEvent);
                }
            }
        }

        private void UpdateMovementEntries(GameTime gameTime)
        {
            foreach (OptionMovementEntry movementEntry in movementOptions)
            {
                movementEntry.Update(gameTime);

                if (movementEntry.IsReadyToSetDisplay())
                {
                    movementEntry.SetDisplay(displayMovement);
                }
            }
        }

        private void UpdateObjectiveEntries(GameTime gameTime)
        {
            foreach (OptionObjectiveEntry objectiveEntry in objectiveOptions)
            {
                objectiveEntry.Update(gameTime);

                if (objectiveEntry.IsReadyToSetDisplay())
                {
                    objectiveEntry.SetDisplay(displayObjective);
                }
            }
        }
        #endregion

        public void Draw(SpriteBatch spriteBatch)
        {
            DrawEnemyEntries(spriteBatch);
            displayEnemy.Draw(spriteBatch);

            DrawPointEntries(spriteBatch);
            displayPointEvent.Draw(spriteBatch);
            
            DrawDurationEntries(spriteBatch);
            displayDurationEvent.Draw(spriteBatch);

            DrawMovementEntries(spriteBatch);
            displayMovement.Draw(spriteBatch);

            DrawObjectiveEntries(spriteBatch);
            displayObjective.Draw(spriteBatch);

            eventEditor.Draw(spriteBatch);
        }

        #region draw
        private void DrawEnemyEntries(SpriteBatch spriteBatch)
        {
            foreach (OptionEnemyEntry entry in enemyOptions)
            {
                entry.Draw(spriteBatch);
            }
        }

        private void DrawPointEntries(SpriteBatch spriteBatch)
        {
            foreach (OptionPointEntry entry in pointOptions)
            {
                entry.Draw(spriteBatch);
            }
        }

        private void DrawDurationEntries(SpriteBatch spriteBatch)
        {
            foreach (OptionDurationEntry entry in durationOptions)
            {
                entry.Draw(spriteBatch);
            }
        }

        private void DrawMovementEntries(SpriteBatch spriteBatch)
        {
            foreach (OptionMovementEntry entry in movementOptions)
            {
                entry.Draw(spriteBatch);
            }
        }

        private void DrawObjectiveEntries(SpriteBatch spriteBatch)
        {
            foreach (OptionObjectiveEntry entry in objectiveOptions)
            {
                entry.Draw(spriteBatch);
            }
        }
        #endregion
    }
}
