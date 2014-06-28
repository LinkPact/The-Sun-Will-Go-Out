using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class EventSetting
    {
        private int value;
        private EditAmountButton editButton;
        private Vector2 position;
        private String display;

        public EventSetting(String display, Vector2 position, int initialValue)
        {
            this.display = display;
            this.value = initialValue;
            this.position = position;
            editButton = new EditAmountButton(MapCreatorGUI.staticSpriteExperiment, new Vector2(position.X + 110, position.Y + 3), -1, -1);
        }

        public EventSetting(String display, Vector2 position, int initialValue, int min, int max)
        {
            this.display = display;
            this.value = initialValue;
            this.position = position;
            editButton = new EditAmountButton(MapCreatorGUI.staticSpriteExperiment, new Vector2(position.X + 110, position.Y + 3), min, max);
        }

        public void SetValue(int newValue)
        {
            value = newValue;
        }

        public int GetValue()
        {
            return value;
        }

        public void Update(GameTime gameTime)
        {
            editButton.Update(gameTime);

            if (editButton.HasAction())
            {
                List<Action> actions = editButton.GetActions();

                foreach (Action action in actions)
                    ((SettingAmountAction)action).PerformAction(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch, String str)
        {
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, display + " " + value, position, new Color(128,50,150));
            editButton.Draw(spriteBatch);
        }
    }
}
