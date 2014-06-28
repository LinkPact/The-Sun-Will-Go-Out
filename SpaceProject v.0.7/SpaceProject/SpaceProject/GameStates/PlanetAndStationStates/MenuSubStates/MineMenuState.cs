using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class MineMenuState : MenuState
    {
        public MineMenuState(Game1 game, String name, BaseStateManager manager, BaseState baseState) :
            base(game, name, manager, baseState)
        {
        }

        public override void Initialize()
        { }

        public override void OnEnter()
        { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void ButtonActions()
        { }

        public override void CursorActions()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        { }
    }
}
