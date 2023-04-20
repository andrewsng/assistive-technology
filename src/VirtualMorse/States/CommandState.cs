using System;
using System.Collections.Generic;
using System.Data;
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

        void enterCommand()
        {
            char commandLetter = Function.morseToText(context.currentMorse);
            string address = "";
            string contents = "";
            string index = "";
            switch (commandLetter)
            {
                case 'l':
                    Console.WriteLine("read last sentence");
                    string sentence = Function.getLastSentence(context.getDocument());
                    Console.WriteLine(sentence);
                    Function.speak(sentence);
                    context.clearMorse();
                    context.clearWord();
                    break;

                case 'g':
                    Console.WriteLine("checks email");
                    try
                    {
                        List<int> email_count = Function.getEmailCounts();
                        int newEmails = email_count[0];
                        int unread = email_count[1];
                        int total = email_count[2];
                        Console.WriteLine(">> You have {0} new emails.", newEmails);
                        Console.WriteLine(">> You have {0} unread emails.", unread);
                        Console.WriteLine(">> Total messages: {0}", total);
                        Function.speak("you have " + unread + " unread emails.");
                        Function.speak("you have " + total + " total emails.");
                    }
                    catch
                    {
                        Console.WriteLine("Failed to check emails.");
                        Function.speak("Failed to check emails.");
                    }
                    context.clearMorse();
                    context.clearWord();
                    break;
                case 'd':
                    {
                        Console.WriteLine("deletes email");
                        int emailIndex;
                        try
                        {
                            emailIndex = Int32.Parse(context.getCurrentWord());
                        }
                        catch
                        {
                            Console.WriteLine("Error parsing index from current word");
                            break;
                        }

                        try
                        {
                            Function.deleteEmail(emailIndex);
                            Function.speak("email number " + emailIndex + " deleted");
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to delete email.");
                            Console.WriteLine(ex.Message);
                            Function.speak("Failed to delete email.");
                        }
                        context.clearMorse();
                        context.clearWord();
                        break;
                    }
                case 'h':
                    {
                        Console.WriteLine("read email headers");
                        int emailIndex;
                        try
                        {
                            emailIndex = Int32.Parse(context.getCurrentWord());
                        }
                        catch
                        {
                            Console.WriteLine("Error parsing index from current word");
                            break;
                        }

                        List<string> header;
                        try
                        {
                            header = Function.getEmailHeader(emailIndex);

                            Console.WriteLine("Date Sent: {0}", header[1]);
                            Console.WriteLine("Sender Name: {0}", header[2]);
                            Console.WriteLine("Sender Address: {0}", header[3]);
                            Console.WriteLine("Subject Line: {0}", header[4]);
                            Function.speak("Email header number: " + header[0]);
                            Function.speak("Date and time sent: " + header[1]);
                            Function.speak("Sender's display name: " + header[2]);
                            Function.speak("Sender's address: " + header[3]);
                            Function.speak("Email subject line: " + header[4]);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to read email header.");
                            Console.WriteLine(ex.Message);
                            Function.speak("Failed to read email header.");
                        }
                        context.clearMorse();
                        context.clearWord();
                        break;
                    }
                case 'r':
                    {
                        Console.WriteLine("reads email");
                        int emailIndex;
                        try
                        {
                            emailIndex = Int32.Parse(context.getCurrentWord());
                        }
                        catch
                        {
                            Console.WriteLine("Error parsing index from current word");
                            break;
                        }

                        List<string> header;
                        try
                        {
                            header = Function.getEmailHeader(emailIndex);
                            string body = Function.readEmail(emailIndex);

                            Console.WriteLine("Date Sent: {0}", header[1]);
                            Console.WriteLine("Sender Name: {0}", header[2]);
                            Console.WriteLine("Sender Address: {0}", header[3]);
                            Console.WriteLine("Subject Line: {0}", header[4]);
                            Console.WriteLine("Body: {0}", body);
                            Function.speak("Email header number: " + header[0]);
                            Function.speak("Date and time sent: " + header[1]);
                            Function.speak("Sender's display name: " + header[2]);
                            Function.speak("Sender's address: " + header[3]);
                            Function.speak("Email subject line: " + header[4]);
                            Function.speak("Email Contents: " + body);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("Failed to read email .");
                            Console.WriteLine(ex.Message);
                            Function.speak("Failed to read email.");
                        }
                        context.clearMorse();
                        context.clearWord();
                        break;
                    }
                case 'e':
                    Console.WriteLine("create/send email");
                    address = context.getCurrentWord();
                    contents = context.getDocument();
                    Function.sendEmail(address, contents);

                    if(Function.has_executed == true)
                    {
                        Function.speak("sent");
                    }
                    else
                    {
                        Function.speak("Email failed to send");
                    }
                    context.clearMorse();
                    context.clearWord();
                    break;

                case 'y':
                    {
                        List<string> header;
                        Console.WriteLine("reply to email");
                        index = context.getCurrentWord();
                        contents = context.getDocument();
                        header = Function.getEmailHeader(Int32.Parse(index));
                        Function.sendEmail(header[3], contents);
                        if (Function.has_executed == true)
                        {
                            Function.speak("replied to " + header[3]);
                        }
                        else
                        {
                            Function.speak("failed to reply to email");
                        }
                        context.clearMorse();
                        context.clearWord();
                        break;
                    }
                case 'n':
                    Console.WriteLine("adds email address nickname");
                    nickname = context.getCurrentWord();
                    Function.createNickname(nickname);
                    Function.speak("added nickname " + nickname);
                    context.clearMorse();
                    context.clearWord();
                    break;

                case 'a':
                    Console.WriteLine("ties email address to nickname");
                    address = context.getCurrentWord();
                    Function.addEmailToBook(address);
                    Function.speak("added email address " + address);
                    context.clearMorse();
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
            context.clearMorse();
            context.transitionToState(new TypingState(context));
            Console.WriteLine("move to typing state");
        }

        void sayUnprogrammedError()
        {
            Function.speak("That command is not programmed.");
        }
    }
}
