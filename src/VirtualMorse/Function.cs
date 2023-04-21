using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Speech.Synthesis;
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
    public class AddressBook
    {
        public string Nickname { get; set; }
        public string Email { get; set; }
    }




    public static class Function
    {
        static string directory;
        static string addressBook;
        static string virtualMorseVersion = "2023";
        static string nickname = "";
        static SpeechSynthesizer speaker;
        static Function()
        {
            directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
            addressBook = "AddressBook.csv";
            DotNetEnv.Env.TraversePath().Load();

            speaker = new SpeechSynthesizer();
            speaker.SetOutputToDefaultAudioDevice();
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

        public static char morseToText(string morse)
        {
            if (morse_map.ContainsKey(morse))
                return morse_map[morse];
            else
                return '\0';
        }

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

        public static void addToFile(string directory, string file, string text)
        {

            using (StreamWriter writer = new StreamWriter(directory + file))
            {
                writer.WriteLine(text);
            }
        }
        public static List<string> readFullFile(string directory, string file)
        {
            List<string> return_string = new List<string>();
            using (StreamReader reader = new StreamReader(directory + file))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    return_string.Add(line);
                }
            }
            return return_string;
        }

        public static void speak(string message)
        {
            speaker.SpeakAsync(message);
        }

        public static void cancelSpeech()
        {
            speaker.SpeakAsyncCancelAll();
        }

        public static MimeMessage createEmail(string address, string contents)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Sender Name", DotNetEnv.Env.GetString("EMAIL__ACCOUNT") + "@gmail.com"));
            message.To.Add(new MailboxAddress("Receiver Name", address));
            message.Subject = "This message sent with Virtual Morse " + virtualMorseVersion;

            message.Body = new TextPart("plain")
            {
                Text = contents
            };

            return message;
        }

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

        public static void sendEmail(MimeMessage message)
        {
            using (var client = new SmtpClient())
            {
                connectSmtpClient(client);
                
                client.Send(message);

                client.Disconnect(true);
            }
        }

        public static MimeMessage getEmail(int index)
        {
            MimeMessage message;
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                message = inbox.GetMessage(index);

                client.Disconnect(true);
            }
            return message;
        }

        public static string readEmail(int index)
        {
            string body = "";
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var message = inbox.GetMessage(index);

                body = message.TextBody;

                // TODO: Detect hyperlinks in email body and avoid reading them out loud.

                client.Disconnect(true);
            }
            return body;
        }

        public static List<string> getEmailHeader(int index)
        {
            List<string> emailHeader = new List<string>();
            using (var client = new ImapClient())
            {
                connectImapClient(client);

                var inbox = client.Inbox;
                inbox.Open(FolderAccess.ReadOnly);

                var message = inbox.GetMessage(index);

                emailHeader.Add((index + 1).ToString());
                emailHeader.Add(message.Date.ToString());
                emailHeader.Add(message.From.ToString());
                emailHeader.Add(message.From.Mailboxes.FirstOrDefault().Address.ToString());
                emailHeader.Add(message.Subject.ToString());

                client.Disconnect(true);
            }
            return emailHeader;
        }

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

        public static void addEmailToBook(string email)
        {

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = false,
            };

            using (var writer = new StreamWriter(directory + addressBook, true))
            {
                using (var csv = new CsvWriter(writer, config))
                {
                    csv.NextRecord();
                    AddressBook new_entry = new AddressBook { Nickname = Function.nickname, Email = email };
                    csv.WriteRecord(new_entry);

                }
            }
        }

        public static string checkNickname(string address)
        {
            string email = address;

            using (var reader = new StreamReader(directory + addressBook))
            {
                using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
                {
                    var records = csv.GetRecords<AddressBook>();

                    records.ToList().ForEach(record =>
                    {
                        if (record.Nickname.Equals(address))
                        {
                            email = record.Email;
                        }

                    });
                }
            }
            return email;
        }

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

        static void connectSmtpClient(SmtpClient smtpClient)
        {
            connectMailService(smtpClient, "smtp.gmail.com", 465,
                "Error connecting or authenticating SMTP client.");
        }

        static void connectImapClient(ImapClient imapClient)
        {
            connectMailService(imapClient, "imap.gmail.com", 993,
                "Error connecting or authenticating IMAP client.");
        }
    }
}

