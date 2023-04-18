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
        static public bool has_executed = true;
        static Function()
        {
            directory = AppDomain.CurrentDomain.BaseDirectory;
            directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
            addressBook = "AddressBook.csv";
            DotNetEnv.Env.TraversePath().Load();

        }
        static Dictionary<string, string> morse_map = new Dictionary<string, string>() {
            // Letters
            {".-", "a"},
            {"-...", "b"},
            {"-.-.", "c"},
            {"-..", "d"},
            {".", "e"},
            {"..-.", "f"},
            {"--.", "g"},
            {"....", "h"},
            {"..", "i"},
            {".---", "j"},
            {"-.-", "k"},
            {".-..", "l"},
            {"--", "m"},
            {"-.", "n"},
            {"---", "o"},
            {".--.", "p"},
            {"--.-", "q"},
            {".-.", "r"},
            {"...", "s"},
            {"-", "t"},
            {"..-", "u"},
            {"...-", "v"},
            {".--", "w"},
            {"-..-", "x"},
            {"-.--", "y"},
            {"--..", "z"},

            // Numbers
            {".----", "1"},
            {"..---", "2"},
            {"...--", "3"},
            {"....-", "4"},
            {".....", "5"},
            {"-....", "6"},
            {"--...", "7"},
            {"---..", "8"},
            {"----.", "9"},
            {"-----", "0"},

            // Special Characters
            {".-.-.-", "."},
            {"--..--", ","},
            {"..--..", "?"},
            {"-..-.", "/"},
            {".......", "!"},
            {".----.", "'"},
            {"-.-.-", ";"},
            {"---...", ":"},
            {"-....-", "-"},
            {"..--.-", "_"},
            {"-.--.-", "("},
            {"-.--..", ")"},

            // Custom Symbol (Email)
            {"..--", "@"}
        };

        public static string morseToText(string morse)
        {
            if (morse_map.ContainsKey(morse))
                return morse_map[morse];
            else
                return "";
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


        public static void sendEmail(string address, string contents)
        {
            address = checkNickname(address);

            var message = new MimeMessage();
            string test = DotNetEnv.Env.GetString("EMAIL__ACCOUNT") + "@gmail.com";
            Console.WriteLine(test);
            Console.WriteLine(address);
            message.From.Add(new MailboxAddress("Sender Name", test));
            message.To.Add(new MailboxAddress("Receiver Name", address));
            message.Subject = "This message sent with Virtual Morse " + virtualMorseVersion;

            message.Body = new TextPart("plain")
            {
                Text = contents
            };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587);

                client.AuthenticationMechanisms.Remove("XOAUTH2");

                // Note: only needed if the SMTP server requires authentication.
                client.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));

                // Speaks name & email destination (spells out email address).
                Console.WriteLine(">> Sending email to [LOCATION].");
                
                    client.Send(message);
                    has_executed = true;
                }
                catch(Exception e)
                {
                    Console.WriteLine("Email failed to send");
                    has_executed = false;

                }
                finally
                {
                    client.Disconnect(true);
                }
                
            }
        }


        public static string readEmail(int index)
        {
            index--;
            string body = "";

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, true);

                    client.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    // Email header number 
                    // Oldest email = 1 vs. newest email = nth inbox location.

                    // Decrement emailNumber by 1 to get the correct email header number.
                    if (index >= 0 && index < inbox.Count)
                    {
                        var message = inbox.GetMessage(index);
                        body = message.TextBody;
                    }

                    // TODO: Detect hyperlinks in email body and avoid reading them out loud.

                    Console.WriteLine("Body: {0}", body);

                    client.Disconnect(true);
                    has_executed = true;
                }
                catch
                {
                    Console.WriteLine("failed to read email");
                    has_executed = false;
                }
                finally
                {
                    client.Disconnect(true);
                }
            }
            return body;
        }


        public static List<string> readEmailHeader(int index)
        {
            index--;
            List<string> return_string = new List<string>();


            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, true);

                client.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));
                
                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    // Email header number 
                    // Oldest email = 1 vs. newest email = nth inbox location.
                    if (index >= 0 && index < inbox.Count)
                    {
                        var message = inbox.GetMessage(index);
                        var dateSent = message.Date;
                        var senderName = message.From;
                        var senderAddress = message.From.Mailboxes.FirstOrDefault().Address;
                        var subjectLine = message.Subject;

                        Console.WriteLine("Date Sent: {0}", dateSent);
                        Console.WriteLine("Sender Name: {0}", senderName);
                        Console.WriteLine("Sender Address: {0}", senderAddress);
                        Console.WriteLine("Subject Line: {0}", subjectLine);

                        return_string.Add((index + 1).ToString());
                        return_string.Add(dateSent.ToString());
                        return_string.Add(senderName.ToString());
                        return_string.Add(senderAddress.ToString());
                        return_string.Add(subjectLine.ToString());

                    }

                    client.Disconnect(true);
                    has_executed = true;
                }
                catch
                {
                    Console.WriteLine("failed to retrieve header");
                    has_executed = false;
                }
                finally
                {
                    client.Disconnect(true);
                }
            }

            return return_string;
        }

        public static List<int> checkEmail()
        {
            List<int> return_list = new List<int>();


            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, true);

                    client.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadOnly);

                    Console.WriteLine(">> Checking mail server...");

                    var unreadEmails = inbox.Search(SearchQuery.NotSeen);

                    if (unreadEmails.Count == 1)
                    {
                        Console.WriteLine(">> You have {0} unread email.", unreadEmails.Count);
                    }
                    else
                    {
                        Console.WriteLine(">> You have {0} unread emails.", unreadEmails.Count);
                    }

                    var totalMessages = inbox.Count;

                    // FIXME: How to deal with threaded conversations within the inbox.

                    Console.WriteLine(">> Total messages: {0}", totalMessages);

                    return_list.Add(unreadEmails.Count);
                    return_list.Add(inbox.Count);


                    client.Disconnect(true);
                    has_executed = true;
                }
                catch
                {
                    Console.WriteLine("failed to check email");
                    has_executed = false;
                }
                finally
                {
                    client.Disconnect(true);
                }


            }

            return return_list;
        }


        public static void deleteEmail(int index)
        {
            index--;

            using (var client = new ImapClient())
            {
                try
                {
                    client.Connect("imap.gmail.com", 993, true);

                    client.Authenticate(DotNetEnv.Env.GetString("EMAIL__ACCOUNT"), DotNetEnv.Env.GetString("APP__PASSWORD"));

                    var inbox = client.Inbox;
                    inbox.Open(FolderAccess.ReadWrite);

                    if (index >= 0 && index < inbox.Count)
                    {
                        inbox.AddFlags(index, MessageFlags.Deleted, true);

                        inbox.Expunge();

                        Console.WriteLine(">> Deleted.");
                    }

                    client.Disconnect(true);
                    has_executed = true;
                }
                catch
                {
                    Console.WriteLine("failed to delete email");
                    has_executed = false;
                }
                finally
                {
                    client.Disconnect(true);
                }
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

    }
}

