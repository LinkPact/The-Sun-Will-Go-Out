using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace SpaceProject_Linux
{
    public abstract class ImmobileSpaceObject : GameObjectOverworld
    {
        private Shop shop;

        public List<Item> ShopInventory 
        { 
            get { return shop.ShopInventory; } 
            set { shop.ShopInventory = value; } 
        }

        public ShopFilling ShopFilling 
        { 
            get { return shop.ShopFilling; } 
        }

        public List<Item> OnEnterShopInventory 
        { 
            get { return shop.OnEnterShopInventory; } 
            set { shop.OnEnterShopInventory = value; } 
        }

        public List<Item> ItemPool 
        { 
            get { return shop.ItemPool; } 
            set { shop.ItemPool = value; } 
        }

        public ImmobileSpaceObject(Game1 Game, Sprite SpriteSheet)
            : base(Game, SpriteSheet)
        {
            shop = new Shop(Game, this.ToString());
        }

        public void AddShopEntry(ShopInventoryEntry entry)
        {
            shop.AddShopEntry(entry);
        }

        public void AddMandatoryItem(ShopInventoryEntry entry)
        {
            shop.AddMandatoryShipItem(entry);
        }

        public void SetShopFilling(ShopFilling filling)
        {
            shop.ShopFilling = filling;
        }

        public void UpdateShopInventory()
        {
            shop.UpdateShopInventory();
        }

        public void SaveShop()
        {
            shop.SaveShop();
        }

        public void LoadShop()
        {
            shop.LoadShop();
        }
    }
}
