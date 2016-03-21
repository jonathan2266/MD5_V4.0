using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using MD5_V2;
using System.ComponentModel;
using System.Threading;

namespace MD5_V4._0_C
{
    public class slave
    {
        private int port;
        private tcpSlave s;
        private BackgroundWorker[] TList;
        private string word; //is the last word
        private StringBuilder[] listOfHash;
        private int[] startNr;
        private int[] status;  // 0 = not finished 1 = finished (creating the hash list) 2 = written to master
        private int subCount;

        public slave(int port)
        {
            this.port = port;
            listenToMaster();
        }
        private void listenToMaster()
        {
            TcpListener listener = new TcpListener(IPAddress.Any,8001);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream NStream = client.GetStream();//read what is send
            listener = null;

            s = new tcpSlave(client, NStream);

            while (true)
            {
                recieveJob();
            }
            
        }

        private void recieveJob()
        {
            bool working = true;
            int threads = Environment.ProcessorCount - 1;
            listOfHash = new StringBuilder[threads];
            TList = new BackgroundWorker[threads];
            startNr = new int[threads];
            status = new int[threads];
            subCount = 0;

            word = s.Recieve();
            if (word == "*")
            {
                word = ""; //cant recieve an empty byte maybe it will when i add :: when sending
            }

            for (int i = 0; i < threads; i++)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Bw_DoWork;
                TList[i] = bw;
                StringBuilder stringB = new StringBuilder();
                listOfHash[i] = stringB;
                status[i] = 2;
                startNr[i] = int.MaxValue;

            }

            while (working)
            {

                for (int i = 0; i < TList.Length; i++)
                {
                    if (!TList[i].IsBusy)
                    {
                        //can we give it a new job? or do we have to wait? (statuscheck)
                        if (status[i] == 2 && subCount < 400) //meaning the thread is free so give it a job
                        {
                            startSub(i);
                        }
                        if (status[i] == 1) //meaning it if finished but can it be writen? everything has to be send in order
                        {
                            writeSub(i);
                        }
                    }
                }
                if (subCount >= 400)
                {
                    int emptyCount = 0;
                    for (int i = 0; i < status.Length; i++)
                    {
                        if (status[i] == 2)
                        {
                            emptyCount++;
                        }
                    }
                    if (emptyCount == threads)
                    {
                        working = false;
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void writeSub(int nr)
        {
            //need to see if the lowest startNr is the same is the int recieved from method
            int lowestIndex = 0;
            for (int i = 0; i < startNr.Length; i++)
            {
                if (startNr[i] < startNr[lowestIndex])
                {
                    lowestIndex = i;
                }
            }

            if (lowestIndex == nr) //can write
            {
                s.SendStuff(listOfHash[nr].ToString());
                listOfHash[nr].Clear();
                startNr[nr] = int.MaxValue;
                status[nr] = 2;
            }
        }

        private void startSub(int nr)
        {
            status[nr] = 0;
            startNr[nr] = subCount;

            object[] threadInfo = new object[2];
            threadInfo[0] = nr;
            threadInfo[1] = word;
            TList[nr].RunWorkerAsync(threadInfo);

            wordXFromReference x = new wordXFromReference(word, 31250);
            word = x.DoJump();
            subCount++; //until 400
            
        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] info = e.Argument as object[];
            int nr = (int)info[0];
            string word = (string)info[1];
            if (word == null)
            {
                word = "";
            }
            wordGenerator w = new wordGenerator(word);
            hasher h = new hasher();
            int amount = 12500000 / 400;

            string temp;
            for (int i = 0; i < amount; i++)
            {
                temp = w.NewLetter();
                listOfHash[nr].Append(temp + Environment.NewLine);
                listOfHash[nr].Append(h.StartHash(temp) + Environment.NewLine);
            }
            listOfHash[nr].Clear();
            listOfHash[nr].Append(startNr[nr] + Environment.NewLine);
            status[nr] = 1;
        }
    }
}
