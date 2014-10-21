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
        CombatControls,
        CombatBars
    }

    public class TutorialManager
    {
        private Game1 game;

        private Sprite tutorialImageCanvas;
        private Sprite tutorialSpriteSheet;
        private List<Sprite> tutorialImages;

        private bool tutorialsUsed;
        public bool TutorialsUsed { get { return tutorialsUsed; } set { tutorialsUsed = value; } }
        private int tempTimer = 1250;
        private int tempTimer2 = 200;
        private int tempTimer3 = 200;

        // progress variables
        private bool hasEnteredSectorX;
        private bool hasEnteredStation;
        private bool hasEnteredPlanet;
        private bool hasEnteredOverworld;
        private bool hasEnteredVerticalShooter;
        private bool hasEnteredShop;
        private bool hasEnteredInventory;
        private bool hasEnteredHighfenceBeaconArea;
        private bool hasActivatedHighfenceBeacon;

        public TutorialManager(Game1 game)
        {
            this.game = game;
        }

        public void Initialize()
        {
            tutorialSpriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/tutorial_spritesheet"), null);
            tutorialImageCanvas = tutorialSpriteSheet.GetSubSprite(new Rectangle(0, 0, 400, 400));

            tutorialImages = new List<Sprite>();
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(403, 1, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(403, 199, 366, 197)));

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

        public void UpdateTutorialMessages(GameTime gameTime)
        {
            if (!hasEnteredStation && GameStateManager.currentState.Equals("StationState") &&
                game.stateManager.stationState.SubStateManager.ButtonControl == ButtonControl.Menu)
            {
                hasEnteredStation = true;

                DisplayTutorialMessage("This is the station menu, here you can select missions, listen to rumors and buy/sell items and fuel. Move the cursor with the arrow-keys and press 'Enter' to select.");
            }

            if (!hasEnteredOverworld && GameStateManager.currentState.Equals("OverworldState"))
            {
                tempTimer -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer < 0)
                {
                    tempTimer = 1250;
                    hasEnteredOverworld = true;

                    DisplayTutorialMessage(new List<string> {"Welcome to the overworld! To move your ship you use left and right arrow-keys to rotate and accelerate with the up-key. To enter stations or planets you position your ship above them and press 'Enter'. Press 'Escape' to bring up the menu.",
                    "Your current objective is to go to coordinates (2635, 940). To find out where that is, look at the coordinates at the bottom right of the screen, just above the minimap. The sun, in the middle of the sector, is the center point (origin) of the coordinate system.", 
                    "If you forget where you need to go you can at any time check your current mission objectives in the mission log. Press 'M' to bring up the mission screen. From there, you can select your current missions and view their objectives.",
                    "Next to the fuel bar at the bottom-right of the screen is your overall health. This determines how much health you have when entering combat. When your overall health is reduced to 0, the game is over."});
                }
            }

            if (!hasEnteredPlanet && GameStateManager.currentState.Equals("PlanetState") &&
                game.stateManager.planetState.SubStateManager.ActiveMenuState ==
                game.stateManager.planetState.SubStateManager.OverviewMenuState &&
                game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            {
                hasEnteredPlanet = true;

                DisplayTutorialMessage("This is the planet menu. Compared to the station menu, you have a few different options. You can view information about the planet and mine for resources if the planet has any. Not all planets are inhabited though.");
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
                DisplayTutorialMessage("This is a 'beacon'. Beacons are used for traveling quickly between stations, but they need to be activated before use. Try activating it now."); 
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
                    DisplayTutorialMessage(new List<String>{"Use the movement keys (Default arrowkeys) to move your ship, the fire key ('Left Control') to fire and the switch key ('Left Shift') to change weapon. You can rebind the keys in the options menu.",
                    "Down at the bottom-left are three bars:\n\nYour health - when this runs out you fail the level and your overall health is reduced a bit.", "Your energy - weapons use energy to fire.\n\nYour shield - protects your ship from damage. Recharges over time."},
                    new List<TutorialImage> { TutorialImage.CombatControls, TutorialImage.CombatBars }, new List<int> {1});
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

        public void DisplayTutorialMessage(String message)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessage(message, true);
            }
        }

        public void DisplayTutorialMessage(List<String> messages)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessage(messages, true);
            }
        }

        public void DisplayTutorialMessage(String message, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessageWithImage(message, tutorialImageCanvas, GetImageFromEnum(imageID), true);
            }
        }

        public void DisplayTutorialMessage(List<String> messages, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                game.messageBox.DisplayMessageWithImage(messages, tutorialImageCanvas, GetImageFromEnum(imageID), true);
            }
        }

        public void DisplayTutorialMessage(List<String> messages, List<TutorialImage> imageID, List<int>imageTriggers)
        {
            if (tutorialsUsed)
            {
                List<Sprite> sprites = new List<Sprite>();

                foreach (TutorialImage imgID in imageID)
                {
                    sprites.Add(GetImageFromEnum(imgID));
                }

                game.messageBox.DisplayMessageWithImage(messages, tutorialImageCanvas, sprites,  true, imageTriggers);
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
        }

        private Sprite GetImageFromEnum(TutorialImage imageID)
        {
            switch (imageID)
            {
                case TutorialImage.CombatControls:
                    return tutorialImages[0];

                case TutorialImage.CombatBars:
                    return tutorialImages[1];

                default:
                    throw new ArgumentException("Image ID not recognized.");
            }
        }
    }
}
