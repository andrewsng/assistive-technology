using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace VirtualMorse
{
    public class Program : Form
    {
        public StateMachine stateMachine;
        public static string programName = "Virtual Morse";
        public static string programVersion = "2023";

        public Program()
        {
            stateMachine = new StateMachine();

            this.Text = programName + " " + programVersion;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.Controls.Add(stateMachine.getTextBox());
        }

        [STAThread]
        public static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.Run(new Program());
        }
    }
}