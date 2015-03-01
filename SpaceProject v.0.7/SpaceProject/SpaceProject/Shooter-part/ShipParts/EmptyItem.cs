using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class EmptyItem : Item
    {

        public EmptyItem(Game1 Game, ItemVariety variety = ItemVariety.regular) :
            base(Game)
        {
            Name = "---";
            Kind = "Empty";
            Weight = 0;
            Value = 0;
        }

        protected override String GetDescription()
        {
            return "";
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();
            infoText.Add("Empty");
            return infoText;
        }
    }
}
