using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

public enum Trigger
{
    Untriggered,
    Running,
    Completed
}

namespace SpaceProject
{
    public abstract class LevelEvent
    {
        private enum CreationFlag
        {
            WITHOUT_POSITION,
            X_POSITION,
            VECTOR_POSITION
        }

        // Used to give a margin between edges and enemies creation position
        protected const float xPadding = 25; 

        #region declaration
   
        private Trigger triggerStatus;
        protected Trigger TriggerStatus { get { return triggerStatus; } set { triggerStatus = value; } }

        protected Movement movement = Movement.None;
        protected float sightRange = -1.0f;

        protected SetupCreature setupCreature;

        protected float xPos;

        //Panic-solution. I don't know if it's possible to access enum-values from outside the class.
        //TODO This should possibly be removed 
        public String RetrieveTriggerStatus()
        {
            if (triggerStatus == Trigger.Untriggered) return "Untriggered";

            if (triggerStatus == Trigger.Running) return "Running";

            if (triggerStatus == Trigger.Completed) return "Completed";

            return "";
        }

        protected Game1 Game;
        protected PlayerVerticalShooter player;
        protected Sprite spriteSheet;
        protected Level level;

        protected float startTime;
        public float StartTime { get { return startTime; } }

        protected bool timeTriggered;
        public bool TimeTriggered { get { return timeTriggered; } }

        protected bool hasDuration;
        public bool HasDuration { get { return hasDuration; } }
        
        protected Random random = new Random();

        protected EnemyType enemyType;
        public EnemyType EnemyType { get { return enemyType; } }

        protected List<VerticalShooterShip> creatureList;

        #endregion

        #region constructors
        //This constructor is called if the event is timetriggered.
        protected LevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType enemyType, float startTime)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.player = player;
            this.level = level;

            //this.identifier = identifier;

            this.startTime = startTime;

            //PickCreatures(identifier);
            this.enemyType = enemyType;

            triggerStatus = Trigger.Untriggered;
        }
        
        //This constructor is called if the event is timetriggered.
        //Takes a list of creatures as input instead of identifier, when wanting to use non-standard characteristics.
        protected LevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, List<VerticalShooterShip> creatureList, float startTime)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.player = player;
            this.level = level;

            this.creatureList = creatureList;

            this.startTime = startTime;

            //PickCreatures(identifier);

            triggerStatus = Trigger.Untriggered;
        }
        
        //This constructor is called if the event is manually triggered, for example by a LastingLevelEvent.
        protected LevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, EnemyType enemyType)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.player = player;
            this.level = level;

            this.enemyType = enemyType;

            //PickCreatures(identifier);

            triggerStatus = Trigger.Untriggered;
        }
        
        //At least initially meant for "WavesLevelEvent".
        protected LevelEvent(Game1 Game, PlayerVerticalShooter player, Sprite spriteSheet, Level level, float startTime)
        {
            this.Game = Game;
            this.spriteSheet = spriteSheet;
            this.player = player;
            this.level = level;
            this.startTime = startTime;

            //this.identifier = identifier;
            triggerStatus = Trigger.Untriggered;
        }
        #endregion

        public Boolean AttemptTrigger(float levelTime)
        {
            if (levelTime > startTime && triggerStatus == Trigger.Untriggered)
            {
                triggerStatus = Trigger.Running;
                return true;
            }
            return false;
        }

        public abstract void Run(GameTime gameTime);

        protected void EndEvent()
        {
            triggerStatus = Trigger.Completed;
        }
        
        public void CreatureSetup(SetupCreature setupCreature)
        {
            this.setupCreature = setupCreature;
        }
        
        protected VerticalShooterShip RetrieveCreatureFromEnum(EnemyType identifier)
        {
            VerticalShooterShip creature = null;

            switch (identifier)
            {
                // Allies
                case EnemyType.fighterAlly:
                    {
                        creature = new FighterAlly(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.freighterAlly:
                    {
                        creature = new FreighterAlly(Game, spriteSheet, player);
                        break;
                    }

                // Neutral
                #region neutral
                case EnemyType.big_R:
                    {
                        creature = new AllianceBigFighterMkI(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.bigMissile_R:
                    {
                        creature = new AllianceBigMissileEnemy(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.turret:
                    {
                        creature = new Turret(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.meteor:
                    {
                        int rNbr = random.Next(4);

                        switch (rNbr)
                        {
                            case 0:
                                {
                                    creature = new Meteorite15(Game, spriteSheet, player);
                                    break;
                                }
                            case 1:
                                {
                                    creature = new Meteorite20(Game, spriteSheet, player);
                                    break;
                                }
                            case 2:
                                {
                                    creature = new Meteorite25(Game, spriteSheet, player);
                                    break;
                                }
                            case 3:
                                {
                                    creature = new Meteorite30(Game, spriteSheet, player);
                                    break;
                                }
                            default:
                                {
                                    throw new ArgumentException("Check the randomized range");
                                }
                        }
                        break;
                    }
                case EnemyType.homingBullet_R:
                    {
                        creature = new AllianceHomingShotEnemy(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.homingMissile_R:
                    {
                        creature = new AllianceHomingMissileEnemy(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.medium:
                    {
                        creature = new RebelMediumSizedEnemy(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.smallShooter_R:
                    {
                        creature = new AllianceSmallShooter(Game, spriteSheet, player);
                        break;
                    }
                #endregion

                // Rebel
                #region rebel
                case EnemyType.R_mosquito:
                    {
                        creature = new RebelMosquito(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_thickShooter:
                    {
                        creature = new RebelThickShooter(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_smallSniper:
                    {
                        creature = new RebelSmallSniper(Game, spriteSheet, player);
                        break;
                    }

                case EnemyType.R_lightMinelayer:
                    {
                        creature = new RebelLightMinelayer(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_homingMissile:
                    {
                        creature = new RebelHomingMissile(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_bomber:
                    {
                        creature = new RebelBomber(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_burster:
                    {
                        creature = new RebelBurster(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_minelayer:
                    {
                        creature = new RebelMinelayer(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_smallAttack:
                    {
                        creature = new RebelSmallAttackShip(Game, spriteSheet, player);
                        break;
                    }

                case EnemyType.R_missileAttackShip:
                    {
                        creature = new RebelMissileAttack(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.R_fatzo:
                    {
                        creature = new RebelFatzo(Game, spriteSheet, player);
                        break;
                    }
                #endregion

                // Alliance
                #region alliance
                case EnemyType.A_drone:
                    {
                        creature = new AllianceDrone(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_smallLaserShip:
                    {
                        creature = new AllianceSmallLaserShip(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_singleHoming:
                    {
                        creature = new AllianceSingleHoming(Game, spriteSheet, player);
                        break;
                    }

                case EnemyType.A_shielder:
                    {
                        creature = new AllianceShielder(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_attackStealth:
                    {
                        creature = new AllianceStealthAttackShip(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_lightBeamer:
                    {
                        creature = new AllianceLightBeamer(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_multipleShot:
                    {
                        creature = new AllianceMultipleShot(Game, spriteSheet, player);
                        break;
                    }

                case EnemyType.A_stealthShip:
                    {
                        creature = new AllianceStealthShip(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_heavyBeamer:
                    {
                        creature = new AllianceHeavyBeamer(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_ballistic:
                    {
                        creature = new AllianceBallistic(Game, spriteSheet, player);
                        break;
                    }
                case EnemyType.A_hangar:
                    {
                        creature = new AllianceHangar(Game, spriteSheet, player);
                        break;
                    }
                #endregion

                default:
                    {
                        throw new ArgumentException("Argument not implemented!");
                    }
            }

            return creature;
        }

        protected void CreateCreature(float xPos = -1f)
        {
            CreationFlag flag;
            if (xPos == -1f)
                flag = CreationFlag.WITHOUT_POSITION;
            else
                flag = CreationFlag.X_POSITION;

            if (xPos == -1f) 
                xPos = level.RelativeOrigin + (float)(random.NextDouble() * level.LevelWidth);

            Vector2 xVector = new Vector2(xPos, 0);

            VerticalShooterShip ship = VerticalCreationLogic(xVector, flag);
            
            Game.stateManager.shooterState.gameObjects.Add(ship);
        }
        
        protected void CreateCreature(Vector2 position)
        {
            VerticalShooterShip creature = RetrieveCreatureFromEnum(enemyType);
            VerticalShooterShip ship = VerticalCreationLogic(position, CreationFlag.VECTOR_POSITION);
            Game.stateManager.shooterState.gameObjects.Add(ship);
        }

        protected VerticalShooterShip ReturnCreature(float xPos = -1f)
        {
            if (xPos == -1f) 
                xPos = (float)(random.NextDouble() * 800);

            return CreateReturnCreature(new Vector2(xPos, 0));
        }

        protected VerticalShooterShip ReturnCreature(Vector2 position)
        {
            if (position.X == -1f) 
                position = new Vector2((float)(random.NextDouble() * 800), 0);
            
            return CreateReturnCreature(position);
        }

        private VerticalShooterShip CreateReturnCreature(Vector2 position)
        {
            VerticalShooterShip creature = RetrieveCreatureFromEnum(enemyType);
            creature = StandardCreatureSetup(creature);
            creature.Position = position;
            return creature;
        }

        private VerticalShooterShip VerticalCreationLogic(Vector2 position, CreationFlag flag)
        {
            if (flag != CreationFlag.VECTOR_POSITION)
                position.Y -= 50; // Setting a startup marginal to not "pop" on the screen

            if (Game.Window.ClientBounds.Height > 600)
            {
                position.Y += (Game.Window.ClientBounds.Height - 600) / 2;
            }

            switch (flag)
            {
                case CreationFlag.WITHOUT_POSITION:
                    {
                        xPos = level.RelativeOrigin + (float)(random.NextDouble() * level.LevelWidth);
                        break;
                    }
                case CreationFlag.X_POSITION:
                    {
                        xPos = level.RelativeOrigin + position.X;
                        break;
                    }
                case CreationFlag.VECTOR_POSITION:
                    {
                        xPos = level.RelativeOrigin + position.X;
                        break;
                    }
                default:
                    {
                        throw new ArgumentException("New unhandled flag-variant is probably present");
                    }
            }

            VerticalShooterShip creature = RetrieveCreatureFromEnum(enemyType);

            creature.PositionX = xPos;

            if (creature is AlliedShip)
                creature.PositionY = 600;
            else
                creature.PositionY = position.Y;

            creature = StandardCreatureSetup(creature);
            creature.SetLevelWidth(level.LevelWidth);
            creature.MovementSetup();

            return creature;

        }

        private VerticalShooterShip StandardCreatureSetup(VerticalShooterShip creature)
        {
            creature.Initialize();
            creature.Direction = new Vector2(0, 1.0f);

            if (setupCreature != null)
            {
                creature.HP *= setupCreature.HPFactor;
                creature.Speed *= setupCreature.speedFactor;

                if (setupCreature.newMovement != Movement.None)
                {
                    if (setupCreature.newMovement != Movement.BossStop_X)
                        creature.SetMovement(setupCreature.newMovement);
                    else
                        creature.SetBossMovement(setupCreature.YStopPosition);
                }
            }

            //Extra logic for allys
            if (creature is AlliedShip)
            {
                ((AlliedShip)creature).SetFormationArea(new Rectangle((int)(creature.PositionX), 500, 1, 7));

                if (creature is FighterAlly)
                    ((AlliedShip)creature).CreateAI(AIBehaviour.Standard);
                else if (creature is FreighterAlly)
                    ((AlliedShip)creature).CreateAI(AIBehaviour.NoWeapon);
                else
                    throw new ArgumentException("Unknown ally type!");
            }

            // Slightly alters the draw layer to give all gameobjects individual layers
            creature.DrawLayer += (float)(MathFunctions.GetExternalRandomDouble() * 0.01);

            return creature;
        }

        public virtual List<CreaturePackage> RetrieveCreatures()
        {
            return null;
        }

        protected void SetRandomXPosition()
        {
            xPos = xPadding + (float)(random.NextDouble() * (level.LevelWidth - 2 * xPadding));
        }
    }
}
