using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    //Ritar upp manamataren da man ar ute pa nagon niva
    public class ManaBar
    {
        private Sprite border;
        private Sprite manaBit;
        private PlayerVerticalShooter player;
        private float drawLayer = 0.8f;

        public ManaBar(PlayerVerticalShooter player, Sprite spriteSheet)
        {
            this.player = player;

            border = spriteSheet.GetSubSprite(new Rectangle(4, 53, 139, 12));
            manaBit = spriteSheet.GetSubSprite(new Rectangle(1, 54, 1, 10));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            float manaPerBit = player.MPmax / (border.Width - 2);
            int drawnBits = (int)(player.MP / manaPerBit);

            spriteBatch.Draw(border.Texture, position, border.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, drawLayer);

            if (!player.IsKilled)
            {
                for (int n = 1; n < drawnBits + 1; n++)
                    spriteBatch.Draw(manaBit.Texture, new Vector2(position.X + n, position.Y + 1), manaBit.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, drawLayer);
            }
        }
    }
}
