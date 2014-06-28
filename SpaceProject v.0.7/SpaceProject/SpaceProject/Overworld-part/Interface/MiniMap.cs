using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    //
    // Class to create a simple minimap
    //
    class MiniMap
    {
        private Game1 Game;

        // List of all positions of the objects on screen
        private List<Vector2> objektsOnMap = null;
        private Vector2 playerpos;

        public Vector2 Origin;
        public int Height;
        public int Width;
        public float scaleX;
        public float scaleY;

        public Sprite ObjectSprite;
        protected Sprite spriteSheet;
        protected Sprite radar;

        // -------------------------------------
        public MiniMap(Game1 game, Sprite spriteSheet)
        {
            this.Game = game;
            this.spriteSheet = spriteSheet;
        }

        // Secondary Constructor 
        // (optional, provided if you would whish to create and initialize the map at the same time)
        //---------------------------------------
        public MiniMap(Game1 game, Sprite spriteSheet, Vector2 origin, Vector2 worldSize)
        {
            this.Game = game;
            this.spriteSheet = spriteSheet;
            Initialize(origin, worldSize);
        }

        //---------------------------------------
        public void Initialize(Vector2 Origin, Vector2 worldSize)
        {
            objektsOnMap = new List<Vector2>();
            this.Origin = Origin;
            Height = 198;
            Width = 198;
            scaleX = worldSize.X / Width;
            scaleY = worldSize.Y / Height;

            ObjectSprite = spriteSheet.GetSubSprite(new Rectangle(0, 3, 3, 3));

            radar = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/radar"), new Rectangle(0, 0, 198, 198));
        }

        // Copy all the positions of the objects in the system, also update the position of the camera
        //---------------------------------------
        public void Update(GameTime gameTime, List<GameObjectOverworld> objects, Vector2 cameraPos, Vector2 systemPos)
        {
            objektsOnMap.Clear();
            foreach (GameObjectOverworld obj in objects)
            {

                objektsOnMap.Add(StaticFunctions.NormalizePosition(obj.position, systemPos));

            }
            playerpos = StaticFunctions.NormalizePosition(Game.player.position, systemPos);
            Origin = new Vector2(cameraPos.X + Game.Window.ClientBounds.Width / 2 - radar.SourceRectangle.Value.Width,
                cameraPos.Y + Game.Window.ClientBounds.Height / 2 - radar.SourceRectangle.Value.Height);

        }

        //---------------------------------------
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(radar.Texture,
                    Origin,
                    radar.SourceRectangle,
                    Color.White,
                    0.0f,
                    new Vector2(0, 0),
                    1.0f,
                    SpriteEffects.None,
                    0.9f
                    );

            foreach (Vector2 obj in objektsOnMap)
            {
                spriteBatch.Draw(ObjectSprite.Texture,
                    new Vector2(Origin.X + (obj.X / scaleX), Origin.Y + (obj.Y / scaleY)),
                    ObjectSprite.SourceRectangle,
                    Color.Red,
                    0.0f,
                    new Vector2(ObjectSprite.SourceRectangle.Value.Width / 2, ObjectSprite.SourceRectangle.Value.Height / 2),
                    2.0f,
                    SpriteEffects.None,
                    0.91f
                    );
            }

            spriteBatch.Draw(ObjectSprite.Texture,
                new Vector2(Origin.X + (playerpos.X / scaleX), Origin.Y + (playerpos.Y / scaleY)),
                ObjectSprite.SourceRectangle,
                Color.Beige,
                0.0f,
                new Vector2(ObjectSprite.SourceRectangle.Value.Width / 2, ObjectSprite.SourceRectangle.Value.Height / 2),
                2.0f,
                SpriteEffects.None,
                0.911f
                );
        }
    }
}
