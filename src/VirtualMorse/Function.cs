//Function.cs
//The purpose of this file is to implement functionality that would otherwise be out of place in the other files
//Right now, it implements reading morse code and email functions

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MailKit.Net.Smtp;
using MailKit;
using MimeKit;
using CsvHelper;
using CsvHelper.Configuration;
using System.Globalization;
using MailKit.Search;
using MailKit.Net.Imap;
using MailKit.Security;
using MimeKit.Encodings;

namespace VirtualMorse
{

    //Address book
    //Class used to represent one line in the address book
    public class AddressBook
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
    }

    public static class Function
    {
        static string addressBook = "AddressBook.csv";
        static string nickname = "";

        static Function()
        {
            DotNetEnv.Env.TraversePath().Load();
        }

        static Dictionary<string, char> morse_map = new Dictionary<string, char>() {
            // Letters
            {".-", 'a'},
            {"-...", 'b'},
            {"-.-.", 'c'},
            {"-..", 'd'},
            {".", 'e'},
            {"..-.", 'f'},
            {"--.", 'g'},
            {"....", 'h'},
            {"..", 'i'},
            {".---", 'j'},
            {"-.-", 'k'},
            {".-..", 'l'},
            {"--", 'm'},
            {"-.", 'n'},
            {"---", 'o'},
            {".--.", 'p'},
            {"--.-", 'q'},
            {".-.", 'r'},
            {"...", 's'},
            {"-", 't'},
            {"..-", 'u'},
            {"...-", 'v'},
            {".--", 'w'},
            {"-..-", 'x'},
            {"-.--", 'y'},
            {"--..", 'z'},

            // Numbers
            {".----", '1'},
            {"..---", '2'},
            {"...--", '3'},
            {"....-", '4'},
            {".....", '5'},
            {"-....", '6'},
            {"--...", '7'},
            {"---..", '8'},
            {"----.", '9'},
            {"-----", '0'},

            // Special Characters
            {".-.-.-", '.'},
            {"--..--", ','},
            {"..--..", '?'},
            {"-..-.", '/'},
            {".......", '!'},
            {".----.", '\''},
            {"-.-.-", ';'},
            {"---...", ':'},
            {"-....-", '-'},
            {"..--.-", '_'},
            {"-.--.-", '('},
            {"-.--..", ')'},

            // Custom Symbol (Email)
            {"..--", '@'}
        };


        //translates a morse string into a character
        public static char morseToText(string morse)
        {
            if (morse_map.ContainsKey(morse))
                return morse_map[morse];
            else
                return '\0';
        }


        //returns the last sentance in the current text document
        public static string getLastSentence(string document)
        {
            char[] chars = { '.', '!', '?' };
            List<string> sentences = document.Split(chars, StringSplitOptions.RemoveEmptyEntries).ToList();
            if (sentences.Count == 0)
            {
                return "";
            }
            sentences[sentences.Count - 1] = sentences.Last().Trim();
            while (sentences.Last() == "")
            {
                sentences.RemoveAt(sentences.Count - 1);
                if (sentences.Count == 0)
                {
                    return "";
                }
                sentences[sentences.Count - 1] = sentences.Last().Trim();
            }
            return sentences.Last();
        }
        
        //creates a Mailkit email object
        //The object has the address, subject, and email contents in a single place
        public static MimeMessage createEmail(string address, string contents)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Sender Name", DotNetEnv.Env.GetString("EMAIL__ACCOUNT") + "@gmail.com"));
            message.To.Add(new MailboxAddress("Receiver Name", address));
            message.Subject = "This message sent with Virtual Morse " + Program.programVersion;

            message.Body = new TextPart("plain")
            {
                Text = contents
            };

            return message;
        }

        // create a reply to a given email
        // https://github.com/jstedfast/MailKit/blob/master/FAQ.md#reply-message
        public static MimeMessage createReply(MimeMessage message, string contents)
        {
            var reply = new MimeMessage();

            reply.From.Add(new MailboxAddress("Sender Name", DotNetEnv.Env.GetString("EMAIL__ACCOUNT") + "@gmail.com"));
            
            if (message.ReplyTo.Count > 0)
            {
                reply.To.AddRange(message.ReplyTo);
            }
            else if (message.From.Count > 0)
            {
                reply.To.AddRange(message.From);
            }
            else if (message.Sender != null)
            {
                reply.To.Add(message.Sender);
            }

            if (!message.Subject.StartsWith("Re: ", StringComparison.OrdinalIgnoreCase))
            {
                reply.Subject = "Re: " + message.Subject;
            }
            else
            {
                reply.Subject = message.Subject;
            }

            reply.InReplyTo = message.MessageId;
            foreach (var id in message.References)
            {
                reply.References.Add(id);
            }
            reply.References.Add(message.MessageId);

            using (var quoted = new StringWriter())
            {
                var sender = message.Sender ?? message.From.Mailboxes.FirstOrDefault();
                quoted.WriteLine("On {0} {1} wrote:", message.Date.ToString("f"), !string.IsNullOrEmpty(sender.Name) ? sender.Name : sender.Address);
                using (var reader = new StringReader(message.TextBody))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        quoted.Write("> ");
                        quoted.WriteLine(line);
                    }
                }

                reply.Body = new TextPart("plain")
                {
                    Text = contents + "\n\n" + quoted.ToString()
                };
            }
            return reply;
        }

        //sends an email using Mailkit
        public static void sendEmail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                connectSmtpClient(client);
                
                client.Send(message);

                client.Disconnect(true);
            }
        }

        //retrieves an email based on a given index
        public static MimeMessage getEmail(int index, bool markAsRead = false)
        {
            MimeMessage message;
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;

                if (markAsRead)
                {
                    inbox.Open(FolderAccess.ReadWrite);
                    inbox.AddFlags(index, MessageFlags.Seen, true);
                }
                else
                {
                    inbox.Open(FolderAccess.ReadOnly);
                }

                message = inbox.GetMessage(index);
                
                client.Disconnect(true);
            }
            return message;
        }

        //returns the number of new emails, unopened emails, and total emails as a list
        public static List<int> getEmailCounts()
        {
            List<int> emailCounts = new List<int>();
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                // FIXME: How to deal with threaded conversations within the inbox.

                emailCounts.Add(inbox.Search(SearchQuery.New).Count());
                emailCounts.Add(inbox.Search(SearchQuery.NotSeen).Count());
                emailCounts.Add(inbox.Count);

                client.Disconnect(true);
            }
            return emailCounts;
        }
        
        //delete email
        //email to be deleted is based of its index
        public static void deleteEmail(int index)
        {
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadWrite);

                if (index >= inbox.Count)
                {
                    client.Disconnect(true);
                    throw new ArgumentException("Index greater than number of emails");
                }
                inbox.AddFlags(index, MessageFlags.Deleted, true);
                inbox.Expunge();

                client.Disconnect(true);
            }
        }
        public static void createNickname(string nickname)
        {
            Function.nickname = nickname;
        }

        //returns a list of all entries in the address book as a list of nickname / address pairs
        public static List<AddressBook> readAddressBook()
        {
            List<AddressBook> records = new List<AddressBook>();
            try
            {
                using (var reader = new StreamReader(Path.Combine(Program.fileDirectory, addressBook)))
                {
                    using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                    {
                        records = csv.GetRecords<AddressBook>().ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed to read from Address Book");
                Console.WriteLine(ex.Message);
            }
            return records;
        }

        //adds a given email / nickname pair to the address book
        public static void addEmailToBook(string email)
        {
            var records = readAddressBook();
            using (var writer = new StreamWriter(Path.Combine(Program.fileDirectory, addressBook)))
            {
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    records.Add(new AddressBook { Nickname = Function.nickname, Email = email });
                    csv.WriteRecords(records);
                }
            }
        }

        //checks to see if a given string is a nickname
        //if the string is in the address book, it is a nickname
        //      return the address corresponding to the nickname
        //otherwise return the string
        public static string checkNickname(string address)
        {
            string email = address;
            var records = readAddressBook();
            records.ForEach(record =>
            {
                if (record.Nickname.Equals(address))
                {
                    email = record.Email;
                }
            });
            return email;
        }

        //connects to MailKit servers
        static void connectMailService(MailService mailService, string host, int port, string errorMessage)
        {
            try
            {
                mailService.Connect(host, port, SecureSocketOptions.SslOnConnect);
                mailService.AuthenticationMechanisms.Remove("XOAUTH2");
                mailService.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));
            }
            catch
            {
                Console.WriteLine(errorMessage);
                throw;
            }
        }

        //connect to SMTP client
        //used for email
        static void connectSmtpClient(SmtpClient smtpClient)
        {
            connectMailService(smtpClient, "smtp.gmail.com", 465,
                "Error connecting or authenticating SMTP client.");
        }

        //connect to IMAP client
        //used for email
        static void connectImapClient(ImapClient imapClient)
        {
            connectMailService(imapClient, "imap.gmail.com", 993,
                "Error connecting or authenticating IMAP client.");
        }
    }
}

