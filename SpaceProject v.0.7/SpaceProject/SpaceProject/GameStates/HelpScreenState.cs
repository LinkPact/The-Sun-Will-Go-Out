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

            options.Add("Overworld Controls");
            options.Add("Shooter Controls");
            options.Add("Menus");
            options.Add("Overworld");
            options.Add("Combat");
            options.Add("Health");
            options.Add("Stations");
            options.Add("Planets & Colonies");
            options.Add("Beacons");
            options.Add("Missions");
            options.Add("Shop");
            options.Add("Inventory");
            options.Add("Back");

            helpText.Add("- Use 'Arrows' to navigate \n- 'Enter' or 'Left Ctrl' to interact with planets and other things\n- 'R' to toggle radar between far-out and zoomed-in-and-detailed\n- 'M' to access mission screen\n- 'H' to access help screen\n- 'I' to access inventory\n- 'Escape' to access menu");
            helpText.Add("- 'Arrows' to navigate\n- 'Left Ctrl' to fire current weapon\n- 'Left Shift' to switch primary weapon\n- 'Space' to toggle firing of secondary weapon on and off\n- 'Escape' to paus / access menus");
            helpText.Add("- 'Arrows' to move around\n- 'Enter' or 'Left Ctrl' to confirm\n- 'Escape' or 'Left Shift' to go back ");
            helpText.Add("In the overword you are able to see stats like how much fuel you have (yellow bar down to the left), how much health your ship has (green bar). \nYou can also see your current coordinates (this space only has two spacial dimensions). \nFinally you have a radar which you can toggle between two modes. \n\nFar-out radar \nHere, you have an overview of Sector X. You can see your position (white) in relation to planets and stations (red).\n\nZoomed-in radar\nThis is a more detaied radar showing information about planets and space stations (yellow) mission objects (gray), other space objects (gray), friendly ships (blue) and hostile ships (red).");
            helpText.Add("Down to the left there are three bars and a name. \n\nThe red bar is your ships health. When this is zero you lose the level.\nThe green bar is your current energy. Energy is depleted when you fire. \nThe blue bar is your shield. The shield can absorb some damage, and while your health won't replenish during the level, your shield will. It charges faster if your energy is filled.\n\nThe goal is usually to manage your way through level. This is usually best achieved by a combination of shooting some enemies while avoiding others.");
            helpText.Add("Health, or 'Armor', determines how many hits your ship can take before exploding. There are two different health variables in the game:\n\n- The health you have in the overworld, this determines your starting health when entering combat. If this health runs out, it's game over.\n\n- The health you have in combat. When this health runs out, you fail the level and your overworld-health is reduced a bit."); 
            helpText.Add("- Missions: Interact with the sector's inhabitants and embark on various missions. There is a main story you can follow.\nThere are also several side missions you can take in order to help the inhabitants in Sector X.\n- Rumours: Here you can get useful hints and non-useful dravel. You decide what is what.\n- Shop: Here you can buy and sell inventory and fuel.\n- Exit: Exit the space station.");
            helpText.Add("- Colony: If there are inhabitants on the planet, you can access the same menues as are present at the space stations here.\n- Mining: Rumours say you are able to mine valuable minerals on one planet in Sector X, if you manage to find a DrillBeam..\n- Planet info: General info about the planet.");
            helpText.Add("Beacons are warp points you can use to travel between planets. A beacon has to be activated before you can use it. When you have activated a beacon, you will be able to travel back to that beacon from any other beacon.");
            helpText.Add("In the game you will be given different missions from the inhabitants of the system. Most missions can be found on space stations and colonies on planets. You can view your current missions by pressing M. This screen shows you your current missions, your completed missions and your failed missions. Always check the mission screen if you are unsure what to do on a mission.");
            helpText.Add("- In order to buy or sell items you first mark the desired item using 'Enter' or 'Left Ctrl'. A menu will then pop up with options for what you can do with the item, for example buy or sell. When you are finished, click 'Escape' or 'Left Shift' to exit the shop.\n\n- You are also able to buy fuel, which is necessary to move in space. You start out with a filled tank, so you will manage quite long before needing to refill.");
            helpText.Add("The inventory allows you to access the equipment you buy or gather during your quests. \n\n- Use 'I' to access.\n\n Your ship have several equipment-slots.\n- Two primary weapon slots. \n- One Secondary weapon slot.\n- One Shield slot.\n- One Energy cell slot.\n\n To change your equipment select the slot you wish to equip on with 'Enter' or 'Left Ctrl' and select your new item from the list. Confirm with 'Enter' or 'Left Ctrl'.\n\nIt is also possible to trash items from the inventory by selecting them with 'Enter' or 'Left Ctrl' and moving them to the trash slot. ");
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

            Controls(gameTime);
        }

        private void Controls(GameTime gameTime)
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
                ControlManager.CheckKeypress(Keys.Enter))
            {
                ButtonActions();
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
            base.Draw(spriteBatch);

            for (int i = 0; i < options.Count; i++)
            {
                if (cursorIndex == i)
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), options[i],
                        new Vector2(10, 10 + (i * 20)), Color.Red);
                }

                else
                {
                    spriteBatch.DrawString(Game.fontManager.GetFont(14), options[i],
                        new Vector2(10, 10 + (i * 20)), Color.White);
                }
            }

            if (!options[cursorIndex].ToLower().Equals("back"))
            {
                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                    TextUtils.WordWrap(Game.fontManager.GetFont(14), helpText[cursorIndex], 390),
                    new Vector2(400, 10), Color.White);
            }
        }
    }
}
