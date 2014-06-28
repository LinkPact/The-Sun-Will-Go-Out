using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class BaseState : GameState
    {
        protected Sprite spriteSheet;

        protected Planet planet;
        protected Station station;

        #region String Fields

        protected string padding;

        protected string dataHead;
        protected Vector2 dataHeadStringPosition;
        protected Vector2 dataHeadStringOrigin;

        protected string dataBody;
        protected Vector2 dataBodyStringPosition;

        protected Vector2 nameStringPosition;
        protected Vector2 nameStringOrigin;

        protected string iconExpl;
        protected Vector2 iconExplPos;
        protected Vector2 iconExplOrigin;

        #endregion

        public Sprite SpriteSheet { get { return spriteSheet; } }

        #region String Properties

        public string Padding { get { return padding; } set { padding = value; } }

        public string DataHead { get { return dataHead; } set { dataHead = value; } }
        public string DataBody { get { return dataBody; } set { dataBody = value; } }

        #endregion

        public GameObjectOverworld GetBase()
        {
            if (this is PlanetState)
            {
                return planet;
            }

            else
            {
                return station;
            }
        }

        protected BaseState(Game1 Game, string Name) :
            base (Game, Name) 
        {

        }

        public override void Initialize()
        {
            base.Initialize();
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
