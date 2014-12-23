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
                    star.PositionX = (float)rand.NextDouble() * windowWidth;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (backgroundType.Equals("deadSpace"))
                Game.GraphicsDevice.Clear(Color.Black);

            DrawInfo(spriteBatch);

            healthBar.Draw(spriteBatch);
            energyBar.Draw(spriteBatch);
            shieldBar.Draw(spriteBatch);

            DrawShipInfo(spriteBatch);

            foreach (GameObjectVertical backObj in backgroundStars)
            {
                backObj.Draw(spriteBatch);
            }
        }

        private void DrawInfo(SpriteBatch spriteBatch)
        {
            String objectiveString = level.GetObjectiveString();
            Vector2 objectiveStringPos = new Vector2(8, Game.Window.ClientBounds.Height - 103) + Game.fontManager.FontOffset;
            DrawStandardString(spriteBatch, shipInfoFontSmall, objectiveString, objectiveStringPos);
            
            String primaryString = "Primary: " + ShipInventoryManager.currentPrimaryWeapon.Name;
            Vector2 primaryStringPos = new Vector2(8, Game.Window.ClientBounds.Height - 83) + Game.fontManager.FontOffset;
            DrawStandardString(spriteBatch, shipInfoFontSmall, primaryString, primaryStringPos);

            String secondaryString = "Secondary: " + ShipInventoryManager.equippedSecondary.Name;
            if (!(ShipInventoryManager.equippedSecondary is EmptyWeapon)) secondaryString = AppendSecondaryActivatedInformation(secondaryString);
            Vector2 secondaryStringPos = new Vector2(8, Game.Window.ClientBounds.Height - 68) + Game.fontManager.FontOffset;
            DrawStandardString(spriteBatch, shipInfoFontSmall, secondaryString, secondaryStringPos);

            String shipStatsNamesString = "Health:\nEnergy:\nShield:";
            Vector2 shipStatsNamesStringPos = new Vector2(8, Game.Window.ClientBounds.Height - 49) + Game.fontManager.FontOffset;
            DrawStandardString(spriteBatch, shipInfoFontSmall, shipStatsNamesString, shipStatsNamesStringPos);
        }

        private String AppendSecondaryActivatedInformation(String rawSecondaryWeaponString)
        {
            if (PlayerShotHandler.SecondaryOn)
                rawSecondaryWeaponString += " - On";
            else
                rawSecondaryWeaponString += " - Off";

            return rawSecondaryWeaponString;
        }

        private void DrawShipInfo(SpriteBatch spriteBatch)
        {
            int statsInfoXOffset = 220;

            String healthString = string.Format("{0}/{1}", (int)player.HP, player.HPmax);
            Vector2 healthStringPos = new Vector2(statsInfoXOffset, healthPos.Y - 5);
            DrawStandardString(spriteBatch, shipInfoFontSmall, healthString, healthStringPos);

            String energyString = string.Format("{0}/{1}", (int)player.MP, player.MPmax);
            Vector2 energyStringPos = new Vector2(statsInfoXOffset, energyPos.Y - 5);
            DrawStandardString(spriteBatch, shipInfoFontSmall, energyString, energyStringPos);

            String shieldString = string.Format("{0}/{1}", (int)player.Shield, player.ShieldMax);
            Vector2 shieldStringPos = new Vector2(statsInfoXOffset, shieldPos.Y - 5);
            DrawStandardString(spriteBatch, shipInfoFontSmall, shieldString, shieldStringPos);
        }

        private void DrawStandardString(SpriteBatch spriteBatch, SpriteFont font, String text, Vector2 position)
        {
            spriteBatch.DrawString(font, text, position, Color.White, 0.0f, Vector2.Zero, 
                1.0f, SpriteEffects.None, drawLayerText);
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
                    star.PositionX = (float)rand.NextDouble() * windowWidth;
                    star.PositionY = (float)rand.NextDouble() * windowHeight;
                    backgroundStars.Add(star);
                }
            }
        }
    }
}
