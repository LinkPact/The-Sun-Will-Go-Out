using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class SpaceDuck : SubInteractiveObject
    {
        public SpaceDuck(Game1 Game, Sprite spriteSheet) :
            base(Game, spriteSheet)
        { }

        public override void Initialize()
        {
            sprite = spriteSheet.GetSubSprite(new Rectangle(80, 1400, 92, 81));
            position = MathFunctions.CoordinateToPosition(new Vector2(100, 100));
            name = "Space Duck";
            base.Initialize();

            String activationText = "The spaceduck speaks.#\"You have travelled very far to reach me.\"#\"Hereby, I will grant you the ability to travel with ludicrous speed.\"#\"You activate this by pressing the button found in my name.\"#\"Goodbye.\"";
            overworldEvent = new EnableHyperSpeedAtSpaceDuckOE(Game, activationText, this);
        }

        public void PlayerActivated()
        {
            //Game.stateManager.overworldState.AddEffectsObject(ExplosionGenerator.GenerateOverworldExplosion(Game, Game.spriteSheetVerticalShooter, this));
            //Game.stateManager.overworldState.AddEffectsObject(ExplosionGenerator.GenerateOverworldExplosion(Game, Game.spriteSheetVerticalShooter, this));
        }

        protected override void SetClearedText()
        {
            
        }
    }
}
