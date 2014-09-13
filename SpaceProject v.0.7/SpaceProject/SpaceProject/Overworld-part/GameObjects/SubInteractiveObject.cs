using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SubInteractiveObject : GameObjectOverworld
    {
        protected List<string> text;
        protected List<string> options;
        protected MessageBox messageBox;

        protected SubInteractiveObject(Game1 game, Sprite spriteSheet, MessageBox messageBox) :
            base(game, spriteSheet)
        {
            this.messageBox = messageBox;
        }

        public override void Initialize()
        {
            base.Initialize();

            text = new List<string>();
            options = new List<string>();

            scale = 1f;
            layerDepth = 0.3f;
            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void Interact()
        {

        }
    }
}
