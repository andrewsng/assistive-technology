using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO.Ports; //access to SerialPort Class 


namespace VirtualMorse.Input
{
    public class ArduinoComms
    {
        RichTextBox textBox;
        SerialPort _serialPort;

        //Constructors
        public ArduinoComms(RichTextBox textBox)
        {
            this.textBox = textBox;
            _serialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += DataHandler;
        }

        public ArduinoComms(RichTextBox textBox, string portName, int baudRate)
        {
            this.textBox = textBox;
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += DataHandler;
        }

        //Event to Detect when a button is pressed 
        public event EventHandler<SwitchInputEventArgs> buttonPressed;

        //Dictionary holding all button presses, and associated switches
        public Dictionary<string, Switch> targetKeys = new Dictionary<string, Switch>()
        {
            {  "0", Switch.Switch1 },
            {  "1", Switch.Switch2 },
            {  "2", Switch.Switch3 },
            {  "3", Switch.Switch4 },
            {  "4", Switch.Switch5 },
            {  "5", Switch.Switch6 },
            {  "6", Switch.Switch7 },
            {  "7", Switch.Switch8 },
            {  "8", Switch.Switch9 },
            {  "9", Switch.Switch10 },
        };

        private void DataHandler(object sender, SerialDataReceivedEventArgs button)
        {
            try //Error may occur when opening a Serial Port 
            {
                //Set up Local serial port and cast the sender object to it 
                SerialPort sp = (SerialPort)sender;
                string str = sp.ReadLine();
                str = str.Trim();
                if (targetKeys.ContainsKey(str))
                {
                    Action action = delegate () {
                        buttonPressed?.Invoke(this, new SwitchInputEventArgs(targetKeys[str]));
                    };
                    textBox.Invoke(action);  // Event must be handled on the text box's thread.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }
    }
}
