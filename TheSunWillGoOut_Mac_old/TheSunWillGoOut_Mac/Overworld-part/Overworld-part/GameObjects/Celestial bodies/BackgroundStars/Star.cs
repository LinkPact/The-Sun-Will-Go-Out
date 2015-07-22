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
        protected float speedMod;
        #endregion

        public Star(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {          
        }

        public override void Initialize()
        {
            Class = "SmallStar";
            name = "smallStar";
            layerDepth = 0.1f;

            if (Game.camera != null)
            {
                //Initialize star at a random position relative to camera position.
                position = new Vector2((float)Game.random.Next(((int)Game.camera.cameraPos.X - Game1.ScreenSize.X / 2),
                (int)(Game.camera.cameraPos.X) + (Game1.ScreenSize.X) / 2), (float)Game.random.Next((int)Game.camera.cameraPos.Y
                - (Game1.ScreenSize.Y) / 2, (int)(Game.camera.cameraPos.Y + (Game1.ScreenSize.Y / 2))));
            }

            else
            {
                //Initialize star at a random position relative to window position.
                position = new Vector2((float)Game.random.Next(0,
                Game1.ScreenSize.X), (float)Game.random.Next(0, Game1.ScreenSize.Y));
            }

            sprite = spriteSheet.GetSubSprite(new Rectangle(4, 4, 2, 2));
            color = new Color(Game.random.Next(200, 255), Game.random.Next(155, 255), Game.random.Next(200, 255), 255);
            scale = (float)(Game.random.NextDouble() * 1.5);
            speedMod = Game.random.Next(4, 10) / 10f;

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

            UpdateStarPosition(gameTime);
            base.Update(gameTime);
        }

        public void UpdateStarPosition(GameTime gameTime)
        {
            //Update star position
            position += ((Game.player.speed * speedMod)  * Game.player.Direction.GetDirectionAsVector()) * gameTime.ElapsedGameTime.Milliseconds;

            if (StaticFunctions.IsPositionOutsideScreen(position, Game))
            {
                position = StaticFunctions.GetCoordinateInsideScreen(position, Game);
            }
        }

        public void Draw(SpriteBatch spriteBatch, bool hyperSpeedOn, float speed, Direction direction)
        {
            if (!hyperSpeedOn)
            {
                spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                    0f, centerPoint, scale, SpriteEffects.None, layerDepth);
            }
            else
            {
                starAngle = (float)((Math.PI * 90) / 180) + (float)(MathFunctions.RadiansFromDir(direction.GetDirectionAsVector()));

                yScale = scale + speed * STRETCH;

                if (yScale < scale)
                {
                    yScale = scale;
                }

                spriteBatch.Draw(sprite.Texture, position, sprite.SourceRectangle, color,
                    starAngle, centerPoint, new Vector2(scale, yScale), SpriteEffects.None, layerDepth);
            }
        }
    }
}
