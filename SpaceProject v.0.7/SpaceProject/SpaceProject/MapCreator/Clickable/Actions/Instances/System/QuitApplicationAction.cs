﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject.MapCreator
{
    public class QuitApplicationAction : SystemAction
    {

        public QuitApplicationAction() : base()
        { }

        protected override void ActionLogic(Game1 game)
        {
            game.stateManager.ChangeState("MainMenuState");
        }
    }
}
