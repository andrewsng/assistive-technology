﻿using System;
using System.Collections.Generic;
using VirtualMorse.Input;

namespace VirtualMorse.States
{
    public class CommandState : State
    {
        static string nickname = "";

        Dictionary<Switch, Action> switchResponses;

        public CommandState(WritingContext context) : base(context)
        {
            switchResponses = new Dictionary<Switch, Action>(){
                { Switch.Switch1,  command },
                { Switch.Switch2,  printPage },
                { Switch.Switch3,  clearDocument },
                { Switch.Switch7,  enterCommand },
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
            else
            {
                moveToTypingState();
                sayUnprogrammedError();
            }
        }
        void command()
        {
            context.setState(context.getPunctuationState());
            speak("move to punctuation state.");
        }

        void printPage()
        {
            Console.WriteLine("Print page");
            moveToTypingState();
        }

        void clearDocument()
        {
            context.setDocument("");
            moveToTypingState();
            Console.WriteLine("clear document");
            speak("Document cleared.");
        }

        void enterCommand()
        {
            string commandLetter = Function.morseToText(context.currentLetter);
            List<string> header;
            string address = "";
            string contents = "";
            string index = "";
            switch (commandLetter)
            {
                case "l":
                    Console.WriteLine("read last sentence");
                    string sentence = Function.getLastSentence(context.getDocument());
                    Console.WriteLine(sentence);
                    speak(sentence);
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "g":
                    Console.WriteLine("checks email");
                    List<int> email_count = Function.checkEmail();
                    if (Function.has_executed == true)
                    {
                        int unread = email_count[0];
                        int total = email_count[1];
                        speak("you have " + unread + " unread emails.");
                        speak("you have " + total + " total emails.");
                    }
                    else
                    {
                        speak("failed to retrieve email header");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "d":
                    Console.WriteLine("deletes email");
                    index = context.getCurrentWord();
                    Function.deleteEmail(Int32.Parse(index));
                    if (Function.has_executed == true)
                    {
                        speak("email number " + index + " deleted");
                    }
                    else
                    {
                        speak("failed to delete email");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "h":
                    Console.WriteLine("read email headers");
                    index = context.getCurrentWord();
                    header = Function.readEmailHeader(Int32.Parse(index));
                    if (Function.has_executed == true)
                    {
                        speak("Email header number: " + header[0]);
                        speak("Date and time sent: " + header[1]);
                        speak("Sender's display name: " + header[2]);
                        speak("Sender's address: " + header[3]);
                        speak("Email subject line: " + header[4]);
                    }
                    else
                    {
                        speak("failed to read email header");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "r":
                    Console.WriteLine("reads email");
                    index = context.getCurrentWord();
                    header = Function.readEmailHeader(Int32.Parse(index));
                    if (Function.has_executed == true)
                    {
                        speak("Email header number: " + header[0]);
                        speak("Date and time sent: " + header[1]);
                        speak("Sender's display name: " + header[2]);
                        speak("Sender's address: " + header[3]);
                        speak("Email subject line: " + header[4]);
                        string email = Function.readEmail(Int32.Parse(index));
                        speak("Email Contents: " + email);
                    }
                    else
                    {
                        speak("failed to read email");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "e":
                    Console.WriteLine("create/send email");
                    address = context.getCurrentWord();
                    contents = context.getDocument();
                    Function.sendEmail(address, contents);

                    if(Function.has_executed == true)
                    {
                        speak("sent");
                    }
                    else
                    {
                        speak("Email failed to send");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "y":
                    Console.WriteLine("reply to email");
                    index = context.getCurrentWord();
                    contents = context.getDocument();
                    header = Function.readEmailHeader(Int32.Parse(index));
                    Function.sendEmail(header[3], contents);
                    if (Function.has_executed == true)
                    {
                        speak("replied to " + header[3]);
                    }
                    else
                    {
                        speak("failed to reply to email");
                    }
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "n":
                    Console.WriteLine("adds email address nickname");
                    nickname = context.getCurrentWord();
                    Function.createNickname(nickname);
                    speak("added nickname " + nickname);
                    context.clearLetter();
                    context.clearWord();
                    break;

                case "a":
                    Console.WriteLine("ties email address to nickname");
                    address = context.getCurrentWord();
                    Function.addEmailToBook(address);
                    speak("added email address " + address);
                    context.clearLetter();
                    context.clearWord();
                    break;

                default:
                    Console.WriteLine("invalid command");
                    sayUnprogrammedError();
                    break;
            }
            moveToTypingState();
        }

        // Helper functions

        void moveToTypingState()
        {
            context.clearLetter();
            context.setState(context.getTypingState());
            Console.WriteLine("move to typing state");
        }

        void sayUnprogrammedError()
        {
            speak("That command is not programmed.");
        }
    }
}
