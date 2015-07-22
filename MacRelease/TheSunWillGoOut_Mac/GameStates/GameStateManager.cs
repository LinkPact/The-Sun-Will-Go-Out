using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class GameStateManager
    {
        #region init

        private List<GameState> gameStates;
        public GameState currentGameState { get; private set; }
        private Game1 Game;

        public static string currentState;
        public static string previousState;
        public static string previousPreviousState;

        public MainMenuState mainMenuState;
        public StartGameState startGameState;
        public OptionsMenuState optionsMenuState;
        public HelpScreenState helpScreenState;
        public IntroFirstState introFirstState;
        public IntroSecondState introSecondState;
        public OutroState outroState;
        public OverworldState overworldState;
        public ShooterState shooterState;
        public ShipManagerState shipManagerState;
        public PlanetState planetState;
        public StationState stationState;
        public MissionScreenState missionScreenState;
        
        public MapCreatorState mapCreatorState;
        public LevelTesterState levelTesterState;
        public CampaignState campaignState;
        public CreditState creditState;

        #endregion

        public GameStateManager(Game1 Game)
        {
            this.Game = Game;

            currentState = "";
            previousState = "";
        }

        public void ChangeState(string name)
        {
            for (int i = 0; i < gameStates.Count; i++)
            {
                if (gameStates[i].Name == name)
                {
                    if (currentGameState != null)
                        currentGameState.OnLeave();

                    if (previousState != null)
                    {
                        previousPreviousState = previousState;
                    }
                    previousState = currentState;

                    currentGameState = gameStates[i];
                    currentState = currentGameState.Name;
                    
                    currentGameState.OnEnter();                 
                }
            }
        }

        public void StartGame(string name)
        {
            overworldState.Initialize();
            shooterState.Initialize();
            shipManagerState.Initialize();
            planetState.Initialize();
            stationState.Initialize();
            missionScreenState.Initialize();

            ChangeState(name);
        }

        public void GotoPlanetSubScreen(String planetName, String planetSubState)
        {
            Planet planet = Game.stateManager.overworldState.GetPlanet(planetName);
            Game.player.position = planet.position;

            Game.stateManager.planetState.LoadPlanetData(planet);
            ChangeState("PlanetState");

            if (!planetSubState.Equals("Overview"))
                Game.stateManager.planetState.SubStateManager.ChangeMenuSubState(planetSubState);
        }

        public void GotoStationSubScreen(String stationName, String stationSubScreen)
        {
            Station station = Game.stateManager.overworldState.GetStation(stationName);
            Game.player.position = station.position;

            Game.stateManager.stationState.LoadStationData(station);
            Game.stateManager.ChangeState("StationState");

            if (!stationSubScreen.Equals("Overview"))
                Game.stateManager.stationState.SubStateManager.ChangeMenuSubState(stationSubScreen);
        }

        public void GotoMapCreatorTestRun(String fileName)
        {
            float testStartTime = 0;
            shooterState.SetupMapCreatorTestRun(fileName, testStartTime);
            shooterState.BeginLevel("testRun");
        }

        public void Initialize()
        {
            gameStates = new List<GameState>();

            mainMenuState = new MainMenuState(Game, "MainMenuState");
            startGameState = new StartGameState(Game, "StartGameState");
            optionsMenuState = new OptionsMenuState(Game, "OptionsMenuState");
            helpScreenState = new HelpScreenState(Game, "HelpScreenState");
            introFirstState = new IntroFirstState(Game, "IntroFirstState");
            introSecondState = new IntroSecondState(Game, "IntroSecondState");
            outroState = new OutroState(Game, "OutroState");
            overworldState = new OverworldState(Game, "OverworldState");
            mapCreatorState = new MapCreatorState(Game, "MapCreatorState");
            shooterState = new ShooterState(Game, "ShooterState");
            shipManagerState = new ShipManagerState(Game, "ShipManagerState");
            planetState = new PlanetState(Game, "PlanetState");
            stationState = new StationState(Game, "StationState");
            missionScreenState = new MissionScreenState(Game, "MissionScreenState");
            creditState = new CreditState(Game, "CreditState");

            levelTesterState = new LevelTesterState(Game, "LevelTesterState");
            campaignState = new CampaignState(Game, "CampaignState");

            gameStates.Add(mainMenuState);
            gameStates.Add(startGameState);
            gameStates.Add(optionsMenuState);
            gameStates.Add(helpScreenState);
            gameStates.Add(introFirstState);
            gameStates.Add(introSecondState);
            gameStates.Add(outroState);
            gameStates.Add(overworldState);
            gameStates.Add(shooterState);
            gameStates.Add(shipManagerState);
            gameStates.Add(planetState);
            gameStates.Add(stationState);
            gameStates.Add(missionScreenState);
            gameStates.Add(mapCreatorState);
            gameStates.Add(creditState);

            gameStates.Add(levelTesterState);
            gameStates.Add(campaignState);

            mainMenuState.Initialize();
            startGameState.Initialize();
            optionsMenuState.Initialize();
            helpScreenState.Initialize();
            introFirstState.Initialize();
            introSecondState.Initialize();
            outroState.Initialize();
            mapCreatorState.Initialize();
            
            overworldState.Initialize();
            shooterState.Initialize();
            shipManagerState.Initialize();
            planetState.Initialize();
            stationState.Initialize();
            missionScreenState.Initialize();

            creditState.Initialize();
            levelTesterState.Initialize();
            campaignState.Initialize();

            ChangeState("MainMenuState");
        }

        public void Update(GameTime gameTime)
        {
            if (currentGameState != null)
                currentGameState.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (currentGameState != null)
                currentGameState.Draw(spriteBatch);
        }

        public void ResetState(String name)
        {
            for (int n = 0; n < gameStates.Count; n++)
            {
                if (name == gameStates[n].Name)
                {
                    gameStates[n].Initialize();
                }
            }
        }
    }
}
