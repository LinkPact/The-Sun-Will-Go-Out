using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /// <summary>
    /// Instance of one of the ships that populates the overworld. 
    /// </summary>
    class PirateShip : OverworldShip
    {
        public PirateShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        { }

        public override void Initialize()
        {
            name = "Pirate Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(182, 29, 27, 32));
            viewRadius = 3000;
            position = new Vector2(0,0);
            speed = 0.35f;

            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2, sprite.SourceRectangle.Value.Height / 2);
            color = Color.White;
            scale = 1.0f;
            layerDepth = 0.6f;
            SetDefaultBehavior();

            base.Initialize();
        }

        public void Initialize(Sector sec)
        {
            Initialize();

            sector = sec;
            SetPositionInSector();
            destination = new Vector2(position.X + 100, position.Y + 100);
        }

        public void SetDefaultBehavior()
        {
               PriorityAction tmpAction = new PriorityAction();
            tmpAction.Add(new FollowInViewAction(this, Game.player));
            tmpAction.Add(new PatrolAction(this, Game.stateManager.overworldState.GetSectorX));
            AIManager = tmpAction;
            collisionEvent = new PirateColllisionEvent(Game, this, Game.player);
        }

        public void SetPositionInSector()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            position = new Vector2(
                (int)r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                (int)r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
        }
        
        public override void FinalGoodbye()
        {
            IsDead = true;
        }

        public override void Update(GameTime gameTime)
        {          
            // Select target
            //if (FollowPlayer && CollisionDetection.IsPointInsideRectangle(Game.player.position, view))
            //{
            //    destination = Game.player.position;
            //}
            //else if(roam == true && sector != null)
            //{
            //    if (CollisionDetection.IsPointInsideRectangle(destination, Bounds))
            //    {                
            //        Random r = new Random(DateTime.Now.Millisecond);
            //        destination = new Vector2(
            //            r.Next(sector.SpaceRegionArea.Left , sector.SpaceRegionArea.Right), 
            //            r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            //    }
            //}
            //else if(roam == false)
            //    destination = Vector2.Zero;

            angle = (float)(MathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI) / 2 + Math.PI);

            base.Update(gameTime);

        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
               base.Draw(spriteBatch);
        }

    }
}
