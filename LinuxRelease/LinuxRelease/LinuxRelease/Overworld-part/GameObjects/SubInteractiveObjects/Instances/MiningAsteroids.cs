using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class MiningAsteroids : SubInteractiveObject
    {
        public MiningAsteroids(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {
            
        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(580, 9, 239, 224));

            name = "Mining Asteroids";

            position = new Vector2(124500, 93000);
            scale = 1f;
            color = Color.White;
            layerDepth = 0.5f;

            base.Initialize();
            overworldEvent = new DisplayTextOE("A group of asteroids used for mining.");
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
