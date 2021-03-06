﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SectorX : Sector
    {
        private SectorXStar sectorXStar;
        public SectorXStar SectorXStar { get { return sectorXStar; } private set { ;} }

        private Lavis lavis;
        private Fortrun fortrun;
        private Highfence highfence;
        private NewNorrland newNorrland;
        private Peye peye;

        private SoelaraStation soelaraStation;
        private LavisStation lavisStation;
        private FortrunStation1 fortrunStation1;
        private InformationStation informationStation;

        private HighfenceShop highfenceShop;
        private FortrunShop fortrunShop;
        private PeyeShop peyeShop;

        private Beacon highfenceBeacon;
        private Beacon peyeBeacon;
        private Beacon lavisBeacon;
        private Beacon fortrunBeacon;
        private Beacon soelaraBeacon;
        private Beacon newNorrlandBeacon;

        private TrainingArea2 trainingArea2;

        // Sub interactive objects
        private List<SubInteractiveObject> subInteractiveObjects = new List<SubInteractiveObject>();

        private Soelara soelara;
        private FortrunStation2 fortrunStation2;
        private LonelyAsteroid lonelyAsteroid;
        private DamagedShip damagedShip;
        private SpaceDuck spaceDuck;

        public SectorX(Game1 game) :
            base(game)
        { }

        public override void Initialize()
        {
            base.Initialize();

            name = "Sector X";

            spriteSheet = new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/SectorXSpriteSheet"), null);

            spaceRegionArea = new Rectangle(85000, 85000, 30000, 30000);
            Vector2 offset = new Vector2(spaceRegionArea.X, spaceRegionArea.Y);

            sectorXStar = new SectorXStar(game, spriteSheet);
            sectorXStar.Initialize();

            // Sub-interactive objects
            soelara = new Soelara(game, spriteSheet);
            fortrun = new Fortrun(game, spriteSheet);
            lavis = new Lavis(game, spriteSheet);
            informationStation = new InformationStation(game, spriteSheet);
            fortrunStation2 = new FortrunStation2(game, spriteSheet);
            lonelyAsteroid = new LonelyAsteroid(game, spriteSheet);
            damagedShip = new DamagedShip(game, new Sprite(game.Content.Load<Texture2D>("Overworld-Sprites/MissionObjectSpriteSheet"), null));
            spaceDuck = new SpaceDuck(game, spriteSheet);

            var rebelAsteroidFieldGenerator = new RebelOutpostAsteroidField(game, spriteSheet);
            var sunAsteroidBeltGenerator = new SunAsteroidBelt(game, spriteSheet);
            var lavisAsteroidBeltGenerator = new LavisAsteroidBelt(game, spriteSheet);
            var westernAsteroidFieldGenerator = new WesternAsteroidField(game, spriteSheet);
            var telmunAsteroidFieldGenerator = new TelmunAsteroidBelt(game, spriteSheet);

            // For testing purposes // Jakob 150623
            //var testFieldGenerator = new TestingField(game, spriteSheet);
            //subInteractiveObjects.AddRange(testFieldGenerator.GetAsteroids());

            subInteractiveObjects.AddRange(rebelAsteroidFieldGenerator.GetAsteroids());
            subInteractiveObjects.AddRange(sunAsteroidBeltGenerator.GetAsteroids());
            subInteractiveObjects.AddRange(lavisAsteroidBeltGenerator.GetAsteroids());
            subInteractiveObjects.AddRange(westernAsteroidFieldGenerator.GetAsteroids());
            subInteractiveObjects.AddRange(telmunAsteroidFieldGenerator.GetAsteroids());

            subInteractiveObjects.Add(soelara);
            subInteractiveObjects.Add(fortrun);
            subInteractiveObjects.Add(lavis);
            subInteractiveObjects.Add(fortrunStation2);
            subInteractiveObjects.Add(lonelyAsteroid);
            subInteractiveObjects.Add(damagedShip);
            subInteractiveObjects.Add(spaceDuck);
            subInteractiveObjects.Add(informationStation);

            foreach (var obj in subInteractiveObjects)
            {
                obj.Initialize();
            }

            // Planets
            highfence = new Highfence(game, spriteSheet, offset);
            newNorrland = new NewNorrland(game, spriteSheet, offset);
            peye = new Peye(game, spriteSheet, offset);
            highfence.Initialize();
            newNorrland.Initialize();
            peye.Initialize();

            // Stations
            lavisStation = new LavisStation(game, spriteSheet, lavis.position);
            soelaraStation = new SoelaraStation(game, spriteSheet, soelara.position);
            fortrunStation1 = new FortrunStation1(game, spriteSheet, fortrun.position);
            lavisStation.Initialize();
            soelaraStation.Initialize();
            fortrunStation1.Initialize();

            // Shops
            highfenceShop = new HighfenceShop(game, spriteSheet, highfence.position);
            fortrunShop = new FortrunShop(game, spriteSheet, fortrun.position);
            peyeShop = new PeyeShop(game, spriteSheet, peye.position);
            highfenceShop.Initialize();
            fortrunShop.Initialize();
            peyeShop.Initialize();

            // Training area
            var outpostSpriteSheet = game.stateManager.overworldState.outpostSpriteSheet;
            trainingArea2 = new TrainingArea2(game, outpostSpriteSheet, MathFunctions.CoordinateToPosition(new Vector2(-1680, 10)));
            trainingArea2.Initialize();

            highfenceBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Highfence Beacon", highfence.position + new Vector2(300, 250));
            highfenceBeacon.Initialize();

            fortrunBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Fortrun Beacon", fortrun.position + new Vector2(350, 100));
            fortrunBeacon.Initialize();

            peyeBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Peye Beacon", peye.position + new Vector2(-200, -450));
            peyeBeacon.Initialize();

            lavisBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Lavis Beacon", lavis.position + new Vector2(150, 350));
            lavisBeacon.Initialize();

            soelaraBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Soelara Beacon", soelara.position + new Vector2(300, 350));
            soelaraBeacon.Initialize();

            newNorrlandBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "New Norrland Beacon", newNorrland.position + new Vector2(-300, 100));
            newNorrlandBeacon.Initialize();

            game.stateManager.overworldState.AddBeacon(highfenceBeacon);
            game.stateManager.overworldState.AddBeacon(fortrunBeacon);
            game.stateManager.overworldState.AddBeacon(peyeBeacon);
            game.stateManager.overworldState.AddBeacon(lavisBeacon);
            game.stateManager.overworldState.AddBeacon(soelaraBeacon);
            game.stateManager.overworldState.AddBeacon(newNorrlandBeacon);

            AddGameObject(sectorXStar);

            AddGameObject(highfence);
            AddGameObject(newNorrland);
            AddGameObject(peye);

            AddGameObject(lavisStation);
            AddGameObject(fortrunStation1);
            AddGameObject(soelaraStation);
            AddGameObject(trainingArea2);

            AddGameObject(highfenceShop);
            AddGameObject(fortrunShop);
            AddGameObject(peyeShop);

            AddGameObject(highfenceBeacon);
            AddGameObject(fortrunBeacon);
            AddGameObject(peyeBeacon);
            AddGameObject(lavisBeacon);
            AddGameObject(soelaraBeacon);
            AddGameObject(newNorrlandBeacon);
            
            foreach (var obj in subInteractiveObjects)
            {
                AddGameObject(obj);
            }
        }

        public override void OnEnter()
        {
            base.OnEnter();
        }

        public override void OnLeave()
        {
            base.OnLeave();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
