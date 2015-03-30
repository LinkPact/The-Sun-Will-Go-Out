using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    public abstract class StoryItem : Item
    {
        private string text;

        #region Properties

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        #endregion

        protected StoryItem(Game1 Game) :
            base(Game)
        {
        }

        public virtual void Initialize()
        { }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();
            infoText.Add(Name);
            infoText.Add(text);
            return infoText;
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }
    }
}
