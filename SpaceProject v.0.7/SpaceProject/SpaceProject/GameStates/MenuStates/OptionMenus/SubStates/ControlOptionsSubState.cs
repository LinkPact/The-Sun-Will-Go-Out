using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
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
                    checkInput = true;
                    keyToRebind = RebindableKeys.Action1;

                    PlaySelectSound();
                    break;
            
                case "action 2 / switch weapons / back":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Action2;

                    PlaySelectSound();
                    break;
            
                case "action 3":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Action3;

                    PlaySelectSound();
                    break;
            
                case "up":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Up;

                    PlaySelectSound();
                    break;
            
                case "down":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Down;

                    PlaySelectSound();
                    break;
            
                case "left":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Left;

                    PlaySelectSound();
                    break;
            
                case "right":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Right;

                    PlaySelectSound();
                    break;
            
                case "pause":
                    checkInput = true;
                    keyToRebind = RebindableKeys.Pause;

                    PlaySelectSound();
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
            string tempKey = "";

            menuOptions[cursorIndex, 1] = "Press key";

            #region Keyboard
            if (!ControlManager.UseGamepad)
            {
                if (ControlManager.CheckKeyPress(Keys.A))
                {
                    ControlManager.RebindKey(false ,keyToRebind, Keys.A, null);
                    tempKey = Keys.A.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.B))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.B, null);
                    tempKey = Keys.B.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.C))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.C, null);
                    tempKey = Keys.C.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.D))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.D, null);
                    tempKey = Keys.D.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.E))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.E, null);
                    tempKey = Keys.E.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.F))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.F, null);
                    tempKey = Keys.F.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.G))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.G, null);
                    tempKey = Keys.G.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.H))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.H, null);
                    tempKey = Keys.H.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.I))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.I, null);
                    tempKey = Keys.I.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.J))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.J, null);
                    tempKey = Keys.J.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.K))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.K, null);
                    tempKey = Keys.K.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.L))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.L, null);
                    tempKey = Keys.L.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.M))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.M, null);
                    tempKey = Keys.M.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.N))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.N, null);
                    tempKey = Keys.N.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.O))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.O, null);
                    tempKey = Keys.O.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.P))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.P, null);
                    tempKey = Keys.P.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Q))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Q, null);
                    tempKey = Keys.Q.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.R))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.R, null);
                    tempKey = Keys.R.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.S))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.S, null);
                    tempKey = Keys.S.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.T))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.T, null);
                    tempKey = Keys.T.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.U))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.U, null);
                    tempKey = Keys.U.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.V))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.V, null);
                    tempKey = Keys.V.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.W))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.W, null);
                    tempKey = Keys.W.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.X))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.X, null);
                    tempKey = Keys.X.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Y))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Y, null);
                    tempKey = Keys.Y.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Z))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Z, null);
                    tempKey = Keys.Z.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Up))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Up, null);
                    tempKey = Keys.Up.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Down))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Down, null);
                    tempKey = Keys.Down.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Left))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Left, null);
                    tempKey = Keys.Left.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Right))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Right, null);
                    tempKey = Keys.Right.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Enter))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Enter, null);
                    tempKey = Keys.Enter.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Space))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Space, null);
                    tempKey = Keys.Space.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Back))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Enter, null);
                    tempKey = Keys.Enter.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Escape))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Escape, null);
                    tempKey = Keys.Escape.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.LeftShift))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.LeftShift, null);
                    tempKey = Keys.LeftShift.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.RightShift))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.RightShift, null);
                    tempKey = Keys.RightShift.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.Tab))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.Tab, null);
                    tempKey = Keys.Tab.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.LeftControl))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.LeftControl, null);
                    tempKey = Keys.LeftControl.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.RightControl))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.RightControl, null);
                    tempKey = Keys.RightControl.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.LeftAlt))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.LeftAlt, null);
                    tempKey = Keys.LeftAlt.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.RightAlt))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.RightAlt, null);
                    tempKey = Keys.RightAlt.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.OemComma))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.OemComma, null);
                    tempKey = Keys.OemComma.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.OemPeriod))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.OemPeriod, null);
                    tempKey = Keys.OemPeriod.ToString();
                    goto end;
                }

                else if (ControlManager.CheckKeyPress(Keys.OemMinus))
                {
                    ControlManager.RebindKey(false, keyToRebind, Keys.OemMinus, null);
                    tempKey = Keys.OemMinus.ToString();
                    goto end;
                }

            end:
                if (tempKey != "")
                {
                    switch (keyToRebind)
                    {
                        case RebindableKeys.Action1:
                            menuOptions[1, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Action2:
                            menuOptions[2, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Action3:
                            menuOptions[3, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Up:
                            menuOptions[4, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Down:
                            menuOptions[5, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Left:
                            menuOptions[6, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Right:
                            menuOptions[7, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    
                        case RebindableKeys.Pause:
                            menuOptions[8, 1] = tempKey;
                            optionsMenuState.SaveSettings();
                            break;
                    }
                    
                    checkInput = false;
                    //keyToRebind = null;
                }
            }

            #endregion

            #region Gamepad
            else
            {
                if (ControlManager.CheckButtonPress(Buttons.A))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.A);
                    tempKey = Buttons.A.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.B))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.B);
                    tempKey = Buttons.B.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.X))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.X);
                    tempKey = Buttons.X.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.Y))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.Y);
                    tempKey = Buttons.Y.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.Start))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.Start);
                    tempKey = Buttons.Start.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.Back))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.Back);
                    tempKey = Buttons.Back.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftTrigger))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftTrigger);
                    tempKey = Buttons.LeftTrigger.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightTrigger))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightTrigger);
                    tempKey = Buttons.RightTrigger.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftShoulder))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftShoulder);
                    tempKey = Buttons.LeftShoulder.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightShoulder))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightShoulder);
                    tempKey = Buttons.RightShoulder.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftStick))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftStick);
                    tempKey = Buttons.LeftStick.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightStick))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightStick);
                    tempKey = Buttons.RightStick.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.DPadUp))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.DPadUp);
                    tempKey = Buttons.DPadUp.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.DPadDown))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.DPadDown);
                    tempKey = Buttons.DPadDown.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.DPadRight))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.DPadRight);
                    tempKey = Buttons.DPadRight.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.DPadLeft))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.DPadLeft);
                    tempKey = Buttons.DPadLeft.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftThumbstickUp))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftThumbstickUp);
                    tempKey = Buttons.LeftThumbstickUp.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftThumbstickDown))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftThumbstickDown);
                    tempKey = Buttons.LeftThumbstickDown.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftThumbstickRight))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftThumbstickRight);
                    tempKey = Buttons.LeftThumbstickRight.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.LeftThumbstickLeft))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.LeftThumbstickLeft);
                    tempKey = Buttons.LeftThumbstickLeft.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightThumbstickUp))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightThumbstickUp);
                    tempKey = Buttons.RightThumbstickUp.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightThumbstickDown))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightThumbstickDown);
                    tempKey = Buttons.RightThumbstickDown.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightThumbstickRight))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightThumbstickRight);
                    tempKey = Buttons.RightThumbstickRight.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.RightThumbstickLeft))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.RightThumbstickLeft);
                    tempKey = Buttons.RightThumbstickLeft.ToString();
                    goto end;
                }

                else if (ControlManager.CheckButtonPress(Buttons.BigButton))
                {
                    ControlManager.RebindKey(true, keyToRebind, null, Buttons.BigButton);
                    tempKey = Buttons.BigButton.ToString();
                    goto end;
                }

            end:
                if (tempKey != "")
                {
                    switch (keyToRebind)
                    {
                        case RebindableKeys.Action1:
                            menuOptions[1, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Action2:
                            menuOptions[2, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Action3:
                            menuOptions[3, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Up:
                            menuOptions[4, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Down:
                            menuOptions[5, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Left:
                            menuOptions[6, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Right:
                            menuOptions[7, 1] = tempKey;
                            break;
                    
                        case RebindableKeys.Pause:
                            menuOptions[8, 1] = tempKey;
                            break;
                    }
                    
                    checkInput = false;
                    //keyToRebind = "";
                }
            }
            #endregion
        }
    }
}