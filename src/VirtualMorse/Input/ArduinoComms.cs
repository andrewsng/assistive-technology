using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports; //access to SerialPort Class 


namespace VirtualMorse.Input
{
    public class ArduinoComms
    {
        public string coms;
        public int buad;
        SerialPort _serialPort;

        //Constructors
        public ArduinoComms()
        {
            _serialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);
            _serialPort.Open();
            _serialPort.DataReceived += DataHandler;
        }
        public ArduinoComms(string COMS, int baud_rate)
        {
            coms = COMS;
            buad = baud_rate;
            _serialPort = new SerialPort(COMS, baud_rate, Parity.None, 8, StopBits.One);
            _serialPort.Open();

        }

        //Event to Detect when a button is pressed 
        public event EventHandler<SwitchInputEventArgs> ButtonPressed;



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


                //Set up Loccal serial port and cast the sender object to it 
                SerialPort sp = (SerialPort)sender;
                string str = sp.ReadLine();
           

                if (targetKeys.ContainsKey(str.Trim()))
                {
                    Console.Write(str);
                    ButtonPressed?.Invoke(sp, new SwitchInputEventArgs(targetKeys[str.Trim()]));
                }
                else { Console.Write("nope"); }

            }
            catch (Exception e)
            {
               
            }
        }

    }
}
