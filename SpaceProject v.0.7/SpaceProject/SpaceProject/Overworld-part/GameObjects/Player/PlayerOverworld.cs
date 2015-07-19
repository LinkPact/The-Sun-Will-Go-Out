using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject
{
    public class PlayerOverworld : GameObjectOverworld
    {
        private readonly int HyperSpeedDistanceTreshold = 7500;

        private readonly float InvinsibilityOpacity = 0.5f;
        private readonly int InvincibilitySpan = 2500;

        private int damageTimer = 0;
        private bool controlsEnabled;

        private float turningSpeed;
        private bool usingBoost;
        private float playerAcc;
        private readonly float commonAcc = 0.012f;
        private readonly float developAcc = 0.1f;
        
        private const float normalFuelCost = 0.025f;
        private const float boostFuelCost = 0.085f;

        private readonly float commonSpeed = 0.65f;
        private readonly float boostSpeed = 1.2f;
        private readonly float developSpeed = 20f;

        private readonly float commonTurningSpeed = 3.5f;
        private readonly float boostTurningSpeed = 3.0f;

        private bool isHyperSpeedUnlocked = false;
        private bool isDevelopSpeedUnlocked = false;

        public bool UsingBoost { get { return usingBoost; } }

        public bool IsHyperSpeedUnlocked { get { return isHyperSpeedUnlocked; } }
        public bool IsDevelopSpeedUnlocked { get { return isDevelopSpeedUnlocked; } }
        public bool IsInvincible { get { return isInvincible; } }

        private bool hyperspeedOn;
        private Vector2 hyperspeedCoordinates;
        private double totalHyperspeedDistance;
        private double currentHyperspeedDistance;

        private const float HYPERSPEED_ACC = 0.0016f;
        private const float HYPERSPEED_MAX_SPEED = 5f;
        private const double DEACCELERATION_DISTANCE = HYPERSPEED_MAX_SPEED * HYPERSPEED_MAX_SPEED /(2 * HYPERSPEED_ACC);

        public bool HyperspeedOn
        {
            get { return hyperspeedOn; }
            set { hyperspeedOn = value; }
        }
        public Vector2 HyperspeedCoordinates
        {
            get { return hyperspeedCoordinates; }
            set { hyperspeedCoordinates = value; }
        }
        public double CurrentHyperspeedDistance
        {
            get { return currentHyperspeedDistance; }
        }
        public double DeccelerationDistance
        {
            get { return DEACCELERATION_DISTANCE; }
        }
        public double TotalHyperspeedDistance
        {
            get { return totalHyperspeedDistance; }
        }
        public bool IsControlsEnabled
        {
            get { return controlsEnabled; }
        }

        private bool isInvincible;
        private float invincibilityTime;
        
        private List<Particle> particles = new List<Particle>();
        private List<Particle> deadParticles = new List<Particle>();

        public PlayerOverworld(Game1 Game, Sprite spriteSheet):
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            Class = "Player";
            name = "player";

            controlsEnabled = true;

            layerDepth = 0.6f;
            scale = 1.0f;
            sprite = spriteSheet.GetSubSprite(new Rectangle(78, 0, 29, 27));
            color = Color.White;
            position = new Vector2(118400, 98700);
            Direction.SetDirection(new Vector2(1, 0));
            centerPoint = new Vector2(sprite.SourceRectangle.Value.Width / 2,
            sprite.SourceRectangle.Value.Height / 2);

            hyperspeedOn = false;
            isInvincible = false;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            damageTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (StatsManager.GetShipLife() <= 0
                && GameStateManager.currentState == "OverworldState")
            {
                Game.stateManager.outroState.SetOutroType(OutroType.GameOver);
                Game.stateManager.ChangeState("OutroState");
            }

            foreach (Particle par in particles)
            {
                par.Update(gameTime, this);
            }

            if (IsUsed && !hyperspeedOn)
            {
                PlayerMovement(gameTime);
                base.Update(gameTime);
            }

            if (!IsControlsEnabled
                && !Game.stateManager.overworldState.IsBurnOutEndingActivated)
            {
                AddParticle();
            }

            if (isInvincible)
            {
                UpdateInvincibility(gameTime);
            }

            #region UpdateHyperSpeed

            if (hyperspeedOn == true)
            {
                HyperSpeedMovement(gameTime);
                AddHyperSpeedParticle();
                base.Update(gameTime);
                angle = (float)(MathFunctions.RadiansFromDir(
                    new Vector2(Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI * 90) / 180);

                if (currentHyperspeedDistance <= 20 || speed <= 0)
                {
                    hyperspeedOn = false;
                    CheckForMoreJumps();
                }
            }

            #endregion

            #region UpdateParticles

            for (int i = 0; i < particles.Count; i++)
            {
                if (particles[i].lifeSpawn <= 0)
                {
                    deadParticles.Add(particles[i]);
                }
            }

            RemoveParticle();

            #endregion
        }

        private void PlayerMovement(GameTime gameTime)
        {
            if (speed > maxSpeed)
            {
                speed = maxSpeed;
            }

            else if (speed < -maxSpeed)
            {
                speed = -maxSpeed;
            }

            if (StatsManager.Fuel >= normalFuelCost)
            {
                if (ControlManager.CheckHold(RebindableKeys.Action3) 
                    && isHyperSpeedUnlocked 
                    && !isDevelopSpeedUnlocked 
                    && controlsEnabled)
                {
                    maxSpeed = boostSpeed;
                    turningSpeed = boostTurningSpeed;
                    playerAcc = commonAcc;
                    usingBoost = true;
                }

                else if (ControlManager.CheckHold(RebindableKeys.Action3) 
                    && isDevelopSpeedUnlocked 
                    && controlsEnabled)
                {
                    maxSpeed = developSpeed;
                    turningSpeed = boostTurningSpeed;
                    playerAcc = developAcc;
                    usingBoost = true;
                }

                else
                {
                    maxSpeed = commonSpeed;
                    turningSpeed = commonTurningSpeed;
                    playerAcc = commonAcc;
                    usingBoost = false;
                }
            }

            if (ControlManager.CheckHold(RebindableKeys.Up) && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    if (ControlManager.GamepadReady && ControlManager.ThumbStickAngleY != 0)
                    {
                        if (StatsManager.Fuel > normalFuelCost)
                        {
                            speed += playerAcc * MathFunctions.FPSSyncFactor(gameTime);
                        }

                        AddParticle();
                    }
                    else
                    {
                        if (StatsManager.Fuel > normalFuelCost)
                        {
                            speed += playerAcc * MathFunctions.FPSSyncFactor(gameTime);
                        }

                        AddParticle();

                        Game.soundEffectsManager.PlaySoundEffect(SoundEffects.OverworldEngine, 0f, 0f);
                    }
                }
            }

            if (ControlManager.CheckHold(RebindableKeys.Right) && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    Direction.SetDirection(Direction.GetDirectionAsDegree() + turningSpeed * MathFunctions.FPSSyncFactor(gameTime));
                }
            }

            else if (ControlManager.CheckHold(RebindableKeys.Left)
                && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    Direction.SetDirection(Direction.GetDirectionAsDegree() - turningSpeed * MathFunctions.FPSSyncFactor(gameTime));
                }
            }

            if (!ControlManager.CheckHold(RebindableKeys.Up) && controlsEnabled)
            {

                if (speed > 0)
                {
                    speed -= playerAcc * MathFunctions.FPSSyncFactor(gameTime);
                }

                else if (speed <= 0) 
                {
                    speed = 0;
                }

                Game.soundEffectsManager.FadeOutSoundEffect(SoundEffects.OverworldEngine);
            }

            angle = (float)(MathFunctions.RadiansFromDir(new Vector2(
                Direction.GetDirectionAsVector().X, Direction.GetDirectionAsVector().Y)) + (Math.PI * 90) / 180);
        }

        public void InitializeHyperSpeedJump(Vector2 coordinates, bool useDistanceCheck)
        {
            speed = 0;
            hyperspeedCoordinates = coordinates;
            totalHyperspeedDistance = Math.Abs(Vector2.Distance(coordinates, position));

            if (useDistanceCheck && totalHyperspeedDistance < HyperSpeedDistanceTreshold)
            {
                Game.helper.DisplayText("Could not initialize Hyper Speed. Distance is too short.", 2);
                return;
            }

            hyperspeedOn = true;
            maxSpeed = HYPERSPEED_MAX_SPEED;
        }

        public void BounceBack()
        {
            var pos = new Vector2(position.X + (100 * -Direction.GetDirectionAsVector().X),
                position.Y + (100 * -Direction.GetDirectionAsVector().Y));

            Direction.SetDirection(new Vector2(pos.X - position.X, pos.Y - position.Y));
            InitializeHyperSpeedJump(pos, false);
        }

        private void HyperSpeedMovement(GameTime gameTime)
        {
            if (speed > HYPERSPEED_MAX_SPEED)
                speed = HYPERSPEED_MAX_SPEED;

            Direction.RotateTowardsPoint(gameTime, position, hyperspeedCoordinates, turningSpeed);
            currentHyperspeedDistance = Math.Abs(Vector2.Distance(hyperspeedCoordinates, position));

            // Total distance is enough to accelerate ship to max speed
            if (totalHyperspeedDistance > DEACCELERATION_DISTANCE * 2)
            {
                if (currentHyperspeedDistance > DEACCELERATION_DISTANCE)
                {
                    if (speed < HYPERSPEED_MAX_SPEED)
                    {
                        speed += HYPERSPEED_ACC * gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
                else if (currentHyperspeedDistance < DEACCELERATION_DISTANCE)
                {
                    if (speed > 0)
                    {
                        speed -= HYPERSPEED_ACC * gameTime.ElapsedGameTime.Milliseconds;
                    }
                }
            }

            // Total distance is NOT enough to accelerate ship to max speed
            else
            {
                if (currentHyperspeedDistance > totalHyperspeedDistance / 2)
                {
                    if (speed < HYPERSPEED_MAX_SPEED)
                        speed += HYPERSPEED_ACC * gameTime.ElapsedGameTime.Milliseconds;
                }
                else
                {
                    if (speed > 0)
                        speed -= HYPERSPEED_ACC * gameTime.ElapsedGameTime.Milliseconds;
                }
            }
        }

        private void AddParticle()
        {
            Particle par = new Particle(Game, spriteSheet);
            par.Initialize(this, position);
            if (isInvincible)
            {
                par.SetOpacity(InvinsibilityOpacity * 1.5f);
            }
            particles.Add(par);
        }

        private void AddHyperSpeedParticle()
        {
            HyperSpeedParticle par = new HyperSpeedParticle(Game, spriteSheet);
            par.Initialize(this, position);
            particles.Add(par);
        }

        private void RemoveParticle()
        {
            for (int i = 0; i < deadParticles.Count; i++)
            {
                particles.Remove(deadParticles[i]);
            }

            deadParticles.Clear();
        }

        public void InitializeInvincibility()
        {
            invincibilityTime = StatsManager.PlayTime.GetFutureOverworldTime(InvincibilitySpan);
            color = Color.White * InvinsibilityOpacity;
            isInvincible = true;
        }

        public void UpdateInvincibility(GameTime gameTime)
        {
            if (StatsManager.PlayTime.HasOverworldTimePassed(invincibilityTime))
            {
                color = Color.White;
                isInvincible = false;
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            foreach (Particle par in particles)
                par.Draw(spriteBatch);
        }

        public void Save()
        {
            SortedDictionary<String, String> saveData = new SortedDictionary<string, string>();
            int x = (int)position.X;
            int y = (int)position.Y;
            saveData.Add("positionx", x.ToString());
            saveData.Add("positiony", y.ToString());
            Game.saveFile.Save(Game1.SaveFilePath, "save.ini", "playeroverworld", saveData);
        }

        public void Load()
        {
            int x = Game.saveFile.GetPropertyAsInt("playeroverworld", "positionx", 0);
            int y = Game.saveFile.GetPropertyAsInt("playeroverworld", "positiony", 0);
            position = new Vector2(x, y);

            if (StatsManager.gameMode.Equals(GameMode.Develop))
                Game.player.UnlockDevelopHyperSpeed();
        }

        public void UnlockHyperSpeed()
        {
            isHyperSpeedUnlocked = true;
        }

        public void UnlockDevelopHyperSpeed()
        {
            isDevelopSpeedUnlocked = true;
        }

        List<Beacon> beacons;
        private void CheckForMoreJumps()
        {
            if (beacons == null)
            {
                beacons = new List<Beacon>();

                foreach (GameObjectOverworld obj in Game.stateManager.overworldState.GetSectorX.GetGameObjects())
                {
                    if (obj is Beacon)
                    {
                        beacons.Add((Beacon)obj);
                    }
                }
            }

            foreach (Beacon beacon in beacons)
            {
                if (beacon.GetJumpPath.Count > 0)
                {
                    beacon.StartJump(beacon.GetFinalDestination);
                }
            }
        }

        public void DisableControls()
        {
            controlsEnabled = false;
        }

        public void EnableControls()
        {
            controlsEnabled = true;
        }
    }
}
