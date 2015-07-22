using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class HyperSpeedParticle : Particle
    {
        private readonly float ScaleStretch = 4f;

        public HyperSpeedParticle(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        {

        }

        public override void Initialize(GameObjectOverworld obj, Vector2 startingPoint)
        {
            base.Initialize(obj, startingPoint);

            speed = obj.speed * 0.65f;
            color = new Color(255, 255, 255, 255);         
        }


        public override void Update(GameTime gameTime, GameObjectOverworld obj)
        {
            base.Update(gameTime, obj);

            color = GetFadingBlueFireColor(gameTime, color) * opacity;
            yScale = scale + speed * ScaleStretch;
            parAngle = (float)((Math.PI * 90) / 180) + (float)(MathFunctions.RadiansFromDir(Direction.GetDirectionAsVector()));
        }

        private static Color GetFadingBlueFireColor(GameTime gameTime, Color initialColor)
        {
            Color resultingColor = initialColor;

            int redFadeRate = 30;
            int greenFadeRate = 30;
            int blueFadeRate = 15;

            resultingColor.R = UpdateSingleColor(gameTime, initialColor.R, redFadeRate);
            resultingColor.G = UpdateSingleColor(gameTime, initialColor.G, greenFadeRate);
            resultingColor.B = UpdateSingleColor(gameTime, initialColor.B, blueFadeRate);

            return resultingColor;
        }
    }
}
