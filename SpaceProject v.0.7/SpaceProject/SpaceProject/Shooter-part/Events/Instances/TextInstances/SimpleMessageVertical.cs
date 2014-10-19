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

        public SimpleMessageVertical(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, 
            float startTime, String message)
            : base(Game, player, spriteSheet, level, startTime)
        {
            this.message = message;
        }

        //The function called when the class is activated.
        //For events inheriting from the "PointLevelEvent"-class
        //this is all that is executed before the event is removed.
        //Check "Swarm"-class to see how lasting events can look.
        public override void Run(GameTime gameTime)
        {
            Game.messageBox.DisplayMessage(message, false);
            TriggerStatus = Trigger.Completed;
        }

        public override List<CreaturePackage> RetrieveCreatures()
        {
            throw new ArgumentNullException("This class has no creatures to return, look into this");
        }
    }
}
