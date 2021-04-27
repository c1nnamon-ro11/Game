using System.IO.Ports;

namespace FirstGame
{
    public static class Controler
    {
        static SerialPort serialPort;
        static bool isControlerConnected;
        static bool isAutoFire = true;
        static bool isShot;

        public static SerialPort SerialPort
        {
            get { return serialPort; }
            set { serialPort = value; }
        }

        public static bool IsControlerConnected
        {
            get { return isControlerConnected; }
            set { isControlerConnected = value; }
        }

        public static bool IsAutoFire
        {
            get { return isAutoFire; }
            set { isAutoFire = value; }
        }

        public static bool IsShot
        {
            get { return isShot; }
            set { isShot = value; }
        }

        public static void ControlerInitialization()
        {
            //Arduino
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 19200;
            try
            {
                serialPort.Open();
                isControlerConnected = true;
            }
            catch
            {
                isControlerConnected = false;
            }
            if (isControlerConnected)
            {
                isAutoFire = false;
            }
        }

        public static void controlerSend(string message)
        {
            serialPort.WriteLine(message);
        }
        
        public static void controlerCleaner()
        {
            serialPort.ReadExisting();
        }

        public static string controlerRecieve()
        {
            return serialPort.ReadLine();
        }

        public static void controlerOperation(Ship ship)
        {
            controlerCleaner();
            switch (controlerRecieve())
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
                    isShot = true;
                    break;
                default: break;
            }
        }
    }
}
