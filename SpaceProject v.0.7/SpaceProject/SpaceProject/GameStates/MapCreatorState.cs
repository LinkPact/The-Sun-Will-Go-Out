using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
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

        public MapCreatorState(Game1 game, string name) :
            base(game, name)
        {
            stringLibrary = new DataConversionLibrary();

            actions = new List<Action>();
            deadActions = new List<Action>();
            smallFont = game.Content.Load<SpriteFont>("Fonts/Iceland_12");
            spriteSheet = new Sprite(game.Content.Load<Texture2D>("MapCreator/MapCreatorSpriteSheet"));
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

            //ActiveSong = Music.Jigsaw;
        }

        public override void OnEnter()
        {
            base.OnEnter();
            String currentName = levelMechanics.GetName();
            levelMechanics.LoadFile(currentName);            
            squarePalethera.Initialize();

            StatsManager.gameMode = GameMode.develop;
        }

        public override void OnLeave()
        { }

        public override void Update(GameTime gameTime)
        {
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
            if (ControlManager.CheckKeypress(Keys.D1))
            {
                ShipInventoryManager.MapCreatorEquip(1);
            }

            if (ControlManager.CheckKeypress(Keys.D2))
            {
                ShipInventoryManager.MapCreatorEquip(2);
            }

            if (ControlManager.CheckKeypress(Keys.D3))
            {
                ShipInventoryManager.MapCreatorEquip(3);
            }

            if (ControlManager.CheckKeypress(Keys.D4))
            {
                ShipInventoryManager.MapCreatorEquip(4);
            }

            if (ControlManager.CheckKeypress(Keys.D5))
            {
                ShipInventoryManager.MapCreatorEquip(5);
            }

            if (ControlManager.CheckKeypress(Keys.D6))
            {
                ShipInventoryManager.MapCreatorEquip(6);
            }

            if (ControlManager.CheckKeypress(Keys.D7))
            {
                ShipInventoryManager.MapCreatorEquip(7);
            }

            if (ControlManager.CheckKeypress(Keys.D8))
            {
                ShipInventoryManager.MapCreatorEquip(8);
            }

            if (ControlManager.CheckKeypress(Keys.D9))
            {
                ShipInventoryManager.MapCreatorEquip(9);
            }
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
            Game.GraphicsDevice.Clear(Color.LightGray);
            
            levelMechanics.Draw(spriteBatch);
            gui.Draw(spriteBatch);

            squarePalethera.Draw(spriteBatch);
        }
    }
}
