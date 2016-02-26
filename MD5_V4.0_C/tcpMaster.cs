using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace MD5_V4._0_C
{

    public delegate void MyEventHandler(object source, MyEventArgs e);
    public class tcpMaster
    {
        private TcpClient client;
        private NetworkStream stream;
        private bool run = true;
        private byte[] data;
        private string responseData;
        private int sizeOfbyte = 100000000;
        public event MyEventHandler GotData;
        private int nr;
        
        public tcpMaster(TcpClient client, NetworkStream stream,int nr)
        {
            this.client = client;
            this.stream = stream;
            this.nr = nr;
            BackgroundWorker worker = new BackgroundWorker();
            worker.DoWork += Worker_DoWork; //check if it recieves stuff //cancelasync
            worker.RunWorkerAsync();
        }

        public void sendData(string data)
        {
            byte[] bdata = Encoding.ASCII.GetBytes(data);
            stream.Write(bdata, 0, bdata.Length);
        }
        private void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            List<char> recieved = new List<char>();
            data = new byte[sizeOfbyte];

            while (run)
            {
                try
                {
                    if (stream.DataAvailable)
                    {
                        int nrbytes = stream.Read(data, 0, data.Length);
                        responseData = Encoding.ASCII.GetString(data, 0, nrbytes);
                        sizeOfbyte = nrbytes + 100;
                        data = new byte[sizeOfbyte];
                        if (GotData != null)
                        {
                            GotData(this, new MyEventArgs(recieved,nr));

                        }
                        //raise event OR let it poll
                    }
                }
                catch (SocketException)
                {
                    throw;
                }

                Thread.Sleep(1);
            }
        }
    }

    public class MyEventArgs : EventArgs
    {
        private object[] EventInfo = new object[2];
        public MyEventArgs(List<char> recieved, int nr)
        {
            EventInfo[0] = recieved;
            EventInfo[1] = nr;
        }
        public object[] GetInfo()
        {
            return EventInfo;
        }
    }
}
