﻿using System;
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

            word = s.Recieve();
            if (word == "*")
            {
                word = ""; //cant recieve an empty byte maybe it will when i add :: when sending
            }

            GenericCounter = -1; //gg if this ever overflows :p
            int count = -1; //offset compared to word //or max of 40 parts maybe??

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
                A:

                //we look for the lowest startNr
                int lowest = startNr[0];
                int nrLowest = 0;
                for (int i = 0; i < startNr.Length; i++)
                {
                    if (lowest > startNr[i])
                    {
                        lowest = startNr[i];
                        nrLowest = i;
                    }
                }

                //Now we look if we can do anything with the corresponding data if not we wait!!
                if (lowest != int.MaxValue)
                {
                    if (status[nrLowest] == 1) //finished but not written
                    {
                        s.SendStuff(listOfHash[nrLowest].ToString());
                        listOfHash[nrLowest].Clear();
                        status[nrLowest] = 2; //written to master
                        startNr[nrLowest] = int.MaxValue;
                    }
                }


                for (int i = 0; i < TList.Length; i++)
                {
                    if (!TList[i].IsBusy)
                    {
                        int Genericnumber = -1;
                        for (int j = 0; j < listOfHash.Length; j++)
                        {
                            if (status[j] == 2)
                            {
                                Genericnumber = j;
                                break;
                            }
                        }

                        if (Genericnumber == -1)
                        {
                            Thread.Sleep(2);
                            goto A;// maybe add a delay
                        }

                        count++;
                        if (count >= 400)
                        {
                            WaitUntilDone();
                            working = false;
                            break;
                        }

                        GenericCounter++;
                        startNr[i] = GenericCounter;
                        Console.WriteLine("GenericCounter: " + GenericCounter);
                        for (int z = 0; z < startNr.Length; z++)
                        {
                            Console.WriteLine(startNr[z]);
                        }

                        object[] threadInfo = new object[3];
                        threadInfo[0] = i;
                        threadInfo[1] = word;
                        threadInfo[2] = Genericnumber;

                        //now change word to the next one 1250000/400
                        status[Genericnumber] = 0;
                        TList[i].RunWorkerAsync(threadInfo);
                        wordXFromReference x = new wordXFromReference(word, 31250);
                        word = x.DoJump();

                    }
                }
                Thread.Sleep(10);
            }
        }

        private void WaitUntilDone()
        {
            Console.WriteLine("WaituntilDone");

            B:
            int DoneCounter = 0;
            for (int i = 0; i < TList.Length; i++)
            {
                if (!TList[i].IsBusy)
                {
                    DoneCounter++;
                    Console.WriteLine(DoneCounter);
                }
            }
            if (DoneCounter != TList.Length)
            {
                goto B;
            }

            //now we have to write everything. this is double code too :D
            C:

            int lowest = startNr[0];
            int nrLowest = 0;
            for (int i = 0; i < startNr.Length; i++)
            {
                if (lowest > startNr[i])
                {
                    lowest = startNr[i];
                    nrLowest = i;
                }
            }

            if (lowest != int.MaxValue)
            {
                s.SendStuff(listOfHash[nrLowest].ToString());
                listOfHash[nrLowest].Clear();
                startNr[nrLowest] = int.MaxValue;
                goto C;
            }

            //now wait in a response from server
            Console.WriteLine("RecieveNewJob");
            //recieveJob();



        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            object[] info = e.Argument as object[];
            int nr = (int)info[2];
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
