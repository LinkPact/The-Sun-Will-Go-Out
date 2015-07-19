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

        private readonly float BeaconActivationRadius = 300;

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
                tempList.Add(GetSectorX.GetGameObject("Lavis"));
                tempList.Add(GetSectorX.GetGameObject("Fortrun"));
                tempList.Add(GetSectorX.GetGameObject("Star"));
                tempList.Add(GetRebelOutpost.GetGameObject("Rebel Base"));
                foreach (GameObjectOverworld obj in GetAllOverworldGameObjects)
                {
                    if (obj is Planet
                        && !obj.name.Equals("Mysterious Asteroid"))
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

        private HighFenceOrbit highFenceOrbit;
        public HighFenceOrbit GethighFenceOrbit { get { return highFenceOrbit; } private set { ;} }

        private FortrunOrbit fortrunOrbit;
        public FortrunOrbit GetFortrunOrbit { get { return fortrunOrbit; } private set { ;} }

        private NewNorrlandOrbit newNorrlandOrbit;
        public NewNorrlandOrbit GetNewNorrlandOrbit { get { return newNorrlandOrbit; } private set { ;} }

        private SoelaraOrbit soelaraOrbit;
        public SoelaraOrbit GetSoelaraOrbit { get { return soelaraOrbit; } private set { ;} }

        private LavisOrbit lavisOrbit;
        public LavisOrbit GetLavisOrbit { get { return lavisOrbit; } private set { ;} }

        private PeyeOrbit peyeOrbit;
        public PeyeOrbit GetPeyeOrbit { get { return peyeOrbit; } private set { ;} }

        public List<SpaceRegion> RestrictedSpaceRebels
        {
            get
            {
                List<SpaceRegion> tempList = new List<SpaceRegion>();
                tempList.Add(highFenceOrbit);
                tempList.Add(fortrunOrbit);
                tempList.Add(newNorrlandOrbit);
                tempList.Add(soelaraOrbit);
                tempList.Add(lavisOrbit);
                tempList.Add(peyeOrbit);
                return tempList;
            }
        }

        public List<SpaceRegion> RestrictedSpaceAlliance
        {
            get
            {
                List<SpaceRegion> tempList = new List<SpaceRegion>();
                tempList.Add(highFenceOrbit);
                tempList.Add(newNorrlandOrbit);
                tempList.Add(soelaraOrbit);
                tempList.Add(lavisOrbit);
                tempList.Add(peyeOrbit);
                return tempList;
            }
        }

        #endregion

        #region Other Objects

        private PeyeScienceStation peyeScienceStation;
        public HeadsUpDisplay HUD;
        public BackgroundManagerOverworld bGManagerOverworld;

        #endregion

        private Camera camera;
        public Sprite planetarySystemSpriteSheet;

        private BurnOutEnding burnOutEnding;
        public bool IsBurnOutEndingActivated { get { return burnOutEnding.Activated; } }

        public Sprite shooterSheet;
        public Sprite outpostSpriteSheet;

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
            planetarySystemSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/planetarySystemSpriteSheet"));
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
            HUD.Initialize(planetarySystemSpriteSheet, new Vector2(OVERWORLD_WIDTH / 2, OVERWORLD_HEIGHT / 2), new Vector2(OVERWORLD_WIDTH, OVERWORLD_HEIGHT));

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

            highFenceOrbit = new HighFenceOrbit(Game, outpostSpriteSheet);
            highFenceOrbit.Initialize();

            fortrunOrbit = new FortrunOrbit(Game, outpostSpriteSheet);
            fortrunOrbit.Initialize();

            newNorrlandOrbit = new NewNorrlandOrbit(Game, outpostSpriteSheet);
            newNorrlandOrbit.Initialize();

            soelaraOrbit = new SoelaraOrbit(Game, outpostSpriteSheet);
            soelaraOrbit.Initialize();

            lavisOrbit = new LavisOrbit(Game, outpostSpriteSheet);
            lavisOrbit.Initialize();

            peyeOrbit = new PeyeOrbit(Game, outpostSpriteSheet);
            peyeOrbit.Initialize();

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

            bGManagerOverworld.AddStar(planetarySystemSpriteSheet);

            burnOutEnding = new BurnOutEnding(Game, planetarySystemSpriteSheet);
            burnOutEnding.Initialize();

            base.Initialize();
        }

        public override void OnEnter()
        {
            base.OnEnter();

            if (GameStateManager.previousState.Equals("IntroSecondState") || GameStateManager.previousState.Equals("StartGameState"))
            {
                MissionManager.MarkMissionAsActive(MissionID.Main1_1_RebelsInTheAsteroids);
                Game.stateManager.GotoStationSubScreen("Border Station", "Mission");
            }

            else
            {
                Game.musicManager.PlayMusic(ActiveSong);
            }

            if (Game.SaveOnEnterOverworld)
            {
                Game.SaveOnEnterOverworld = false;
                Game.AutoSave();
            }
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            StaticFunctions.CheckObjectUsage(Game, deepSpaceGameObjects);

            camera.CameraUpdate(gameTime, Game.player);

            DetermineCurrentRegion();

            sectorX.Update(gameTime);
            outpostX.Update(gameTime);
            borderXOutpost.Update(gameTime);
            miningOutpost.Update(gameTime);
            rebelOutpost.Update(gameTime);
            highFenceOrbit.Update(gameTime);
            fortrunOrbit.Update(gameTime);
            newNorrlandOrbit.Update(gameTime);
            soelaraOrbit.Update(gameTime);
            lavisOrbit.Update(gameTime);
            peyeOrbit.Update(gameTime);

            UpdateDeepSpaceObjects(gameTime);

            bGManagerOverworld.Update(gameTime);

            HUD.Update(gameTime, GetAllOverworldGameObjects);

            if (ZoomMap.MapState == MapState.Off)
            {
                Inputhandling();
            }

            if (burnOutEnding.Finished
                && GameStateManager.currentState.Equals("OverworldState"))
            {
                Game.stateManager.outroState.SetOutroType(OutroType.OnYourOwnEnd);
                Game.stateManager.ChangeState("OutroState");
                Game.player.EnableControls();
            }

            if (StatsManager.gameMode == GameMode.Develop)
            {
                InputhandlingDebug();
            }

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

            foreach (Beacon beacon in beacons)
            {
                if (Vector2.Distance(Game.player.position, beacon.position) < BeaconActivationRadius
                    && !beacon.IsActivated)
                {
                    beacon.PlayerGetsClose();
                }
            }

            base.Update(gameTime);
        }

        private void UpdateDeepSpaceObjects(GameTime gameTime)
        {     
            foreach (GameObjectOverworld obj in deepSpaceGameObjects)
            {
                obj.Update(gameTime);

                if (!Game.player.HyperspeedOn && OverworldShip.FollowPlayer && !Game.player.IsInvincible)
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

            if (PlanetState.PreviousPlanet != ""
                || StationState.PreviousStation != "")
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
            if ((ControlManager.CheckPress(RebindableKeys.Action1)
                || ControlManager.CheckKeyPress(Keys.Enter)))
            {
                EnterCheck();
            }

            else if (ControlManager.CheckPress(RebindableKeys.Pause)
                && Game.player.IsControlsEnabled
                && (MissionManager.GetMission(MissionID.Main5_Retribution).MissionState != StateOfMission.Active
                || (MissionManager.GetMission(MissionID.Main5_Retribution).ObjectiveIndex <= 2 
                || MissionManager.GetMission(MissionID.Main5_Retribution).ObjectiveIndex >= 6))) 
            {
                PopupHandler.DisplayMenu();
            }

            else if (ControlManager.CheckKeyPress(Keys.M))
            {
                Game.stateManager.ChangeState("MissionScreenState");
            }

            else if (ControlManager.CheckKeyPress(Keys.I))
            {
                Game.stateManager.ChangeState("ShipManagerState");
                Game.stateManager.shooterState.Initialize();
            }
            else if (ControlManager.CheckKeyPress(Keys.H))
            {
                Game.stateManager.ChangeState("HelpScreenState");
            }

            if (StatsManager.gameMode == GameMode.Develop && ControlManager.CheckKeyPress(Keys.U))
            {
                DevelopCommands();
            }
        }

        private void InputhandlingDebug()
        {
            //Changes states to play-state and playerstats-state
            if (ControlManager.CheckKeyPress(Keys.P))
            {
                Game.stateManager.shooterState.BeginLevel("Level1");
            }
            if (ControlManager.CheckKeyPress(Keys.E))
            {
                Game.stateManager.shooterState.BeginLevel("ExperimentLevel");
            }

            if (ControlManager.CheckKeyPress(Keys.U))
            {
                Game.stateManager.shooterState.BeginLevel("EscortLevel");
            }

            if (ControlManager.CheckKeyPress(Keys.Y))
            {
                PopupHandler.DisplayMessage("Player position: " + Game.player.position.ToString());
            }

            if (ControlManager.CheckKeyPress(Keys.F12))
            {
                ActivateBurnOutEnding();
            }
        }

        // Collision-Detection for entering stations and planets located in deepspaceobjects
        private void EnterCheck()
        {
            for (int i = 0; i < deepSpaceGameObjects.Count; i++)
            {
                if (CollisionDetection.IsRectInRect(Game.player.Bounds, deepSpaceGameObjects[i].Bounds))
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

                    else if (deepSpaceGameObjects[i] is OverworldShip)
                    {
                        ((OverworldShip)deepSpaceGameObjects[i]).Interact();
                    }
                }
            }
        }

        private void DevelopCommands()
        {
            PopupHandler.DisplayMessage("Gameshops updated");
            ShopManager.ShopUpdateTime = 0;
        }

        private void DetermineCurrentRegion()
        {
            if (CollisionDetection.IsRectInRect(Game.player.Bounds, highFenceOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != highFenceOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = highFenceOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, fortrunOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != fortrunOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = fortrunOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, newNorrlandOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != newNorrlandOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = newNorrlandOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, soelaraOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != soelaraOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = soelaraOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, lavisOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != lavisOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = lavisOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, peyeOrbit.SpaceRegionArea))
            {
                if (currentSpaceRegion != peyeOrbit)
                {
                    previousSpaceRegion = currentSpaceRegion;
                    currentSpaceRegion = peyeOrbit;
                    currentSpaceRegion.OnEnter();
                }
            }

            else if (CollisionDetection.IsRectInRect(Game.player.Bounds, sectorX.SpaceRegionArea))
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
            highFenceOrbit.Draw(spriteBatch);
            fortrunOrbit.Draw(spriteBatch);
            newNorrlandOrbit.Draw(spriteBatch);
            soelaraOrbit.Draw(spriteBatch);
            lavisOrbit.Draw(spriteBatch);
            peyeOrbit.Draw(spriteBatch);

            DrawDeepSpaceObjects(spriteBatch);

            Game.player.Draw(spriteBatch);

            if (!ZoomMap.IsMapOn)
            {
                bGManagerOverworld.Draw(spriteBatch);

                if (!IsBurnOutEndingActivated)
                {
                    HUD.Draw(spriteBatch);
                }
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
                    if (!burnOutEnding.Activated)
                    {
                        if (deepSpaceGameObjects[i].Class == "Planet")
                        {
                            CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i].Bounds);
                            Game.helper.DisplayText("Press 'Enter' to enter planetary orbit.");
                        }
                
                        else if (deepSpaceGameObjects[i].Class == "Station")
                        {
                            CollisionHandlingOverWorld.DrawRectAroundObject(Game, spriteBatch, deepSpaceGameObjects[i].Bounds);
                            Game.helper.DisplayText("Press 'Enter' to dock with station.");
                        }
                    }
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
                if ((obj is RebelShip || obj is AllianceShip || obj is HangarShip)
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
            Game.player.DisableControls();
            burnOutEnding.Activate(Game.camera.Position, 25);
            Game.musicManager.PlayMusic(Music.none);
        }

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();

            // Save Ships
            saveData.Add("count", deepSpaceGameObjects.Count.ToString());
            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "spaceobjects", saveData);

            for (int i = 0; i < deepSpaceGameObjects.Count; i++)
            {
                if (deepSpaceGameObjects[i] is OverworldShip &&
                    ((OverworldShip)deepSpaceGameObjects[i]).SaveShip)
                {
                    saveData.Clear();
                    saveData.Add("name", deepSpaceGameObjects[i].name);
                    saveData.Add("posx", Convert.ToString(deepSpaceGameObjects[i].position.X, CultureInfo.InvariantCulture));
                    saveData.Add("posy", Convert.ToString(deepSpaceGameObjects[i].position.Y, CultureInfo.InvariantCulture));
                    if (deepSpaceGameObjects[i] is RebelShip)
                    {
                        if (((RebelShip)deepSpaceGameObjects[i]).Level != null && ((RebelShip)deepSpaceGameObjects[i]).Level != "")
                            saveData.Add("level", ((RebelShip)deepSpaceGameObjects[i]).Level);
                    }
                    if (deepSpaceGameObjects[i] is FreighterShip)
                    {
                        saveData.Add("dest", ((FreighterShip)deepSpaceGameObjects[i]).destinationPlanet.ToString());
                    }
                    Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "spaceobj" + i, saveData);
                }

                else if (deepSpaceGameObjects[i] is SubInteractiveObject)
                {
                    ((SubInteractiveObject)deepSpaceGameObjects[i]).Save();
                }
            }

            // Save sub interactive objects
            List<GameObjectOverworld> sectorXObjects = sectorX.GetGameObjects();

            for (int i = 0; i < sectorXObjects.Count; i++)
            {
                if (sectorXObjects[i] is SubInteractiveObject)
                {
                    ((SubInteractiveObject)sectorXObjects[i]).Save();
                }
            }
        }

        public void Load()
        {
            int count = Game.saveFile.GetPropertyAsInt("spaceobjects", "count", 0);
            RemoveAllPirates();

            for (int i = 0; i < count; i++)
            {
                float posx = Game.saveFile.GetPropertyAsFloat("spaceobj" + i, "posx", 0);
                float posy = Game.saveFile.GetPropertyAsFloat("spaceobj" + i, "posy", 0);

                switch (Game.saveFile.GetPropertyAsString("spaceobj" + i, "name", "error"))
                {
                    case "Rebel Ship":
                        string levelName = Game.saveFile.GetPropertyAsString("spaceobj" + i, "level", "");
                        sectorX.shipSpawner.AddRebelShip(new Vector2(posx, posy), levelName);
                        break;
                    case "Freighter Ship":
                        String destName = Game.saveFile.GetPropertyAsString("spaceobj" + i, "dest", "error");
                        GameObjectOverworld dest = GetCelestialBodyFromString(destName);
                        FreighterShip tmpShip = new FreighterShip(Game, Game.spriteSheetVerticalShooter);
                        tmpShip.Initialize();
                        tmpShip.SetSector(Game.stateManager.overworldState.GetSectorX);
                        tmpShip.SetDefaultBehaviour();
                        tmpShip.SetEndPlanet(dest);
                        sectorX.shipSpawner.AddFreighterToSector(tmpShip, new Vector2(posx, posy));
                        break;
                    case "Alliance Ship":
                        sectorX.shipSpawner.AddAllianceShip(new Vector2(posx, posy));
                        break;
                    case "Hangar Ship":
                        sectorX.shipSpawner.AddHangarShip(new Vector2(posx, posy));
                        break;
                }
            }

            // Load Shop Inventory
            foreach (GameObjectOverworld obj in GetImmobileObjects)
            {
                if (obj is SubInteractiveObject)
                {
                    ((SubInteractiveObject)obj).Load();
                }
            }
        }
    }
}
