> This page references information also available via our [System Architecture](https://github.com/andrewsng/assistive-technology/wiki/System-Architecture) diagram.


## Definitions 
- Switch: The signal that the State machine uses. Button or key inputs are translated into Switches.
- Button: Physical button on the Arduino.
- Key: Function Keys F1 - F9.
- Current Letter: A string that stores the dots and dashes currently entered.
- Current Word: A string that holds the current translated Morse Code characters.
- Current Document: A string that holds all of the words sent to the screen.
- index: the index of a specific email in the inbox. The oldest email is represented by index 1 and the newest email is represented by the largest current index.
- C#: Programming language used to create Virtual Morse
- .NET: C# Framework used to Manage COM ports and GUI
- Mailkit: C# library used for sending Emails.
- Mailkit object: an object representation of an email. Stores the sender address, receiver address, subject line, and body of an email in one variable.

## State Machine
There are currently 10 different switches in the system. The functionality of each switch is determined by the state machine, which handles the main logic of the program. A state machine is basically a flowchart, where depending on the inputs you give it, it moves from state to state. This functionality of the state machine allows us to let one switch to do multiple different things, depending on the context surrounding it. The following headers are the currently implemented States, and what each switch does in that state.

### Typing State
The typing state is the main state of the program. In it, a user can input Morse code to the document, alongside with other writing tools such as capitalization and saving.

- Switch 1 - Command: moves to command state.
- Switch 2 - Shift: toggles capitalization.
- Switch 3 - Save: Saves current text document.
- Switch 4 - Space: either adds the current word or adds a space to the document.
- Switch 5 - Dot: adds a dot to the current letter.
- Switch 6 - Dash: adds a dash to the current letter.
- Switch 7 - Enter: adds current letter to the current word.
- Switch 8 - Backspace: deletes current letter and last entered character.
- Switch 9 - Command: moves to command state.
- Switch 10 - Command: moves to command state.

### Command State
The command state is where different functions can be implemented. Currently, the command state deals with e-mail, printing, and other various functions.
- Switch 1 - Command: moves to punctuation state.
- Switch 2 - Print Page: prints the current document.
- Switch 3 - Clear Document: deletes all text from the current document. Utilizes confirmation state to avoid accidental clears.
- Switch 4: nothing
- Switch 5: nothing
- Switch 6: nothing
- Switch 7 - Enter Command: reads the current letter, and performs the command associated with it.
- Switch 8: nothing
- Switch 9 - Command: moves to punctuation state.
- Switch 10 - Command: moves to punctuation state.

The command to be executed is determined by the character read by Enter Command. The following list is all the current commands. If a command needs additional parameters (i.e. index of email, email address), then the additional parameter will be found in the current word.
- "l" - Read Last Sentence: reads the last sentence of the current document
- "g" - Check Email: returns the number of new, unread, and total emails.
- "d" - Delete Email: deletes the email with the specified index.
- "h" - Read Email Header: Reads the sender's address, sender's name, sent date, and subject line of the email with the specified index.
- "r" - Read Email: Reads the sender's address, sender's name, sent date, subject line, and email body of the email with the specified index.
- "e" - Send Email: Sends an email to the specified address. The current document serves as the body of the email.
- "y" - Reply: Sends an email to the email address corresponding to the specified index. One is the first email in the inbox, while the most recent email is the total number of emails. The current document serves as the body of the email.
- "n" - Add Nickname: Stores the specified nickname. Used in tandem with Store Email Address to add entries to the address book.
- "a" - Store Email Address: stores the specified email address, alongside the already stored nickname to the address book.

### Punctuation State
The punctuation state is used to quickly add punctuation to page, as their Morse equivalents take 6 - 8 dots or dashes to enter.
- Switch 1 - Command: moves to typing state.
- Switch 2 - Apostrophe: adds an apostrophe to the current word.
- Switch 3 - Quotation Mark: adds a quotation mark to the current word.
- Switch 4 - Exclamation Mark: adds an exclamation mark to the current word.
- Switch 5 - Period: adds a period to the current word.
- Switch 6 - Comma: adds a comma to the current word.
- Switch 7 - Question Mark: adds a question mark to the current word.
- Switch 8 - Hyphen: adds  a hyphen to the current word.
- Switch 9 - Command: moves to the typing state.
- Switch 10 - Command: moves to the typing state.

### Confirmation State
The confirmation state is used in tandem with the command state. Functions such as sending a reply and clearing the document are irreversible, so this state is implemented to prevent accidentally executing these commands.
- Switch 1: nothing
- Switch 2: nothing
- Switch 3: nothing
- Switch 4 - Confirm: used to confirm that you want to execute the command.
- Switch 5: nothing
- Switch 6: nothing
- Switch 7: nothing
- Switch 8 - Cancel: used to cancel the current command.
- Switch 9: nothing
- Switch 10: nothing

## Function.cs
This file is used to keep additionally required classes and procedures in a centralized location. The following list details all of the currently implemented classes and functions
- AddressBook: Class used to represent one line (one nickname / address) pair.
- Function: Class used to store helper functions.
- morse_map: Dictionary used to translate Morse code into characters.
- morseToText: function that returns the translated Morse code character.
- getLastSentance: returns the last sentence in the current document.
- createEmail: used to create a Mailkit object.
- createReply: creates a reply to a given email using a Mailkit object. additional information can be found at https://github.com/jstedfast/MailKit/blob/master/FAQ.md#reply-message.
- SendEmail: sends an email using a Mailkit object.
- getEmail: retrieves an email specified by the given index.
- getEmailCounts: returns the number of new, unopened, and total emails as a list.
- deleteEmail: deletes the email at the specified index.
- createNickname: stores the specified nickname.
- readAddressBook: returns a list of all the current nickname / address pairs in the address book.
- AddEmailToBook: stores the stored nickname and specified email as a new entry in the address book.
- checkNicname:  checks to see if the specified nickname is in the address book. If it is, return its associated email address.
- connectMailService: connects to the Mailkit Server.
- connectSmtpClient: connects to the SMTP client. Used in Mailkit.
- connectImapClient: connects to the IMAP client. Used in Mailkit.

