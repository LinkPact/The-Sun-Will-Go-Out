using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public enum RebindableKeys
    {
        Up,
        Down,
        Left,
        Right,
        Pause,
        Action1,
        Action2,
        Action3,
        Inventory,
        Map,
        Missions,
        Help
    }

    public static class ControlManager
    {
        private static List<RebindableKeys> keys = new List<RebindableKeys>
            {
                RebindableKeys.Action1,
                RebindableKeys.Action2,
                RebindableKeys.Action3,
                RebindableKeys.Down,
                RebindableKeys.Left,
                RebindableKeys.Pause,
                RebindableKeys.Right,
                RebindableKeys.Up,
                RebindableKeys.Inventory,
                RebindableKeys.Map,
                RebindableKeys.Missions,
                RebindableKeys.Help
            };

        // Gamepad
        public static bool IsGamepadConnected;
        public static bool UseGamepad;

        public static GamePadState CurrentGamepadState;
        public static GamePadState PreviousGamepadState;

        public static float ThumbStickAngleX;
        public static float ThumbStickAngleY;

        public static Buttons GamepadUp;
        public static Buttons GamepadDown;
        public static Buttons GamepadRight;
        public static Buttons GamepadLeft;
        public static Buttons GamepadAction;
        public static Buttons GamepadAction2;
        public static Buttons GamepadAction3;
        public static Buttons GamepadPause;

        // Keyboard
        public static KeyboardState CurrentKeyboardState;
        public static KeyboardState PreviousKeyboardState;

        public static Keys KeyboardUp;
        public static Keys KeyboardDown;
        public static Keys KeyboardRight;
        public static Keys KeyboardLeft;
        public static Keys KeyboardPause;
        public static Keys KeyboardAction;
        public static Keys KeyboardAction2;
        public static Keys KeyboardAction3;
        public static Keys KeyboardInventory;
        public static Keys KeyboardMap;
        public static Keys KeyboardMissions;
        public static Keys KeyboardHelp;

        // Mouse
        private static MouseState currentMouseState;
        private static MouseState previouseMouseState;
        private static bool showMouse;

        public static bool GamepadReady
        {
            get 
            {
                return (IsGamepadConnected && UseGamepad);
            }
        }

        public static void Update(GameTime gameTime)
        {
            PreviousGamepadState = CurrentGamepadState;
            CurrentGamepadState = GamePad.GetState(PlayerIndex.One);

            ThumbStickAngleX = CurrentGamepadState.ThumbSticks.Left.X;
            ThumbStickAngleY = CurrentGamepadState.ThumbSticks.Left.Y;

            PreviousKeyboardState = CurrentKeyboardState;
            CurrentKeyboardState = Keyboard.GetState();

            previouseMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            IsGamepadConnected = CurrentGamepadState.IsConnected;

            UseGamepad = false;
        }

		// MAC CHANGE - Change default controls to z, x, y
        public static void SetDefaultControls()
        {
            GamepadUp = Buttons.LeftThumbstickUp;
            GamepadDown = Buttons.LeftThumbstickDown;
            GamepadRight = Buttons.LeftThumbstickRight;
            GamepadLeft = Buttons.LeftThumbstickLeft;
            GamepadAction = Buttons.A;
            GamepadAction2 = Buttons.B;
            GamepadAction3 = Buttons.X; 
            GamepadPause = Buttons.Start;

            KeyboardUp = Keys.Up;
            KeyboardDown = Keys.Down;
            KeyboardRight = Keys.Right;
            KeyboardLeft = Keys.Left;
			KeyboardPause = Keys.Escape;

			KeyboardAction = Keys.Z;
            KeyboardAction2 = Keys.X;
            KeyboardAction3 = Keys.C;

            KeyboardInventory = Keys.I;
            KeyboardMap = Keys.M;
            KeyboardMissions = Keys.L;
            KeyboardHelp = Keys.H;

            showMouse = true;
        }

        public static void LoadControls(SaveFile settingsFile)
        {
            StringBuilder key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "action1", "LeftControl"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardAction);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "action2", "LeftShift"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardAction2);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "action3", "Space"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardAction3);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "up", "Up"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardUp);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "down", "Down"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardDown);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "right", "Right"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardRight);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "left", "Left"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardLeft);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "pause", "Escape"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardPause);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "inv", "I"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardInventory);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "map", "M"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardMap);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "log", "L"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardMissions);

            key = new StringBuilder(settingsFile.GetPropertyAsString("keys", "help", "H"));
            key[0] = Char.ToUpper(key[0]);
            Enum.TryParse<Keys>(key.ToString(), out KeyboardHelp);

			// MAC CHANGE - Change default controls to z, x, y
            if (KeyboardAction == Keys.None)
                KeyboardAction = Keys.Z;

            if (KeyboardAction2 == Keys.None)
                KeyboardAction2 = Keys.X;

            if (KeyboardAction3 == Keys.None)
                KeyboardAction3 = Keys.C;

            if (KeyboardUp == Keys.None)
                KeyboardUp = Keys.Up;

            if (KeyboardDown == Keys.None)
                KeyboardDown = Keys.Down;

            if (KeyboardRight == Keys.None)
                KeyboardRight = Keys.Right;

            if (KeyboardLeft == Keys.None)
                KeyboardLeft = Keys.Left;

            if (KeyboardPause == Keys.None)
                KeyboardPause = Keys.Escape;

            if (KeyboardInventory == Keys.None)
                KeyboardInventory = Keys.I;

            if (KeyboardMap == Keys.None)
                KeyboardMap = Keys.M;

            if (KeyboardMissions == Keys.None)
                KeyboardMissions = Keys.L;

            if (KeyboardHelp == Keys.None)
                KeyboardHelp = Keys.H;

            GamepadUp = Buttons.LeftThumbstickUp;
            GamepadDown = Buttons.LeftThumbstickDown;
            GamepadRight = Buttons.LeftThumbstickRight;
            GamepadLeft = Buttons.LeftThumbstickLeft;
            GamepadAction = Buttons.A;
            GamepadAction2 = Buttons.B;
            GamepadAction3 = Buttons.X;
            GamepadPause = Buttons.Start;

            showMouse = settingsFile.GetPropertyAsBool("game options", "showmouse", true);
        }

        public static bool CheckKeyPress(Keys key)
        {
            if (CurrentKeyboardState.IsKeyDown(key) && PreviousKeyboardState.IsKeyUp(key))
                return true;
            else
                return false;
        }

        public static bool CheckKeyHold(Keys key)
        {
            if (CurrentKeyboardState.IsKeyDown(key))
                return true;
            else
                return false;
        }

        public static bool CheckButtonPress(Buttons button)
        {
            if (CurrentGamepadState.IsButtonDown(button) && PreviousGamepadState.IsButtonUp(button))
                return true;
            else
                return false;
        }

        public static bool CheckPress(RebindableKeys key)
        {
            switch (key)
            {
                case RebindableKeys.Action1:
                    if (GamepadReady)
                        return CheckButtonPress(GamepadAction);
                    else
                        return CheckKeyPress(KeyboardAction);

                case RebindableKeys.Action2:
                    if (GamepadReady)
                        return CheckButtonPress(GamepadAction2);
                    else
                        return CheckKeyPress(KeyboardAction2);

                case RebindableKeys.Action3:
                    if (GamepadReady)
                        return CheckButtonPress(GamepadAction3);
                    else
                        return CheckKeyPress(KeyboardAction3);

                case RebindableKeys.Up:
                    if (GamepadReady)
                    {
                        if (CheckButtonPress(GamepadUp))
                            return true;
                        else
                            return CheckButtonPress(Buttons.DPadUp);
                    }
                    else
                        return CheckKeyPress(KeyboardUp);

                case RebindableKeys.Down:
                    if (GamepadReady)
                    {
                        if (CheckButtonPress(GamepadDown))
                            return true;
                        else
                            return CheckButtonPress(Buttons.DPadDown);
                    }
                    else
                        return CheckKeyPress(KeyboardDown);

                case RebindableKeys.Left:
                    if (GamepadReady)
                    {
                        if (CheckButtonPress(GamepadLeft))
                            return true;
                        else
                            return CheckButtonPress(Buttons.DPadLeft);
                    }
                    else
                        return CheckKeyPress(KeyboardLeft);

                case RebindableKeys.Right:
                    if (GamepadReady)
                    {
                        if (CheckButtonPress(GamepadRight))
                            return true;
                        else
                            return CheckButtonPress(Buttons.DPadRight);
                    }
                    else
                        return CheckKeyPress(KeyboardRight);

                case RebindableKeys.Pause:
                    if (GamepadReady)
                        return CheckButtonPress(GamepadPause);
                    else
                        return CheckKeyPress(KeyboardPause);

                case RebindableKeys.Inventory:
                    if (GamepadReady)
                        return false;
                    else
                        return CheckKeyPress(KeyboardInventory);

                case RebindableKeys.Map:
                    if (GamepadReady)
                        return false;
                    else
                        return CheckKeyPress(KeyboardMap);

                case RebindableKeys.Missions:
                    if (GamepadReady)
                        return false;
                    else
                        return CheckKeyPress(KeyboardMissions);

                case RebindableKeys.Help:
                    if (GamepadReady)
                        return false;
                    else
                        return CheckKeyPress(KeyboardHelp);

                default:
                    return false;
            }
        }

        public static bool CheckHold(RebindableKeys key)
        {
            switch (key)
            {
                case RebindableKeys.Action1:
                    if (GamepadReady)
                        return CurrentGamepadState.IsButtonDown(GamepadAction);
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardAction);

                case RebindableKeys.Action2:
                    if (GamepadReady)
                        return CurrentGamepadState.IsButtonDown(GamepadAction2);
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardAction2);

                case RebindableKeys.Action3:
                    if (GamepadReady)
                        return CurrentGamepadState.IsButtonDown(GamepadAction3);
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardAction3);

                case RebindableKeys.Up:
                    if (GamepadReady)
                    {
                        if (CurrentGamepadState.IsButtonDown(GamepadUp))
                            return true;
                        else
                            return CurrentGamepadState.IsButtonDown(Buttons.DPadUp);
                    }
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardUp);

                case RebindableKeys.Down:
                    if (GamepadReady)
                    {
                        if (CurrentGamepadState.IsButtonDown(GamepadDown))
                            return true;
                        else
                            return CurrentGamepadState.IsButtonDown(Buttons.DPadDown);
                    }
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardDown);

                case RebindableKeys.Left:
                    if (GamepadReady)
                    {
                        if (CurrentGamepadState.IsButtonDown(GamepadLeft))
                            return true;
                        else
                            return CurrentGamepadState.IsButtonDown(Buttons.DPadLeft);
                    }
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardLeft);

                case RebindableKeys.Right:
                    if (GamepadReady)
                    {
                        if (CurrentGamepadState.IsButtonDown(GamepadRight))
                            return true;
                        else
                            return CurrentGamepadState.IsButtonDown(Buttons.DPadRight);
                    }
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardRight);

                case RebindableKeys.Pause:
                    if (GamepadReady)
                        return CurrentGamepadState.IsButtonDown(GamepadPause);
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardPause);

                case RebindableKeys.Inventory:
                    if (GamepadReady)
                        return false;
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardInventory);

                case RebindableKeys.Map:
                    if (GamepadReady)
                        return false;
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardMap);

                case RebindableKeys.Missions:
                    if (GamepadReady)
                        return false;
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardMissions);

                case RebindableKeys.Help:
                    if (GamepadReady)
                        return false;
                    else
                        return CurrentKeyboardState.IsKeyDown(KeyboardHelp);

                default:
                    return false;
            }
        }

        public static bool PreviousKeyUp(RebindableKeys key)
        {
            switch (key)
            {
                case RebindableKeys.Action1:
                    if (GamepadReady)
                        return PreviousGamepadState.IsButtonUp(GamepadAction);
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardAction);

                case RebindableKeys.Action2:
                    if (GamepadReady)
                        return PreviousGamepadState.IsButtonUp(GamepadAction2);
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardAction2);

                case RebindableKeys.Action3:
                    if (GamepadReady)
                        return PreviousGamepadState.IsButtonUp(GamepadAction3);
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardAction3);

                case RebindableKeys.Up:
                    if (GamepadReady)
                    {
                        if (PreviousGamepadState.IsButtonUp(GamepadUp))
                            return true;
                        else
                            return PreviousGamepadState.IsButtonUp(Buttons.DPadUp);
                    }
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardUp);

                case RebindableKeys.Down:
                    if (GamepadReady)
                    {
                        if (PreviousGamepadState.IsButtonUp(GamepadDown))
                            return true;
                        else
                            return PreviousGamepadState.IsButtonUp(Buttons.DPadDown);
                    }
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardDown);

                case RebindableKeys.Left:
                    if (GamepadReady)
                    {
                        if (PreviousGamepadState.IsButtonUp(GamepadLeft))
                            return true;
                        else
                            return PreviousGamepadState.IsButtonUp(Buttons.DPadLeft);
                    }
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardLeft);

                case RebindableKeys.Right:
                    if (GamepadReady)
                    {
                        if (PreviousGamepadState.IsButtonUp(GamepadRight))
                            return true;
                        else
                            return PreviousGamepadState.IsButtonUp(Buttons.DPadRight);
                    }
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardRight);

                case RebindableKeys.Pause:
                    if (GamepadReady)
                        return PreviousGamepadState.IsButtonUp(GamepadPause);
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardPause);

                case RebindableKeys.Inventory:
                    if (GamepadReady)
                        return false;
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardInventory);

                case RebindableKeys.Map:
                    if (GamepadReady)
                        return false;
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardMap);

                case RebindableKeys.Missions:
                    if (GamepadReady)
                        return false;
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardMissions);

                case RebindableKeys.Help:
                    if (GamepadReady)
                        return false;
                    else
                        return PreviousKeyboardState.IsKeyUp(KeyboardHelp);

                default:
                    return false;
            } 
        }

        public static string GetKeyName(RebindableKeys key)
        {
            switch (key)
            {
                case RebindableKeys.Action1:
                    if (GamepadReady)
                        return GamepadAction.ToString();
                    else
                    {
                        return KeyboardAction.ToString();
                    }
                case RebindableKeys.Action2:
                    if (GamepadReady)
                        return GamepadAction2.ToString();
                    else
                        return KeyboardAction2.ToString();

                case RebindableKeys.Action3:
                    if (GamepadReady)
                        return GamepadAction3.ToString();
                    else
                        return KeyboardAction3.ToString();

                case RebindableKeys.Up:
                    if (GamepadReady)
                        return GamepadUp.ToString();
                    else
                        return KeyboardUp.ToString();

                case RebindableKeys.Down:
                    if (GamepadReady)
                        return GamepadDown.ToString();
                    else
                        return KeyboardDown.ToString();

                case RebindableKeys.Left:
                    if (GamepadReady)
                        return GamepadLeft.ToString();
                    else
                        return KeyboardLeft.ToString();

                case RebindableKeys.Right:
                    if (GamepadReady)
                        return GamepadRight.ToString();
                    else
                        return KeyboardRight.ToString();

                case RebindableKeys.Pause:
                    if (GamepadReady)
                        return GamepadPause.ToString();
                    else
                        return KeyboardPause.ToString();

                case RebindableKeys.Inventory:
                    if (GamepadReady)
                        return "I AM ERROR. GAMEPAD CONTROLS NOT IMPLEMENTED";
                    else
                        return KeyboardInventory.ToString();

                case RebindableKeys.Map:
                    if (GamepadReady)
                        return "I AM ERROR. GAMEPAD CONTROLS NOT IMPLEMENTED";
                    else
                        return KeyboardMap.ToString();

                case RebindableKeys.Missions:
                    if (GamepadReady)
                        return "I AM ERROR. GAMEPAD CONTROLS NOT IMPLEMENTED";
                    else
                        return KeyboardMissions.ToString();

                case RebindableKeys.Help:
                    if (GamepadReady)
                        return "I AM ERROR. GAMEPAD CONTROLS NOT IMPLEMENTED";
                    else
                        return KeyboardHelp.ToString();

                default:
                    return "I AM ERROR";
            }
        }

        public static bool RebindKey(bool gamepad, RebindableKeys key)
        {
            Buttons newbutton = Buttons.A;
            Keys newkey;

            // Wait for key input
            if (gamepad)
            {
                throw new NotImplementedException("Gamepad support not implemented!");
            }
            else
            {
                Keys[] pressedKeys = CurrentKeyboardState.GetPressedKeys();
                if (pressedKeys.Length <= 0
                    || PreviousKeyboardState.IsKeyDown(pressedKeys[0]))
                {
                    return false;
                }

                newkey = CurrentKeyboardState.GetPressedKeys()[0];

                if (IsKeyBound(newkey, key))
                {
                    return false;
                }
            }

            switch (key)
            {
                case RebindableKeys.Action1:
                    if (gamepad)
                        GamepadAction = (Buttons)newbutton;
                    else
                        KeyboardAction = (Keys)newkey;
                    break;

                case RebindableKeys.Action2:
                    if (gamepad)
                        GamepadAction2 = (Buttons)newbutton;
                    else
                        KeyboardAction2 = (Keys)newkey;
                    break;

                case RebindableKeys.Action3:
                    if (gamepad)
                        GamepadAction3 = (Buttons)newbutton;
                    else
                        KeyboardAction3 = (Keys)newkey;
                    break;

                case RebindableKeys.Up:
                    if (gamepad)
                        GamepadUp = (Buttons)newbutton;
                    else
                        KeyboardUp = (Keys)newkey;
                    break;

                case RebindableKeys.Down:
                    if (gamepad)
                        GamepadDown = (Buttons)newbutton;
                    else
                        KeyboardDown = (Keys)newkey;
                    break;

                case RebindableKeys.Left:
                    if (gamepad)
                        GamepadLeft = (Buttons)newbutton;
                    else
                        KeyboardLeft = (Keys)newkey;
                    break;

                case RebindableKeys.Right:
                    if (gamepad)
                        GamepadRight = (Buttons)newbutton;
                    else
                        KeyboardRight = (Keys)newkey;
                    break;

                case RebindableKeys.Pause:
                    if (gamepad)
                        GamepadPause = (Buttons)newbutton;
                    else
                        KeyboardPause = (Keys)newkey;
                    break;

                case RebindableKeys.Inventory:
                    if (gamepad)
                        throw new NotImplementedException();
                    else
                        KeyboardInventory = (Keys)newkey;
                    break;

                case RebindableKeys.Map:
                    if (gamepad)
                        throw new NotImplementedException();
                    else
                        KeyboardMap = (Keys)newkey;
                    break;

                case RebindableKeys.Missions:
                    if (gamepad)
                        throw new NotImplementedException();
                    else
                        KeyboardMissions = (Keys)newkey;
                    break;

                case RebindableKeys.Help:
                    if (gamepad)
                        throw new NotImplementedException();
                    else
                        KeyboardHelp = (Keys)newkey;
                    break;

            }

            return true;
        }

        private static Vector2 GetMousePosition()
        {
            return new Vector2(currentMouseState.X, currentMouseState.Y);
        }

        private static Vector2 GetPreviousMousePosition()
        {
            return new Vector2(previouseMouseState.X, previouseMouseState.Y);
        }

        public static bool IsLeftMouseButtonPressed()
        {
            return showMouse && currentMouseState.LeftButton == ButtonState.Pressed;
        }

        public static bool IsLeftMouseButtonClicked()
        {
            return (showMouse && previouseMouseState.LeftButton == ButtonState.Pressed &&
                !IsLeftMouseButtonPressed());
        }

        public static bool IsRightMouseButtonPressed()
        {
            return showMouse && currentMouseState.RightButton == ButtonState.Pressed;
        }

        public static bool IsRightMouseButtonClicked()
        {
            return (showMouse && previouseMouseState.RightButton == ButtonState.Pressed &&
                !IsRightMouseButtonPressed());
        }

        public static bool IsMouseOverArea(Rectangle area)
        {
            return showMouse && CollisionDetection.IsPointInsideRectangle(new Vector2(currentMouseState.X, currentMouseState.Y), area);
        }

        public static bool IsMouseOverText(SpriteFont font, String text, Vector2 textPosition, bool textCentered = true)
        {
            Vector2 textOrigin;
            Vector2 textDimension;
            Rectangle textRect;

            textOrigin = textCentered ? font.MeasureString(text) / 2 : Vector2.Zero;
            textDimension = font.MeasureString(text);
            textRect = new Rectangle((int)(textPosition.X - textOrigin.X), (int)(textPosition.Y - textOrigin.Y),
                    (int)textDimension.X, (int)textDimension.Y);

            return showMouse && CollisionDetection.IsPointInsideRectangle(ControlManager.GetMousePosition(), textRect);
        }

        public static bool IsMouseOverText(SpriteFont font, String text, Vector2 textPosition, Vector2 screenPos, bool textCentered)
        {
            return IsMouseOverText(font, text, textPosition - screenPos, textCentered);
        }

        public static bool IsMouseMoving()
        {
            return ControlManager.GetMousePosition() != ControlManager.GetPreviousMousePosition();
        }

        public static bool IsMouseShown()
        {
            return showMouse;
        }

        public static void ToggleMouseHidden()
        {
            ControlManager.showMouse = !ControlManager.showMouse;
        }

        private static bool IsKeyBound(Keys newkey, RebindableKeys keyToRebind)
        {
            foreach (RebindableKeys key in ControlManager.keys)
            {
                if (!key.Equals(keyToRebind) 
                    && newkey.ToString().ToLower().Equals(GetKeyName(key).ToLower()))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
