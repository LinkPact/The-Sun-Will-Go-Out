using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{

    //Class used to create a single instance of an enemy
    class SimpleMessageVertical : LevelEvent
    {
        private String message;
        private PortraitID portrait;

        public SimpleMessageVertical(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, 
            float startTime, String message, PortraitID portrait = PortraitID.None)
            : base(Game, player, spriteSheet, level, startTime)
        {
            this.message = message;
            this.portrait = portrait;
        }

        //The function called when the class is activated.
        //For events inheriting from the "PointLevelEvent"-class
        //this is all that is executed before the event is removed.
        //Check "Swarm"-class to see how lasting events can look.
        public override void Run(GameTime gameTime)
        {
            if (portrait != PortraitID.None)
            {
                PopupHandler.DisplayPortraitMessage(portrait, message);
            }
            else
            {
                PopupHandler.DisplayMessage(message);
            }
            TriggerStatus = Trigger.Completed;
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            throw new ArgumentNullException("This class has no creatures to return, look into this");
        }
    }
}
