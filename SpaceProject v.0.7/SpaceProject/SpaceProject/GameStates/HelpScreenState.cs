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
            //options.Add("Health");
            options.Add("Stations");
            options.Add("Planets & Colonies");
            options.Add("Beacons");
            options.Add("Missions");
            options.Add("Shop");
            options.Add("Inventory");
            options.Add("Back");

            helpText.Add("- 'Arrows' to navigate \n- 'Enter' or 'Left Ctrl' to interact with planets, stations and other things\n- 'Escape' to access menu\n\n-'N' to access map\n- 'M' to access mission screen\n- 'H' to access help screen\n- 'I' to access inventory");
            helpText.Add("- 'Arrows' to navigate\n- 'Left Ctrl' to fire current weapon\n- 'Left Shift' to switch primary weapon\n- 'Escape' to pause / access menus");
            helpText.Add("- 'Arrows' to move around\n- 'Enter' or 'Left Ctrl' to confirm\n- 'Escape' or 'Left Shift' to go back ");
            helpText.Add("In the overworld you can see your current coordinates (this space only has two spacial dimensions) as well as the radar at the bottom right of the screen.\n\nThe Radar gives you information about the ships, planets, stations and other objects near you by different colored dots:\n\nWhite: The dot in the middle of the radar is your ship's position.\nBlue: Stations, planets and other objects that you can interact with.\nDollar signs: Shops where you can upgrade your ship's equipment.\nGreen: Friendly ships travelling the sector.\nRed: Hostile ships travelling the sector. Careful, they will attack you if they get the chance.\nGray: Asteroids. They are mostly empty, but some of them might hold a secret.\nBlinking gold/silver: Mission objective locations. Gold is for main missions, silver is for side missions. When not on screen, they are shown as an arrow pointing to the location.");
            helpText.Add("Down to the left there are three bars and a name. \n\nThe red bar is your ships health. When this is zero you lose the level.\nThe green bar is your current energy. Energy is depleted when you fire. \nThe blue bar is your shield. The shield can absorb some damage, and while your health won't replenish during the level, your shield will. It charges faster if your energy is filled.\n\nThe goal is usually to survive through the level. This is usually best achieved by a combination of shooting some enemies while avoiding others.");
            //helpText.Add("Health, or 'Armor', determines how many hits your ship can take before exploding. There are two different health variables in the game:\n\n- The health you have in the overworld, this determines your starting health when entering combat. If this health runs out, it's game over.\n\n- The health you have in combat. When this health runs out, you fail the level and your overworld-health is reduced a bit."); 
            helpText.Add("- Missions: Interact with the sector's inhabitants and embark on various missions. There is a main story you can follow.\nThere are also several side missions you can take in order to help the inhabitants in Sector X.\n- Rumours: Here you can get useful hints and non-useful dravel. You decide what is what.\n- Shop: Here you can buy and sell items and ship parts.\n- Exit: Exit the space station.");
            helpText.Add("- Colony: If there are inhabitants on the planet, you can access the same menues as are present at the space stations here.\n- Planet info: General info about the planet.");
            helpText.Add("Beacons are warp points you can use to travel between planets. A beacon has to be activated before you can use it. When you have activated a beacon, you will be able to travel back to that beacon from any other beacon.");
            helpText.Add("In the game you will be given different missions from the inhabitants of the system. Most missions can be found on space stations and colonies on planets. You can view your current missions by pressing M. This screen shows you your current missions, your completed missions and your failed missions. Always check the mission screen if you are unsure what to do on a mission.");
            helpText.Add("- In order to buy or sell items you first mark the desired item using 'Enter' or 'Left Ctrl'. A menu will then pop up with options for what you can do with the item, for example buy or sell. When you are finished, click 'Escape' or 'Left Shift' to exit the shop.");
            helpText.Add("The inventory allows you to access the equipment you buy or gather during your quests. \n\n- Use 'I' to access.\n\n Your ship have several equipment-slots.\n- Two primary weapon slots. \n- One Secondary weapon slot.\n- One Shield slot.\n- One Energy cell slot.\n- One Plating slot\n\n To change your equipment select the slot you wish to equip on with 'Enter' or 'Left Ctrl' and select your new item from the list. Confirm with 'Enter' or 'Left Ctrl'.");

            helpImages.Add("overworld controls", TutorialImage.OverworldControls);
            helpImages.Add("shooter controls", TutorialImage.CombatControls);
            helpImages.Add("menu controls", TutorialImage.MenuControls);
            helpImages.Add("overworld", TutorialImage.Radar);
            helpImages.Add("combat", TutorialImage.CombatBars);
            helpImages.Add("stations", TutorialImage.Stations);
            helpImages.Add("planets & colonies", TutorialImage.Planets);
            helpImages.Add("beacons", TutorialImage.Beacons);
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
                        TextUtils.WordWrap(Game.fontManager.GetFont(14), helpText[cursorIndex], 390),
                        new Vector2(Game.ScreenCenter.X, 10), Color.White);
                    break;

                case "back":
                    break;

                default:
                    TutorialImage tutImg;
                    helpImages.TryGetValue(options[cursorIndex].ToLower(), out tutImg);
                    sprite = Game.tutorialManager.GetImageFromEnum(tutImg);

                    spriteBatch.Draw(sprite.Texture,
                        new Vector2(Game.ScreenCenter.X, 10),
                        sprite.SourceRectangle,
                        Color.White,
                        0f,
                        Vector2.Zero,
                        1f,
                        SpriteEffects.None,
                        1f);

                    spriteBatch.DrawString(Game.fontManager.GetFont(14),
                        TextUtils.WordWrap(Game.fontManager.GetFont(14), helpText[cursorIndex], 390),
                        new Vector2(Game.ScreenCenter.X, 210), Color.White);
                    break;
            }
        }
    }
}
