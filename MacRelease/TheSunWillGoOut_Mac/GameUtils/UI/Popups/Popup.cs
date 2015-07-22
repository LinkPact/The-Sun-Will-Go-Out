using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject_Mac
{
    enum PopupState
    {
        Hidden,
        Showing,
        Finished
    }

    class Popup
    {
        public static int delayTimer;

        // constants
        private readonly int CanvasOpacity = 230;
        private readonly float LayerDepth = 0.95f;
        protected readonly int PressDelay = 50;
        protected readonly float OkayButtonYArea = 41;

        // variables
        protected Game1 game;
        protected PopupState popupState;

        protected Sprite canvas;
        protected Vector2 canvasPosition;
        protected Vector2 canvasOrigin;
        protected Vector2 canvasScale;

        protected bool usePause;
        protected float popupTime;

        protected bool useOkayButton;
        private Sprite okayButton;
        protected Vector2 okayButtonPosition;

        // properties
        public PopupState PopupState { get { return popupState; } protected set { ;} }

        protected Popup(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            okayButton = spriteSheet.GetSubSprite(new Rectangle(473, 262, 66, 21));
        }

        public virtual void Initialize()
        {
            InitializePositions();
            useOkayButton = true;
            popupState = PopupState.Hidden;
            canvasOrigin = new Vector2(canvas.SourceRectangle.Value.Width / 2,
                                       canvas.SourceRectangle.Value.Height / 2);
            canvasScale = new Vector2(1, 1);
        }

        public virtual void Update(GameTime gameTime)
        {
            delayTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (popupTime != -1
                && StatsManager.PlayTime.HasPlayTimePassed(popupTime))
            {
                Show();
                popupTime = -1;
            }

            if (delayTimer < 0)
            {
                ButtonControls();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawCanvas(spriteBatch);

            if (useOkayButton)
                DrawOkayButton(spriteBatch);
        }

        public virtual void Show()
        {
            popupState = PopupState.Showing;

            if (usePause)
            {
                Game1.Paused = true;
            }
        }

        public virtual void SetDelay(float milliseconds)
        {
            popupTime = StatsManager.PlayTime.GetFuturePlayTime(milliseconds);
        }

        protected virtual void OnPress(RebindableKeys key)
        {
            if (key == RebindableKeys.Action1)
            {
                Hide();
            }
        }

        protected virtual void Hide()
        {
            if (usePause)
            {
                Game1.Paused = false;
            }

            popupState = PopupState.Finished;
        }

        protected virtual void InitializePositions()
        {
            // Sets position of canvas
            if (GameStateManager.currentState == "OverworldState")
            {
                canvasPosition = new Vector2(game.camera.cameraPos.X, game.camera.cameraPos.Y);
            }
            else
            {
                canvasPosition = new Vector2(Game1.ScreenSize.X / 2, Game1.ScreenSize.Y / 2);
            }

            // Sets position of button
            okayButtonPosition = new Vector2(canvasPosition.X,
                canvasPosition.Y + canvas.SourceRectangle.Value.Height / 2
                - OkayButtonYArea / 2);
        }

        private void DrawCanvas(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(canvas.Texture,
                 canvasPosition,
                 canvas.SourceRectangle,
                 new Color(255, 255, 255, CanvasOpacity),
                 0.0f,
                 canvasOrigin,
                 canvasScale,
                 SpriteEffects.None,
                 LayerDepth);
        }

        private void DrawOkayButton(SpriteBatch spriteBatch)
        {
            // Draws "okay"-button
            spriteBatch.Draw(okayButton.Texture,
                okayButtonPosition,
                okayButton.SourceRectangle,
                Color.White,
                0f,
                new Vector2(okayButton.SourceRectangle.Value.Width / 2,
                    okayButton.SourceRectangle.Value.Height / 2),
                1f,
                SpriteEffects.None,
                0.975f);

            // Draws "okay"-text
            spriteBatch.DrawString(FontManager.GetFontStatic(14),
                 "Okay",
                 okayButtonPosition + FontManager.FontOffsetStatic,
                 FontManager.FontSelectColor2,
                 0f,
                 FontManager.GetFontStatic(14).MeasureString("Okay") / 2,
                 1f,
                 SpriteEffects.None,
                 1f);
        }

        private void ButtonControls()
        {
            if (ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeyPress(Keys.Enter))
            {
                OnPress(RebindableKeys.Action1);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Action2))
            {
                OnPress(RebindableKeys.Action2);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Pause)
                || ControlManager.CheckKeyPress(Keys.Escape))
            {
                OnPress(RebindableKeys.Pause);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Right))
            {
                OnPress(RebindableKeys.Right);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Left))
            {
                OnPress(RebindableKeys.Left);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Up))
            {
                OnPress(RebindableKeys.Up);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Down))
            {
                OnPress(RebindableKeys.Down);
            }
            else if (ControlManager.IsLeftMouseButtonClicked())
            {
                OnPress(RebindableKeys.Action1);
            }
        }
    }
}
