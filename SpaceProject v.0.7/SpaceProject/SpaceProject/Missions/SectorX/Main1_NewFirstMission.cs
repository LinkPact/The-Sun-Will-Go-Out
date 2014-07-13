using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class NewFirstMission: Mission
    {
        private enum EventID
        {
            
        }

        public NewFirstMission(Game1 Game, string section, Sprite spriteSheet) :
            base(Game, section, spriteSheet)
        {
        }

        public override void Initialize()
        {
            base.Initialize();
        }

        public override void StartMission()
        {
            ObjectiveIndex = 0;
            progress = 0;

            missionHelper.ShowEvent(EventList[0].Key);
        }

        public override void OnLoad()
        {
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
