using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class ActiveObjectiveSquare : ActiveSquare
    {
        private Sprite overlaySprite;
        private Vector2 markerPosition;

        public ActiveObjectiveSquare(Sprite spriteSheet, Vector2 position)
            : base(position)
        {
            overlaySprite = spriteSheet.GetSubSprite(new Rectangle(171, 0, 13, 13));
            this.position = position;
            markerPosition = new Vector2(position.X + 110, position.Y + 5);
            markerPosition.X += 80; // Ugly solution for aligning active cross over correct squares at startup.
            color = Color.White;
        }

        public void SetDisplay(LevelObjective newObjective, Vector2 pos)
        {
            ActiveData.levelObjective = newObjective;
            markerPosition = pos;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        private Vector2 UpdatePosition(int posNbr)
        {
            Vector2 returnVector = new Vector2();
            returnVector = GetMovementPos(posNbr);

            return returnVector;
        }

        private Vector2 GetMovementPos(int n)
        {
            return new Vector2(position.X + 90, position.Y - 50);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            if (overlaySprite != null)
                spriteBatch.Draw(overlaySprite.Texture, markerPosition, overlaySprite.SourceRectangle, Color.White);
        }
    }
}
