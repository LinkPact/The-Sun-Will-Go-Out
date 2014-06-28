using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //Superclass for button instances.
    public abstract class MapCreatorButton : Clickable
    {
        //private Action action;
        private List<Action> actions;
        protected Game1 game;
        protected Boolean isActive;

        //Constructor used when sprite is used
        protected MapCreatorButton(Sprite spriteSheet, Vector2 position)
            : base(spriteSheet, position)
        {
            this.spriteSheet = spriteSheet;
            actions = new List<Action>();
            isActive = true;
        }

        //Constructor used when no sprite is used
        protected MapCreatorButton(SpriteFont font, Vector2 position)
            : base(position)
        {
            this.spriteSheet = spriteSheet;
            actions = new List<Action>();

            standardFont = font;
            isActive = true;
        }

        public void Activate()
        {
            isActive = true;
        }

        public void Inactivate()
        {
            isActive = false;
        }

        public override void Update(GameTime gameTime)
        {
            if (isActive)
            {
                base.Update(gameTime);

                if (IsLeftClicked())
                    ClickAction();
            }
        }

        protected void AddAction(Action action)
        {
            actions.Add(action);
        }

        //Demands implementation of button-specific logic.
        public abstract void ClickAction();

        public Boolean HasAction()
        {
            return (actions.Count != 0);
        }

        public List<Action> GetActions()
        {
            List<Action> temp = new List<Action>();
            foreach (Action action in actions)
            {
                temp.Add(action);
            }
            actions.Clear();
            return temp;
        }
    }
}
