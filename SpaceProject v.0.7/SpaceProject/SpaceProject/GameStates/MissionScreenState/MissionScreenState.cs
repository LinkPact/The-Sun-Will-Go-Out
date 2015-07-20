using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class MissionScreenState: GameState
    {
        #region initVariables
        
        //Technical
        private Sprite spriteSheet;
        private int elapsedTimeMilliseconds;
        private int elapsedSinceKey;
        private Sprite menuSpriteSheet;

        //Visuell kuriosa
        private Sprite lineTexture;

        private MissionScreenCursor cursorManager;
        private MissionScreenText fontManager;
        private MissionInformation informationManager;

        private const int BG_WIDTH = 92;
        private const int BG_HEIGHT = 92;

        //Cursor-related variables
        private int cursorLevel;
        private int cursorLevel1Position;
        private int cursorLevel2Position;

        private static Rectangle upperLeftRectangle;
        private static Rectangle lowerLeftRectangle;
        private static Rectangle rightRectangle;

        public static Rectangle GetUpperLeftRectangle { get { return upperLeftRectangle; } private set { ;} }
        public static Rectangle GetLowerLeftRectangle { get { return lowerLeftRectangle; } private set { ;} }
        public static Rectangle GetRightRectangle { get { return rightRectangle; } private set { ;} }
        #endregion

        public MissionScreenState(Game1 Game, String name) :
            base(Game, name)
        {
            this.Game = Game;
            spriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/missionScreenSpriteSheet"));
        }

        public override void Initialize() 
        {
            upperLeftRectangle = new Rectangle(0, 0,
                (int)Game.Window.ClientBounds.Width / 2, (int)Game.Window.ClientBounds.Height / 3);

            lowerLeftRectangle = new Rectangle(0, upperLeftRectangle.Height,
                upperLeftRectangle.Width, (int)Game.Window.ClientBounds.Width * 2 / 3);

            rightRectangle = new Rectangle(lowerLeftRectangle.Width, 0,
                lowerLeftRectangle.Width, (int)Game.Window.ClientBounds.Height);

            //Managers for cursor and text.
            cursorManager = new MissionScreenCursor(Game, spriteSheet);
            cursorManager.Initialize();
            fontManager = new MissionScreenText(Game);
            fontManager.Initialize();
            informationManager = new MissionInformation(Game);
            informationManager.Initialize();

            //Data about current active user position.
            cursorLevel = 1;
            cursorLevel1Position = 0;
            cursorLevel2Position = 0;

            elapsedSinceKey = 0;
        }

        public override void OnEnter()
        {
            menuSpriteSheet = new Sprite(Game.Content.Load<Texture2D>("Overworld-Sprites/missionScreenSpriteSheet"));
            elapsedTimeMilliseconds = 0;

            //Kuriosa
            lineTexture = menuSpriteSheet.GetSubSprite(new Rectangle(0, 0, 1, 1));

            cursorLevel1Position = 0;
            cursorLevel2Position = 0;
        }

        public override void OnLeave() 
        { }

        public override void Update(GameTime gameTime)
        {
            elapsedTimeMilliseconds += gameTime.ElapsedGameTime.Milliseconds;

            CheckKeys();

            cursorManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel2Position);
            fontManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel2Position);
            informationManager.Update(gameTime, cursorLevel, cursorLevel1Position, cursorLevel2Position, "MissionScreenState");

            elapsedSinceKey += gameTime.ElapsedGameTime.Milliseconds;

        }
        
        public override void Draw(SpriteBatch spriteBatch)
        {
            Game.GraphicsDevice.Clear(Color.White);

            DrawBackground(spriteBatch);

            cursorManager.Draw(spriteBatch);
            fontManager.Draw(spriteBatch);
            informationManager.Draw(spriteBatch);

        }

        private void DrawBackground(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(spriteSheet.Texture,
                 new Vector2(0, 0),
                 new Rectangle(162, 0, 1280, 720),
                 Color.White,
                 0f,
                 Vector2.Zero,
                 new Vector2(Game.Window.ClientBounds.Width / 1280f,
                             Game.Window.ClientBounds.Height / 720f),
                 SpriteEffects.None,
                 0.0f);
        }

        private void CheckKeysCursorLevel1()
        {
            int temporaryCount = cursorManager.displayList.Count;
            
            if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > 100)
            {
                if (cursorLevel1Position == 0) { cursorLevel1Position = 3; }
                else if (cursorLevel1Position == 1) { cursorLevel1Position = 0; }
                else if (cursorLevel1Position == 2) { cursorLevel1Position = 1; }
                else if (cursorLevel1Position == 3) { cursorLevel1Position = 2; }
                
                elapsedSinceKey = 0;
            }
            
            if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > 100)
            {
                if (cursorLevel1Position == 0) { cursorLevel1Position = 1; }
                else if (cursorLevel1Position == 1) { cursorLevel1Position = 2; }
                else if (cursorLevel1Position == 2) { cursorLevel1Position = 3; }
                else if (cursorLevel1Position == 3) { cursorLevel1Position = 0; }
                
                elapsedSinceKey = 0;
            
                elapsedSinceKey = 0;
            }
            
            if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) 
                && elapsedSinceKey > 100)
            {
                OnPressCursorLevel1();
            }
        }

        private void CheckMouseCursorLevel1()
        {
            for (int i = 0; i < cursorManager.displayList.Count; i++)
            {
                if (ControlManager.IsMouseOverArea(cursorManager.displayList[i].Bounds))
                {
                    cursorLevel1Position = i;

                    if (ControlManager.IsLeftMouseButtonClicked())
                    {
                        OnPressCursorLevel1();
                    }
                }
            }
        }

        private void CheckKeysCursorLevel2()
        {
            if (cursorLevel1Position == 0)
            {
                if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position += 1;

                    if (cursorLevel2Position > MissionManager.ReturnActiveMissions().Count)
                        cursorLevel2Position = 0;

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position -= 1;
                    if (cursorLevel2Position < 0)
                        cursorLevel2Position = MissionManager.ReturnActiveMissions().Count;

                    elapsedSinceKey = 0;
                }

                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) 
                    && elapsedSinceKey > 100)
                {
                    int missionCount = MissionManager.ReturnActiveMissions().Count;

                    if (missionCount > 0 &&
                        cursorLevel2Position == missionCount)
                    {
                        cursorLevel = 1;
                    }

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > 100)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }
            }

            else if (cursorLevel1Position == 1)
            {

                if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position += 1;
                    if (cursorLevel2Position > MissionManager.ReturnCompletedDeadMissions().Count)
                        cursorLevel2Position = 0;

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position -= 1;
                    if (cursorLevel2Position < 0)
                        cursorLevel2Position = MissionManager.ReturnCompletedDeadMissions().Count;

                    elapsedSinceKey = 0;
                }

                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) 
                    && elapsedSinceKey > 100)
                {
                    int missionCount = MissionManager.ReturnCompletedDeadMissions().Count;

                    if (missionCount > 0 &&
                        cursorLevel2Position == missionCount)
                    {
                        cursorLevel = 1;
                    }

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > 100)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }
            }

            else if (cursorLevel1Position == 2)
            {
                if (ControlManager.CheckPress(RebindableKeys.Down) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position += 1;
                    if (cursorLevel2Position > MissionManager.ReturnFailedDeadMissions().Count)
                        cursorLevel2Position = 0;

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Up) && elapsedSinceKey > 100)
                {
                    cursorLevel2Position -= 1;
                    if (cursorLevel2Position < 0)
                        cursorLevel2Position = MissionManager.ReturnFailedDeadMissions().Count;

                    elapsedSinceKey = 0;
                }

                if ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeyPress(Keys.Enter)) 
                    && elapsedSinceKey > 100)
                {
                    int missionCount = MissionManager.ReturnFailedDeadMissions().Count;

                    if (missionCount > 0 &&
                        cursorLevel2Position == missionCount)
                    {
                        cursorLevel = 1;
                    }

                    elapsedSinceKey = 0;
                }

                if (ControlManager.CheckPress(RebindableKeys.Action2) && elapsedSinceKey > 100)
                {
                    cursorLevel = 1;
                    elapsedSinceKey = 0;
                }
            }

        }

        private void CheckMouseCursorLevel2()
        {
            List<Mission> missions;

            switch (cursorLevel1Position)
            {
                case 0:
                    missions = MissionManager.ReturnActiveMissions();
                    break;

                case 1:
                    missions = MissionManager.ReturnCompletedDeadMissions();
                    break;

                case 2:
                    missions = MissionManager.ReturnFailedDeadMissions();
                    break;

                default:
                    missions = new List<Mission>();
                    break;
            }

            for (int i = 0; i < missions.Count + 1; i++)
            {
                string text = i < missions.Count ? missions[i].MissionName : "Back";

                if (ControlManager.IsMouseOverText(FontManager.GetFontStatic(14), text,
                    new Vector2(MissionScreenState.GetRightRectangle.X + Game.Window.ClientBounds.Width / 16,
                                93 + i * 23) + Game.fontManager.FontOffset, Vector2.Zero, false))
                {
                    cursorLevel2Position = i;

                    if (ControlManager.IsLeftMouseButtonClicked()
                        && i == missions.Count)
                    {
                        cursorLevel = 1;
                        elapsedSinceKey = 0;
                    }
                }
            }
        }

        private void OnPressCursorLevel1()
        {
            if (cursorLevel1Position == 0 && MissionManager.ReturnActiveMissions().Count > 0)
            {
                cursorLevel = 2;
                cursorLevel2Position = 0;
                elapsedSinceKey = 0;
            }

            else if (cursorLevel1Position == 1 && MissionManager.ReturnCompletedDeadMissions().Count > 0)
            {
                cursorLevel = 2;
                cursorLevel2Position = 0;
                elapsedSinceKey = 0;
            }

            else if (cursorLevel1Position == 2 && MissionManager.ReturnFailedDeadMissions().Count > 0)
            {
                cursorLevel = 2;
                cursorLevel2Position = 0;
                elapsedSinceKey = 0;
            }

            else if (cursorLevel1Position == 3)
            {
                Game.stateManager.ChangeState("OverworldState");
                cursorLevel1Position = 0;
                elapsedSinceKey = 0;
            }
        }

        private void CheckStateChangeCommands()
        {
            //State-Changers
            if (ControlManager.CheckPress(RebindableKeys.Pause)
                && elapsedSinceKey > 100 && cursorLevel == 1)
            {
                Game.stateManager.ChangeState(GameStateManager.previousState);
            }

            if (ControlManager.CheckPress(RebindableKeys.Action2)
                && elapsedSinceKey > 100 && cursorLevel == 1)
            {
                Game.stateManager.ChangeState(GameStateManager.previousState);
            }

            if (ControlManager.CheckPress(RebindableKeys.Pause)
                && elapsedSinceKey > 100 && cursorLevel > 1)
            {
                cursorLevel -= 1;
                elapsedSinceKey = 0;
            }

            if (ControlManager.CheckPress(RebindableKeys.Missions) && elapsedTimeMilliseconds > 200
                && elapsedSinceKey > 100)
            {
                Game.stateManager.ChangeState(GameStateManager.previousState);
            }
        }

        private void CheckKeys()
        {
            CheckStateChangeCommands();

            if (cursorLevel == 1)
            {
                CheckKeysCursorLevel1();
                CheckMouseCursorLevel1();
            }
            else if (cursorLevel == 2)
            {
                CheckKeysCursorLevel2();
                CheckMouseCursorLevel2();
            }
        }
    }
}
