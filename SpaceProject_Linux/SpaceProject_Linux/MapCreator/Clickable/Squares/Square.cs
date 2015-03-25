using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Linux
{
    // Visualization of a single event in the level.
    public abstract class Square : Clickable
    {
        protected Sprite overlayDisplay;
        protected Sprite emptySquare;
        public SquareData data;

        protected SquareClick clickInformation;
        protected Coordinate coordinate;

        public Square(SquareData data, Vector2 position, Sprite spriteSheet, Coordinate coordinate)
            : base(spriteSheet, position)
        {
            this.data = data;
            this.position = position;
            this.coordinate = coordinate;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            UpdateState();
            color = DataConversionLibrary.GetSquareColor(data.enemyType);

            if (IsTargeted())
                MapCreatorGUI.SetTargetedSquare(this);
        }

        protected abstract void UpdateState();

        public Boolean HasClickInformation()
        {
            return clickInformation != null;
        }

        public SquareClick GetClickInformation()
        {
            SquareClick temp = clickInformation;
            clickInformation = null;
            return temp;
        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (overlayDisplay != null)
                spriteBatch.Draw(overlayDisplay.Texture, position + offset, overlayDisplay.SourceRectangle, Color.White);
        }

        public virtual void DrawInfo(SpriteBatch spriteBatch, SpriteFont font, Vector2 position)
        {
            if (data.enemyType == EnemyType.none)
                return;

            List<String> squareInfo = GetSquareInfo();
            int spacing = 15;

            for (int n = 0; n < squareInfo.Count; n++)
            {
                spriteBatch.DrawString(font, squareInfo[n], new Vector2(position.X, position.Y + n * spacing), Color.Black);
            }
        }

        protected abstract List<String> GetSquareInfo();

        public EnemyType GetEnemyType()
        {
            return data.enemyType;
        }
    }
}
