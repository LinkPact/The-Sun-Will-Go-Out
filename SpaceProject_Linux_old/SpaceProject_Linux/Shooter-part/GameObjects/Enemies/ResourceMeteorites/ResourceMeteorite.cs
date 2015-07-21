using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class ResourceMeteorite : EnemyShip
    {
        protected float rotationDir;

        protected ResourceMeteorite(Game1 Game, Sprite SpriteSheet, PlayerVerticalShooter player):
            base(Game,SpriteSheet, player)
        {
            this.player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            ObjectClass = "enemy";
        }

        public override void DeInitialize()
        {
            base.DeInitialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            rotationDir += Rotation;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //base.Draw(spriteBatch);
            spriteBatch.Draw(CurrentAnim.CurrentFrame.Texture, Position, CurrentAnim.CurrentFrame.SourceRectangle, Color.White, rotationDir, CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
