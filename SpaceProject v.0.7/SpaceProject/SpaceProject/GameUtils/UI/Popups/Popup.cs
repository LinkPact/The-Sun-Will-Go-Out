using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    enum PopupState
    {
        Hidden,
        Showing,
        Finished
    }

    class Popup
    {
        private readonly int Opacity = 230;
        private readonly float LayerDepth = 0.95f;

        protected Game1 game;
        protected PopupState popupState;
        public PopupState PopupState { get { return popupState; } protected set { ;} }

        protected Sprite canvas;
        protected Vector2 canvasPosition;
        protected float canvasScale;

        private float popupTime;
        private Sprite selectedButton;

        protected int tempTimer;

        protected bool usePause;

        protected Popup(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            selectedButton = spriteSheet.GetSubSprite(new Rectangle(180, 0, 66, 21));
        }

        public virtual void Initialize()
        {
            // Sets position of canvas and text
            if (GameStateManager.currentState == "OverworldState")
            {
                canvasPosition = new Vector2(game.camera.cameraPos.X, game.camera.cameraPos.Y);
            }
            else
            {
                canvasPosition = new Vector2(game.Window.ClientBounds.Width / 2, game.Window.ClientBounds.Height / 2);
            }
            canvasScale = 1;
            tempTimer = 50;
            popupState = PopupState.Hidden;
        }

        public virtual void Update(GameTime gameTime)
        {
            tempTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (popupTime != -1
                && StatsManager.PlayTime.HasPlayTimePassed(popupTime))
            {
                Show();
                popupTime = -1;
            }

            if (tempTimer < 0)
            {
                ButtonControls();
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            DrawCanvas(spriteBatch);
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

        public virtual void OnPress(RebindableKeys key)
        {
            if (key == RebindableKeys.Action1)
            {
                Hide();
            }
        }

        public void SetDelay(float milliseconds)
        {
            popupTime = StatsManager.PlayTime.GetFuturePlayTime(milliseconds);
        }

        private void DrawCanvas(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(canvas.Texture,
                 canvasPosition,
                 canvas.SourceRectangle,
                 new Color(255, 255, 255, Opacity),
                 0.0f,
                 new Vector2(canvas.SourceRectangle.Value.Width / 2,
                             canvas.SourceRectangle.Value.Height / 2),
                 canvasScale,
                 SpriteEffects.None,
                 LayerDepth);
        }

        private void DrawOkayButton(SpriteBatch spriteBatch)
        {
            // Draws "okay"-button
            spriteBatch.Draw(selectedButton.Texture,
                new Vector2(canvasPosition.X, canvasPosition.Y),
                selectedButton.SourceRectangle,
                Color.White,
                0f,
                new Vector2(selectedButton.SourceRectangle.Value.Width / 2,
                    selectedButton.SourceRectangle.Value.Height / 2),
                1f,
                SpriteEffects.None,
                0.975f);

            // Draws "okay"-text
            spriteBatch.DrawString(game.fontManager.GetFont(14),
                 "Okay",
                 new Vector2(canvasPosition.X,
                         canvasPosition.Y) + game.fontManager.FontOffset,
                 Color.LightBlue,
                 0f,
                 game.fontManager.GetFont(14).MeasureString("Okay") / 2,
                 1f,
                 SpriteEffects.None,
                 1f);
        }

        protected virtual void Hide()
        {
            if (usePause)
            {
                Game1.Paused = false;
                popupState = PopupState.Finished;
            }
        }

        private void ButtonControls()
        {
            if (ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeypress(Keys.Enter))
            {
                OnPress(RebindableKeys.Action1);
            }
            else if (ControlManager.CheckPress(RebindableKeys.Pause)
                || ControlManager.CheckKeypress(Keys.Escape))
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
        }
    }
}
