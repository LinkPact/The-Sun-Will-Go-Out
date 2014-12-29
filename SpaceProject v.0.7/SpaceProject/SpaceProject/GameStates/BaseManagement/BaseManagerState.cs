using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class BaseManagerState : GameState
    {
        #region initVariables

        //Technical
        private Sprite spriteSheet;
        private int elapsedTimeMilliseconds;
        private int elapsedSinceKey;
        private int elapseDelay;

        //private KeyboardState previousKeyboardState;
        //private KeyboardState currentKeyboardState;
        private Sprite menuSpriteSheet;

        //Visuell kuriosa
        private Sprite verticalLine;
        private Sprite horizontalLine;

        private BaseManagerCursor cursorManager;
        private BaseManagerText fontManager;
        private InventoryInformation informationManager;

        private const int BG_WIDTH = 92;
        private const int BG_HEIGHT = 92;

        //Cursor-related variables
        private int cursorLevel;
        private int cursorLevel1Position;
        private int cursorLevel2Position;
        private int cursorLevel3Position;

        private int column;
        private int section;
        private int startSection;
        private bool cursorSplit;

        private int invLeavePos;
        private int baseLeavePos;

        private int holdTimer;

        //Resource-related variables
        bool selectAmount;
        private float amountToMove;

        //Comparison-related variables
        private ItemComparison itemComp;
        private Item tempItem;

        #endregion

        //temp
        public MotherShip MotherShip;

        public BaseManagerState(Game1 Game, String name) :
            base(Game, name)
        {
            this.Game = Game;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/MenuSheet"));
        }

        public override void Initialize()
        {
            //Managers for cursor and text.
            cursorManager = new BaseManagerCursor(Game, spriteSheet);
            cursorManager.Initialize();
            fontManager = new BaseManagerText(Game);
            fontManager.Initialize();
            informationManager = new InventoryInformation(Game);
            informationManager.Initialize();

            //Data about current active user position.
            cursorLevel = 1;
            cursorLevel1Position = 0;
            cursorLevel2Position = 0;
            cursorLevel3Position = 0;
            column = 1;
            startSection = 0;
            section = 0;
            cursorSplit = false;

            elapsedSinceKey = 0;
            elapseDelay = 50;

            invLeavePos = 0;
            baseLeavePos = BaseInventoryManager.inventorySize / 2;

            itemComp = new ItemComparison(this.Game, this.spriteSheet, new Rectangle(386, 14, 9, 5),
                new Rectangle(386, 29, 9, 8), new Rectangle(386, 20, 9, 8));
        }
        public override void OnEnter()
        {
            menuSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/MenuSheet"));
            elapsedTimeMilliseconds = 0;

            //Kuriosa
            verticalLine = menuSpriteSheet.GetSubSprite(new Rectangle(365, 0, 1, 600));
            horizontalLine = menuSpriteSheet.GetSubSprite(new Rectangle(0, 189, 800, 1));

            holdTimer = Game.HoldKeyTreshold;
        }
        public override void OnLeave()
        {
        }
        public override void Update(GameTime gameTime)
        {
            //if (currentKeyboardState != null)
            //    previousKeyboardState = currentKeyboardState;
            //
            //currentKeyboardState = Keyboard.GetState();
            elapsedTimeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

            if (!selectAmount)
                CheckKeys(gameTime);

            else
                SelectAmountControls();

            if (cursorLevel == 2 && section == 0 && cursorLevel2Position > ShipInventoryManager.ShipItems.Count - 1)
                cursorLevel2Position = ShipInventoryManager.ShipItems.Count - 1;
            else if (cursorLevel == 3 && section == 0 && cursorLevel3Position > ShipInventoryManager.ShipItems.Count - 1)
                cursorLevel3Position = ShipInventoryManager.ShipItems.Count - 1;
            
            if(!selectAmount)
                cursorManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel2Position, cursorLevel3Position, section, cursorSplit);

            fontManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel2Position, cursorLevel3Position, section);
            informationManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel3Position, "BaseManagerState", section);

            elapsedSinceKey += gameTime.ElapsedGameTime.Milliseconds;

            itemComp.CompareStats();

            #region ItemComparison

            switch (cursorLevel)
            {
                case 1:
                    {
                        itemComp.OkayToClearStats = true;
                        itemComp.OkayToClearSymbols = true;
                        break;
                    }

                case 2:
                    {
                        switch (cursorLevel1Position)
                        {
                            case 0:
                                if (section == 0)
                                {
                                    if (!CheckForOutOfRange(cursorLevel, "ship"))
                                    {
                                        tempItem = ShipInventoryManager.ShipItems[cursorLevel2Position];
                                        itemComp.FindEquippedItem(ShipInventoryManager.ShipItems[cursorLevel2Position].Kind);
                                        itemComp.SetItem2(ShipInventoryManager.ShipItems[cursorLevel2Position]);
                                        itemComp.ShowSymbols = true;
                                    }
                                }

                                else
                                {
                                    if (!CheckForOutOfRange(cursorLevel, "base"))
                                    {
                                        tempItem = BaseInventoryManager.BaseItems[cursorLevel2Position];
                                        itemComp.FindEquippedItem(BaseInventoryManager.BaseItems[cursorLevel2Position].Kind);
                                        itemComp.SetItem2(BaseInventoryManager.BaseItems[cursorLevel2Position]);
                                        itemComp.ShowSymbols = true;
                                    }
                                }
                                break;

                            case 1:
                                tempItem = BaseInventoryManager.equippedShip;
                                break;
                        }

                        break;
                    }

                case 3:
                    {
                        switch (cursorLevel1Position)
                        {
                            case 0:
                                {

                                    if (section == 0)
                                    {
                                        if (!CheckForOutOfRange(cursorLevel, "ship"))
                                        {
                                            itemComp.SetItem1(tempItem);
                                            itemComp.SetItem2(ShipInventoryManager.ShipItems[cursorLevel3Position]);
                                            itemComp.ShowSymbols = true;
                                        }
                                    }

                                    else
                                    {
                                        if (!CheckForOutOfRange(cursorLevel, "base"))
                                        {
                                            itemComp.SetItem1(tempItem);
                                            itemComp.SetItem2(BaseInventoryManager.BaseItems[cursorLevel3Position]);
                                            itemComp.ShowSymbols = true;
                                        }
                                    }
                                }
                                break;

                            case 1:
                                itemComp.SetItem1(tempItem);
                                itemComp.SetItem2(BaseInventoryManager.ownedShips[cursorLevel3Position]);
                                itemComp.ShowSymbols = true;
                                break;
                        }
                        break;
                    }
            }



            #endregion
        }
        private bool CheckForOutOfRange(int cursorLevel, string listType)
        {
            if (cursorLevel == 2)
            {
                if (listType.ToLower() == "base")
                {
                    if (cursorLevel2Position < 0 || cursorLevel3Position > BaseInventoryManager.BaseItems.Count - 1)
                        return true;
                }

                else if (listType.ToLower() == "ship")
                {
                    if (cursorLevel2Position < 0 || cursorLevel3Position > ShipInventoryManager.ShipItems.Count - 1)
                        return true;
                }
            }

            else if (cursorLevel == 3)
            {
                if (listType == "base")
                {
                    if (cursorLevel3Position < 0 || cursorLevel3Position > BaseInventoryManager.BaseItems.Count - 1)
                        return true;
                }

                else if (listType == "ship")
                {
                    if (cursorLevel3Position < 0 || cursorLevel3Position > ShipInventoryManager.ShipItems.Count - 1)
                        return true;
                }
            }

            return false;
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.White);

            //Ritar bakgrundsobjekt (linjer)
            DrawBackground(spriteBatch);

            cursorManager.Draw(spriteBatch);
            fontManager.Draw(spriteBatch);
            informationManager.Draw(spriteBatch);

            //Game.Window.Title = "Layer: " + cursorLevel.ToString() + " Pos1: " + cursorLevel1Position.ToString() +
            //    " Pos2: " + cursorLevel2Position.ToString() + " Pos3: " + cursorLevel3Position.ToString() + " Section: " + section.ToString()
            //    + " Split " + cursorSplit.ToString() + " Amount to move: " + amountToMove;

            itemComp.Draw(spriteBatch, new Vector2(405, 496), 20); 
        }
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            //Backdrop
            for (int i = 0; i < (int)((Game.Window.ClientBounds.Width / BG_WIDTH) + 1); i++)
            {
                for (int j = 0; j < (int)((Game.Window.ClientBounds.Height / BG_HEIGHT) + 1); j++)
                    spriteBatch.Draw(spriteSheet.Texture, new Vector2(BG_WIDTH * i, BG_HEIGHT * j),
                    new Rectangle(0, 190, BG_WIDTH, BG_HEIGHT), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            //Bakgrundslinjer
            spriteBatch.Draw(verticalLine.Texture, new Vector2(400, 0), verticalLine.SourceRectangle, Color.Black);
            spriteBatch.Draw(horizontalLine.Texture, new Vector2(-400, 150), horizontalLine.SourceRectangle, Color.Black);
            spriteBatch.Draw(horizontalLine.Texture, new Vector2(400, 450), horizontalLine.SourceRectangle, Color.Black);
        }
        private void CheckKeys(GameTime gameTime)
        {
            CheckStateChangeCommands();

            if (cursorLevel == 1)
            {
                CheckCursorLevel1();
            }
            else if (cursorLevel == 2 || cursorLevel == 3)
            {
                CheckCursorLevel2(gameTime);
            }

            if (cursorLevel1Position == 0) ChangeInventory();

            if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && cursorLevel == 3)
                cursorSplit = false;
        }
        private void SelectAmountControls()
        {
            if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                if (cursorSplit && section == 0)
                {
                    if (BaseInventoryManager.BaseItems[cursorLevel2Position] is QuantityItem)
                    {
                        if (amountToMove < ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity)
                            amountToMove++;
                    }
                }

                else if (cursorSplit && section == 1)
                {
                    if (ShipInventoryManager.ShipItems[cursorLevel2Position] is QuantityItem)
                    {
                        if (amountToMove < ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity)
                            amountToMove++;
                    }
                }
            }

            else if (ControlManager.CheckHold(RebindableKeys.Down))
            { 
                if (amountToMove > 0)
                    amountToMove--;                       
            }

            if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && elapsedSinceKey > elapseDelay)
            {
                if (selectAmount && amountToMove > 0)
                {
                    if (cursorSplit && section == 0)
                        SwitchComponentsBetween(true);

                    else if (cursorSplit && section == 1)
                        SwitchComponentsBetween(false);

                    amountToMove = 0;
                    cursorLevel2Position = cursorLevel3Position;
                    selectAmount = false;
                }

                else
                {
                    selectAmount = false;
                    amountToMove = 0;
                    cursorLevel2Position = cursorLevel3Position;
                }
            }

            if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > elapseDelay)
            {
                selectAmount = false;
                amountToMove = 0;
                cursorLevel2Position = cursorLevel3Position;
            }

        }
        private void CheckStateChangeCommands()
        {
            //State-Changers
            if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > elapseDelay && cursorLevel == 1)
            {
                Game.stateManager.ChangeState(GameStateManager.previousState);
            }

            if (ControlManager.CheckKeypress(Keys.B) && elapsedTimeMilliseconds > 200 && elapsedSinceKey > elapseDelay)
            {
                Game.stateManager.ChangeState(GameStateManager.previousState);
            }
        }
        private void ChangeInventory()
        {
            #region switchWithAction3
            /*
            //Using actionkey3
            if (CheckPress("action3"))
            {
                if (cursorLevel == 2)
                {
                    if (section == 0)
                    {
                        if (!cursorSplit) startSection = 0;

                        section = 1;
                    }
                    else if (section == 1)
                    {
                        if (!cursorSplit) startSection = 1;

                        section = 0;

                        if (cursorLevel2Position > ShipInventoryManager.ShipItems.Count)
                        {
                            cursorLevel2Position = ShipInventoryManager.ShipItems.Count;
                        }
                    }

                    cursorSplit = false;
                }
                else if (cursorLevel == 3)
                {
                    if (section == 0)
                    {
                        if (!cursorSplit) startSection = 0;

                        section = 1;
                    }
                    else if (section == 1)
                    {
                        if (!cursorSplit) startSection = 1;

                        section = 0;

                        if (cursorLevel3Position > BaseInventoryManager.BaseItems.Count)
                        {
                            cursorLevel3Position = BaseInventoryManager.BaseItems.Count;
                        }
                    }

                    if (section != startSection)
                        cursorSplit = true;
                    else
                        cursorSplit = false;
                }
            }
            */
            #endregion

            //Using arrows
            if (ControlManager.CheckPress(RebindableKeys.Left) && section == 0 && elapsedSinceKey > elapseDelay)
            {
                if (cursorLevel == 2 && cursorLevel2Position < ShipInventoryManager.ColumnSize)
                {
                    if (!cursorSplit) startSection = 0;
                    section = 1;

                    invLeavePos = cursorLevel2Position;
                    cursorLevel2Position = baseLeavePos;

                    cursorSplit = false;

                    elapsedSinceKey = 0;
                }
                else if (cursorLevel == 3 && cursorLevel3Position < ShipInventoryManager.ColumnSize)
                {
                    if (!cursorSplit) startSection = 0;
                    section = 1;

                    invLeavePos = cursorLevel3Position;
                    cursorLevel3Position = baseLeavePos;

                    if (section != startSection)
                        cursorSplit = true;
                    else
                        cursorSplit = false;

                    elapsedSinceKey = 0;
                }
            }

            if (ControlManager.CheckPress(RebindableKeys.Right) && section == 1
                && elapsedSinceKey > elapseDelay)
            {
                if (cursorLevel == 2 && cursorLevel2Position >= BaseInventoryManager.ColumnSize)
                {
                    if (!cursorSplit) startSection = 1;

                    section = 0;
                    baseLeavePos = cursorLevel2Position;
                    cursorLevel2Position = invLeavePos;

                    cursorSplit = false;

                    elapsedSinceKey = 0;
                }
                else if (cursorLevel == 3 && cursorLevel3Position >= BaseInventoryManager.ColumnSize)
                {
                    if (!cursorSplit) startSection = 1;

                    section = 0;
                    baseLeavePos = cursorLevel3Position;
                    cursorLevel3Position = invLeavePos;

                    if (section != startSection)
                        cursorSplit = true;
                    else
                        cursorSplit = false;

                    elapsedSinceKey = 0;            
                }
            }
        }
        private void CheckCursorLevel1()
        {
            int temporaryCount = cursorManager.displayList.Count;
            if (ControlManager.CheckPress(RebindableKeys.Down) && cursorLevel == 1 && elapsedSinceKey > elapseDelay)
            {
                if (cursorLevel1Position == 0) { cursorLevel1Position = 1; }
                //else if (cursorLevel1Position == 2) { cursorLevel1Position = 3; }
                //else if (cursorLevel1Position == 4) { cursorLevel1Position = 5; }
            
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Up) && cursorLevel == 1 && elapsedSinceKey > elapseDelay)
            {
                if (cursorLevel1Position == 1) { cursorLevel1Position = 0; }
                //else if (cursorLevel1Position == 3) { cursorLevel1Position = 2; }
                //else if (cursorLevel1Position == 5) { cursorLevel1Position = 4; }
            
                elapsedSinceKey = 0;
            }

            if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && cursorLevel == 1 && elapsedSinceKey > elapseDelay)
            {
                cursorLevel = 2;
                cursorLevel2Position = 0;
                elapsedSinceKey = 0;
                section = 0;
            }
        }
        private void CheckCursorLevel2(GameTime gameTime)
        {
            if (cursorLevel == 2) BaseLevel2(gameTime);
            if (cursorLevel == 3) BaseLevel3(gameTime);
        }
        //private void ShipLevel2()
        //{
        //    //The hangar!
        //    if (cursorLevel == 2 && cursorLevel1Position == 1)
        //    {
        //        int listLength = 1;
        //
        //        int ownedListLength = ShipInventoryManager.ownedShips.Count;
        //
        //        if (CheckPress(Keys.Down) && cursorLevel == 2 && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel2Position += 1;
        //            if (cursorLevel2Position > listLength - 1)
        //                cursorLevel2Position -= listLength;
        //
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress(Keys.Up) && cursorLevel == 2 && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel2Position -= 1;
        //            if (cursorLevel2Position < 0)
        //                cursorLevel2Position += listLength;
        //
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress("action1") && cursorLevel == 2 && elapsedSinceKey > elapseDelay && ownedListLength > 0)
        //        {
        //            cursorLevel = 3;
        //            cursorLevel3Position = 0;
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress("action2") && cursorLevel == 2 && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel = 1;
        //            elapsedSinceKey = 0;
        //        }
        //    }
        //
        //    //The inventory!
        //    if (cursorLevel == 2 && cursorLevel1Position == 0)
        //    {
        //        int listLength;
        //        int firstHalf;
        //        int secondHalf;
        //        
        //        if (section == 0)
        //        {
        //            listLength = ShipInventoryManager.inventorySize;
        //            firstHalf = Convert.ToInt32(listLength / 2);
        //            secondHalf = listLength - firstHalf;
        //        }
        //        else
        //        {
        //            listLength = BaseInventoryManager.inventorySize;
        //            firstHalf = Convert.ToInt32(listLength / 2);
        //            secondHalf = listLength - firstHalf;
        //        }
        //
        //        if (CheckPress(Keys.Down) && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel2Position += 1;
        //
        //            if (cursorLevel2Position == firstHalf) { cursorLevel2Position -= firstHalf; }
        //            if (cursorLevel2Position == listLength) { cursorLevel2Position -= secondHalf; }
        //
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress(Keys.Up) && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel2Position -= 1;
        //            if (cursorLevel2Position == -1) { cursorLevel2Position += firstHalf; }
        //            if (cursorLevel2Position == firstHalf - 1 && column == 2) { cursorLevel2Position += secondHalf; }
        //
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress(Keys.Right) && elapsedSinceKey > elapseDelay)
        //        {
        //            if (cursorLevel2Position < firstHalf)
        //            {
        //                cursorLevel2Position += firstHalf;
        //                column = 2;
        //            }
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress(Keys.Left) && elapsedSinceKey > elapseDelay)
        //        {
        //            if (cursorLevel2Position >= firstHalf)
        //            {
        //                cursorLevel2Position -= firstHalf;
        //                column = 1;
        //            }
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress("action1") && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel = 3;
        //            cursorLevel3Position = cursorLevel2Position;
        //            elapsedSinceKey = 0;
        //        }
        //
        //        if (CheckPress("action2") && elapsedSinceKey > elapseDelay)
        //        {
        //            cursorLevel = 1;
        //            elapsedSinceKey = 0;
        //        }
        //    }
        //}
        private void BaseLevel2(GameTime gameTime)
        {
            if (cursorLevel1Position == 0)
            {
                #region inventorypart
                if (cursorLevel == 2)
                {
                    int listLength;
                    int firstHalf;
                    int secondHalf;

                    if (section == 0)
                    {
                        listLength = ShipInventoryManager.inventorySize;
                        firstHalf = ShipInventoryManager.ColumnSize;
                        secondHalf = listLength - firstHalf;

                        if (ShipInventoryManager.inventorySize > 14)
                        {
                            firstHalf = 14;
                            secondHalf = listLength - firstHalf;
                        }
                        else
                        {
                            firstHalf = listLength;
                            secondHalf = 0;
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Down)
                            && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel2Position += 1;

                            if (cursorLevel2Position == firstHalf && column == 1) { cursorLevel2Position -= firstHalf; }
                            if (cursorLevel2Position == listLength && column == 2) { cursorLevel2Position = firstHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.HoldKeyTreshold;
                        }

                        else if (ControlManager.CheckHold(RebindableKeys.Down))
                        {
                            holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                            if (holdTimer <= 0)
                            {
                                cursorLevel2Position += 1;

                                if (cursorLevel2Position == firstHalf && column == 1) cursorLevel2Position = firstHalf - 1;
                                if (cursorLevel2Position == listLength && column == 2) cursorLevel2Position = listLength - 1;

                                elapsedSinceKey = 0;

                                holdTimer = Game.ScrollSpeedFast;
                            }
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Up)
                            && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel2Position -= 1;

                            if (cursorLevel2Position == -1 && column == 1) { cursorLevel2Position += firstHalf; }
                            if (cursorLevel2Position == firstHalf - 1 && column == 2) { cursorLevel2Position += secondHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.HoldKeyTreshold;
                        }

                        else if (ControlManager.CheckHold(RebindableKeys.Up))
                        {
                            holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                            if (holdTimer < 0)
                            {
                                cursorLevel2Position -= 1;

                                if (cursorLevel2Position == -1 && column == 1) { cursorLevel2Position = 0; }
                                if (cursorLevel2Position == firstHalf - 1 && column == 2) { cursorLevel2Position = firstHalf; }

                                elapsedSinceKey = 0;

                                holdTimer = Game.ScrollSpeedFast;
                            }
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Right)
                            && elapsedSinceKey > elapseDelay)
                        {
                            if (cursorLevel2Position < firstHalf && secondHalf != 0)
                            {
                                if (cursorLevel2Position + firstHalf < listLength)
                                {
                                    cursorLevel2Position += firstHalf;
                                    column = 2;
                                }
                                else
                                {
                                    cursorLevel2Position = listLength - 1;
                                }
                                column = 2;
                            }
                            elapsedSinceKey = 0;
                        }

                        if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))
                            && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel = 3;
                            cursorLevel3Position = cursorLevel2Position;
                            elapsedSinceKey = 0;
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Action2)
                            && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel = 1;
                            elapsedSinceKey = 0;
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Left) && elapsedSinceKey > elapseDelay && cursorLevel2Position >= firstHalf)
                        {
                            cursorLevel2Position -= firstHalf;
                            column = 1;
                            elapsedSinceKey = 0;
                        }
                    }
                    else if (section == 1)
                    {
                        listLength = BaseInventoryManager.inventorySize;
                        firstHalf = BaseInventoryManager.ColumnSize;
                        secondHalf = listLength - firstHalf;

                        if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel2Position += 1;

                            if (cursorLevel2Position == firstHalf) { cursorLevel2Position -= firstHalf; }
                            if (cursorLevel2Position == listLength) { cursorLevel2Position -= secondHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.HoldKeyTreshold;
                        }

                        else if (ControlManager.CheckHold(RebindableKeys.Down))
                        {
                            holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                            if (holdTimer <= 0)
                            {
                                cursorLevel2Position += 1;

                                if (cursorLevel2Position == firstHalf) { cursorLevel2Position = firstHalf - 1; }
                                if (cursorLevel2Position == listLength) { cursorLevel2Position = listLength - 1; }

                                elapsedSinceKey = 0;

                                holdTimer = Game.ScrollSpeedFast;
                            }
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel2Position -= 1;
                            if (cursorLevel2Position == -1) { cursorLevel2Position += firstHalf; }
                            if (cursorLevel2Position == firstHalf - 1 && column == 2) { cursorLevel2Position += secondHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.HoldKeyTreshold;
                        }

                        else if (ControlManager.CheckHold(RebindableKeys.Up))
                        {
                            holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                            if (holdTimer <= 0)
                            {
                                cursorLevel2Position -= 1;
                                if (cursorLevel2Position == -1) { cursorLevel2Position = 0; }
                                if (cursorLevel2Position == firstHalf - 1 && column == 2) { cursorLevel2Position = firstHalf; }

                                elapsedSinceKey = 0;

                                holdTimer = Game.ScrollSpeedFast;
                            }
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Right) && elapsedSinceKey > elapseDelay && cursorLevel2Position < firstHalf)
                        {
                            cursorLevel2Position += firstHalf;
                            column = 2;
                            elapsedSinceKey = 0;
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Left) && elapsedSinceKey > elapseDelay && cursorLevel2Position >= firstHalf)
                        {
                            cursorLevel2Position -= firstHalf;
                            column = 1;
                            elapsedSinceKey = 0;
                        }

                        if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel = 3;
                            cursorLevel3Position = cursorLevel2Position;
                            elapsedSinceKey = 0;
                        }

                        if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > elapseDelay)
                        {
                            cursorLevel = 1;
                            elapsedSinceKey = 0;
                        }

                    }
                }
                #endregion
            }
            else if (cursorLevel1Position == 1)
            {
                #region hangarpart

                int listLength = ShipInventoryManager.equipCounts[cursorLevel1Position];

                int ownedListLength = ShipInventoryManager.ownCounts[cursorLevel1Position];

                if (ControlManager.CheckPress(RebindableKeys.Down) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel2Position += 1;
                    if (cursorLevel2Position > listLength - 1)
                        cursorLevel2Position -= listLength;

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel2Position -= 1;
                    if (cursorLevel2Position < 0)
                        cursorLevel2Position += listLength;

                    elapsedSinceKey = 0;
                }

                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))
                    && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay &&
                    ownedListLength > 0)
                {
                    cursorLevel = 3;
                    cursorLevel3Position = 0;
                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }

                #endregion
            }
            
        }
        private void BaseLevel3(GameTime gameTime)
        {
            #region inventory
            //Inventory/baseinventory!
            if (cursorLevel == 3 && cursorLevel1Position == 0)
            {
                int listLength;
                int firstHalf;
                int secondHalf;

                if (section == 0)
                {
                    listLength = ShipInventoryManager.inventorySize;
                    firstHalf = ShipInventoryManager.ColumnSize;
                    secondHalf = listLength - firstHalf;

                    if (ShipInventoryManager.inventorySize > 14)
                    {
                        firstHalf = 14;
                        secondHalf = listLength - firstHalf;
                    }
                    else
                    {
                        firstHalf = listLength;
                        secondHalf = 0;
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > elapseDelay)
                    {
                        cursorLevel3Position += 1;

                        if (cursorLevel3Position == firstHalf && column == 1) { cursorLevel3Position -= firstHalf + 1; }
                        if (cursorLevel3Position == listLength && column == 2) { cursorLevel3Position = firstHalf; }

                        elapsedSinceKey = 0;

                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Down))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            cursorLevel3Position += 1;

                            if (cursorLevel3Position == firstHalf && column == 1) { cursorLevel3Position = firstHalf - 1; }
                            if (cursorLevel3Position == listLength && column == 2) { cursorLevel3Position = listLength - 1; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > elapseDelay)
                    {
                        cursorLevel3Position -= 1;

                        if (cursorLevel3Position == -2 && column == 1) { cursorLevel3Position += firstHalf + 1; }
                        if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position += secondHalf; }

                        elapsedSinceKey = 0;

                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Up))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            cursorLevel3Position -= 1;

                            if (cursorLevel3Position == -1 && column == 1) { cursorLevel3Position = 0; }
                            if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position = firstHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Right) && elapsedSinceKey > elapseDelay 
                        && cursorLevel3Position < firstHalf && secondHalf != 0)
                    {
                        if (cursorLevel3Position + firstHalf < listLength)
                        {
                            cursorLevel3Position += firstHalf;
                            column = 2;
                        }
                        else cursorLevel3Position = listLength - 1;

                        column = 2;
                        elapsedSinceKey = 0;
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Left) && elapsedSinceKey > elapseDelay && cursorLevel3Position >= firstHalf)
                    {
                        cursorLevel3Position -= firstHalf;
                        column = 1;
                        elapsedSinceKey = 0;
                    }
                }
            #endregion
                #region base
                else if (section == 1 && cursorLevel1Position == 0)
                {
                    listLength = BaseInventoryManager.inventorySize;
                    firstHalf = BaseInventoryManager.ColumnSize;
                    secondHalf = listLength - firstHalf;

                    if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > elapseDelay)
                    {
                        cursorLevel3Position += 1;

                        if (cursorLevel3Position == firstHalf) { cursorLevel3Position -= firstHalf + 1; }
                        if (cursorLevel3Position == listLength) { cursorLevel3Position -= secondHalf; }

                        elapsedSinceKey = 0;

                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Down))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            cursorLevel3Position += 1;

                            if (cursorLevel3Position == firstHalf) { cursorLevel3Position = firstHalf - 1; }
                            if (cursorLevel3Position == listLength) { cursorLevel3Position = listLength - 1; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > elapseDelay)
                    {
                        cursorLevel3Position -= 1;

                        if (cursorLevel3Position == -2) { cursorLevel3Position += firstHalf + 1; }
                        if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position += secondHalf; }

                        elapsedSinceKey = 0;

                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Up))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            cursorLevel3Position -= 1;

                            if (cursorLevel3Position == -1) { cursorLevel3Position = 0; }
                            if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position = firstHalf; }

                            elapsedSinceKey = 0;

                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Right) && elapsedSinceKey > elapseDelay && cursorLevel3Position < firstHalf)
                    {
                        cursorLevel3Position += firstHalf;
                        column = 2;
                        elapsedSinceKey = 0;
                    }

                    if (ControlManager.CheckPress(RebindableKeys.Left) && elapsedSinceKey > elapseDelay && cursorLevel3Position >= firstHalf)
                    {
                        cursorLevel3Position -= firstHalf;
                        column = 1;
                        elapsedSinceKey = 0;
                    }
                }
                #endregion
                #region magichappens
                //This is where the magic happens.
                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && elapsedSinceKey > elapseDelay)
                {
                    //This is the command for equipping an owned weapon.
                    if (cursorLevel3Position != -1 && section == 0 && !cursorSplit)
                    {
                        SwitchComponentsInventory();
                        cursorLevel2Position = cursorLevel3Position;
                    }
                    else if (cursorLevel3Position != -1 && section == 0 && cursorSplit)
                    {
                        if (BaseInventoryManager.BaseItems[cursorLevel2Position] is QuantityItem)
                        {
                            selectAmount = true;
                        }

                        else
                        {
                            SwitchComponentsBetween(true);
                            cursorLevel2Position = cursorLevel3Position;
                        }
                    }
                    else if (cursorLevel3Position != -1 && section == 1 && !cursorSplit)
                    {
                        SwitchComponentsBase();
                        cursorLevel2Position = cursorLevel3Position;
                    }
                    else if (cursorLevel3Position != -1 && section == 1 && cursorSplit)
                    {
                        if (ShipInventoryManager.ShipItems[cursorLevel2Position] is QuantityItem)
                        {
                            selectAmount = true;
                        }

                        else
                        {
                            SwitchComponentsBetween(false);
                            cursorLevel2Position = cursorLevel3Position;
                        }
                    }
                    else if (cursorLevel3Position == -1 && section == 0 && !cursorSplit)
                    {
                        EraseComponentInventory();
                    }
                    else if (cursorLevel3Position == -1 && section == 1 && !cursorSplit)
                    {
                        EraseComponentBase();
                    }
                    else if (cursorLevel3Position == -1 && section == 0 && cursorSplit)
                    {
                        EraseComponentBase();
                    }
                    else if (cursorLevel3Position == -1 && section == 1 && cursorSplit)
                    {
                        EraseComponentInventory();
                    }

                    cursorLevel = 2;
                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel = 2;
                    elapsedSinceKey = 0;
                }
                #endregion
            }

            #region Hangar
            if (cursorLevel1Position == 1)
            {
                int listLength = BaseInventoryManager.ownedShips.Count;

                if (ControlManager.CheckPress(RebindableKeys.Down) && cursorLevel == 3 && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel3Position += 1;
                    if (cursorLevel3Position > listLength - 1)
                        cursorLevel3Position -= listLength;

                    elapsedSinceKey = 0;

                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Down))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorLevel3Position += 1;
                        if (cursorLevel3Position > listLength - 1)
                            cursorLevel3Position = listLength - 1;

                        elapsedSinceKey = 0;

                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && cursorLevel == 3 && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel3Position -= 1;
                    if (cursorLevel3Position < 0)
                        cursorLevel3Position += listLength;

                    elapsedSinceKey = 0;

                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Up))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorLevel3Position -= 1;
                        if (cursorLevel3Position < 0)
                            cursorLevel3Position = 0;

                        elapsedSinceKey = 0;

                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                //This is where the magic happens.
                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && cursorLevel == 3 && elapsedSinceKey > elapseDelay)
                {
                    //This is the command for changing equipped ship.
                    BaseInventoryManager.ChangeShip(cursorLevel3Position);

                    cursorLevel = 2;
                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && cursorLevel == 3 && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel = 2;
                    elapsedSinceKey = 0;
                }
            }
            #endregion

        }        
        private void SwitchComponentsInventory()
        {
            //Check if resource
            if (ShipInventoryManager.ShipItems[cursorLevel3Position].Kind == ShipInventoryManager.ShipItems[cursorLevel2Position].Kind &&
                ShipInventoryManager.ShipItems[cursorLevel3Position].Name == ShipInventoryManager.ShipItems[cursorLevel2Position].Name &&
                ShipInventoryManager.ShipItems[cursorLevel3Position] != ShipInventoryManager.ShipItems[cursorLevel2Position])
            {
                ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel3Position]).Quantity += ((QuantityItem)
                    ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity;

                EraseComponentInventory();

                ShipInventoryManager.CheckQuantityCount(this.Game);
            }

            else
                ShipInventoryManager.SwitchItems(cursorLevel2Position, cursorLevel3Position);
        }
        private void SwitchComponentsBase()
        {
            //Check if resource
            if (BaseInventoryManager.BaseItems[cursorLevel3Position].Kind == BaseInventoryManager.BaseItems[cursorLevel2Position].Kind &&
                BaseInventoryManager.BaseItems[cursorLevel3Position].Name == BaseInventoryManager.BaseItems[cursorLevel2Position].Name &&
                BaseInventoryManager.BaseItems[cursorLevel3Position] != BaseInventoryManager.BaseItems[cursorLevel2Position])
            {
                ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel3Position]).Quantity += ((QuantityItem)
                    BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity;

                EraseComponentBase();

                BaseInventoryManager.CheckQuantityCount(this.Game);
            }

            else
                BaseInventoryManager.SwitchItems(cursorLevel2Position, cursorLevel3Position);
        }
        private void SwitchComponentsBetween(bool toInventory)
        {
            #region To inventory
            if (toInventory)
            {
                //Resource
                if (ShipInventoryManager.ShipItems[cursorLevel3Position] is QuantityItem)
                {
                    QuantityItem invQuant = (QuantityItem)ShipInventoryManager.ShipItems[cursorLevel3Position];
                    QuantityItem baseQuant;
                    Item baseItem;

                    if (BaseInventoryManager.BaseItems[cursorLevel2Position] is QuantityItem)
                    {
                        baseQuant = (QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position];

                        if (baseQuant.Name == invQuant.Name)
                        {
                            if (amountToMove == ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity)
                            {
                                BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
                                ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel3Position]).Quantity += baseQuant.Quantity;
                            }

                            else
                            {
                                ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel3Position]).Quantity += amountToMove;
                                ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity -= amountToMove;
                            }
                        }

                        else
                        {
                            if (amountToMove == ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity)
                            {
                                ShipInventoryManager.RemoveItemAt(cursorLevel3Position);
                                ShipInventoryManager.InsertItem(cursorLevel3Position, baseQuant);
                                BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
                                BaseInventoryManager.InsertItem(cursorLevel2Position, invQuant);
                            }

                            else
                            {
                                QuantityItem tempQuantityItem = null;

                                switch (baseQuant.Name.ToLower())
                                { 
                                    case "copper":
                                        tempQuantityItem = new CopperResource(this.Game, amountToMove);
                                        break;

                                    case "gold":
                                        tempQuantityItem = new GoldResource(this.Game, amountToMove);
                                        break;

                                    case "titanium":
                                        tempQuantityItem = new TitaniumResource(this.Game, amountToMove);
                                        break;

                                    case "fine whiskey":
                                        tempQuantityItem = new FineWhiskey(this.Game, amountToMove);
                                        break;

                                }

                                ShipInventoryManager.InsertItem(cursorLevel3Position, tempQuantityItem);
                                ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity -= amountToMove;
                            }
                        }

                    }

                    else
                    {
                        baseItem = BaseInventoryManager.BaseItems[cursorLevel2Position];

                        ShipInventoryManager.RemoveItemAt(cursorLevel3Position);
                        ShipInventoryManager.InsertItem(cursorLevel3Position, baseItem);
                        BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
                        BaseInventoryManager.InsertItem(cursorLevel2Position, invQuant);
                    }

                    BaseInventoryManager.CheckQuantityCount(this.Game);
                    ShipInventoryManager.CheckQuantityCount(this.Game);
                }

                else if (BaseInventoryManager.BaseItems[cursorLevel2Position] is QuantityItem)
                {
                    QuantityItem baseQuant = ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]);
                    QuantityItem tempQuant = null;


                    switch (baseQuant.Name.ToLower())
                    {
                        case "copper":
                            tempQuant = new CopperResource(this.Game, amountToMove);
                            break;

                        case "gold":
                            tempQuant = new GoldResource(this.Game, amountToMove);
                            break;

                        case "titanium":
                            tempQuant = new TitaniumResource(this.Game, amountToMove);
                            break;

                        case "fine whiskey":
                            tempQuant = new FineWhiskey(this.Game, amountToMove);
                            break;

                    }
                    if (amountToMove == baseQuant.Quantity)
                    {
                        ShipInventoryManager.InsertItem(cursorLevel3Position, tempQuant);
                        EraseComponentBase();
                    }

                    else
                    {

                        if (ShipInventoryManager.ShipItems[cursorLevel3Position].Kind != "Empty")
                            ShipInventoryManager.InsertItem(cursorLevel3Position, tempQuant);
                        else
                        {
                            ShipInventoryManager.RemoveItemAt(cursorLevel3Position);
                            ShipInventoryManager.InsertItem(cursorLevel3Position, tempQuant);
                        }

                        ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel2Position]).Quantity -= amountToMove;
                    }
                }

                //Normal
                else if (ShipInventoryManager.ShipItems[cursorLevel3Position] != ShipInventoryManager.equippedEnergyCell)
                { 
                    Item invItem = ShipInventoryManager.ShipItems[cursorLevel3Position];
                    Item baseItem = BaseInventoryManager.BaseItems[cursorLevel2Position];

                    ShipInventoryManager.RemoveItemAt(cursorLevel3Position);
                    ShipInventoryManager.InsertItem(cursorLevel3Position, baseItem);
                    BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
                    BaseInventoryManager.InsertItem(cursorLevel2Position, invItem);
                }
            }
            #endregion

            #region From inventory
            if (!toInventory)
            {
                //Resource
                if (ShipInventoryManager.ShipItems[cursorLevel2Position] is QuantityItem)
                {
                    QuantityItem invQuant = (QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position];
                    QuantityItem baseQuant;
                    Item baseItem;

                    if (BaseInventoryManager.BaseItems[cursorLevel3Position] is QuantityItem)
                    {
                        baseQuant = (QuantityItem)BaseInventoryManager.BaseItems[cursorLevel3Position];

                        if (baseQuant.Name.ToLower() == invQuant.Name.ToLower())
                        {
                            if (amountToMove == invQuant.Quantity)
                            {
                                ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                                ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel3Position]).Quantity += invQuant.Quantity;
                            }

                            else
                            {
                                ((QuantityItem)BaseInventoryManager.BaseItems[cursorLevel3Position]).Quantity += amountToMove;
                                ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity -= amountToMove;
                            }                                                       
                        }

                        else
                        {
                            if (amountToMove == ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity)
                            {
                                ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                                ShipInventoryManager.InsertItem(cursorLevel2Position, baseQuant);
                                BaseInventoryManager.RemoveItemAt(cursorLevel3Position);
                                BaseInventoryManager.InsertItem(cursorLevel3Position, invQuant);
                            }

                            else
                            {
                                QuantityItem tempQuant = null;

                                switch (invQuant.Name.ToLower())
                                {
                                    case "copper":
                                        tempQuant = new CopperResource(this.Game, amountToMove);
                                        break;

                                    case "gold":
                                        tempQuant = new GoldResource(this.Game, amountToMove);
                                        break;

                                    case "titanium":
                                        tempQuant = new TitaniumResource(this.Game, amountToMove);
                                        break;

                                    case "fine whiskey":
                                        tempQuant = new FineWhiskey(this.Game, amountToMove);
                                        break;

                                }

                                BaseInventoryManager.InsertItem(cursorLevel3Position, tempQuant);
                                ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity -= amountToMove;
                            }
                        }

                    }

                    else
                    {
                        if (amountToMove == ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity)
                        {
                            baseItem = BaseInventoryManager.BaseItems[cursorLevel3Position];

                            ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                            ShipInventoryManager.InsertItem(cursorLevel2Position, baseItem);
                            BaseInventoryManager.RemoveItemAt(cursorLevel3Position);
                            BaseInventoryManager.InsertItem(cursorLevel3Position, invQuant);
                        }

                        else
                        {
                            QuantityItem tempQuant = null;

                            switch (invQuant.Name.ToLower())
                            {
                                case "copper":
                                    tempQuant = new CopperResource(this.Game, amountToMove);
                                    break;

                                case "gold":
                                    tempQuant = new GoldResource(this.Game, amountToMove);
                                    break;

                                case "titanium":
                                    tempQuant = new TitaniumResource(this.Game, amountToMove);
                                    break;

                                case "fine whiskey":
                                    tempQuant = new FineWhiskey(this.Game, amountToMove);
                                    break;

                            }

                            BaseInventoryManager.InsertItem(cursorLevel3Position, tempQuant);
                            ((QuantityItem)ShipInventoryManager.ShipItems[cursorLevel2Position]).Quantity -= amountToMove;
                        }
                    }

                    BaseInventoryManager.CheckQuantityCount(this.Game);
                    ShipInventoryManager.CheckQuantityCount(this.Game);
                }

                //Normal
                else if (ShipInventoryManager.ShipItems[cursorLevel2Position] != ShipInventoryManager.equippedEnergyCell)
                {
                    Item invItem = ShipInventoryManager.ShipItems[cursorLevel2Position];
                    Item baseItem = BaseInventoryManager.BaseItems[cursorLevel3Position];

                    ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                    ShipInventoryManager.InsertItem(cursorLevel2Position, baseItem);
                    BaseInventoryManager.RemoveItemAt(cursorLevel3Position);
                    BaseInventoryManager.InsertItem(cursorLevel3Position, invItem);
                }
            }
            #endregion
        }
        private void EraseComponentInventory()
        {
            Item erasedItem = new EmptyItem(Game);

            if (ShipInventoryManager.ShipItems[cursorLevel2Position] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[cursorLevel2Position] != BaseInventoryManager.equippedShip)
            {
                ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                ShipInventoryManager.InsertItem(cursorLevel2Position, erasedItem);
            }
        }
        private void EraseComponentBase()
        {
            Item empty = new EmptyItem(Game);

            BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
            BaseInventoryManager.InsertItem(cursorLevel2Position, empty);
        }
        //private bool CheckPress(Keys key)
        //{ 
        //    if (currentKeyboardState.IsKeyDown(key) && previousKeyboardState.IsKeyUp(key))
        //        return true;
        //    else
        //        return false;
        //}
        //Right now only functioning for the inventory
        private void SwitchComponents()
        {
            if (cursorLevel1Position == 0)
            {
                if (section == 0) ShipInventoryManager.SwitchItems(cursorLevel2Position, cursorLevel3Position);
                else if (section == 1) BaseInventoryManager.SwitchItems(cursorLevel2Position, cursorLevel3Position);
            }
        }
        private void EraseComponent()
        {
            if (cursorLevel1Position == 0)
            {
                if (section == 0)
                {
                    Item erasedItem = new EmptyItem(Game);

                    if (ShipInventoryManager.ShipItems[cursorLevel2Position] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[cursorLevel2Position] != BaseInventoryManager.equippedShip)
                    {
                        ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
                        ShipInventoryManager.InsertItem(cursorLevel2Position, erasedItem);
                    }
                }
                else if (section == 1)
                {
                    Item erasedItem = new EmptyItem(Game);

                    BaseInventoryManager.RemoveItemAt(cursorLevel2Position);
                    BaseInventoryManager.InsertItem(cursorLevel2Position, erasedItem);
                }
            }
        }
    }
}
