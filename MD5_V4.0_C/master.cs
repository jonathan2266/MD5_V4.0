using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;
using test;
using System.Net.Sockets;
using MD5_V2;
using System.Threading;

namespace MD5_V4._0_C
{
    public class master
    {
        private List<IPAddress> IPAdressess; //list of slaves
        private string mainDirectory; //Pure main dir
        private List<tcpMaster> listOfTCPConnections = new List<tcpMaster>();
        private List<int> PiecesWritten = new List<int>();
        private string[] arrayFileToWrite;
        private int lastFileNr;
        private wordXFromReference x;

        public master(List<IPAddress> ListOfSlaves)
        {
            IPAdressess = ListOfSlaves;
            selectMainDirectory();
            checkCurrentFiles();
        }

        private void checkCurrentFiles()
        {
            string[] index;
            DateTime[] date;
            List<string> unCompletedFiles = new List<string>();

            FolderScanner folderS = new FolderScanner(mainDirectory);
            folderS.RefreshIndex();
            folderS.getAlIndex(out index, out date);
            date = null;

            for (int i = 0; i < index.Length; i++)
            {
                char[] temp = index[i].ToCharArray();

                string isRUN = "";
                for (int j = 0; j < temp.Length; j++)
                {
                    if (temp[j] == 'R')
                    {
                        isRUN += "R";
                    }
                    if (temp[j] == 'U')
                    {
                        isRUN += "U";
                    }
                    if (temp[j] == 'N')
                    {
                        isRUN += "N";
                        if (isRUN == "RUN")
                        {
                            unCompletedFiles.Add(index[i]);
                            isRUN = "";
                            break;
                        }
                    }
                }
            }
            Console.WriteLine("found " + unCompletedFiles.Count + " unfinished files. Finishing them: " + DateTime.Now);
            finishUncompletedFiles(unCompletedFiles);
            doMainWork();
        }

        private void doMainWork()
        {
            string lastEntry;
            string firstUnhashedWord;
            getLastCompletedFile(out lastFileNr,out lastEntry);

            wordGenerator word = new wordGenerator(lastEntry);
            firstUnhashedWord = word.NewLetter();
            x = new wordXFromReference(lastEntry, 12500000);

            for (int i = 0; i < listOfTCPConnections.Count; i++)
            {
                listOfTCPConnections[i].sendData(x.DoJump());
                lastFileNr++;
                StreamWriter Swriter = new StreamWriter(mainDirectory + "\\" + lastFileNr + ".RUN.txt");
                arrayFileToWrite[i] = lastFileNr.ToString();
                PiecesWritten[i] = 0;
                Swriter.Close();
            }
            while (true)
            {

                //do i have to write stuff from who to where // event handled



                //do i have to give new orders?
                //what orders

                Thread.Sleep(5);
            }

        }

        private void getLastCompletedFile(out int lastFileNr, out string lastEntry)
        {
            string[] index;
            DateTime[] date;
            string fileName;
            lastEntry = "";

            lastFileNr = 1;

            FolderScanner folder = new FolderScanner(mainDirectory);
            folder.RefreshIndex();
            folder.getAlIndex(out index, out date);
            date = null;

            int last = index.Length - 1;
            int count = 0; //used way down
            if (last == -1)
            {
                Console.WriteLine("whoops folder was empty making file1");
                fileName = "\\1.RUN.txt";
                lastEntry = "";
            }
            else
            {
                int[] dfd = new int[index.Length];
                for (int i = 0; i < index.Length; i++)
                {
                    index[i] = index[i].Remove(0, folder.Directory1.Length + 1);
                    index[i] = index[i].Remove(index[i].Length - 4, 4);
                    dfd[i] = Convert.ToInt32(index[i]);
                }
                int biggest = 0;
                int number = 0;
                for (int i = 0; i < dfd.Length; i++)
                {
                    if (dfd[i] > number)
                    {
                        number = dfd[i];
                        biggest = i;
                    }
                }


                fileName = index[biggest];

                lastFileNr = Convert.ToInt32(fileName);

                fileName = "\\" + fileName + ".txt";

                StreamReader reader = new StreamReader(folder.Directory1 + fileName);
                lastEntry = reader.ReadLine();
                reader.Close();

            }
        }
        private void finishUncompletedFiles(List<string> filesToComplete)
        {
            connectToSlaves();

            if (filesToComplete.Count != 0)
            {
                //iets
            }
        }

        private void connectToSlaves()
        {
            for (int i = 0; i < IPAdressess.Count; i++)
            {
                try
                {
                    int port = 8001;
                    int nr = -1;
                    TcpClient client = new TcpClient(IPAdressess[i].ToString(), port);
                    NetworkStream stream = client.GetStream();
                    nr++;
                    tcpMaster connection = new tcpMaster(client, stream,nr);
                    connection.GotData += Connection_GotData;
                    listOfTCPConnections.Add(connection);
                    PiecesWritten.Add(0);
                        
                    arrayFileToWrite = new string[listOfTCPConnections.Count];

                }
                catch (SocketException e)
                {

                    Console.WriteLine("connectToSlave failed " + e);
                }
            }

        }

        private void Connection_GotData(object source, MyEventArgs e)
        {
            object[] recieved = e.GetInfo();
            int who = (int)recieved[1];
            PiecesWritten[who]++;
            if (PiecesWritten[who] == 400)
            {
                PiecesWritten[who] = 0;
                write(recieved);
                File.Move((mainDirectory + "\\" + arrayFileToWrite[(int)recieved[1]] + ".RUN.txt"), mainDirectory + "\\" + arrayFileToWrite[(int)recieved[1]] + ".txt");

                listOfTCPConnections[who].sendData(x.DoJump());
                lastFileNr++;
                StreamWriter Swriter = new StreamWriter(mainDirectory + "\\" + lastFileNr + ".RUN.txt");
                arrayFileToWrite[who] = lastFileNr.ToString();
                PiecesWritten[who] = 0;
                Swriter.Close();


            }
            else
            {
                write(recieved);
            }
        }


        private void write(object[] recieved)
        {
            StreamWriter writer = File.AppendText(mainDirectory + "\\" + arrayFileToWrite[(int)recieved[1]] + ".RUN.txt");
            writer.Write((string)recieved[0]);
            writer.Close();
        }
        private void selectMainDirectory()
        {
            Console.Clear();
            Console.WriteLine("Write the directory where you will store the txt files. Directory must be empty OR only contain files made by this program");
            string tempPath;
            tempPath = Console.ReadLine();
            if (Directory.Exists(tempPath))
            {
                mainDirectory = tempPath;
            }
            else
            {
                Console.WriteLine("Dir does not exist. Press enter to proceed");
                Console.ReadLine();
                selectMainDirectory();
            }
        }
    }
}
