using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum BackgroundType
    {
        deadSpace,
        none
    }

    //Hanterar bakgrunden, samt GUI for nivaerna
    public class BackgroundManager
    {
        private Game1 Game;
        private Sprite verticalShooterSpriteSheet;
        private Sprite hudSpriteSheet;
        private PlayerVerticalShooter player;

        private int windowWidth;
        private int windowHeight;

        private float drawLayerText = 0.8f;

        private Bar healthBar;
        private Bar energyBar;
        private Bar shieldBar;

        private Vector2 healthPos;
        private Vector2 energyPos;
        private Vector2 shieldPos;

        private List<GameObjectVertical> backgroundStars;

        private BackgroundType backgroundType;

        private SpriteFont shipInfoFont;
        private SpriteFont shipInfoFontSmall;

        private Random rand = new Random();

        private Level level;

        public BackgroundManager(Game1 Game, PlayerVerticalShooter player, Level level)
        {
            this.Game = Game;
            this.verticalShooterSpriteSheet = Game.spriteSheetVerticalShooter;
            this.player = player;

            windowWidth = Game.Window.ClientBounds.Width;
            windowHeight = Game.Window.ClientBounds.Height;

            hudSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/planetarySystemSpriteSheet"));
            verticalShooterSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));

            shipInfoFont = Game.fontManager.GetFont(14);
            shipInfoFontSmall = Game.fontManager.GetFont(12);
            this.level = level;
        }

        public void Initialize(BackgroundType backgroundType)
        {
            backgroundStars = new List<GameObjectVertical>();
            this.backgroundType = backgroundType;

            float barXSpacing = 70;

            healthBar = new Bar(Game, hudSpriteSheet, Color.Red, true);
            healthPos = new Vector2(barXSpacing, Game.Window.ClientBounds.Height - 44);
            energyBar = new Bar(Game, hudSpriteSheet, Color.Green, true);
            energyPos = new Vector2(barXSpacing, Game.Window.ClientBounds.Height - 28);
            shieldBar = new Bar(Game, hudSpriteSheet, Color.Blue, true);
            shieldPos = new Vector2(barXSpacing, Game.Window.ClientBounds.Height - 12);

            InitializeBackground(backgroundType);
        }

        public void Update(GameTime gameTime)
        {
            healthBar.Update(gameTime, player.HP, player.HPmax, healthPos);
            energyBar.Update(gameTime, player.MP, player.MPmax, energyPos);
            shieldBar.Update(gameTime, player.Shield, player.ShieldMax, shieldPos);

            foreach (GameObjectVertical star in backgroundStars)
            {
                star.Update(gameTime);
            
                if (star.PositionY > windowHeight)
                {
                    star.PositionY = 0;
                    star.PositionX = (float)rand.NextDouble() * windowWidth - 30;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            
            if (backgroundType.Equals("deadSpace"))
                Game.GraphicsDevice.Clear(Color.Black);

            //Shows stats
            spriteBatch.DrawString(
                shipInfoFontSmall, 
                "Primary: " + ShipInventoryManager.currentPrimaryWeapon.Name, 
                //new Vector2(160, Game.Window.ClientBounds.Height - 33) + Game.fontManager.FontOffset,
                new Vector2(8, Game.Window.ClientBounds.Height - 68) + Game.fontManager.FontOffset,
                Color.White, 
                0.0f, 
                Vector2.Zero, 
                1.0f, 
                SpriteEffects.None, 
                drawLayerText);

            String objectiveString = level.GetObjectiveString();
            spriteBatch.DrawString(
                shipInfoFont,
                objectiveString,
                new Vector2(8, Game.Window.ClientBounds.Height - 88) + Game.fontManager.FontOffset,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                drawLayerText);

            spriteBatch.DrawString(
                shipInfoFontSmall,
                "Health:\nEnergy:\nShield:",
                new Vector2(8, Game.Window.ClientBounds.Height - 49) + Game.fontManager.FontOffset,
                Color.White,
                0.0f,
                Vector2.Zero,
                1.0f,
                SpriteEffects.None,
                drawLayerText);

            healthBar.Draw(spriteBatch);
            energyBar.Draw(spriteBatch);
            shieldBar.Draw(spriteBatch);
            
            foreach (GameObjectVertical backObj in backgroundStars)
            {
                backObj.Draw(spriteBatch);
            }
        }

        private void InitializeBackground(BackgroundType backgroundType)
        {
            switch (backgroundType) 
            {
                case BackgroundType.deadSpace:
                    {
                        InitializeBackgroundStars();
                        break;
                    }
                case BackgroundType.none:
                    {
                        backgroundStars.Clear();
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("Enum value not implemented!");
                    }
            }
        }

        private void InitializeBackgroundStars()
        {
            if (backgroundType == BackgroundType.deadSpace)
            {
                backgroundStars.Clear();

                int numberOfStars = 500;

                for (int n = 0; n < numberOfStars; n++)
                {
                    BackgroundStar star = new BackgroundStar(Game, verticalShooterSpriteSheet);
                    star.Initialize();
                    star.PositionX = (float)rand.NextDouble() * windowWidth - 30;
                    star.PositionY = (float)rand.NextDouble() * windowHeight - 30;
                    backgroundStars.Add(star);
                }
            }
        }
    }
}
