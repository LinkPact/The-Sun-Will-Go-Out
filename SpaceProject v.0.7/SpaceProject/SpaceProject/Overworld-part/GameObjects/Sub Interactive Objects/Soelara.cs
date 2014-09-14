﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Soelara : SubInteractiveObject
    {
        public Soelara(Game1 Game, Sprite spriteSheet, MessageBox messageBox) :
            base(Game, spriteSheet, messageBox)
        {

        }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(311, 966, 412, 412));
            position = new Vector2(100000, 112000);
            name = "Soelara";

            base.Initialize();

            SetupText("A big gas giant. A space station orbits the planet.");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Interact()
        {
            base.Interact();
        }
    }
}
