using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class FindTextOnAsteroidOE : OverworldEvent
    {
        private List<String> beginningText = new List<String> { 
            "You find a deserted building.#It seems to once have been a space station. There is nothing of value here now.", 
            "You find a message scribbled into the stone.#\"Noone is who they seem to be anymore...\"", 
            "You find a message scribbled into the stone.#\"Once upon a time. What happened then?\"#\"It is all gone.\"", 
            "You find a message scribbled into the stone.#\"Help me\"#Looking around, you see a man laying on the ground.#It is too late to help him now.", 
            "You find a deserted building.#It might have belonged to the rebels once. Outside stands a broken minelayer ship.",
            "You find a deserted building.#They seem to have stored crop in there. It smells so badly.#Why did they leave it?", 
            "You find a deserted building.#There is a huge antenna sprouting from its top. It might have been a broadcast station once.",
            "This asteroid smells like old socks. Best leave it...",
            "This asteroid looks like a huge potato.",
            "[SAIR] An ambush!#...#[SAIR] Sorry, I should probably update my sensors. False alarm.",
            "[SAIR] Another... asteroid! It gets so repetitive.",
            "You find a message scribbled into the stone.#\"Have you been to the corner of the universe? The upper left.\"#\"My grandmother hid some forgotten technology there. Far, far away...\"",
            "You find a message scribbled into the stone.#\"Sometimes, voices whisper three names to me. Who are they?\""
        };

        private List<String> allianceText = new List<String> { "alliance1#...", "alliance2#followup", "alliance3#more#more!" };
        
        private List<String> rebelText = new List<String> { "rebel1#...", "rebel2#followup", "rebel3#more#more!" };
        
        private List<String> endText = new List<String> { "end1#...", "end2#followup", "end3#more#more!" };

        private String displayText = "";

        public FindTextOnAsteroidOE() :
            base()
        { }

        public override Boolean Activate()
        {
            var eventTextList = new List<String>();
            Boolean successfullyActivated = false;

            if (!IsCleared())
            {
                if (displayText == "")
                {
                    displayText = GetProgressBasedText();
                }
                
                var itemOE = new DisplayTextOE(displayText);
                successfullyActivated = itemOE.Activate();
            }

            return successfullyActivated;
        }

        private String GetProgressBasedText()
        {
            var currentPhase = MissionManager.GetCurrentGamePhase();

            switch (currentPhase)
            {
                case GamePhase.beginning:
                    {
                        return MathFunctions.PickRandomFromList<String>(beginningText);
                    }
                case GamePhase.withAlliance:
                    {
                        return MathFunctions.PickRandomFromList<String>(beginningText);
                    }
                case GamePhase.withRebels:
                    {
                        return MathFunctions.PickRandomFromList<String>(beginningText);
                    }
                case GamePhase.ending:
                    {
                        return MathFunctions.PickRandomFromList<String>(beginningText);
                    }
                default:
                    {
                        throw new ArgumentException(string.Format("Unknown argument given: {0}", currentPhase.ToString()));
                    }
            }
        }
    }
}
