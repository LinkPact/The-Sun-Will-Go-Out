﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{

    public abstract class AnimatedGameObject : GameObjectVertical
    {
        protected Sprite spriteSheet;

        public Color ObjectColor;
        protected Color baseColor;
        
        protected Animation anim;
        private Animation currentAnim;

        protected bool useDeathAnim = false;
        protected Animation DeathAnimation = new Animation();

        protected int animDamageTimer = 0;
        protected int animDamageInterval = 100;
        protected Animation DamageAnimation = new Animation();

        protected float transparency = 1;
        protected float scale = 1f;
        protected float angle = 0.0f;

        public Animation Anim { get { return anim; } }
        public Animation CurrentAnim { get { return currentAnim; }
            set 
            {
                CenterPoint = new Vector2(value.Width / 2, value.Height / 2);
                currentAnim = value;
            }
        }

        public float Width { get { return anim.Width; } }
        public float Height { get { return anim.Height; } }

        protected AnimatedGameObject(Game1 Game, Sprite spriteSheet):
            base(Game)
        {
            this.ObjectColor = Color.White;
            this.spriteSheet = spriteSheet;
        }

        public override void Initialize( )
        {
            anim = new Animation();
            CurrentAnim = anim;
            base.Initialize();
        }

        public override void DeInitialize()
        { 
            base.DeInitialize(); 
        }

        public override void Update(GameTime gameTime)
        {
            currentAnim.Update(gameTime);
            UpdateDamageAnim(gameTime);
            
            Bounding = new Rectangle((int)PositionX - (int)CenterPointX, (int)PositionY - (int)CenterPointY, currentAnim.Width, currentAnim.Height);

            BoundingWidth = currentAnim.Width;
            BoundingHeight = currentAnim.Height;

            BoundingX = (int)PositionX - (currentAnim.Width / 2);
            BoundingY = (int)PositionY - (currentAnim.Height / 2);

            base.Update(gameTime);

            IsOutside = CheckOutside();
        }

        private void UpdateDamageAnim(GameTime gameTime)
        {
            if (animDamageTimer < animDamageInterval)
            {
                animDamageTimer += gameTime.ElapsedGameTime.Milliseconds;
                if (animDamageTimer > animDamageInterval)
                    this.ObjectColor = baseColor;
            }
        }

        public override void OnDamage()
        {
            animDamageTimer = 0;
            this.baseColor = ObjectColor;
            this.ObjectColor = Color.LightSalmon;
        }

        public virtual bool CheckOutside()
        {
            float centerX = windowWidth / 2;
            float leftEdgeX = centerX - LevelWidth / 2;
            float rightEdgeX = centerX + LevelWidth / 2;
            float xPadding = 50;
            float paddingAboveWindow = 500;

            if (PositionX + anim.Width < leftEdgeX - xPadding || PositionX - anim.Width > rightEdgeX + xPadding
                || PositionY + anim.Width < -paddingAboveWindow || PositionY - anim.Height > windowHeight)
            {
                return true;
            }
            else
                return false;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(currentAnim.CurrentFrame.Texture, Position, currentAnim.CurrentFrame.SourceRectangle, ObjectColor * transparency, angle + RotationAngle, CenterPoint, scale, SpriteEffects.None, DrawLayer);
        }
    }
}
