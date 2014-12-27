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
    class ShipManagerCursor
    {
        #region variables

        //Technical
        private Game1 Game;
        private Sprite spriteSheet;

        //Display-objects
        private ShipInventoryDisplayObject platingDisplay;
        private ShipInventoryDisplayObject cellDisplay;
        private ShipInventoryDisplayObject shieldDisplay;
        private ShipInventoryDisplayObject primaryDisplay;
        private ShipInventoryDisplayObject primaryDisplay2;
        private ShipInventoryDisplayObject secondaryDisplay;
        private ShipInventoryDisplayObject inventoryDisplay;

        private ShipInventoryDisplayObject back;

        public List<ShipInventoryDisplayObject> displayList;

        private int inventoryPos = 5;

        //Cursor-related variables
        private int layer;
        private CursorCoordinate cursorCoordLv1;
        private int cursorLv2Pos;
        private int cursorLv3Pos;

        private Cursor CursorLevel2;
        private Cursor CursorLevel3;

        #endregion
        public ShipManagerCursor(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            int xSpacing = 80;
            int ySpacing = 60;
            int totalWidth = 200;
            int totalHeight = 100;
            int xOrigin = (ShipManagerState.GetUpperLeftRectangle.Width - totalWidth) / 3;
            int yOrigin = (ShipManagerState.GetUpperLeftRectangle.Height - totalHeight) / 2;

            displayList = new List<ShipInventoryDisplayObject>();
         
            Coordinate primaryCoord1 = new Coordinate(0, 0);
            displayList.Add(primaryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(303, 773, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(383, 773, 40, 40)), 
                new Vector2(xOrigin, yOrigin), primaryCoord1));

            Coordinate primaryCoord2 = new Coordinate(0, 1);
            displayList.Add(primaryDisplay2 = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(303, 773, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(383, 773, 40, 40)), 
                new Vector2(xOrigin, yOrigin), primaryCoord2));

            Coordinate secondaryCoord = new Coordinate(1, 0);
            displayList.Add(secondaryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(303, 813, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(383, 813, 40, 40)),
                new Vector2(xOrigin + xSpacing, yOrigin), secondaryCoord));

            Coordinate platingCoord = new Coordinate(2, 0);
            displayList.Add(platingDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(303, 853, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(383, 853, 40, 40)),
                new Vector2(xOrigin + xSpacing * 2, yOrigin), platingCoord));

            Coordinate batteryCoord = new Coordinate(0, 1);
            displayList.Add(cellDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(343, 853, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(423, 853, 40, 40)),
                new Vector2(xOrigin, yOrigin + ySpacing), batteryCoord));

            Coordinate shieldCoord = new Coordinate(1, 1);
            displayList.Add(shieldDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(303, 893, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(383, 893, 40, 40)),
                new Vector2(xOrigin + xSpacing, yOrigin + ySpacing), shieldCoord));

            Coordinate inventoryCoord = new Coordinate(2, 1);
            displayList.Add(inventoryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(225, 15, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(305, 15, 40, 40)),
                new Vector2(xOrigin + xSpacing * 2, yOrigin + ySpacing), inventoryCoord));

            Coordinate back2Coord = new Coordinate(3, 1);
            displayList.Add(back = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(145, 95, 40, 40)),
                spriteSheet.GetSubSprite(new Rectangle(145, 135, 40, 40)),
                new Vector2(xOrigin + xSpacing * 3, yOrigin + ySpacing), back2Coord));

            CursorLevel2 = new Cursor(Game, spriteSheet);
            CursorLevel2.Initialize();
            CursorLevel3 = new Cursor(Game, spriteSheet);
            CursorLevel3.Initialize();
        }

        public void Update(GameTime gameTime, int layer, CursorCoordinate cursorCoordLv1, int cursorLv2Pos, int cursorLv3Pos)
        {
            this.layer = layer;
            this.cursorCoordLv1 = cursorCoordLv1;
            this.cursorLv2Pos = cursorLv2Pos;
            this.cursorLv3Pos = cursorLv3Pos;

            foreach (ShipInventoryDisplayObject menuDisplay in displayList)
            { 
                menuDisplay.UpdateActivity(cursorCoordLv1);
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
                        int cursorLevel1Position = cursorCoordLv1.ToInt();

                        CursorLevel2.isVisible = true;
                        CursorLevel2.isActive = true;
                        CursorLevel3.isVisible = false;
                        CursorLevel3.isActive = false;

                        if (cursorLevel1Position != inventoryPos)
                        {
                            CursorLevel2.position.X = Game.Window.ClientBounds.Width / 2 + 39;
                            CursorLevel2.position.Y = (float)(105 + cursorLv2Pos * 23);
                        }

                        if (cursorCoordLv1.ToInt() == inventoryPos)
                        {
                            if (cursorLv2Pos < 14)
                            {
                                CursorLevel2.position.X = Game.Window.ClientBounds.Width / 2 + 10;
                                CursorLevel2.position.Y = (float)(53 + cursorLv2Pos * 23);
                            }
                            else
                            {
                                CursorLevel2.position.X = Game.Window.ClientBounds.Width * 3/4 - 10;
                                CursorLevel2.position.Y = (float)(53 + (cursorLv2Pos - 14) * 23);
                            }
                        }

                        break;
                    }
                case 3:
                    {
                        int cursorLevel1Position = cursorCoordLv1.ToInt();

                        CursorLevel2.isVisible = true;
                        CursorLevel2.isActive = false;
                        CursorLevel3.isVisible = true;
                        CursorLevel3.isActive = true;

                        if (cursorLevel1Position != inventoryPos)
                        {
                            CursorLevel3.position.X = Game.Window.ClientBounds.Width / 2 + 39;
                            CursorLevel3.position.Y = (float)(185 + cursorLv3Pos * 23);
                        }

                        if (cursorLevel1Position == inventoryPos)
                        {
                            if (cursorLv3Pos == -1)
                            {
                                CursorLevel3.position.X = Game.Window.ClientBounds.Width / 2 + 10;
                                CursorLevel3.position.Y = Game.Window.ClientBounds.Width / 2 + 13;
                            }
                            else if (cursorLv3Pos < 14)
                            {
                                CursorLevel3.position.X = Game.Window.ClientBounds.Width / 2 + 10;
                                CursorLevel3.position.Y = (float)(53 + cursorLv3Pos * 23);
                            }
                            else
                            {
                                CursorLevel3.position.X = Game.Window.ClientBounds.Width * 3 / 4 - 10;
                                CursorLevel3.position.Y = (float)(53 + (cursorLv3Pos - 14) * 23);
                            }
                        }

                        break;
                    }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ShipInventoryDisplayObject dispObj in displayList)
            {
                dispObj.Draw(spriteBatch);
            }

            #region InventorySubMenues
            if (primaryDisplay.isActive)
            {
                ShipManagerText.DisplayPrimaryWeaponInfo(spriteBatch);

                if (layer >= 2)
                    InventoryInformation.DisplayPrimaryWeaponInfo(spriteBatch);
            }

            if (secondaryDisplay.isActive)
            {
                ShipManagerText.DisplaySecondaryInfo(spriteBatch);

                if (layer >= 2)
                    InventoryInformation.DisplaySecondaryWeaponInfo(spriteBatch);
            }

            if (platingDisplay.isActive)
            {
                ShipManagerText.DisplayPlatingInfo(spriteBatch);

                if (layer >= 2)
                    InventoryInformation.DisplayPlatingInfo(spriteBatch);
            }

            if (cellDisplay.isActive)
            {
                ShipManagerText.DisplayEnergyCellInfo(spriteBatch);

                if (layer >= 2)
                    InventoryInformation.DisplayEnergyCellInfo(spriteBatch);
            }

            if (shieldDisplay.isActive)
            {
                ShipManagerText.DisplayShieldInfo(spriteBatch);

                if (layer >= 2)
                    InventoryInformation.DisplayShieldInfo(spriteBatch);
            }

            //if (inventoryDisplay.isActive && cursorLv3Pos != -1) // Why was this here to begin with? /Jakob 140615
            if (inventoryDisplay.isActive)
            {
                ShipManagerText.DisplayInventory(spriteBatch, Game.Resolution);

                if (layer >= 2)
                    InventoryInformation.DisplayInventoryInfo(spriteBatch);
            }

            if (back.isActive)
            {
                ShipManagerText.DisplayBackInfo(spriteBatch);
            }

            #endregion

            CursorLevel2.Draw(spriteBatch);
            CursorLevel3.Draw(spriteBatch);
        }
    }
}
