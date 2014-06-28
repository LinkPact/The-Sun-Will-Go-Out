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
        private Vector2 position;
        public bool isActive;

        //Danne
        private Vector2 origin;
        private Sprite selected;
        public bool isSelected;
        public bool isVisible;
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

        #endregion

        public MenuDisplayObject(Game1 Game, Sprite passive, Sprite active, Vector2 position)
        {
            this.Game = Game;
            this.passive = passive;
            this.active = active;
            this.position = position;
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
            if (origin == null)
            {
                if (isActive)
                {                    
                    spriteBatch.Draw(active.Texture, position, active.SourceRectangle, Color.White);
                }
                else
                {
                    if (isSelected)
                        spriteBatch.Draw(selected.Texture, position, selected.SourceRectangle, Color.White);

                    else if (isDeactivated)
                        spriteBatch.Draw(passive.Texture, position, passive.SourceRectangle, Color.DarkGray);

                    else
                        spriteBatch.Draw(passive.Texture, position, passive.SourceRectangle, Color.White);
                }
            }

            else
            {
                if (isActive)
                {                   
                    spriteBatch.Draw(active.Texture,
                                     position,
                                     active.SourceRectangle,
                                     Color.White,
                                     0.0f,
                                     origin,
                                     1.0f,
                                     SpriteEffects.None,
                                     1.0f);
                }
                else
                {
                    if (isSelected)
                        spriteBatch.Draw(selected.Texture,
                                     position,
                                     selected.SourceRectangle,
                                     Color.White,
                                     0.0f,
                                     origin,
                                     1.0f,
                                     SpriteEffects.None,
                                     1.0f);


                    else if (isDeactivated)
                        spriteBatch.Draw(passive.Texture,
                                         position,
                                         passive.SourceRectangle,
                                         Color.DarkGray,
                                         0.0f,
                                         origin,
                                         1.0f,
                                         SpriteEffects.None,
                                         1.0f);

                    else
                        spriteBatch.Draw(passive.Texture,
                                     position,
                                     passive.SourceRectangle,
                                     Color.White,
                                     0.0f,
                                     origin,
                                     1.0f,
                                     SpriteEffects.None,
                                     1.0f);
                }
            }
        }
    }
}
