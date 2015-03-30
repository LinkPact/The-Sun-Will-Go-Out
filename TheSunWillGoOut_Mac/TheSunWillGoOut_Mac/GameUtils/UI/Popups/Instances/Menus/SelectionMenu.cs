using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class SelectionMenu : Menu
    {
        private readonly float MenuOptionYDistance = 30f;

        protected TextContainer textContainer;

        public SelectionMenu(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            canvas = spriteSheet.GetSubSprite(new Rectangle(0, 884, 400, 309));
        }

        public override void Initialize()
        {
            base.Initialize();

            textContainer = new TextContainer(canvasPosition, canvas.SourceRectangle.Value);
            textContainer.Initialize();
            textContainer.SetDefaultPosition(this.GetType());
            textContainer.UseScrolling = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            textContainer.Update(gameTime, canvasPosition);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            textContainer.Draw(spriteBatch);
            DrawMenuOptions(spriteBatch);
        }

        protected override void OnPress(RebindableKeys key)
        {
            base.OnPress(key);

            switch (key)
            {
                case RebindableKeys.Up:
                    {
                        cursorIndex--;
                        CheckCursorIndex();
                        break;
                    }

                case RebindableKeys.Down:
                    {
                        cursorIndex++;
                        CheckCursorIndex();
                        break;
                    }
            }
        }

        protected override void DefaultOnPressActions()
        {
            switch (menuOptions[cursorIndex].ToLower())
            {
                case "save and exit to menu":
                case "save and restart":
                    game.GameStarted = false;
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
                    Hide();
                    game.Restart();
                    break;

                case "exit to desktop without saving":
                    game.Exit();
                    break;

                case "cancel":
                    Hide();
                    DisplayMenuOnReturn(5);
                    break;
            }
        }

        protected override void DrawMenuOptions(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuOptions.Count; i++)
            {
                Color color = FontManager.FontColorStatic;

                if (cursorIndex == i)
                {
                    color = Color.LightBlue;
                }

                spriteBatch.DrawString(FontManager.GetFontStatic(14),
                    menuOptions[i],
                    new Vector2(canvasPosition.X,
                                canvasPosition.Y + (i * MenuOptionYDistance)) + FontManager.FontOffsetStatic,
                    color,
                    0f,
                    FontManager.GetFontStatic(14).MeasureString(menuOptions[i]) / 2,
                    1f,
                    SpriteEffects.None,
                    1f);
            }
        }

        public void SetMessage(params string[] messages)
        {
            textContainer.SetMessage(messages);
        }
    }
}
