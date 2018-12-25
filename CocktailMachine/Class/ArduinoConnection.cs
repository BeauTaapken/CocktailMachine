using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;
using System.Text.RegularExpressions;
using CocktailMachine.Window;

namespace CocktailMachine.Class
{
    public class ArduinoConnection
    {
        private SerialPort serialPort;
        private MessageBuilder messageBuilder;
        private DispatcherTimer readMessageTimer;
        private Drink drinks = new Drink();
        private AddUserAccount addUserAccount;
        private Database db;
        private bool messageReceived;

        public bool MessageReceived
        {

            get
            {
                return messageReceived;
            }
            set
            {
                messageReceived = value;
            }
        }

        //public int BaudRate { get { return serialPort.BaudRate; } }

        //public string PortName { get { return serialPort.PortName; } }

        //Initializes the arduino to be able to connect properly
        public ArduinoConnection(string portName, int baudRate, MessageBuilder messageBuilder, AddUserAccount adduseraccount)
        {
            if (portName == null)
            {
                throw new ArgumentNullException("portName");

            }

            if (baudRate < 9600)
            {
                throw new ArgumentOutOfRangeException("baudRate");
            }

            if (messageBuilder == null)
            {
                throw new ArgumentNullException("messageBuilder");

            }
            serialPort = new SerialPort();
            serialPort.BaudRate = baudRate;
            serialPort.PortName = portName;

            this.messageBuilder = messageBuilder;
            addUserAccount = adduseraccount;
        }
        
        //Connects to the arduino and the serial port
        public void Connect()
        {
            if (!serialPort.IsOpen)
            {
                serialPort.Open();
                if (serialPort.IsOpen)
                {
                    serialPort.DiscardInBuffer();
                    serialPort.DiscardOutBuffer();
                }
            }
        }
        
        //Disconnects from the arduino and serial port
        public void Disconnect()
        {
            if (serialPort.IsOpen)
            {
                serialPort.Close();
            }
        }

        //Gets the available serialport names
        public string[] GetAvailablePortNames()
        {
            return SerialPort.GetPortNames();
        }

        //Checks if serialport is open
        public bool IsConnected()
        {
            return serialPort.IsOpen;
        }

        //Code to send message to serial port of arduino
        public bool SendMessage(string message)
        {
            if (serialPort.IsOpen)
            {
                serialPort.Write(messageBuilder.MessageBeginMarker + message + messageBuilder.MessageEndMarker);
                return true;
            }
            return false;
        }

        //Code to read message in serialport of arduino
        public string[] ReadMessages()
        {
            if (serialPort.IsOpen
                && serialPort.BytesToRead > 0)
            {
                string data = serialPort.ReadExisting();
                messageBuilder.Add(data);

                int messageCount = messageBuilder.MessageCount;
                if (messageCount > 0)
                {
                    string[] messages = new string[messageCount];
                    for (int i = 0; i < messageCount; i++)
                    {
                        messages[i] = messageBuilder.GetNextMessage();
                    }
                    return messages;
                }
            }
            return null;
        }

        //Code for starting the connection with the arduino
        public void ConnectArduino()
        {
            readMessageTimer = new DispatcherTimer();
            readMessageTimer.Interval = new TimeSpan(0, 0, 0, 0, 10);
            readMessageTimer.Tick += new EventHandler(ReadMessageTimer_Tick);
            try
            {
                Connect();
                MessageBox.Show("connection started");
                readMessageTimer.Start();
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.Message);
            }
        }

        //Code for reading the message in the serialport
        private void ReadMessageTimer_Tick(object sender, EventArgs e)
        {
            string[] messages = ReadMessages();
            if (messages != null)
            {
                foreach (string message in messages)
                {
                    processReceivedMessage(message);
                }
            }
        }

        //Code for checking what to do based on message
        private void processReceivedMessage(string message)
        {
            if (message.StartsWith("FingerID:"))
            {
                MessageReceived = true;
                addUserAccount.iudFingerprint.Text = message.Replace("FingerID:", "");
            }
            else if (message.StartsWith("Cocktail:"))
            {
                drinks.ArduinoDrinksStringForCocktail(message.Replace("Cocktail:", ""));
            }
        }

        
    }
}
