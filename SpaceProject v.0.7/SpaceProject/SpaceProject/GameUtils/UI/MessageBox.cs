using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public enum MessageState
    {
        Invisible,
        Message,
        RealtimeMessage,
        MessageWithImage,
        Image,
        Tutorial,
        Menu,
        YesNo,
        SelectionMenu
    }

    public class MessageBox
    {
        #region variables

        private Game1 game;
        private Sprite spriteSheet;

        private MessageState messageState;

        private List<String> textBuffer;
        private Sprite messageCanvas;
        private Vector2 textBoxPosition;
        private Vector2 textPosition;

        private List<string> menuOptions;
        private List<System.Action> menuActions;
        private int cursorIndex;
        private int currentIndexMax;

        private List<String> textStorage;
        private List<String> optionStorage;
        private List<System.Action> actionStorage;
        private int popupDelay;

        private readonly Vector2 RELATIVE_OKAY_BUTTON_POSITION_IMAGE = new Vector2(0, 179);

        private readonly int OPACITY = 230;

        private float time;
        private float realTimeMessageDelay;
        bool scrollingFinished;
        bool flushScrollText;
        private int holdTimer;
        int tempTimer;
        bool displayOnReturn = false;

        #endregion

        #region Properties

        public MessageState MessageState { get { return messageState; } set { messageState = value; } }
        public bool TextBufferEmpty { get { return popupQueue.Count <= 0 && MessageState == MessageState.Invisible; } }

        #endregion

        private List<Popup> popupQueue;

        public MessageBox(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            popupQueue = new List<Popup>();

            menuOptions = new List<string>();
            menuActions = new List<System.Action>();

            holdTimer = game.HoldKeyTreshold;

            displayOnReturn = false;

            optionStorage = new List<String>();
            actionStorage = new List<System.Action>();
        }

        public void DisplayRealtimeMessage(float delay, params string[] messages)
        {
            RealTimeMessage realTimeMessage = new RealTimeMessage(game, spriteSheet);

            realTimeMessage.Initialize();
            realTimeMessage.SetMessage(messages);
            realTimeMessage.SetDelay(delay);

            popupQueue.Add(realTimeMessage);

            menuOptions.Clear();
        }

        //Call this method, feed in a string and the message will appear on screen 
        public void DisplayMessage(int delay = 0, params string[] messages)
        {
            TextMessage textMessage = new TextMessage(game, spriteSheet);

            textMessage.Initialize();
            textMessage.SetMessage(messages);
            textMessage.SetDelay(delay);

            popupQueue.Add(textMessage);

            tempTimer = 5;

            menuOptions.Clear();
        }

        public void DisplayMessageWithImage(List<Sprite> images, List<int> imageTriggers, params string[] messages)
        {
            if (messages.Length < images.Count)
            {
                throw new ArgumentException("At least one string per image is required.");
            }

            if (images.Count - 1 != imageTriggers.Count)
            {
                throw new ArgumentException("One trigger is required for each image except the first.");
            }

            ImageMessage imageMessage = new ImageMessage(game, spriteSheet);

            imageMessage.Initialize();
            imageMessage.SetMessage(messages);
            imageMessage.SetImages(images, imageTriggers);

            tempTimer = 5;

            menuOptions.Clear();
        }

        public void DisplayImage(params Sprite[] images)
        {
            ImagePopup imagePopup = new ImagePopup(game, spriteSheet);

            imagePopup.Initialize();
            imagePopup.SetImages(images);

            tempTimer = 5;

            menuOptions.Clear();
        }

        //Displays the "overmenu"
        public void DisplayMenu()
        {
            if (!displayOnReturn)
                cursorIndex = 0;

            messageState = MessageState.Menu;

            Game1.Paused = true;

            menuOptions.Clear();

            if (GameStateManager.currentState != "ShooterState")
            {
                menuOptions.Add("Help");
                menuOptions.Add("Ship Inventory");
                menuOptions.Add("Missions Screen");
                menuOptions.Add("Options");
                menuOptions.Add("Save");
                menuOptions.Add("Exit Game");
                menuOptions.Add("Return To Game");
            }

            else
            {
                menuOptions.Add("Restart Level");
                menuOptions.Add("Give Up Level");
                menuOptions.Add("Exit Game");
                menuOptions.Add("Return To Game");
            }

            currentIndexMax = menuOptions.Count;

            tempTimer = 5;
        }

        public void DisplaySelectionMenu(string message, List<String> options)
        {
            messageState = MessageState.SelectionMenu;
            Game1.Paused = true;
            textBuffer.Add(message);
            tempTimer = 5;

            menuOptions.Clear();

            for (int i = 0; i < options.Count; i++)
            {
                menuOptions.Add(options[i]);
            }
        }

        public void DisplaySelectionMenu(string message, List<String> options, List<System.Action> actions)
        {
            // TODO: Make more general
            if (MissionManager.MissionEventBuffer.Count > 0)
            {
                textStorage.Clear();
                textStorage.Add(message);
                optionStorage = options;
                actionStorage = actions;
                return;
            }

            messageState = MessageState.SelectionMenu;
            Game1.Paused = true;
            textBuffer.Add(message);
            tempTimer = 5;

            menuOptions.Clear();

            if (options.Count > actions.Count)
            {
                throw new ArgumentException("Actions needed for each menu option.");
            }

            for (int i = 0; i < options.Count; i++)
            {
                menuOptions.Add(options[i]);
                menuActions.Add(actions[i]);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (popupQueue.Count > 0
                && MessageState == MessageState.Invisible)
            {
                popupQueue[0].Show();
            }

            if (MessageState == MessageState.RealtimeMessage
                && StatsManager.PlayTime.HasOverworldTimePassed(time))
            {
                HideMessage();
            }

            if (actionStorage.Count > 0)
            {
                if (MissionManager.MissionEventBuffer.Count <= 0
                    && game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
                {
                    DisplaySelectionMenu(textStorage[0], optionStorage, actionStorage);

                    textStorage.Clear();
                    optionStorage.Clear();
                    actionStorage.Clear();
                }
            }

            if (messageState != MessageState.Invisible)
            {
                ButtonControls(gameTime);
                if (GameStateManager.currentState == "OptionsMenuState")
                {
                    //MouseControls();
                }
            }

            //Makes the messagebox invisible when pressing the esc or back if it's displaying the menu
            else if (messageState == MessageState.Menu)
            {
                if (tempTimer < 0 && ControlManager.CheckPress(RebindableKeys.Pause) ||
                    ControlManager.CheckPress(RebindableKeys.Action2) /*&& messageState != MessageState.Inventory*/)
                {
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                }
            }

            //displays the overmenu automaticly when returning from the inventory or mission screen 
            if (displayOnReturn == true)
            {
                if (GameStateManager.currentState.Equals("OverworldState"))
                {
                    DisplayMenu();
                    displayOnReturn = false;
                }
            }

            tempTimer--;
        }

        //The different actions that happens when you move items in different ways.
        //(In the inventory popup)
        //private void InventoryAction()
        //{
        //    if (cursorColumn == 0 && pickedColumn == 0 && cursorIndex < throwList.Count)
        //    {
        //        int pos1 = pickedIndex;
        //        int pos2 = cursorIndex;
        //
        //        Item tempItem1 = throwList[pos1];
        //        Item tempItem2 = throwList[pos2];
        //
        //        throwList.RemoveAt(pos1);
        //        throwList.Insert(pos1, tempItem2);
        //        throwList.RemoveAt(pos2);
        //        throwList.Insert(pos2, tempItem1);
        //
        //        menuOptions.Clear();
        //
        //        foreach (Item item in throwList)
        //            menuOptions.Add(item.Name);
        //
        //        menuOptions.Add("Finish");
        //    }
        //    else if (pickedColumn == 0 && cursorColumn != 0)
        //    {
        //        int pos1 = pickedIndex;
        //        int pos2;
        //
        //        if (cursorColumn == 1) pos2 = cursorIndex;
        //        else pos2 = cursorIndex + 14;
        //
        //        Item tempItem1 = throwList[pos1];
        //        Item tempItem2 = ShipInventoryManager.ShipItems[pos2];
        //
        //        throwList.RemoveAt(pos1);
        //        throwList.Insert(pos1, tempItem2);
        //        ShipInventoryManager.RemoveItemAt(pos2);
        //        ShipInventoryManager.InsertItem(pos2, tempItem1);
        //
        //        menuOptions.Clear();
        //
        //        foreach (Item item in throwList)
        //            menuOptions.Add(item.Name);
        //
        //        menuOptions.Add("Finish");
        //    }
        //    else if (pickedColumn != 0 && cursorColumn == 0)
        //    {
        //        int pos1;
        //        int pos2 = cursorIndex;
        //
        //        if (pickedColumn == 1) pos1 = pickedIndex;
        //        else pos1 = pickedIndex + 14;
        //
        //        Item tempItem1 = ShipInventoryManager.ShipItems[pos1];
        //        Item tempItem2 = throwList[pos2];
        //
        //        throwList.RemoveAt(pos2);
        //        throwList.Insert(pos2, tempItem1);
        //        ShipInventoryManager.RemoveItemAt(pos1);
        //        ShipInventoryManager.InsertItem(pos1, tempItem2);
        //
        //        menuOptions.Clear();
        //
        //        foreach (Item item in throwList)
        //            menuOptions.Add(item.Name);
        //
        //        menuOptions.Add("Finish");
        //    }
        //    else if (cursorColumn != 0 && pickedColumn != 0)
        //    {
        //        int position1;
        //        int position2;
        //
        //        if (pickedColumn == 1) position1 = pickedIndex;
        //        else position1 = pickedIndex + 14;
        //
        //        if (cursorColumn == 1) position2 = cursorIndex;
        //        else position2 = cursorIndex + 14;
        //
        //        ShipInventoryManager.SwitchItems(position1, position2);
        //    }
        //}

        //Control over the inventory.
        //private void InventoryCursorControls(GameTime gameTime)
        //{
        //    if (ControlManager.CheckPress(RebindableKeys.Action2))
        //    {
        //        isPicked = false;
        //    }
        //
        //    if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
        //    {
        //        if (!isPicked)
        //        {
        //            if (!(cursorColumn == 0 && cursorIndex == throwList.Count))
        //            {
        //                isPicked = true;
        //                pickedIndex = cursorIndex;
        //                pickedColumn = cursorColumn;
        //            }
        //            else if (cursorColumn == 0 && cursorIndex == throwList.Count)
        //            {
        //                Game1.Paused = false;
        //                messageState = MessageState.Invisible;
        //            }
        //        }
        //        else
        //        {
        //            isPicked = false;
        //
        //            //Prevents InventoryAction from attempting to move the finish-slot.
        //            if (!(cursorColumn == 0 && cursorIndex == throwList.Count))
        //                //LOGIC - THIS IS WHERE THE MAGIC HAPPENS
        //                InventoryAction();
        //        }
        //    }
        //
        //    if (ControlManager.CheckPress(RebindableKeys.Down))
        //    {
        //        cursorIndex++;
        //        holdTimer = Game.HoldKeyTreshold;
        //    }
        //
        //    else if (ControlManager.CheckHold(RebindableKeys.Down))
        //    {
        //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
        //
        //        if (holdTimer <= 0)
        //        {
        //            cursorIndex++;
        //            holdTimer = Game.ScrollSpeedFast;
        //        }
        //    }
        //
        //    else if (ControlManager.CheckPress(RebindableKeys.Up))
        //    {
        //        cursorIndex--;
        //        holdTimer = Game.HoldKeyTreshold;
        //    }
        //
        //    else if (ControlManager.CheckHold(RebindableKeys.Up))
        //    {
        //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
        //
        //        if (holdTimer <= 0)
        //        {
        //            cursorIndex--;
        //            holdTimer = Game.ScrollSpeedFast;
        //        }
        //    }
        //
        //    if (ControlManager.CheckPress(RebindableKeys.Right) && cursorColumn < 2)
        //    {
        //        cursorColumn++;
        //
        //        if (cursorColumn == 1)
        //        {
        //            if (inventoryRef.Count <= 14)
        //                currentIndexMax = inventoryRef.Count - 1;
        //            else
        //                currentIndexMax = 14;
        //        }
        //        else if (cursorColumn == 2)
        //        {
        //            currentIndexMax = inventoryRef.Count - 14;
        //        }
        //
        //        holdTimer = Game.HoldKeyTreshold;
        //    }
        //
        //    else if (ControlManager.CheckHold(RebindableKeys.Right) && cursorColumn < 2)
        //    {
        //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
        //
        //        if (holdTimer <= 0)
        //        {
        //            cursorColumn++;
        //
        //            if (cursorColumn == 1)
        //            {
        //                if (inventoryRef.Count <= 14)
        //                    currentIndexMax = inventoryRef.Count - 1;
        //                else
        //                    currentIndexMax = 14;
        //            }
        //            else if (cursorColumn == 2)
        //            {
        //                currentIndexMax = inventoryRef.Count - 14;
        //            }
        //
        //            holdTimer = Game.ScrollSpeedSlow;
        //        }
        //    }
        //
        //    if (ControlManager.CheckPress(RebindableKeys.Left) && cursorColumn > 0)
        //    {
        //        cursorColumn--;
        //
        //        if (cursorColumn == 0)
        //            currentIndexMax = menuOptions.Count;
        //        else if (cursorColumn == 1)
        //        {
        //            if (inventoryRef.Count <= 14)
        //                currentIndexMax = inventoryRef.Count - 1;
        //            else
        //                currentIndexMax = 14;
        //        }
        //
        //        holdTimer = Game.HoldKeyTreshold;
        //    }
        //
        //    else if (ControlManager.CheckHold(RebindableKeys.Left) && cursorColumn > 0)
        //    {
        //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
        //
        //        if (holdTimer <= 0)
        //        {
        //            cursorColumn--;
        //
        //            if (cursorColumn == 0)
        //                currentIndexMax = menuOptions.Count;
        //            else if (cursorColumn == 1)
        //            {
        //                if (inventoryRef.Count <= 14)
        //                    currentIndexMax = inventoryRef.Count - 1;
        //                else
        //                    currentIndexMax = 14;
        //            }
        //
        //            holdTimer = Game.ScrollSpeedSlow;
        //        }
        //    }
        //
        //
        //    if (cursorIndex > currentIndexMax - 1)
        //    {
        //        if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
        //            cursorIndex = 0;
        //        else
        //            cursorIndex = currentIndexMax - 1;
        //    }
        //
        //    else if (cursorIndex < 0)
        //    {
        //        if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
        //            cursorIndex = currentIndexMax - 1;
        //        else
        //            cursorIndex = 0;
        //    }
        //}
        private void ButtonControls(GameTime gameTime)
        {
            if (messageState == MessageState.Menu)
            {
                if (ControlManager.CheckPress(RebindableKeys.Right))
                {
                    cursorIndex++;
                    holdTimer = game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Right))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex++;
                        holdTimer = game.ScrollSpeedSlow;
                    }
                }

                else if (ControlManager.CheckPress(RebindableKeys.Left))
                {
                    cursorIndex--;
                    holdTimer = game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Left))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorIndex--;
                        holdTimer = game.ScrollSpeedSlow;
                    }
                }

                if (cursorIndex > menuOptions.Count - 1)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Right))
                        cursorIndex = 0;
                    else cursorIndex = menuOptions.Count - 1;
                }

                else if (cursorIndex < 0)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Left))
                        cursorIndex = menuOptions.Count - 1;
                    else
                        cursorIndex = 0;
                }
            }

            else if (messageState == MessageState.SelectionMenu)
            {
                if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    cursorIndex--;
                }

                else if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    cursorIndex++;
                }

                if (cursorIndex > menuOptions.Count - 1)
                {
                    cursorIndex = 0;
                }

                else if (cursorIndex < 0)
                {
                    cursorIndex = menuOptions.Count - 1;
                }
            }

            if (((ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeypress(Keys.Enter))
                || (GameStateManager.currentState == "MainMenuState" && ControlManager.IsLeftMouseButtonClicked() && messageState == MessageState.Message)
                && tempTimer <= 0))
            {
                ButtonActions();
            }
        }

        //Called when atempting to go back
        private void HideMessage()
        {
            TextUtils.RefreshTextScrollBuffer();

            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message
            if (scrollingFinished 
                && messageState == MessageState.Message && tempTimer < 0)
            {
                textBuffer.Remove(textBuffer[0]);

                if (textBuffer.Count <= 0)
                {
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                }

                else
                {
                    TextToSpeech.Speak(textBuffer[0]);
                }

                scrollingFinished = false;
                flushScrollText = false;
            }

            //else if (scrollingFinished
            //    && messageState == MessageState.MessageWithImage && tempTimer < 0)
            //{
            //    textBuffer.RemoveAt(0);
            //
            //    if (textBuffer.Count <= 0)
            //    {
            //        Game1.Paused = false;
            //        messageState = MessageState.Invisible;
            //    }
            //
            //    else
            //    {
            //        imageTriggerCount++;
            //
            //        if (imageTriggers.Count > 0 &&
            //            imageTriggerCount == imageTriggers[0])
            //        {
            //            imageTriggers.RemoveAt(0);
            //            imageBuffer.RemoveAt(0);
            //        }
            //    }
            //
            //    scrollingFinished = false;
            //    flushScrollText = false;
            //}
            //
            //else if (messageState == MessageState.Image
            //    && tempTimer < 0)
            //{
            //    if (imageBuffer.Count <= 1)
            //    {
            //        imageBuffer.Clear();
            //        Game1.Paused = false;
            //        messageState = MessageState.Invisible;
            //    }
            //
            //    else
            //    {
            //        imageBuffer.RemoveAt(0);
            //    }
            //}
        }

        private void ButtonActions()
        {
            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message
            if ((messageState == MessageState.Message
                || messageState == MessageState.MessageWithImage
                || messageState == MessageState.Image)
                && tempTimer < 0)
            {
                TextToSpeech.Stop();

                if (messageState != MessageState.Image
                    && !scrollingFinished)
                {
                    flushScrollText = true;
                }

                //if (useDisableTutorialButton)
                //{
                //    switch (cursorIndex)
                //    {
                //        case 0:
                //            HideMessage();
                //            break;
                //
                //        case 1:
                //            game.tutorialManager.TutorialsUsed = !game.tutorialManager.TutorialsUsed;
                //            UpdateButtonLabels();
                //            ClearBuffers();
                //            break;
                //
                //        default:
                //            throw new IndexOutOfRangeException("Cursor index is out of range.");
                //    }
                //}
                //else
                //{
                //    HideMessage();
                //}
            }

            //checks which option the user selects and executes it.  
            else if (messageState == MessageState.Menu && tempTimer < 0)
            {
                switch (menuOptions[cursorIndex])
                {
                    case "Ship Inventory":
                        game.stateManager.ChangeState("ShipManagerState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Missions Screen":
                        game.stateManager.ChangeState("MissionScreenState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Exit Game":
                        if (GameStateManager.currentState.Equals("OverworldState"))
                        {
                            DisplaySelectionMenu("What do you want to do?",
                                new List<string> { "Save and exit to menu", "Save and exit to desktop", "Exit to menu without saving",
                                "Exit to desktop without saving", "Cancel"});
                        }

                        else if (GameStateManager.currentState.Equals("ShooterState"))
                        {
                            DisplaySelectionMenu("What do you want to do? You cannot save during combat.",
                                new List<string> { "Exit to menu without saving", "Exit to desktop without saving", "Cancel" });
                        }
                        break;

                    case "Options":
                        game.stateManager.ChangeState("OptionsMenuState");
                        game.menuBGController.SetBackdropPosition(new Vector2(-903, -101));
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Save":
                        game.Save();
                        break;

                    case "Help":
                        game.stateManager.ChangeState("HelpScreenState");
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        displayOnReturn = true;
                        break;

                    case "Return To Game":
                        messageState = MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Restart Level":
                        game.stateManager.shooterState.CurrentLevel.ResetLevel();
                        game.stateManager.shooterState.Initialize();
                        game.stateManager.shooterState.CurrentLevel.Initialize();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Give Up Level":
                        game.stateManager.shooterState.CurrentLevel.GiveUpLevel();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;

                    case "Exit Level":
                        game.stateManager.shooterState.CurrentLevel.LeaveLevel();
                        messageState = SpaceProject.MessageState.Invisible;
                        Game1.Paused = false;
                        break;
                }
            }

            else if (messageState == MessageState.SelectionMenu && tempTimer < 0)
            {
                if (menuActions.Count <= 0)
                {
                    switch (menuOptions[cursorIndex].ToLower())
                    {
                        case "save and exit to menu":
                        case "save and restart":
                            game.GameStarted = false;
                            textBuffer.Remove(textBuffer[0]);
                            messageState = MessageState.Invisible;
                            Game1.Paused = false;
                            game.Save();
                            game.Restart();
                            break;

                        case "save and exit to desktop":
                            game.Save();
                            game.Exit();
                            break;

                        case "exit to menu without saving":
                        case "restart":
                            game.GameStarted = false;
                            textBuffer.Remove(textBuffer[0]);
                            messageState = MessageState.Invisible;
                            Game1.Paused = false;
                            game.Restart();
                            break;

                        case "exit to desktop without saving":
                            game.Exit();
                            break;

                        case "cancel":
                            textBuffer.Remove(textBuffer[0]);
                            Game1.Paused = false;
                            messageState = MessageState.Invisible;

                            if (GameStateManager.currentState.Equals("OverworldState"))
                            {
                                DisplayMenu();
                                cursorIndex = 5;
                            }
                            break;

                    }
                }

                else
                {
                    menuActions[cursorIndex].Invoke();
                    textBuffer.Remove(textBuffer[0]);
                    Game1.Paused = false;
                    messageState = MessageState.Invisible;
                    menuActions.Clear();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            String text;

            switch (messageState)
            {
                case MessageState.Message:
                    {
                        DrawMenuOptions(spriteBatch);
                    }
                    break;

                case MessageState.RealtimeMessage:
                    {
                       
                    }
                    break;

                case MessageState.MessageWithImage:
                    {
                        DrawMenuOptions(spriteBatch);
                        break;
                    }

                case MessageState.Image:
                    {
                        DrawMenuOptions(spriteBatch);
                        break;
                    }

                case MessageState.SelectionMenu:
                    {
                        spriteBatch.Draw(messageCanvas.Texture,
                             textBoxPosition,
                             messageCanvas.SourceRectangle,
                             new Color(255, 255, 255, OPACITY),
                             0.0f,
                             new Vector2(messageCanvas.SourceRectangle.Value.Width / 2,
                                         messageCanvas.SourceRectangle.Value.Height / 2),
                             1.5f,
                             SpriteEffects.None,
                             0.95f);

                        text = TextUtils.WordWrap(game.fontManager.GetFont(14),
                                                    TextUtils.ScrollText(textBuffer[0], flushScrollText, out scrollingFinished),
                                                    (int)Math.Round((messageCanvas.SourceRectangle.Value.Width * 1.45) - 25, 0));

                        spriteBatch.DrawString(game.fontManager.GetFont(14),
                                                text,
                                                new Vector2(textPosition.X,
                                                            textPosition.Y) + game.fontManager.FontOffset,
                                                game.fontManager.FontColor,
                                                0f,
                                                Vector2.Zero,
                                                1f,
                                                SpriteEffects.None,
                                                1f);

                        DrawMenuOptions(spriteBatch);
                    }
                    break;

                case MessageState.Menu:
                    {
                        Vector2 pos;
                        if (GameStateManager.currentState == "OverworldState")
                        {
                            pos = new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2,
                                              game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2);
                        }
                        else
                        {
                            pos = Vector2.Zero;
                        }

                        spriteBatch.Draw(messageCanvas.Texture,
                             pos,
                             messageCanvas.SourceRectangle,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(game.Window.ClientBounds.Width, .15f),
                             SpriteEffects.None,
                             0.95f);

                        DrawMenuOptions(spriteBatch);
                    }
                    break;
            }
        }

        private void DrawMenuOptions(SpriteBatch spriteBatch)
        {
            Vector2 menuOptionPos = Vector2.Zero;
            Vector2 okayButtonOffset = Vector2.Zero;
            Vector2 showTutorialButtonOffset = Vector2.Zero;

            int numberOfMenuOptions = menuOptions.Count;
            int ySpacing = 30;

            if (numberOfMenuOptions == 5)
            {
                ySpacing = -30;
            }

            if (messageState == MessageState.Message
                || messageState == MessageState.MessageWithImage 
                || messageState == MessageState.Image)
            {
                switch (messageState)
                {
                    case MessageState.Message:
                        menuOptionPos = new Vector2(textPosition.X + 136, textPosition.Y + 99);
                        okayButtonOffset = new Vector2(0, 109);;
                        showTutorialButtonOffset = new Vector2(140, 109);
                        break;

                    case MessageState.MessageWithImage:
                        menuOptionPos = new Vector2(textPosition.X + 171, textPosition.Y - 38);
                        okayButtonOffset = new Vector2(0, 179);;
                        showTutorialButtonOffset = new Vector2(140, 179);;
                        break;

                    case MessageState.Image:
                        menuOptionPos = new Vector2(textPosition.X + 171, textPosition.Y - 38);
                        okayButtonOffset = new Vector2(0, 109);;
                        showTutorialButtonOffset = new Vector2(140, 109);
                        break;
                }
                //if (useDisableTutorialButton)
                //{
                //    //loops through the menu options and colors the selected one red
                //    for (int i = 0; i < menuOptions.Count; i++)
                //    {
                //        if (i == cursorIndex)
                //        {
                //            if (menuOptions[i].ToLower().Equals("show"))
                //            {
                //                spriteBatch.Draw(activeTutorialButton.Texture,
                //                    new Vector2(textBoxPos.X, textBoxPos.Y) + showTutorialButtonOffset,
                //                    activeTutorialButton.SourceRectangle,
                //                    Color.White,
                //                    0f,
                //                    new Vector2(activeTutorialButton.SourceRectangle.Value.Width / 2,
                //                        activeTutorialButton.SourceRectangle.Value.Height / 2),
                //                    1f,
                //                    SpriteEffects.None,
                //                    0.975f);
                //
                //                spriteBatch.DrawString(game.fontManager.GetFont(14),
                //                                     menuOptions[i],
                //                                     new Vector2(menuOptionPos.X + (i * 140),
                //                                             menuOptionPos.Y) + game.fontManager.FontOffset + okayButtonOffset,
                //                                     Color.LightBlue,
                //                                     0f,
                //                                     game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                //                                     1f,
                //                                     SpriteEffects.None,
                //                                     1f);
                //            }
                //
                //            else
                //            {
                //                spriteBatch.Draw(buttonSelected.Texture,
                //                    new Vector2(textBoxPos.X, textBoxPos.Y) + okayButtonOffset,
                //                    buttonSelected.SourceRectangle,
                //                    Color.White,
                //                    0f,
                //                    new Vector2(buttonSelected.SourceRectangle.Value.Width / 2,
                //                        buttonSelected.SourceRectangle.Value.Height / 2),
                //                    1f,
                //                    SpriteEffects.None,
                //                    0.975f);
                //
                //                spriteBatch.DrawString(game.fontManager.GetFont(14),
                //                                     menuOptions[i],
                //                                     new Vector2(menuOptionPos.X + (i * 140),
                //                                             menuOptionPos.Y) + game.fontManager.FontOffset + okayButtonOffset,
                //                                     Color.LightBlue,
                //                                     0f,
                //                                     game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                //                                     1f,
                //                                     SpriteEffects.None,
                //                                     1f);
                //            }
                //        }
                //
                //        else
                //        {
                //            if (menuOptions[i].ToLower().Equals("show"))
                //            {
                //                spriteBatch.Draw(activeTutorialButton.Texture,
                //                    new Vector2(textBoxPos.X, textBoxPos.Y) + showTutorialButtonOffset,
                //                    activeTutorialButton.SourceRectangle,
                //                    Color.White,
                //                    0f,
                //                    new Vector2(activeTutorialButton.SourceRectangle.Value.Width / 2,
                //                        activeTutorialButton.SourceRectangle.Value.Height / 2),
                //                    1f,
                //                    SpriteEffects.None,
                //                    0.975f);
                //
                //                spriteBatch.DrawString(game.fontManager.GetFont(14),
                //                    menuOptions[i],
                //                    new Vector2(menuOptionPos.X + (i * 140),
                //                        menuOptionPos.Y) + game.fontManager.FontOffset + okayButtonOffset,
                //                    game.fontManager.FontColor,
                //                    0f,
                //                    game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                //                    1f,
                //                    SpriteEffects.None,
                //                    1f);
                //            }
                //
                //            else
                //            {
                //                spriteBatch.Draw(buttonUnselected.Texture,
                //                    new Vector2(textBoxPos.X, textBoxPos.Y) + okayButtonOffset,
                //                    buttonUnselected.SourceRectangle,
                //                    Color.White,
                //                    0f,
                //                    new Vector2(buttonUnselected.SourceRectangle.Value.Width / 2,
                //                        buttonUnselected.SourceRectangle.Value.Height / 2),
                //                    1f,
                //                    SpriteEffects.None,
                //                    0.975f);
                //
                //                spriteBatch.DrawString(game.fontManager.GetFont(14),
                //                    menuOptions[i],
                //                    new Vector2(menuOptionPos.X + (i * 140),
                //                        menuOptionPos.Y) + game.fontManager.FontOffset + okayButtonOffset,
                //                    game.fontManager.FontColor,
                //                    0f,
                //                    game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                //                    1f,
                //                    SpriteEffects.None,
                //                    1f);
                //            }
                //        }
                //
                //    }
                //}
                //
                //else
                //{
                //    spriteBatch.Draw(buttonSelected.Texture,
                //                    new Vector2(textBoxPos.X, textBoxPos.Y) + okayButtonOffset,
                //                    buttonSelected.SourceRectangle,
                //                    Color.White,
                //                    0f,
                //                    new Vector2(buttonSelected.SourceRectangle.Value.Width / 2,
                //                        buttonSelected.SourceRectangle.Value.Height / 2),
                //                    1f,
                //                    SpriteEffects.None,
                //                    0.975f);
                //
                //    spriteBatch.DrawString(game.fontManager.GetFont(14),
                //                                     "Okay",
                //                                     new Vector2(menuOptionPos.X,
                //                                             menuOptionPos.Y) + game.fontManager.FontOffset + okayButtonOffset,
                //                                     Color.LightBlue,
                //                                     0f,
                //                                     game.fontManager.GetFont(14).MeasureString("Okay") / 2,
                //                                     1f,
                //                                     SpriteEffects.None,
                //                                     1f);
                //
                //    
                //}
            }

            else if (messageState == MessageState.SelectionMenu)
            {
                menuOptionPos = new Vector2(textBoxPosition.X,
                    textPosition.Y + 75);

                //loops through the menu options and colors the selected one red
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == cursorIndex)
                        spriteBatch.DrawString(game.fontManager.GetFont(14),
                                             menuOptions[i],
                                             new Vector2(menuOptionPos.X,
                                                    menuOptionPos.Y + ySpacing + (i * 30)) + game.fontManager.FontOffset,
                                             Color.LightBlue,
                                             0f,
                                             game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                                             1f,
                                             SpriteEffects.None,
                                             1f);

                    else
                        spriteBatch.DrawString(game.fontManager.GetFont(14),
                                                 menuOptions[i],
                                                 new Vector2(menuOptionPos.X,
                                                    menuOptionPos.Y + ySpacing + (i * 30)) + game.fontManager.FontOffset,
                                                 game.fontManager.FontColor,
                                                 0f,
                                                 game.fontManager.GetFont(14).MeasureString(menuOptions[i]) / 2,
                                                 1f,
                                                 SpriteEffects.None,
                                                 1f);

                }
            }

            else if (messageState == MessageState.Menu)
            {
                Vector2 pos;
                if (GameStateManager.currentState == "OverworldState")
                    pos = new Vector2(game.camera.cameraPos.X - game.Window.ClientBounds.Width / 2,
                                      game.camera.cameraPos.Y - game.Window.ClientBounds.Height / 2 + 4);

                else
                {
                    pos = new Vector2(0, 4);
                }

                float XposAcc = 5;

                //loops through the menu options and colors the selected one red
                for (int i = 0; i < menuOptions.Count; i++)
                {
                    if (i == cursorIndex)
                        spriteBatch.DrawString(game.fontManager.GetFont(14),
                                             menuOptions[i],
                                             new Vector2(pos.X + XposAcc, pos.Y) + game.fontManager.FontOffset,
                                             Color.LightBlue,
                                             0f,
                                             Vector2.Zero,
                                             1f,
                                             SpriteEffects.None,
                                             1f);

                    else
                        spriteBatch.DrawString(game.fontManager.GetFont(14),
                                                 menuOptions[i],
                                                 new Vector2(pos.X + XposAcc, pos.Y) + game.fontManager.FontOffset,
                                                 game.fontManager.FontColor,
                                                 0f,
                                                 Vector2.Zero,
                                                 1f,
                                                 SpriteEffects.None,
                                                 1f);

                    XposAcc += game.fontManager.GetFont(14).MeasureString(menuOptions[i]).X + 15;
                }
            }
        }

        private void ClearBuffers()
        {
            List<String> tempTextBuffer = new List<String>();

            for (int i = 1; i < textBuffer.Count; i++)
            {
                tempTextBuffer.Add(textBuffer[i]);
            }

            foreach (String str in tempTextBuffer)
            {
                textBuffer.Remove(str);
            }

            tempTextBuffer.Clear();
        }
    }
}