﻿using System;
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

        private int piratesInOverworld;
        private int freightersInOverworld;
        private int rebelsInOverworld;

        private int spawnLimitPirates;
        private int spawnLimitFreighters;

        //private int spawnLimitRebels;

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

            spawnLimitPirates = 5;
            spawnLimitFreighters = 4;
            //spawnLimitRebels = 5;
        }

        public void AddPirateShip(Vector2 pos)
        {
            PirateShip tmpShip = new PirateShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.SetPosition(pos);
            overworld.AddOverworldObject(tmpShip);
            piratesInOverworld++;
        }

        public void AddRebelShip(Vector2 pos)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.SetPosition(pos);
            overworld.AddOverworldObject(tmpShip);
            rebelsInOverworld++;
        }

        public void AddRebelShip(Vector2 pos, string levelToStart)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.SetPosition(pos);
            tmpShip.SetLevel(levelToStart);
            overworld.AddOverworldObject(tmpShip);
            rebelsInOverworld++;
        }

        public void AddRebelShip(Vector2 pos, string levelToStart, GameObjectOverworld target)
        {
            RebelShip tmpShip = new RebelShip(game, spriteSheet);
            tmpShip.Initialize();
            tmpShip.SetPosition(pos);
            tmpShip.SetLevel(levelToStart);
            tmpShip.SetTarget(target);
            overworld.AddOverworldObject(tmpShip);
            rebelsInOverworld++;
        }

        public void AddPirateToSector()
        {
            PirateShip tempShip = new PirateShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
            piratesInOverworld++;
        }

        public void AddFreighterToSector()
        {
            FreighterShip tempShip = new FreighterShip(game, spriteSheet);
            tempShip.Initialize(sector);
            overworld.AddOverworldObject(tempShip);
            freightersInOverworld++;

        }

        public void AddFreighterToSector(Vector2 pos)
        {
            FreighterShip tempShip = new FreighterShip(game, spriteSheet);
            tempShip.Initialize(sector);
            tempShip.SetPosition(pos);
            overworld.AddOverworldObject(tempShip);
            freightersInOverworld++;

        }

        public void AddFreighterToSector(Vector2 pos, GameObjectOverworld startingPoint, GameObjectOverworld destination)
        {
            FreighterShip tempShip = new FreighterShip(game, spriteSheet);
            tempShip.Initialize(sector, startingPoint, destination);
            tempShip.SetPosition(pos);
            overworld.AddOverworldObject(tempShip);
            freightersInOverworld++;

        }

        public void AddFreighterToSector(FreighterShip freighter, Vector2 pos)
        {
            freighter.SetPosition(pos);
            overworld.AddOverworldObject(freighter);
            freightersInOverworld++;
        }

        public void RemovePirateShip()
        {
            piratesInOverworld--;
        }

        public void RemoveAllPirateShips()
        {
            piratesInOverworld = 0;
        }

        public void RemoveFreighterShip()
        {
            freightersInOverworld--;
        }

        public void Update(GameTime gameTime)
        {
            UpdatePirates(gameTime);
            UpdateFreighters(gameTime);
        }

        public void UpdatePirates(GameTime gameTime)
        {
            if (piratesInOverworld < spawnLimitPirates)
            {
                AddPirateToSector();
            }
        }

        public void UpdateFreighters(GameTime gameTime)
        {
            if (freightersInOverworld < spawnLimitFreighters)
            {
                AddFreighterToSector();
            }
        }
    }
}