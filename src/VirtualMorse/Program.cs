using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.Drawing;

namespace VirtualMorse
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SpeechSynthesizer synth = new SpeechSynthesizer();

            // Configure the audio output.   
            synth.SetOutputToDefaultAudioDevice();

            // Speak a string.  
            synth.Speak("Virtual Morse 2023");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();

            MyForm form = new MyForm();
            form.CreateMyForm();
        }
    }

    public class MyForm : Form
    {
        public void CreateMyForm()
        {
            // Create a new instance of the form.
            Form form1 = new Form();

            // Set the caption bar text of the form.   
            form1.Text = "My Dialog Box";

            form1.StartPosition = FormStartPosition.CenterScreen;

            RichTextBox richTextBox1 = new RichTextBox();
            richTextBox1.Dock = DockStyle.Fill;

            richTextBox1.LoadFile(".\\MyDocument.rtf");

            richTextBox1.SelectionFont = new Font("Verdana", 12, FontStyle.Bold);
            richTextBox1.SelectionColor = Color.Red;

            richTextBox1.SaveFile(".\\MyDocument.rtf", RichTextBoxStreamType.RichText);

            form1.Controls.Add(richTextBox1);

            // Display the form as a modal dialog box.
            form1.ShowDialog();
        }
    }
}