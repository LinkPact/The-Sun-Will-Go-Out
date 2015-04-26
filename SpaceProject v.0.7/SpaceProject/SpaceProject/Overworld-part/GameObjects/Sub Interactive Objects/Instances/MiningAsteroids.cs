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
            SetupText("A group of asteroids used for mining.");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            base.Interact();
        }

        protected override void SetClearedText()
        {
            clearedText = "EMPTY";
        }
    }
}
