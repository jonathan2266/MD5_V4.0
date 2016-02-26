using System;
using System.Collections.Generic;
using System.Net;

#region //stuff

// network hasherV4.0 Date: 12-2-16
//changelog: none
//things to keep in check: 

//standard port is 8001
// Dojump method could auto update the reference word. in wordgen -> wordXFromReference

//finsish the slave route in Program.cs //slave

//improvements cuz lazy
//in master->selectMainDirectory can be improved
//slave autodetect with UDP broadcast
#endregion
namespace MD5_V4._0_C
{
    class Program
    {
        static void Main(string[] args)
        {
            string answer;

            jumpToFirstLine: //goto jump

            Console.WriteLine("define this service by either typing master or slave");
            answer = Console.ReadLine();
            if (answer == "master")
            {
                List<IPAddress> IPaddresses = new List<IPAddress>();
                Console.WriteLine("Type in the IP adresses of all the hosts 1 at a time. make no mistake :p");

                jump2: //jumps when you wanne add another IP

                Console.WriteLine("slave IP?");
                answer = Console.ReadLine();

                try
                {
                    IPaddresses.Add(IPAddress.Parse(answer));
                }
                catch (FormatException)
                {

                    Console.WriteLine("Not a valid IP");
                    goto jump;
                }

                jump: //jump when you made a typing mistake;

                Console.WriteLine("wanne add anothe IP? y or n");
                answer = Console.ReadLine();

                if (answer == "y")
                {
                    goto jump2;
                }
                else if(answer == "n")
                {
                    master m = new master(IPaddresses);
                }
                else
                {
                    Console.WriteLine("you wrote " + answer);
                    goto jump;
                }
            }
            else if (answer == "slave")
            {
                slave s = new slave(8001);
            }
            else
            {
                Console.WriteLine("wrong input can either be master or slave and not: " + answer);
                goto jumpToFirstLine;
            }
        }
    }
}
