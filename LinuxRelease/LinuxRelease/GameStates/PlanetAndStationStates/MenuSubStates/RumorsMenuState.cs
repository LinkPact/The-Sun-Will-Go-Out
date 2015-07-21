using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text.RegularExpressions;

namespace SpaceProject
{
    public class RumorsMenuState : MenuState
    {
        private ConfigFile configFileRumors;
        List<String> rumorStrings = new List<String>();
        private string rumorString = "";
        private int rumorIndex;

        public bool HasRumors
        {
            get { return rumorStrings.Count > 0; }
        }

        public RumorsMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
            configFileRumors = new ConfigFile();
            configFileRumors.Load("Data/rumordata.dat");
        }

        public override void OnEnter()
        {
            base.OnEnter();
            rumorIndex = MathFunctions.GetExternalRandomInt(0, rumorStrings.Count - 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void LoadRumors(GameObjectOverworld baseObject)
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

            rumorStrings = new List<String>();

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
        }

        public void DisplayRumors()
        {
            PopupHandler.DisplayMessage(rumorStrings[CheckValidRumorIndex()]);
        }

        private int CheckValidRumorIndex()
        {
            rumorIndex++;

            if (rumorIndex >= rumorStrings.Count)
            {
                rumorIndex = 0;
            }

            return rumorIndex;
        }
    }
}
