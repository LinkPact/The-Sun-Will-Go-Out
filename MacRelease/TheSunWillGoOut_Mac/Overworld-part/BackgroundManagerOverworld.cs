using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class BackgroundManagerOverworld
    {
        public List<Star> Stars;
        private int numberOfStars = 400;
        private List<Star> deadStars;

        private Game1 Game;

        public BackgroundManagerOverworld(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        { 
            Stars = new List<Star>();
            deadStars = new List<Star>();
        }

        public void AddStar(Sprite spriteSheet)
        {
            int count = Stars.OfType<Star>().Count();
            
            while (count <= numberOfStars)
            {
                Star star = new Star(Game, spriteSheet);
                star.Initialize();
                Stars.Add(star);
                count++;
            }

        }

        public void Update(GameTime gameTime)
        {
            foreach (Star star in Stars)
            {
                star.Update(gameTime);
            }    
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Star star in Stars)
            {
                if (GameStateManager.currentState.Equals("IntroState"))
                    star.Draw(spriteBatch, true, 1, null);
                else
                    star.Draw(spriteBatch, Game.player.HyperspeedOn || Game.player.UsingBoost, Game.player.speed, Game.player.Direction);
            }
        }

        public void ClearStarList()
        {
            for (int i = 0; i < Stars.Count; i++)
            {
                deadStars.Add(Stars[i]);
            }

            for (int i = 0; i < deadStars.Count; i++)
            {
                Stars.Remove(deadStars[i]);
            }

            deadStars.Clear();
        }
    }
}
