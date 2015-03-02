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
    public class MessageBox
    {
        #region variables

        private Game1 game;
        private Sprite spriteSheet;

        private List<Popup> popupQueue;
        private bool displayOnReturn = false;

        #endregion

        #region Properties

        public bool TextBufferEmpty { get { return popupQueue.Count <= 0; } }

        #endregion

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
            textMessage.Show();

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
            if (popupQueue.Count > 0
                && popupQueue[0].Finished)
            {
                popupQueue.RemoveAt(0);
            }

            if (popupQueue.Count > 0)
            {
                popupQueue[0].Update(gameTime);
            }

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

        public void Draw(SpriteBatch spriteBatch)
        {
            if (popupQueue.Count > 0)
            {
                popupQueue[0].Draw(spriteBatch);
            }
        }
    }
}