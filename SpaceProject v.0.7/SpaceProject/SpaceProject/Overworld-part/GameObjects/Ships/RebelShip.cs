﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    /// <summary>
    /// 
    /// </summary>
    class RebelShip : OverworldShip
    {
        private Rectangle view;
        private int viewRadius;
        private bool roam;
        public Vector2 destination;

        private GameObjectOverworld target;

        // Used to determine which level starts when player runs into this ship.
        private string level;
        public string GetLevel { get { return level; } }

        private Sector sector = null;

        public RebelShip(Game1 Game, Sprite SpriteSheet) :
            base(Game, SpriteSheet)
        { }

        public override void Initialize()
        {
            Class = "RebelShip";
            name = "Rebel Ship";

            sprite = spriteSheet.GetSubSprite(new Rectangle(484, 0, 23, 34));
            viewRadius = 400;
            position = new Vector2(0, 0);
            speed = 0.42f;
            roam = true;
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

        public void SetPosition(Vector2 pos) { position = pos; }
        public void SetPositionInSector()
        {
            Random r = new Random(DateTime.Now.Millisecond);
            position = new Vector2(
                (int)r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                (int)r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
            //if (CollisionDetection.IsPointInsideRectangle(position, Game.stateManager.overWorldState.HUD.radar.View))
            //    SetPositionInSector();
        }
        public void SetSector(Sector sec) { sector = sec; }
        public void SetRoamBehaviour(bool newValue) { roam = newValue; }
        public void SetLevel(string level) { this.level = level; }
        public void SetTarget(GameObjectOverworld target) { this.target = target; }
        public override void FinalGoodbye()
        {
            IsDead = true;
            if (sector != null)
            {
                sector.shipSpawner.RemovePirateShip();
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (GameStateManager.currentState == "OverworldState")
                IsUsed = true;
            else
                IsUsed = false;

            // Update view
            view = new Rectangle((int)position.X - viewRadius, (int)position.Y - viewRadius, viewRadius * 2, viewRadius * 2);

            // Select target
            if (CollisionDetection.IsPointInsideRectangle(target.position, view))
            {
                destination = target.position;
            }
            else if (roam == true && sector != null)
            {
                if (CollisionDetection.IsPointInsideRectangle(destination, Bounds))
                {
                    Random r = new Random(DateTime.Now.Millisecond);
                    destination = new Vector2(
                        r.Next(sector.SpaceRegionArea.Left, sector.SpaceRegionArea.Right),
                        r.Next(sector.SpaceRegionArea.Top, sector.SpaceRegionArea.Bottom));
                }
            }
            else if (roam == false)
                destination = Vector2.Zero;

            // Adjust course towards target
            if (destination != Vector2.Zero)
            {
                Direction.RotateTowardsPoint(this.position, destination, 0.2f);
                AddParticle();
            }
            else
                Direction = Direction.Zero;

            angle = (float)(GlobalFunctions.RadiansFromDir(new Vector2(
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