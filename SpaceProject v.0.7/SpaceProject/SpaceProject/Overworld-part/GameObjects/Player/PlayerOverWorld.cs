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
        private int damageTimer = 0;

        private bool controlsEnabled;

        #region Misc. Variables
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

        #endregion

        #region Misc. Properties

        public bool UsingBoost { get { return usingBoost; } }

        public bool IsHyperSpeedUnlocked { get { return isHyperSpeedUnlocked; } }
        public bool IsDevelopSpeedUnlocked { get { return isDevelopSpeedUnlocked; } }

        #endregion

        #region HyperspeedVariables

        private bool hyperspeedOn;
        private Vector2 hyperspeedCoordinates;
        private double totalHyperspeedDistance;
        private double currentHyperspeedDistance;

        private const float HYPERSPEED_ACC = 0.0004f;
        private const float HYPERSPEED_MAX_SPEED = 4f;
        private const double DEACCELERATION_DISTANCE = HYPERSPEED_MAX_SPEED * HYPERSPEED_MAX_SPEED /(2 * HYPERSPEED_ACC);

        #endregion

        #region HyperspeedProperties

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
        #endregion

        #region HeatWarningVariables

        public bool heatWarning;
        private bool upOrDown;
        public int heatCountdown;

        #endregion

        #region ParticleVariables
        private List<Particle> particles = new List<Particle>();
        private List<Particle> deadParticles = new List<Particle>();

        #endregion

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
            heatWarning = false;

            base.Initialize();
        }

        public override void Update(GameTime gameTime)
        {
            damageTimer -= gameTime.ElapsedGameTime.Milliseconds;

            if (StatsManager.GetShipLife() <= 0)
                Game.stateManager.ChangeState("OutroState");

            foreach (Particle par in particles)
            {
                par.Update(gameTime, this);
            }

            if (IsUsed && !hyperspeedOn)
            {
                PlayerMovement(gameTime);
                base.Update(gameTime);
            }

            #region UpdateHeatWarning

            if (heatWarning == true)
            {
                FlashRed();
            }

            #endregion

            #region UpdateHyperSpeed

            if (hyperspeedOn == true)
            {
                HyperSpeedMovement(gameTime);
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

            if (ControlManager.CheckHold(RebindableKeys.Up) 
                && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    if (ControlManager.GamepadReady && ControlManager.ThumbStickAngleY != 0)
                    {

                        if (StatsManager.Fuel > normalFuelCost)
                        {
                            speed += playerAcc;
                        }

                        AddParticle();
                    }
                    else
                    {

                        if (StatsManager.Fuel > normalFuelCost)
                        {
                            speed += playerAcc;
                        }

                        AddParticle();

                        Game.soundEffectsManager.PlaySoundEffect(SoundEffects.OverworldEngine, 0f, 0f);
                    }
                }
            }

            if (ControlManager.CheckHold(RebindableKeys.Right)
                && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    if (ControlManager.GamepadReady && ControlManager.ThumbStickAngleX != 0)
                    {
                        Direction.SetDirection(Direction.GetDirectionAsDegree() + turningSpeed);
                    }
                    else
                    {
                        Direction.SetDirection(Direction.GetDirectionAsDegree() + turningSpeed);
                    }
                }
            }

            else if (ControlManager.CheckHold(RebindableKeys.Left)
                && controlsEnabled)
            {
                if (StatsManager.Fuel > normalFuelCost)
                {
                    if (ControlManager.GamepadReady && ControlManager.ThumbStickAngleX != 0)
                    {
                        Direction.SetDirection(Direction.GetDirectionAsDegree() - turningSpeed);
                    }
                    else
                    {
                        Direction.SetDirection(Direction.GetDirectionAsDegree() - turningSpeed);
                    }
                }
            }

            if (!ControlManager.CheckHold(RebindableKeys.Up)
                && controlsEnabled)
            {

                if (speed > 0)
                {
                    speed -= playerAcc;
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

        #region HyperSpeedMethods

        public void InitializeHyperSpeedJump(Vector2 coordinates, bool useDistanceCheck)
        {
            // DON'T DELETE
            //if (!BaseInventoryManager.equippedShip.FusionCellsDepleted() ||
            //    BaseInventoryManager.equippedShip.EmergencyFusionCell > 0)
            //{
                speed = 0;
                hyperspeedCoordinates = coordinates;
                totalHyperspeedDistance = Math.Abs(Vector2.Distance(coordinates, position));

                if (useDistanceCheck && totalHyperspeedDistance < 7500)
                {
                    Game.helper.DisplayText("Could not initialize Hyper Speed. Distance is too short.", 2);
                    return;
                }

                hyperspeedOn = true;
                maxSpeed = HYPERSPEED_MAX_SPEED;

            // DON'T DELETE
                //if (!BaseInventoryManager.equippedShip.FusionCellsDepleted())
                //    BaseInventoryManager.equippedShip.RemoveFusionCell();
                //else
                //    BaseInventoryManager.equippedShip.EmergencyFusionCell--;
            //}
            //
            //else
            //{
            //    Game.helper.DisplayText("Could not initialize Hyper Speed. Not enough fusion cells.", 2);
            //}
        }

        private void HyperSpeedMovement(GameTime gameTime)
        {
            if (speed > HYPERSPEED_MAX_SPEED)
                speed = HYPERSPEED_MAX_SPEED;

            Direction.RotateTowardsPoint(position, hyperspeedCoordinates, turningSpeed);
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
        #endregion

        #region ParticleMethods

        private void AddParticle()
        {
            Particle par = new Particle(Game, spriteSheet);
            par.Initialize(this);
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

        #endregion

        #region HeatWarningMethods

        public void InitializeHeatWarning()
        {
            if (heatCountdown < 300)
            {
                heatCountdown += 3;
            }

            if (heatWarning == false)
            {
                upOrDown = true;
            }

            heatWarning = true;
        }

        public void FlashRed()
        {
            if (heatCountdown > 0)
            {
                if (upOrDown == false)
                {
                    color.G += 5;
                    color.B += 5;
                }

                if (upOrDown == true)
                {
                    color.G -= 5;
                    color.B -= 5;
                }

                if (color.G >= 255)
                    upOrDown = true;

                if (color.G <= 0)
                    upOrDown = false;

                heatCountdown -= 1;
            }

            if (heatCountdown == 0)
            {
                upOrDown = false;
                color = Color.White;
                heatWarning = false;
            }

        }

        #endregion

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (IsUsed == true)
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
            Game.saveFile.Save("save.ini", "playeroverworld", saveData);
        }

        public void Load()
        {
            int x = Game.saveFile.GetPropertyAsInt("playeroverworld", "positionx", 0);
            int y = Game.saveFile.GetPropertyAsInt("playeroverworld", "positiony", 0);
            position = new Vector2(x, y);
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

        public void OnDamage(GameObjectOverworld obj)
        {

            if (obj is SystemStar)
            {
                InitializeHeatWarning();

                if (CollisionDetection.IsPointInsideCircle(Game.player.position, obj.position, obj.sprite.SourceRectangle.Value.Width / 2)
                    && damageTimer < 0)
                {
                    damageTimer = 20;
                    StatsManager.ReduceShipLife(3);
                }

                else if (damageTimer < 0)
                {
                    damageTimer = 20;
                    StatsManager.ReduceShipLife(1);
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
