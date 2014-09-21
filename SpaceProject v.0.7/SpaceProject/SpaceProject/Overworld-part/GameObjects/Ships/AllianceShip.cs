using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /// <summary>
    /// 
    /// </summary>
    public class AllianceShip : OverworldShip
    {
        public AllianceShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        { }

        public override void Initialize()
        {
            Class = "RebelShip";
            name = "Rebel Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(435, 80, 25, 26));
            viewRadius = 700;
            position = new Vector2(0, 0);
            speed = 0.4f;
            target = Game.player;

            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2, sprite.SourceRectangle.Value.Height / 2);
            color = Color.White;
            scale = 1.0f;
            layerDepth = 0.6f;

            base.Initialize();
        }

        public void Initialize(Sector sec)
        {
            Initialize();

            sector = sec;
            SetPositionInSector();
            destination = new Vector2(position.X + 100, position.Y + 100);
        }

        public void SetPositionInSector()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            position = new Vector2(
                (int)r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                (int)r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            //if (CollisionDetection.IsPointInsideRectangle(position, Game.stateManager.overWorldState.HUD.radar.View))
            //    SetPositionInSector();
        }
        public override void FinalGoodbye()
        {
            IsDead = true;
        }

        public override void Update(GameTime gameTime)
        {
            // Select target
            //if (CollisionDetection.IsPointInsideRectangle(target.position, view))
            //{
            //    destination = target.position;
            //}
            //else if (roam == true && sector != null)
            //{
            //    if (CollisionDetection.IsPointInsideRectangle(destination, Bounds))
            //    {
            //        Random r = new Random(DateTime.Now.Millisecond);
            //        destination = new Vector2(
            //            r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
            //            r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            //    }
            //}
            //else if (roam == false)
            //    destination = Vector2.Zero;

            // Rotate ship
            angle = (float)(MathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI) / 2 + Math.PI);

            if (IsUsed)
            {
                base.Update(gameTime);
            }

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            if (IsUsed)
                base.Draw(spriteBatch);
        }

    }
}
