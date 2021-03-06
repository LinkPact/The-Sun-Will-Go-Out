﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    public class CreditState : GameState
    {
        private TextBox textBoxLeft;
        private TextBox textBoxRight;
        private SpriteFont spriteFont;

        private Texture2D backdrop;  

        public CreditState(Game1 Game, string name) :
            base(Game, name)
        {
            int xOffset = 10;
            int yOffset = 10;

            spriteFont = Game.fontManager.GetFont(14);
            textBoxLeft = TextUtils.CreateTextBox(spriteFont, new Rectangle(xOffset, yOffset, Game1.ScreenSize.X / 2 - xOffset,
                                        Game1.ScreenSize.Y - 20), false, false,
                                        "Design and development:\nDaniel Alm Grundstrom\nJakob Willforss\nJohan Philipsson\n\n" +
                                        "Visuals:\nDaniel Alm Grundstrom\n\n" +
                                        "Music:\nJakob Willforss\nDaniel Alm Grundstrom\n\n" +
                                        "Story:\nJakob Willforss\n\n" +
                                        "Portraits:\nJosefin Voigt (many thanks!)\n\n" +
                                        "Beta testers:\nDaniel Willforss\nRasmus Grundstrom\nErik M\nJohannes E\nVidmantas\n\nAnd a big thanks to all of our alpha testers!");

            textBoxRight = TextUtils.CreateTextBox(spriteFont, new Rectangle(Game1.ScreenSize.X / 2 + xOffset, yOffset, Game1.ScreenSize.X / 2 - xOffset,
                                        Game1.ScreenSize.Y - 20), false, false,
                                        "Fonts:\n- 'Iceland' by Cyreal (SIL Open Font License) Downloaded from Google Fonts\n- 'ISL Jupiter' by Isurus Labs (Public domain) Downloaded from dafont.com \n\n" +
                                        "Space sounds:\n- 'Space Music Ambient' by evanjones4 (Creative Commons 0 Licence) Downloaded from FreeSound.org \n\n" +
                                        "Sound effects:\n- 'menu click' by fins (Creative Commons 0 Licence) Downloaded from FreeSound.org \n- 'Hover 2' by plasterbrain " +
                                        "(Creative Commons 0 Licence) Downloaded from FreeSound.org \n- '" +
                                        "ship fire' by Nbs Dark (Creative Commons 0 Licence) Downloaded from FreeSound.org \n- 'Thruster_Level_II' by nathanshadow (Sampling+ Licence) Downloaded from FreeSound.org \n" +
                                        "- 'Huge Explosion' by Tobiasz 'unfa' Karon (CC Attribution 3.0 Unported Licence) Downloaded from https://www.freesound.org/people/unfa/sounds/259300/");
        }

        public override void Initialize()
        {
            base.Initialize();
            backdrop = Game.Content.Load<Texture2D>("Overworld-Sprites/introBackdrop");
        }

        public override void Update(GameTime gameTime)
        {
            textBoxLeft.Update(gameTime);
            textBoxRight.Update(gameTime);

            if (ControlManager.CheckPress(RebindableKeys.Action1) 
                || ControlManager.CheckKeyPress(Keys.Enter)
                || ControlManager.CheckPress(RebindableKeys.Action2)
                || ControlManager.CheckKeyPress(Keys.Escape)
                || ControlManager.IsLeftMouseButtonClicked())
            {
                Game.stateManager.ChangeState("MainMenuState");
            } 
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(backdrop,
                             Vector2.Zero,
                             null,
                             Color.White,
                             0.0f,
                             Vector2.Zero,
                             new Vector2(Game1.ScreenSize.X / Game.DefaultResolution.X,
                                         Game1.ScreenSize.Y / Game.DefaultResolution.Y),
                             SpriteEffects.None,
                             0.5f);

            textBoxLeft.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);
            textBoxRight.Draw(spriteBatch, Game.fontManager.FontColor, Game.fontManager.FontOffset);
        }

    }
}
