using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace SpaceProject_Mac
{
    class HangarShip : OverworldShip
    {
        public HangarShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        { }

        public override void Initialize()
        {
            name = "Hangar Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(0, 380, 159, 258));
            viewRadius = 3000;
            position = new Vector2(0,0);
            speed = 0.15f;
            

            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2, sprite.SourceRectangle.Value.Height / 2);
            color = Color.White;
            scale = 1.0f;
            layerDepth = 0.61f;
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
            collisionEvent = new HangarCollisionEvent(Game, this, Game.player);
        }

        public void SetPositionInSector()
        {
            position = new Vector2(
                (int)Game.random.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                (int)Game.random.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
        }
        
        public override void FinalGoodbye()
        {
            IsDead = true;
        }

        public override void Update(GameTime gameTime)
        {          
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
