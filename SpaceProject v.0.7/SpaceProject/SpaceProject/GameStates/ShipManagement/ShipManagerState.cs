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

        //private int column;

        private int holdTimer;

        //Comparison-related variables
        private ItemComparison itemComp;

        #endregion

        public bool IsShieldSlotSelected
        {
            get
            {
                return (cursorCoordLv1.Position == 2
                    && cursorLevel == 2);
            }
        }

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
            //column = 1;

            elapsedSinceKey = 0;
            elapseDelay = 50;

            itemComp = new ItemComparison(this.Game, this.spriteSheet, new Rectangle(504, 772, 9, 5),
                new Rectangle(504, 787, 9, 8), new Rectangle(504, 778, 9, 8));
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
            if (StatsManager.gameMode == GameMode.Develop)
                DeveloperOptions();
            
            elapsedTimeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

            CheckKeys(gameTime);

            cursorManager.Update(gameTime, cursorLevel, cursorCoordLv1, cursorLevel2Position);
            fontManager.Update(gameTime, cursorLevel, cursorCoordLv1.ToInt(), cursorLevel2Position);
            informationManager.Update(gameTime, cursorLevel, cursorCoordLv1.ToInt(),
                cursorCoordLv1.Y, cursorLevel2Position, "ShipManagerState", -1);

            elapsedSinceKey += gameTime.ElapsedGameTime.Milliseconds;

            itemComp.CompareStats();

            ItemComparision();
        }

        private void ItemComparision()
        {
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
                        switch (cursorCoordLv1.ToInt())
                        {
                            case 0:
                                itemComp.SetItem2(ShipInventoryManager.ownedEnergyCells[cursorLevel2Position]);
                                itemComp.SetItem1(ShipInventoryManager.equippedEnergyCell);
                                itemComp.ShowSymbols = true;
                                break;
            
                            case 1:
                                itemComp.SetItem2(ShipInventoryManager.OwnedPlatings[cursorLevel2Position]);
                                itemComp.SetItem1(ShipInventoryManager.equippedPlating);
                                itemComp.ShowSymbols = true;
                                break;
            
                            case 2:
                                itemComp.SetItem2(ShipInventoryManager.ownedShields[cursorLevel2Position]);
                                itemComp.SetItem1(ShipInventoryManager.equippedShield);
                                itemComp.ShowSymbols = true;
                                break;
            
                            case 3:
                                itemComp.SetItem2(ShipInventoryManager.OwnedSecondary[cursorLevel2Position]);
                                itemComp.SetItem1(ShipInventoryManager.equippedSecondary);
                                itemComp.ShowSymbols = true;
                                break;
            
                            case 4:
                                if (cursorCoordLv1.Y == 0)
                                {
                                    itemComp.SetItem2(ShipInventoryManager.GetAvailablePrimaryWeapons(1)[cursorLevel2Position]);
                                    itemComp.SetItem1(ShipInventoryManager.equippedPrimaryWeapons[0]);
                                    itemComp.ShowSymbols = true;
                                }

                                else if (cursorCoordLv1.Y == 1)
                                {
                                    itemComp.SetItem2(ShipInventoryManager.GetAvailablePrimaryWeapons(2)[cursorLevel2Position]);
                                    itemComp.SetItem1(ShipInventoryManager.equippedPrimaryWeapons[1]);
                                    itemComp.ShowSymbols = true;
                                }
                                break;
                        }
                        break;
                    }
            }
        }

        private void DeveloperOptions()
        {
            if (ControlManager.CheckKeyPress(Keys.D1))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_1");
            }

            if (ControlManager.CheckKeyPress(Keys.D2))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_2");
            }

            if (ControlManager.CheckKeyPress(Keys.D3))
            {
                Game.stateManager.shooterState.BeginLevel("flightTraining_3");
            }

            if (ControlManager.CheckKeyPress(Keys.D0))
            {
                Game.stateManager.shooterState.BeginLevel("JakobDevelop");
            }

            if (ControlManager.CheckKeyPress(Keys.D9))
            {
                Game.stateManager.shooterState.BeginLevel("mapCreator2");
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad0))
            {
                ShipInventoryManager.ActivateCheatPrimary();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad1))
            {
                ShipInventoryManager.ActivateCheatSecondary();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad2))
            {
                ShipInventoryManager.ActivateCheatEnergy();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad3))
            {
                ShipInventoryManager.ActivateCheatShield();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad4))
            {
                ShipInventoryManager.ActivateCheatPlating();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad5))
            {
                ShipInventoryManager.ActivateCheatEquip1();
            }

            if (ControlManager.CheckKeyPress(Keys.NumPad6))
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
            {
                CheckKeysLevel1();
                CheckMouseLevel1();
            }
            else if (cursorLevel == 2)
            {
                CheckKeysLevel2(gameTime);
                CheckMouseLevel2();
            }
        }     
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.White);

            //Ritar bakgrundsobjekt
            DrawBackground(spriteBatch);

            cursorManager.Draw(spriteBatch);
            fontManager.Draw(spriteBatch);
            informationManager.Draw(spriteBatch);

            if (Game.Window.ClientBounds.Height.Equals(768))
            {
                itemComp.Draw(spriteBatch, new Vector2(34, 481), 15); 
            }

            else if (Game.Window.ClientBounds.Height.Equals(720))
            {
                itemComp.Draw(spriteBatch, new Vector2(34, 456), 15); 
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
        
        private void CheckKeysLevel1()
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

            if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) && cursorLevel == 1
                && elapsedSinceKey > elapseDelay))
            {
                OnPressLevel1();
            }
        }

        private void CheckMouseLevel1()
        {
            for (int i = 0; i < cursorManager.displayList.Count; i++)
            {
                if (ControlManager.IsMouseOverArea(cursorManager.displayList[i].Bounds))
                {
                    cursorManager.CursorCoordLv1.X = cursorManager.displayList[i].Coordinate.X;
                    cursorManager.CursorCoordLv1.Y = cursorManager.displayList[i].Coordinate.Y;

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        OnPressLevel1();
                    }
                }
            }
        }

        private void CheckKeysLevel2(GameTime gameTime)
        {
            if (cursorLevel == 2 && cursorCoordLv1.Position != inventoryPos)
            {
                int listLength;

                if (cursorCoordLv1.Position != 4)
                {
                    listLength = ShipInventoryManager.ownCounts[cursorCoordLv1.Position];
                }

                else
                {
                    if (cursorCoordLv1.Y == 0)
                    {
                        listLength = ShipInventoryManager.GetAvailablePrimaryWeapons(1).Count;
                    }
                    else
                    {
                        listLength = ShipInventoryManager.GetAvailablePrimaryWeapons(2).Count;
                    }
                }

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
                if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay))
                {
                    OnPressLevel2();
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && cursorLevel == 2
                    && elapsedSinceKey > elapseDelay)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }
            }
        }

        private void CheckMouseLevel2()
        {
            int listLength;
            string text = "";

            if (cursorCoordLv1.Position != 4)
            {
                listLength = ShipInventoryManager.ownCounts[cursorCoordLv1.Position];
            }

            else
            {
                if (cursorCoordLv1.Y == 0)
                {
                    listLength = ShipInventoryManager.GetAvailablePrimaryWeapons(1).Count;
                }
                else
                {
                    listLength = ShipInventoryManager.GetAvailablePrimaryWeapons(2).Count;
                }
            }

            for (int i = 0; i < listLength; i++)
            {
                if (cursorCoordLv1.Position != 4)
                {
                    if (cursorCoordLv1.Position == 0)
                    {
                        text = ShipInventoryManager.ownedEnergyCells[i].Name;
                    } 
                    else if (cursorCoordLv1.Position == 1)
                    {
                        text = ShipInventoryManager.ownedPlatings[i].Name;
                    }
                    else if (cursorCoordLv1.Position == 2)
                    {
                        text = ShipInventoryManager.ownedShields[i].Name;
                    }
                    else if (cursorCoordLv1.Position == 3)
                    {
                        text = ShipInventoryManager.OwnedSecondary[i].Name;
                    }
                }

                else
                {
                    if (cursorCoordLv1.Y == 0)
                    {
                        text = ShipInventoryManager.GetAvailablePrimaryWeapons(1)[i].Name;
                    }
                    else
                    {
                        text = ShipInventoryManager.GetAvailablePrimaryWeapons(2)[i].Name;
                    }
                }

                if (ControlManager.IsMouseOverText(FontManager.GetFontStatic(16), text, 
                    new Vector2(Game.Window.ClientBounds.Width / 2 + 50, 93 + i * 23), Vector2.Zero, false))
                {
                    cursorLevel2Position = i;

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        OnPressLevel2();
                    }
                }
            }
        }

        private void OnPressLevel1()
        {
            if (cursorCoordLv1.Position != backPos
                    && cursorCoordLv1.Position < 4
                    && ShipInventoryManager.ownCounts[cursorCoordLv1.Position] > 0)
            {
                cursorLevel = 2;
                cursorLevel2Position = 0;
                elapsedSinceKey = 0;
            }

            else if (cursorCoordLv1.Position == 4)
            {
                switch (cursorCoordLv1.Y)
                {
                    case 0:
                        if (ShipInventoryManager.GetAvailablePrimaryWeapons(1).Count > 0)
                        {
                            cursorLevel = 2;
                            cursorLevel2Position = 0;
                            elapsedSinceKey = 0;
                        }
                        break;

                    case 1:
                        if (ShipInventoryManager.GetAvailablePrimaryWeapons(2).Count > 0)
                        {
                            cursorLevel = 2;
                            cursorLevel2Position = 0;
                            elapsedSinceKey = 0;
                        }
                        break;
                }
            }
            else if (cursorCoordLv1.Position == backPos)
            {
                BackToOverworldLogic();
            }
        }

        private void OnPressLevel2()
        {
            //This is the command for equipping an owned weapon.
            EquipComponent();

            cursorLevel = 2;
            elapsedSinceKey = 0;
        }

        private void BackToOverworldLogic()
        {
            Game.stateManager.ChangeState(GameStateManager.previousState);
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

            if (ControlManager.CheckKeyPress(Keys.I) && elapsedTimeMilliseconds > 200
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
                        switch (cursorCoordLv1.Y)
                        {
                            case 0:
                                ShipInventoryManager.EquipItemFromSublist(ShipParts.Primary1, cursorCoordLv1.Y, cursorLevel2Position);
                                break;

                            case 1:
                                ShipInventoryManager.EquipItemFromSublist(ShipParts.Primary2, cursorCoordLv1.Y, cursorLevel2Position);
                                break;
                        }
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Illegal or non-implemented value tried!");
                    }
            }
        }
    }
}
