using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MainX1_BeginningOfTheEnd : Mission
    {
        private enum EventID
        {
            Introduction,
            TalkAtHighfence,
            TalkAtSoelara,
            TalkAtFortun,
            ActionAtTelmun
        }

        private Boolean firstRebelTriggered = false;
        private Boolean secondRebelTriggered = false;

        private RebelShip rebel1;
        private RebelShip rebel2;

        public MainX1_BeginningOfTheEnd(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Planet highfence = Game.stateManager.overworldState.GetPlanet("Highfence");
            Station soelara = Game.stateManager.overworldState.GetStation("Soelara Station");
            Station fortrun = Game.stateManager.overworldState.GetStation("Fotrun Station I");
            Planet telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            rebel1 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel1.Initialize();
            rebel1.SetPosition(MathFunctions.SpreadPos(highfence.position, 200));
            rebel1.Level = "PirateLevel1";

            rebel2 = new RebelShip(Game, Game.spriteSheetVerticalShooter);
            rebel2.Initialize();
            rebel2.SetPosition(MathFunctions.SpreadPos(highfence.position, 200));
            rebel2.Level = "PirateLevel2";

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], highfence, 
                new EventTextCapsule(GetEvent((int)EventID.TalkAtHighfence), null, EventTextCanvas.BaseState)));
            //objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], soelara, 
            //    new EventTextCapsule(GetEvent((int)EventID.TalkAtSoelara), null, EventTextCanvas.BaseState)));
            //objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], fortrun, 
            //    new EventTextCapsule(GetEvent((int)EventID.TalkAtFortun), null, EventTextCanvas.BaseState)));
            //objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], telmun, 
            //    new EventTextCapsule(GetEvent((int)EventID.ActionAtTelmun), null, EventTextCanvas.BaseState)));
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));

            Game.stateManager.overworldState.AddOverworldObject(rebel1);
            Game.stateManager.overworldState.AddOverworldObject(rebel2);
        }

        public override void OnLoad()
        {
         
        }

        public override void OnReset()
        {
            base.OnReset();

            ObjectiveIndex = 0;

            for (int i = 0; i < objectives.Count; i++)
            {
                objectives[i].Reset();
            }
        }

        public override void MissionLogic()
        {
            base.MissionLogic();

            if (!firstRebelTriggered && CollisionDetection.IsRectInRect(Game.player.Bounds, rebel1.Bounds))
            {
                Game.stateManager.overworldState.RemoveOverworldObject(rebel1);
                Game.messageBox.DisplayMessage("You should have stayed in the warm comfort of you home planet1.");
                Game.stateManager.shooterState.BeginLevel(rebel1.Level);
                firstRebelTriggered = true;
            }

            if (!secondRebelTriggered && CollisionDetection.IsRectInRect(Game.player.Bounds, rebel2.Bounds))
            {
                Game.stateManager.overworldState.RemoveOverworldObject(rebel2);
                Game.messageBox.DisplayMessage("You should have stayed in the warm comfort of you home planet2.");
                Game.stateManager.shooterState.BeginLevel(rebel2.Level);
                secondRebelTriggered = true;
            }

            if (!firstRebelTriggered && !secondRebelTriggered)
            { 
            
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override int GetProgress()
        {
            return progress;
        }

        public override void SetProgress(int progress)
        {
            this.progress = progress;
        }
    }
}
