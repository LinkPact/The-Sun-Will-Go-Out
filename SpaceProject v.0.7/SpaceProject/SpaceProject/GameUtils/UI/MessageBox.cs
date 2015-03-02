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

        private readonly Vector2 RELATIVE_OKAY_BUTTON_POSITION_IMAGE = new Vector2(0, 179);
        private readonly int OPACITY = 230;

        private Game1 game;
        private Sprite spriteSheet;

        private MessageState messageState;

        private float time;
        private float realTimeMessageDelay;
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

            displayOnReturn = false;
        }

        public void DisplayRealtimeMessage(float delay, params string[] messages)
        {
            RealTimeMessage realTimeMessage = new RealTimeMessage(game, spriteSheet);

            realTimeMessage.Initialize();
            realTimeMessage.SetMessage(messages);
            realTimeMessage.SetDelay(delay);

            popupQueue.Add(realTimeMessage);
        }

        //Call this method, feed in a string and the message will appear on screen 
        public void DisplayMessage(int delay = 0, params string[] messages)
        {
            TextMessage textMessage = new TextMessage(game, spriteSheet);

            textMessage.Initialize();
            textMessage.SetMessage(messages);
            textMessage.SetDelay(delay);

            popupQueue.Add(textMessage);
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

            popupQueue.Add(imageMessage);
        }

        public void DisplayImage(params Sprite[] images)
        {
            ImagePopup imagePopup = new ImagePopup(game, spriteSheet);

            imagePopup.Initialize();
            imagePopup.SetImages(images);

            popupQueue.Add(imagePopup);
        }

        //Displays the "overmenu"
        public void DisplayMenu()
        {
            Menu menu = new Menu(game, spriteSheet);
            menu.Initialize();

            if (GameStateManager.currentState != "ShooterState")
            {
                menu.SetMenuOptions("Help", "Ship Inventory", "Mission Screen", "Options",
                    "Save", "Exit Game", "Return To Game");
            }
            else
            {
                menu.SetMenuOptions("Restart Level", "Give Up Level", "Exit Game", "Return To Game");
            }

            popupQueue.Add(menu);
        }

        public void DisplaySelectionMenu(string message, List<String> options, List<System.Action> actions)
        {
            //// TODO: Make more general
            //if (MissionManager.MissionEventBuffer.Count > 0)
            //{
            //    textStorage.Clear();
            //    textStorage.Add(message);
            //    optionStorage = options;
            //    actionStorage = actions;
            //    return;
            //}

            if (actions.Count > 0 
                && options.Count != actions.Count)
            {
                throw new ArgumentException("Actions needed for each menu option.");
            }

            SelectionMenu selectionMenu = new SelectionMenu(game, spriteSheet);
            selectionMenu.Initialize();

            selectionMenu.SetMessage(message);
            selectionMenu.SetMenuOptions(options.ToArray());
            selectionMenu.SetMenuActions(actions.ToArray());

            popupQueue.Add(selectionMenu);
        }

        public void Update(GameTime gameTime)
        {
            //if (actionStorage.Count > 0)
            //{
            //    if (MissionManager.MissionEventBuffer.Count <= 0
            //        && game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            //    {
            //        DisplaySelectionMenu(textStorage[0], optionStorage, actionStorage);
            //
            //        textStorage.Clear();
            //        optionStorage.Clear();
            //        actionStorage.Clear();
            //    }
            //}

            //displays the overmenu automaticly when returning from the inventory or mission screen 
            if (displayOnReturn == true)
            {
                if (GameStateManager.currentState.Equals("OverworldState"))
                {
                    DisplayMenu();
                    displayOnReturn = false;
                }
            }
        }

        private void ButtonControls(GameTime gameTime)
        {
        }

        //Called when atempting to go back
        private void HideMessage()
        {
            //Makes the messagebox invisible when pressing the actionkey if it's displaying a message


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
            if (messageState == MessageState.Message
                || messageState == MessageState.MessageWithImage
                || messageState == MessageState.Image)
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