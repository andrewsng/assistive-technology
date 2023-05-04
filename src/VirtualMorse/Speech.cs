// static class Speech
// Maintains a single instance of the .NET SpeechSynthesizer.
// Offers functions to speak a message made with a string or a PromptBuilder.
// This class implements behavior where all spoken messages cancel the previous message,
//   but function 'speakFully' allows a spoken message to block inputs and other speech.
//   This is done using a Queue for message prompts, where the front of the queue is
//   checked if it is interruptible when adding a new message.

using System;
using System.Collections.Generic;
using System.Speech.Synthesis;

namespace VirtualMorse
{
    public static class Speech
    {
        static SpeechSynthesizer speaker;
        static int speechRate = -2;  // Variable for controlling speech rate - possible values [-10, 10]
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

        // Cancels current speech if the prompt at the front of
        //   the queue is not blocking inputs.
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
