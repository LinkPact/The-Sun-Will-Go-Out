using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SpaceProject_Mac
{
    class SoundOptionsSubState : OptionSubState
    {
        public SoundOptionsSubState(Game1 game, Sprite buttonsSprite, OptionsMenuState optionsMenuState, String name) :
            base(game, buttonsSprite, optionsMenuState, name)
        {
        }

        private MenuDisplayObject musicLeftButton;
        private MenuDisplayObject musicRightButton;

        private MenuDisplayObject soundLeftButton;
        private MenuDisplayObject soundRightButton;

        public override void Initialize()
        {
            base.Initialize();

            musicLeftButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(20, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(game.Window.ClientBounds.Width - 168, game.Window.ClientBounds.Height / 3 - 1 + (1 * 22)),
                new Vector2(6, 5));
            musicLeftButton.Initialize();

            musicRightButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(game.Window.ClientBounds.Width - 112, game.Window.ClientBounds.Height / 3 - 1 + (1 * 22)),
                new Vector2(6, 5));
            musicRightButton.Initialize();

            soundLeftButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(20, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(game.Window.ClientBounds.Width - 168, game.Window.ClientBounds.Height / 3 - 1 + (3 * 22)),
                new Vector2(6, 5));
            soundLeftButton.Initialize();
            
            soundRightButton = new MenuDisplayObject(game, buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)), buttonsSprite.GetSubSprite(new Rectangle(4, 135, 12, 11)),
                new Vector2(game.Window.ClientBounds.Width - 112, game.Window.ClientBounds.Height / 3 - 1 + (3 * 22)),
                new Vector2(6, 5));
            soundRightButton.Initialize();
            
            soundLeftButton.name = "left sound";
            soundRightButton.name = "right sound";
            musicLeftButton.name = "left music";
            musicRightButton.name = "right music";

            directionalButtons.Add(musicLeftButton);
            directionalButtons.Add(musicRightButton);
            directionalButtons.Add(soundLeftButton);
            directionalButtons.Add(soundRightButton);

            menuOptions = new String[5, 2];
        }

        public override void OnDisplay()
        {
            menuOptions[0, 0] = "Music";
            if (game.musicManager.IsMusicMuted())
                menuOptions[0, 1] = "Off";
            else
                menuOptions[0, 1] = "On";

            menuOptions[1, 0] = "Music Volume";
            menuOptions[1, 1] = Math.Round(game.musicManager.GetMusicVolume() * 10).ToString();

            menuOptions[2, 0] = "Sound Effects";
            if (game.soundEffectsManager.isSoundMuted())
                menuOptions[2, 1] = "Off";
            else
                menuOptions[2, 1] = "On";
            
            menuOptions[3, 0] = "Sound Volume";
            menuOptions[3, 1] = Math.Round(game.soundEffectsManager.GetSoundVolume() * 10).ToString();

            //menuOptions[4, 0] = "Load Sound Effects";
            //if (SoundEffectsManager.LoadSoundEffects)
            //{
            //    menuOptions[4, 1] = "On";
            //}
            //else
            //{
            //    menuOptions[4, 1] = "Off";
            //}

            //menuOptions[5, 0] = "Text-To-Speech";
            //if (TextToSpeech.TTSMode == TextToSpeechMode.Full)
            //{
            //    menuOptions[5, 1] = "Full";
            //}
            //else if (TextToSpeech.TTSMode == TextToSpeechMode.Dialog)
            //{
            //    menuOptions[5, 1] = "Dialog";
            //}
            //else
            //{
            //    menuOptions[5, 1] = "Off";
            //}

            menuOptions[4, 0] = "Back";
            menuOptions[4, 1] = "";
            base.OnDisplay();
        }

        public override void OnEnter()
        {
            cursorIndex = 0;
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnLeave()
        {

        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (ControlManager.CheckPress(RebindableKeys.Right))
                VolumeControl("right");

            else if (ControlManager.CheckPress(RebindableKeys.Left))
                VolumeControl("left");

            if (!SoundEffectsManager.LoadSoundEffects)
            {
                game.soundEffectsManager.SetSoundMuted(true);
                menuOptions[2, 1] = "Off";
            }
        }

        public override void ButtonActions()
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
                case "music":
                    game.musicManager.SwitchMusicMuted();
                    break;
            
                case "sound effects":
                    game.soundEffectsManager.SwitchSoundMuted();
                    break;

                case "load sound effects":
                    SoundEffectsManager.LoadSoundEffects = !SoundEffectsManager.LoadSoundEffects;
                    break;

                case "text-to-speech":
                    int index = (int)TextToSpeech.TTSMode;
                    if (++index > (int)TextToSpeechMode.Off)
                    {
                        index = 0;
                    }
                    TextToSpeech.TTSMode = (TextToSpeechMode)index;
                    break;
            
                case "back":
                    optionsMenuState.LeaveSubState();
                    OnLeave();

                    PlayLowPitchSelectSound();
                    break;
            }

            UpdateText();
        }

        public override void DirectionalButtonActions(String buttonName) 
        {
            switch (buttonName.ToLower())
            {
                case "left music":
                    cursorIndex = 1;
                    VolumeControl("left");
                    break;

                case "left sound":
                    cursorIndex = 3;
                    VolumeControl("left");
                    break;

                case "right music":
                    cursorIndex = 1;
                    VolumeControl("right");
                    break;
                
                case "right sound":
                    cursorIndex = 3;
                    VolumeControl("right");
                    break;

                default:
                    throw new ArgumentException(String.Format("'%s' is not a valid identifier."));
            }
        }

        public void VolumeControl(String dir)
        {
            switch (menuOptions[cursorIndex, 0].ToLower())
            {
                case "music volume":
                    if(dir == "right")
                        game.musicManager.SetMusicVolume(game.musicManager.GetMusicVolume() + 0.1f);
                    else
                        game.musicManager.SetMusicVolume(game.musicManager.GetMusicVolume() - 0.1f);
                    break;
            
                case "sound volume":
                    if (dir == "right")
                        game.soundEffectsManager.SetSoundVolume(
                            game.soundEffectsManager.GetSoundVolume() + 0.1f);
                    else
                        game.soundEffectsManager.SetSoundVolume(
                            game.soundEffectsManager.GetSoundVolume() - 0.1f);
                
                    game.soundEffectsManager.PlaySoundEffect(SoundEffects.SmallLaser, 0f);
                    break;
            }

            UpdateText();
        }

        private void UpdateText()
        {
            if (game.musicManager.IsMusicMuted())
                menuOptions[0, 1] = "Off";
            else
                menuOptions[0, 1] = "On";
            
            menuOptions[1, 1] = Math.Round(game.musicManager.GetMusicVolume() * 10 ).ToString();

            if (game.soundEffectsManager.isSoundMuted())
                menuOptions[2, 1] = "Off";
            else
                menuOptions[2, 1] = "On";
            
            menuOptions[3, 1] = Math.Round(game.soundEffectsManager.GetSoundVolume() * 10).ToString();

            //if (SoundEffectsManager.LoadSoundEffects)
            //{
            //    menuOptions[4, 1] = "On";
            //}
            //else
            //{
            //    menuOptions[4, 1] = "Off";
            //}
            //
            //if (TextToSpeech.TTSMode == TextToSpeechMode.Full)
            //{
            //    menuOptions[5, 1] = "Full";
            //}
            //else if (TextToSpeech.TTSMode == TextToSpeechMode.Dialog)
            //{
            //    menuOptions[5, 1] = "Dialog";
            //}
            //else
            //{
            //    menuOptions[5, 1] = "Off";
            //}
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}