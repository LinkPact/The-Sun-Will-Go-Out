using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    class ControlOptionsSubState : OptionSubState
    {
        private int holdTimer;

        private bool checkInput;
        private RebindableKeys keyToRebind = RebindableKeys.Action1;

        public ControlOptionsSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name) :
            base(game, buttonsSprite, optionsMenuState, name)
        { }

        public override void Initialize()
        {
            base.Initialize();

            menuOptions = new String[15, 2];
            onEnterMenuOptions = new String[15];

            yOffset = -75;
        }

        public override void OnDisplay()
        {
            holdTimer = game.HoldKeyTreshold;
            cursorIndex = 0;

            menuOptions[0, 0] = "Control Type:";
            if (ControlManager.GamepadReady)
            {
                menuOptions[0, 1] = "Gamepad";
            }
            else
            {
                menuOptions[0, 1] = "Keyboard";
            }
            
            menuOptions[1, 0] = "Action 1 / Fire / Confirm";
            menuOptions[1, 1] = ControlManager.GetKeyName(RebindableKeys.Action1);
            
            menuOptions[2, 0] = "Action 2 / Switch weapons / Back";
            menuOptions[2, 1] = ControlManager.GetKeyName(RebindableKeys.Action2);
            
            menuOptions[3, 0] = "Action 3";
            menuOptions[3, 1] = ControlManager.GetKeyName(RebindableKeys.Action3);
            
            menuOptions[4, 0] = "Up";
            menuOptions[4, 1] = ControlManager.GetKeyName(RebindableKeys.Up);
            
            menuOptions[5, 0] = "Down";
            menuOptions[5, 1] = ControlManager.GetKeyName(RebindableKeys.Down);
            
            menuOptions[6, 0] = "Left";
            menuOptions[6, 1] = ControlManager.GetKeyName(RebindableKeys.Left);
            
            menuOptions[7, 0] = "Right";
            menuOptions[7, 1] = ControlManager.GetKeyName(RebindableKeys.Right);
            
            menuOptions[8, 0] = "Pause / Access Menu";
            menuOptions[8, 1] = ControlManager.GetKeyName(RebindableKeys.Pause);

            menuOptions[9, 0] = "Access Inventory";
            menuOptions[9, 1] = ControlManager.GetKeyName(RebindableKeys.Inventory);

            menuOptions[10, 0] = "Access Map";
            menuOptions[10, 1] = ControlManager.GetKeyName(RebindableKeys.Map);

            menuOptions[11, 0] = "Access Mission Log";
            menuOptions[11, 1] = ControlManager.GetKeyName(RebindableKeys.Missions);

            menuOptions[12, 0] = "Access Help Screen";
            menuOptions[12, 1] = ControlManager.GetKeyName(RebindableKeys.Help);
            
            menuOptions[13, 0] = "Reset Default Keys";
            menuOptions[13, 1] = "";
            
            menuOptions[14, 0] = "Back";
            menuOptions[14, 1] = "";

            base.OnDisplay();
        }

        public override void Update(GameTime gameTime)
        {
            if (checkInput)
            {
                CheckForInput();
            }

            else
            {
                base.Update(gameTime);
            }
        }

        public override void ButtonActions()
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
                case "control type:":
                    {
                        if (menuOptions[0, 1] == "Gamepad")
                        {
                            menuOptions[0, 1] = "Keyboard";
                            ControlManager.UseGamepad = false;
                            ResetMenuOptions(true);
                        }
                        else
                        {
                            if (ControlManager.GamepadReady)
                            {
                                menuOptions[0, 1] = "Gamepad";
                                ControlManager.UseGamepad = true;
                                ResetMenuOptions(false);
                            }
                        }
                    }
                    break;
            
                case "action 1 / fire / confirm":
                    InitInputCheck(RebindableKeys.Action1);
                    break;
            
                case "action 2 / switch weapons / back":
                    InitInputCheck(RebindableKeys.Action2);
                    break;
            
                case "action 3":
                    InitInputCheck(RebindableKeys.Action3);
                    break;
            
                case "up":
                    InitInputCheck(RebindableKeys.Up);
                    break;
            
                case "down":
                    InitInputCheck(RebindableKeys.Down);
                    break;
            
                case "left":
                    InitInputCheck(RebindableKeys.Left);
                    break;
            
                case "right":
                    InitInputCheck(RebindableKeys.Right);
                    break;

                case "pause / access menu":
                    InitInputCheck(RebindableKeys.Pause);
                    break;

                case "access inventory":
                    InitInputCheck(RebindableKeys.Inventory);
                    break;

                case "access map":
                    InitInputCheck(RebindableKeys.Map);
                    break;

                case "access mission log":
                    InitInputCheck(RebindableKeys.Missions);
                    break;

                case "access help screen":
                    InitInputCheck(RebindableKeys.Help);
                    break;
            
                case "reset default keys":
                    ControlManager.SetDefaultControls();
                    ResetMenuOptions(menuOptions[0, 1] == "Keyboard");

                    PlaySelectSound();
                    break;
            
                case "back":
                    optionsMenuState.LeaveSubState();
                    OnLeave();

                    PlayLowPitchSelectSound();
                    break;
            } 
        }

        private void ResetMenuOptions(bool Keyboard)
        {
            if (Keyboard)
            {
                menuOptions[0, 1] = "Keyboard";
                menuOptions[1, 1] = ControlManager.KeyboardAction.ToString();
                menuOptions[2, 1] = ControlManager.KeyboardAction2.ToString();
                menuOptions[3, 1] = ControlManager.KeyboardAction3.ToString();
                menuOptions[4, 1] = ControlManager.KeyboardUp.ToString();
                menuOptions[5, 1] = ControlManager.KeyboardDown.ToString();
                menuOptions[6, 1] = ControlManager.KeyboardLeft.ToString();
                menuOptions[7, 1] = ControlManager.KeyboardRight.ToString();
                menuOptions[8, 1] = ControlManager.KeyboardPause.ToString();
                menuOptions[9, 1] = ControlManager.KeyboardInventory.ToString();
                menuOptions[10, 1] = ControlManager.KeyboardMap.ToString();
                menuOptions[11, 1] = ControlManager.KeyboardMissions.ToString();
                menuOptions[12, 1] = ControlManager.KeyboardHelp.ToString();
            }
            
            else
            {
                menuOptions[0, 1] = "Gamepad";
                menuOptions[1, 1] = ControlManager.GamepadAction.ToString();
                menuOptions[2, 1] = ControlManager.GamepadAction2.ToString();
                menuOptions[3, 1] = ControlManager.GamepadAction3.ToString();
                menuOptions[4, 1] = ControlManager.GamepadUp.ToString();
                menuOptions[5, 1] = ControlManager.GamepadDown.ToString();
                menuOptions[6, 1] = ControlManager.GamepadLeft.ToString();
                menuOptions[7, 1] = ControlManager.GamepadRight.ToString();
                menuOptions[8, 1] = ControlManager.GamepadPause.ToString();
            }
        }

        private void CheckForInput()
        {
            menuOptions[cursorIndex, 1] = "Press key";

            if (ControlManager.RebindKey(ControlManager.UseGamepad, keyToRebind))
            {
                menuOptions[cursorIndex, 1] = ControlManager.GetKeyName(keyToRebind);
                checkInput = false;
            }
        }

        private void InitInputCheck(RebindableKeys keyToRebind)
        {
            checkInput = true;
            this.keyToRebind = keyToRebind;
            PlaySelectSound();
        }
    }
}