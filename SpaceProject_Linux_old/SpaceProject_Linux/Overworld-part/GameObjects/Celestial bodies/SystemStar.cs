using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    public class SystemStar: GameObjectOverworld
    {
        public int ObjectHeatRadius;

        protected SystemStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            Class = "BigStar";
            layerDepth = 0.3f;
            speed = 0;
            color = Color.White;

            ObjectHeatRadius = sprite.SourceRectangle.Value.Width;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            if (GameStateManager.currentState == "OverworldState")
                IsUsed = true;
            else
                IsUsed = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsed == true)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
