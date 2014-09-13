using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    class ButtonPinger
    {
        private Game1 game;
        private List<MapCreatorButton> buttons;

        public ButtonPinger(Game1 game, List<MapCreatorButton> buttons)
        {
            this.game = game;
            this.buttons = buttons;
        }

        public List<Action> GetActions()
        {
            List<Action> actions = new List<Action>();

            foreach (MapCreatorButton button in buttons)
            {
                if (button.HasAction())
                {
                    foreach (Action ac in button.GetActions())
                    {
                        actions.Add(ac);
                    }
                }
            }

            return actions;
        }
    }
}
