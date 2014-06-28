using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BackgroundManagerOverworld
    {
        public List<Star> Stars;
        private int numberOfStars;
        private List<Star> deadStars;

        Sprite spriteSheet;

        private Game1 Game;

        public BackgroundManagerOverworld(Game1 Game)
        {
            this.Game = Game;
        }

        public void Initialize()
        { 
            Stars = new List<Star>();
            deadStars = new List<Star>();
            numberOfStars = 0;
        }

        public void InitializeStars()
        {
            foreach (Star star in Stars)
            {
                star.Initialize();
            }
        }

        public void AddStar(int numberOfStars, Sprite spriteSheet)
        {
            if (numberOfStars == 0)
            {
                this.numberOfStars = numberOfStars;
                this.spriteSheet = spriteSheet;
            }

            while(numberOfStars > 0)
            {
                Star star = new Star(Game, spriteSheet);
                star.Initialize();
                Stars.Add(star);
                numberOfStars--;
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

        public void RemoveStar()
        {
            foreach (Star star in Stars)
            {
                deadStars.Add(star);
            }
        }
    }
}
