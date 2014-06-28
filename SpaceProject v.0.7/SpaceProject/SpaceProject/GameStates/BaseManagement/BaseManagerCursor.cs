using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpaceProject;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    class BaseManagerCursor
    {
        #region init

        //Technical
        private Game1 Game;
        private Sprite spriteSheet;
        //private KeyboardState currentKeyboardState;
        //private KeyboardState previousKeyboardState;

        //Display-objects
        private MenuDisplayObject shipDisplay;
        private MenuDisplayObject batteryDisplay;
        //private MenuDisplayObject shieldDisplay;
        //private MenuDisplayObject primaryDisplay;
        //private MenuDisplayObject secondaryDisplay;
        //private MenuDisplayObject specialDisplay;
        //private MenuDisplayObject passiveDisplay;

        public List<MenuDisplayObject> displayList;

        //Cursor-related variables
        private int layer;
        private int var1;
        private int var2;
        private int var3;

        private int section;
        private bool split;

        private Cursor CursorLevel2;
        private Cursor CursorLevel3;

        #endregion
        public BaseManagerCursor(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
        }
        public void Initialize()
        {
            displayList = new List<MenuDisplayObject>();
            //50, 127, 204, 281, 358
            //displayList.Add(primaryDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(185, 15, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(265, 15, 40, 40)), new Vector2(40, 40)));
            //displayList.Add(secondaryDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(185, 55, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(265, 55, 40, 40)), new Vector2(40, 100)));
            displayList.Add(shipDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(185, 95, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(265, 95, 40, 40)), new Vector2(40, 40)));
            displayList.Add(batteryDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(225, 95, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(305, 95, 40, 40)), new Vector2(40, 100)));
            //displayList.Add(shieldDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(185, 135, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(265, 135, 40, 40)), new Vector2(160, 40)));
            //displayList.Add(specialDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(225, 15, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(305, 15, 40, 40)), new Vector2(160, 100)));
            //displayList.Add(passiveDisplay = new MenuDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(225, 55, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(305, 55, 40, 40)), new Vector2(50, 512)));

            CursorLevel2 = new Cursor(Game, spriteSheet);
            CursorLevel2.Initialize();
            CursorLevel3 = new Cursor(Game, spriteSheet);
            CursorLevel3.Initialize();
        }
        public void Update(GameTime gameTime, int layer, int var1, int var2, int var3, int section, bool split)
        {
            this.layer = layer;
            this.var1 = var1;
            this.var2 = var2;
            this.var3 = var3;
            this.section = section;
            this.split = split;

            //if (currentKeyboardState != null)
            //    previousKeyboardState = currentKeyboardState;
            //
            //currentKeyboardState = Keyboard.GetState();

            for (int n = 0; n < displayList.Count; n++)
            {
                if (n != var1)
                    displayList[n].isActive = false;
                else if (n == var1)
                    displayList[n].isActive = true;
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
                        CursorLevel3.isActive = false;
                        CursorLevel3.isVisible = false;
                        break;
                    }
                case 2:
                    {
                        if (section == 0)
                        {
                            CursorLevel2.isVisible = true;
                            CursorLevel2.isActive = true;
                            CursorLevel2.isSmall = false;
                            CursorLevel3.isVisible = false;
                            CursorLevel3.isActive = false;
                            //CursorLevel3.isSmall = false;

                            if (var1 == 1)
                            {
                                //CursorLevel2.position.X = 650.0f;
                                CursorLevel2.position.X = 439.0f;
                                CursorLevel2.position.Y = (float)(105 + var2 * 23);
                            }

                            if (var1 == 0)
                            {
                                if (var2 < 14)
                                {
                                    CursorLevel2.position.X = 410.0f;
                                    CursorLevel2.position.Y = (float)(53 + var2 * 23);
                                }
                                else
                                {
                                    CursorLevel2.position.X = 590.0f;
                                    CursorLevel2.position.Y = (float)(53 + (var2 - 14) * 23);
                                }

                                /*
                                if (var2 < Convert.ToInt32(ShipInventoryManager.inventorySize / 2))
                                {
                                    //CursorLevel2.position.X = 582.0f;
                                    CursorLevel2.position.X = 416.0f;
                                    CursorLevel2.position.Y = (float)(53 + var2 * 23);
                                }
                                else
                                {
                                    //CursorLevel2.position.X = 770.0f;
                                    CursorLevel2.position.X = 610.0f;
                                    CursorLevel2.position.Y = (float)(53 + (var2 - Convert.ToInt32(ShipInventoryManager.inventorySize / 2)) * 23);
                                }*/
                            }
                        }
                        else if (section == 1)
                        {
                            CursorLevel2.isVisible = true;
                            CursorLevel2.isActive = true;
                            CursorLevel2.isSmall = true;
                            CursorLevel3.isVisible = false;
                            CursorLevel3.isActive = false;

                            if (split) CursorLevel3.isSmall = false;
                            else CursorLevel3.isSmall = true;

                            //if (var1 == 1)
                            //{
                            //    CursorLevel2.position.X = 650.0f;
                            //    CursorLevel2.position.Y = (float)(107 + var2 * 23);
                            //}

                            if (var1 == 0)
                            {
                                if (var2 < Convert.ToInt32(BaseInventoryManager.inventorySize / 2))
                                {
                                    CursorLevel2.position.X = 20.0f;
                                    CursorLevel2.position.Y = (float)(195 + var2 * 19);
                                }
                                else
                                {
                                    CursorLevel2.position.X = 210.0f;
                                    CursorLevel2.position.Y = (float)(195 + (var2 - Convert.ToInt32(BaseInventoryManager.inventorySize / 2)) * 19);
                                }
                            }
                        }

                        break;
                    }
                case 3:
                    {
                        if (section == 0)
                        {
                            CursorLevel2.isVisible = true;
                            CursorLevel2.isActive = false;
                            CursorLevel3.isVisible = true;
                            CursorLevel3.isActive = true;

                            if (split)
                            {
                                CursorLevel2.isSmall = true;
                                CursorLevel3.isSmall = false;
                            }
                            else
                            {
                                CursorLevel2.isSmall = false;
                                CursorLevel3.isSmall = false;     
                            }
                            
                            if (var1 == 1)
                            {
                                CursorLevel3.position.X = 439.0f;
                                CursorLevel3.position.Y = (float)(185 + var3 * 23);
                            }

                            if (var1 == 0)
                            {
                                if (var3 == -1)
                                {
                                    CursorLevel3.position.X = 410.0f;
                                    CursorLevel3.position.Y = 413.0f;
                                }
                                else if (var3 < 14)
                                {
                                    CursorLevel3.position.X = 410.0f;
                                    CursorLevel3.position.Y = (float)(53 + var3 * 23);
                                }
                                else
                                {
                                    CursorLevel3.position.X = 590.0f;
                                    CursorLevel3.position.Y = (float)(53 + (var3 - 14) * 23);
                                }
                            }
                        }
                        else if (section == 1)
                        {
                            CursorLevel2.isVisible = true;
                            CursorLevel2.isActive = false;
                            CursorLevel3.isVisible = true;
                            CursorLevel3.isActive = true;
                            
                            if (section == 1 && split)
                            {
                                CursorLevel2.isSmall = false;
                                CursorLevel3.isSmall = true;
                            }
                            else
                            {
                                CursorLevel2.isSmall = true;
                                CursorLevel3.isSmall = true;
                            }

                            if (var3 == -1)
                            {
                                CursorLevel3.position.X = 20.0f;
                                CursorLevel3.position.Y = 580.0f;
                            }
                            else if (var3 < Convert.ToInt32(BaseInventoryManager.inventorySize / 2))
                            {
                                CursorLevel3.position.X = 20.0f;
                                CursorLevel3.position.Y = (float)(195 + var3 * 19);
                            }
                            else
                            {
                                CursorLevel3.position.X = 210.0f;
                                CursorLevel3.position.Y = (float)(195 + (var3 - Convert.ToInt32(BaseInventoryManager.inventorySize / 2)) * 19);
                            }
                        }

                        break;
                    }
            }
        }
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MenuDisplayObject dispObj in displayList)
            {
                dispObj.Draw(spriteBatch);
            }

            CursorLevel2.Draw(spriteBatch);
            CursorLevel3.Draw(spriteBatch);
        }
    }
}
