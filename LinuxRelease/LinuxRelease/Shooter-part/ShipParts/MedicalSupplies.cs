using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class MedicalSupplies : StoryItem
    {
        public MedicalSupplies(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        // Quick hack to make this class Savefile compatable. Please ignore. 
        public MedicalSupplies(Game1 Game, int foo) :
            base(Game)
        {
            Setup();
        }

        private void Setup()
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
