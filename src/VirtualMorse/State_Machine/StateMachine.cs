using System;
using System.Drawing;
using System.Speech.Synthesis;
using System.Windows.Forms;
using VirtualMorse;
using VirtualMorse.State_Machine;

public class StateMachine
{
    RichTextBox textBox;

    FunctionKeyInput functionKeys;
    // ArduinoInput arduino;

    public SpeechSynthesizer speaker;

    State typingState;
	State commandState;
	State currentState;

    public string currentLetter = "";
    public string currentWord = "";

    string directory;
    string file = "test.txt";

    public StateMachine()
    {
        textBox = new RichTextBox();
        textBox.Dock = DockStyle.Fill;
        textBox.SelectionFont = new Font("Arial", 16, FontStyle.Regular);
        textBox.SelectionColor = Color.Black;
        textBox.AutoWordSelection = false;
        textBox.SelectionChanged += TextBox_SelectionChanged;

        functionKeys = new FunctionKeyInput();
        functionKeys.KeyPressed += Handler_InputReceived;
        textBox.KeyDown += functionKeys.TextBox_KeyDown;

        speaker = new SpeechSynthesizer();
        speaker.SetOutputToDefaultAudioDevice();
        speaker.SpeakAsync(Program.programName + " " + Program.programVersion);

        typingState = new TypingState(this);
        commandState = new CommandState(this);
        currentState = typingState;  //set initial state

        directory = AppDomain.CurrentDomain.BaseDirectory;
        directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
        setDocument(( Function.readFullFile(directory, file) )[0]);
    }

    private void Handler_InputReceived(object sender, SwitchInputEventArgs e)
    {
        Switch input = e.switchInput;
        switch (input)
        {
            case Switch.Switch1:  // Fall through
            case Switch.Switch9:
                currentState.command();
                break;
            case Switch.Switch2:
                currentState.shift();
                break;
            case Switch.Switch3:
                currentState.save();
                break;
            case Switch.Switch4:
                currentState.space();
                break;
            case Switch.Switch5:
                currentState.dot();
                break;
            case Switch.Switch6:
                currentState.dash();
                break;
            case Switch.Switch7:
                currentState.enter();
                break;
            case Switch.Switch8:
                currentState.backspace();
                break;
        }
        Console.WriteLine("current letter: '" + getCurrentLetter() + "'");
        Console.WriteLine("current word: '" + getCurrentWord() + "'");
        Console.WriteLine("current document: '" + getDocument() + "'");
        Console.WriteLine();
    }

    private void TextBox_SelectionChanged(Object sender, EventArgs e)
    {
        textBox.SelectionFont = new Font("Arial", 16, FontStyle.Regular);
        textBox.SelectionColor = Color.Black;
    }

    //fetching / setting functions

    public RichTextBox getTextBox()
    {
        return textBox;
    }

    public void setState(State state)
    {
		this.currentState = state;
    }

	public State getState()
    {
		return currentState;
    }

	public State getTypingState()
    {
		return typingState;
    }

	public State getCommandState()
    {
		return commandState;
    }

	public  string getCurrentWord()
	{
		return currentWord;
	}

	public  string getCurrentLetter()
	{
		return currentLetter;
	}

	public  string getDocument()
	{
		return textBox.Text;
	}

	public void setDocument(string text)
	{
        textBox.Focus();
        textBox.SelectionStart = 0;
        textBox.SelectionLength = textBox.TextLength;
        textBox.SelectedText = text;
    }

	public void appendToDocument(string text)
	{
		textBox.Focus();
		textBox.SelectionStart = textBox.TextLength;
		textBox.SelectionLength = 0;
        textBox.SelectedText = text;
	}

    public void backspaceDocument()
    {
        if (textBox.TextLength > 0)
        {
            textBox.Focus();
            textBox.SelectionStart = textBox.TextLength - 1;
            textBox.SelectionLength = 1;
            textBox.SelectedText = "";
        }
    }

    public void saveDocumentFile()
    {
        Function.addToFile(directory, file, getDocument());
    }
}