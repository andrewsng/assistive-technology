using MimeKit;
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
            Console.WriteLine("Move to punctuation state.");
            Speech.speak("Punctuation Mode.");
        }

        void printPage()
        {
            Console.WriteLine("Print page");
            moveToTypingState();
        }

        void clearDocument()
        {
            Console.WriteLine("clear document");
            Speech.speakFully("Clear document.");
            context.transitionToState(
                new ConfirmationState(
                    context,
                    () => {
                        context.setDocument("");
                        Speech.speak("Document cleared.");
                    }
                )
            );
        }

        bool tryEmailFunction(Func<WritingContext, string> function, string errorMessage)
        {
            string output;
            bool success = true;
            try
            {
                output = function(context);
            }
            catch (Exception ex)
            {
                success = false;
                output = errorMessage;
                Console.WriteLine(ex.Message);
            }
            Console.WriteLine(output);
            Speech.speak(output);
            return success;
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
                    Speech.speak(sentence);
                    break;

                case 'g':
                    Console.WriteLine("checks email");
                    Speech.speakFully("Checking email.");
                    tryEmailFunction(
                        context => {
                            Function.EmailCounts emailCount = Function.getEmailCounts();
                            return $"You have {emailCount.Unread} unread emails.\n" +
                                   $"You have {emailCount.Total} total emails.";
                        },
                        "Failed to check emails."
                    );
                    break;
                case 'd':
                    Console.WriteLine("deletes email");
                    Speech.speakFully("Deleting email.");
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
                    Speech.speakFully("Reading email header.");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            var message = Function.getEmail(emailIndex);
                            var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();
                            return $"Email {context.getCurrentWord()} " +
                                   $"is from {sender.Name} " +
                                   $"with address {sender.Address} " +
                                   $"on {message.Date.Date.ToString("d")}.\n" +
                                   $"Subject: {message.Subject}.\n";

                        },
                        "Failed to read email header."
                    );
                    break;
                case 'r':
                    Console.WriteLine("reads email");
                    Speech.speakFully("Reading email.");
                    tryEmailFunction(
                        context => {
                            int emailIndex = parseIndex(context.getCurrentWord());
                            var message = Function.getEmail(emailIndex, true);
                            var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();
                            return $"Email {context.getCurrentWord()} " +
                                   $"is from {sender.Name} " +
                                   $"with address {sender.Address} " +
                                   $"on {message.Date.Date.ToString("d")}.\n" +
                                   $"Subject: {message.Subject}\n" +
                                   $"Email Contents:\n{message.TextBody}\n";
                        },
                        "Failed to read email."
                    );
                    break;
                case 'e':
                    Console.WriteLine("create/send email");
                    Speech.speakFully("Sending email.");
                    tryEmailFunction(
                        context => {
                            string address = context.getCurrentWord();
                            address = Function.checkNickname(address);
                            string contents = context.getDocument();
                            Function.sendEmail(Function.createEmail(address, contents));
                            return $"Email sent to {address}.";
                        },
                        "Failed to send email."
                    );
                    break;
                case 'y':
                    Console.WriteLine("reply to email");
                    Speech.speakFully("Replying to email.");
                    MimeMessage saveMessage = null;
                    string senderName = null;
                    string output = "Failed to reply to email.";
                    try {
                        int emailIndex = parseIndex(context.getCurrentWord());
                        saveMessage = Function.getEmail(emailIndex);
                        var sender = saveMessage.Sender ?? saveMessage.From.Mailboxes.FirstOrDefault();
                        senderName = (!string.IsNullOrEmpty(sender.Name) ? sender.Name : sender.Address);
                        output = $"Reply to email number {context.getCurrentWord()} from {sender.Name ?? ""} {sender.Address}.";
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        break;
                    }
                    finally
                    {
                        Console.WriteLine(output);
                        Speech.speakFully(output);
                    }
                    context.transitionToState(
                        new ConfirmationState(
                            context,
                            () => {
                                tryEmailFunction(
                                    context =>
                                    {
                                        string contents = context.getDocument();
                                        Function.sendEmail(Function.createReply(saveMessage, contents));
                                        return $"Reply to {senderName} has been sent.";
                                    },
                                    "Failed to send reply."
                                );
                            }
                        )
                    );
                    context.clearMorse();
                    context.clearWord();
                    return;
                case 'n':
                    Console.WriteLine("adds email address nickname");
                    string nickname = context.getCurrentWord();
                    Function.createNickname(nickname);
                    Speech.speakFully("Nickname " + nickname);
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
            Speech.speak("That command is not programmed.");
        }

        int parseIndex(string str)
        {
            return Int32.Parse(str) - 1;
        }
    }
}
