﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public abstract class LevelAction : Action
    {
        protected int incrementAmount;

        public LevelAction() : base ()
        { }

        public LevelAction(int incrementAmount)
            : base()
        {
            this.incrementAmount = incrementAmount;
        }
    }
}