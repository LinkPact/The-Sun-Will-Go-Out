using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SpaceProject
{
    public class StartingLevel: Level
    {
        public StartingLevel(Game1 game, Sprite spriteSheet, PlayerVerticalShooter player, MissionType missionType) :
            base(game, spriteSheet, player, missionType)
        {
            Name = "StartingLevel";
        }

        public override void Initialize()
        {
            base.Initialize();

            LevelWidth = 400;

            //SetVictoryToTime(0);
            SetCustomVictoryCondition(LevelObjective.Finish, 0);

            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 500, 2, 500, Game.Window.ClientBounds.Width / 5));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 500, 2, 500, Game.Window.ClientBounds.Width * (4 / 5)));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "green", 500, 2, 2500, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 3500, 2, 50, Game.Window.ClientBounds.Width / 3));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 3500, 2, 50, Game.Window.ClientBounds.Width * (2 / 3)));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "green", 5000, 2, 50, 50, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "turret", 8500, Game.Window.ClientBounds.Width / 3));
            //untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, this, "turret", 8500, Game.Window.ClientBounds.Width * (2 / 3)));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 10000, 2, 50, Game.Window.ClientBounds.Width / 4));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 10000, 2, 50, Game.Window.ClientBounds.Width * (3 / 4)));
            ////untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "homingshot", 10500, Game.ScreenCenter.X));
            //
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 14000, 2, 50, 50, Game.ScreenCenter.X));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "yellow", 15000, 2, 50, 50, Game.Window.ClientBounds.Width / 3));
            //untriggeredEvents.Add(new VFormation(Game, player, spriteSheet, this, "red", 16000, 2, 50, 50, Game.Window.ClientBounds.Width * (2 / 3)));
            //
            ////untriggeredEvents.Add(new SingleEnemy(Game, player, spriteSheet, "homingmissle", 18000, Game.ScreenCenter.X));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 20000, 2, 50, Game.Window.ClientBounds.Width * (1 / 4)));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 20000, 2, 50, Game.Window.ClientBounds.Width * (3 / 4)));
            //
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "red", 21000, 5, 50, Game.ScreenCenter.X));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "yellow", 22000, 2, 50, Game.ScreenCenter.X));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "yellow", 23000, 2, 50, Game.Window.ClientBounds.Width / 3));
            //untriggeredEvents.Add(new LineFormation(Game, player, spriteSheet, this, "yellow", 24000, 2, 50, Game.Window.ClientBounds.Width * (2 / 3)));
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (!IsObjectiveCompleted)
            {
                SpawnControlUpdate(gameTime);
            }

            else
                EndText = "Press 'Enter' to return..";
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
