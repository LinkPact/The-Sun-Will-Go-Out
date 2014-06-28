using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class SpaceRegion
    {
        protected Game1 game;
        protected String name;
        protected Sprite spriteSheet;

        protected List<GameObjectOverworld> gameObjects;
        protected List<GameObjectOverworld> garbageGameObjects;

        protected Rectangle spaceRegionArea;

        public Rectangle SpaceRegionArea { get { return spaceRegionArea; } private set { ; } }

        protected SpaceRegion(Game1 game, Sprite spriteSheet)
        {
            this.game = game;
            this.spriteSheet = spriteSheet;
        }

        protected SpaceRegion(Game1 game)
        {
            this.game = game;
        }

        public virtual void Initialize()
        {
            gameObjects = new List<GameObjectOverworld>();
            garbageGameObjects = new List<GameObjectOverworld>();
        }

        public virtual void OnEnter()
        {
            game.helper.DisplayText("Entering " + name + " .", 2);
        }

        public virtual void OnLeave()
        {
            game.helper.DisplayText("Leaving " + name + ".", 2);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (GameObjectOverworld obj in gameObjects)
            {
                obj.Update(gameTime);
            }

            if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter)) && !game.player.HyperspeedOn))
            {
                EnterCheck();
            }

            if (garbageGameObjects.Count > 0)
            {
                DeleteRemovedGameObjects();
            }

            ShopManager.UpdateShops(gameObjects);
        }
        private void EnterCheck()
        {
            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (CollisionDetection.IsRectInRect(game.player.Bounds, gameObjects[i].Bounds))
                {
                    // TODO Repair the ship. Temporary solution.
                    StatsManager.RepairShip(StatsManager.Armor());

                    if (gameObjects[i] is Planet)
                    {
                        game.stateManager.planetState.LoadPlanetData((Planet)gameObjects[i]);
                        game.stateManager.ChangeState("PlanetState");
                    }

                    else if (gameObjects[i] is Station)
                    {
                        game.stateManager.stationState.LoadStationData((Station)gameObjects[i]);
                        game.stateManager.ChangeState("StationState");
                    }

                    else if (gameObjects[i] is Beacon)
                    {
                        ((Beacon)gameObjects[i]).Interact();
                    }
                }

            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameObjectOverworld obj in gameObjects)
            {
                obj.Draw(spriteBatch);
            }

            for (int i = 0; i < gameObjects.Count; i++)
            {
                if (CollisionDetection.IsRectInRect(game.player.Bounds, gameObjects[i].Bounds))
                {
                    if (gameObjects[i].Class == "Planet")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, gameObjects[i]);
                        game.helper.DisplayText("Press 'Enter' to enter planetary orbit.");
                    }

                    else if (gameObjects[i].Class == "Station")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, gameObjects[i]);
                        game.helper.DisplayText("Press 'Enter' to dock with station.");
                    }

                    else if (gameObjects[i].Class == "MotherShip")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, gameObjects[i]);
                        game.helper.DisplayText("Press 'Enter' to dock with mothership.");
                    }

                    else if (gameObjects[i] is Beacon && !game.player.HyperspeedOn)
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(game, spriteBatch, gameObjects[i]);
                        if (!((Beacon)gameObjects[i]).IsActivated)
                        {
                            game.helper.DisplayText("Press 'Enter' to activate beacon.");
                        }

                        else
                        {
                            game.helper.DisplayText("Press 'Enter' to interact with beacon.");
                        }
                    }
                }
            }
        }

        public void AddGameObject(GameObjectOverworld gameObject)
        {
            gameObjects.Add(gameObject);
        }
        public void RemoveGameObject(GameObjectOverworld gameObject)
        {
            if (gameObjects.Contains(gameObject))
                garbageGameObjects.Add(gameObject);
        }
        public ICollection<GameObjectOverworld> GetGameObjects()
        {
            return gameObjects;
        }
        public Boolean ContainsGameObject(GameObjectOverworld obj)
        {
            return gameObjects.Contains(obj);
        }

        private void DeleteRemovedGameObjects()
        {
            foreach (GameObjectOverworld obj in garbageGameObjects)
                gameObjects.Remove(obj);

            garbageGameObjects.Clear();
        }
    }
}
