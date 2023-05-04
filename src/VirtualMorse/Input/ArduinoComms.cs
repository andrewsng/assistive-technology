// class ArduinoComms
// Derived from base class InputSource
// Maintains a serial port connection
// Function SerialPort_DataReceived runs whenever the
//   SerialPort.DataReceived event is raised.

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.IO.Ports; //access to SerialPort Class 

namespace VirtualMorse.Input
{
    public class ArduinoComms : InputSource
    {
        SerialPort _serialPort;

        //Constructors
        public ArduinoComms(RichTextBox textBox) : base(textBox)
        {
            _serialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += SerialPort_DataReceived;  // Subscribe to DataReceived event
        }

        public ArduinoComms(RichTextBox textBox, string portName, int baudRate) : base(textBox)
        {
            _serialPort = new SerialPort(portName, baudRate, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += SerialPort_DataReceived;  // Subscribe to DataReceived event
        }

        //Dictionary holding switches associated with strings read from serial input
        public Dictionary<string, Switch> serialInputMap = new Dictionary<string, Switch>()
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

        private void SerialPort_DataReceived(object sender, SerialDataReceivedEventArgs button)
        {
            try //Error may occur when opening a Serial Port 
            {
                //Set up Local serial port and cast the sender object to it 
                SerialPort sp = (SerialPort)sender;
                string str = sp.ReadLine();
                str = str.Trim();
                if (serialInputMap.ContainsKey(str))
                {
                    // This SerialPort event handler runs automatically on a separate thread
                    //   (because the SerialPort is constantly waiting for data to come in).
                    // Calling OnSwitchActivated might make changes to the textBox which are only
                    //   allowed on the main thread that the textBox runs on. We create an Action
                    //   then invoke that Action through the textBox so it runs on it's own thread.
                    // See Remarks here https://learn.microsoft.com/en-us/dotnet/api/system.io.ports.serialport.datareceived
                    Action action = delegate () {
                        OnSwitchActivated(new SwitchInputEventArgs(serialInputMap[str]));
                    };
                    textBox.Invoke(action);  // Action must be run on the text box's thread.
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("{0} Exception caught.", e);
            }
        }
    }
}
