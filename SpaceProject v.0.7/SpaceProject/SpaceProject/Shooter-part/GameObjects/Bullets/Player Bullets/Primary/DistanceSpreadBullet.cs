﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public class DistanceSpreadBullet : PlayerBullet
    {
        public DistanceSpreadBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            IsKilled = false;
            ObjectClass = "bullet";

            Damage = 35;
            Duration = 4000;
            Speed = 0.7f;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(56, 25, 5, 9)));

            Bounding = new Rectangle(56, 25, 5, 9);
            BoundingSpace = 0;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
