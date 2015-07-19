using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class FortrunStation2 : SubInteractiveObject
    {
        private List<Item> randomItems;
        private Item item;

        public FortrunStation2(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(342, 871, 93, 93));
            position = new Vector2(95300, 97000);
            name = "Fortrun Station II";

            base.Initialize();

            Setup();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (((ItemShopOE)overworldEvent).IsItemBought)
            {
                ((ItemShopOE)overworldEvent).SetNewItem(SelectRandomItem());
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            base.Interact();
        }

        private Item SelectRandomItem()
        {
            randomItems = new List<Item>();

            randomItems.Add(new BasicLaserWeapon(Game, GetRandomItemVariety()));
            randomItems.Add(new DualLaserWeapon(Game, GetRandomItemVariety()));
            randomItems.Add(new SpreadBulletWeapon(Game, GetRandomItemVariety()));
            randomItems.Add(new WaveBeamWeapon(Game, GetRandomItemVariety()));
            randomItems.Add(new BasicShield(Game, GetRandomItemVariety()));
            randomItems.Add(new BasicEnergyCell(Game, GetRandomItemVariety()));
            randomItems.Add(new BasicPlating(Game, GetRandomItemVariety()));

            return randomItems[Game.random.Next(0, randomItems.Count - 1)];
        }

        private void Setup()
        {
            item = SelectRandomItem();

            overworldEvent = new ItemShopOE(item, "Do you want to buy this one-of-a-kind " + item.Name + "? Only 300 Crebits!",
                "Thanks anyway, sir!",
                "Thank you! Have a lovely day, sir!",
                "I'm afraid you do not have enough money, sir.",
                "Your inventory is full! How silly of you, sir!", 300, false);
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }

        private ItemVariety GetRandomItemVariety()
        {
            var val = MathFunctions.GetExternalRandomInt(0, 2);

            switch (val)
            {
                case 0:
                    return ItemVariety.Low;

                case 1:
                    return ItemVariety.Regular;

                case 2:
                    return ItemVariety.High;

                default:
                    throw new ArgumentException("Invalid number of cases.");
            }
        }
    }
}
