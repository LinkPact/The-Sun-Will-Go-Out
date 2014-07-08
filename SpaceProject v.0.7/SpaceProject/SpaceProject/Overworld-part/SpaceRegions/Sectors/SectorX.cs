using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class SectorX : Sector
    {
        private System1Star sectorXStar;
        public System1Star SectorXStar { get { return sectorXStar; } private set { ;} }

        private Soelara soelara;
        private Lavis lavis;
        private Fotrun fotrun;
        private Highfence highfence;
        private NewNorrland newNorrland;
        private Peye peye;

        private SoelaraStation soelaraStation;
        private LavisStation lavisStation;
        private FotrunStation1 fotrunStation1;
        private FotrunStation2 fotrunStation2;

        private List<Beacon> beacons;
        private Beacon highfenceBeacon;
        private Beacon peyeBeacon;
        private Beacon lavisBeacon;
        private Beacon fotrunBeacon;
        private Beacon soelaraBeacon;
        private Beacon newNorrlandBeacon;

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

            sectorXStar = new System1Star(game, spriteSheet);
            sectorXStar.Initialize();

            soelara = new Soelara(game, spriteSheet, offset);
            lavis = new Lavis(game, spriteSheet, offset);
            fotrun = new Fotrun(game, spriteSheet, offset);
            highfence = new Highfence(game, spriteSheet, offset);
            newNorrland = new NewNorrland(game, spriteSheet, offset);
            peye = new Peye(game, spriteSheet, offset);
            soelara.Initialize();
            lavis.Initialize();
            fotrun.Initialize();
            highfence.Initialize();
            newNorrland.Initialize();
            peye.Initialize();

            lavisStation = new LavisStation(game, spriteSheet, lavis.position);
            soelaraStation = new SoelaraStation(game, spriteSheet, soelara.position);
            fotrunStation1 = new FotrunStation1(game, spriteSheet, fotrun.position);
            fotrunStation2 = new FotrunStation2(game, spriteSheet, fotrun.position);
            lavisStation.Initialize();
            soelaraStation.Initialize();
            fotrunStation1.Initialize();
            fotrunStation2.Initialize();

            beacons = new List<Beacon>();

            highfenceBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Highfence Beacon", highfence.position + new Vector2(300, 250));
            highfenceBeacon.Initialize();

            fotrunBeacon = new Beacon(game, spriteSheet, new Rectangle(588, 844, 100, 100), new Rectangle(487, 844, 100, 100),
                "Fotrun Beacon", fotrun.position + new Vector2(-400, -200));
            fotrunBeacon.Initialize();

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

            beacons.Add(highfenceBeacon);
            beacons.Add(fotrunBeacon);
            beacons.Add(peyeBeacon);
            beacons.Add(lavisBeacon);
            beacons.Add(soelaraBeacon);
            beacons.Add(newNorrlandBeacon);

            AddGameObject(sectorXStar);

            AddGameObject(soelara);
            AddGameObject(lavis);
            AddGameObject(fotrun);
            AddGameObject(highfence);
            AddGameObject(newNorrland);
            AddGameObject(peye);

            AddGameObject(soelaraStation);
            AddGameObject(lavisStation);
            AddGameObject(fotrunStation1);
            AddGameObject(fotrunStation2);

            AddGameObject(highfenceBeacon);
            AddGameObject(fotrunBeacon);
            AddGameObject(peyeBeacon);
            AddGameObject(lavisBeacon);
            AddGameObject(soelaraBeacon);
            AddGameObject(newNorrlandBeacon);

            highfenceBeacon.AddKnownBeacons(gameObjects);
            fotrunBeacon.AddKnownBeacons(gameObjects);
            peyeBeacon.AddKnownBeacons(gameObjects);
            lavisBeacon.AddKnownBeacons(gameObjects);
            soelaraBeacon.AddKnownBeacons(gameObjects);
            newNorrlandBeacon.AddKnownBeacons(gameObjects);

            //AddGameObject(new PirateShip(game, system1SpriteSheet));
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
            //Initializes heat warning of player if player is too close to the sun
            if (!game.player.HyperspeedOn && CollisionDetection.IsPointInsideCircle(game.player.position, sectorXStar.position,
                sectorXStar.ObjectHeatRadius))
            {   
                game.player.OnDamage(sectorXStar);
            }

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
    }
}
