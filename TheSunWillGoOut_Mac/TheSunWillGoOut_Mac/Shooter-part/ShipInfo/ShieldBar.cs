using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    //Ritar upp livmataren da man ar ute pa nagon niva
    public class ShieldBar
    {
        private Sprite border;
        private Sprite healthBit;
        private PlayerVerticalShooter player;
        private float drawLayer = 0.8f;

        public ShieldBar(PlayerVerticalShooter player, Sprite spriteSheet)
        {
            this.player = player;

            border = spriteSheet.GetSubSprite(new Rectangle(4, 53, 139, 12));
            healthBit = spriteSheet.GetSubSprite(new Rectangle(2, 54, 1, 5));
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            float shieldPerBit = player.ShieldMax / (border.Width - 2);
            int drawnBits = (int)(player.Shield / shieldPerBit);

            spriteBatch.Draw(border.Texture, position, border.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, drawLayer);

            if (!player.IsKilled)
            {
                for (int n = 1; n < drawnBits + 1; n++)
                    spriteBatch.Draw(healthBit.Texture, new Vector2(position.X + n, position.Y + 1), healthBit.SourceRectangle, Color.White, 0.0f, Vector2.Zero, 1.0f, SpriteEffects.None, drawLayer);
            }
        }
    }
}
