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
        private int numberOfSmallStars = 90;
        private int numberOfMediumStars = 90;
        private int numberOfLargeStars = 90;
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
            int countSmall = Stars.OfType<SmallStar>().Count();
            int countMedium = Stars.OfType<MediumStar>().Count(); ;
            int countLarge = Stars.OfType<LargeStar>().Count(); ;

            while (countSmall <= numberOfSmallStars)
            {
                SmallStar star = new SmallStar(Game, spriteSheet);
                star.Initialize();
                Stars.Add(star);
                countSmall++;
            }
            while (countMedium <= numberOfMediumStars)
            {
                MediumStar star = new MediumStar(Game, spriteSheet);
                star.Initialize();
                Stars.Add(star);
                countMedium++;
            }
            while (countLarge <= numberOfLargeStars)
            {
                LargeStar star = new LargeStar(Game, spriteSheet);
                star.Initialize();
                Stars.Add(star);
                countLarge++;
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
