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
            _serialPort = new SerialPort("COM3", 9600, Parity.None, 8, StopBits.One);
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
            {  "2", Switch.Switch1 },
            {  "3", Switch.Switch2 },
            {  "4", Switch.Switch3 },
            {  "5", Switch.Switch4 },
            {  "6", Switch.Switch5 },
            {  "7", Switch.Switch6 },
            {  "8", Switch.Switch7 },
            {  "9", Switch.Switch8 },
            { "10", Switch.Switch9 },
            { "11", Switch.Switch10 },
        };

        private void DataHandler(object sender, SerialDataReceivedEventArgs button)
        {
            try //Erorr may occur when opening a Serial Port 
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
               
            }
        }
    }
}
