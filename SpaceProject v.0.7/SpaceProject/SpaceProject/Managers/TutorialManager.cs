﻿/* TutorialManager.cs
 * 
 * Handles displaying of tutorial messages
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum TutorialImage
    {
        OverworldControls,
        CombatControls,
        MenuControls,
        CombatBars,
        Coordinates
    }

    public class TutorialManager
    {
        private Game1 game;
        private Sprite tutorialSpriteSheet;
        private List<Sprite> tutorialImages;

        private bool tutorialsUsed;
        public bool TutorialsUsed { get { return tutorialsUsed; } set { tutorialsUsed = value; } }
        private int tempTimer = 50;
        private int tempTimer2 = 200;
        private int tempTimer3 = 200;

        // progress flags
        private bool hasEnteredSectorX;
        private bool hasEnteredStation;
        private bool hasEnteredPlanet;
        private bool hasEnteredOverworld;
        private bool hasEnteredVerticalShooter;
        private bool hasEnteredShop;
        private bool hasEnteredInventory;
        private bool hasEnteredHighfenceBeaconArea;
        private bool hasActivatedHighfenceBeacon;
        private bool coordinatesDisplayed;

        public TutorialManager(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            tutorialSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/tutorial_spritesheet"), null);

            tutorialImages = new List<Sprite>();
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(368, 1, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(0, 0, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(735, 1, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(1, 199, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(368, 199, 366, 197)));

            hasEnteredSectorX = false;
            hasEnteredStation = false;
            hasEnteredPlanet = false;
            hasEnteredOverworld = false;
            hasEnteredVerticalShooter = false;
            hasEnteredShop = false;
            hasEnteredInventory = false;
            hasEnteredHighfenceBeaconArea = false;
            hasActivatedHighfenceBeacon = false;
        }

        public void Update(GameTime gameTime)
        {
            UpdateTutorialMessages(gameTime);
        }

        private void UpdateTutorialMessages(GameTime gameTime)
        {
            if (!hasEnteredStation && GameStateManager.currentState.Equals("StationState") &&
                game.stateManager.stationState.SubStateManager.ButtonControl == ButtonControl.Menu)
            {
                hasEnteredStation = true;

                DisplayTutorialImage(TutorialImage.MenuControls);
            }

            if (!hasEnteredOverworld && GameStateManager.currentState.Equals("OverworldState"))
            {
                tempTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer < 0)
                {
                    tempTimer = 50;
                    hasEnteredOverworld = true;

                    DisplayTutorialImage(TutorialImage.OverworldControls);

                    //"At the bottom-left of the screen is your overall health. This determines how much health you have when entering combat. When your overall health is reduced to 0, the game is over." - After first level
                }
            }

            else if (!coordinatesDisplayed 
                && hasEnteredOverworld 
                && game.messageBox.MessageState == MessageState.Invisible)
            {
                coordinatesDisplayed = true;

                DisplayTutorialMessage(new List<String>{"Your current objective is to go to coordinates (2635, 940). To find out where that is, look at the coordinates at the bottom right of the screen, just above the minimap. The sun, in the middle of the sector, is the center point (origin) of the coordinate system.",
                "If you forget where you need to go you can at any time check your current mission objectives in the mission log. Press 'M' to bring up the mission screen. From there, you can select your current missions and view their objectives."}, TutorialImage.Coordinates);
            }

            if (!hasEnteredPlanet && GameStateManager.currentState.Equals("PlanetState") &&
                game.stateManager.planetState.SubStateManager.ActiveMenuState ==
                game.stateManager.planetState.SubStateManager.OverviewMenuState &&
                game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                hasEnteredPlanet = true;

                //DisplayTutorialMessage("This is the planet menu. If the planet has a colony, you can buy/sell items there, accept missions and listen to rumors from it's inhabitants. Not all planets are inhabited though.");
            }

            if (!hasEnteredSectorX &&
                    MissionManager.GetMission("Main - New First Mission").MissionState == StateOfMission.CompletedDead)
            {
                if (CollisionDetection.IsRectInRect(game.player.Bounds,
                    game.stateManager.overworldState.GetSectorX.SpaceRegionArea) &&
                    game.messageBox.MessageState == MessageState.Invisible)
                {
                    hasEnteredSectorX = true;

                    DisplayTutorialMessage(new List<string> {"Welcome to the Sector X. You are now free to explore the sector's planets and stations. Some stations or colonies on planets have missions you can take on to earn money and learn more about the people living here.",
                        "You will also encounter ships traveling between destinations. Some are friendly and some are not so friendly. You can use the minimap to determine the allegiance of the ships: blue is neutral and red is hostile. Be careful!"});
                }
            }

            Vector2 highfenceBeaconPosition = game.stateManager.overworldState.GetBeacon("Highfence Beacon").position; 

            if (!hasEnteredHighfenceBeaconArea &&
                CollisionDetection.IsRectInRect(game.player.Bounds, new Rectangle((int)highfenceBeaconPosition.X - 200, (int)highfenceBeaconPosition.Y - 200, 400, 400)))
            {
                hasEnteredHighfenceBeaconArea = true;
                DisplayTutorialMessage("This is a 'beacon'. Beacons are used for traveling quickly between stations and planets, but they need to be activated before use. Try activating it now."); 
            }

            if (!hasActivatedHighfenceBeacon &&
                game.stateManager.overworldState.GetBeacon("Highfence Beacon").IsActivated)
            {
                hasActivatedHighfenceBeacon = true;
                DisplayTutorialMessage("Good! This beacon is now activated and you can press 'Enter' above it to open the beacon menu. However, you won't be able to use it until you activate more beacons. All planets in the sector have a beacon orbiting it. Don't forget to activate them when you see them!");
            }

            if (!hasEnteredVerticalShooter && GameStateManager.currentState.Equals("ShooterState"))
            {
                tempTimer2 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer2 < 0)
                {
                    tempTimer2 = 500;

                    hasEnteredVerticalShooter = true;
                    DisplayTutorialMessage(new List<String>{"You can rebind the keys in the options menu.",
                    "Down at the bottom-left is some information and three bars:\n\nYour objective - Displays condition to complete level.", "Your currently active primary weapon - Toggle between your equipped primary weapons with 'Shift'.\n\nYour health - when this runs out you fail the level and your overall health is reduced a bit.", "Your energy - weapons use energy to fire.\n\nYour shield - protects your ship from damage. Recharges over time."},
                    new List<TutorialImage> { TutorialImage.CombatControls, TutorialImage.CombatBars},
                        new List<int> {1});
                }
            }

            if (!hasEnteredShop && ((GameStateManager.currentState.Equals("PlanetState") &&
                game.stateManager.planetState.SubStateManager.ActiveMenuState.Equals(
                game.stateManager.planetState.SubStateManager.ShopMenuState)) ||
                (GameStateManager.currentState.Equals("StationState") &&
                game.stateManager.stationState.SubStateManager.ActiveMenuState.Equals(
                game.stateManager.stationState.SubStateManager.ShopMenuState))))
            {
                hasEnteredShop = true;

                DisplayTutorialMessage(new List<String> { "This is the shop, here you can buy new equipment for your ship as well as sell your old equipment. The two left columns displays your current inventory and the right column displays the shop's inventory.", 
                    "Move the cursor with the arrowkeys. Press 'Enter' to get options for buying and selling items. When you are done shopping, press 'Escape' to leave the shop."});
            }

            if (!hasEnteredInventory && GameStateManager.currentState.Equals("ShipManagerState"))
            {
                tempTimer3 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer3 < 0)
                {
                    tempTimer3 = 200;
                    hasEnteredInventory = true;

                    DisplayTutorialMessage(new List<String> { "This is your inventory. Here, you can check and equip your different ship parts. You can also see your complete inventory. In order to equip a part, enter that menu using 'Enter'. Then choose the position you want to equip to. Finally choose what part you want to the chosen position.",
                    "What parts you have equipped is crucial for your success. Come back here often and try different combinations of ship parts. Now equip what you bought and leave the inventory by pressing 'Escape'." });
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        private void DisplayTutorialMessage(String message)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessage(message, true);
            }
        }

        private void DisplayTutorialMessage(List<String> messages)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessage(messages, true);
            }
        }

        private void DisplayTutorialMessage(String message, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessageWithImage(message, GetImageFromEnum(imageID), true);
            }
        }

        private void DisplayTutorialMessage(List<String> messages, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessageWithImage(messages, GetImageFromEnum(imageID), true);
            }
        }

        private void DisplayTutorialMessage(List<String> messages, List<TutorialImage> imageID, List<int>imageTriggers)
        {
            if (tutorialsUsed)
            {
                List<Sprite> sprites = new List<Sprite>();

                foreach (TutorialImage imgID in imageID)
                {
                    sprites.Add(GetImageFromEnum(imgID));
                }

                game.messageBox.DisplayMessageWithImage(messages, sprites,  true, imageTriggers);
            }
        }

        private void DisplayTutorialImage(TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayImage(GetImageFromEnum(imageID), true);
            }
        }

        private void DisplayTutorialImage(List<TutorialImage> imageID)
        {
            if (tutorialsUsed)
            {
                List<Sprite> sprites = new List<Sprite>();

                foreach (TutorialImage imgID in imageID)
                {
                    sprites.Add(GetImageFromEnum(imgID));
                }

                game.messageBox.DisplayImages(sprites, true);
            }
        }

        public void Save()
        {
            SortedDictionary<string, string> tutorialProgress = new SortedDictionary<string, string>();

            tutorialProgress.Add("hasEnteredSectorX", hasEnteredSectorX.ToString());
            tutorialProgress.Add("hasEnteredStation", hasEnteredStation.ToString());
            tutorialProgress.Add("hasEnteredPlanet", hasEnteredPlanet.ToString());
            tutorialProgress.Add("hasEnteredOverworld", hasEnteredOverworld.ToString());
            tutorialProgress.Add("hasEnteredVerticalShooter", hasEnteredVerticalShooter.ToString());
            tutorialProgress.Add("hasEnteredShop", hasEnteredShop.ToString());
            tutorialProgress.Add("hasEnteredInventory", hasEnteredInventory.ToString());
            tutorialProgress.Add("hasEnteredHighfenceBeaconArea", hasEnteredHighfenceBeaconArea.ToString());
            tutorialProgress.Add("hasActivatedHighfenceBeacon", hasActivatedHighfenceBeacon.ToString());
            tutorialProgress.Add("hasStartedSecondMission", coordinatesDisplayed.ToString());

            game.saveFile.Save("save.ini", "tutorialprogress", tutorialProgress);
        }

        public void Load()
        {
            hasEnteredSectorX = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredsectorx", false);
            hasEnteredStation = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredstation", false);
            hasEnteredPlanet = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredplanet", false);
            hasEnteredOverworld = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredoverworld", false);
            hasEnteredVerticalShooter = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredverticalshooter", false);
            hasEnteredShop = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredshop", false);
            hasEnteredInventory = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredinventory", false);
            hasEnteredHighfenceBeaconArea = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredhighfencebeaconarea", false);
            hasActivatedHighfenceBeacon = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasactivatedhighfencebeacon", false);
            coordinatesDisplayed = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasstartedsecondmission", false);
        }

        public Sprite GetImageFromEnum(TutorialImage imageID)
        {
            switch (imageID)
            {
                case TutorialImage.OverworldControls:
                    return tutorialImages[0];

                case TutorialImage.CombatControls:
                    return tutorialImages[1];

                case TutorialImage.MenuControls:
                    return tutorialImages[2];

                case TutorialImage.CombatBars:
                    return tutorialImages[3];

                case TutorialImage.Coordinates:
                    return tutorialImages[4];

                default:
                    throw new ArgumentException("Image ID not recognized.");
            }
        }
    }
}
