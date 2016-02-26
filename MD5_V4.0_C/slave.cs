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
        private List<BackgroundWorker> TList = new List<BackgroundWorker>();
        private string word; //is the last word
        private List<StringBuilder> listOfHash = new List<StringBuilder>();
        private List<int> startNr = new List<int>();
        private List<int> status = new List<int>();  // 0 = not finished 1 = finished (creating the hash list) 2 = written to master
        private int GenericCounter;
        public slave(int port)
        {
            this.port = port;
            listenToMaster();
            GenericCounter = 0;
        }
        private void listenToMaster()
        {
            TcpListener listener = new TcpListener(IPAddress.Any,8001);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream NStream = client.GetStream();//read what is send
            listener = null;
            //string identity = "MD5_V4_SLAVE";
            //byte[] buffer = Encoding.ASCII.GetBytes(identity);
            //NStream.Write(buffer, 0, buffer.Length);

            s = new tcpSlave(client, NStream);

            recieveJob();
        }

        private void recieveJob()
        {
            listOfHash.Clear();
            word = s.Recieve();
            int threads = Environment.ProcessorCount - 1;
            GenericCounter = Environment.ProcessorCount; //gg if this ever overflows :p
            int count = 0; //offset compared to word //or max of 40 parts maybe??

            TList.Clear();
            for (int i = 0; i < threads; i++)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += Bw_DoWork;
                //bw.RunWorkerCompleted += Bw_RunWorkerCompleted;
                TList.Add(bw);
                StringBuilder stringB = new StringBuilder();
                listOfHash.Add(stringB);
                status.Add(2);
                startNr.Add(int.MaxValue);

            }

            while (true)
            {
                A:

                int lowest = startNr[0];
                int nrLowest = 0;
                for (int i = 0; i < startNr.Count; i++)
                {
                    if (lowest > startNr[i])
                    {
                        lowest = startNr[i];
                        nrLowest = i;
                    }
                }

                if (lowest != int.MaxValue)
                {
                    if (status[nrLowest] == 1) //finished but not written
                    {
                        s.SendStuff(listOfHash[nrLowest].ToString());
                        listOfHash[nrLowest].Clear();
                        startNr[nrLowest] = int.MaxValue;
                    }
                    if (status[nrLowest] == 3) //written then find next one in line
                    {
                        startNr[nrLowest] = int.MaxValue;
                        goto A;
                    }
                }


                for (int i = 0; i < TList.Count; i++)
                {
                    if (!TList[i].IsBusy)
                    {
                        count++;
                        if (count >= 400)
                        {
                            WaitUntilDone();
                        }
                        object[] threadInfo = new object[3];
                        threadInfo[0] = i;
                        threadInfo[1] = word;

                        //A:
                        int Genericnumber = -1;
                        for (int j = 0; j < listOfHash.Count; j++)
                        {
                            if (listOfHash[j].ToString() == "" && status[j] == 2)
                            {
                                Genericnumber = j;
                                break;
                            }
                        }

                        if (Genericnumber == -1)
                        {
                            goto A;// maybe ad a delay
                        }

                        threadInfo[2] = Genericnumber;

                        //now change word to the next one 1250000/400
                        TList[i].RunWorkerAsync(threadInfo);
                        wordXFromReference x = new wordXFromReference(word, 31250);
                        word = x.DoJump();

                        GenericCounter++;
                        startNr[i] = GenericCounter;
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void WaitUntilDone()
        {
            B:
            int DoneCounter = 0;
            for (int i = 0; i < TList.Count; i++)
            {
                if (!TList[i].IsBusy)
                {
                    DoneCounter++;
                }
            }
            if (DoneCounter != TList.Count)
            {
                goto B;
            }

            //now we have to write everything. this is double code too :D
            C:

            int lowest = startNr[0];
            int nrLowest = -20;
            for (int i = 0; i < startNr.Count; i++)
            {
                if (lowest > startNr[i])
                {
                    lowest = startNr[i];
                    nrLowest = i;
                }
            }

            if (lowest != -20)
            {
                s.SendStuff(listOfHash[nrLowest].ToString());
                listOfHash[nrLowest].Clear();
                startNr[nrLowest] = int.MaxValue;
                goto C;
            }

            //now wait in a response from server

            recieveJob();



        }

        //private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{

        //}

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] info = e.Argument as object[];
            int nr = (int)info[2];
            string word = (string)info[1];
            wordGenerator w = new wordGenerator(word);
            hasher h = new hasher();

            string temp;
            for (int i = 0; i < 12500000/400; i++)
            {
                temp = w.NewLetter();
                listOfHash[nr].Append(temp + Environment.NewLine);
                listOfHash[nr].Append(h.StartHash(temp) + Environment.NewLine);
            }
        }
    }
}
