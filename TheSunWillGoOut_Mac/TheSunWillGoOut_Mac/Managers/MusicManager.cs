using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Media;

namespace SpaceProject_Mac
{
    public enum Music
    {
        GoingOut,
        MainMenu,
        Outro,
        SpaceStation,
        TheOboeSong,

        DarkPiano,
        DarkSpace,
        SpaceAmbience,

        AllianceBattle,
        Falling,
        PowerSong,
        Stars,
        
        none
    }

    public class MusicManager
    {
        private Game1 game;

        private List<Song> tracks;

        private Music activeSong;

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
            MediaPlayer.Volume = game.settingsFile.GetPropertyAsFloat("sound", "musicvolume", 0.6f);

            AddSongToTracks(tracks, "Music/other/GoingOut");
            AddSongToTracks(tracks, "Music/other/MainMenu");
            AddSongToTracks(tracks, "Music/other/Outro");
            AddSongToTracks(tracks, "Music/other/SpaceStation");
            AddSongToTracks(tracks, "Music/other/TheOboeSong");

            AddSongToTracks(tracks, "Music/overworld/DarkPiano");
            AddSongToTracks(tracks, "Music/overworld/DarkSpace");
            AddSongToTracks(tracks, "Music/overworld/SpaceAmbience");

            AddSongToTracks(tracks, "Music/vertical/AllianceBattle");
            AddSongToTracks(tracks, "Music/vertical/Falling");
            AddSongToTracks(tracks, "Music/vertical/PowerSong");
            AddSongToTracks(tracks, "Music/vertical/Stars");

        }

        private void AddSongToTracks(List<Song> tracks, String path)
        {
            tracks.Add(game.Content.Load<Song>(path));
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
