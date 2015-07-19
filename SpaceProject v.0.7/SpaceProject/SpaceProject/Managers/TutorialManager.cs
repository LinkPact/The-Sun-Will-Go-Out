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
        StationsPlanets,
        Beacons,
        Radar,
        ShopStations
    }

    public class TutorialManager
    {
        private readonly float BeaconTutorialActivationRadius = 200;

        private Game1 game;
        private Sprite tutorialSpriteSheet;
        private List<Sprite> tutorialImages;

        private bool tutorialsUsed;
        public bool TutorialsUsed { get { return tutorialsUsed; } set { tutorialsUsed = value; } }
        private int tempTimer = 50;
        private int tempTimer2 = 200;
        private int tempTimer3 = 1000;

        // progress flags
        private bool hasEnteredStation;
        private bool hasEnteredOverworld;
        private bool hasEnteredVerticalShooter;
        private bool hasEnteredShop;
        private bool hasEnteredInventory;
        private bool hasEnteredHighfenceBeaconArea;
        private bool hasEnteredShooterWithShield;
        private bool longShotTutorialActivated;
        private bool secondaryWeaponTutorialDisplayed;
        private bool coordinatesDisplayed;

        private bool hasDisplayedSkipMessage;
        private bool isAMissionFailed;
        public bool IsAMissionFailed { get { return isAMissionFailed; } set { isAMissionFailed = value; } }

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
            //tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(1, 397, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(368, 397, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(735, 397, 366, 197)));
            tutorialImages.Add(tutorialSpriteSheet.GetSubSprite(new Rectangle(1, 595, 366, 197)));

            hasEnteredStation = false;
            hasEnteredOverworld = false;
            hasEnteredVerticalShooter = false;
            hasEnteredShop = false;
            hasEnteredInventory = false;
            hasEnteredHighfenceBeaconArea = false;
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
                }
            }

            else if (!coordinatesDisplayed 
                && hasEnteredOverworld 
                && PopupHandler.TextBufferEmpty)
            {
                coordinatesDisplayed = true;

                DisplayTutorialMessage("Your current objective is to go to coordinates (2450, 700). To find that location, follow the blinking dot on your radar. Main missions are represented by gold-colored dots and secondary missions by silver-colored dots.",
                    TutorialImage.Radar);
            }

            Vector2 highfenceBeaconPosition = game.stateManager.overworldState.GetBeacon("Highfence Beacon").position; 

            if (!hasEnteredHighfenceBeaconArea &&
                Vector2.Distance(game.player.position, highfenceBeaconPosition) < BeaconTutorialActivationRadius)
            {
                hasEnteredHighfenceBeaconArea = true;
                DisplayTutorialMessage("This is a 'beacon'. Beacons are used for traveling quickly between planets, but they need to be activated before use. Activate a beacon by flying close to it!"); 
            }

            if (!hasEnteredVerticalShooter && GameStateManager.currentState.Equals("ShooterState"))
            {
                tempTimer2 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer2 < 0)
                {
                    tempTimer2 = 500;

                    hasEnteredVerticalShooter = true;
                    DisplayTutorialMessage(new List<String>{"You can rebind the keys in the options menu.",
                    "Down at the bottom-left you can find information about the current level objective, your active weapons and on your ship's health, energy and shields."},
                    new List<TutorialImage> { TutorialImage.CombatControls, TutorialImage.CombatBars},
                        new List<int> {1});
                }
            }

            if (!hasEnteredShooterWithShield && !(ShipInventoryManager.equippedShield is EmptyShield)
                && GameStateManager.currentState.Equals("ShooterState"))
            {
                tempTimer2 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer2 < 0)
                {
                    tempTimer2 = 500;

                    hasEnteredShooterWithShield = true;
                    DisplayTutorialMessage("You now have a shield to protect your ship's hull! If you take a hit, the shield will absorb the damage if it has enough power.");
                }
            }

            if (equipShieldTutorial
                && !equipShieldTutorialFinished)
            {
                game.stateManager.stationState.SubStateManager.ShopMenuState.DisplayBuyAndEquip = false;
                switch (equipShieldProgress)
                {
                    case 0:
                        if (GameStateManager.currentState.Equals("StationState") &&
                            game.stateManager.stationState.Station.name.Equals("Highfence Shop"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Start by entering the shop and selecting 'Buy & Sell Items'. Here is two hundred Crebits, it should cover the cost for the shield.\"");
                            PopupHandler.DisplayMessage("You recieved 200 Crebits.");
                            StatsManager.Crebits += 200;
                            equipShieldProgress = 1;
                        }
                        break;

                    case 1:
                        if (GameStateManager.currentState.Equals("StationState") &&
                            game.stateManager.stationState.Station.name.Equals("Highfence Shop")
                            && game.stateManager.stationState.SubStateManager.ActiveMenuState.Equals(game.stateManager.stationState.SubStateManager.ShopMenuState))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Select the 'Basic shield' in the column to the right and select 'Buy'.\"");
                            equipShieldProgress = 2;
                        }
                        break;

                    case 2:
                        if (GameStateManager.currentState.Equals("StationState") &&
                            game.stateManager.stationState.Station.name.Equals("Highfence Shop")
                            && game.stateManager.stationState.SubStateManager.ActiveMenuState.Equals(game.stateManager.stationState.SubStateManager.ShopMenuState)
                            && ShipInventoryManager.ownedShields.Count > 0)
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Good! Now exit the shop by selecting and pressing 'Go Back' and return to the overworld!\"");
                            equipShieldProgress = 3;
                        }
                        break;

                    case 3:
                        if (GameStateManager.currentState.Equals("OverworldState"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Now, press 'I' to access your inventory.\"");
                            equipShieldProgress = 4;
                        }
                        break;

                    case 4:
                        if (GameStateManager.currentState.Equals("ShipManagerState"))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"To equip your shield, select the shield slot and press 'Enter' to select from your list of available shields.\"");
                            equipShieldProgress = 5;
                        }
                        break;

                    case 5:
                        if (GameStateManager.currentState.Equals("ShipManagerState")
                            && game.stateManager.shipManagerState.IsShieldSlotSelected)
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Now, press 'Enter' again to equip the selected shield.\"");
                            equipShieldProgress = 6;
                        }
                        break;

                    case 6:
                        if (!(ShipInventoryManager.equippedShield is EmptyShield))
                        {
                            PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "[Alliance Pilot] \"Good! Now your shield is equipped! What parts you have equipped is crucial for your success. Come back here often and try different combinations of ship parts.\"#\"Now, exit the inventory by pressing 'Escape' and return to me!\"");
                            equipShieldTutorialFinished = true;
                        }
                        break;
                }

                if (equipShieldProgress < 2
                    && ShipInventoryManager.OwnedShields.Count > 0)
                {
                    PopupHandler.DisplayPortraitMessage(PortraitID.AlliancePilot, "You already bought a shield? Okay, let me tell you how to equip it! Start by pressing 'I' to access your inventory.");
                    equipShieldProgress = 4;
                }
            }

            if (equipShieldTutorialFinished
                && game.stateManager.stationState.SubStateManager.ShopMenuState.DisplayBuyAndEquip == false)
            {
                game.stateManager.stationState.SubStateManager.ShopMenuState.DisplayBuyAndEquip = true;
            }

            if (ShipInventoryManager.HasItemOfKind("LongShot")
                && MissionManager.GetMission(MissionID.Main3_DefendColony).MissionState.Equals(StateOfMission.Unavailable)
                && GameStateManager.currentState.Equals("OverworldState") && !longShotTutorialActivated)
            {
                tempTimer3 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer3 <= 0)
                {
                    tempTimer3 = 1000;
                    longShotTutorialActivated = true;
                    DisplayTutorialMessage(new List<string>(){
                    String.Format("You have recieved your first long-range weapon: 'LongShot'. You can have two weapons equipped at a time, and you can toggle between them in combat using '{0}'.", ControlManager.GetKeyName(RebindableKeys.Action2)), 
                    "Try accessing your inventory with 'I' and equip LongShot on one slot and SpreadBullet on the other. Then you can vary your strategy in combat depending on which enemies you encounter."});
                }
            }

            if (ShipInventoryManager.OwnedSecondary.Count > 0 && GameStateManager.currentState.Equals("OverworldState") 
                && !secondaryWeaponTutorialDisplayed)
            {
                tempTimer3 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer3 <= 0)
                {
                    tempTimer3 = 1000;
                    secondaryWeaponTutorialDisplayed = true;
                    DisplayTutorialMessage("You have acquired your first secondary weapon! Don't forget to equip it if you havn't done so already. Secondary weapons are fired automatically and don't use energy, so they are very handy!");
                }
            }

            if (isAMissionFailed && GameStateManager.currentState.Equals("OverworldState")
                && !hasDisplayedSkipMessage)
            {
                tempTimer2 -= gameTime.ElapsedGameTime.Milliseconds;

                if (tempTimer2 < 0)
                {
                    tempTimer2 = 500;

                    hasDisplayedSkipMessage = true;
                    DisplayTutorialMessage("When you fail a mission, you can replay most of them by going back to where they started. Press 'Escape' to skip dialogs and mission events you have already seen.");
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
            tutorialProgress.Add("hasStartedSecondMission", coordinatesDisplayed.ToString());
            tutorialProgress.Add("equipShieldTutorial", equipShieldTutorialFinished.ToString());
            tutorialProgress.Add("longShotTutorial", longShotTutorialActivated.ToString());
            tutorialProgress.Add("hasEnteredShooterWithShield", hasEnteredShooterWithShield.ToString());
            tutorialProgress.Add("hasDisplayedSecondary", secondaryWeaponTutorialDisplayed.ToString());
            tutorialProgress.Add("hasDisplayedSkipMessage", hasDisplayedSkipMessage.ToString());

            game.saveFile.Save(Game1.SaveFilePath, "save.ini", "tutorialprogress", tutorialProgress);
        }

        public void Load()
        {
            hasEnteredStation = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredstation", false);
            hasEnteredOverworld = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredoverworld", false);
            hasEnteredVerticalShooter = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredverticalshooter", false);
            hasEnteredShop = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredshop", false);
            hasEnteredInventory = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredinventory", false);
            hasEnteredHighfenceBeaconArea = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredhighfencebeaconarea", false);
            coordinatesDisplayed = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasstartedsecondmission", false);
            equipShieldTutorialFinished = game.saveFile.GetPropertyAsBool("tutorialprogress", "equipshieldtutorial", false);
            longShotTutorialActivated = game.saveFile.GetPropertyAsBool("tutorialprogress", "longshottutorial", false);
            hasEnteredShooterWithShield = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasenteredshooterwithshield", false);
            secondaryWeaponTutorialDisplayed = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasdisplayedsecondary", false);
            hasDisplayedSkipMessage = game.saveFile.GetPropertyAsBool("tutorialprogress", "hasdisplayedskipmessage", false);
        }

        public Sprite GetImageFromEnum(TutorialImage imageID)
        {
            return tutorialImages[(int)imageID];
        }

        public void EnableEquipTutorial()
        {
            equipShieldTutorial = true;
        }
    }
}
