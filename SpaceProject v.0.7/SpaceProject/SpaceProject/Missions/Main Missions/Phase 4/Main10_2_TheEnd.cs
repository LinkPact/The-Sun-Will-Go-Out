using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class Main10_2_TheEnd : Mission
    {
        private enum EventID
        {
            ToAsteroidBelt1,
            ToAsteroidBelt2,
            ToAsteroidBelt3,
            ToAsteroidBelt4,
            AtAsteroidBelt
        }

        public Main10_2_TheEnd(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();

            Planet telmun = Game.stateManager.overworldState.GetPlanet("Telmun");

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], telmun,
                new List<String>{
                    GetEvent((int)EventID.ToAsteroidBelt1).Text
                }, 3000, 3000));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], telmun,
                new List<String>{
                    GetEvent((int)EventID.ToAsteroidBelt2).Text
                }, 3000, 3000));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], telmun,
                new List<String>{
                    GetEvent((int)EventID.ToAsteroidBelt3).Text
                }, 3000, 3000));

            objectives.Add(new TimedMessageObjective(Game, this, ObjectiveDescriptions[0], telmun,
                new List<String>{
                    GetEvent((int)EventID.ToAsteroidBelt4).Text
                }, 3000, 3000));

            objectives.Add(new CloseInOnLocationObjective(Game, this, ObjectiveDescriptions[0], telmun, 600,
                new EventTextCapsule(GetEvent((int)EventID.AtAsteroidBelt), null, EventTextCanvas.MessageBox)));
        }

        public override void StartMission()
        {
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
