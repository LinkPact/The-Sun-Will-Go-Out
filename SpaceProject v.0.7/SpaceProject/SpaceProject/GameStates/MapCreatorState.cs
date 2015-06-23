using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    /**
     * The entire map creator runs from this state
     *  
     * It contains following main parts:
     * - levelMechanics:    This is the point where logic related to the MapCreator is situated.
     *                          From here, users input is handled, and data handling is performed
     * - gui:               Uses data retrieved from levelMechanics to display the current state
     * - squarePalethera:   Handles logic strictly related to the various grids the user can see and interact with
     * - stringLibrary:     Information container. Main converting point, for example between save data and the
     *                          actual classes which whom the data is related
     */

    public class MapCreatorState : GameState
    {
        #region declaration
        private Sprite spriteSheet;

        private LevelMechanics levelMechanics;
        private MapCreatorGUI gui;
        private SquarePalethera squarePalethera;
        private DataConversionLibrary stringLibrary;

        private List<Action> actions;
        private List<Action> deadActions;

        private SpriteFont smallFont;
        #endregion

        private Sprite cursorSprite;

        public MapCreatorState(Game1 game, string name) :
            base(game, name)
        {
            stringLibrary = new DataConversionLibrary();

            actions = new List<Action>();
            deadActions = new List<Action>();
            smallFont = game.Content.Load<SpriteFont>("Fonts/Iceland_12");
            spriteSheet = new Sprite(game.Content.Load<Texture2D>("MapCreator/MapCreatorSpriteSheet"));

            cursorSprite = spriteSheet.GetSubSprite(new Rectangle(99, 35, 16, 16));
        }

        public override void Initialize()
        {
            base.Initialize();

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("MapCreator/MapCreatorSpriteSheet"));

            String currentName = "map1";

            levelMechanics = new LevelMechanics(Game, spriteSheet, currentName);
            levelMechanics.Initialize();
            gui = new MapCreatorGUI(Game, spriteSheet, levelMechanics);

            squarePalethera = new SquarePalethera(spriteSheet, new Vector2(380, 100));

            String equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            gui.SetEquipInfo(equipInfo);

        }

        public override void OnEnter()
        {
            base.OnEnter();
            String currentName = levelMechanics.GetName();
            levelMechanics.LoadFile(currentName);            
            squarePalethera.Initialize();

            StatsManager.gameMode = GameMode.Develop;
            Game.IsMouseVisible = false;
        }

        public override void OnLeave()
        {
            Game.IsMouseVisible = true;
        }

        public override void Update(GameTime gameTime)
        {
            MouseState mouseState = Mouse.GetState();
            Game.Window.Title = "Pos x: " + mouseState.X + " Pos y: " + mouseState.Y;

            base.Update(gameTime);

            levelMechanics.Update(gameTime);

            actions = gui.Update(gameTime);
            if (actions.Count != 0)
            {
                PerformActions(actions);
            }

            squarePalethera.Update(gameTime);

            ApplyEquipments();
        }

        private void ApplyEquipments()
        {
            String equipInfo = "";

            if (ControlManager.CheckKeyPress(Keys.D1))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(1);
            }

            if (ControlManager.CheckKeyPress(Keys.D2))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(2);
            }

            if (ControlManager.CheckKeyPress(Keys.D3))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(3);
            }

            if (ControlManager.CheckKeyPress(Keys.D4))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(4);
            }

            if (ControlManager.CheckKeyPress(Keys.D5))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(5);
            }

            if (ControlManager.CheckKeyPress(Keys.D6))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(6);
            }

            if (ControlManager.CheckKeyPress(Keys.D7))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(7);
            }

            if (ControlManager.CheckKeyPress(Keys.D8))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(8);
            }

            if (ControlManager.CheckKeyPress(Keys.D9))
            {
                equipInfo = ShipInventoryManager.MapCreatorEquip(9);
            }

            if (equipInfo != "")
                gui.SetEquipInfo(equipInfo);
        }

        private void PerformActions(List<Action> actions)
        {
            foreach (Action action in actions)
            {
                if (action is SystemAction)
                {
                    ((SystemAction)action).PerformAction(Game);
                }
                else if (action is LevelNonIncrAction)
                {
                    ((LevelNonIncrAction)action).PerformAction(levelMechanics);
                }
                else if (action is LevelIncrAction)
                {
                    ((LevelIncrAction)action).PerformAction(levelMechanics);
                }
                else if (action is SystemAndLevelAction)
                {
                    ((SystemAndLevelAction)action).PerformAction(Game, levelMechanics);
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(new Color(186, 186, 186));
            
            levelMechanics.Draw(spriteBatch);
            gui.Draw(spriteBatch);

            squarePalethera.Draw(spriteBatch);

            MouseState mouseState = Mouse.GetState();
            spriteBatch.Draw(cursorSprite.Texture, new Vector2(mouseState.X, mouseState.Y), cursorSprite.SourceRectangle, Color.White);
        }
    }
}
