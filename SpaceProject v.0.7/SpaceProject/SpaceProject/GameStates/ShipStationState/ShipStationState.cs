using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{

    public class ShipStationState : GameState
    {
        private Random random;

        public static string PreviousShip;

        private List<string> textBuffer;
        public List<string> TextBuffer { get { return textBuffer; } set { textBuffer = value; } }

        private List<string> stateBuffer;
        public List<string> StateBuffer { get { return stateBuffer; } set { stateBuffer = value; } }

        public string LevelToStart;
        public string ReturnState;

        private Sprite spriteSheet;

        private List<TextBox> textBoxes;
        private Rectangle lowerScreenRectangle;

        #region Texture Fields

        private Vector2 shipTexturePosition;
        private Vector2 shipTextureOrigin;

        private Sprite lineTexture;

        #endregion

        #region Planet Data Fields

        //planet data variables
        private MissionObject ship;
        private Sprite shipSprite;
        private string shipName;
        private float shipScale;

        #endregion

        #region String Fields

        private string padding;

        private string dataHead;
        private Vector2 dataHeadStringPosition;
        private Vector2 dataHeadStringOrigin;

        private string dataBody;
        private Vector2 dataBodyStringPosition;

        private Vector2 nameStringPosition;
        private Vector2 nameStringOrigin;

        //private string iconExpl;
        private Vector2 iconExplPos;
        private Vector2 iconExplOrigin;

        #endregion

        #region Properties

        public Sprite SpriteSheet { get { return spriteSheet; } }

        public MissionObject Ship { get { return ship; } }

        #region String Properties

        //public SpriteFont FontBig { get { return Game.fontManager.GetFont(16); } }
        //public SpriteFont Game.fontManager.GetFont(14) { get { return Game.fontManager.GetFont(14); } }

        public string Padding { get { return padding; } set { padding = value; } }

        public string DataHead { get { return dataHead; } set { dataHead = value; } }
        public string DataBody { get { return dataBody; } set { dataBody = value; } }

        #endregion

        #endregion

        public ShipStationState(Game1 Game, string name) :
            base(Game, name)
        {
        }

        public override void Initialize()
        {
            random = new Random();

            textBuffer = new List<string>();
            stateBuffer = new List<string>();

            textBoxes = new List<TextBox>();

            lowerScreenRectangle = new Rectangle(Game.Window.ClientBounds.Width / 3 + 10,
                                                (Game.Window.ClientBounds.Height / 2) + 10,
                                                 (Game.Window.ClientBounds.Width * 2 / 3) - 20,
                                                (Game.Window.ClientBounds.Height / 2) - 20);

            #region Initailize Strings

            //Game.fontManager.GetFont(16) = Game.fontManager.GetFont(16);
            //Game.fontManager.GetFont(14) = Game.fontManager.GetFont(14);

            nameStringPosition = new Vector2(Game.Window.ClientBounds.Width * 3 / 4, 15);

            dataHeadStringPosition = new Vector2(Game.Window.ClientBounds.Width / 4, 30);

            dataBodyStringPosition = new Vector2(5, 60);

            padding = "";

            dataHead = "";
            dataBody = "";

            //iconExpl = "";
            iconExplPos = new Vector2(Game.Window.ClientBounds.Width / 6, Game.Window.ClientBounds.Height / 2 + 10);
            iconExplOrigin = Vector2.Zero;            

            #endregion

            #region Initailize Textures/Sprites

            shipTexturePosition = new Vector2(Game.Window.ClientBounds.Width / 2 + Game.Window.ClientBounds.Width / 4,
                                         Game.Window.ClientBounds.Height / 4);

            

            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/PlanetOverviewSpritesheet"),null);

            lineTexture = spriteSheet.GetSubSprite(new Rectangle(0, 0, 1, 1));

            #endregion          

        }

        //Method for loading data from the planet that the player has entered 
        public void LoadShipData(MissionObject ship)
       {
            shipSprite = ship.sprite;
            shipSprite.GetSubSprite(ship.sprite.SourceRectangle);

            shipName = ship.name;
            shipScale = ship.scale;

            shipTextureOrigin = new Vector2(ship.sprite.SourceRectangle.Value.Width / 2,
                                              ship.sprite.SourceRectangle.Value.Height / 2);

            dataBody = "";

            nameStringOrigin = Game.fontManager.GetFont(16).MeasureString(shipName) / 2;

            this.ship = ship;
        }

        private float ScaleShip(float diameter, float shipScale)
        {
            float preferredSize = Game.Window.ClientBounds.Width / 4f;
            float Scale = shipScale;

            while ((diameter * Scale) > preferredSize)
                Scale -= 0.01f;

            while ((diameter * Scale) < preferredSize)
                Scale += 0.01f;

            return Scale;
        }

        public override void OnEnter()
        {
            //iconExpl = "Colony";
            GetNextBufferLine();
        }

        public override void OnLeave()
        {
            PreviousShip = ship.name;
            ship = null;
            shipSprite = null;
            shipName = "";
        }

        private void GetNextBufferLine()
        {
            textBoxes.Clear();

            if (textBuffer.Count > 0)
            {
                textBoxes.Add(TextUtils.CreateTextBox(Game.fontManager.GetFont(16), lowerScreenRectangle, false, textBuffer[0] + "\n\n"
                    + "Press 'Enter' to continue..."));

                textBuffer.Remove(textBuffer[0]);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //Game.Window.Title = ("SpaceExplorationGame - " + "ShipOverview");

            if (((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
                if (textBuffer.Count > 0)
                {
                    GetNextBufferLine();
                }

                else
                    if (stateBuffer.Count > 0)
                    {
                        string tempState = stateBuffer[0];
                        stateBuffer.Remove(stateBuffer[0]);

                        if (tempState == "ShooterState")
                            Game.stateManager.shooterState.BeginLevel(LevelToStart);

                        else
                            Game.stateManager.ChangeState(tempState);
                    }

                    else
                        Game.stateManager.ChangeState(ReturnState);

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.SlateGray);

            Color lineColor = new Color(40, 40, 40, 255);

            #region Textures

            //Backdrop
            int spriteWidth = 92;
            int spriteHeight = 92;

            for (int i = 0; i < (int)((Game.Window.ClientBounds.Width / spriteWidth) + 1); i++)
            {
                for (int j = 0; j < (int)((Game.Window.ClientBounds.Height / spriteHeight) + 1); j++)
                    spriteBatch.Draw(spriteSheet.Texture, new Vector2(spriteWidth * i, spriteHeight * j),
                    new Rectangle(0, 241, 92, 92), Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            }

            //Draw Black texture around planet
            spriteBatch.Draw(spriteSheet.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 2 - 2,
                                           0),
                             new Rectangle(241, 3, 400, 300),
                             Color.White,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width / Game.DefaultResolution.X,
                                         Game.Window.ClientBounds.Height / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.1f);


            //Draw planet texture
            spriteBatch.Draw(shipSprite.Texture,
                             shipTexturePosition,
                             shipSprite.SourceRectangle,
                             Color.White,
                             .0f,
                             shipTextureOrigin,
                             ScaleShip(shipSprite.SourceRectangle.Value.Width, shipScale),
                             SpriteEffects.None,
                             .5f);

            //Draw lines
            //Vertical
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 2, 0),
                             lineTexture.SourceRectangle,
                             lineColor,
                             (float)(Math.PI * 90) / 180,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Height / 2, 2),
                             SpriteEffects.None,
                             .8f);

            //Vertical
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(Game.Window.ClientBounds.Width / 3, Game.Window.ClientBounds.Height / 2),
                             lineTexture.SourceRectangle,
                             lineColor,
                             (float)(Math.PI * 90) / 180,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Height / 2, 2),
                             SpriteEffects.None,
                             .8f);

            //Horizontal
            spriteBatch.Draw(lineTexture.Texture,
                             new Vector2(0, Game.Window.ClientBounds.Height / 2),
                             lineTexture.SourceRectangle,
                             lineColor,
                             0f,
                             Vector2.Zero,
                             new Vector2(Game.Window.ClientBounds.Width, 2),
                             SpriteEffects.None,
                             .8f);

            #endregion

            #region strings

            //Draw planet name string
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   shipName,
                                   nameStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   nameStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            ////Draw icon expl string
            //spriteBatch.DrawString(fontBig,
            //                       iconExpl,
            //                       iconExplPos,
            //                       Color.White,
            //                       .0f,
            //                       iconExplOrigin,
            //                       1.0f,
            //                       SpriteEffects.None,
            //                       .75f);

            dataHeadStringOrigin = new Vector2(Game.fontManager.GetFont(16).MeasureString(dataHead).X / 2, 5);

            //Draw planet data head
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   dataHead,
                                   dataHeadStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   dataHeadStringOrigin,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            //Draw planet data body
            spriteBatch.DrawString(Game.fontManager.GetFont(16),
                                   dataBody,
                                   dataBodyStringPosition + Game.fontManager.FontOffset,
                                   Game.fontManager.FontColor,
                                   .0f,
                                   Vector2.Zero,
                                   1.0f,
                                   SpriteEffects.None,
                                   .75f);

            foreach (TextBox textBox in textBoxes)
                textBox.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);

            #endregion
        }

    }
}
