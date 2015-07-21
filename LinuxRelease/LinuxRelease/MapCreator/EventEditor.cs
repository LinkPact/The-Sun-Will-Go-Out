using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class EventEditor
    {
        private static Dictionary<PointEventType, SortedDictionary<string, EventSetting>> masterPointDictionary;
        private static Dictionary<DurationEventType, SortedDictionary<string, EventSetting>> masterDurationDictionary;
        private static Dictionary<LevelObjective, SortedDictionary<string, EventSetting>> masterObjectiveDictionary;
        
        public EventEditor()
        {
            CreateMasterPointDictionary();
            CreateMasterDurationDictionary();
            CreateMasterObjectiveDictionary();
        }

        #region listCreation
        private void CreateMasterPointDictionary()
        {
            masterPointDictionary = new Dictionary<PointEventType, SortedDictionary<string, EventSetting>>();

            SortedDictionary<string, EventSetting> point = new SortedDictionary<string, EventSetting>();
            
            SortedDictionary<string, EventSetting> horizontal = new SortedDictionary<string, EventSetting>();
            horizontal.Add("D", new EventSetting("dir (1,2)", GetPointSettingPosition(0), 1));
            horizontal.Add("Y", new EventSetting("y-pos (%)", GetPointSettingPosition(1), 200));

            SortedDictionary<string, EventSetting> line = new SortedDictionary<string, EventSetting>();
            line.Add("W", new EventSetting("x-count", GetPointSettingPosition(0), 4));
            line.Add("X", new EventSetting("x-spacing", GetPointSettingPosition(1), 30));

            SortedDictionary<string, EventSetting> square = new SortedDictionary<string, EventSetting>();
            square.Add("W", new EventSetting("x-count", GetPointSettingPosition(0), 4));
            square.Add("X", new EventSetting("x-spacing", GetPointSettingPosition(1), 30));
            square.Add("H", new EventSetting("y-count", GetPointSettingPosition(2), 2));
            square.Add("Y", new EventSetting("y-spacing", GetPointSettingPosition(3), 30));

            SortedDictionary<string, EventSetting> vformation = new SortedDictionary<string, EventSetting>();
            vformation.Add("H", new EventSetting("lines", GetPointSettingPosition(0), 2));
            vformation.Add("X", new EventSetting("x-spacing", GetPointSettingPosition(1), 30));
            vformation.Add("Y", new EventSetting("y-spacing", GetPointSettingPosition(2), 30));

            masterPointDictionary.Add(PointEventType.point, point);
            masterPointDictionary.Add(PointEventType.horizontal, horizontal);
            masterPointDictionary.Add(PointEventType.line, line);
            masterPointDictionary.Add(PointEventType.square, square);
            masterPointDictionary.Add(PointEventType.vformation, vformation);
        }

        private void CreateMasterDurationDictionary()
        {
            masterDurationDictionary = new Dictionary<DurationEventType, SortedDictionary<string, EventSetting>>();

            SortedDictionary<string, EventSetting> even = new SortedDictionary<string, EventSetting>();
            even.Add("D", new EventSetting("Density", GetDurationSettingPosition(0), 20));
            
            SortedDictionary<string, EventSetting> gradient = new SortedDictionary<string, EventSetting>();
            gradient.Add("S", new EventSetting("BaseD", GetDurationSettingPosition(0), 10));
            gradient.Add("P", new EventSetting("PeakD", GetDurationSettingPosition(1), 50));
            gradient.Add("T", new EventSetting("Peak %", GetDurationSettingPosition(2), 50, 0, 100));
            
            masterDurationDictionary.Add(DurationEventType.even, even);
            masterDurationDictionary.Add(DurationEventType.gradient, gradient);
        }

        private void CreateMasterObjectiveDictionary()
        {
            masterObjectiveDictionary = new Dictionary<LevelObjective, SortedDictionary<string, EventSetting>>();

            SortedDictionary<string, EventSetting> finish = new SortedDictionary<string, EventSetting>();

            SortedDictionary<string, EventSetting> killNumber = new SortedDictionary<string, EventSetting>();
            killNumber.Add("N", new EventSetting("Number", GetObjectiveSettingPosition(0), 30));

            SortedDictionary<string, EventSetting> killPercentage = new SortedDictionary<string, EventSetting>();
            killPercentage.Add("P", new EventSetting("%", GetObjectiveSettingPosition(0), 20));

            SortedDictionary<string, EventSetting> killPercentageOrSurvive = new SortedDictionary<string, EventSetting>();
            killPercentageOrSurvive.Add("U", new EventSetting("%OrSurvive", GetObjectiveSettingPosition(0), 20));

            SortedDictionary<string, EventSetting> killNumberOrSurvive = new SortedDictionary<string, EventSetting>();
            killNumberOrSurvive.Add("S", new EventSetting("NumberOrSurvive", GetObjectiveSettingPosition(0), 30));

            SortedDictionary<string, EventSetting> countMayNotPass = new SortedDictionary<string, EventSetting>();
            countMayNotPass.Add("C", new EventSetting("Count", GetObjectiveSettingPosition(0), 10));

            SortedDictionary<string, EventSetting> time = new SortedDictionary<string, EventSetting>();
            time.Add("T", new EventSetting("Time", GetObjectiveSettingPosition(0), 20));

            SortedDictionary<string, EventSetting> bossDummy = new SortedDictionary<string, EventSetting>();

            masterObjectiveDictionary.Add(LevelObjective.Finish, finish);
            masterObjectiveDictionary.Add(LevelObjective.KillNumber, killNumber);
            masterObjectiveDictionary.Add(LevelObjective.KillPercentage, killPercentage);
            masterObjectiveDictionary.Add(LevelObjective.KillNumberOrSurvive, killNumberOrSurvive);
            masterObjectiveDictionary.Add(LevelObjective.KillPercentageOrSurvive, killPercentageOrSurvive);
            masterObjectiveDictionary.Add(LevelObjective.CountMayNotPass, countMayNotPass);
            masterObjectiveDictionary.Add(LevelObjective.Time, time);
            masterObjectiveDictionary.Add(LevelObjective.Boss, bossDummy);
        }
        #endregion

        #region positioning
        private Vector2 GetPointSettingPosition(int position)
        {
            return new Vector2(670, 100 + 30 * position);
        }

        private Vector2 GetDurationSettingPosition(int position)
        {
            return new Vector2(800, 100 + 30 * position);
        }

        private Vector2 GetObjectiveSettingPosition(int position)
        {
            return new Vector2(800, 250 + 30 * position);
        }
        #endregion

        #region update
        public void Update(GameTime gameTime)
        {
            UpdatePointSettings(gameTime);
            UpdateDurationSettings(gameTime);
            UpdateObjectiveSettings(gameTime);
        }
        private void UpdatePointSettings(GameTime gameTime)
        {
            PointEventType activePointType = ActiveData.pointEventState;

            ICollection<string> keys = masterPointDictionary[activePointType].Keys;
            foreach (String key in keys)
            {
                masterPointDictionary[activePointType][key].Update(gameTime);
            }
        }
        private void UpdateDurationSettings(GameTime gameTime)
        {
            DurationEventType activeDurationType = ActiveData.durationEventState;

            ICollection<string> keys = masterDurationDictionary[activeDurationType].Keys;
            foreach (String key in keys)
            {
                masterDurationDictionary[activeDurationType][key].Update(gameTime);
            }
        }
        private void UpdateObjectiveSettings(GameTime gameTime)
        {
            LevelObjective activeObjectiveType = ActiveData.levelObjective;

            ICollection<string> keys = masterObjectiveDictionary[activeObjectiveType].Keys;
            foreach (String key in keys)
            {
                masterObjectiveDictionary[activeObjectiveType][key].Update(gameTime);
            }
        }
        #endregion

        #region draw
        public void Draw(SpriteBatch spriteBatch)
        {
            DrawPointSettings(spriteBatch);
            DrawDurationSettings(spriteBatch);
            DrawObjectiveSettings(spriteBatch);
        }
        private void DrawPointSettings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, ActiveData.pointEventState.ToString(), 
                new Vector2(GetPointSettingPosition(0).X, GetPointSettingPosition(-1).Y), Color.Blue);
            SortedDictionary<string, EventSetting> eventValues = masterPointDictionary[ActiveData.pointEventState];

            DrawEventValues(spriteBatch, eventValues);
        }
        private void DrawDurationSettings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, ActiveData.durationEventState.ToString(), 
                new Vector2(GetDurationSettingPosition(0).X, GetDurationSettingPosition(-1).Y), Color.Blue);
            SortedDictionary<string, EventSetting> eventValues = masterDurationDictionary[ActiveData.durationEventState];

            DrawEventValues(spriteBatch, eventValues);
        }
        private void DrawObjectiveSettings(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, ActiveData.levelObjective.ToString(),
                new Vector2(GetObjectiveSettingPosition(0).X, GetObjectiveSettingPosition(-1).Y), Color.Blue);
            SortedDictionary<string, EventSetting> eventValues = masterObjectiveDictionary[ActiveData.levelObjective];

            DrawEventValues(spriteBatch, eventValues);
        }

        private void DrawEventValues(SpriteBatch spriteBatch, SortedDictionary<string,EventSetting> eventValues)
        {
            if (eventValues != null)
            {
                ICollection<string> keys = eventValues.Keys;
                foreach (String str in keys)
                {
                    eventValues[str].Draw(spriteBatch, str);
                }
            }
        }
        #endregion

        public static SortedDictionary<string, int> GetPointSettings(PointEventType eventType)
        {
            if (masterPointDictionary != null)
                return ExtractValueDictionary(masterPointDictionary[eventType]);
            else
                return null;
        }

        public static SortedDictionary<string, int> GetDurationSettings(DurationEventType eventType)
        {
            if (masterDurationDictionary != null)
                return ExtractValueDictionary(masterDurationDictionary[eventType]);
            else
                return null;
        }

        public static int GetObjectiveInt(LevelObjective objective)
        {
            SortedDictionary<string, int> dict = ExtractValueDictionary(masterObjectiveDictionary[objective]);
            ICollection<int> values = dict.Values;
            if (values.Count == 1)
                return values.ElementAt(0);
            else
                return 0;
        }

        public static String GetObjectiveString()
        {
            LevelObjective currentObj = ActiveData.levelObjective;

            String returnString = currentObj.ToString() + "|";
            SortedDictionary<string, int> objectiveSettings = ExtractValueDictionary(masterObjectiveDictionary[currentObj]);

            returnString += GetSaveString(objectiveSettings);
            
            return returnString;
        }

        //Extract string-value dictionary from string-EventSetting dictionary
        private static SortedDictionary<string, int> ExtractValueDictionary(SortedDictionary<string, EventSetting> settings)
        {
            SortedDictionary<string, int> returnDic = new SortedDictionary<string,int>();
            ICollection<string> keys = settings.Keys;
            foreach (string key in keys)
            {
                returnDic.Add(key, settings[key].GetValue());
            }
            return returnDic;
        }

        public static String GetSaveString(SortedDictionary<string, int> settings)
        {
            ICollection<string> keys = settings.Keys;
            String settingsString = "";

            foreach (String key in keys)
            {
                settingsString += key;
                settingsString += settings[key].ToString();
            }
            
            return settingsString;
        }
    }
}
