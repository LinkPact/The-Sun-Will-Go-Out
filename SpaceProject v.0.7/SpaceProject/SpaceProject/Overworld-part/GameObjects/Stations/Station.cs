using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public enum ShopFilling
    { 
        verySparse,
        sparse,
        regular,
        filled,
        veryFilled,
        none
    }

    public class Station : ImmobileSpaceObject
    {
        #region declaration
        public string PlanetCodeName;
        public string StationCodeName;

        private float positionX;
        private float positionY;

        private Vector2 positionOffset;

        protected float stationInhabitants;

        private ConfigFile stationDataConfigFile;

        private bool abandoned;

        #endregion

        #region Properties

        public float StationInhabitants
        {
            get { return stationInhabitants; }
        }

        public bool Abandoned { get { return abandoned; } set { abandoned = value; } }

        #endregion

        protected Station(Game1 Game, Sprite SpriteSheet, Vector2 positionOffset) :
            base(Game, SpriteSheet)
        {
            abandoned = false;

            stationDataConfigFile = new ConfigFile();
            stationDataConfigFile.Load("Data/stationdata.dat");

            this.positionOffset = positionOffset;
        }

        public override void Initialize()
        {
            Class = "Station";
            layerDepth = 0.31f;
            speed = 0;
            scale = 1.0f;
            color = Color.White;

            shopInventory = new List<Item>();
            onEnterShopInventory = new List<Item>();
            itemPool = new List<Item>();

            base.Initialize();
        }

        protected void LoadStationData(string codeName)
        {
            name = stationDataConfigFile.GetPropertyAsString(codeName, "Name", "");
            positionX = stationDataConfigFile.GetPropertyAsFloat(codeName, "PositionX", 0);
            positionY = stationDataConfigFile.GetPropertyAsFloat(codeName, "PositionY", 0);

            position = new Vector2(positionX + positionOffset.X, positionY + positionOffset.Y);

            stationInhabitants = stationDataConfigFile.GetPropertyAsFloat(StationCodeName, "Inhabitants", 0);
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
            if (IsUsed)
            {
                base.Draw(spriteBatch);
            }
        }

    }
}
