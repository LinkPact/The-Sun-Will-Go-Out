using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceProject
{
    public enum TextToSpeechMode
    {
        Dialog,
        Full,
        Off
    }
    
    public class TextToSpeech
    {
        #region Constants

        /// Windows 8 voices
        private static readonly string MaleVoiceW8 = "Microsoft David Desktop";
        private static readonly string FemaleVoiceW8 = "Microsoft Hazel Desktop";
        private static readonly string SAIRVoiceW8 = "Microsoft Zira Desktop";

        // Windows 7/Vista voices
        private static readonly string VoiceW7 = "Microsoft Anna";

        // Windows XP voices
        private static readonly string MaleVoiceWXP = "Microsoft Mike";
        private static readonly string FemaleVoiceWXP = "Microsoft Mary";

        public const int DefaultRate = 2;
        public const int FastRate = 5;

        #endregion

        #region Variables
        //private static SpeechSynthesizer synth = new SpeechSynthesizer();
        private static string maleVoice;
        private static string femaleVoice;
        private static string sairVoice;

        private static TextToSpeechMode ttsMode;
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets (if OS is supported) text-to-speech mode.
        /// </summary>
        public static TextToSpeechMode TTSMode
        {
            get
            {
                return ttsMode;
            }

            set
            {
                if (IsOSSupported())
                {
                    ttsMode = value;
                }
                else
                {
                    ttsMode = TextToSpeechMode.Off;
                }
            }
        }

        //public static int Volume { get { return synth.Volume; } set { synth.Volume = value; } }

        #endregion

        public static void Initialize()
        {
            if (IsOSSupported())
            {
                SetVoices();
                //synth.SetOutputToDefaultAudioDevice();
                //synth.SelectVoice(maleVoice);
            }
        }

        /// <summary>
        /// Cancels all currently playing text-to-speech voices.
        /// </summary>
        public static void Stop()
        {
            //if (ttsMode != TextToSpeechMode.Off
            //    && synth.State == SynthesizerState.Speaking)
            //{
            //    synth.SpeakAsyncCancelAll();
            //}
        }

        /// <summary>
        /// Starts text-to-speech on string parameter if text-to-speech is enabled.
        /// </summary>
        /// <param name="text">Text to be spoken</param>
        /// <param name="rate">Rate of speech</param>
        public static void Speak(string text, int rate = DefaultRate)
        {
            if (ttsMode != TextToSpeechMode.Off)
            {
                //synth.Rate = rate;
                //SelectVoice(text);
                //
                //synth.SpeakAsync(FormatText(text));
            }
        }

        /// <summary>
        /// Format string parameter so only text within quotation marks is returned.
        /// </summary>
        /// <param name="s">Unformatted text</param>
        /// <returns>Returns only text to be spoken.</returns>
        private static string FormatText(string s)
        {
            bool copyChar = false;

            StringBuilder newString = new StringBuilder("");

                for (int i = 0; i < s.Length; i++)
                {
                    if (TTSMode == TextToSpeechMode.Dialog)
                    {
                        if (s[i] == '\"')
                        {
                            copyChar = !copyChar;
                        }
                    }
                    else
                    {
                        copyChar = true;
                    }
                        
                    if (copyChar)
                    {
                        newString.Append(s[i]);
                    }      
                }

            if (newString.ToString().Contains("]"))
            {
                newString.Insert(newString.ToString().IndexOf("]"), "says: ");
            }

            if (newString.ToString().Contains("SAIR"))
            {
                newString.Replace("SAIR", "Sair");
            }

            if (newString.ToString().Contains("Ai"))
            {
                newString.Replace("Ai", "Eye");
            }

            return newString.ToString();
        }

        /// <summary>
        /// Selects voice depending on certain keywords in string parameter. 
        /// </summary>
        /// <param name="s"></param>
        private static void SelectVoice(string s)
        {
            //if (s.Contains("[SAIR]"))
            //{
            //    synth.SelectVoice(sairVoice);
            //}
            //
            //else if (s.Contains("[Ai]")
            //    || s.Contains("[Squad member 2]"))
            //{
            //    synth.SelectVoice(femaleVoice);
            //}
            //
            //else if (s.Contains("[Captain]")
            //    || s.Contains("[Berr]")
            //    || s.Contains("[Commander]")
            //    || s.Contains("[Rok]")
            //    || s.Contains("[Other escort ship]")
            //    || s.Contains("[Rebel Ship]")
            //    || s.Contains("[Squad member 1]")
            //    || s.Contains("[Squad member 3]")
            //    || s.Contains("[Attack fleet leader]")
            //    || s.Contains("[Ente]")
            //    || s.Contains("[Rebel]")
            //    || s.Contains("[Civilian]"))
            //{
            //    synth.SelectVoice(maleVoice);
            //}
        }

        private static bool IsOSSupported()
        {
            return !StaticFunctions.GetOSName().Equals("Unknown");
        }

        /// <summary>
        /// Sets default voices depending on OS.
        /// </summary>
        private static void SetVoices()
        {
            switch (StaticFunctions.GetOSName())
            {
                case "Windows 2000":
                case "Windows XP":
                    maleVoice = MaleVoiceWXP;
                    femaleVoice = FemaleVoiceWXP;
                    sairVoice = FemaleVoiceWXP;
                    break;

                case "Windows Vista":
                case "Windows 7":
                    maleVoice = VoiceW7;
                    femaleVoice = VoiceW7;
                    sairVoice = VoiceW7;
                    break;

                case "Windows 8":
                case "Windows 8.1":
                case "Windows 10":
                    maleVoice = MaleVoiceW8;
                    femaleVoice = FemaleVoiceW8;
                    sairVoice = SAIRVoiceW8;
                    break;
            }
        }
    }
}
