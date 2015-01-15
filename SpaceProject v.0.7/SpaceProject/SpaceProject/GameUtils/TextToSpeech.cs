using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Speech.Synthesis;

namespace SpaceProject
{
    public class TextToSpeech
    {
        // Constants
        private static readonly string MaleVoice = "Microsoft David Desktop";
        private static readonly string FemaleVoice = "Microsoft Hazel Desktop";
        private static readonly string SAIRVoice = "Microsoft Zira Desktop";

        // variables
        private static SpeechSynthesizer synth = new SpeechSynthesizer();

        private static bool useTTS;

        // properties
        public static bool UseTTS
        {
            get
            {
                return useTTS;
            }

            set
            {
                if (IsVoicesInstalled())
                {
                    useTTS = value;
                }
                else
                {
                    useTTS = false;
                }
            }
        }
        public static int Volume { get { return synth.Volume; } set { synth.Volume = value; } }

        public static void Initialize()
        {
            if (useTTS)
            {
                synth.SetOutputToDefaultAudioDevice();
                synth.SelectVoice(MaleVoice);
            }
        }

        public static void Stop()
        {
            if (useTTS
                && synth.State == SynthesizerState.Speaking)
            {
                synth.SpeakAsyncCancelAll();
            }
        }

        public static void Speak(string text, int rate)
        {
            if (useTTS)
            {
                synth.Rate = rate;
                SetVoice(text);
                synth.SpeakAsync(FormatText(text));
            }
        }

        private static string FormatText(string s)
        {
            bool copyChar = false;

            StringBuilder newString = new StringBuilder("");

            for (int i = 0; i < s.Length; i++)
            {
                if (s[i] == '\"')
                {
                    copyChar = !copyChar;
                }

                if (copyChar)
                {
                    newString.Append(s[i]);
                }
            }

            if (s.Contains("Ai"))
            {
                newString.Replace("Ai", "Eye");
            }

            return newString.ToString();
        }

        private static void SetVoice(string s)
        {
            if (s.Contains("[SAIR]"))
            {
                synth.SelectVoice(SAIRVoice);
            }

            else if (s.Contains("[Ai]")
                || s.Contains("[Squad member 2]"))
            {
                synth.SelectVoice(FemaleVoice);
            }

            else if (s.Contains("[Captain]")
                || s.Contains("[Berr]")
                || s.Contains("[Commander]")
                || s.Contains("[Rok]")
                || s.Contains("[Other escort ship]")
                || s.Contains("[Rebel Ship]")
                || s.Contains("[Squad member 1]")
                || s.Contains("[Squad member 3]")
                || s.Contains("[Attack fleet leader]")
                || s.Contains("[Ente]")
                || s.Contains("[Rebel]")
                || s.Contains("[Civilian]"))
            {
                synth.SelectVoice(MaleVoice);
            }
        }

        private static bool IsVoicesInstalled()
        {
            int count = 0;

            foreach (InstalledVoice voice in synth.GetInstalledVoices())
            {
                if (voice.VoiceInfo.Name.Equals(MaleVoice))
                {
                    count++;
                    continue;
                }

                if (voice.VoiceInfo.Name.Equals(FemaleVoice))
                {
                    count++;
                    continue;
                }

                if (voice.VoiceInfo.Name.Equals(SAIRVoice))
                {
                    count++;
                    continue;
                }
            }

            if (count == 3)
            {
                return true;
            }

            return false;
        }
    }
}
