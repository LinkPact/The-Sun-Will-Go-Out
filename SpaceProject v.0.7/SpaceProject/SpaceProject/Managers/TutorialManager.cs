/* TutorialManager.cs
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
        Coordinates,
        Stations,
        Planets,
        Beacons,
        Radar
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

        // progress flags
        private bool hasEnteredStation;
        private bool hasEnteredOverworld;
        private bool hasEnteredVerticalShooter;
        private bool hasEnteredShop;
        private bool hasEnteredInventory;
        private bool hasEnteredHighfenceBeaconArea;
        private bool hasActivatedHighfenceBeacon;
        private bool coordinatesDisplayed;

        private bool equipShieldTutorial;
        private int equipShieldProgress;
        private bool equipShieldTutorialFinished;

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
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(735, 199, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(1, 397, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(368, 397, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(735, 397, 366, 197)));

            hasEnteredStation = false;
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

                DisplayTutorialMessage("You can disable tutorial messages in the options menu.", TutorialImage.MenuControls);
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
                && PopupHandler.TextBufferEmpty)
            {
                coordinatesDisplayed = true;

                DisplayTutorialMessage("Your current objective is to go to coordinates (2635, 940). To find that location, just follow the blinking dot on your radar. Main missions are represented by gold-colored dots and secondary missions by silver-colored dots.",
                    TutorialImage.Radar);
            }

            //if (!hasEnteredPlanet && GameStateManager.currentState.Equals("PlanetState") &&
            //    game.stateManager.planetState.SubStateManager.ActiveMenuState ==
            //    game.stateManager.planetState.SubStateManager.OverviewMenuState &&
            //    game.stateManager.planetState.SubStateManager.ButtonControl != ButtonControl.Confirm)
            //{
            //    hasEnteredPlanet = true;
            //
            //    DisplayTutorialMessage("This is the planet menu. If the planet has a colony, you can buy/sell items there, accept missions and listen to rumors from it's inhabitants. Not all planets are inhabited though.");
            //}

            Vector2 highfenceBeaconPosition = game.stateManager.overworldState.GetBeacon("Highfence Beacon").position; 

            if (!hasEnteredHighfenceBeaconArea &&
                CollisionDetection.IsRectInRect(game.player.Bounds, new Rectangle((int)highfenceBeaconPosition.X - 200, (int)highfenceBeaconPosition.Y - 200, 400, 400)))
            {
                hasEnteredHighfenceBeaconArea = true;
                DisplayTutorialMessage("This is a 'beacon'. Beacons are used for traveling quickly between stations and planets, but they need to be activated before use. Try activating it now."); 
            }

            if (hasEnteredHighfenceBeaconArea
                && !hasActivatedHighfenceBeacon
                && game.stateManager.overworldState.GetBeacon("Highfence Beacon").IsActivated)
            {
                hasActivatedHighfenceBeacon = true;

                if (MissionManager.GetMission(MissionID.Main2_1_TheConvoy).MissionState != StateOfMission.Active
                    || !((EscortObjective)MissionManager.GetMission(MissionID.Main2_1_TheConvoy).CurrentObjective).Started)
                {
                    DisplayTutorialMessage("Good! This beacon is now activated and you can press 'Enter' above it to access it. All planets in the sector have a beacon orbiting it. Don't forget to activate them when you see them!");
                }
            }

            if (!hasEnteredVerticalShooter && GameStateManager.currentState.Equals("ShooterState"))
            {
                tempTimer2 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer2 < 0)
                {
                    tempTimer2 = 500;

                    hasEnteredVerticalShooter = true;
                    DisplayTutorialMessage(new List<String>{"You can rebind the keys in the options menu.",
                    "Down at the bottom-left is some information and three bars:\n\nObjective - Displays condition to complete the level.", "Primary - your selected primary weapon. Toggle between equipped weapons with '" + ControlManager.GetKeyName(RebindableKeys.Action2) + "'.\n\nSecondary - Your currently equipped secondary weapon. Turn it on/off with '" + ControlManager.GetKeyName(RebindableKeys.Action3) + "'", "Health - Your ship's armor, when it's reduced to zero, the current level is failed.\n\nEnergy - weapons use energy to fire.", "Shield - protects your ship from damage. Recharges over time."},
                    new List<TutorialImage> { TutorialImage.CombatControls, TutorialImage.CombatBars},
                        new List<int> {1});
                }
            }

            //if (!hasEnteredShop && ((GameStateManager.currentState.Equals("PlanetState") &&
            //    game.stateManager.planetState.SubStateManager.ActiveMenuState.Equals(
            //    game.stateManager.planetState.SubStateManager.ShopMenuState)) ||
            //    (GameStateManager.currentState.Equals("StationState") &&
            //    game.stateManager.stationState.SubStateManager.ActiveMenuState.Equals(
            //    game.stateManager.stationState.SubStateManager.ShopMenuState))))
            //{
            //    hasEnteredShop = true;
            //
            //    DisplayTutorialMessage(new List<String> { "This is the shop, here you can buy new equipment for your ship as well as sell your old equipment. The two left columns displays your current inventory and the right column displays the shop's inventory.", 
            //        "Move the cursor with the arrowkeys. Press 'Enter' to get options for buying and selling items. When you are done shopping, press 'Escape' to leave the shop."});
            //}

            //if (!hasEnteredInventory && GameStateManager.currentState.Equals("ShipManagerState"))
            //{
            //    hasEnteredInventory = true;
            //
            //    DisplayTutorialMessage(new List<String> { "This is your inventory. Here, you can check and equip your different ship parts. You can also see your complete inventory. In order to equip a part, enter that menu using 'Enter'. Then choose the position you want to equip to. Finally choose what part you want to the chosen position.",
            //    "What parts you have equipped is crucial for your success. Come back here often and try different combinations of ship parts. Now equip what you bought and leave the inventory by pressing 'Escape'." });
            //}

            if (equipShieldTutorial
                && !equipShieldTutorialFinished)
            {
                game.stateManager.planetState.SubStateManager.ShopMenuState.DisplayBuyAndEquip = false;
                switch (equipShieldProgress)
                {
                    case 0:
                        if (GameStateManager.currentState.Equals("PlanetState") &&
                            game.stateManager.planetState.Planet.name.Equals("Highfence"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Start by entering the shop and selecting 'Buy & Sell Items'.\"");
                            equipShieldProgress = 1;
                            StatsManager.Rupees += 200;
                        }
                        break;

                    case 1:
                        if (GameStateManager.currentState.Equals("PlanetState") &&
                            game.stateManager.planetState.Planet.name.Equals("Highfence")
                            && game.stateManager.planetState.SubStateManager.ActiveMenuState.Equals(game.stateManager.planetState.SubStateManager.ShopMenuState))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Select the 'Basic shield' in the column to the right and select 'Buy'.\"");
                            equipShieldProgress = 2;
                        }
                        break;

                    case 2:
                        if (GameStateManager.currentState.Equals("PlanetState") &&
                            game.stateManager.planetState.Planet.name.Equals("Highfence")
                            && game.stateManager.planetState.SubStateManager.ActiveMenuState.Equals(game.stateManager.planetState.SubStateManager.ShopMenuState)
                            && ShipInventoryManager.ownedShields.Count > 0)
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Good! Now exit the shop with 'Escape' and return to the overworld!\"");
                            equipShieldProgress = 3;
                        }
                        break;

                    case 3:
                        if (GameStateManager.currentState.Equals("OverworldState"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Now, press 'I' to access your inventory.\"");
                            equipShieldProgress = 4;
                        }
                        break;

                    case 4:
                        if (GameStateManager.currentState.Equals("ShipManagerState"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"To equip your shield, select the shield slot and press 'Enter' to select from your list of available shields.\"");
                            equipShieldProgress = 5;
                        }
                        break;

                    case 5:
                        if (GameStateManager.currentState.Equals("ShipManagerState")
                            && game.stateManager.shipManagerState.IsShieldSlotSelected)
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Now, press 'Enter' again to equip the selected shield.\"");
                            equipShieldProgress = 6;
                        }
                        break;

                    case 6:
                        if (!(ShipInventoryManager.equippedShield is EmptyShield))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "[Captain] \"Good! Now your shield is equipped! What parts you have equipped is crucial for your success. Come back here often and try different combinations of ship parts.\"#\"Now, exit the inventory by pressing 'Escape' and return to me!\"");
                            equipShieldTutorialFinished = true;
                        }
                        break;
                }

                if (equipShieldProgress < 2
                    && ShipInventoryManager.OwnedShields.Count > 0)
                {
                    PopupHandler.DisplayPortraitMessage(PortraitID.AllianceCaptain, "You already bought a shield? Okay, let me tell you how to equip it! Start by pressing 'I' to access your inventory.");
                    equipShieldProgress = 4;
                }
            }

            if (equipShieldTutorialFinished
                && game.stateManager.planetState.SubStateManager.ShopMenuState.DisplayBuyAndEquip == false)
            {
                game.stateManager.planetState.SubStateManager.ShopMenuState.DisplayBuyAndEquip = true;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {

        }

        private void DisplayTutorialMessage(String message)
        {
            if (tutorialsUsed)
            {
                PopupHandler.DisplayMessage(message);
            }
        }

        private void DisplayTutorialMessage(List<String> messages)
        {
            if (tutorialsUsed)
            {
                PopupHandler.DisplayMessage(messages.ToArray());
            }
        }

        private void DisplayTutorialMessage(String message, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                PopupHandler.DisplayMessageWithImage(new List<Sprite>(){GetImageFromEnum(imageID)}, new List<int>(){}, message);
            }
        }

        private void DisplayTutorialMessage(List<String> messages, TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                PopupHandler.DisplayMessageWithImage(new List<Sprite> { GetImageFromEnum(imageID) },
                    new List<int>(), messages.ToArray());
            }
        }

        private void DisplayTutorialMessage(List<String> messages, List<TutorialImage> imageID, List<int> imageTriggers)
        {
            if (tutorialsUsed)
            {
                List<Sprite> sprites = new List<Sprite>();

                foreach (TutorialImage imgID in imageID)
                {
                    sprites.Add(GetImageFromEnum(imgID));
                }

                PopupHandler.DisplayMessageWithImage(sprites, imageTriggers, messages.ToArray());
            }
        }

        private void DisplayTutorialImage(TutorialImage imageID)
        {
            if (tutorialsUsed)
            {
                PopupHandler.DisplayImage(GetImageFromEnum(imageID));
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

                PopupHandler.DisplayImage(sprites.ToArray());
            }
        }

        public void Save()
        {
            SortedDictionary<string, string> tutorialProgress = new SortedDictionary<string, string>();

            tutorialProgress.Add("hasEnteredStation", hasEnteredStation.ToString());
            tutorialProgress.Add("hasEnteredOverworld", hasEnteredOverworld.ToString());
            tutorialProgress.Add("hasEnteredVerticalShooter", hasEnteredVerticalShooter.ToString());
            tutorialProgress.Add("hasEnteredShop", hasEnteredShop.ToString());
            tutorialProgress.Add("hasEnteredInventory", hasEnteredInventory.ToString());
            tutorialProgress.Add("hasEnteredHighfenceBeaconArea", hasEnteredHighfenceBeaconArea.ToString());
            tutorialProgress.Add("hasActivatedHighfenceBeacon", hasActivatedHighfenceBeacon.ToString());
            tutorialProgress.Add("hasStartedSecondMission", coordinatesDisplayed.ToString());
            tutorialProgress.Add("equipShieldTutorial", equipShieldTutorialFinished.ToString());

            game.saveFile.Save("save.ini", "tutorialprogress", tutorialProgress);
        }

        public void Load()
        {
            hasEnteredStation = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredstation", false);
            hasEnteredOverworld = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredoverworld", false);
            hasEnteredVerticalShooter = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredverticalshooter", false);
            hasEnteredShop = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredshop", false);
            hasEnteredInventory = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredinventory", false);
            hasEnteredHighfenceBeaconArea = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredhighfencebeaconarea", false);
            hasActivatedHighfenceBeacon = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasactivatedhighfencebeacon", false);
            coordinatesDisplayed = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasstartedsecondmission", false);
            equipShieldTutorialFinished = game.saveFile.GetPropertyAsBool("tutorialprogress", "equipshieldtutorial", false);
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

                case TutorialImage.Stations:
                    return tutorialImages[5];

                case TutorialImage.Planets:
                    return tutorialImages[6];

                case TutorialImage.Beacons:
                    return tutorialImages[7];

                case TutorialImage.Radar:
                    return tutorialImages[8];

                default:
                    throw new ArgumentException("Image ID not recognized.");
            }
        }

        public void EnableEquipTutorial()
        {
            equipShieldTutorial = true;
        }
    }
}
