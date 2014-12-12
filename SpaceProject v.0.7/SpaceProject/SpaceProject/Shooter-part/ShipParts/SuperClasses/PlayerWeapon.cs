using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceProject
{
    public abstract class PlayerWeapon : PlayerActivated
    {
        //From bullet
        protected float damage;
        public float Damage { get { return damage; } private set { } }

        protected float duration;
        public float Duration { get { return duration; } private set { } }

        protected float speed;
        public float Speed { get { return speed; } private set { } }

        protected GameObjectVertical bullet;
        public GameObjectVertical Bullet { get { return bullet; } private set { } }

        protected PlayerWeapon(Game1 Game) 
            : base(Game)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            random = new Random();
        }

        protected PlayerWeapon(Game1 Game, ItemVariety variety) 
            : base(Game, variety)
        {
            this.spriteSheet = Game.spriteSheetVerticalShooter;
            random = new Random();
        }


        public override void Update(PlayerVerticalShooter player, GameTime gameTime)
        {
            base.Update(player, gameTime);
        }

        public override void Use(PlayerVerticalShooter player, GameTime gameTime)
        {
            base.Use(player, gameTime);
        }

        public virtual void PlaySound()
        { }

        protected override void SetShipPartVariety(double percent, double quality)
        {
            if (quality < 0) { quality = 0; }

            damage = (float)quality * Damage;
            duration = (float)quality * Duration;
            delay = (float)(1/quality) * Delay;

            Value = (int)(Value * ((float)quality) * ((float)quality));
        }

        protected override List<String> GetInfoText()
        {
            List<String> infoText = new List<String>();

            infoText.Add(Name);
            infoText.Add("Damage: " + Math.Round((double)Damage, 1).ToString() + " units");
            infoText.Add("Rate: " + Math.Round((double)1000 / Delay, 1).ToString() + " shots/second");
            infoText.Add("Range: " + Math.Round((double)Speed * Duration, 1).ToString() + " units");
            infoText.Add("Energy: " + Math.Round((double)energyCostPerSecond, 1).ToString() + " energy/second");
            infoText.Add("Value: " + Math.Round((double)Value, 0).ToString() + " Rupees");
            infoText.Add("Quality: " + Variety.ToString());

            infoText.Add("");
            infoText.Add(GetDescription());

            return infoText;
        }

        public override String RetrieveSaveData()
        {
            return "emptyitem";
        }

        protected void BasicBulletSetup(PlayerBullet bullet)
        {
            bullet.Direction = new Vector2(0, -1);
            bullet.Radians = MathFunctions.RadiansFromDir(bullet.Direction);
        
            BasicBulletSetupInternal(bullet);
        }
        
        protected void BasicBulletSetup(PlayerBullet bullet, Vector2 direction)
        {
            bullet.Direction = direction;
            bullet.Radians = MathFunctions.RadiansFromDir(bullet.Direction);
        
            BasicBulletSetupInternal(bullet);
        }
        
        private void BasicBulletSetupInternal(PlayerBullet bullet)
        {
            bullet.Speed = Speed;
            bullet.Duration = Duration;
            bullet.Damage = Damage;
        
            bullet.Initialize();
        }

        public float CalculateRange()
        {
            return bullet.Speed * bullet.Duration;
        }
    }
}
