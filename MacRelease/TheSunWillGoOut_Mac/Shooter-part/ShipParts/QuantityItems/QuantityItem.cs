using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public abstract class QuantityItem : Item
    {
        protected string text;
        public string Text { get { return text; } set { text = value; } }

        protected float quantity;
        protected float maxQuantity;

        public float Quantity { get { return quantity; } set { quantity = value; } }
        public float MaxQuantity { get { return maxQuantity; } set { maxQuantity = value; } }

        protected QuantityItem(Game1 Game):
            base(Game)
        {
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();
            infoText.Add(Name);
            infoText.Add(text);
            infoText.Add("Quantity: " + Quantity + " units");
            infoText.Add("Value: " + Value + " Crebits per unit");
            return infoText;
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }

    }
}
