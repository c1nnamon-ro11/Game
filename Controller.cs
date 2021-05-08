using System.IO.Ports;

namespace FirstGame
{
    //Controller connection
    public static class Controller
    {
        //Controller`s variables
        static SerialPort serialPort;
        static bool isControlerConnected;
        static bool isAutoFire = true;
        static bool isShot;

        //Properties
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

        //Initialization of controller stats
        public static void ControlerInitialization()
        {
            //Controller`s stats
            serialPort = new SerialPort();
            serialPort.PortName = "COM4";
            serialPort.BaudRate = 19200;
            //attempt to connect controller to PC
            try
            {
                serialPort.Open();
                isControlerConnected = true;
            }
            catch
            {
                isControlerConnected = false;
            }
            //Abolition of autofire if game controller connected
            if (isControlerConnected)
            {
                isAutoFire = false;
            }
        }

        //Sending message (score) to controller to diplay
        public static void controllerSend(string message)
        {
            serialPort.WriteLine(message);
        }
        
        //Read first bite from controller to clean port before recieving information
        public static void controllerCleaner()
        {
            serialPort.ReadExisting();
        }

        //Recieving information from contoller
        public static string controllerRecieve()
        {
            return serialPort.ReadLine();
        }

        //Calculating Ship option, recieved from controller
        public static void controllerOperation(Ship ship)
        {
            controllerCleaner();
            //Operations
            //1-8 - moving
            //9 - shot
            //other - nothing
            switch (controllerRecieve())
            {
                case "1\r":
                    GameFunctional.GameStart();
                    ship.Up();
                    break;
                case "2\r":
                    GameFunctional.GameStart();
                    ship.Up();
                    ship.Right();
                    break;
                case "3\r":
                    GameFunctional.GameStart();
                    ship.Right();
                    break;
                case "4\r":
                    GameFunctional.GameStart();
                    ship.Right();
                    ship.Down();
                    break;
                case "5\r":
                    GameFunctional.GameStart();
                    ship.Down();
                    break;
                case "6\r":
                    GameFunctional.GameStart();
                    ship.Down();
                    ship.Left();
                    break;
                case "7\r":
                    GameFunctional.GameStart();
                    ship.Left();
                    break;
                case "8\r":
                    GameFunctional.GameStart();
                    ship.Up();
                    ship.Left();
                    break;
                case "9\r":
                    GameFunctional.GameStart();
                    isShot = true;
                    break;
                default: break;
            }
        }
    }
}
