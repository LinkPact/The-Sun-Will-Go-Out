﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    class ShipManagerCursor
    {
        #region variables

        //Technical
        private Game1 Game;
        private Sprite spriteSheet;

        //Display-objects
        private ShipInventoryDisplayObject back;
        private ShipInventoryDisplayObject platingDisplay;
        private ShipInventoryDisplayObject cellDisplay;
        private ShipInventoryDisplayObject shieldDisplay;
        private ShipInventoryDisplayObject primaryDisplay;
        private ShipInventoryDisplayObject primaryDisplay2;
        private ShipInventoryDisplayObject secondaryDisplay;
        //private ShipInventoryDisplayObject inventoryDisplay;

        public List<ShipInventoryDisplayObject> displayList;

        //private int inventoryPos = 5;
        
        //position variables
        private readonly int BACKGROUND_SHIP_LENGTH = 208;

        //Cursor-related variables
        private int layer;
        private CursorCoordinate cursorCoordLv1;
        public CursorCoordinate CursorCoordLv1 { get { return cursorCoordLv1; } set { cursorCoordLv1 = value; } }
        private int cursorLv2Pos;

        #endregion
        public ShipManagerCursor(Game1 Game, Sprite spriteSheet)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
        }

        public void Initialize()
        {
            int xSpacing = 70;
            int ySpacing = 140;
            int yOffset = ySpacing / 2;
            int xOffset = -50;
            int originYOffset = -30;
            Vector2 center = StaticFunctions.PointToVector2(ShipManagerState.GetUpperLeftRectangle.Center);
            Vector2 origin = new Vector2(center.X + xOffset - BACKGROUND_SHIP_LENGTH / 2f, center.Y + originYOffset);
            Vector2 backOrigin = new Vector2(origin.X / 2 - 20, origin.Y);

            displayList = new List<ShipInventoryDisplayObject>();

            Coordinate backCoord = new Coordinate(0, 0);
            displayList.Add(back = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(780, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(780, 901, 60, 60)),
                backOrigin, backCoord));

            Coordinate batteryCoord = new Coordinate(1, 0);
            displayList.Add(cellDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(600, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(600, 901, 60, 60)),
                new Vector2(origin.X, origin.Y), batteryCoord));

            Coordinate platingCoord = new Coordinate(2, 0);
            displayList.Add(platingDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(480, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(480, 901, 60, 60)),
                new Vector2(origin.X + xSpacing, origin.Y - yOffset), platingCoord));

            Coordinate shieldCoord = new Coordinate(2, 1);
            displayList.Add(shieldDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(540, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(540, 901, 60, 60)),
                new Vector2(origin.X + xSpacing, origin.Y - yOffset + ySpacing), shieldCoord));

            Coordinate secondaryCoord = new Coordinate(3, 0);
            displayList.Add(secondaryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(720, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(720, 901, 60, 60)),
                new Vector2(origin.X + xSpacing * 2, origin.Y), secondaryCoord));

            Coordinate primaryCoord1 = new Coordinate(4, 0);
            displayList.Add(primaryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(660, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(660, 901, 60, 60)),
                new Vector2(origin.X + xSpacing * 3, origin.Y - yOffset), primaryCoord1));

            Coordinate primaryCoord2 = new Coordinate(4, 1);
            displayList.Add(primaryDisplay2 = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(660, 840, 60, 60)),
                spriteSheet.GetSubSprite(new Rectangle(660, 901, 60, 60)),
                new Vector2(origin.X + xSpacing * 3, origin.Y - yOffset + ySpacing), primaryCoord2));
            
            //Coordinate inventoryCoord = new Coordinate(5, 0);
            //displayList.Add(inventoryDisplay = new ShipInventoryDisplayObject(Game, spriteSheet.GetSubSprite(new Rectangle(225, 15, 40, 40)),
            //    spriteSheet.GetSubSprite(new Rectangle(305, 15, 40, 40)),
            //    new Vector2(origin.X + xSpacing * 5, origin.Y), inventoryCoord));
        }

        public void Update(GameTime gameTime, int layer, CursorCoordinate cursorCoordLv1, int cursorLv2Pos)
        {
            this.layer = layer;
            this.cursorCoordLv1 = cursorCoordLv1;
            this.cursorLv2Pos = cursorLv2Pos;

            foreach (ShipInventoryDisplayObject menuDisplay in displayList)
            { 
                menuDisplay.UpdateActivity(cursorCoordLv1);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (ShipInventoryDisplayObject dispObj in displayList)
            {
                dispObj.Draw(spriteBatch);
            }

            if (back.isActive)
            {
                ShipManagerText.DisplayBackInfo(spriteBatch);
            }

            else if (primaryDisplay.isActive)
            {
                ShipManagerText.DisplayPrimaryWeaponInfo1(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.GetAvailablePrimaryWeapons(1).Count)
                {
                    InventoryInformation.DisplayPrimaryWeaponInfo(spriteBatch);
                }
            }

            else if (primaryDisplay2.isActive)
            {
                ShipManagerText.DisplayPrimaryWeaponInfo2(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.GetAvailablePrimaryWeapons(2).Count)
                {
                    InventoryInformation.DisplayPrimaryWeaponInfo(spriteBatch);
                }
            }

            else if (secondaryDisplay.isActive)
            {
                ShipManagerText.DisplaySecondaryInfo(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.OwnedSecondary.Count)
                {
                    InventoryInformation.DisplaySecondaryWeaponInfo(spriteBatch);
                }
            }

            else if (platingDisplay.isActive)
            {
                ShipManagerText.DisplayPlatingInfo(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.ownedPlatings.Count)
                {
                    InventoryInformation.DisplayPlatingInfo(spriteBatch);
                }
            }

            else if (cellDisplay.isActive)
            {
                ShipManagerText.DisplayEnergyCellInfo(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.ownedEnergyCells.Count)
                {
                    InventoryInformation.DisplayEnergyCellInfo(spriteBatch);
                }
            }

            else if (shieldDisplay.isActive)
            {
                ShipManagerText.DisplayShieldInfo(spriteBatch);

                if (layer >= 2 && cursorLv2Pos < ShipInventoryManager.ownedShields.Count)
                {
                    InventoryInformation.DisplayShieldInfo(spriteBatch);
                }
            }

            //if (inventoryDisplay.isActive && cursorLv3Pos != -1) // Why was this here to begin with? /Jakob 140615
            //if (inventoryDisplay.isActive)
            //{
            //    ShipManagerText.DisplayInventory(spriteBatch, Game.Resolution);
            //
            //    if (layer >= 2)
            //        InventoryInformation.DisplayInventoryInfo(spriteBatch);
            //}
        }
    }
}
