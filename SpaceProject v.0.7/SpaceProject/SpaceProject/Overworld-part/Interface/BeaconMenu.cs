using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class BeaconMenu
    {
        private Game1 game;
        private Sprite sprite;
        private SpriteFont font;

        private Vector2 position;

        private List<String> options;
        private int cursorIndex;

        private bool displayed;
        private int tempTimer;

        private List<Beacon> beacons;
        private Beacon currentBeacon;

        public BeaconMenu(Game1 game, Sprite sprite)
        {
            this.game = game;
            this.sprite = sprite.GetSubSprite(new Rectangle(0, 0, 571, 310));
        }

        public void Initialize()
        {
            font = game.fontManager.GetFont(14);
            beacons = new List<Beacon>();
            options = new List<String>();
            position = Vector2.Zero;
        }

        public void AddBeacon(Beacon beacon)
        {
            if (!beacons.Contains(beacon))
            {
                if (options.Contains("Back"))
                {
                    options.Remove("Back");
                }

                beacons.Add(beacon);
                options.Add(beacon.name);
                options.Add("Back");
            }
        }

        public void Display(Beacon currentBeacon)
        {
            this.currentBeacon = currentBeacon;
            Game1.Paused = true;
            displayed = true;
            tempTimer = 50;
        }

        public void Hide()
        {
            Game1.Paused = false;
            displayed = false;
        }

        public void Update(GameTime gameTime)
        {
            tempTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (displayed && tempTimer <= 0)
            {
                if (ControlManager.CheckPress(RebindableKeys.Down))
                {
                    cursorIndex++;
                }

                else if (ControlManager.CheckPress(RebindableKeys.Up))
                {
                    cursorIndex--;
                }

                if (cursorIndex > options.Count - 1)
                {
                    cursorIndex = 0;
                }

                else if (cursorIndex < 0)
                {
                    cursorIndex = options.Count - 1;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter))
                {
                    if (cursorIndex < beacons.Count)
                    {
                        currentBeacon.StartJump(beacons[cursorIndex]);
                    }

                    Hide();
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (displayed)
            {
                spriteBatch.Draw(sprite.Texture, game.camera.cameraPos, sprite.SourceRectangle, Color.White, 0.0f,
                        new Vector2(sprite.Width / 2, sprite.Height / 2), 1f, SpriteEffects.None, 0.9f);

                for (int i = 0; i < options.Count; i++)
                {
                    Color color = Color.White;

                    if (i == cursorIndex)
                    {
                        color = Color.Red;
                    }

                    spriteBatch.DrawString(font, options[i], new Vector2(game.camera.cameraPos.X + position.X + 120,
                                                                         game.camera.cameraPos.Y + position.Y - 100 + (i * 20)),
                            color, 0.0f,
                            Vector2.Zero, 1f, SpriteEffects.None, 0.95f);
                }
            }
        }
    }
}
