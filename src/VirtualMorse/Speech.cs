using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace VirtualMorse
{
    public static class Speech
    {
        static SpeechSynthesizer speaker;
        static int speechRate = -2;  // Possible values [-10, 10]
        static Queue<(Prompt, bool)> messageQueue = new Queue<(Prompt, bool)>();

        static Speech()
        {
            speaker = new SpeechSynthesizer();
            speaker.SetOutputToDefaultAudioDevice();
            speaker.Rate = speechRate;
            speaker.SpeakCompleted += synth_SpeakCompleted;
        }

        public static void speak(string message)
        {
            queuePrompt(new Prompt(message), false);
        }
        public static void speak(PromptBuilder message)
        {
            queuePrompt(new Prompt(message), false);
        }

        public static void speakFully(string message)
        {
            queuePrompt(new Prompt(message), true);
        }
        public static void speakFully(PromptBuilder message)
        {
            queuePrompt(new Prompt(message), true);
        }

        public static void cancelSpeech()
        {
            if (!isBlockingInputs())
            {
                speaker.SpeakAsyncCancel(messageQueue.Peek().Item1);
            }
        }

        public static bool isBlockingInputs()
        {
            if (messageQueue.Count > 0)
            {
                return messageQueue.Peek().Item2;
            }
            else
            {
                return false;
            }
        }

        static void queuePrompt(Prompt prompt, bool canBeCancelled)
        {
            messageQueue.Enqueue((prompt, canBeCancelled));
            cancelSpeech();
            speaker.SpeakAsync(prompt);
        }

        static void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            if (messageQueue.Count > 0)
            {
                messageQueue.Dequeue();
            }
        }
    }
}
