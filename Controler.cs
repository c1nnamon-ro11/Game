
using System.IO.Ports;

namespace FirstGame
{
    public static class Controler
    {
        static SerialPort serialPort;
        static bool isAutoFireOn = true;
        static bool isControllerConnected; //if controller - false, autoFire always true

        public static bool isShot;

        static public SerialPort SerialPort
        {
            get { return serialPort; }
            set { serialPort = value; }
        }

        static public bool IsAutoFireOn
        {
            get { return isAutoFireOn; }
            set { isAutoFireOn = value; }
        }

        static public bool IsControllerConnected
        {
            get { return isControllerConnected; }
            set { isControllerConnected = value; }
        }

        public static void ControlerInitialization()
        {
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 19200;
            try {
                serialPort.Open();
                isControllerConnected = true;
                isAutoFireOn = false;
            }
            catch {
                isControllerConnected = false;
            }                
        }

        public static void joystickControlSend(string shipScore)
        {
            serialPort.WriteLine(shipScore);           
        }

        public static string joystickContolOperation()
        {
            return serialPort.ReadLine();
        }
        
        public static void joystickControlOperation(Ship ship)
        {
            switch (serialPort.ReadLine())
            {
                case "1\r":
                    ship.Up();
                    break;

                case "2\r":
                    ship.Up();
                    ship.Right();
                    break;

                case "3\r":
                    ship.Right();
                    break;

                case "4\r":
                    ship.Right();
                    ship.Down();
                    break;

                case "5\r":
                    ship.Down();
                    break;

                case "6\r":
                    ship.Down();
                    ship.Left();
                    break;

                case "7\r":
                    ship.Left();
                    break;

                case "8\r":
                    ship.Up();
                    ship.Left();
                    break;

                case "9\r":
                    //Shot();
                    break;
                default: break;
            }
        }
    }
}
