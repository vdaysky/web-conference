using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Threading;
using WebConf.screens;
using System.Windows.Controls;

namespace WebConf
{
    public partial class App : Application
    {
        
        public delegate void onBytesRead(byte[] bytes, int size);

        private static WebConferenceModel _model;

        public static WebConferenceModel Model { get {
                return _model;
            }
        }

        private SplashScreen splashScreen;
        private static MainWindow window;

        public event onBytesRead byteReadHandler;
        public static Window ActivatedWindow { get; set; }

        Socket socket;
        byte[] BUFFER = new byte[100000];
        int BUFFER_SIZE = 0;


        public App() {
            _model = new WebConferenceModel();

            splashScreen = new SplashScreen("splash.png");
            splashScreen.Show(false);
           
            new Thread(startSocketEventLoop).Start();
        }

        public void send(byte[] bytes) {
            int bytesSent = socket.Send(bytes);
        }

        public static MainWindow getWindow() {
            return window;
        }

        public static void Navigate(Page page) {
            getWindow().___MainFrame.Navigate(page);
        }

        private void onLoad() {
            Model.onAppLoad();

            splashScreen.Close(TimeSpan.FromSeconds(0));
            window = new MainWindow();
            window.Show();
            getWindow().___MainFrame.Navigate(new AuthPage());
        }


        private void startSocketEventLoop()
        {

            // Create a TCP/IP  socket.  
            socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Thread.Sleep(3000);

            // Connect the socket to the remote endpoint. Catch any errors.  
            socket.Connect("127.0.0.1", 8000);

            Current.Dispatcher.Invoke(new Action(() => {
                onLoad();
            }));

            while (true)
            {
                byte[] buff = new byte[100000]; 

                int bytesRec = socket.Receive(buff);

                if (bytesRec == 0)
                    continue;

                string end = "<EOF>";
                int start = bytesRec - end.Length;

                byte[] ending = new ArraySegment<byte>(buff, start, end.Length).ToArray();

                for (int i = 0; i < bytesRec; i++)
                {
                    BUFFER[BUFFER_SIZE++] = buff[i];
                }

                if (Encoding.ASCII.GetString(ending, 0, ending.Length) == end) {
                    byteReadHandler(BUFFER, BUFFER_SIZE - end.Length); // trigger event
                    BUFFER_SIZE = 0;
                }          
            }
        }

    }
}
