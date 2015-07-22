using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject_Mac
{
    public class Beacon : GameObjectOverworld
    {
        private const int RADIUS = 22500;

        private bool activated;
        public bool IsActivated { get { return activated; } private set { ;} }

        private List<Beacon> knownBeacons;

        private List<Vector2> jumpPath;
        public List<Vector2> GetJumpPath { get { return jumpPath; } private set { ;} }
        private Beacon finalDestination;
        public Beacon GetFinalDestination { get { return finalDestination; } private set { ;} }

        private Rectangle activeSourceRect;
        private Rectangle passiveSourceRect;

        public Beacon(Game1 Game, Sprite spriteSheet, Rectangle passiveSourceRect, Rectangle activeSourceRect, String name, Vector2 position) :
            base(Game, spriteSheet)
        {
            this.name = name;
            this.passiveSourceRect = passiveSourceRect;
            this.activeSourceRect = activeSourceRect;
            this.position = position;

            sprite = spriteSheet.GetSubSprite(passiveSourceRect);
            activated = false;
        }

        public override void Initialize()
        {
            Class = "Beacon";
            layerDepth = 0.3f;
            speed = 0;
            scale = 1.0f;
            color = Color.White;

            knownBeacons = new List<Beacon>();
            jumpPath = new List<Vector2>();

            base.Initialize();
        }

        public void AddKnownBeacons(List<GameObjectOverworld> overworldobjects)
        {
            String name = this.name;

            for (int i = 0; i < overworldobjects.Count; i++)
            {
                if (overworldobjects[i] != this &&
                    overworldobjects[i] is Beacon)
                {
                    knownBeacons.Add((Beacon)overworldobjects[i]);
                }
            }
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
            if (IsUsed == true)
            {
                base.Draw(spriteBatch);
            }
        }

        public void Activate()
        {
            activated = true;
            sprite = spriteSheet.GetSubSprite(activeSourceRect);
            Game.statsManager.AddDiscoveredBeacon(this);
            Game.soundEffectsManager.PlaySoundEffect(SoundEffects.BeaconActivate);
        }

        public void PlayerGetsClose()
        {
            Activate();
            Game.helper.DisplayText(name + " activated!", 2);
        }

        public void Interact()
        {
            Game.GetBeaconMenu.Display(this);
        }

        public void StartJump(Beacon finalDestination)
        {
            if (finalDestination != this)
            {
                this.finalDestination = finalDestination;

                Game.player.InitializeHyperSpeedJump(finalDestination.position, false);
            }
        }

        public void OnLoad()
        {
            if (Game.statsManager.DiscoveredBeacons.Contains(this))
            {
                activated = true;
                sprite = spriteSheet.GetSubSprite(activeSourceRect);
                Game.statsManager.AddDiscoveredBeacon(this);
            }
        }
    }
}