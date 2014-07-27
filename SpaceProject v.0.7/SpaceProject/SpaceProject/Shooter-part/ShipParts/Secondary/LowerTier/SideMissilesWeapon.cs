using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class SideMissilesWeapon : PlayerWeapon
    {

        public SideMissilesWeapon(Game1 Game) :
            base(Game)
        {
            Setup();
        }

        public SideMissilesWeapon(Game1 Game, ItemVariety variety) :
            base(Game, variety)
        {
            Setup();
        }

        protected override String GetDescription()
        {
            return "Shoots four short range missiles protecting the flanks of the ship";
        }

        private void Setup()
        {
            Name = "SideMissiles";
            Kind = "Secondary";
            energyCostPerSecond = 1f;
            delay = 700;
            Weight = 500;

            bullet = new RegularMissile(Game, spriteSheet);
            bullet.Initialize();

            damage = Bullet.Damage;
            duration = Bullet.Duration;
            speed = Bullet.Speed;

            Value = 200;
        }

        public override Boolean Activate(PlayerVerticalShooter player, GameTime gameTime)
        {
            RegularMissile missile1 = new RegularMissile(Game, spriteSheet);
            //Position
            missile1.PositionX = player.PositionX - 2;
            missile1.PositionY = player.PositionY;
            //Direction
            missile1.Direction = MathFunctions.DirFromRadians(-Math.PI / 2 - (2 * Math.PI / 16));
            missile1.Radians = MathFunctions.RadiansFromDir(missile1.Direction);
            //Initialize
            missile1.Initialize();

            RegularMissile missile2 = new RegularMissile(Game, spriteSheet);
            missile2.PositionX = player.PositionX - 4;
            missile2.PositionY = player.PositionY;
            missile2.Direction = MathFunctions.DirFromRadians(-Math.PI / 2 - (3 * Math.PI / 16));
            missile2.Radians = MathFunctions.RadiansFromDir(missile2.Direction);
            missile2.Initialize();

            RegularMissile missile3 = new RegularMissile(Game, spriteSheet);
            missile3.PositionX = player.PositionX + 2;
            missile3.PositionY = player.PositionY;
            missile3.Direction = MathFunctions.DirFromRadians(-Math.PI / 2 + (2 * Math.PI / 16));
            missile3.Radians = MathFunctions.RadiansFromDir(missile3.Direction);
            missile3.Initialize();

            RegularMissile missile4 = new RegularMissile(Game, spriteSheet);
            missile4.PositionX = player.PositionX + 4;
            missile4.PositionY = player.PositionY;
            missile4.Direction = MathFunctions.DirFromRadians(-Math.PI / 2 + (3 * Math.PI / 16));
            missile4.Radians = MathFunctions.RadiansFromDir(missile4.Direction);
            missile4.Initialize();

            //Adds created bullets to list with active objects.
            Game.stateManager.shooterState.gameObjects.Add(missile1);
            Game.stateManager.shooterState.gameObjects.Add(missile2);
            Game.stateManager.shooterState.gameObjects.Add(missile3);
            Game.stateManager.shooterState.gameObjects.Add(missile4);

            return true;
        }
    }
}
