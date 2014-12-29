using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class ShipManagerState : GameState
    {
        #region initVariables
        
        //Technical
        private Sprite spriteSheet;
        private int elapsedTimeMilliseconds;
        private int elapsedSinceKey;
        private int elapseDelay;

        //Visuell kuriosa
        private Sprite background;
        private Sprite ship;

        private ShipManagerCursor cursorManager;
        private ShipManagerText fontManager;
        private InventoryInformation informationManager;

        private const int BG_WIDTH = 92;
        private const int BG_HEIGHT = 92;

        private int inventoryPos = -1;
        private int backPos = -2;

        private static Rectangle upperLeftRectangle;
        private static Rectangle lowerLeftRectangle;
        private static Rectangle rightRectangle;

        public static Rectangle GetUpperLeftRectangle { get { return upperLeftRectangle; } private set { ;} }
        public static Rectangle GetLowerLeftRectangle { get { return lowerLeftRectangle; } private set { ;} }
        public static Rectangle GetUpperRightRectangle { get { return rightRectangle; } private set { ;} }
        public static Rectangle GetLowerRightRectangle { get { return lowerLeftRectangle; } private set { ;} }

        //Cursor-related variables
        private int cursorLevel;
        private CursorCoordinate cursorCoordLv1;
        private int cursorLevel2Position;

        private int column;

        private int holdTimer;

        //Comparison-related variables
        private ItemComparison itemComp;
        private Item tempItem;

        #endregion

        public ShipManagerState(Game1 Game, String name) :
            base(Game, name)
        {
            this.Game = Game;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/MenuSheet"));
        }

        public override void Initialize() 
        {
            background = spriteSheet.GetSubSprite(new Rectangle(0, 0, 1024, 768));
            ship = spriteSheet.GetSubSprite(new Rectangle(0, 771, 212, 185));

            upperLeftRectangle = new Rectangle(0, 0,
                (int)Game.Window.ClientBounds.Width / 2, (int)Game.Window.ClientBounds.Height / 2);

            lowerLeftRectangle = new Rectangle(0, upperLeftRectangle.Height,
                upperLeftRectangle.Width, upperLeftRectangle.Height);

            rightRectangle = new Rectangle(lowerLeftRectangle.Width, 0,
                lowerLeftRectangle.Width, (int)(Game.Window.ClientBounds.Height));


            //Managers for cursor and text.
            cursorManager = new ShipManagerCursor(Game, spriteSheet);
            cursorManager.Initialize();
            fontManager = new ShipManagerText(Game);
            fontManager.Initialize();
            informationManager = new InventoryInformation(Game);
            informationManager.Initialize();

            //Data about current active user position.
            cursorLevel = 1;
            //cursorLevel1Position = 0;
            cursorCoordLv1 = new CursorCoordinate(0, 0, 4, 2, true);
            cursorLevel2Position = 0;
            column = 1;

            elapsedSinceKey = 0;
            elapseDelay = 50;

            itemComp = new ItemComparison(this.Game, this.spriteSheet, new Rectangle(386, 14, 9, 5),
                new Rectangle(386, 29, 9, 8), new Rectangle(386, 20, 9, 8));
        }
        
        public override void OnEnter()
        {
            elapsedTimeMilliseconds = 0;

            holdTimer = Game.HoldKeyTreshold;
        }

        public override void OnLeave() 
        { }

        public override void Update(GameTime gameTime)
        {
            if (StatsManager.gameMode == GameMode.develop)
                DeveloperOptions();
            
            elapsedTimeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

            CheckKeys(gameTime);

            cursorManager.Update(gameTime, cursorLevel, cursorCoordLv1, cursorLevel2Position);
            fontManager.Update(gameTime, cursorLevel, cursorCoordLv1.ToInt(), cursorLevel2Position);
            informationManager.Update(gameTime, cursorLevel, cursorCoordLv1.ToInt(), cursorLevel2Position, "ShipManagerState", -1);

            elapsedSinceKey += gameTime.ElapsedGameTime.Milliseconds;

            itemComp.CompareStats();

            ItemComparision();
        }

        private void ItemComparision()
        {
            //switch (cursorLevel)
            //{
            //    case 1:
            //        {
            //            itemComp.OkayToClearStats = true;
            //            itemComp.OkayToClearSymbols = true;
            //            break;
            //        }
            //
            //    case 3:
            //        {
            //            switch (cursorCoordLv1.ToInt())
            //            {
            //                case 0:
            //                    itemComp.SetItem2(ShipInventoryManager.OwnedPrimaryWeapons[cursorLevel3Position]);
            //                    itemComp.SetItem1(tempItem);
            //                    itemComp.ShowSymbols = true;
            //                    break;
            //
            //                case 1:
            //                    itemComp.SetItem2(ShipInventoryManager.OwnedSecondary[cursorLevel3Position]);
            //                    itemComp.SetItem1(tempItem);
            //                    itemComp.ShowSymbols = true;
            //                    break;
            //
            //                case 2:
            //                    itemComp.SetItem2(ShipInventoryManager.OwnedPlatings[cursorLevel3Position]);
            //                    itemComp.SetItem1(tempItem);
            //                    itemComp.ShowSymbols = true;
            //                    break;
            //
            //                case 3:
            //                    itemComp.SetItem2(ShipInventoryManager.ownedEnergyCells[cursorLevel3Position]);
            //                    itemComp.SetItem1(tempItem);
            //                    itemComp.ShowSymbols = true;
            //                    break;
            //
            //                case 4:
            //                    itemComp.SetItem2(ShipInventoryManager.ownedShields[cursorLevel3Position]);
            //                    itemComp.SetItem1(tempItem);
            //                    itemComp.ShowSymbols = true;
            //                    break;
            //
            //                case 5:
            //
            //                    if (!CheckForOutOfRange())
            //                    {
            //                        if (ShipInventoryManager.ShipItems[cursorLevel3Position].Kind != "Empty")
            //                        {
            //                            if (!CheckForOutOfRange())
            //                                itemComp.SetItem2(ShipInventoryManager.ShipItems[cursorLevel3Position]);
            //
            //                            itemComp.SetItem1(tempItem);
            //
            //                            itemComp.ShowSymbols = true;
            //                        }
            //
            //                        else
            //                            itemComp.ShowSymbols = false;
            //                    }
            //                    break;
            //            }
            //            break;
            //        }
            //}
        }

        private void DeveloperOptions()
        {
            if (ControlManager.CheckKeypress(Keys.D1))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_1");
            }

            if (ControlManager.CheckKeypress(Keys.D2))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_2");
            }

            if (ControlManager.CheckKeypress(Keys.D3))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_3");
            }

            if (ControlManager.CheckKeypress(Keys.D0))
            {
                Game.stateManager.shooterState.BeginLevel("JakobDevelop");
            }

            if (ControlManager.CheckKeypress(Keys.D9))
            {
                Game.stateManager.shooterState.BeginLevel("mapCreator2");
            }

            //if (ControlManager.CurrentKeyboardState.IsKeyDown(Keys.Delete) && !ShipInventoryManager.isCheatActivated)
            //{
            //    ShipInventoryManager.ActivateCheat();
            //}

            if (ControlManager.CheckKeypress(Keys.NumPad0))
            {
                ShipInventoryManager.ActivateCheatPrimary();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad1))
            {
                ShipInventoryManager.ActivateCheatSecondary();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad2))
            {
                ShipInventoryManager.ActivateCheatEnergy();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad3))
            {
                ShipInventoryManager.ActivateCheatShield();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad4))
            {
                ShipInventoryManager.ActivateCheatPlating();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad5))
            {
                ShipInventoryManager.ActivateCheatEquip1();
            }

            if (ControlManager.CheckKeypress(Keys.NumPad6))
            {
                ShipInventoryManager.ActivateCheatEquip2();
            }

        }
        
        private bool CheckForOutOfRange()
        {
            if (cursorLevel2Position < 0 || cursorLevel2Position > ShipInventoryManager.ShipItems.Count - 1)
                return true;

            return false;
        }
        
        private void CheckKeys(GameTime gameTime)
        {
            CheckStateChangeCommands();

            if (cursorLevel == 1)
                CheckCursorLevel1();
            else if (cursorLevel == 2)
                CheckCursorLevel2(gameTime);
        }     
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.White);

            //Ritar bakgrundsobjekt
            DrawBackground(spriteBatch);

            cursorManager.Draw(spriteBatch);
            fontManager.Draw(spriteBatch);
            informationManager.Draw(spriteBatch);

            if (Game.Window.ClientBounds.Height.Equals(600))
            {
                itemComp.Draw(spriteBatch, new Vector2(7, 247), 15); 
            }

            else if (Game.Window.ClientBounds.Height.Equals(768))
            {
                itemComp.Draw(spriteBatch, new Vector2(7, 303), 15); 
            }

            else if (Game.Window.ClientBounds.Height.Equals(576))
            {
                itemComp.Draw(spriteBatch, new Vector2(7, 239), 15); 
            }

            else if (Game.Window.ClientBounds.Height.Equals(720))
            {
                itemComp.Draw(spriteBatch, new Vector2(7, 287), 15); 
            }
        }
        
        private void DrawBackground(SpriteBatch spriteBatch)
        {
            //Backdrop
            spriteBatch.Draw(background.Texture, Vector2.Zero, background.SourceRectangle, Color.White, 0f, Vector2.Zero,
                new Vector2(Game.Resolution.X / background.Width, Game.Resolution.Y / background.Height), SpriteEffects.None,
                0.0f);

            // Ship
            spriteBatch.Draw(ship.Texture, StaticFunctions.PointToVector2(upperLeftRectangle.Center),
                ship.SourceRectangle, Color.White, 0f, ship.CenterPoint,
                1f, SpriteEffects.None, 0.0f);

            cursorCoordLv1.Draw(spriteBatch);
        }
        
        private void CheckCursorLevel1()
        {
            int temporaryCount = cursorManager.displayList.Count;
            if (ControlManager.CheckPress(RebindableKeys.Down) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay)
            {
                cursorCoordLv1.MoveCursor(0, 1);                
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Up) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay)
            {
                cursorCoordLv1.MoveCursor(0, -1);
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Left) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay)
            {
                cursorCoordLv1.MoveCursor(-1, 0);                
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Right) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay)
            {
                cursorCoordLv1.MoveCursor(1, 0);
                elapsedSinceKey = 0;
            }

            if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay))
            {
                if (cursorCoordLv1.Position != backPos
                    && ShipInventoryManager.ownCounts[cursorCoordLv1.Position] > 0)
                {
                    cursorLevel = 2;
                    cursorLevel2Position = 0;
                    elapsedSinceKey = 0;
                }
                else
                {
                    BackToOverworldLogic();  
                }
            }
        }

        private void BackToOverworldLogic()
        {
            Game.stateManager.ChangeState(GameStateManager.previousState);
        }

        private void CheckCursorLevel2(GameTime gameTime)
        {
            if (cursorLevel == 2 && cursorCoordLv1.Position != inventoryPos)
            {
                int listLength = ShipInventoryManager.ownCounts[cursorCoordLv1.Position];

                if (ControlManager.CheckPress(RebindableKeys.Down) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel2Position += 1;
                    if (cursorLevel2Position > listLength - 1)
                        cursorLevel2Position -= listLength;

                    elapsedSinceKey = 0;

                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Down))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorLevel2Position += 1;
                        if (cursorLevel2Position > listLength - 1)
                            cursorLevel2Position = listLength - 1;

                        elapsedSinceKey = 0;

                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel2Position -= 1;
                    if (cursorLevel2Position < 0)
                        cursorLevel2Position += listLength;

                    elapsedSinceKey = 0;

                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Up))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        cursorLevel2Position -= 1;
                        if (cursorLevel2Position < 0)
                            cursorLevel2Position = 0;

                        elapsedSinceKey = 0;

                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                //This is where the magic happens.
                if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay))
                {
                    //This is the command for equipping an owned weapon.
                    EquipComponent();

                    cursorLevel = 2;
                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }
            }

            //if (cursorLevel == 3 && cursorCoordLv1.Position == inventoryPos)
            //{
            //    int listLength = ShipInventoryManager.inventorySize;
            //
            //    int firstHalf;
            //    int secondHalf;
            //
            //    if (ShipInventoryManager.inventorySize > 14)
            //    {
            //        firstHalf = 14;
            //        secondHalf = listLength - firstHalf;
            //    }
            //    else
            //    {
            //        firstHalf = listLength;
            //        secondHalf = 0;
            //    }
            //
            //    if (ControlManager.CheckPress(RebindableKeys.Down)
            //        && elapsedSinceKey > elapseDelay)
            //    {
            //        cursorLevel3Position += 1;
            //
            //        if (cursorLevel3Position == firstHalf && column == 1) { cursorLevel3Position -= firstHalf + 1; }
            //        if (cursorLevel3Position == listLength && column == 2) { cursorLevel3Position = firstHalf; }                    
            //
            //        elapsedSinceKey = 0;
            //
            //        holdTimer = Game.HoldKeyTreshold;
            //    }
            //
            //    else if (ControlManager.CheckHold(RebindableKeys.Down))
            //    {
            //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
            //
            //        if (holdTimer <= 0)
            //        {
            //            cursorLevel3Position += 1;
            //
            //            if (cursorLevel3Position == firstHalf && column == 1) { cursorLevel3Position = firstHalf - 1; }
            //            if (cursorLevel3Position == listLength && column == 2) { cursorLevel3Position = listLength - 1; }
            //
            //            elapsedSinceKey = 0;
            //
            //            holdTimer = Game.ScrollSpeedFast;
            //        }
            //    }
            //
            //    if (ControlManager.CheckPress(RebindableKeys.Up)
            //        && elapsedSinceKey > elapseDelay)
            //    {
            //        cursorLevel3Position -= 1;
            //
            //        if (cursorLevel3Position == -2 && column == 1) { cursorLevel3Position += firstHalf + 1; }
            //        if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position += secondHalf; }
            //
            //        elapsedSinceKey = 0;
            //
            //        holdTimer = Game.HoldKeyTreshold;
            //    }
            //
            //    else if (ControlManager.CheckHold(RebindableKeys.Up))
            //    {
            //        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;
            //
            //        if (holdTimer <= 0)
            //        {
            //            cursorLevel3Position -= 1;
            //
            //            if (cursorLevel3Position == -1 && column == 1) { cursorLevel3Position = 0; }
            //            if (cursorLevel3Position == firstHalf - 1 && column == 2) { cursorLevel3Position = firstHalf; }
            //
            //            elapsedSinceKey = 0;
            //
            //            holdTimer = Game.ScrollSpeedFast;
            //        }
            //    }
            //
            //    if (ControlManager.CheckPress(RebindableKeys.Right)
            //        && elapsedSinceKey > elapseDelay)
            //    {
            //        if (cursorLevel3Position < firstHalf && secondHalf != 0)
            //        {
            //            if (cursorLevel3Position + firstHalf < listLength)
            //            {
            //                cursorLevel3Position += firstHalf;
            //                column = 2;
            //            }
            //            else
            //            {
            //                cursorLevel3Position = listLength - 1;
            //            }
            //            column = 2;
            //        }
            //        
            //        elapsedSinceKey = 0;
            //    }
            //
            //    if (ControlManager.CheckPress(RebindableKeys.Left)
            //        && elapsedSinceKey > elapseDelay)
            //    {
            //        if (cursorLevel3Position >= firstHalf)
            //        {
            //            cursorLevel3Position -= firstHalf;
            //            column = 1;
            //        }
            //        column = 1;
            //
            //        elapsedSinceKey = 0;
            //    }
            //
            //    //This is where the magic happens.
            //    if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))
            //        && elapsedSinceKey > elapseDelay))
            //    {
            //        //This is the command for equipping an owned weapon.
            //        if (cursorLevel3Position != -1)
            //        {
            //            SwitchComponents();
            //        }
            //        else if (cursorLevel3Position == -1)
            //        {
            //            EraseComponent();
            //        }
            //
            //        cursorLevel = 2;
            //        elapsedSinceKey = 0;
            //    }
            //
            //    if (ControlManager.CheckPress(RebindableKeys.Action2)
            //        && elapsedSinceKey > elapseDelay)
            //    {
            //        cursorLevel = 2;
            //        elapsedSinceKey = 0;
            //    }
            //}
        }
        
        private void CheckStateChangeCommands()
        {
            //State-Changers
            if (ControlManager.CheckPress(RebindableKeys.Pause) && elapsedSinceKey > elapseDelay && cursorLevel == 1)
                BackToOverworldLogic();

            if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > elapseDelay && cursorLevel == 1)
                BackToOverworldLogic();

            if (ControlManager.CheckPress(RebindableKeys.Pause) && elapsedSinceKey > elapseDelay && cursorLevel > 1)
            {
                cursorLevel = 1;
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckKeypress(Keys.I) && elapsedTimeMilliseconds > 200
                && elapsedSinceKey > elapseDelay)
                BackToOverworldLogic();

        }
        
        private void EquipComponent()
        {
            switch (cursorCoordLv1.ToInt())
            {
                case 0:
                    {
                        ShipInventoryManager.EquipItemFromSublist(ShipParts.EnergyCell, 0, cursorLevel2Position);
                        break; 
                    }
                case 1:
                    {
                        ShipInventoryManager.EquipItemFromSublist(ShipParts.Plating, 0, cursorLevel2Position);
                        break;
                    }
                case 2:
                    {
                        ShipInventoryManager.EquipItemFromSublist(ShipParts.Shield, 0, cursorLevel2Position);
                        break;
                    }
                case 3:
                    {
                        ShipInventoryManager.EquipItemFromSublist(ShipParts.Secondary, 0, cursorLevel2Position);
                        break;
                    }
                case 4:
                    {
                        ShipInventoryManager.EquipItemFromSublist(ShipParts.Primary, cursorCoordLv1.Y, cursorLevel2Position);
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Illegal or non-implemented value tried!");
                    }
            }
        }
        
        //private void SwitchComponents()
        //{
        //    if (cursorCoordLv1.Position == inventoryPos)
        //    {
        //        ShipInventoryManager.SwitchItems(cursorLevel2Position, cursorLevel3Position);
        //    }
        //}
        
        //private void EraseComponent()
        //{
        //    if (cursorCoordLv1.Position == inventoryPos)
        //    {
        //        Item erasedItem = new EmptyItem(Game);
        //
        //        if (ShipInventoryManager.ShipItems[cursorLevel2Position] != ShipInventoryManager.equippedEnergyCell && ShipInventoryManager.ShipItems[cursorLevel2Position] != ShipInventoryManager.equippedPlating)
        //        {
        //            ShipInventoryManager.RemoveItemAt(cursorLevel2Position);
        //            ShipInventoryManager.InsertItem(cursorLevel2Position, erasedItem);
        //        }
        //    }
        //}
    }
}
