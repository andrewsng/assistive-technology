using System;
using System.Drawing;
using System.Windows.Forms;
using VirtualMorse.State_Machine;

public class StateMachine
{
    RichTextBox textBox;

    State typingState;
	State commandState;
	State state;

    public string currentLetter = "";
    public string currentWord = "";

    string directory;
    string file = "test.txt";

    public StateMachine(RichTextBox textBoxRef)
    {
        textBox = textBoxRef;  // must be before typingState is constructed

        typingState = new TypingState(this);
        commandState = new CommandState(this);

        //set initial state
        state = typingState;

        directory = AppDomain.CurrentDomain.BaseDirectory;
        directory = directory.Replace("bin\\Debug\\", "Text_documents\\");
        setDocument(( Function.readFullFile(directory, file) )[0]);
    }

	//Initialize States
	public void dot()
    {
		state.dot();
    }

	public void dash()
    {
		state.dash();
    }

	public void space()
    {
		state.space();
    }

	public void shift()
    {
		state.shift();
    }

	public void enter()
    {
		state.enter();
    }

	public void backspace()
    {
		state.backspace();
    }

	public void save()
    {
		state.save();
    }

	public void command()
    {
		state.command();
    }

	//fetching / setting functions for states
	public void setState(State state)
    {
		this.state = state;
    }

	public State getState()
    {
		return state;
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
