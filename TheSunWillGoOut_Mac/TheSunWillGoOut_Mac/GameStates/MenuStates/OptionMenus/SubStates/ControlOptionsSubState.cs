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

            menuOptions = new String[11, 2];
        }

        public override void OnDisplay()
        {
            base.OnDisplay();

            holdTimer = game.HoldKeyTreshold;
            cursorIndex = 0;

            menuOptions[0, 0] = "Control Type:";
            if (ControlManager.GamepadReady)
                menuOptions[0, 1] = "Gamepad";
            else
                menuOptions[0, 1] = "Keyboard";
            
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
            
            menuOptions[8, 0] = "Pause";
            menuOptions[8, 1] = ControlManager.GetKeyName(RebindableKeys.Pause);
            
            menuOptions[9, 0] = "Reset Default Keys";
            menuOptions[9, 1] = "";
            
            menuOptions[10, 0] = "Back";
            menuOptions[10, 1] = "";
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
            
                case "pause":
                    InitInputCheck(RebindableKeys.Pause);
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

        public override void DirectionalButtonActions(String buttonName) { }

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

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (checkInput)
            //{
            //    //for (int i = 0; i < menuOptions.Length / 2; i++)
            //    //{
            //    //    if (i == cursorIndex)
            //    //    {
            //    //        spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 0],
            //    //            new Vector2(350 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]).X / 2,
            //    //                        175 + (i * 23)) + game.fontManager.FontOffset,
            //    //            Color.LightSkyBlue, 0f,
            //    //            game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]) / 2,
            //    //            1f, SpriteEffects.None, 1f);
            //    //
            //    //        spriteBatch.DrawString(game.fontManager.GetFont(14), "Press key..",
            //    //            new Vector2(700 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]).X / 2,
            //    //                        175 + (i * 23)) + game.fontManager.FontOffset,
            //    //            Color.LightSkyBlue, 0f,
            //    //            game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]) / 2,
            //    //            1f, SpriteEffects.None, 1f);
            //    //    }
            //    //    else
            //    //    {
            //    //        spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 0],
            //    //            new Vector2(350 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]).X / 2,
            //    //                        175 + (i * 23)) + game.fontManager.FontOffset,
            //    //            game.fontManager.FontColor, 0f,
            //    //            game.fontManager.GetFont(14).MeasureString(menuOptions[i, 0]) / 2,
            //    //            1f, SpriteEffects.None, 1f);
            //    //
            //    //        spriteBatch.DrawString(game.fontManager.GetFont(14), menuOptions[i, 1],
            //    //            new Vector2(700 + game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]).X / 2,
            //    //                        175 + (i * 23)) + game.fontManager.FontOffset,
            //    //            game.fontManager.FontColor, 0f,
            //    //            game.fontManager.GetFont(14).MeasureString(menuOptions[i, 1]) / 2,
            //    //            1f, SpriteEffects.None, 1f);
            //    //    }
            //    //}
            //}
            //
            //else 
            //{
                base.Draw(spriteBatch);
            //}
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