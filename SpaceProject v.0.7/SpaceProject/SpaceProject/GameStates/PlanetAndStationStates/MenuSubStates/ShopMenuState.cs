using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class ShopMenuState : MenuState
    {
        private const int INVENTORY_Y_SPACING = 18;

        #region Cursor Fields

        private Cursor activeInventoryCursor;
        private Cursor passiveInventoryCursor;
        private Vector2 inventoryCursorIndex;

        private int holdTimer;

        #endregion

        private List<Item> shopInventory;
        private readonly int shopInventorySize = 14;

        private float savedYPos;

        private List<Item> onEnterInventoryItems;
        private List<Item> onEnterShopItems;

        private Sprite confirmMenuSprite;
        private int confirmMenuIndex;
        private string confirmMenuMessage;
        private Item itemToBuy;
        private Item itemToSell;
        private int quantityCounterIndex;

        private List<string> confirmOptions;

        private bool displayBuyAndEquip = true;
        public bool DisplayBuyAndEquip { get { return displayBuyAndEquip; } set { displayBuyAndEquip = value; } }

        private ItemComparison itemComp;

        public ShopMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
        }

        public override void Initialize()
        {
            #region Initialize Cursors

            activeInventoryCursor = new Cursor(this.Game, this.SpriteSheet, new Rectangle(201, 121, 14, 14), new Rectangle(201, 135, 14, 14));
            passiveInventoryCursor = new Cursor(this.Game, this.SpriteSheet, new Rectangle(201, 121, 14, 14), new Rectangle(201, 135, 14, 14));

            #endregion

            onEnterInventoryItems = new List<Item>();
            onEnterShopItems = new List<Item>();

            itemComp = new ItemComparison(this.Game, this.SpriteSheet);
            itemComp.Initialize();

            confirmMenuSprite = SpriteSheet.GetSubSprite(new Nullable<Rectangle>(new Rectangle(0, 334, 200, 150))); 
            confirmOptions = new List<string>();
        }

        public override void OnEnter()
        {
            BaseStateManager.ButtonControl = ButtonControl.Inventory;

            LoadInventory();

            List<Item> onEnterShopInventory = new List<Item>();

            if (BaseState.GetBase() is Planet)
            {
                onEnterShopInventory = ((Planet)BaseState.GetBase()).OnEnterShopInventory;
            }

            else if (BaseState.GetBase() is Station)
            {
                onEnterShopInventory = ((Station)BaseState.GetBase()).OnEnterShopInventory;
            }

            for (int i = 0; i < onEnterShopInventory.Count; i++)
            {
                if (onEnterShopInventory[i] != shopInventory[i])
                {
                    if (shopInventory[i].Kind != "Empty")
                        shopInventory[i].ShopColor = 3;
                }
            }

            foreach (Item item in shopInventory)
                onEnterShopItems.Add(item);

            foreach (Item item in ShipInventoryManager.ShipItems)
                onEnterInventoryItems.Add(item);

            #region Reset Cursors

            activeInventoryCursor.isVisible = true;
            activeInventoryCursor.isActive = true;
            activeInventoryCursor.isSmall = true;

            passiveInventoryCursor.isSmall = true;
            passiveInventoryCursor.isActive = false;
            passiveInventoryCursor.isVisible = false;
            
            inventoryCursorIndex = new Vector2(1, 1);

            #endregion

            BaseStateManager.ActiveButton = null;

            savedYPos = -1;

            BaseStateManager.ButtonControl = ButtonControl.Shop;

            holdTimer = Game.HoldKeyTreshold;
        }

        public override void OnLeave()
        {
            ReturnInventory();

            foreach (Item item in ShipInventoryManager.ShipItems)
            {
                item.ShopColor = 0;
            }

            if (BaseState.GetBase() is Planet)
            {
                ((Planet)BaseState.GetBase()).OnEnterShopInventory.Clear();
            }

            else if (BaseState.GetBase() is Station)
            {
                ((Station)BaseState.GetBase()).OnEnterShopInventory.Clear();
            }

            foreach (Item item in shopInventory)
            {
                if (BaseState.GetBase() is Planet)
                {
                    ((Planet)BaseState.GetBase()).OnEnterShopInventory.Add(item);
                }

                else if (BaseState.GetBase() is Station)
                {
                    ((Station)BaseState.GetBase()).OnEnterShopInventory.Add(item);
                }
            }

            onEnterInventoryItems.Clear();
            onEnterShopItems.Clear();
        }

        private void LoadInventory()
        {
            if (BaseState.GetBase() is Planet)
            {
                this.shopInventory = ((Planet)BaseState.GetBase()).ShopInventory;
            }

            else if (BaseState.GetBase() is Station)
            {
                this.shopInventory = ((Station)BaseState.GetBase()).ShopInventory;
            }

            for (int i = shopInventory.Count; i < 14; i++)
                shopInventory.Add(new EmptyItem(this.Game));

        }

        private void ReturnInventory()
        {
            foreach (Item item in shopInventory)
                item.ShopColor = 0;

            if (BaseState.GetBase() is Planet)
            {
                ((Planet)BaseState.GetBase()).ShopInventory = this.shopInventory;
            }

            else if (BaseState.GetBase() is Station)
            {
                ((Station)BaseState.GetBase()).ShopInventory = this.shopInventory;
            }
        }

        public override void Update(GameTime gameTime)
        {
            #region Input

            #region Keyboard
            if (BaseStateManager.ButtonControl == ButtonControl.Shop)
            {
                if (ControlManager.CheckPress(RebindableKeys.Action2) ||
                    ControlManager.CheckPress(RebindableKeys.Pause))
                {
                    base.Update(gameTime);
                }
            }

            #region CursorControls
            if (BaseStateManager.ButtonControl == ButtonControl.Shop)
            {
                //Moves button cursor down when pressing down. 
                if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    inventoryCursorIndex.Y++;
                    savedYPos = -1;
                    itemComp.ShowSymbols = false;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Down))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        inventoryCursorIndex.Y++;
                        savedYPos = -1;
                        itemComp.ShowSymbols = false;
                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                //Moves button cursor up when pressing up
                else if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    inventoryCursorIndex.Y--;
                    savedYPos = -1;
                    itemComp.ShowSymbols = false;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Up))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        inventoryCursorIndex.Y--;
                        savedYPos = -1;
                        itemComp.ShowSymbols = false;
                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                //Moves button cursor right when pressing right. 
                if (ControlManager.CheckPress(RebindableKeys.Right))
                {
                    if (ShipInventoryManager.inventorySize > 14)
                    {
                        if (savedYPos != -1)
                        {
                            inventoryCursorIndex.Y = savedYPos;
                            savedYPos = -1;
                        }

                        if (inventoryCursorIndex.X == 1)
                            savedYPos = inventoryCursorIndex.Y;

                        inventoryCursorIndex.X++;

                        if (inventoryCursorIndex.X > 3)
                            inventoryCursorIndex.X = 3;

                        if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize - 14 &&
                            inventoryCursorIndex.X.Equals(2))
                            inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                    }

                    else
                    {
                        inventoryCursorIndex.X = 3;
                    }

                    itemComp.ShowSymbols = false;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Right))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        if (ShipInventoryManager.inventorySize > 14)
                        {
                            if (savedYPos != -1)
                            {
                                inventoryCursorIndex.Y = savedYPos;
                                savedYPos = -1;
                            }

                            if (inventoryCursorIndex.X == 1)
                                savedYPos = inventoryCursorIndex.Y;

                            inventoryCursorIndex.X++;

                            if (inventoryCursorIndex.X > 3)
                                inventoryCursorIndex.X = 3;

                            if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize - 14 &&
                                inventoryCursorIndex.X.Equals(2))
                                inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                        }

                        else
                        {
                            inventoryCursorIndex.X = 3;
                        }

                        itemComp.ShowSymbols = false;

                        holdTimer = Game.ScrollSpeedFast;
                    }
                }

                //Moves button cursor left when pressing left
                else if (ControlManager.CheckPress(RebindableKeys.Left))
                {
                    if (ShipInventoryManager.inventorySize > 14)
                    {
                        if (savedYPos != -1)
                        {
                            inventoryCursorIndex.Y = savedYPos;
                            savedYPos = -1;
                        }

                        if (inventoryCursorIndex.X == 3)
                            savedYPos = inventoryCursorIndex.Y;

                        inventoryCursorIndex.X--;

                        if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize - 14 &&
                            inventoryCursorIndex.X.Equals(2))
                            inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                    }

                    else
                    {
                        inventoryCursorIndex.X = 1;

                        if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize &&
                            inventoryCursorIndex.X.Equals(1))
                            inventoryCursorIndex.Y = ShipInventoryManager.inventorySize;
                    }

                    itemComp.ShowSymbols = false;
                    holdTimer = Game.HoldKeyTreshold;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Left))
                {
                    holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                    if (holdTimer <= 0)
                    {
                        if (ShipInventoryManager.inventorySize > 14)
                        {
                            if (savedYPos != -1)
                            {
                                inventoryCursorIndex.Y = savedYPos;
                                savedYPos = -1;
                            }

                            if (inventoryCursorIndex.X == 3)
                                savedYPos = inventoryCursorIndex.Y;

                            inventoryCursorIndex.X--;

                            if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize - 14 &&
                                inventoryCursorIndex.X.Equals(2))
                                inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                        }

                        else
                        {
                            inventoryCursorIndex.X = 1;

                            if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize &&
                                inventoryCursorIndex.X.Equals(1))
                                inventoryCursorIndex.Y = ShipInventoryManager.inventorySize;
                        }

                        itemComp.ShowSymbols = false;
                        holdTimer = Game.ScrollSpeedFast;
                    }
                }
            }

            else if (BaseStateManager.ButtonControl == ButtonControl.TransactionConfirm)
            {
                if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    confirmMenuIndex--;
                }

                else if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    confirmMenuIndex++;
                }

                if (confirmMenuIndex > confirmOptions.Count - 1)
                {
                    confirmMenuIndex = 0;
                }

                else if (confirmMenuIndex < 0)
                {
                    confirmMenuIndex = confirmOptions.Count - 1;
                }

                if (confirmOptions.Count > 0 && confirmMenuIndex == 1 &&
                    (confirmOptions[1].ToLower().EndsWith("unit") || confirmOptions[1].ToLower().EndsWith("units")))
                {
                    if (ControlManager.CheckPress(RebindableKeys.Right))
                    {
                        quantityCounterIndex++;
                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Right))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            quantityCounterIndex++;
                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }

                    else if (ControlManager.CheckPress(RebindableKeys.Left))
                    {
                        quantityCounterIndex--;
                        holdTimer = Game.HoldKeyTreshold;
                    }

                    else if (ControlManager.CheckHold(RebindableKeys.Left))
                    {
                        holdTimer -= gameTime.ElapsedGameTime.Milliseconds;

                        if (holdTimer <= 0)
                        {
                            quantityCounterIndex--;
                            holdTimer = Game.ScrollSpeedFast;
                        }
                    }
                    if (itemToSell != null)
                    {
                        if (quantityCounterIndex > ((QuantityItem)itemToSell).Quantity)
                        {
                            quantityCounterIndex = (int)((QuantityItem)itemToSell).Quantity;
                        }
                    }

                    else if (itemToBuy != null)
                    {
                        if (quantityCounterIndex > ((QuantityItem)itemToBuy).Quantity)
                        {
                            quantityCounterIndex = (int)((QuantityItem)itemToBuy).Quantity;
                        }
                    }

                    if (quantityCounterIndex < 0)
                    {
                        quantityCounterIndex = 0;
                    }

                    if (quantityCounterIndex > 1)
                    {
                        if (itemToSell != null)
                        {
                            confirmOptions[1] = "Sell " + quantityCounterIndex.ToString() + " units";
                        }

                        else if (itemToBuy != null)
                        {
                            confirmOptions[1] = "Buy " + quantityCounterIndex.ToString() + " units";
                        }
                    }

                    else
                    {
                        if (itemToSell != null)
                        {
                            confirmOptions[1] = "Sell " + quantityCounterIndex.ToString() + " unit";
                        }

                        else if (itemToBuy != null)
                        {
                            confirmOptions[1] = "Buy " + quantityCounterIndex.ToString() + " unit";
                        }
                    }

                }

                if ((itemToBuy != null && itemToBuy is QuantityItem) ||
                    itemToSell != null && itemToSell is QuantityItem)
                {
                    UpdateConfirmMenuMessage();
                }
            }
            #endregion

            #endregion

            #region Wrapping And Positioning

            if (inventoryCursorIndex.X > 3)
                    inventoryCursorIndex.X = 3;

                else if (inventoryCursorIndex.X < 1)
                    inventoryCursorIndex.X = 1;

            if (inventoryCursorIndex.X.Equals(1))
            {
                if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                        inventoryCursorIndex.Y = 1;

                    else
                        inventoryCursorIndex.Y = ShipInventoryManager.inventorySize;
                }

                else if (inventoryCursorIndex.Y > 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                        inventoryCursorIndex.Y = 1;

                    else
                        inventoryCursorIndex.Y = 14;
                }

                else if (inventoryCursorIndex.Y < 1 && ShipInventoryManager.inventorySize >= 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                        inventoryCursorIndex.Y = 14;
                    else
                        inventoryCursorIndex.Y = 1;
                }

                else if (inventoryCursorIndex.Y < 1 && ShipInventoryManager.inventorySize < 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                        inventoryCursorIndex.Y = ShipInventoryManager.inventorySize;
                    else
                        inventoryCursorIndex.Y = 1;
                }

                activeInventoryCursor.position.X = Game.Window.ClientBounds.Width / 3 + 10;
            }

            else if (inventoryCursorIndex.X.Equals(2))
            {
                if (inventoryCursorIndex.Y > 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                        inventoryCursorIndex.Y = 1;

                    else
                        inventoryCursorIndex.Y = 14;
                }

                else if (inventoryCursorIndex.Y > ShipInventoryManager.inventorySize - 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                        inventoryCursorIndex.Y = 1;

                    else
                        inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                }

                else if (inventoryCursorIndex.Y < 1)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                        inventoryCursorIndex.Y = ShipInventoryManager.inventorySize - 14;
                    else
                        inventoryCursorIndex.Y = 1;
                }

                activeInventoryCursor.position.X = Game.Window.ClientBounds.Width * 1 / 3 + 180;
            }

            else if (inventoryCursorIndex.X.Equals(3))
            {
                if (inventoryCursorIndex.Y > 14)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Down))
                        inventoryCursorIndex.Y = 1;

                    else
                        inventoryCursorIndex.Y = 14;
                }

                else if (inventoryCursorIndex.Y > shopInventory.Count)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                        inventoryCursorIndex.Y = 1;
                    else
                        inventoryCursorIndex.Y = shopInventory.Count;
                }

                else if (inventoryCursorIndex.Y < 1)
                {
                    if (ControlManager.PreviousKeyUp(RebindableKeys.Up))
                        inventoryCursorIndex.Y = shopInventory.Count;
                    else
                        inventoryCursorIndex.Y = 1;
                }

                activeInventoryCursor.position.X = Game.Window.ClientBounds.Width * 4 / 5 - 10;
            }

            activeInventoryCursor.position.Y = (Game.Window.ClientBounds.Height / 2 + 20) + inventoryCursorIndex.Y * INVENTORY_Y_SPACING;

            #endregion            

            #endregion

            itemComp.CompareStats();

            if (!itemComp.ShowSymbols)
            {
                if (inventoryCursorIndex.X == 1)
                {
                    itemComp.SetItem2(ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y - 1]);
                    itemComp.FindEquippedItem(ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y - 1].Kind);
                    itemComp.ShowSymbols = true;
                }

                else if (inventoryCursorIndex.X == 2)
                {
                    itemComp.SetItem2(ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y + 13]);
                    itemComp.FindEquippedItem(ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y + 13].Kind);
                    itemComp.ShowSymbols = true;
                }

                else
                {
                    itemComp.SetItem2(shopInventory[(int)inventoryCursorIndex.Y - 1]);
                    itemComp.FindEquippedItem(shopInventory[(int)inventoryCursorIndex.Y - 1].Kind);
                    itemComp.ShowSymbols = true;
                }
            }

        }

        int tempIndex;
        public override void ButtonActions()
        {
            itemComp.ShowSymbols = false;

            switch ((int)inventoryCursorIndex.X)
            {
                case 1:
                case 3:
                    tempIndex = (int)inventoryCursorIndex.Y - 1;
                    break;

                case 2:
                    tempIndex = (int)inventoryCursorIndex.Y + 13;
                    break;
            }

            if (BaseStateManager.ButtonControl != ButtonControl.TransactionConfirm)
            {
                if (inventoryCursorIndex.X < 3)
                {
                    itemToSell = ShipInventoryManager.ShipItems[tempIndex];

                    if (itemToSell is EmptyItem)
                    {
                        itemToSell = null;
                        return;
                    }

                    DisplayConfirmMenu();
                }

                if (inventoryCursorIndex.X.Equals(3))
                {
                    itemToBuy = shopInventory[tempIndex];

                    if (itemToBuy is EmptyItem)
                    {
                        itemToBuy = null;
                        return;
                    }

                    DisplayConfirmMenu();
                }
            }

            else
            {
                switch (confirmOptions[confirmMenuIndex].ToLower())
                {
                    case "buy":
                        BuyItem(itemToBuy, tempIndex);
                        break;

                    case "buy & equip":
                        if (BuyItem(itemToBuy, tempIndex))
                        {
                            ShipInventoryManager.EquipItemFromInventory(itemToBuy.Kind, ShipInventoryManager.IndexOfItem(itemToBuy));
                        }
                        break;

                    case "buy all":
                        BuyQuantityItem(itemToBuy, tempIndex, (int)((QuantityItem)itemToBuy).Quantity);
                        break;
                           
                    case "don't buy":
                        break;

                    case "sell":
                        SellItem(itemToSell, tempIndex);
                        break;

                    case "remove & sell":
                        UnequipItem(itemToSell);
                        SellItem(itemToSell, tempIndex);
                        break;

                    case "sell all":
                        SellQuantityItem(itemToSell, tempIndex, (int)((QuantityItem)itemToSell).Quantity);
                        break;

                    case "don't sell":
                        break;

                    case "equip":
                        ShipInventoryManager.EquipItemFromInventory(itemToSell.Kind, ShipInventoryManager.IndexOfItem(itemToSell));
                        break;

                    default:
                        if (itemToSell != null)
                        {
                            if (confirmOptions[1].EndsWith("unit") || confirmOptions[1].EndsWith("units"))
                            {
                                SellQuantityItem(itemToSell, tempIndex, quantityCounterIndex);
                            }
                        }

                        else if (itemToBuy != null)
                        {
                            if (confirmOptions[1].EndsWith("unit") || confirmOptions[1].EndsWith("units"))
                            {
                                BuyQuantityItem(itemToBuy, tempIndex, quantityCounterIndex);
                            }
                        }
                        break;
                }

                itemToSell = null;
                itemToBuy = null;
                BaseStateManager.ButtonControl = ButtonControl.Shop;
            }
        }

        public override void CursorActions()
        { }

        public override void Draw(SpriteBatch spriteBatch)
        {
            DisplayInventory(spriteBatch);
            DisplayShopInventory(spriteBatch);

            passiveInventoryCursor.Draw(spriteBatch);
            activeInventoryCursor.Draw(spriteBatch);

            if (inventoryCursorIndex.X == 1)
                ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y - 1].DisplayInfo(spriteBatch,
                                                                                           BaseState.Game.fontManager.GetFont(14),
                                                                                           new Vector2(10, 50),
                                                                                           Game.fontManager.FontColor,
                                                                                           Game.Window.ClientBounds.Width / 2 - 10);

            else if (inventoryCursorIndex.X == 2)
                ShipInventoryManager.ShipItems[(int)inventoryCursorIndex.Y + 13].DisplayInfo(spriteBatch,
                                                                                       BaseState.Game.fontManager.GetFont(14),
                                                                                       new Vector2(10, 50),
                                                                                       Game.fontManager.FontColor,
                                                                                       Game.Window.ClientBounds.Width / 2 - 10);

            else if (inventoryCursorIndex.X == 3)
                shopInventory[(int)inventoryCursorIndex.Y - 1].DisplayInfo(spriteBatch,
                                                                           BaseState.Game.fontManager.GetFont(14),
                                                                           new Vector2(10, 50),
                                                                           Game.fontManager.FontColor,
                                                                            Game.Window.ClientBounds.Width / 2 - 10);

            itemComp.Draw(spriteBatch, new Vector2(1, 91), 15);

            spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                   "Rupees: " + StatsManager.Rupees,
                                   new Vector2(10, Game.Window.ClientBounds.Height / 2 - 25) + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor);

            if (BaseStateManager.ButtonControl == ButtonControl.TransactionConfirm)
            {      
                spriteBatch.Draw(confirmMenuSprite.Texture,
                    new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2),
                    confirmMenuSprite.SourceRectangle, new Color(255, 255, 255, 185), .0f,
                    new Vector2(confirmMenuSprite.SourceRectangle.Value.Width / 2, confirmMenuSprite.SourceRectangle.Value.Height / 2),
                    1.5f, SpriteEffects.None, 0.95f);

                spriteBatch.DrawString(Game.fontManager.GetFont(14),
                    TextUtils.WordWrap(Game.fontManager.GetFont(14), confirmMenuMessage, (int)Math.Round(confirmMenuSprite.Width * 1.5f, 0) - 20),
                    new Vector2((Game.Window.ClientBounds.Width / 2 - (confirmMenuSprite.Width * 1.5f) / 2) + 10,
                        (Game.Window.ClientBounds.Height / 2 - (confirmMenuSprite.Height * 1.5f) / 2) + 20),
                    Color.White, .0f, Vector2.Zero,
                    1f, SpriteEffects.None, 1f);

                int divider = 4;

                if (confirmOptions.Count > 2)
                {
                    divider = 6;
                }

                for (int i = 0; i < confirmOptions.Count; i++)
                {
                    Color tempColor = Color.White;

                    if (confirmMenuIndex == i)
                    {
                        tempColor = Color.Red;
                    }

                    spriteBatch.DrawString(Game.fontManager.GetFont(14),
                        confirmOptions[i],
                        new Vector2(Game.Window.ClientBounds.Width / 2, Game.Window.ClientBounds.Height / 2 + ((confirmMenuSprite.Height * 1.5f) / divider) + (i * 20)),
                        tempColor, .0f,
                        Game.fontManager.GetFont(14).MeasureString(confirmOptions[i]) / 2,
                        1f, SpriteEffects.None, 1f);
                }
            }

            //// Draw back-button
            //spriteBatch.DrawString(Game.fontManager.GetFont(14), "Go Back",
            //    new Vector2(Game.Window.ClientBounds.Width / 3 + 18,
            //        Game.Window.ClientBounds.Height - 25) + Game.fontManager.FontOffset,
            //    Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
        }

        private void DisplayInventory(SpriteBatch spriteBatch)
        {
            int itemCount = ShipInventoryManager.ShipItems.Count;

            spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                   "Inventory",
                                   new Vector2(Game.Window.ClientBounds.Width / 2 + 25,
                                               Game.Window.ClientBounds.Height / 2 + 15) + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   0,
                                   BaseState.Game.fontManager.GetFont(14).MeasureString("Inventory") / 2,
                                   1.0f,
                                   SpriteEffects.None,
                                   0.5f);

            int columnSize = 14;
            int inventory = ShipInventoryManager.inventorySize;

            if (inventory > columnSize)
            {
                for (int n = 0; n < columnSize; n++)
                {
                    if (ShipInventoryManager.IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                          ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Blue, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else if(ShipInventoryManager.ShipItems[n].ShopColor == 1)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                          ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Green, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                    else
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                          ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                }

                for (int n = 0; n < inventory - columnSize; n++)
                {
                    if (ShipInventoryManager.IsEquipped(ShipInventoryManager.ShipItems[n + columnSize]))
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n + columnSize].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 190,
                                                           ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Blue, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else if (ShipInventoryManager.ShipItems[n + columnSize].ShopColor == 1)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n + columnSize].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 190,
                                                          ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Green, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n + columnSize].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 190,
                                                           ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                }
            }
            else
            {
                for (int n = 0; n < inventory; n++)
                {
                    if (ShipInventoryManager.IsEquipped(ShipInventoryManager.ShipItems[n]))
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                           ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Blue, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else if (ShipInventoryManager.ShipItems[n].ShopColor == 1)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                          ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Color.Green, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                               ShipInventoryManager.ShipItems[n].Name,
                                               new Vector2(Game.Window.ClientBounds.Width / 3 + 20,
                                                           ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                               Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                }
            }

        }

        private void DisplayShopInventory(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                        "Shop",
                        new Vector2(Game.Window.ClientBounds.Width * 4/5 + 55,
                                    Game.Window.ClientBounds.Height / 2 + 15) + Game.fontManager.FontOffset,
                        Game.fontManager.FontColor,
                        0,
                        BaseState.Game.fontManager.GetFont(14).MeasureString("Shop") / 2,
                        1.0f,
                        SpriteEffects.None,
                        0.5f);

            int columnSize = 14;

            if (shopInventory.Count > columnSize)
            {

                for (int n = 0; n < columnSize; n++)
                {
                    if(shopInventory[n].ShopColor == 2)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Color.Red, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else if (shopInventory[n].ShopColor == 3)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Color.Yellow, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else
                    spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                }
            }

            else
            {
                for (int n = 0; n < shopInventory.Count; n++)
                {
                    if (shopInventory[n].ShopColor == 2)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Color.Red, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else if (shopInventory[n].ShopColor == 3)
                        spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Color.Yellow, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);

                    else
                    spriteBatch.DrawString(BaseState.Game.fontManager.GetFont(14),
                                           shopInventory[n].Name,
                                           new Vector2(Game.Window.ClientBounds.Width * 4 / 5,
                                                      ((Game.Window.ClientBounds.Height / 2) + 25) + n * INVENTORY_Y_SPACING) + Game.fontManager.FontOffset,
                                           Game.fontManager.FontColor, .0f, Vector2.Zero, 1f, SpriteEffects.None, .5f);
                }
            }
        }

        private void DisplayConfirmMenu()
        {
            confirmOptions.Clear();

            StringBuilder message = new StringBuilder("Do you want to ");

            if (itemToBuy != null)
            {
                message.Append("buy ")
                .Append(itemToBuy.Name)
                .Append("?")
                .Append("\n\nIt will cost: ")
                .Append(itemToBuy.Value)
                .Append(" rupees");

                if (itemToBuy is QuantityItem)
                {
                    quantityCounterIndex = 1;
                    confirmOptions.Add("Buy all");
                    confirmOptions.Add("Buy 1 unit");
                }

                else
                {
                    confirmOptions.Add("Buy");
                }

                if (displayBuyAndEquip 
                    && (itemToBuy is PlayerWeapon 
                        || itemToBuy is PlayerEnergyCell 
                        || itemToBuy is PlayerShield 
                        || itemToBuy is PlayerPlating))
                {
                    confirmOptions.Add("Buy & equip");
                }

                confirmOptions.Add("Don't buy");

                confirmMenuIndex = confirmOptions.Count - 1;
                confirmMenuMessage = message.ToString();
                BaseStateManager.ButtonControl = ButtonControl.TransactionConfirm;
            }

            else if (itemToSell != null)
            {
                if (ShipInventoryManager.IsEquipped(itemToSell) 
                    && itemToSell is PlayerEnergyCell)
                {
                    PopupHandler.DisplayMessage("You can't sell equipped energy cells!");
                }

                else if (ShipInventoryManager.IsEquipped(itemToSell) 
                    && itemToSell is PlayerPlating
                    && ShipInventoryManager.IsLastEquipped(itemToSell))
                {
                    PopupHandler.DisplayMessage("You can't sell your last plating!");
                }
                else
                {

                    message.Append("sell ");

                    if (ShipInventoryManager.IsEquipped(itemToSell))
                    {
                        message.Append("your equipped ");
                        confirmOptions.Add("Remove & sell");
                    }

                    else if (itemToSell is QuantityItem)
                    {
                        quantityCounterIndex = 1;
                        confirmOptions.Add("Sell all");
                        confirmOptions.Add("Sell 1 unit");
                    }

                    else
                    {
                        confirmOptions.Add("Sell");
                        confirmOptions.Add("Equip");
                    }

                    confirmOptions.Add("Don't sell");

                    message.Append(itemToSell.Name)
                    .Append("?")
                    .Append("\n\nYou will recieve: ")
                    .Append((int)Math.Round(itemToSell.Value / 2, 0))
                    .Append(" rupees");

                    if (ShipInventoryManager.IsLastEquipped(itemToSell))
                    {
                        if (itemToSell.Kind.ToLower().Equals("primary"))
                        {
                            message.Append("\n\nWARNING: This is your LAST primary weapon.");
                        }

                        else if (itemToSell.Kind.ToLower().Equals("secondary"))
                        {
                            message.Append("\n\nWARNING: This is your LAST secondary weapon.");
                        }

                        else if (itemToSell.Kind.ToLower().Equals("shield"))
                        {
                            message.Append("\n\nWARNING: This is your LAST shield.");
                        }
                    }

                    confirmMenuIndex = confirmOptions.Count - 1;
                    confirmMenuMessage = message.ToString();
                    BaseStateManager.ButtonControl = ButtonControl.TransactionConfirm;
                }
            }
        }
        
        private void UpdateConfirmMenuMessage()
        {
            StringBuilder message = new StringBuilder("Do you want to ");

            if (itemToSell != null && itemToSell is QuantityItem)
            {
                message.Append("sell ")
                .Append(itemToSell.Name)
                .Append("?");

                switch (confirmMenuIndex)
                {
                    case 0:
                        message.Append("\n\nYou will recieve: ")
                        .Append(((Int32)Math.Round(itemToSell.Value / 2 * ((ResourceItem)itemToSell).Quantity, 0)).ToString())
                        .Append(" rupees");
                        break;

                    case 1:
                        message.Append("\n\nYou will recieve: ")
                        .Append(((Int32)Math.Round(itemToSell.Value / 2 * quantityCounterIndex, 0)).ToString())
                        .Append(" rupees");
                        break;

                    default:
                        break;
                }

                confirmMenuMessage = message.ToString();
            }

            else if (itemToBuy != null && itemToBuy is QuantityItem)
            {
                message.Append("buy ")
                .Append(itemToBuy.Name)
                .Append("?");

                switch (confirmMenuIndex)
                {
                    case 0:
                        message.Append("\n\nIt will cost: ")
                        .Append(((Int32)Math.Round(itemToBuy.Value * ((ResourceItem)itemToBuy).Quantity, 0)).ToString())
                        .Append(" rupees");
                        break;

                    case 1:
                        message.Append("\n\nIt will cost: ")
                        .Append(((Int32)Math.Round(itemToBuy.Value * quantityCounterIndex, 0)).ToString())
                        .Append(" rupees");
                        break;

                    default:
                        break;
                }

                confirmMenuMessage = message.ToString();
            }
        }

        private void SellItem(Item item, int indexOfItem)
        {
            int shopIndex = 0;

            for (int i = shopInventory.Count - 1; i >= 0; i--)
            {
                if (shopInventory[i] is EmptyItem)
                {
                    shopIndex = i;
                }
            }

            shopInventory.RemoveAt(shopIndex);

            shopInventory.Insert(shopIndex, item);
            ShipInventoryManager.RemoveItemAt(indexOfItem);
            ShipInventoryManager.InsertItem(indexOfItem, new EmptyItem(Game));
            StatsManager.Rupees += (int)Math.Round(item.Value / 2, 0);

            if (shopInventory.Count > shopInventorySize)
            {
                shopInventory.RemoveAt(shopInventorySize);
            }
        }

        private void SellQuantityItem(Item item, int indexOfItem, int quantity)
        {
            QuantityItem tempItem;
            int shopIndex = 0;

            for (int i = shopInventory.Count - 1; i >= 0; i--)
            {
                if (shopInventory[i] is EmptyItem)
                {
                    shopIndex = i;
                }
            }

            // Sell all
            if (quantity == ((ResourceItem)item).Quantity)
            {
                shopInventory.Insert(shopIndex, item);
                ShipInventoryManager.RemoveItemAt(indexOfItem);
                ShipInventoryManager.InsertItem(indexOfItem, new EmptyItem(Game));
            }

            // Sell some
            else if (quantity < ((ResourceItem)item).Quantity)
            {
                ((QuantityItem)item).Quantity -= quantity;

                switch (((QuantityItem)item).Name.ToLower())
                {
                    case "gold":
                        tempItem = new GoldResource(Game, quantity);
                        break;

                    case "copper":
                        tempItem = new CopperResource(Game, quantity);
                        break;

                    case "titanium":
                        tempItem = new TitaniumResource(Game, quantity);
                        break;

                    case "finewhiskey":
                        tempItem = new FineWhiskey(Game, quantity);
                        break;

                    default:
                        tempItem = null;
                        break;
                }

                shopInventory.Insert(shopIndex, tempItem);
            }

            StatsManager.Rupees += (int)Math.Round(item.Value / 2 * quantity, 0);

            if (shopInventory.Count > shopInventorySize)
            {
                shopInventory.RemoveAt(shopInventorySize);
            }
        }

        private bool BuyQuantityItem(Item item, int indexOfItem, int quantity)
        {
            if (!ShipInventoryManager.HasAvailableSlot())
            {
                PopupHandler.DisplayMessage("You do not have any free inventory slots. Sell something first!");
                return false;
            }

            else if (StatsManager.Rupees < Math.Round(item.Value * quantity, 0))
            {
                PopupHandler.DisplayMessage("You do not have enough rupees!");
                return false;
            }

            // Buy all
            if (quantity == ((ResourceItem)item).Quantity)
            {
                ShipInventoryManager.AddQuantityItem(Game, quantity, item.Kind, item.Name);
                shopInventory.RemoveAt(indexOfItem);
                shopInventory.Insert(indexOfItem, new EmptyItem(Game));
            }

            // Buy some
            else if (quantity < ((ResourceItem)item).Quantity)
            {
                ((QuantityItem)item).Quantity -= quantity;

                ShipInventoryManager.AddQuantityItem(Game, quantity, item.Kind, item.Name);
            }

            StatsManager.Rupees -= (int)Math.Round(item.Value * quantity, 0);

            return true;
        }

        // Call to buy items. Returns false either if player does not have enough money or if
        // the inventory is full.
        private bool BuyItem(Item item, int indexOfItem)
        {
            if (!ShipInventoryManager.HasAvailableSlot())
            {
                PopupHandler.DisplayMessage("You do not have any free inventory slots. Sell something first!");
                return false;
            }

            else if (StatsManager.Rupees < item.Value)
            {
                PopupHandler.DisplayMessage("You do not have enough rupees!");
                return false;
            }

            ShipInventoryManager.AddItem(item);
            shopInventory.RemoveAt(indexOfItem);
            shopInventory.Insert(indexOfItem, new EmptyItem(Game));
            StatsManager.Rupees -= (int)item.Value;
            return true;
        }

        private void UnequipItem(Item item)
        {
            if (item is PlayerWeapon)
            {
                if (item.Kind == "Primary")
                {
                    int tempIndex = ShipInventoryManager.equippedPrimaryWeapons.IndexOf((PlayerWeapon)item);
                    ShipInventoryManager.equippedPrimaryWeapons.Remove((PlayerWeapon)item);
                    ShipInventoryManager.equippedPrimaryWeapons.Insert(tempIndex, new EmptyWeapon(Game)); 
                }

                else if (item.Kind == "Secondary")
                {
                    ShipInventoryManager.equippedSecondary = new EmptyWeapon(Game);
                }
            }

            else if (item is PlayerShield)
            {
                ShipInventoryManager.equippedShield = new EmptyShield(Game);
            }
        }
    }
}
