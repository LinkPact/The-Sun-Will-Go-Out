﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class FragmentMissile : PlayerBullet
    {
        private int fragments;

        public FragmentMissile(Game1 Game, Sprite spriteSheet, int fragments) :
            base(Game, spriteSheet)
        {
            this.fragments = fragments;
        }

        public override void Initialize()
        {
            base.Initialize();

            //Egenskaper
            Speed = 0.2f;
            IsKilled = false;
            Damage = 75;
            ObjectClass = "bullet";
            ObjectName = "FragmentMissile";
            Duration = 500;

            anim.LoopTime = 300;
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(149, 60, 9, 26)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(158, 60, 9, 26)));
            anim.AddFrame(spriteSheet.GetSubSprite(new Rectangle(167, 60, 9, 26)));

            Bounding = new Rectangle(149, 60, 9, 26);

            BoundingSpace = 0;

            CenterPoint = new Vector2(anim.Width / 2, anim.Height / 2);

            useDeathAnim = true;
        }

        public override void Update(GameTime gameTime)
        {
            if (Duration <= 0)
                Explode();

            base.Update(gameTime);
        }

        private void Explode()
        {
            for (int n = 0; n < fragments; n++)
            {
                MissileFragment fragment = new MissileFragment(Game, spriteSheet);
                fragment.Initialize();
                fragment.Position = Position;
                fragment.Duration = 400;
                fragment.Direction = MathFunctions.SpreadDir(new Vector2(0, -1), Math.PI / 3);

                float fragmentBaseSpeed = 0.6f;

                fragment.Speed = (float)(random.NextDouble() * fragmentBaseSpeed + fragmentBaseSpeed);
                fragment.Speed *= 0.5f;

                Game.stateManager.shooterState.gameObjects.Add(fragment);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(anim.CurrentFrame.Texture, Position, anim.CurrentFrame.SourceRectangle, Color.White, (float)(Radians + Math.PI / 2), CenterPoint, 1.0f, SpriteEffects.None, DrawLayer);
        }

    }
}
