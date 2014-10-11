using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class IntroSecondState : GameState
    {
        private static Random random = new Random();
        private TextBox textBox;
        private SpriteFont spriteFont;
        private Sprite contrastBackdrop;

        private float txtSpeed;
        private const float txtMaxSpeed = 0.95f;
        private const float txtMinSpeed = 0.25f;
        //private Texture2D backdrop;

        private Sprite starSprite;
        private float starSpeed;
        private Vector2[] starPositions;
        private float starAngle;
        private Vector2 starScale;

        private Sprite spriteSheet;
        private Sprite introMercSprite;

        private Sprite txtBackdrop;

        public IntroSecondState(Game1 Game, string name) :
            base(Game, name)
        {
            spriteFont = Game.fontManager.GetFont(14);
            textBox = TextUtils.CreateTextBoxConfig(spriteFont,
                                                    new Rectangle(10,
                                                                  Game.Window.ClientBounds.Height,
                                                                  Game.Window.ClientBounds.Width - 20,
                                                                  Game.Window.ClientBounds.Height - 20),
                                                    "Data/storydata.dat",
                                                    false);
        }

        public override void Initialize()
        {
            txtSpeed = 0.15f;
            base.Initialize();
            //backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/smallObjectSpriteSheet"), null);
            introMercSprite = spriteSheet.GetSubSprite(new Rectangle(78, 0, 29, 27));
            contrastBackdrop = spriteSheet.GetSubSprite(new Rectangle(0, 0, 1, 1));
            starSprite = spriteSheet.GetSubSprite(new Rectangle(0, 32, 2, 2));
            starAngle = (float)((Math.PI * 90) / 180) ;
            starScale = new Vector2(1f, 40f);
            starSpeed = 3;

            starPositions = new Vector2[100];
            for (int i = 0; i < starPositions.Length; i++)
                starPositions[i] = new Vector2(random.Next(0, Game.Window.ClientBounds.Width),
                                               random.Next(0, Game.Window.ClientBounds.Height));


            List<string> tempList = new List<string>();

            tempList.Add("introsecond1");
            tempList.Add("introsecond2");
            tempList.Add("introsecond3");
            tempList.Add("introsecond4");

            textBox.LoadFormattedText("introsecond",
                                     tempList,
                                     null);

            txtBackdrop = new Sprite(spriteSheet.Texture, new Nullable<Rectangle>(new Rectangle(6, 1, 1, 1)));
        }

        public override void OnEnter() { }

        public override void OnLeave() { }

        public override void Update(GameTime gameTime)
        {
            //Game.Window.Title = ("SpaceExplorationGame - " + "Intro");

            #region Input

            if (ControlManager.CheckHold(RebindableKeys.Up))
            {
                txtSpeed += 0.01f;
            }

            else
                txtSpeed -= 0.01f;

            if (ControlManager.GamepadReady == false)
            {

                if (ControlManager.CheckPress(RebindableKeys.Action1) ||
                    ControlManager.CheckKeypress(Keys.Enter))
                {
                    Game.stateManager.ChangeState("OverworldState");
                }
            }

            if (ControlManager.GamepadReady == true)
            {

                if (ControlManager.CurrentGamepadState.IsButtonDown(ControlManager.GamepadAction))
                {
                    Game.stateManager.ChangeState("OverworldState");
                }
            }

            #endregion

            textBox.TextBoxPosY -= txtSpeed;

            if (txtSpeed > txtMaxSpeed)
                txtSpeed = txtMaxSpeed;

            else if (txtSpeed < txtMinSpeed)
                txtSpeed = txtMinSpeed;

            if (textBox.TextBoxPosY < 200)
                Game.stateManager.ChangeState("OverworldState");

            for (int i = 0; i < starPositions.Length; i++)
            {
                starPositions[i].X += starSpeed * gameTime.ElapsedGameTime.Milliseconds;

                int returnValue = StaticFunctions.IsPositionOutsideScreenX(starPositions[i], Game);

                if (returnValue == 1)
                {
                }

                else if (returnValue == 2)
                {
                    starPositions[i] = new Vector2(-30, (float)random.Next(0, Game.Window.ClientBounds.Height));
                }
            }

        }

        //private const float widthMultiplier = 0.5f;
        public override void Draw(SpriteBatch spriteBatch)
        {

            foreach (Vector2 v in starPositions)
            {
                if (v.Y > Game.Window.ClientBounds.Height * 2 / 3)
                {
                    spriteBatch.Draw(starSprite.Texture, v, starSprite.SourceRectangle,
                        Color.White, -starAngle, Vector2.Zero, starScale, SpriteEffects.None, 0.2f);
                }

                else
                {
                    spriteBatch.Draw(starSprite.Texture, v, starSprite.SourceRectangle,
                        Color.White, -starAngle, Vector2.Zero, starScale, SpriteEffects.None, 0.45f);
                }
            }

            spriteBatch.Draw(contrastBackdrop.Texture, new Vector2(5, Game.Window.ClientBounds.Height * 2/3),
                contrastBackdrop.SourceRectangle, Color.Black, 0.0f, Vector2.Zero,
                new Vector2(Game.Window.ClientBounds.Width - 10, Game.Window.ClientBounds.Height / 3 - 5),
                SpriteEffects.None, 0.25f);

            textBox.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset, 0.3f);

            spriteBatch.Draw(txtBackdrop.Texture, new Vector2(0, 0), txtBackdrop.SourceRectangle, new Color(0, 0, 0, 255),
                0f, Vector2.Zero, new Vector2(Game.Window.ClientBounds.Width, Game.Window.ClientBounds.Height * 2 / 3),
                SpriteEffects.None, 0.4f);

            // Player ship
            spriteBatch.Draw(introMercSprite.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 2,
                                        Game.Window.ClientBounds.Height / 2),
                             introMercSprite.SourceRectangle, Color.White, (float)(Math.PI / 180 * 270),
                             new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
                                         introMercSprite.SourceRectangle.Value.Height / 2),
                             1.0f, SpriteEffects.None, .5f);

            //// Merc ships
            //spriteBatch.Draw(introMercSprite.Texture,
            //                 new Vector2(Game.Window.ClientBounds.Width * widthMultiplier - 50,
            //                            Game.Window.ClientBounds.Height / 2 - 40),
            //                 introMercSprite.SourceRectangle, Color.White, (float)Math.PI / 2,
            //                 new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
            //                             introMercSprite.SourceRectangle.Value.Height / 2),
            //                 1.0f, SpriteEffects.None, .5f);
            //
            //spriteBatch.Draw(introMercSprite.Texture,
            //                 new Vector2(Game.Window.ClientBounds.Width * widthMultiplier - 50,
            //                            Game.Window.ClientBounds.Height / 2 + 40),
            //                 introMercSprite.SourceRectangle, Color.White, (float)Math.PI / 2,
            //                 new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
            //                             introMercSprite.SourceRectangle.Value.Height / 2),
            //                 1.0f, SpriteEffects.None, .5f);
            //
            //spriteBatch.Draw(introMercSprite.Texture,
            //                 new Vector2(Game.Window.ClientBounds.Width * widthMultiplier - 100,
            //                            Game.Window.ClientBounds.Height / 2 - 80),
            //                 introMercSprite.SourceRectangle, Color.White, (float)Math.PI / 2,
            //                 new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
            //                             introMercSprite.SourceRectangle.Value.Height / 2),
            //                 1.0f, SpriteEffects.None, .5f);
            //
            //spriteBatch.Draw(introMercSprite.Texture,
            //                 new Vector2(Game.Window.ClientBounds.Width * widthMultiplier - 100,
            //                            Game.Window.ClientBounds.Height / 2),
            //                 introMercSprite.SourceRectangle, Color.White, (float)Math.PI / 2,
            //                 new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
            //                             introMercSprite.SourceRectangle.Value.Height / 2),
            //                 1.0f, SpriteEffects.None, .5f);
            //
            //spriteBatch.Draw(introMercSprite.Texture,
            //                 new Vector2(Game.Window.ClientBounds.Width * widthMultiplier - 100,
            //                            Game.Window.ClientBounds.Height / 2 + 80),
            //                 introMercSprite.SourceRectangle, Color.White, (float)Math.PI / 2,
            //                 new Vector2(introMercSprite.SourceRectangle.Value.Width / 2,
            //                             introMercSprite.SourceRectangle.Value.Height / 2),
            //                 1.0f, SpriteEffects.None, .5f);

        }

    }
}
