using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class HelpScreenState : GameState
    {
        private readonly int WordWrapWidth = 470;
        private float ImageXOffset = 50;

        private List<string> options;
        private List<string> helpText;
        private Dictionary<string, TutorialImage> helpImages;

        private int cursorIndex;

        public HelpScreenState(Game1 game, String name) :
            base(game, name)
        {

        }

        public override void Initialize()
        {
            base.Initialize();

            options = new List<string>();
            helpText = new List<string>();
            helpImages = new Dictionary<string, TutorialImage>();

            options.Add("Overworld Controls");
            options.Add("Shooter Controls");
            options.Add("Menu Controls");
            options.Add("Overworld");
            options.Add("Combat");
            options.Add("Stations & Planets");
            options.Add("Beacons");
            options.Add("Missions");
            options.Add("Shops");
            options.Add("Inventory");
            options.Add("Back");

            helpText.Add("- 'Arrows' to navigate \n- 'Enter' or 'Left Ctrl' to interact with planets, stations and other things\n- 'Escape' to access menu\n-'M' to access map\n- 'L' to access mission log\n- 'H' to access help screen\n- 'I' to access inventory");
            helpText.Add("- 'Arrows' to navigate\n- 'Left Ctrl' to fire current weapon\n- 'Left Shift' to switch primary weapon\n- 'Escape' to pause / access menus");
            helpText.Add("- 'Arrows' to move around\n- 'Enter' or 'Left Ctrl' to confirm\n- 'Escape' or 'Left Shift' to go back \n\nIn the menus, you can also navigate with the mouse.");
            helpText.Add("In the overworld you can see your current coordinates (this space only has two spacial dimensions) as well as the radar at the bottom right of the screen.\n\nThe Radar gives you information about the ships, planets, stations and other objects near you by different colored dots:\n\n- White: The dot in the middle of the radar is your ship's position.\n\n- Blue: Stations, planets and other objects that you can interact with.\n\n- Dollar signs: Shops where you can upgrade your ship's equipment.\n\n- Green: Friendly ships travelling the sector.\n\n- Red: Hostile ships travelling the sector. Careful, they will attack you if they get the chance.\n\n- Gray: Asteroids. They are mostly empty, but some of them might hold a secret.\n\n- Blinking gold/silver: Mission objective locations. Gold is for main missions, silver is for side missions. When not on screen, they are shown as an arrow pointing to the location.");
            helpText.Add("During combat there are three bars and some text down at the bottom left of the screen. \n\n- 'Objective' displays the condition to win the level. The goal is generally to survive through the level. This is usually best achieved by a combination of shooting some enemies while avoiding others.\n\n- 'Primary' displays your currently armed primary weapon. Switch between equipped primary weapons with 'Left Shift'\n\n- 'Secondary' displays your equipped secondary weapon, they fire automatically and don't use any energy\n\n- The red bar is your ships health. When this is zero you lose the level.\n\n- The green bar is your current energy. Energy is depleted when you fire. \n\n- The blue bar is your shield. The shield can absorb some damage, and while your health won't replenish during the level, your shield will. It charges faster if your energy is filled."); 
            helpText.Add("Some of the stations and planets in the sector have inhabitants living on them. You can interact with them through a few options: \n\n- Missions: Interact with the sector's inhabitants and embark on various missions. There is a main story you can follow.\nThere are also several side missions you can take in order to help the inhabitants in Sector X.\n\n- Rumors: Here you can get useful hints and non-useful dravel. You decide what is what.\n\n- Back: Exit the station or planet.");
            helpText.Add("Beacons are warp points you can use to travel between planets. A beacon has to be activated before you can use it. When you have activated a beacon by flying close to it, you will be able to travel back to that beacon from any other beacon.");
            helpText.Add("In the game you will be given different missions from the inhabitants of the system. Most missions can be found on space stations and colonies on planets. You can view your current missions by pressing 'L'. This screen shows you your current missions, your completed missions and your failed missions. Always check the mission screen if you are unsure what to do on a mission.");
            helpText.Add("There are four shop stations spread out over the sector. Interact with them to enter the shop.\n\nIn order to buy or sell items you first mark the desired item using 'Enter' or 'Left Ctrl'. A menu will then pop up with options for what you can do with the item, for example buy or sell. When you are finished, click 'Escape' or 'Left Shift' to exit the shop.");
            helpText.Add("The inventory allows you to access the equipment you buy or gather during your quests. \n\n- Use 'I' to access.\n\n Your ship have several equipment-slots.\n- Two primary weapon slots. \n- One Secondary weapon slot.\n- One Shield slot.\n- One Energy cell slot.\n- One Plating slot\n\n To change your equipment select the slot you wish to equip on with 'Enter' or 'Left Ctrl' and select your new item from the list. Confirm with 'Enter' or 'Left Ctrl'.");

            helpImages.Add("overworld controls", TutorialImage.OverworldControls);
            helpImages.Add("shooter controls", TutorialImage.CombatControls);
            helpImages.Add("menu controls", TutorialImage.MenuControls);
            helpImages.Add("overworld", TutorialImage.Radar);
            helpImages.Add("combat", TutorialImage.CombatBars);
            helpImages.Add("stations & planets", TutorialImage.StationsPlanets);
            helpImages.Add("beacons", TutorialImage.Beacons);
            helpImages.Add("shops", TutorialImage.ShopStations);
        }

        public override void OnEnter()
        {
            base.OnEnter();

            cursorIndex = 0;
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ButtonControls();
            MouseControls();
        }

        private void ButtonControls()
        {
            if (ControlManager.CheckPress(RebindableKeys.Down))
            {
                cursorIndex++;
            }

            else if (ControlManager.CheckPress(RebindableKeys.Up))
            {
                cursorIndex--;
            }

            if (cursorIndex > options.Count - 1)
            {
                cursorIndex = 0;
            }

            else if (cursorIndex < 0)
            {
                cursorIndex = options.Count - 1;
            }

            if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                ControlManager.CheckKeyPress(Keys.Enter))
            {
                ButtonActions();
            }

            else if (ControlManager.CheckPress(RebindableKeys.Action2)
                || ControlManager.CheckPress(RebindableKeys.Pause)
                || ControlManager.CheckPress(RebindableKeys.Help))
            {
                Game.stateManager.ChangeState("OverworldState");
            }
        }

        private void MouseControls()
        {
            for (int i = 0; i < options.Count; i++)
            {
                if (ControlManager.IsMouseOverText(FontManager.GetFontStatic(14),
                    options[i], new Vector2(10, 10 + (i * 20)), false))
                {
                    if (ControlManager.IsMouseMoving())
                    {
                        cursorIndex = i;
                    }

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        ButtonActions();
                    }
                }
            }
        }

        private void ButtonActions()
        {
            switch (options[cursorIndex].ToLower())
            {
                case "back":
                    Game.stateManager.ChangeState("OverworldState");
                    break;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Sprite sprite;

            base.Draw(spriteBatch);

            for (int i = 0; i < options.Count; i++)
            {
                if (cursorIndex == i)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), options[i],
                        new Vector2(10, 10 + (i * 20)), FontManager.FontSelectColor1);
                }

                else
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), options[i],
                        new Vector2(10, 10 + (i * 20)), Color.White);
                }
            }

            switch(options[cursorIndex].ToLower())
            {
                case "health":
                case "missions":
                case "shop":
                case "inventory":
                    spriteBatch.DrawString(Game.fontManager.GetFont(14),
                        TextUtils.WordWrap(Game.fontManager.GetFont(14), helpText[cursorIndex], WordWrapWidth),
                        new Vector2(Game.ScreenCenter.X, 10), Color.White);
                    break;

                case "back":
                    break;

                default:
                    TutorialImage tutImg;
                    helpImages.TryGetValue(options[cursorIndex].ToLower(), out tutImg);
                    sprite = Game.tutorialManager.GetImageFromEnum(tutImg);

                    spriteBatch.Draw(sprite.Texture,
                        new Vector2(Game.ScreenCenter.X + ImageXOffset, 10),
                        sprite.SourceRectangle,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);

                    spriteBatch.DrawString(Game.fontManager.GetFont(14),
                        TextUtils.WordWrap(Game.fontManager.GetFont(14), helpText[cursorIndex], WordWrapWidth),
                        new Vector2(Game.ScreenCenter.X, 210), Color.White);
                    break;
            }
        }
    }
}
