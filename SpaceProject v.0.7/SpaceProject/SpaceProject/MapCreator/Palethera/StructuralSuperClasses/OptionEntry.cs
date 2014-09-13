using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject.MapCreator
{
    abstract class OptionEntry
    {
        protected String displayText;
        protected OptionSquare optionSquare;
        protected Vector2 position;

        protected Vector2 squarePos;

        protected Boolean hasNoSquare;

        protected OptionEntry(Vector2 position)
        {
            this.position = position;
            squarePos = new Vector2(position.X + 110, position.Y + 5);
            hasNoSquare = false;
        }

        protected void SetHasNoSquare()
        {
            hasNoSquare = true;
        }

        public void Update(GameTime gameTime)
        {
            if (!hasNoSquare)
                optionSquare.Update(gameTime);
        }

        public Boolean IsReadyToSetDisplay()
        {
            if (!hasNoSquare)
                return optionSquare.IsReadyToSetDisplay();
            else
                return false;
        }

        public void SetDisplay(ActiveSquare display)
        {
            optionSquare.SetDisplay(display);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (!hasNoSquare)
                optionSquare.Draw(spriteBatch);
            
            spriteBatch.DrawString(MapCreatorGUI.staticFontExperiment, displayText, position, Color.Black);
        }
    }
}
