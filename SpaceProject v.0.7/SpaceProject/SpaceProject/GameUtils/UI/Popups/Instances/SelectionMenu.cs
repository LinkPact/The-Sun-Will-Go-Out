using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class SelectionMenu : Menu
    {
        private readonly float TextLayerDepth = 1f;

        public SelectionMenu(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 56, 269, 184));
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void OnPress()
        {
            base.OnPress();
        }

        protected override void DefaultOnPressActions()
        {
            switch (menuOptions[cursorIndex].ToLower())
            {
                case "save and exit to menu":
                case "save and restart":
                    game.GameStarted = false;
                    textBuffer.Remove(textBuffer[0]);
                    Hide();
                    game.Save();
                    game.Restart();
                    break;

                case "save and exit to desktop":
                    game.Save();
                    game.Exit();
                    break;

                case "exit to menu without saving":
                case "restart":
                    game.GameStarted = false;
                    textBuffer.Remove(textBuffer[0]);
                    Hide();
                    game.Restart();
                    break;

                case "exit to desktop without saving":
                    game.Exit();
                    break;

                case "cancel":
                    textBuffer.Remove(textBuffer[0]);
                    Game1.Paused = false;
                    Hide();

                    if (GameStateManager.currentState.Equals("OverworldState"))
                    {
                        game.messageBox.DisplayMenu();
                        cursorIndex = 5;
                    }
                    break;
            }
        }
    }
}
