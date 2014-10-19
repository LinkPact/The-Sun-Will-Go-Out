using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class GameState
    {
        public Game1 Game;
        public String Name { get; private set; }

        public String Class;

        protected Music ActiveSong;

        protected GameState(Game1 game, String name)
        { 
            this.Game = game;
            Name = name;

            Class = "";

            ActiveSong = Music.none;
        }

        public virtual void Initialize()
        { }

        public virtual void OnEnter() 
        {
            if (ActiveSong != Music.none)
            {
                Game.musicManager.PlayMusic(ActiveSong);
            }
        }

        public virtual void OnLeave() 
        { 
        
        }

        public virtual void Update(GameTime gameTime)
        {
            if (Class.Equals("play"))
            {
                StatsManager.PlayTime.Update(gameTime);
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
        }


    }
}
