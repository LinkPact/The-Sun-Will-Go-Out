using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class MotherShip : GameObjectOverworld
    {
        private Random random;

        protected List<Item> shopInventory;
        public List<Item> ShopInventory { get { return shopInventory; } set { shopInventory = value; } }

        protected List<Item> onEnterShopInventory;
        public List<Item> OnEnterShopInventory { get { return onEnterShopInventory; } set { onEnterShopInventory = value; } }

        protected List<Item> itemPool;
        public List<Item> ItemPool { get { return itemPool; } set { itemPool = value; } }

        public MotherShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        { }

        public override void Initialize()
        {
            random = new Random();

            shopInventory = new List<Item>();
            onEnterShopInventory = new List<Item>();
            itemPool = new List<Item>();

            Class = "MotherShip";
            name = "Mother Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(552, 95, 161, 77));
            position = new Vector2(500, 1800);
            speed = 0;
            color = Color.White;
            scale = 1.0f;
            layerDepth = 0.31f;

            UpdateItemPool();

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if(GameStateManager.currentState == "OverworldState")
                IsUsed = true;

            else
                IsUsed = false;

            if (IsUsed)
                base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if(IsUsed)
                base.Draw(spriteBatch);
        }

        #region Shop Update
        //Restocks the station's shop inventory using items from the item pool 
        public void UpdateShopInventory()
        {
            for (int i = shopInventory.Count - 1; i >= 0; i--)
            {
                if (shopInventory[i].Kind == "Empty")
                {
                    if (random.Next(100) > 50)
                    {
                        shopInventory.RemoveAt(i);
                        shopInventory.Insert(i, itemPool[i]);
                    }
                }

                else
                {
                    //if (random.Next(100) > 60)
                    //{
                    //    shopInventory.RemoveAt(i);
                    //    shopInventory.Insert(i, new EmptyItem(this.Game, this.spriteSheet));
                    //}

                    if (random.Next(100) > 50)
                    {
                        shopInventory.RemoveAt(i);
                        shopInventory.Insert(i, itemPool[i]);
                    }

                }
            }

            CompressShopInventory();
            UpdateItemPool();
        }

        // Appends items to item pool
        public void UpdateItemPool()
        {
            Random random = new Random();
            Sprite sprite = Game.stateManager.shooterState.spriteSheet;

            itemPool.Clear();

            Item tempItem = null;

            int itemsToAdd = 3 + random.Next(7) + 1;

            for (int i = 0; i < itemsToAdd; i++)
            {
                switch (random.Next(14))
                {
                    case 0:
                        tempItem = new BasicLaserWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 1:
                        tempItem = new MultipleShotWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 2:
                        tempItem = new SpreadBulletWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 3:
                        tempItem = new BeamWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 4:
                        tempItem = new RegularBombWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 5:
                        tempItem = new HomingMissileWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 6:
                        tempItem = new SideMissilesWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 7:
                        tempItem = new DrillBeamWeapon(this.Game, ItemVariety.regular);
                        break;

                    case 8:
                        tempItem = new BasicShield(this.Game, ItemVariety.regular);
                        break;

                    case 9:
                        tempItem = new AdvancedShield(this.Game, ItemVariety.regular);
                        break;

                    case 10:
                        tempItem = new RegularShield(this.Game, ItemVariety.regular);
                        break;

                    case 11:
                        tempItem = new BasicEnergyCell(this.Game, ItemVariety.regular);
                        break;

                    case 12:
                        tempItem = new AdvancedEnergyCell(this.Game, ItemVariety.regular);
                        break;

                    case 13:
                        tempItem = new RegularEnergyCell(this.Game, ItemVariety.regular);
                        break;

                }
                itemPool.Add(tempItem);

            }

            if (itemPool.Count < 14)
            {
                int tempCount = itemPool.Count;

                for (int i = tempCount; i < 14; i++)
                    itemPool.Add(new EmptyItem(this.Game));
            }
        }

        private void CompressShopInventory()
        {
            List<int> removeList = new List<int>();
            for (int n = 0; n < shopInventory.Count; n++)
            {
                if (shopInventory[n].Kind == "Empty")
                    removeList.Add(n);
            }
            for (int n = removeList.Count; n > 0; n--)
            {
                shopInventory.RemoveAt(removeList[n - 1]); ;
                shopInventory.Add(new EmptyItem(this.Game));
            }
        }

        public void FillShopInventory()
        {
            if (shopInventory.Count < 14)
            {
                int tempCount = shopInventory.Count;
                for (int i = 0; i < 14 - tempCount - 1; i++)
                    shopInventory.Add(new EmptyItem(this.Game));
            }
        }
        #endregion
    }
}
