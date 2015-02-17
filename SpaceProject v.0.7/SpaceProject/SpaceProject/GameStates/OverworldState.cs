using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Globalization;


namespace SpaceProject
{
    public class OverworldState : GameState
    {
        public static readonly int OVERWORLD_WIDTH = 200000;
        public static readonly int OVERWORLD_HEIGHT = 200000;

        public List<GameObjectOverworld> GetAllOverworldGameObjects
        {
            get
            {
                List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
                tempList.AddRange(deepSpaceGameObjects);
                tempList.AddRange(sectorX.GetGameObjects());
                tempList.AddRange(outpostX.GetGameObjects());
                tempList.AddRange(borderXOutpost.GetGameObjects());
                tempList.AddRange(miningOutpost.GetGameObjects());
                tempList.AddRange(rebelOutpost.GetGameObjects());
                return tempList;
            }
        }

        public List<GameObjectOverworld> GetZoomObjects
        {
            get
            {
                List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
                tempList.Add(Game.player);
                tempList.Add(GetSectorX.GetGameObject("Soelara"));
                tempList.Add(GetSectorX.GetGameObject("system1Star"));
                foreach (GameObjectOverworld obj in GetAllOverworldGameObjects)
                {
                    if (obj is Planet 
                        || obj is Station)
                    {
                        tempList.Add(obj);
                    }
                }
                return tempList;
            }
        }

        public List<GameObjectOverworld> GetImmobileObjects
        {
            get
            {
                List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();
                tempList.AddRange(sectorX.GetGameObjects());
                tempList.AddRange(borderXOutpost.GetGameObjects());
                return tempList;
            }
        }

        private List<GameObjectOverworld> deepSpaceGameObjects;

        public List<GameObjectOverworld> GetDeepSpaceGameObjects
        {
            get
            {
                List<GameObjectOverworld> tempList = new List<GameObjectOverworld>();

                foreach (GameObjectOverworld obj in deepSpaceGameObjects)
                {
                    if (!garbageDeepSpaceGameObjects.Contains(obj))
                        tempList.Add(obj);
                }

                return tempList;
            }
        }
        private List<GameObjectOverworld> garbageDeepSpaceGameObjects;

        private List<GameObjectOverworld> effectsObjects;
        private List<GameObjectOverworld> garbageEffectsObjects;

        #region Space Regions

        private SpaceRegion previousSpaceRegion;
        private SpaceRegion currentSpaceRegion;

        public SpaceRegion GetCurrentSpaceRegion { get { return currentSpaceRegion; } private set { ; } }

        // Sectors
        private SectorX sectorX;
        public SectorX GetSectorX { get { return sectorX; } private set { ; } }

        // Outposts
        private OutpostX outpostX;
        public OutpostX GetOutpostX { get { return outpostX; } private set { ; } }

        private BorderXOutpost borderXOutpost;
        public BorderXOutpost GetBorderXOutpost { get { return borderXOutpost; } private set { ; } }

        private MiningOutpost miningOutpost;
        public MiningOutpost GetMiningOutpost { get { return miningOutpost; } private set { ; } }

        private RebelOutpost rebelOutpost;
        public RebelOutpost GetRebelOutpost { get { return rebelOutpost; } private set { ;} }

        #endregion

        #region Other Objects

        private PeyeScienceStation peyeScienceStation;

        public HeadsUpDisplay HUD;

        public BackgroundManagerOverworld bGManagerOverworld;

        #endregion

        private Camera camera;
        public Sprite spriteSheet;

        private BurnOutEnding burnOutEnding;

        public Sprite shooterSheet;
        private Sprite outpostSpriteSheet;

        private List<Beacon> beacons;
        public void AddBeacon(Beacon beacon)
        {
            beacons.Add(beacon);
        }

        public OverworldState(Game1 Game, string name) :
            base(Game, name)
        {
            this.Game = Game;
            Class = "play";
        }

        public override void Initialize()
        {
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/planetarySystemSpriteSheet"));
            shooterSheet = new Sprite(Game.Content.Load<Texture2D>("Vertical-Sprites/ShooterSheet"));
            outpostSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/OutpostSpriteSheet"));

            deepSpaceGameObjects = new List<GameObjectOverworld>();
            garbageDeepSpaceGameObjects = new List<GameObjectOverworld>();

            effectsObjects = new List<GameObjectOverworld>();
            garbageEffectsObjects = new List<GameObjectOverworld>();

            camera = new Camera(0, 0, 1, Game);
            camera.WorldWidth = OVERWORLD_WIDTH;
            camera.WorldHeight = OVERWORLD_HEIGHT;
            Game.camera = this.camera;

            // HUD
            HUD = new HeadsUpDisplay(this.Game);
            HUD.Initialize(spriteSheet, new Vector2(OVERWORLD_WIDTH / 2, OVERWORLD_HEIGHT / 2), new Vector2(OVERWORLD_WIDTH, OVERWORLD_HEIGHT));

            bGManagerOverworld = new BackgroundManagerOverworld(Game);
            bGManagerOverworld.Initialize();

            beacons = new List<Beacon>();

            // Sectors
            sectorX = new SectorX(Game);
            sectorX.Initialize();

            outpostX = new OutpostX(Game, outpostSpriteSheet);
            outpostX.Initialize();

            borderXOutpost = new BorderXOutpost(Game, outpostSpriteSheet);
            borderXOutpost.Initialize();

            miningOutpost = new MiningOutpost(Game, outpostSpriteSheet);
            miningOutpost.Initialize();

            rebelOutpost = new RebelOutpost(Game, outpostSpriteSheet);
            rebelOutpost.Initialize();

            currentSpaceRegion = sectorX;

            // Misc objects
            peyeScienceStation = new PeyeScienceStation(Game, outpostSpriteSheet, GetPlanet("Peye").position);

            deepSpaceGameObjects.Add(peyeScienceStation);
            deepSpaceGameObjects.Add(new MediumAsteroid(Game, shooterSheet, new Vector2(82550, 117980)));
            deepSpaceGameObjects.Add(new MediumAsteroid(Game, shooterSheet, new Vector2(82520, 118000)));
            deepSpaceGameObjects.Add(new MediumAsteroid(Game, shooterSheet, new Vector2(82490, 118021)));
            deepSpaceGameObjects.Add(new MediumAsteroid(Game, shooterSheet, new Vector2(82552, 118050)));
            deepSpaceGameObjects.Add(new MediumAsteroid(Game, shooterSheet, new Vector2(82575, 118075)));

            ActiveSong = Music.SpaceAmbience;

            foreach (GameObjectOverworld obj in deepSpaceGameObjects)
                obj.Initialize();

            foreach (Beacon beacon in beacons)
            {
                beacon.AddKnownBeacons(Game.stateManager.overworldState.GetAllOverworldGameObjects);
            }

            bGManagerOverworld.AddStar(spriteSheet);

            burnOutEnding = new BurnOutEnding(Game, spriteSheet);
            burnOutEnding.Initialize();

            base.Initialize();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (GameStateManager.previousState == "PlanetState" || GameStateManager.previousState == "StationState")
            {
                RemoveAllPirates();
            }

            if (GameStateManager.previousState.Equals("IntroSecondState") || GameStateManager.previousState.Equals("StartGameState"))
            {
                MissionManager.MarkMissionAsActive("Main - New First Mission");
                Game.stateManager.GotoStationSubScreen("Border Station", "Mission");
            }

            else
            {
                Game.musicManager.PlayMusic(ActiveSong);
            }
        }

        public override void OnLeave()
        {
        }

        public override void Update(GameTime gameTime)
        {
            Game.Window.Title = "The Sun Will Go Out" + " || " +
            "Pos: " + Game.player.position;

            StaticFunctions.CheckObjectUsage(Game, deepSpaceGameObjects);
            camera.CameraUpdate(gameTime, Game.player);

            DetermineCurrentRegion();

            sectorX.Update(gameTime);
            outpostX.Update(gameTime);
            borderXOutpost.Update(gameTime);
            miningOutpost.Update(gameTime);
            rebelOutpost.Update(gameTime);

            UpdateDeepSpaceObjects(gameTime);

            bGManagerOverworld.Update(gameTime);

            HUD.Update(gameTime, GetAllOverworldGameObjects);

            Inputhandling();

            if (burnOutEnding.Finished)
            {
                Game.stateManager.ChangeState("OutroState");
                ControlManager.EnableControls();
            }

            if (ControlManager.CheckKeypress(Keys.F12))
            {
                Game.player.speed = 0;
                ControlManager.DisableControls();
                burnOutEnding.Activate(Game.camera.Position, 100);
            }
            //InputhandlingDebug();

            EdgeCollisionCheck();

            if (garbageDeepSpaceGameObjects.Count > 0)
            {
                DeleteRemovedGameObjects();
            }

            if (garbageEffectsObjects.Count > 0)
            {
                DeleteRemovedEffects();
            }

            burnOutEnding.Update(gameTime);

            base.Update(gameTime);
        }

        private void UpdateDeepSpaceObjects(GameTime gameTime)
        {
            
            foreach (GameObjectOverworld obj in deepSpaceGameObjects)
            {
                obj.Update(gameTime);

                if (!Game.player.HyperspeedOn && OverworldShip.FollowPlayer)
                {
                    if (obj is OverworldShip && ((OverworldShip)obj).collisionEvent != null)
                    {
                        if (CollisionDetection.IsRectInRect(obj.Bounds, ((OverworldShip)obj).collisionEvent.target.Bounds))
                        {
                            ((OverworldShip)obj).collisionEvent.Invoke();
                        }                        
                    }
                }
            }

            if (PlanetState.PreviousPlanet != "")
            {
                foreach (GameObjectOverworld obj in deepSpaceGameObjects)
                {
                    if (obj is Planet)
                    {
                        Planet planet = (Planet)obj;

                        if (obj.name == PlanetState.PreviousPlanet)
                        {
                            Game.player.position = planet.position;
                            Game.player.speed = 0;
                            PlanetState.PreviousPlanet = "";
                            break;
                        }
                    }

                    else if (obj is Station)
                    {
                        Station station = (Station)obj;

                        if (station.name == StationState.PreviousStation)
                        {
                            Game.player.position = station.position;
                            Game.player.speed = 0;
                            StationState.PreviousStation = "";
                            break;
                        }
                    }
                }
            }

            foreach (GameObjectOverworld obj in effectsObjects)
            {
                obj.Update(gameTime);
                if (obj.IsDead)
                    RemoveEffectsObject(obj);
            }

            //if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            //{
            //    if (!Game.player.HyperspeedOn)
            //        EnterCheck();
            //}
        }

        private void EdgeCollisionCheck()
        {
            if (Game.player.position.X - Game.player.centerPoint.X < 0)
            {
                Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                Game.player.position.X = 0 + Game.player.centerPoint.X;
            }

            else if (Game.player.position.X + Game.player.centerPoint.X > Game.camera.WorldWidth)
            {
                Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                Game.player.position.X = Game.camera.WorldWidth - Game.player.centerPoint.X;
            }

            if (Game.player.position.Y - Game.player.centerPoint.Y < 0)
            {
                Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                Game.player.position.Y = 0 + Game.player.centerPoint.Y;
            }

            else if (Game.player.position.Y + Game.player.centerPoint.Y > Game.camera.WorldHeight)
            {
                Game.player.Direction.SetDirection(Game.player.Direction.GetDirectionAsVector() * -1);
                Game.player.position.Y = Game.camera.WorldHeight - Game.player.centerPoint.Y;
            }
        }

        private void Inputhandling()
        {
            if (ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeypress(Keys.Enter))
            {
                EnterCheck();
            }

            else if (ControlManager.CheckPress(RebindableKeys.Pause))
            {
                if (Game.messageBox.MessageState == MessageState.Invisible)
                {
                    Game.messageBox.DisplayMenu();
                }
            }

            else if (ControlManager.CheckKeypress(Keys.M))
            {
                Game.stateManager.ChangeState("MissionScreenState");
            }

            else if (ControlManager.CheckKeypress(Keys.I))
            {
                Game.stateManager.ChangeState("ShipManagerState");
                Game.stateManager.shooterState.Initialize();
            }
            else if (ControlManager.CheckKeypress(Keys.H))
            {
                Game.stateManager.ChangeState("HelpScreenState");
            }

            if (StatsManager.gameMode == GameMode.develop && ControlManager.CheckKeypress(Keys.U))
            {
                DevelopCommands();
            }
        }

        private void InputhandlingDebug()
        {
            ////Testing throw-item-menu
            //if (ControlManager.GamepadReady == false)
            //{
            //    if (ControlManager.PreviousKeyboardState.IsKeyUp(Keys.L)
            //       && ControlManager.CurrentKeyboardState.IsKeyDown(Keys.L))
            //    {
            //        List<Item> testList = new List<Item>();
            //
            //        Item item;
            //        Item item2;
            //        Item item3;
            //
            //        testList.Add(item = new BasicLaserWeapon(Game, ItemVariety.regular));
            //        testList.Add(item2 = new SpreadBulletWeapon(Game, ItemVariety.regular));
            //        testList.Add(item3 = new HomingMissileWeapon(Game, ItemVariety.regular));
            //
            //        Game.messageBox.DisplayTrashMenu(testList);
            //    }
            //}

            //Changes states to play-state and playerstats-state
            if (ControlManager.CheckKeypress(Keys.P))
            {
                Game.stateManager.shooterState.BeginLevel("Level1");
            }
            if (ControlManager.CheckKeypress(Keys.E))
            {
                Game.stateManager.shooterState.BeginLevel("ExperimentLevel");
            }

            if (ControlManager.CheckKeypress(Keys.O))
            {
                Game.stateManager.shooterState.BeginLevel("DanneLevel");
            }

            if (ControlManager.CheckKeypress(Keys.U))
            {
                Game.stateManager.shooterState.BeginLevel("EscortLevel");
            }
        }

        private void DevelopCommands()
        {
            Game.messageBox.DisplayMessage("Gameshops updated", false);
            ShopManager.ShopUpdateTime = 0;
        }

        private void DetermineCurrentRegion()
        {
            if (CollisionDetection.IsRectInRect(Game.player.Bounds, sectorX.SpaceRegionArea))
            {
                if (currentSpaceRegion != sectorX)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = sectorX;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, outpostX.SpaceRegionArea))
            {
                if (currentSpaceRegion != outpostX)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = outpostX;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, borderXOutpost.SpaceRegionArea))
            {
                if (currentSpaceRegion != borderXOutpost)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = borderXOutpost;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, miningOutpost.SpaceRegionArea))
            {
                if (currentSpaceRegion != miningOutpost)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = miningOutpost;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, rebelOutpost.SpaceRegionArea))
            {
                if (currentSpaceRegion != rebelOutpost)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = rebelOutpost;
                    currentSpaceRegion.OnEnter();
                }
            }

            else
            {
                if (currentSpaceRegion != null)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    previousSpaceRegion.OnLeave();
                    currentSpaceRegion = null;
                }
            }
        }

        // Collision-Detection for entering stations and planets located in deepspaceobjects
        private void EnterCheck()
        {
            for (int i = 0; i < deepSpaceGameObjects.Count; i++)
            {
                if (CollisionDetection.VisiblePixelsColliding(Game.player.Bounds, ((GameObjectOverworld)deepSpaceGameObjects[i]).Bounds,
                    Game.player.sprite, ((GameObjectOverworld)deepSpaceGameObjects[i]).sprite,
                    Game.player.centerPoint, ((GameObjectOverworld)deepSpaceGameObjects[i]).centerPoint) == true)
                {
                    if (deepSpaceGameObjects[i] is Planet)
                    {
                        Game.stateManager.planetState.LoadPlanetData((Planet)deepSpaceGameObjects[i]);
                        Game.stateManager.ChangeState("PlanetState");
                    }
        
                    else if (deepSpaceGameObjects[i] is Station)
                    {
                        Game.stateManager.stationState.LoadStationData((Station)deepSpaceGameObjects[i]);
                        Game.stateManager.ChangeState("StationState");
                    }
                }
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //if (currentSpaceRegion != null)
            //{
            //    currentSpaceRegion.Draw(spriteBatch);
            //}
            sectorX.Draw(spriteBatch);
            outpostX.Draw(spriteBatch);
            borderXOutpost.Draw(spriteBatch);
            miningOutpost.Draw(spriteBatch);
            rebelOutpost.Draw(spriteBatch);

            DrawDeepSpaceObjects(spriteBatch);

            Game.player.Draw(spriteBatch);

            if (!ZoomMap.IsMapOn)
            {
                bGManagerOverworld.Draw(spriteBatch);
                HUD.Draw(spriteBatch);
            }

            burnOutEnding.Draw(spriteBatch);

            base.Draw(spriteBatch);
        }

        private void DrawDeepSpaceObjects(SpriteBatch spriteBatch)
        {
            foreach (GameObjectOverworld obj in deepSpaceGameObjects)
            {
                obj.Draw(spriteBatch);
            }

            for (int i = 0; i < deepSpaceGameObjects.Count; i++)
            {
                if (CollisionDetection.IsRectInRect(Game.player.Bounds, deepSpaceGameObjects[i].Bounds))
                {
                    if (deepSpaceGameObjects[i].Class == "Planet")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i]);
                        Game.helper.DisplayText("Press 'Enter' to enter planetary orbit.");
                    }

                    else if (deepSpaceGameObjects[i].Class == "Station")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i]);
                        Game.helper.DisplayText("Press 'Enter' to dock with station.");
                    }

                    else if (deepSpaceGameObjects[i].Class == "MotherShip")
                    {
                        CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i]);
                        Game.helper.DisplayText("Press 'Enter' to dock with mothership.");
                    }

                    //else if (deepSpaceGameObjects[i] is Battlefield &&
                    //    MissionManager.GetMission("Main - A Cold Welcome").GetProgress().Equals(2))
                    //{
                    //    CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i]);
                    //    Game.helper.DisplayText("Press 'Enter' to investigate battlefield.");
                    //}
                }

                foreach (GameObjectOverworld obj in effectsObjects)
                {
                    obj.Draw(spriteBatch);
                }
            }
        }

        public Planet GetPlanet(String planetName)
        {
            foreach (GameObjectOverworld obj in GetAllOverworldGameObjects)
            {
                if (obj is Planet)
                {
                    if (obj.name.ToLower().Equals(planetName.ToLower()))
                        return (Planet)obj;
                }
            }

            throw new ArgumentException(String.Format("No planet by the name of '%s'", planetName));
        }
        public Station GetStation(String stationName)
        {
            foreach (GameObjectOverworld obj in GetAllOverworldGameObjects)
            {
                if (obj is Station)
                {
                    if (obj.name.ToLower().Equals(stationName.ToLower()))
                        return (Station)obj;
                }
            }

            throw new ArgumentException(String.Format("No station by the name of '%s'", stationName));
        }

        public void AddEffectsObject(GameObjectOverworld obj)
        {
            effectsObjects.Add(obj);
        }

        public void AddOverworldObject(GameObjectOverworld obj)
        {
            deepSpaceGameObjects.Add(obj);
        }

        public void RemoveOverworldObject(GameObjectOverworld obj)
        {
            if (deepSpaceGameObjects.Contains(obj) && !garbageDeepSpaceGameObjects.Contains(obj))
            {
                garbageDeepSpaceGameObjects.Add(obj);
                obj.FinalGoodbye();
            }
        }
        
        public void RemoveOverworldObject(int index)
        {
            garbageDeepSpaceGameObjects.Add(deepSpaceGameObjects[index]);
        }

        public void RemoveEffectsObject(GameObjectOverworld obj)
        {
            garbageEffectsObjects.Add(obj);
        }
        
        public Boolean ContainsOverworldObject(GameObjectOverworld obj)
        {
            return deepSpaceGameObjects.Contains(obj);
        }

        private void RemoveAllPirates()
        {
            List<GameObjectOverworld> removeObjects = new List<GameObjectOverworld>();
            foreach (GameObjectOverworld obj in deepSpaceGameObjects)
            {
                if ((obj is RebelShip || obj is AllianceShip)
                    && ((OverworldShip)obj).RemoveOnStationEnter)
                {
                    removeObjects.Add(obj);
                }
            }
            foreach (GameObjectOverworld obj in removeObjects)
            {
                deepSpaceGameObjects.Remove(obj);
            }
            removeObjects.Clear();
        }

        private void DeleteRemovedGameObjects()
        {
            foreach (GameObjectOverworld obj in garbageDeepSpaceGameObjects)
            {
                deepSpaceGameObjects.Remove(obj);
            }

            garbageDeepSpaceGameObjects.Clear();
        }

        private void DeleteRemovedEffects()
        {
            foreach (GameObjectOverworld obj in garbageEffectsObjects)
            {
                effectsObjects.Remove(obj);
            }

            garbageEffectsObjects.Clear();
        }

        private GameObjectOverworld GetCelestialBodyFromString(string classname)
        {
            Type type = Type.GetType(classname);
            foreach (GameObjectOverworld obj in GetImmobileObjects)
            {
                if (obj.GetType().Equals(type))
                    return obj;
            }
            return null;
        }

        public Beacon GetBeacon(String beacon)
        {
            for (int i = 0; i < beacons.Count; i++)
            {
                if (beacon.ToLower().Equals(beacons[i].name.ToLower()))
                {
                    return beacons[i];
                }
            }

            return null;
        }

        public void ActivateBurnOutEnding()
        {
            Game.player.speed = 0;
            ControlManager.DisableControls();
            burnOutEnding.Activate(Game.camera.Position, 100);
        }

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            // Save Ships
            saveData.Add("count", deepSpaceGameObjects.Count.ToString());
            Game.saveFile.Save("save.ini", "spaceobjects", saveData);

            for (int i = 0; i < deepSpaceGameObjects.Count; i++)
            {
                if (deepSpaceGameObjects[i] is OverworldShip)
                {
                    saveData.Clear();
                    saveData.Add("name", deepSpaceGameObjects[i].name);
                    saveData.Add("posx", Convert.ToString(deepSpaceGameObjects[i].position.X, CultureInfo.InvariantCulture));
                    saveData.Add("posy", Convert.ToString(deepSpaceGameObjects[i].position.Y, CultureInfo.InvariantCulture));
                    if (deepSpaceGameObjects[i] is RebelShip)
                    {
                        saveData.Add("level", ((RebelShip)deepSpaceGameObjects[i]).Level);
                    }
                    if (deepSpaceGameObjects[i] is FreighterShip)
                    {
                        saveData.Add("dest", ((FreighterShip)deepSpaceGameObjects[i]).destinationPlanet.ToString());
                    }
                    Game.saveFile.Save("save.ini", "spaceobj" + i, saveData);
                }

                else if (deepSpaceGameObjects[i] is SubInteractiveObject)
                {
                    ((SubInteractiveObject)deepSpaceGameObjects[i]).Save();
                }
            }

            List<GameObjectOverworld> sectorXObjects = sectorX.GetGameObjects();

            for (int i = 0; i < sectorXObjects.Count; i++)
            {
                if (sectorXObjects[i] is SubInteractiveObject)
                {
                    ((SubInteractiveObject)sectorXObjects[i]).Save();
                }
            }

            // Save Shops
            foreach (GameObjectOverworld obj in GetImmobileObjects)
            {
                if (obj is ImmobileSpaceObject)
                {
                    ImmobileSpaceObject tmpObj = (ImmobileSpaceObject)obj;
                    tmpObj.SaveShop();
                }
            }

            // Save sub interactive objects

        }

        public void Load()
        {
            int count = Game.saveFile.GetPropertyAsInt("spaceobjects", "count", 0);

            for (int i = 0; i < count; i++)
            {
                float posx = Game.saveFile.GetPropertyAsFloat("spaceobj" + i, "posx", 0);
                float posy = Game.saveFile.GetPropertyAsFloat("spaceobj" + i, "posy", 0);

                switch (Game.saveFile.GetPropertyAsString("spaceobj" + i, "name", "error"))
                {
                    case "Rebel Ship":
                        string levelName = Game.saveFile.GetPropertyAsString("spaceobj" + i, "level", "error");
                        sectorX.shipSpawner.AddRebelShip(new Vector2(posx, posy), levelName);
                        break;
                    case "Freighter Ship":
                        String destName = Game.saveFile.GetPropertyAsString("spaceobj" + i, "dest", "error");
                        GameObjectOverworld dest = GetCelestialBodyFromString(destName);
                        FreighterShip tmpShip = new FreighterShip(Game, Game.spriteSheetVerticalShooter);
                        tmpShip.Initialize();
                        tmpShip.SetEndPlanet(dest);
                        sectorX.shipSpawner.AddFreighterToSector(tmpShip, new Vector2(posx, posy));
                        break;
                    case "Alliance Ship":
                        sectorX.shipSpawner.AddRebelShip(new Vector2(posx, posy));
                        break;
                    case "Hangar Ship":
                        sectorX.shipSpawner.AddRebelShip(new Vector2(posx, posy));
                        break;
                    case "Ally Ship":
                        sectorX.shipSpawner.AddRebelShip(new Vector2(posx, posy));
                        break;
                }
            }

            // Load Shop Inventory
            foreach (GameObjectOverworld obj in GetImmobileObjects)
            {
                if (obj is ImmobileSpaceObject)
                {
                    ImmobileSpaceObject tmObj = (ImmobileSpaceObject)obj;
                    tmObj.LoadShop();
                }

                else if (obj is SubInteractiveObject)
                {
                    ((SubInteractiveObject)obj).Load();
                }
            }
        }
    }
}
