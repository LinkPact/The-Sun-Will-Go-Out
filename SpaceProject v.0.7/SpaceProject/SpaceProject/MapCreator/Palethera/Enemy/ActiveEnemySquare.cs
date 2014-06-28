using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class ActiveEnemySquare : ActiveSquare
    {
        public ActiveEnemySquare(Sprite spriteSheet, Vector2 position)
            : base(position)
        { 
            this.displaySprite = spriteSheet.GetSubSprite(new Rectangle(50, 0, 12, 12));
        }

        public void SetDisplay(EnemyType newState)
        {
            ActiveData.enemyState = newState;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            color = DataConversionLibrary.GetSquareColor(ActiveData.enemyState);
        }
    }
}
