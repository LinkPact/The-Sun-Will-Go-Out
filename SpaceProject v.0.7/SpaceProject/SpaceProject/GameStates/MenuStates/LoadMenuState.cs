using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

//****NOT USED*****//
// Loading is done through MainMenuState

namespace SpaceProject
{
    public class LoadMenuState: GameState
    {
        public LoadMenuState(Game1 Game, string name):
            base(Game, name)
        { 
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void OnEnter()
        {
        }

        public override void OnLeave()
        {

        }

        public override void Update(GameTime gameTime)
        {
            //if (ControlManager.CheckPress("action2") ||
              //  ControlManager.CheckPress("pause"))
            Game.Load();
            Game.stateManager.ChangeState("OverworldState");
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
        }
    }
}
