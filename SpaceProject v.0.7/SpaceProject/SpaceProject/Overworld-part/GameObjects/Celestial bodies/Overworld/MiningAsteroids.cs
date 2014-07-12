﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    class MiningAsteroids : MisicSpaceObject
    {
        public MiningAsteroids(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
        }

        public MiningAsteroids(Game1 game, Sprite spriteSheet, Vector2 pos) :
            base(game, spriteSheet)
        {
            Initialize(pos);
        }

        public void Initialize(Vector2 pos)
        {
            position = pos;
            Initialize();
        }


        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(580, 9, 239, 224));

            name = "Mining Astroids";

            scale = 1f;
            color = Color.White;
            layerDepth = 0.5f;
            IsUsed = true;
            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}