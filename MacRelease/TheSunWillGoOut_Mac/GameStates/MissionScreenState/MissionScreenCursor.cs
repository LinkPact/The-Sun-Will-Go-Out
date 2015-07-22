using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    class MissionScreenCursor
    {
        //Technical
        private Game1 Game;
        private Sprite spriteSheet;

        //Display-objects
        private MenuDisplayObject activeMissionsDisplay;
        private MenuDisplayObject completedMissionsDisplay;
        private MenuDisplayObject failedMissionsDisplay;
        private MenuDisplayObject backDisplay;

        public List<MenuDisplayObject> displayList;

        //Cursor-related variables
        private int layer;
        private int var1;
        private int var2;

        private Cursor CursorLevel2;

        private float edgePadding;

        public MissionScreenCursor(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            edgePadding = Game1.ScreenSize.X / 16;

            int spacing = 75;
            float totalButtonHeight = spacing * 3;
            int xOrigin = MissionScreenState.GetLowerLeftRectangle.Width / 2;
            float yOrigin = Game.ScreenCenter.Y + Game.ScreenCenter.Y / 2 - totalButtonHeight / 2;

            displayList = new List<MenuDisplayObject>();

            displayList.Add(activeMissionsDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(0, 216, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                new Vector2(xOrigin, yOrigin),
                new Vector2(79.5f, 20)));

            activeMissionsDisplay.name = "Active Missions";

            displayList.Add(completedMissionsDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(0, 216, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                new Vector2(xOrigin, yOrigin + spacing),
                new Vector2(79.5f, 20)));

            completedMissionsDisplay.name = "Completed Missions";

            displayList.Add(failedMissionsDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(0, 216, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                new Vector2(xOrigin, yOrigin + spacing * 2),
                new Vector2(79.5f, 20)));

            failedMissionsDisplay.name = "Failed Missions";

            displayList.Add(backDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(0, 216, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                spriteSheet.GetSubSprite(new Rectangle(0, 258, 159, 40)),
                new Vector2(xOrigin, yOrigin + spacing * 3),
                new Vector2(79.5f, 20)));

            backDisplay.name = "Back";

            CursorLevel2 = new Cursor(Game, spriteSheet, new Rectangle(120, 124, 14, 14), new Rectangle(120, 138, 14, 14));
            CursorLevel2.Initialize();
        }

        public void Update(GameTime gameTime, int layer, int var1, int var2)
        {

            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;        
            
            for (int n = 0; n < displayList.Count; n++)
            {
                if (n != var1)
                {
                    displayList[n].isSelected = false;
                    displayList[n].isActive = false;
                }
                    
                else if (n == var1)
                {
                    if (layer == 1)
                        displayList[n].isActive = true;
                    else if (layer == 2)
                    {
                        displayList[n].isActive = false;
                        displayList[n].isSelected = true;
                    }
                }
            }

            CursorHandling();
        }

        public void CursorHandling()
        {
            switch (layer)
            {
                case 1:
                    {
                        CursorLevel2.isActive = false;
                        CursorLevel2.isVisible = false;
                        break;
                    }
                case 2:
                    {
                        CursorLevel2.isVisible = true;
                        CursorLevel2.isActive = true;

                        if (var1 >= 0 && var1 <= 4)
                        {
                            CursorLevel2.position.X = MissionScreenState.GetRightRectangle.X + edgePadding - 15;
                            CursorLevel2.position.Y = (float)(105 + var2 * 23);
                        }

                        break;
                    }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < displayList.Count; i++)
            {
                displayList[i].Draw(spriteBatch);

                spriteBatch.DrawString(FontManager.GetFontStatic(14), displayList[i].name, displayList[i].Position,
                    var1 == i ? FontManager.FontSelectColor1 : FontManager.FontColorStatic, 0f,
                    FontManager.GetFontStatic(14).MeasureString(displayList[i].name) / 2, 1f, SpriteEffects.None, 1f);
            }

            CursorLevel2.Draw(spriteBatch);
        }
    }
}
