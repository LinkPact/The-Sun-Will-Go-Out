using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class IntroFirstState : GameState
    {
        private ConfigFile configStory;
        private SpriteFont spriteFont;

        private Texture2D backdrop;
        private int timeInIntro;
        private int introtimer;
        private string introText;

        public IntroFirstState(Game1 Game, string name) :
            base(Game, name)
        {
            configStory = new ConfigFile();
            spriteFont = Game.fontManager.GetFont(14);
        }

        public override void Initialize()
        {
            base.Initialize();

            configStory.Load("Data/storydata.dat");

            backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");

            timeInIntro = 13000;

            string tempString = configStory.GetPropertyAsString("introfirst", "introfirst1", "");
            string tempString2 = configStory.GetPropertyAsString("introfirst", "introfirst2", "");

            introText = tempString + "\n\n" + tempString2;

            ActiveSong = Music.TheOboeSong;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            introtimer = 0;
        }

        public override void OnLeave()
        {
        }

        public override void Update(GameTime gameTime)
        {
            #region Input

            if (ControlManager.GamepadReady == false)
            {

                if (ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter))
                {
                    Game.stateManager.ChangeState("IntroSecondState");
                }
            }

            if (ControlManager.GamepadReady == true)
            {

                if (ControlManager.CurrentGamepadState.IsButtonDown(ControlManager.GamepadAction))
                {
                    Game.stateManager.ChangeState("IntroSecondState");
                }
            }

            #endregion

            introtimer += gameTime.ElapsedGameTime.Milliseconds;

            if (introtimer > timeInIntro)
                Game.stateManager.ChangeState("IntroSecondState");

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backdrop, Vector2.Zero, null, Color.White, 0.0f, Vector2.Zero,
                new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                    Game.Window.ClientBounds.Height / Game.DefaultResolution.Y), SpriteEffects.None, 0.5f);

            spriteBatch.DrawString(spriteFont, introText, new Vector2(Game.Window.ClientBounds.Width / 2,
                Game.Window.ClientBounds.Height / 2), Color.White, 0f, spriteFont.MeasureString(introText) / 2, 1f,
                SpriteEffects.None, .5f);

            Game.helper.DisplayText("Press 'Enter' to skip");
                    
        }
    }
}
