using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Star: GameObjectOverworld
    {
        #region Color-fade variables
        private bool colorsSaved;
        private Color savedColor;
        #endregion

        #region Speed-scale varaibles
        private float yScale;
        private const float STRETCH = 10.5f;
        private float starAngle;
        #endregion

        private static Random rand = new Random();
        public Star(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {          
        }

        public override void Initialize()
        {
            Class = "SmallStar";
            name = "smallStar";
            layerDepth = 0.1f;
            IsUsed = true;

            if (Game.camera != null)
            {
                //Initialize star at a random position relative to camera position.
                position = new Vector2((float)rand.Next(((int)Game.camera.cameraPos.X - Game.Window.ClientBounds.Width / 2),
                (int)(Game.camera.cameraPos.X) + (Game.Window.ClientBounds.Width) / 2), (float)rand.Next((int)Game.camera.cameraPos.Y
                - (Game.Window.ClientBounds.Height) / 2, (int)(Game.camera.cameraPos.Y + (Game.Window.ClientBounds.Height / 2))));
            }

            else
            {
                //Initialize star at a random position relative to camera position.
                position = new Vector2((float)rand.Next(0,
                Game.Window.ClientBounds.Width), (float)rand.Next(0, Game.Window.ClientBounds.Height));
            }

            sprite = spriteSheet.GetSubSprite(new Rectangle(4, 4, 2, 2));
            color = new Color(rand.Next(200,255), rand.Next(155,255) , rand.Next(200,255), 255);
            scale = (float)(rand.NextDouble() * 1.5);

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            #region Color-fading
            if (Game.player.HyperspeedOn)
            {
                if (Game.player.TotalHyperspeedDistance + Game.player.TotalHyperspeedDistance > 6000)
                {
                    if (Game.player.CurrentHyperspeedDistance + Game.player.CurrentHyperspeedDistance > 3000)
                    {
                        if (!colorsSaved)
                        {
                            savedColor.B = color.B;
                            savedColor.G = color.G;
                            savedColor.R = color.R;

                            colorsSaved = true;
                        }

                        if (color.B < 255)
                            color.B++;
                        if (color.G < 255)
                            color.G++;
                        if (color.R < 255)
                            color.R++;
                    }

                    else
                    {
                        if (color.B > savedColor.B)
                            color.B--;
                        if (color.G > savedColor.G)
                            color.G--;
                        if (color.R > savedColor.R)
                            color.R--;

                        if (color.B == savedColor.B && color.G == savedColor.G && color.R == savedColor.R)
                            colorsSaved = false;
                    }
                }
            }
            #endregion

            UpdateStarPosition();
            base.Update(gameTime);
        }

        public void UpdateStarPosition()
        {
            //Star moves outside left edges of screen
            if (StaticFunctions.IsPositionOutsideScreenX(position, Game) == 1)
            {
                position = new Vector2(position.X + Game.Window.ClientBounds.Width,
                (float)rand.Next((int)Game.camera.cameraPos.Y - (Game.Window.ClientBounds.Height) / 2,
                (int)(Game.camera.cameraPos.Y + (Game.Window.ClientBounds.Height / 2))));
            }

            //Star moves outside right edges of screen
            else if (StaticFunctions.IsPositionOutsideScreenX(position, Game) == 2)
            {
                position = new Vector2(position.X - Game.Window.ClientBounds.Width,
                (float)rand.Next((int)Game.camera.cameraPos.Y - (Game.Window.ClientBounds.Height / 2),
                (int)(Game.camera.cameraPos.Y + (Game.Window.ClientBounds.Height / 2))));

            }
            //Star moves outside top edges of screen
            if (StaticFunctions.IsPositionOutsideScreenY(position, Game) == 1)
            {
                position = new Vector2((float)rand.Next((int)Game.camera.cameraPos.X
                - (Game.Window.ClientBounds.Width / 2), (int)Game.camera.cameraPos.X + (Game.Window.ClientBounds.Width / 2)),
                position.Y + Game.Window.ClientBounds.Height);
            }

            //Star moves outside bottom edges of screen
            else if (StaticFunctions.IsPositionOutsideScreenY(position, Game) == 2)
            {
                position = new Vector2((float)rand.Next((int)Game.camera.cameraPos.X
            - (Game.Window.ClientBounds.Width / 2), (int)Game.camera.cameraPos.X + (Game.Window.ClientBounds.Width / 2)),
            position.Y - Game.Window.ClientBounds.Height);
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool hyperSpeedOn, float speed, Direction direction)
        {
            if (IsUsed == true)
            {
                if (!hyperSpeedOn)
                    spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                        0f, centerPoint, scale, SpriteEffects.None, layerDepth);
                else
                {
                    starAngle = (float)((Math.PI * 90) / 180) + (float)(GlobalFunctions.RadiansFromDir(direction.GetDirectionAsVector()));

                    yScale = scale + speed * STRETCH;

                    if (yScale < scale)
                        yScale = scale;

                    spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                        starAngle, centerPoint, new Vector2(scale, yScale), SpriteEffects.None, layerDepth);
                }
            }
        }
    }
}
