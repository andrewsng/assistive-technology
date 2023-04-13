using System;
using System.IO.Ports; //access to SerialPort Class 

class ArduinoComms
{

    //Main Method 
    static void Main(string[] args)
    {
        //Set up New Serial Port w/
        //constructors (string portName, int baudRate, System.IO.PortsPairty, int bits, System.IO.PORTS.STopBits stopBits);

        SerialPort _serialPort = new SerialPort("COM4", 9600, Parity.None, 8, StopBits.One);


        try //Erorr may occur when opening a Serial Port 
        {
            _serialPort.Open();

            while (true)
            {
                string str = _serialPort.ReadLine();

                Console.WriteLine(str);

            }
        }
        catch //Throw exception
        {
            Console.WriteLine("Error");

        }
        finally //clean up resources used by try block 
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();


            }
        }
    }
}