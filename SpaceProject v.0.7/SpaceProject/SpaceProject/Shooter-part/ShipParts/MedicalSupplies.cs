using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class MedicalSupplies : StoryItem
    {
        public MedicalSupplies(Game1 Game) :
            base(Game)
        {
            Name = "Medical Supplies";
            Kind = "StoryItems";
            Value = 800;
            Text = "A metal box containing various medical supplies.";
        }

        protected override String GetDescription()
        {
            return "TODO: Move desciption here.";
        }
    }
}
