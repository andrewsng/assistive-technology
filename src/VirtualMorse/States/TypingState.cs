// class TypingState
// Derived from base class State
// Has the implementation for the typing state, in which morse code input will be translated to the screen
using System;
using System.Collections.Generic;
using System.IO;
using System.Speech.Synthesis;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
	public class TypingState : State
    {
        bool isCapitalized = false;

        Dictionary<Switch, Action> switchResponses;

		//state functions
		public TypingState(WritingContext context) : base(context)
		{
			switchResponses = new Dictionary<Switch, Action>(){
				{ Switch.Switch1,  command },
				{ Switch.Switch2,  shift },
				{ Switch.Switch3,  save },
				{ Switch.Switch4,  space },
				{ Switch.Switch5,  dot },
				{ Switch.Switch6,  dash },
				{ Switch.Switch7,  enter },
				{ Switch.Switch8,  backspace },
				{ Switch.Switch9,  command },
				{ Switch.Switch10, command },
			};
        }

        public override void respond(Switch input)
        {
            if (switchResponses.ContainsKey(input))
            {
                switchResponses[input]();
            }
        }

		//adds a dot to the current word
        void dot()
		{
			Console.WriteLine("storing dot");
            context.currentMorse += '.';
        }

		//adds a dash to the current word
		void dash()
		{
			Console.WriteLine("storing dash");
            context.currentMorse += '-';
        }

		//if there is a current word, add word to the document
		//if there is not a current word, add a space to the document
		void space()
		{
			if (context.currentWord != "")
			{
				context.appendToDocument(context.currentWord);
				Console.WriteLine("added word to file: " + context.currentWord);
				Speech.speak(context.currentWord);
                context.clearWord();
            }
			else
			{
				context.appendToDocument(" ");
				Console.WriteLine("SPACE added to file");
				Speech.speak("Space.");
			}
		}

		//toggles capitalization
		void shift()
		{
			if (isCapitalized)
			{
                Speech.speak("Shift cancelled.");
            }
			else
			{
				Speech.speak("Shift.");
			}
            isCapitalized = !isCapitalized;
            Console.WriteLine("capitalization set to: " + isCapitalized);
		}

		//translates current morse code letter
		//if it is a valid letter, add it to the current word
		//if it isn't valid, alert user
		//if adding the letter results in TTT, read current document and clear current word
		void enter()
		{
			string morseString = context.currentMorse;
            char nextLetter;
            if (morseString == "" && context.lastLetter != '\0')
			{
                nextLetter = context.lastLetter;
			}
			else
			{
				nextLetter = Function.morseToText(morseString);
				if (isCapitalized)
				{
					nextLetter = char.ToUpper(nextLetter);
					isCapitalized = false;
				}
			}

            PromptBuilder spokenMessage = new PromptBuilder();
            if (nextLetter != '\0')
			{
                context.currentWord += nextLetter;
                context.lastLetter = nextLetter;
                context.clearMorse();
                Console.WriteLine("added letter: " + nextLetter);
				if (context.currentWord == "ttt")
				{
					context.clearWord();
					string document = context.getDocument();
					spokenMessage.AppendText((document != "") ? document : "This page is blank.");
                    Console.WriteLine("\"ttt\" entered - reading entire page");
                }
				else
                {
					if (char.IsUpper(nextLetter))
                    {
                        spokenMessage.AppendText("Capital ");
                    }
                    spokenMessage.AppendTextWithHint(nextLetter.ToString(), SayAs.SpellOut);
                    spokenMessage.AppendText(".");
                }
            }
			else
			{
				context.clearMorse();
				Console.WriteLine("not a valid letter, try again");
				spokenMessage.AppendText("Try again");
            }
            Speech.speak(spokenMessage);
		}

		//if a morse letter is in progress, delete all of it
		//if not, remove last character
		void backspace()
		{
			if (context.currentWord.Length > 0)
			{
				context.currentWord = context.currentWord.Remove(context.currentWord.Length - 1, 1);
				Console.WriteLine("Delete");
				Speech.speak("Delete.");
			}
			else
			{
				context.backspaceDocument();
				Console.WriteLine("Backspace");
				Speech.speak("Backspace.");
			}
		}

		//save text file
		void save()
        {
            Speech.speak("Now saving.");
            Console.WriteLine("save text doc as is");
			try
            {
                context.saveDocumentFile(Path.Combine(Program.fileDirectory, context.getTextFile()));
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error saving file");
                Speech.speak("Error saving file");
                Console.WriteLine(ex.Message);
            }
		}

		//move to  command state
		void command()
		{
			context.transitionToState(new CommandState(context));
            Console.WriteLine("move to command state");
			Speech.speak("Command Mode.");
        }
	}
}
