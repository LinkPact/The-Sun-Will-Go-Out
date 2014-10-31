using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SectorXStar : SystemStar
    {
        public SectorXStar(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {            
        }

        public override void Initialize()
        {
            name = "system1Star";
            position = new Vector2(OverworldState.OVERWORLD_WIDTH / 2,
                                   OverworldState.OVERWORLD_HEIGHT / 2);
            sprite = spriteSheet.GetSubSprite(new Rectangle(761, 384, 739, 690));
            scale = 1.0f;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            IsUsed = true;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
