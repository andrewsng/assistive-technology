using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
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
            context.transitionToState(new PunctuationState(context));
            Function.speak("move to punctuation state.");
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
            Function.speak("Document cleared.");
        }

        void tryEmailFunction(Func<WritingContext, string> function, string errorMessage)
        {
            string output;
            try
            {
                output = function(context);
            }
            catch (Exception ex)
            {
                output = errorMessage;
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(output);
            Function.speak(output);
        }

        void enterCommand()
        {
            char commandLetter = Function.morseToText(context.currentMorse);
            switch (commandLetter)
            {
                case 'l':
                    Console.WriteLine("read last sentence");
                    string sentence = Function.getLastSentence(context.getDocument());
                    Console.WriteLine(sentence);
                    Function.speak(sentence);
                    break;

                case 'g':
                    Console.WriteLine("checks email");
                    tryEmailFunction(
                        context => {
                            List<int> email_count = Function.getEmailCounts();
                            return $"You have {email_count[0]} new emails.\n" +
                                   $"You have {email_count[1]} unread emails.\n" +
                                   $"You have {email_count[2]} total emails.";
                        },
                        "Failed to check emails."
                    );
                    break;
                case 'd':
                    Console.WriteLine("deletes email");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            Function.deleteEmail(emailIndex);
                            return $"Email number {context.getCurrentWord()} deleted.";
                        },
                        "Failed to delete email."
                        );
                    break;
                case 'h':
                    Console.WriteLine("read email headers");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            List<string> header = Function.getEmailHeader(emailIndex);
                            return $"Email header number: {header[0]}\n" +
                                   $"Date and time sent: {header[1]}\n" +
                                   $"Sender's display name: {header[2]}\n" +
                                   $"Sender's address: {header[3]}\n" +
                                   $"Email subject line: {header[4]}";

                        },
                        "Failed to read email header."
                        );
                    break;
                case 'r':
                    Console.WriteLine("reads email");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            List<string> header = Function.getEmailHeader(emailIndex);
                            string body = Function.readEmail(emailIndex);
                            return $"Email header number: {header[0]}\n" +
                                   $"Date and time sent: {header[1]}\n" +
                                   $"Sender's display name: {header[2]}\n" +
                                   $"Sender's address: {header[3]}\n" +
                                   $"Email subject line: {header[4]}\n" +
                                   $"Email Contents: {body}";
                        },
                        "Failed to read email."
                        );
                    break;
                case 'e':
                    Console.WriteLine("create/send email");
                    tryEmailFunction(
                        context => {
                            string address = context.getCurrentWord();
                            address = Function.checkNickname(address);
                            string contents = context.getDocument();
                            Function.sendEmail(Function.createEmail(address, contents));
                            return $"Sending email to {address}.";
                        },
                        "Failed to send email."
                        );
                    break;
                case 'y':
                    Console.WriteLine("reply to email");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            var message = Function.getEmail(emailIndex);

                            string contents = context.getDocument();
                            Function.sendEmail(Function.createReply(message, contents));
                            var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();
                            return $"Replied to {(!string.IsNullOrEmpty(sender.Name) ? sender.Name : sender.Address)}.";
                        },
                        "Failed to reply to email."
                        );
                    break;
                case 'n':
                    Console.WriteLine("adds email address nickname");
                    nickname = context.getCurrentWord();
                    Function.createNickname(nickname);
                    Function.speak("added nickname " + nickname);
                    break;
                case 'a':
                    Console.WriteLine("ties email address to nickname");
                    tryEmailFunction(
                        context => {
                            string address = context.getCurrentWord();
                            Function.addEmailToBook(address);
                            return $"Added email address {address}";
                        },
                        "Failed to add email as a nickname."
                        );
                    break;
                default:
                    Console.WriteLine("invalid command");
                    sayUnprogrammedError();
                    break;
            }
            context.clearMorse();
            context.clearWord();
            moveToTypingState();
        }

        // Helper functions

        void moveToTypingState()
        {
            context.clearMorse();
            context.transitionToState(new TypingState(context));
            Console.WriteLine("move to typing state");
        }

        void sayUnprogrammedError()
        {
            Function.speak("That command is not programmed.");
        }

        int parseIndex(string str)
        {
            return Int32.Parse(str) - 1;
        }
    }
}
