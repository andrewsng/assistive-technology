using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace VirtualMorse
{
    public static class Speech
    {
        static SpeechSynthesizer speaker;
        static bool blockInputs = false;

        static Speech()
        {
            speaker = new SpeechSynthesizer();
            speaker.SetOutputToDefaultAudioDevice();
            speaker.SpeakCompleted += synth_SpeakCompleted;
        }

        public static void speak(string message)
        {
            cancelSpeech();
            speaker.SpeakAsync(message);
        }
        public static void speak(PromptBuilder message)
        {
            cancelSpeech();
            speaker.SpeakAsync(message);
        }

        public static void speakFully(string message)
        {
            speak(message);
            blockInputs = true;
        }
        public static void speakFully(PromptBuilder message)
        {
            speak(message);
            blockInputs = true;
        }

        public static void cancelSpeech()
        {
            if (!blockInputs)
            {
                speaker.SpeakAsyncCancelAll();
            }
        }

        public static bool isBlockingInputs()
        {
            return blockInputs;
        }

        static void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (!e.Cancelled)  // Only when speech ran to completion (was not cancelled)
            {
                if (blockInputs)
                {
                    blockInputs = false;
                }
            }
        }
    }
}
