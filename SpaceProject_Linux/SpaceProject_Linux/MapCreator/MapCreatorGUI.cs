using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Linux
{
    class MapCreatorGUI
    {
        #region declaration
        private Game1 game;
        private Sprite spriteSheet;
        private LevelMechanics level;

        private int screenW;
        private int screenH;

        private List<MapCreatorButton> buttons;

        private ButtonPinger buttonPinger;

        private List<Action> actions;
        private List<Action> deadActions;

        //Timers
        private SpriteFont font;
        private SpriteFont font8;
        private SpriteFont icelandFont;

        public static SpriteFont staticFontExperiment;
        public static Sprite staticSpriteExperiment;

        private static Square targetedSquare;

        private String equipInfoString;

        #endregion

        #region globalPositions
        private Vector2 widthButtonPos;
        private Vector2 durationButtonPos;
        private Vector2 startTimeButtonPos;

        private Vector2 gridWidthPos;
        private Vector2 gridHeightPos;
        private Vector2 gridViewPos;
        private Vector2 durationWidthPos;
        private Vector2 levelObjectivePos;
        #endregion

        public MapCreatorGUI(Game1 game, Sprite spriteSheet, LevelMechanics level)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
            this.level = level;

            font = game.Content.Load<SpriteFont>("Fonts/MCSmallFont");
            font8 = game.Content.Load<SpriteFont>("Fonts/MCfont8");
            icelandFont = game.Content.Load<SpriteFont>("Fonts/Iceland_14");

            staticFontExperiment = game.Content.Load<SpriteFont>("Fonts/Iceland_14");
            staticSpriteExperiment = new Sprite(game.Content.Load<Texture2D>("MapCreator/MapCreatorSpriteSheet"));

            screenW = game.ScreenSize.X;
            screenH = game.ScreenSize.Y;

            #region regularButtons
            buttons = new List<MapCreatorButton>();

            Vector2 quitpos = new Vector2(screenW - 90, screenH - 60);
            buttons.Add(new QuitApplicationButton(spriteSheet, quitpos));
            Vector2 savepos = new Vector2(screenW - 170, screenH - 60);
            SaveLevelButton save = new SaveLevelButton(game, spriteSheet, savepos);
            buttons.Add(save);
            Vector2 loadpos = new Vector2(screenW - 250, screenH - 60);
            buttons.Add(new LoadLevelButton(game, spriteSheet, loadpos));
            Vector2 editpos = new Vector2(screenW - 90, screenH - 100);
            buttons.Add(new EditNameButton(game, spriteSheet, editpos));
            Vector2 clearpos = new Vector2(screenW - 90, screenH - 140);
            buttons.Add(new ClearGridButton(game, spriteSheet, clearpos));
            Vector2 runpos = new Vector2(screenW - 90, screenH - 180);
            buttons.Add(new RunLevelButton(spriteSheet, runpos));

            levelObjectivePos = new Vector2(550, screenH - 75);
            buttons.Add(new SetLevelObjectiveButton(spriteSheet, levelObjectivePos));

            gridWidthPos = new Vector2(screenW - 250, screenH - 175);
            Vector2 gridWidthBigMinusPos = gridWidthPos;
            buttons.Add(new GridWidthIncrButton(game, spriteSheet, gridWidthBigMinusPos, IncrementalType.largeNegative));
            Vector2 gridWidthSmallMinusPos = new Vector2(gridWidthPos.X + 30, gridWidthPos.Y);
            buttons.Add(new GridWidthIncrButton(game, spriteSheet, gridWidthSmallMinusPos, IncrementalType.smallNegative));
            Vector2 gridWidthSmallPlusPos = new Vector2(gridWidthPos.X + 60, gridWidthPos.Y);
            buttons.Add(new GridWidthIncrButton(game, spriteSheet, gridWidthSmallPlusPos, IncrementalType.smallPositive));
            Vector2 gridWidthBigPlusPos = new Vector2(gridWidthPos.X + 90, gridWidthPos.Y);
            buttons.Add(new GridWidthIncrButton(game, spriteSheet, gridWidthBigPlusPos, IncrementalType.largePositive));

            gridHeightPos = new Vector2(screenW - 250, screenH - 125);
            Vector2 gridHeightBigMinusPos = gridHeightPos;
            buttons.Add(new GridHeightIncrButton(game, spriteSheet, gridHeightBigMinusPos, IncrementalType.largeNegative));
            Vector2 gridHeightSmallMinusPos = new Vector2(gridHeightPos.X + 30, gridHeightPos.Y);
            buttons.Add(new GridHeightIncrButton(game, spriteSheet, gridHeightSmallMinusPos, IncrementalType.smallNegative));
            Vector2 gridHeightSmallPlusPos = new Vector2(gridHeightPos.X + 60, gridHeightPos.Y);
            buttons.Add(new GridHeightIncrButton(game, spriteSheet, gridHeightSmallPlusPos, IncrementalType.smallPositive));
            Vector2 gridHeightBigPlusPos = new Vector2(gridHeightPos.X + 90, gridHeightPos.Y);
            buttons.Add(new GridHeightIncrButton(game, spriteSheet, gridHeightBigPlusPos, IncrementalType.largePositive));

            durationWidthPos = new Vector2(screenW - 80, screenH - 225);
            Vector2 durationWidthMinus = durationWidthPos;
            buttons.Add(new DurationWidthIncrButton(game, spriteSheet, durationWidthMinus, IncrementalType.smallNegative));
            Vector2 durationWidthPlus = new Vector2(durationWidthPos.X - 20, durationWidthPos.Y);
            buttons.Add(new DurationWidthIncrButton(game, spriteSheet, durationWidthPlus, IncrementalType.smallPositive));

            gridViewPos = new Vector2(screenW - 50, screenH - 329);
            Vector2 changeGridViewUpPos = gridViewPos;
            buttons.Add(new ChangeGridViewButton(game, spriteSheet, changeGridViewUpPos, IncrementalType.upArrow));
            Vector2 changeGridViewDownPos = new Vector2(gridViewPos.X + 25, gridViewPos.Y);
            buttons.Add(new ChangeGridViewButton(game, spriteSheet, changeGridViewDownPos, IncrementalType.downArrow));

            widthButtonPos = new Vector2(screenW - 80, screenH - 270);
            buttons.Add(new WidthInPixelsButton(spriteSheet, widthButtonPos, level.GetWidthInPixels()));
            durationButtonPos = new Vector2(screenW - 80, screenH - 250);
            buttons.Add(new DurationButton(spriteSheet, durationButtonPos, level.GetLevelDuration()));
            startTimeButtonPos = new Vector2(screenW - 80, screenH - 290);
            buttons.Add(new StartTimeButton(spriteSheet, startTimeButtonPos));

            foreach (MapCreatorButton button in buttons)
            {
                button.Initialize();
            }
            #endregion

            buttonPinger = new ButtonPinger(game, buttons);

            actions = new List<Action>();
            deadActions = new List<Action>();

            equipInfoString = "";
        }

        // Sets string used to display current player equipment
        public void SetEquipInfo(String equipInfo)
        {
            equipInfoString = equipInfo;
        }

        public List<Action> Update(GameTime gameTime)
        {
            foreach (MapCreatorButton button in buttons)
                button.Update(gameTime);

            return buttonPinger.GetActions();
        }

        public static void SetTargetedSquare(Square square)
        {
            targetedSquare = square;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (MapCreatorButton button in buttons)
            {
                button.Draw(spriteBatch);
                button.Draw(spriteBatch, font);
            }

            int leftPadding = 20;

            if (level.SaveStringTime())
                spriteBatch.DrawString(font, "Save successful!", new Vector2(leftPadding, 582), Color.Green);

            spriteBatch.DrawString(font8, equipInfoString, new Vector2(leftPadding, 600), Color.Green);

            WriteText(spriteBatch);
        }

        private void WriteText(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(icelandFont, "Name: " + level.GetName(), new Vector2(screenW - 250, screenH - 95), Color.Black);
            
            spriteBatch.DrawString(icelandFont, "Level width: " + level.GetWidthInPixels(), new Vector2(screenW - 250, widthButtonPos.Y), Color.Black);
            spriteBatch.DrawString(icelandFont, "Level duration: " + level.GetLevelDuration(), new Vector2(screenW - 250, durationButtonPos.Y), Color.Black);
            
            spriteBatch.DrawString(icelandFont, "Grid width: " + level.GetPointGridWidth(), new Vector2(screenW - 250, gridWidthPos.Y - 20), Color.Black);
            spriteBatch.DrawString(icelandFont, "Grid height: " + level.GetPointGridHeight(), new Vector2(screenW - 250, gridHeightPos.Y - 20), Color.Black);
            spriteBatch.DrawString(icelandFont, "Duration width: " + level.GetDurationGridWidth(), new Vector2(screenW - 250, durationWidthPos.Y), Color.Black);
            
            spriteBatch.DrawString(icelandFont, "View: " + (level.GetViewFrame() + 1) + "/" + (level.GetViewFrameMax() + 1), new Vector2(screenW - 250, screenH - 320), Color.Black);
            spriteBatch.DrawString(icelandFont, "Test start at: " + level.GetTestStartTime(), new Vector2(screenW - 250, startTimeButtonPos.Y), Color.Black);

            spriteBatch.DrawString(icelandFont, "Objective: " + level.GetObjectiveDisplayString(), new Vector2(360, levelObjectivePos.Y), Color.Black);

            if (targetedSquare != null)
                targetedSquare.DrawInfo(spriteBatch, icelandFont, new Vector2(600,600));

            targetedSquare = null;
        }
    }
}
