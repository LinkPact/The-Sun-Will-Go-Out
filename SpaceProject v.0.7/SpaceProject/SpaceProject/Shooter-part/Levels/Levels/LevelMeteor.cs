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
    //Innehaller logik for att kontrollera hur aktuell niva ser ut.
    public class LevelMeteor : Level
    {
        private float spawnDelay;
        private float timeSinceSpawnMeteor;

        private int surviveTime = 30;

        public LevelMeteor(Game1 Game, Sprite spriteSheet, PlayerVerticalShooter player1, MissionType missionType)
            : base(Game, spriteSheet, player1, missionType)
        {
            this.Name = "MeteorLevel";
        }

        public override void Initialize()
        {
            base.Initialize();
            LevelWidth = 800;

            spawnDelay = 100.0f;
            timeSinceSpawnMeteor = 0;

            // Sätter sluttiden till 180 s
            SetCustomVictoryCondition(LevelObjective.Time, surviveTime);

            backgroundManager.Initialize(BackgroundType.deadSpace);

            Game.musicManager.PlayMusic(Music.Asteroids);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            //Game.Window.Title = spawnDelay.ToString();

            timeSinceSpawnMeteor += gameTime.ElapsedGameTime.Milliseconds;

            // Enkelt att avgöra om banan är klar
            if (playTime < victoryTime - 3000) SpawnControlUpdate(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            float tempTime = surviveTime - PlayTimeRounded;
            string tempString = "";

            if (tempTime >= 60)
                tempString = (tempTime / 60).ToString() + " Minutes " + (tempTime % 60) + " seconds";

            else
                tempString = tempTime.ToString() + " Seconds";

            spriteBatch.DrawString(font1, "Survive for: " + tempString,
                new Vector2(10, 10) + Game.fontManager.FontOffset, Game.fontManager.FontColor, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
            base.Draw(spriteBatch);
        }

        public override void SpawnControlUpdate(GameTime gameTime)
        {
            List<GameObjectVertical> listRef = Game.stateManager.shooterState.gameObjects;

            if (timeSinceSpawnMeteor > spawnDelay)
            {
                int val = random.Next(4);

                Creature meteorite;

                if (val == 0) meteorite = new Meteorite15(Game, spriteSheet, player);
                else if (val == 1) meteorite = new Meteorite20(Game, spriteSheet, player);
                else if (val == 2) meteorite = new Meteorite25(Game, spriteSheet, player);
                else meteorite = new Meteorite30(Game, spriteSheet, player);

                meteorite.Initialize();
                meteorite.Position = new Vector2((float)(random.NextDouble() * WindowWidth), 0);
                meteorite.Direction = new Vector2(0, 1.0f);
                Game.stateManager.shooterState.gameObjects.Add(meteorite);

                timeSinceSpawnMeteor -= spawnDelay;
                spawnDelay -= 0.018f;
            }
            
        }

        public override void ReturnToPreviousScreen()
        {
            Game.stateManager.ChangeState("OverworldState");
        }
    }
}
