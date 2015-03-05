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
    public enum PortraitID
    {
        // Central characters
        Sair,
        Ai,
        Rok,
        Ente,
        Berr,
        RebelLeader,
        AllianceCommander,

        // Generic characters
        AllianceCaptain,
        RebelTroopLeader,
        AlliancePilot,
        RebelPilot,
        CommonCitizen
    }

    public class PopupHandler
    {
        #region variables

        private static Game1 game;
        private static Sprite spriteSheet;
        private static Sprite portraitSpriteSheet;

        private static List<Popup> popupQueue;
        private bool displayOnReturn = false;

        #endregion

        #region Properties

        public static bool TextBufferEmpty { get { return popupQueue.Count <= 0; } }

        #endregion

        public PopupHandler(Game1 game, Sprite spriteSheet)
        {
            PopupHandler.game = game;
            PopupHandler.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            portraitSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites\\PortraitSpriteSheet"), null);
            popupQueue = new List<Popup>();

            displayOnReturn = false;
        }

        public static void DisplayRealtimeMessage(float delay, params string[] messages)
        {
            RealTimeMessage realTimeMessage = new RealTimeMessage(game, spriteSheet);
            
            realTimeMessage.Initialize();
            realTimeMessage.SetMessage(messages);
            realTimeMessage.SetDelay(delay);
            
            popupQueue.Add(realTimeMessage);
        }

        public static void DisplayMessage(params string[] messages)
        {
            TextMessage textMessage = new TextMessage(game, spriteSheet);

            textMessage.Initialize();
            textMessage.SetMessage(messages);

            popupQueue.Add(textMessage);
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
            imageMessage.SetImages(images, messages.Length, imageTriggers);

            popupQueue.Add(imageMessage);
        }

        public static void DisplayPortraitMessage(PortraitID portrait, params string[] messages)
        {
            PortraitMessage portraitMessage = new PortraitMessage(game, spriteSheet);
            portraitMessage.Initialize();
            portraitMessage.SetMessage(messages);
            portraitMessage.SetPortrait(PopupHandler.GetPortrait(portrait));

            popupQueue.Add(portraitMessage);
        }

        public static void DisplayImage(params Sprite[] images)
        {
            ImagePopup imagePopup = new ImagePopup(game, spriteSheet);

            imagePopup.Initialize();
            imagePopup.SetImages(images);

            popupQueue.Add(imagePopup);
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

            popupQueue.Add(menu);
        }

        public static void DisplaySelectionMenu(string message, List<String> options, List<System.Action> actions)
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
            if (popupQueue.Count > 0)
            {
                if (popupQueue[0].PopupState == PopupState.Hidden)
                {
                    popupQueue[0].Show();
                }

                else if (popupQueue[0].PopupState == PopupState.Showing)
                {
                    popupQueue[0].Update(gameTime);
                }

                else
                {
                    popupQueue.RemoveAt(0);
                }
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
            if (popupQueue.Count > 0
                && popupQueue[0].PopupState == PopupState.Showing)
            {
                popupQueue[0].Draw(spriteBatch);
            }
        }

        private static Sprite GetPortrait(PortraitID portrait)
        {
            Rectangle sourceRect;

            switch (portrait)
            {
                case PortraitID.Sair:
                    sourceRect = new Rectangle(0, 0, 149, 192);
                    break;

                case PortraitID.Ai:
                    sourceRect = new Rectangle(154, 0, 149, 192);
                    break;

                case PortraitID.Rok:
                    sourceRect = new Rectangle(308, 0, 149, 192);
                    break;

                case PortraitID.Ente:
                    sourceRect = new Rectangle(462, 0, 149, 192);
                    break;

                case PortraitID.Berr:
                    sourceRect = new Rectangle(0, 197, 149, 192);
                    break;

                case PortraitID.RebelLeader:
                    sourceRect = new Rectangle(154, 197, 149, 192);
                    break;

                case PortraitID.AllianceCommander:
                    sourceRect = new Rectangle(308, 197, 149, 192);
                    break;

                case PortraitID.AllianceCaptain:
                    sourceRect = new Rectangle(462, 197, 149, 192);
                    break;

                case PortraitID.RebelTroopLeader:
                    sourceRect = new Rectangle(0, 394, 149, 192);
                    break;

                case PortraitID.AlliancePilot:
                    sourceRect = new Rectangle(154, 394, 149, 192);
                    break;

                case PortraitID.RebelPilot:
                    sourceRect = new Rectangle(308, 394, 149, 192);
                    break;

                case PortraitID.CommonCitizen:
                    sourceRect = new Rectangle(462, 394, 149, 192);
                    break;

                default:
                    throw new ArgumentException("Invalid Portrait ID.");
            }

            return new Sprite(portraitSpriteSheet.Texture, sourceRect);
        }
    }
}