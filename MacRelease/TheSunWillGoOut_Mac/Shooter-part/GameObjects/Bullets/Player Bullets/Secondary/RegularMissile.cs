﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    class RegularMissile : PlayerBullet
    {
        public RegularMissile(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.3f;
            IsKilled = false;
            Damage = 160;
            ObjectClass = "bullet";
            ObjectName = "RegularMissile";
            
            Duration = 3000;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(42, 0, 7, 22)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(49, 0, 7, 22)));

            Bounding = new Rectangle(42, 0, 7, 22);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Speed *= 0.99f;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI/2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }

    }
}
