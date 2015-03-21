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
    public class PopupHandler
    {
        #region variables
        private const int PopupDelay = 20;

        public static bool DisplayMenuOnReturn;

        private static Game1 game;
        private static Sprite spriteSheet;

        private static List<Popup> messageQueue;
        private static List<Popup> menuQueue;
        private static List<Popup> realTimeMessageQueue;

        private static int popupDelayTimer;

        #endregion

        #region Properties

        public static bool TextBufferEmpty
        { 
            get 
            { 
                return messageQueue.Count <= 0 
                    && menuQueue.Count <= 0
                    && realTimeMessageQueue.Count <= 0;
            } 
        }

        public static int MessageQueueCount { get { return messageQueue.Count; } }

        #endregion

        public PopupHandler(Game1 game, Sprite spriteSheet)
        {
            PopupHandler.game = game;
            PopupHandler.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            messageQueue = new List<Popup>();
            menuQueue = new List<Popup>();
            realTimeMessageQueue = new List<Popup>();
            PopupHandler.DisplayMenuOnReturn = false;
        }

        public void Update(GameTime gameTime)
        {
            popupDelayTimer -= gameTime.ElapsedGameTime.Milliseconds;

            //displays the overmenu automaticly when returning from the inventory or mission screen 
            if (PopupHandler.DisplayMenuOnReturn)
            {
                if (GameStateManager.currentState.Equals("OverworldState"))
                {
                    DisplayMenu();
                    PopupHandler.DisplayMenuOnReturn = false;
                }
            }

            if (popupDelayTimer <= 0)
            {
                UpdateQueue(gameTime, messageQueue);
            }
            UpdateQueue(gameTime, menuQueue);
            UpdateQueue(gameTime, realTimeMessageQueue);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (realTimeMessageQueue.Count > 0
                && GameStateManager.currentState.Equals("OverworldState")
                && realTimeMessageQueue[0].PopupState == PopupState.Showing)
            {
                if (menuQueue.Count <= 0 
                    || !(menuQueue[0] is SelectionMenu)
                    || menuQueue[0].PopupState != PopupState.Showing)
                {
                    realTimeMessageQueue[0].Draw(spriteBatch);
                }
            }

            DrawQueue(spriteBatch, messageQueue);
            DrawQueue(spriteBatch, menuQueue);
        }

        public static void DisplayMessage(params string[] messages)
        {
            TextMessage textMessage = new TextMessage(game, spriteSheet);

            textMessage.Initialize();
            textMessage.SetMessage(messages);

            popupDelayTimer = PopupDelay;
            messageQueue.Add(textMessage);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplayPortraitMessage(PortraitID portrait, params string[] messages)
        {
            PortraitMessage portraitMessage = new PortraitMessage(game, spriteSheet);
            portraitMessage.Initialize();
            portraitMessage.SetMessage(messages);
            portraitMessage.SetPortrait(new Portrait(portrait).Sprite);

            popupDelayTimer = PopupDelay;
            messageQueue.Add(portraitMessage);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplayPortraitMessage(List<PortraitID> portraits, List<int> portraitTriggers, params string[] messages)
        {
            List<Sprite> portraitList = new List<Sprite>();

            foreach (PortraitID id in portraits)
            {
                portraitList.Add(new Portrait(id).Sprite);
            }

            PortraitMessage portraitMessage = new PortraitMessage(game, spriteSheet);
            portraitMessage.Initialize();
            portraitMessage.SetMessage(messages);
            portraitMessage.SetPortraits(portraitList, portraitTriggers, TextUtils.GetSplitCount(messages));

            popupDelayTimer = PopupDelay;
            messageQueue.Add(portraitMessage);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplayImage(params Sprite[] images)
        {
            ImagePopup imagePopup = new ImagePopup(game, spriteSheet);

            imagePopup.Initialize();
            imagePopup.SetImages(images);

            popupDelayTimer = PopupDelay;
            messageQueue.Add(imagePopup);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplayMessageWithImage(List<Sprite> images, List<int> imageTriggers, params string[] messages)
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
            imageMessage.SetImages(TextUtils.GetSplitCount(messages), imageTriggers.ToArray<int>(), images.ToArray<Sprite>());

            popupDelayTimer = PopupDelay;
            messageQueue.Add(imageMessage);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplayRealtimePortraitMessage(float delay, PortraitID[] portraits,
            List<int> portraitTriggers, params string[] messages)
        {
            if (messages.Length < portraits.Length)
            {
                throw new ArgumentException("At least one string per image is required.");
            }

            if (portraits.Length - 1 != portraitTriggers.Count)
            {
                throw new ArgumentException("One trigger is required for each image except the first.");
            }

            List<Sprite> portraitList = new List<Sprite>();

            foreach (PortraitID id in portraits)
            {
                portraitList.Add(new Portrait(id).Sprite);
            }

            RealTimePortraitMessage realTimeMessage = new RealTimePortraitMessage(game, spriteSheet);
            realTimeMessage.Initialize();
            realTimeMessage.SetMessage(messages);
            realTimeMessage.SetDelay(delay);

            realTimeMessage.SetPortrait(portraitList, TextUtils.GetSplitCount(messages), portraitTriggers);

            popupDelayTimer = PopupDelay;
            realTimeMessageQueue.Add(realTimeMessage);
        }

        public static void DisplayRealtimeMessage(float delay, params string[] messages)
        {
            RealTimeMessage realTimeMessage = new RealTimeMessage(game, spriteSheet);
            realTimeMessage.Initialize();
            realTimeMessage.SetMessage(messages);
            realTimeMessage.SetDelay(delay);

            popupDelayTimer = PopupDelay;
            realTimeMessageQueue.Add(realTimeMessage);
        }

        public static void DisplayMenu()
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

            popupDelayTimer = PopupDelay;
            menuQueue.Add(menu);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        public static void DisplaySelectionMenu(string message, List<String> options, List<System.Action> actions)
        {
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

            popupDelayTimer = PopupDelay;
            menuQueue.Add(selectionMenu);

            game.soundEffectsManager.StopSoundEffect(SoundEffects.OverworldEngine);
        }

        private static void UpdateQueue(GameTime gameTime, List<Popup> queue)
        {
            if (queue.Count > 0)
            {
                if (queue[0].PopupState == PopupState.Hidden)
                {
                    queue[0].Show();
                }

                else if (queue[0].PopupState == PopupState.Showing)
                {
                    if ((queue[0] is RealTimeMessage
                        && GameStateManager.currentState.Equals("OverworldState"))
                        || !(queue[0] is RealTimeMessage))
                    {
                        queue[0].Update(gameTime);
                        game.helper.Visible = false;
                    }
                }

                else if (queue[0].PopupState == PopupState.Finished)
                {
                    queue.RemoveAt(0);
                }
            }
        }

        private static void DrawQueue(SpriteBatch spriteBatch, List<Popup> queue)
        {
            if (queue.Count > 0
                && queue[0].PopupState == PopupState.Showing)
            {
                queue[0].Draw(spriteBatch);
            }
        }
    }
}