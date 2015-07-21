using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    //Only visual object. Meant to be displayed in menues, with different looks for when it's active and when it's inactive.
    public class MenuDisplayObject
    {
        #region declaration
        private Game1 Game;
        private Sprite passive;
        private Sprite active;
        private Sprite disabled;
        private Vector2 position;
        public bool isActive;

        //Danne
        private Vector2 origin;
        private Sprite selected;
        public bool isSelected;
        public bool isVisible = true;
        public bool isDeactivated;
        public string name;

        public Vector2 Position { get { return position; } set { position = value; } }
        public Vector2 Origin { get { return origin; } set { origin = value; } }

        #endregion

        #region Properties

        public Sprite Passive
        {
            get { return passive; }
            set { passive = value; }
        }

        public Sprite Active
        {
            get { return active; }
            set { active = value; }
        }

        public Rectangle Bounds
        {
            get 
            {
                Vector2 pos = position;

                if (origin != Vector2.Zero)
                {
                    pos = new Vector2(position.X - passive.Width / 2, position.Y - passive.Height / 2);
                }

                return new Rectangle((int)pos.X, (int)pos.Y, passive.Width, passive.Height);
            }
        }

        #endregion

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Vector2 position)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.position = position;
            origin = Vector2.Zero;
        }

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Vector2 position, Vector2 origin)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.position = position;
            this.origin = origin;
        }

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Sprite selected, Vector2 position)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.position = position;
            this.selected = selected;
            origin = Vector2.Zero;
        }

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Sprite selected, Vector2 position, Vector2 origin)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.selected = selected;
            this.position = position;
            this.origin = origin;
        }

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Sprite selected, Sprite disabled, Vector2 position, Vector2 origin)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.disabled = disabled;
            this.selected = selected;
            this.position = position;
            this.origin = origin;
        }

        public void Initialize()
        {
            isActive = false;
            isSelected = false;
        }

        public void Update(GameTime gameTime)
        { 
        
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Sprite sprite;

            if (isDeactivated)
            {
                sprite = disabled;
            }
            else if (isActive)
            {
                sprite = active;
            }
            else if (isSelected)
            {
                sprite = selected;
            }
            else
            {
                sprite = passive;
            }

            if (isVisible)
            {
                spriteBatch.Draw(sprite.Texture,
                                 position,
                                 sprite.SourceRectangle,
                                 Color.White,
                                 0.0f,
                                 origin,
                                 1.0f,
                                 SpriteEffects.None,
                                 0.9f);
            }
        }
    }
}
