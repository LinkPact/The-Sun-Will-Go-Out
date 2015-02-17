using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public class ShipSpawner
    {
        private Game1 game;
        private Sprite spriteSheet;
        private OverworldState overworld;
        private Sector sector;

        private int freightersInOverworld
        {
            get
            {
                return overworld.GetAllOverworldGameObjects.OfType<FreighterShip>().Count();
            }
        }
        private int rebelsInOverworld
        {
            get
            {
                return overworld.GetAllOverworldGameObjects.OfType<RebelShip>().Count();
            }
        }
        private int hangarsInOverworld
        {
            get
            {
                return overworld.GetAllOverworldGameObjects.OfType<HangarShip>().Count();
            }
        }
        private int allianceInOverworld
        {
            get
            {
                return overworld.GetAllOverworldGameObjects.OfType<AllianceShip>().Count();
            }
        }

        private int spawnLimitFreighters;
        private int spawnLimitHangars;
        private int spawnLimitRebels;
        private int spawnLimitAlliance;

        public ShipSpawner(Game1 game)
        {
            this.game = game;
        }
        public ShipSpawner(Game1 game, Sector sec)
        {
            this.game = game;
            Initialize(sec);
        }

        public void Initialize(Sector sec)
        {
            this.overworld = game.stateManager.overworldState;
            spriteSheet = overworld.shooterSheet;
            this.sector = sec;

            spawnLimitFreighters = 0;//4;
            spawnLimitHangars = 0;//3;
            spawnLimitRebels = 0;// 5;
            spawnLimitAlliance = 0;// 5;
        }

        public void AddRebelShip(Vector2 pos)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.position = pos;
            overworld.AddOverworldObject(tmpShip);
        }

        public void AddRebelShip(Vector2 pos, string levelToStart)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.position = pos; ;
            tmpShip.Level = levelToStart;
            overworld.AddOverworldObject(tmpShip);
        }

        public void AddRebelShip(Vector2 pos, string levelToStart, GameObjectOverworld target)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.position = pos;
            tmpShip.Level = levelToStart;
            tmpShip.SetTarget(target);
            overworld.AddOverworldObject(tmpShip);
        }

        public void AddOverworldShip(OverworldShip ship, Vector2 pos, string levelToStart, GameObjectOverworld target)
        {
            ship.position = pos;
            ship.Level = levelToStart;
            ship.SetTarget(target);
            overworld.AddOverworldObject(ship);
        }

        public List<OverworldShip> GetOverworldShips(int number, String type)
        {
            List<OverworldShip> shipList = new List<OverworldShip>();

            switch (type.ToLower())
            {
                case "rebel":
                    for (int i = 0; i < number; i++)
                    {
                        RebelShip tmpShip = new RebelShip(game, spriteSheet);
                        tmpShip.Initialize();
                        shipList.Add(tmpShip);
                    }
                    break;
            }

            return shipList;
        }

        public void AddFreighterToSector()
        {
            FreighterShip tempShip = new FreighterShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
        }

        public void AddHangarToSector()
        {
            HangarShip tempShip = new HangarShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
        }

        public void AddRebelToSector()
        {
            RebelShip tempShip = new RebelShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
        }

        public void AddAllianceToSector()
        {
            AllianceShip tempShip = new AllianceShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
        }

        public void AddFreighterToSector(FreighterShip freighter, Vector2 pos)
        {
            freighter.position = pos;
            overworld.AddOverworldObject(freighter);
        }

        public void Update(GameTime gameTime)
        {
            UpdateRebels(gameTime);
            UpdateAlliance(gameTime);
            UpdateFreighters(gameTime);
            UpdateHangars(gameTime);
        }

        public void UpdateRebels(GameTime gameTime)
        {
            if (rebelsInOverworld < spawnLimitRebels)
            {
                AddRebelToSector();
            }
        }

        public void UpdateAlliance(GameTime gameTime)
        {
            if (allianceInOverworld < spawnLimitAlliance)
            {
                AddAllianceToSector();
            }
        }

        public void UpdateFreighters(GameTime gameTime)
        {
            if (freightersInOverworld < spawnLimitFreighters)
            {
                AddFreighterToSector();
            }
        }

        public void UpdateHangars(GameTime gameTime)
        {
            if (hangarsInOverworld < spawnLimitHangars)
            {
                AddHangarToSector();
            }
        }
    }
}
