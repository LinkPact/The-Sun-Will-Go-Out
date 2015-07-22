using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class GameOptionsSubState : OptionSubState
    {
        public GameOptionsSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name) :
            base(game, buttonsSprite, optionsMenuState, name)
        { }

        public override void Initialize()
        {
            base.Initialize();
            menuOptions = new String[3, 2];
            onEnterMenuOptions = new String[2];
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

            menuOptions[1, 0] = "Display Cursor";
            if (ControlManager.IsMouseShown())
            {
                menuOptions[1, 1] = "On";
            }
            else
            {
                menuOptions[1, 1] = "Off";
            }

            menuOptions[2, 0] = "Back";
            menuOptions[2, 1] = "";        

            base.OnDisplay();
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

            if (ControlManager.IsMouseShown())
            {
                menuOptions[1, 1] = "On";
            }
            else
            {
                menuOptions[1, 1] = "Off";
            }
        }

        public override void ButtonActions()
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
                case "tutorials":
                    game.tutorialManager.TutorialsUsed = !game.tutorialManager.TutorialsUsed;
                    UpdateText();

                    PlaySelectSound();
                    break;

                case "display cursor":
                    ControlManager.ToggleMouseHidden();
                    game.IsMouseVisible = ControlManager.IsMouseShown();
                    UpdateText();

                    PlaySelectSound();
                    break;

                case "back":
                    optionsMenuState.LeaveSubState();
                    OnLeave();

                    PlayLowPitchSelectSound();
                    break;
            }
        }
    }
}
