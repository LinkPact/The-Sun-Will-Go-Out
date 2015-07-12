using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    class EnableHyperSpeedAtSpaceDuckOE : OverworldEvent
    {
        private Game1 game;
        private String activationMessage;
        private SpaceDuck spaceDuck;

        public EnableHyperSpeedAtSpaceDuckOE(Game1 game, String activationMessage, SpaceDuck spaceDuck) :
            base()
        {
            this.game = game;
            this.activationMessage = activationMessage;
            this.spaceDuck = spaceDuck;
        }

        public override bool Activate()
        {
            game.player.UnlockDevelopHyperSpeed();
            PopupHandler.DisplayMessage(activationMessage);
            game.stateManager.overworldState.AddEffectsObject(ExplosionGenerator.GenerateSpaceDuckExplosion(game, game.spriteSheetVerticalShooter, spaceDuck));
            game.stateManager.overworldState.GetSectorX.RemoveGameObject(spaceDuck);
            return true;
        }
    }
}
