using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class Planet: ImmobileSpaceObject
    {
        private Vector2 positionOffset;

        public string PlanetCodeName;
        public string ColonyCodeName;

        private float positionX;
        private float positionY;

        protected string planetSurface;
        protected float planetMass;
        protected float planetTemp;
        protected float planetGravity;
        protected float planetOrbit;
        protected float planetRotation;
        protected string colonyName;
        protected float colonyInhabitants;

        private ConfigFile planetDataConfigFile;
        private ConfigFile colonyDataConfigFile;

        protected Item tempItem;

        protected bool hasColony;

        protected List<string> resourceTypes;
        protected List<float> resourceCount;

        #region planet properties

        public string PlanetSurface
        {
            get { return planetSurface; }
        }

        public float PlanetMass
        {
            get { return planetMass; }
        }

        public float PlanetTemp
        {
            get { return planetTemp; }
        }

        public float PlanetGravity
        {
            get { return planetGravity; }
        }

        public float PlanetOrbit
        {
            get { return planetOrbit; }
        }

        public float PlanetRotation
        {
            get { return planetRotation; }
        }

        public string Habitable(float temp, float gravity)
        {
            {
                if (temp < 50 && temp > -40 && gravity <= 1.5f)
                    return "Yes";

                else
                    return "No";
            }
        }

        public bool HasColony { get { return hasColony; } set { hasColony = value; } }

        public string ColonyName
        {
            get { return colonyName; }
        }

        public float ColonyInhabitants
        {
            get { return colonyInhabitants; }
        }

        public List<string> ResourceTypes { get { return resourceTypes; } set { resourceTypes = value; } }
        public List<float> ResourceCount { get { return resourceCount; } set { resourceCount = value; } }

        #endregion

        protected Planet(Game1 Game, Sprite spriteSheet, Vector2 positionOffset) :
            base(Game, spriteSheet)
        {
            planetDataConfigFile = new ConfigFile();
            planetDataConfigFile.Load("Data/planetdata.dat");
            colonyDataConfigFile = new ConfigFile();
            colonyDataConfigFile.Load("Data/colonydata.dat");

            this.positionOffset = positionOffset;
        }

        public override void Initialize()
        {
            random = new Random();

            resourceTypes = new List<string>();
            resourceCount = new List<float>();

            Class = "Planet";
            layerDepth = 0.3f;
            speed = 0;
            scale = 1.0f;
            color = Color.White;

            shopInventory = new List<Item>();
            onEnterShopInventory = new List<Item>();
            itemPool = new List<Item>();

            base.Initialize();
        }

        protected void LoadPlanetData(String codeName)
        {
            name = planetDataConfigFile.GetPropertyAsString(codeName, "Name", "");
            positionX = planetDataConfigFile.GetPropertyAsFloat(codeName, "PositionX", 0);
            positionY = planetDataConfigFile.GetPropertyAsFloat(codeName, "PositionY", 0);
            position = new Vector2(positionX + positionOffset.X, positionY + positionOffset.Y);

            planetMass = planetDataConfigFile.GetPropertyAsFloat(codeName, "Mass", 0);
            planetTemp = planetDataConfigFile.GetPropertyAsFloat(codeName, "Temp", 0);
            planetGravity = planetDataConfigFile.GetPropertyAsFloat(codeName, "Gravity", 0);
            planetOrbit = planetDataConfigFile.GetPropertyAsFloat(codeName, "Orbit", 0);
            planetRotation = planetDataConfigFile.GetPropertyAsFloat(codeName, "Rotation", 0);
            planetSurface = planetDataConfigFile.GetPropertyAsString(codeName, "Surface", "");
            hasColony = planetDataConfigFile.GetPropertyAsBool(codeName, "HasColony", false);

            if (hasColony)
            {
                colonyName = colonyDataConfigFile.GetPropertyAsString(ColonyCodeName, "Name", "");
                colonyInhabitants = colonyDataConfigFile.GetPropertyAsFloat(ColonyCodeName, "Inhabitants", 0);
            }

        }

        public override void Update(GameTime gameTime)
        {
            if (GameStateManager.currentState == "OverworldState")
                IsUsed = true;
            else
                IsUsed = false;

            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsed == true)
            {
                base.Draw(spriteBatch);
            }
        }
    }
}
