using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum InteractionType
    { 
        Text,
        LevelWithReward,
        FuelShop,
        ItemShop,
        GetItem,
        Custom,
        Nothing
    }

    public abstract class SubInteractiveObject : GameObjectOverworld
    {
        private static int count;
        private int id;

        private bool cleared;
        protected string clearedText = "EMPTY";

        protected List<string> text;
        protected List<string> options;
        protected OverworldEvent overworldEvent;
        protected OverworldEvent overrideEvent;

        protected SubInteractiveObject(Game1 game, Sprite spriteSheet) :
            base(game, spriteSheet)
        {
            SubInteractiveObject.count++;
            id = count;
        }

        public override void Initialize()
        {
            base.Initialize();

            text = new List<string>();
            options = new List<string>();

            scale = 1f;
            layerDepth = 0.3f + (float)(MathFunctions.GetExternalRandomDouble() * 0.01);

            color = Color.White;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (overworldEvent != null)
            {
                overworldEvent.Update(Game, gameTime);
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public virtual void Interact()
        {
            if (overworldEvent != null)
            {
                if (overrideEvent != null)
                {
                    overrideEvent.Activate();
                    overrideEvent = null;
                }
                else
                {
                    overworldEvent.Activate();
                }
            }
        }

        protected abstract void SetClearedText();

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            saveData.Clear();
            saveData.Add("disabled", cleared.ToString());

            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "subobject" + id, saveData);
        }

        public void Load()
        {
            cleared = Game.saveFile.GetPropertyAsBool("subobject" + id, "disabled", false);
            if (cleared)
            {
                SetClearedText();
            }
        }

        public void OverrideEvent(OverworldEvent ev)
        {
            overrideEvent = ev;
        }
    }
}
