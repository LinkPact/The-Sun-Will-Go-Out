using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    class OrbitAction : ShipAction
    {
        OverworldShip ship;
        GameObjectOverworld target;

        private float centerX;
        private float centerY;
        private float radius;
        private float orbitSpeed;
        private float speedScale;
        private float angleMoon;

        public OrbitAction(OverworldShip ship, GameObjectOverworld target)
        {
            this.ship = ship;
            this.target = target;

            centerX = 243063;
            centerY = 252133;
            radius = 600f;
            orbitSpeed = 600f;
            speedScale = (0.001f * 2 * (float)Math.PI) / orbitSpeed;
        }

        public override void Update(GameTime gameTime)
        {
            angleMoon = (float)gameTime.TotalGameTime.TotalMilliseconds * speedScale;

            ship.angle += (float)Math.PI * 0.01f / 180;

            double xCoord = centerX + Math.Sin(ship.angle) * radius;
            double yCoord = centerY + Math.Cos(ship.angle) * radius;

            ship.position = new Vector2((float)xCoord, (float)yCoord);
        }
    }
}
