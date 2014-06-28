using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject
{
    public enum Music
    {
        //MainMenu,
        //Song1,
        //Song2,
        Rebels,
        //SpaceTravel,
        Asteroids,
        //Jigsaw,
        TheOboeSong,
        DarkSpace,
        MainMenu2,
        PowerSong,
        SpaceStation,
        none
    }

    public class MusicManager
    {
        private Game1 game;

        private List<Song> tracks;

        private Music activeSong;

        //private Song mainMenuSong;
        //private Song song1;
        //private Song song2;
        private Song rebels;
        //private Song spaceTravel;
        private Song asteroids;
        //private Song jigsaw;
        private Song theOboeSong;
        private Song darkSpace;
        private Song mainMenu2;
        private Song powerSong;
        private Song spaceStation;

        public Boolean IsSongPlaying(Music song) { return activeSong == song; }

        public MusicManager(Game1 game)
        {
            this.game = game;
        }

        ~MusicManager()
        {
            foreach (Song song in tracks)
                song.Dispose();
        }

        public void Initialize()
        {
            tracks = new List<Song>();

            activeSong = Music.none;
            MediaPlayer.IsRepeating = true;

            // Load settings
            MediaPlayer.IsMuted = game.settingsFile.GetPropertyAsBool("sound", "mutemusic", false);
            MediaPlayer.Volume = game.settingsFile.GetPropertyAsFloat("sound", "musicvolume", 1);

            //mainMenuSong = game.Content.Load<Song>("Music/SpaceProject_Intro");
            //song1 = game.Content.Load<Song>("Music/song1");
            //song2 = game.Content.Load<Song>("Music/song2");
            rebels = game.Content.Load<Song>("Music/Rebels");
            //spaceTravel = game.Content.Load<Song>("Music/SpaceTravel");
            asteroids = game.Content.Load<Song>("Music/Asteroids");
            //jigsaw = game.Content.Load<Song>("Music/Jigsaw");
            theOboeSong = game.Content.Load<Song>("Music/TheEditedOboeSong");
            darkSpace = game.Content.Load<Song>("Music/DarkSpace");
            mainMenu2 = game.Content.Load<Song>("Music/MainMenuTest");

            powerSong = game.Content.Load<Song>("Music/PowerSong");
            spaceStation = game.Content.Load<Song>("Music/SpaceStation");
            
            //tracks.Add(mainMenuSong);
            //tracks.Add(song1);
            //tracks.Add(song2);
            tracks.Add(rebels);
            //tracks.Add(spaceTravel);
            tracks.Add(asteroids);
            //tracks.Add(jigsaw);
            tracks.Add(theOboeSong);
            tracks.Add(darkSpace);
            tracks.Add(mainMenu2);
            tracks.Add(powerSong);
            tracks.Add(spaceStation);
        }

        public void PlayMusic(Music identifier)
        {
            if (MediaPlayer.State == MediaState.Playing && activeSong == identifier)
            {
                return;
            }

            activeSong = identifier;

            int i = (int)identifier;

            if (MediaPlayer.State == MediaState.Playing)
                MediaPlayer.Stop();

            if (identifier != Music.none)
                MediaPlayer.Play(tracks[i]);
            else
                MediaPlayer.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            MediaPlayer.Volume = volume;
        }

        public float GetMusicVolume()
        {
            return MediaPlayer.Volume;
        }

        public void SetMusicMuted(bool val)
        {
            MediaPlayer.IsMuted = val;
        }

        public void SwitchMusicMuted()
        {
            MediaPlayer.IsMuted = !MediaPlayer.IsMuted;
        }

        public bool IsMusicMuted()
        {
            return MediaPlayer.IsMuted;
        }
    }
}
