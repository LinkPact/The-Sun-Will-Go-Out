﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace SpaceProject_Mac
{
    public class TurretBullet : PlayerBullet
    {
        public TurretBullet(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            Speed = 0.8f;
            IsKilled = false;
            Damage = 80;
            ObjectClass = "bullet";
            Duration = 600;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(140, 80, 2, 20)));

            Bounding = new Rectangle(140, 60, 5, 20);
            BoundingSpace = 0;
            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }
    }
}
