using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;

namespace SpaceProject_Mac
{
    public class RumorsMenuState : MenuState
    {
        private ConfigFile configFileRumors;
        private string rumorString;

        public RumorsMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
            configFileRumors = new ConfigFile();
            configFileRumors.Load("Data/rumordata.dat");
        }

        public override void Initialize()
        { }

        public override void OnEnter()
        {
            LoadRumors(BaseState.GetBase());
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void ButtonActions()
        { }

        public override void CursorActions()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        { }

        private void LoadRumors(GameObjectOverworld baseObject)
        {
            string codeName = "";

            if (baseObject is Planet)
            {
                codeName = ((Planet)baseObject).PlanetCodeName;
            }

            else if (baseObject is Station)
            {
                codeName = ((Station)baseObject).StationCodeName;
            }

            else
            {
                throw new ArgumentException("Variable 'baseObject' must be of class 'Planet' or 'Station'");
            }

            List<String> rumorStrings = new List<String>();

            Regex reg = new Regex(@"^\w+");

            int startPos = 1;
            while (true)
            {
                rumorString = configFileRumors.GetPropertyAsString(codeName, "Rumor" + (startPos).ToString(), "");

                if (reg.IsMatch(rumorString))
                {
                    rumorStrings.Add(rumorString);
                    startPos++;
                }
                else
                {
                    break;
                }
            }

            if (rumorStrings.Count > 0)
                rumorString = rumorStrings[Game.random.Next(rumorStrings.Count)];
            else
                rumorString = "";

            //rumorString = configFileRumors.GetPropertyAsString(planet.PlanetCodeName, "Rumor" + (Random.Next(3) + 1).ToString(), "");
        }

        public void DisplayRumors()
        {
            BaseStateManager.TextBoxes.Add(TextUtils.CreateTextBox(BaseState.Game.fontManager.GetFont(14),
                                                                      BaseStateManager.NormalTextRectangle,
                                                                      false,
                                                                      rumorString));
        }
    }
}
