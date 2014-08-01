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

            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], highfence, 
                new EventTextCapsule(GetEvent((int)EventID.TalkAtHighfence), null, EventTextCanvas.BaseState)));
            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], soelara, 
                new EventTextCapsule(GetEvent((int)EventID.TalkAtSoelara), null, EventTextCanvas.BaseState)));
            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], fortrun, 
                new EventTextCapsule(GetEvent((int)EventID.TalkAtFortun), null, EventTextCanvas.BaseState)));
            objectives.Add(new ArriveAtLocationObjective(Game, this, ObjectiveDescriptions[0], telmun, 
                new EventTextCapsule(GetEvent((int)EventID.ActionAtTelmun), null, EventTextCanvas.BaseState)));
        }

        public override void StartMission()
        {
            missionHelper.ShowEvent(GetEvent((int)EventID.Introduction));
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
