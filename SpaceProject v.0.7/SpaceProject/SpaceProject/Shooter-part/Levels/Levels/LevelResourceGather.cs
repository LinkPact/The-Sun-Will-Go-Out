using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public class LevelResourceGather : Level
    {
        #region declaration

        public float spawnDelayMeteor;
        public float spawnDelayEnemy;
        public float spawnDelayResource;
        private float timeSinceSpawnMeteor;
        private float timeSinceSpawnEnemy;
        private float timesinceSpawnResource;

        private int surviveTime = 40;

        private Planet previousPlanet;
        private List<string> resourceTypes = new List<string>();
        private List<float> resources = new List<float>();
        private List<float> initialResources = new List<float>();

        private List<float> resourceTimings = new List<float>();
        private float currentTiming;
        private int currentTimingIndex;

        private List<float> copperStones = new List<float>();
        private List<float> goldStones = new List<float>();
        private List<float> titaniumStones = new List<float>();

        private List<string> enemyTypes = new List<string>();
        private List<int> numberOfEnemies = new List<int>();

        //Reset variables
        private string startingEnemy;
        private int startingEnemyCount;

        private List<string> startingEnemies = new List<string>();
        private List<int> startingEnemiesCount = new List<int>();

        #endregion

        public void SetUpLevel(Planet planet)
        {
            previousPlanet = planet;
            startingEnemy = "";
        }

        public void SetUpLevel(Planet planet, string enemy, int enemyCount)
        {
            previousPlanet = planet;

            this.enemyTypes.Clear();
            this.numberOfEnemies.Clear();

            this.enemyTypes.Add(enemy);
            this.numberOfEnemies.Add(enemyCount);

            startingEnemy = enemy;
            startingEnemyCount = enemyCount;
        }

        public void SetUpLevel(Planet planet, List<string> enemies, List<int> enemyCount)
        {
            previousPlanet = planet;

            this.enemyTypes.Clear();
            numberOfEnemies.Clear();

            for (int i = 0; i < enemies.Count; i++)
            {
                this.enemyTypes.Add(enemies[i]);
                this.numberOfEnemies.Add(enemyCount[i]);
            }

            startingEnemy = "";

            startingEnemies = enemies;
            startingEnemiesCount = enemyCount;
        }

        private void DistributeResources(string type, float amount)
        {
            if (type.Equals("Copper"))
                copperStones = DistributeStones(amount, type);
            if (type.Equals("Gold"))
                goldStones = DistributeStones(amount, type);
            if (type.Equals("Titanium"))
                titaniumStones = DistributeStones(amount, type);
        }

        private List<float> DistributeStones(float resources, string type)
        {
            //List containing the sizes of the stones.
            List<float> resourceList = new List<float>();
            Random rand = new Random();

            if (type.Equals("Copper")) copperStones.Clear();
            if (type.Equals("Gold")) goldStones.Clear();
            if (type.Equals("Titanium")) titaniumStones.Clear(); 
            
            while (resources > 0)
            {
                //Random number to amount of resources.
                int val = rand.Next(2, 30);

                if (resources > val)
                {
                    resourceList.Add(val);
                    resources -= val;
                }
                else
                {
                    resourceList.Add(resources);
                    break;
                }
            }

            return resourceList;
        }

        public LevelResourceGather(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, MissionType missionType)
            : base(Game, spriteSheet, player1, missionType)
        {
            this.Name = "ResourceGatherLevel";
        }

        public override void Initialize()
        {
            base.Initialize();
            LevelWidth = 800;

            LevelEvent swarm = new EvenSwarm(Game, player, spriteSheet, this, EnemyType.R_mosquito, 0, 15000, 5);
            untriggeredEvents.Add(swarm);


            resourceTypes.Clear();
            resources.Clear();

            for (int i = 0; i < previousPlanet.ResourceTypes.Count; i++)
            {
                if (previousPlanet.ResourceCount[i] > 0)
                {
                    resourceTypes.Add(previousPlanet.ResourceTypes[i]);
                    resources.Add(previousPlanet.ResourceCount[i]);

                    DistributeResources(previousPlanet.ResourceTypes[i], previousPlanet.ResourceCount[i]);
                }
            }
            initialResources = resources;

            spawnDelayMeteor = 150.0f;
            spawnDelayEnemy = 1250;

            resourceTimings = SetResourceSpawnDelays();
            currentTimingIndex = 0;
            currentTiming = resourceTimings[currentTimingIndex];

            timeSinceSpawnEnemy = 0;
            timeSinceSpawnMeteor = 0;
            timesinceSpawnResource = 0;

            playTime = 0;

            // Sätter sluttiden till 60 s
            SetCustomVictoryCondition(LevelObjective.Time, surviveTime);

            backgroundManager.Initialize(BackgroundType.deadSpace);

            tempBool = false;
        }

        private List<float> SetResourceSpawnDelays()
        {
            int numberOfStones = copperStones.Count + goldStones.Count + titaniumStones.Count;

            resourceTimings.Clear();
            for (int n = 0; n < numberOfStones; n++)
            {
                resourceTimings.Add(random.Next(surviveTime * 1000));
            }

            resourceTimings.Sort();

            //float delay = (float)surviveTime*1000 / (float)numberOfStones;
            return resourceTimings;
        }

        bool tempBool;
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Game.Window.Title = spawnDelay.ToString();

            timeSinceSpawnMeteor += gameTime.ElapsedGameTime.Milliseconds;
            timeSinceSpawnEnemy += gameTime.ElapsedGameTime.Milliseconds;
            timesinceSpawnResource += gameTime.ElapsedGameTime.Milliseconds;

            // Enkelt att avgöra om banan är klar
            if (playTime < victoryTime - 3000 && !player.IsKilled) 
                SpawnControlUpdate(gameTime);           

            if (IsObjectiveCompleted && ((ControlManager.CheckPress(RebindableKeys.Action1) || ControlManager.CheckKeypress(Keys.Enter))))
            {
                EndLevel();
            }

            if(IsObjectiveCompleted && tempBool == false)
            {
                EndText = "Resources gathered: \n";

                if (player.AmassedCopper > 0)
                    EndText += "Copper: " + player.AmassedCopper + "\n";

                if (player.AmassedGold > 0)
                    EndText += "Gold: " + player.AmassedGold + "\n";

                if (player.AmassedTitanium > 0)
                    EndText += "Titanium: " + player.AmassedTitanium + "\n";

                else if (player.AmassedCopper <= 0 && player.AmassedGold <= 0 && player.AmassedTitanium <= 0)
                    EndText += "None\n";

                EndText += "\nPress " + RebindableKeys.Action1 + " to continue...";
                tempBool = true;
            }
        }

        private void EndLevel()
        {          
            previousPlanet.ResourceCount.Clear();
            previousPlanet.ResourceTypes.Clear();

            for (int i = 0; i < resources.Count; i++)
            {
                resources[i] -= ReturnPlayerAmount(i);
            }

            for (int i = 0; i < resources.Count; i++ )
            {
                if (resources[i] > 0)
                {
                    previousPlanet.ResourceCount.Add(resources[i]);
                    previousPlanet.ResourceTypes.Add(resourceTypes[i]);
                }
            }

            enemyTypes.Clear();
            numberOfEnemies.Clear();

            if (player.AmassedCopper > 0)
            {
                ShipInventoryManager.AddQuantityItem(this.Game, (int)Math.Round(player.AmassedCopper, 0), "Resource", "copper");
                player.AmassedCopper = 0;
            }

            if (player.AmassedGold > 0)
            {
                ShipInventoryManager.AddQuantityItem(this.Game, (int)Math.Round(player.AmassedGold, 0), "Resource", "gold");
                player.AmassedGold = 0;
            }

            if (player.AmassedTitanium > 0)
            {
                ShipInventoryManager.AddQuantityItem(this.Game, (int)Math.Round(player.AmassedTitanium, 0), "Resource", "titanium");
                player.AmassedTitanium = 0;
            }

            Game.stateManager.planetState.LoadPlanetData(previousPlanet);
            Game.stateManager.ChangeState("PlanetState");
            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            string tempString = "";

            tempString = resourceTypes[0] + ": " + (resources[0] - ReturnPlayerAmount(0));

            if (resourceTypes.Count > 1)
                tempString += "\n" + resourceTypes[1] + ": " + (resources[1] - ReturnPlayerAmount(1));

            if (resourceTypes.Count > 2)
                tempString += "\n" + resourceTypes[2] + ": " + (resources[2] - ReturnPlayerAmount(2));

            spriteBatch.DrawString(font1, "Resources left: " + "\n" +  tempString,
                new Vector2(10, 10) + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

            base.Draw(spriteBatch);
        }

        private float ReturnPlayerAmount(int index)
        {
            if (resourceTypes[index] == "Copper") return player.AmassedCopper;
            if (resourceTypes[index] == "Gold") return player.AmassedGold;
            if (resourceTypes[index] == "Titanium") return player.AmassedTitanium;

            else return 0.0f;
        }

        public override void SpawnControlUpdate(GameTime gameTime)
        {
            List<GameObjectVertical> listRef = Game.stateManager.shooterState.gameObjects;

            #region meteorite
                if (timeSinceSpawnMeteor > spawnDelayMeteor)
                {
                    int meteoriteVal = random.Next(32);

                    VerticalShooterShip meteorite;

                    switch (meteoriteVal)
                    {
                        case 0:
                        case 1:
                        case 2:
                        case 3:
                        case 4:
                        case 5:
                            meteorite = new Meteorite15(Game, spriteSheet, player);
                            break;

                        case 6:
                        case 7:
                        case 8:
                        case 9:
                        case 10:
                        case 11:
                            meteorite = new Meteorite20(Game, spriteSheet, player);
                            break;

                        case 12:
                        case 13:
                        case 14:
                        case 15:
                        case 16:
                            meteorite = new Meteorite25(Game, spriteSheet, player);
                            break;

                        case 17:
                        case 18:
                        case 19:
                        case 20:
                        case 21:
                        case 22:
                            meteorite = new Meteorite30(Game, spriteSheet, player);
                            break;

                        default:
                            meteorite = new Meteorite20(Game, spriteSheet, player);
                            break;
                    }

                    meteorite.Initialize();
                    meteorite.Position = new Vector2((float)(random.NextDouble() * WindowWidth), 0);
                    meteorite.Direction = new Vector2(0, 1.0f);
                    Game.stateManager.shooterState.gameObjects.Add(meteorite);

                    timeSinceSpawnMeteor -= spawnDelayMeteor;
                    spawnDelayMeteor -= 0.1f;
                }

                #endregion

            #region enemy

                int enemyVal = random.Next(15);
                if (timeSinceSpawnEnemy > spawnDelayEnemy)
                {
                    if (enemyTypes.Count > 0)
                    {
                        switch (enemyVal)
                        {
                            case 0:
                            case 1:
                            case 2:
                            case 3:
                            case 4:
                            case 5:
                                if (numberOfEnemies[0] > 0)
                                {
                                    ReturnEnemy(enemyTypes[0]);
                                    numberOfEnemies[0]--;
                                }
                                else goto default;
                                break;

                            case 6:
                            case 7:
                            case 8:
                                if (enemyTypes.Count > 1 && numberOfEnemies[1] > 0)
                                {
                                    ReturnEnemy(enemyTypes[1]);
                                    numberOfEnemies[1]--;
                                }
                                else goto default;
                                break;

                            case 9:
                            case 10:
                            case 11:
                                if (enemyTypes.Count > 2 && numberOfEnemies[2] > 0)
                                {
                                    ReturnEnemy(enemyTypes[2]);
                                    numberOfEnemies[2]--;
                                }
                                else goto default;
                                break;

                            case 12:
                            case 13:
                                if (enemyTypes.Count > 3 && numberOfEnemies[3] > 0)
                                {
                                    ReturnEnemy(enemyTypes[3]);
                                    numberOfEnemies[3]--;
                                }
                                else goto default;
                                break;

                            case 14:
                                for (int i = 0; i < enemyTypes.Count; i++)
                                {
                                    if (enemyTypes[i].ToLower().Equals("red") ||
                                        enemyTypes[i].ToLower().Equals("redenemy") &&
                                        numberOfEnemies[i] > 3)
                                    {
                                        //throw new ArgumentException("This is an ancient system and needs to be updated, contact Jakob if this is encountered");
                                        //spawnController.CreateVFormation(Game.Window.ClientBounds.Width / 2, 3);
                                        //numberOfEnemies[i] -= 3;
                                        break;
                                    }
                                }
                                break;

                            default:
                                if (numberOfEnemies[0] > 0)
                                {
                                    ReturnEnemy(enemyTypes[0]);
                                    numberOfEnemies[0]--;
                                }
                                break;
                        }
                        timeSinceSpawnEnemy -= spawnDelayEnemy;
                        spawnDelayEnemy -= 1.5f;
                    }
                }
                #endregion

            #region resource

            int stoneCount = copperStones.Count + goldStones.Count + titaniumStones.Count;

            if (timesinceSpawnResource > currentTiming && stoneCount > 0)
            {
                int val = random.Next(stoneCount) + 1;

                VerticalShooterShip resource;

                if (val <= copperStones.Count)
                {
                    resource = new ResourceMeteoriteCopper(Game, spriteSheet, player);
                    resource.Initialize();
                    resource.HP = copperStones[copperStones.Count - 1] * 10;
                    copperStones.RemoveAt(copperStones.Count - 1);
                }
                else if (val <= copperStones.Count + goldStones.Count)
                {
                    resource = new ResourceMeteoriteGold(Game, spriteSheet, player);
                    resource.Initialize();
                    resource.HP = goldStones[goldStones.Count - 1] * 10;
                    goldStones.RemoveAt(goldStones.Count - 1);
                }
                else
                {
                    resource = new ResourceMeteoriteTitanium(Game, spriteSheet, player);
                    resource.Initialize();
                    resource.HP = titaniumStones[titaniumStones.Count - 1] * 10;
                    titaniumStones.RemoveAt(titaniumStones.Count - 1);
                }

                resource.Position = new Vector2((float)(random.NextDouble() * WindowWidth), 0);
                resource.Direction = new Vector2(0, 1.0f);
                Game.stateManager.shooterState.gameObjects.Add(resource);

                if (currentTimingIndex < resourceTimings.Count - 1)
                    currentTimingIndex += 1;
                
                currentTiming = resourceTimings[currentTimingIndex];

                //timesinceSpawnResource -= spawnDelayResource;
                //spawnDelayResource -= 0.1f;
            }
            #endregion

        }
        
        private void ReturnEnemy(string name)
        {
            switch (name.ToLower())
            { 
                case "red":
                case "redenemy":
                    //throw new ArgumentException("Based on removed system, needs to be fixed.");
                    //spawnController.CreateRedEnemy(new Vector2(random.Next(Game.Window.ClientBounds.Width), 0));
                    break;

                case "blue":
                case "blueenemy":
                    //throw new ArgumentException("Based on removed system, needs to be fixed.");
                    //spawnController.CreateBlueEnemy(new Vector2(random.Next(Game.Window.ClientBounds.Width), 0));
                    break;

                case "green":
                case "greenenemy":
                    //throw new ArgumentException("Based on removed system, needs to be fixed.");
                    //spawnController.CreateGreenEnemy(new Vector2(random.Next(Game.Window.ClientBounds.Width), 0));
                    break;

                case "yellow":
                case "yellowenemy":
                    //throw new ArgumentException("Based on removed system, needs to be fixed.");
                    //spawnController.CreateYellowEnemy(new Vector2(random.Next(Game.Window.ClientBounds.Width), 0));
                    break;

                default:
                    break;
            }
        }
        public override void ResetLevel()
        {
            resourceTypes.Clear();
            resources.Clear();

            enemyTypes.Clear();
            numberOfEnemies.Clear();

            for (int i = 0; i < previousPlanet.ResourceTypes.Count; i++)
            {
                if (previousPlanet.ResourceCount[i] > 0)
                {
                    resourceTypes.Add(previousPlanet.ResourceTypes[i]);
                    resources.Add(previousPlanet.ResourceCount[i]);
                }
            }

            if (startingEnemy != "")
            {
                enemyTypes.Add(startingEnemy);
                numberOfEnemies.Add(startingEnemyCount);
            }

            else if (startingEnemies.Count > 0)
            { 
                for (int i = 0; i < startingEnemies.Count; i++)
                {
                    enemyTypes.Add(startingEnemies[i]);
                    numberOfEnemies.Add(startingEnemiesCount[i]);
                }
            }

            copperStones.Clear();
            goldStones.Clear();
            titaniumStones.Clear();

                
        }
        public override void ReturnToPreviousScreen()
        {
            Game.stateManager.planetState.LoadPlanetData(previousPlanet);
            Game.stateManager.ChangeState("PlanetState");
        }
    }
}
