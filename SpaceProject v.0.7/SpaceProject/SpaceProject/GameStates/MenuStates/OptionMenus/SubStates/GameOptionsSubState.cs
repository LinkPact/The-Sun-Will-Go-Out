using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class GameOptionsSubState : OptionSubState
    {
        public GameOptionsSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name) :
            base(game, buttonsSprite, optionsMenuState, name)
        { }

        public override void Initialize()
        {
            base.Initialize();
            menuOptions = new String[2, 2];
        }

        public override void OnDisplay()
        {
            menuOptions[0, 0] = "Tutorials";
            if (game.tutorialManager.TutorialsUsed)
            {
                menuOptions[0, 1] = "On";
            }
            else
            {
                menuOptions[0, 1] = "Off";
            }

            menuOptions[1, 0] = "Back";
            menuOptions[1, 1] = "";
            

            base.OnDisplay();
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public void UpdateText()
        {
            if (game.tutorialManager.TutorialsUsed)
            {
                menuOptions[0, 1] = "On";
            }
            else
            {
                menuOptions[0, 1] = "Off";
            }
        }

        public override void ButtonActions()
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
                case "tutorials":
                    game.tutorialManager.TutorialsUsed = !game.tutorialManager.TutorialsUsed;
                    UpdateText();
                    optionsMenuState.SaveSettings();

                    PlaySelectSound();
                    break;

                case "back":
                    optionsMenuState.LeaveSubState();
                    OnLeave();

                    PlayLowPitchSelectSound();
                    break;
            }
        }

        public override void DirectionalButtonActions(String buttonName) { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
