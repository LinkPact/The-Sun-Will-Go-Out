﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    class EmptyOE : OverworldEvent
    {
        public EmptyOE() :
            base()
        { }

        public override Boolean Activate() 
        {
            return true;
        }
    }
}
