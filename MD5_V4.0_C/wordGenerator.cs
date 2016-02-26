using System;
using System.Collections.Generic;
using System.Text;

namespace MD5_V2
{
    public class wordGenerator
    {
        private List<int> listOfNumbers = new List<int>();
        string text;
        linker l = new linker();

        public wordGenerator(string lastEntry)
        {
            if (lastEntry == "")
            {
                listOfNumbers.Add(0);
            }
            else
            {
                convertToNumbers(lastEntry);

            }
            
        }
        public string GetText { get { return text; } }

        public string NewLetter()
        {
            bool addLetter = false;
            bool changed = false;
            for (int i = 0; i < listOfNumbers.Count; i++)
            {
                if (listOfNumbers[0] < 63 && i == 0)
                {
                    listOfNumbers[0]++;
                    changed = true;
                }
                if (listOfNumbers[0] == 63 && i < listOfNumbers.Count - 1 && i == 0 && changed == false)
                {
                    listOfNumbers[0] = 1;
                    listOfNumbers[1]++;
                    changed = true;
                }
                if (listOfNumbers[i] == 63 && i == listOfNumbers.Count - 1 && changed == false)
                {
                    listOfNumbers[i] = 1;
                    addLetter = true;
                    changed = true;
                }

                if (listOfNumbers[i] == 64 && i < listOfNumbers.Count - 1 && changed == false)
                {
                    listOfNumbers[i] = 1;
                    listOfNumbers[i + 1]++;
                    changed = true;
                }
                if (listOfNumbers[i] == 64 && i == listOfNumbers.Count - 1 && changed == false)
                {
                    listOfNumbers[i] = 1;
                    addLetter = true;
                    changed = true;
                }
                changed = false;
            }
            if (addLetter == true)
            {
                listOfNumbers.Insert(0, 1);
            }
            transLateToLetters();
            return text;
        }

        private void transLateToLetters()
        {
            int[] reversed = new int[listOfNumbers.Count];
            StringBuilder builder = new StringBuilder();
            listOfNumbers.CopyTo(reversed);
            Array.Reverse(reversed);

            for (int i = 0; i < reversed.Length; i++)
            {
                builder.Append(l.NumberToString(reversed[i]));
                #region //switch turned off
                //switch (reversed[i])
                //{
                //    case 1:
                //        builder.Append("a");
                //        break;
                //    case 2:
                //        builder.Append("b");
                //        break;
                //    case 3:
                //        builder.Append("c");
                //        break;
                //    case 4:
                //        builder.Append("d");
                //        break;
                //    case 5:
                //        builder.Append("e");
                //        break;
                //    case 6:
                //        builder.Append("f");
                //        break;
                //    case 7:
                //        builder.Append("g");
                //        break;
                //    case 8:
                //        builder.Append("h");
                //        break;
                //    case 9:
                //        builder.Append("i");
                //        break;
                //    case 10:
                //        builder.Append("j");
                //        break;
                //    case 11:
                //        builder.Append("k");
                //        break;
                //    case 12:
                //        builder.Append("l");
                //        break;
                //    case 13:
                //        builder.Append("m");
                //        break;
                //    case 14:
                //        builder.Append("n");
                //        break;
                //    case 15:
                //        builder.Append("o");
                //        break;
                //    case 16:
                //        builder.Append("p");
                //        break;
                //    case 17:
                //        builder.Append("q");
                //        break;
                //    case 18:
                //        builder.Append("r");
                //        break;
                //    case 19:
                //        builder.Append("s");
                //        break;
                //    case 20:
                //        builder.Append("t");
                //        break;
                //    case 21:
                //        builder.Append("u");
                //        break;
                //    case 22:
                //        builder.Append("v");
                //        break;
                //    case 23:
                //        builder.Append("w");
                //        break;
                //    case 24:
                //        builder.Append("x");
                //        break;
                //    case 25:
                //        builder.Append("y");
                //        break;
                //    case 26:
                //        builder.Append("z");
                //        break;
                //    case 27:
                //        builder.Append("A");
                //        break;
                //    case 28:
                //        builder.Append("B");
                //        break;
                //    case 29:
                //        builder.Append("C");
                //        break;
                //    case 30:
                //        builder.Append("D");
                //        break;
                //    case 31:
                //        builder.Append("E");
                //        break;
                //    case 32:
                //        builder.Append("F");
                //        break;
                //    case 33:
                //        builder.Append("G");
                //        break;
                //    case 34:
                //        builder.Append("H");
                //        break;
                //    case 35:
                //        builder.Append("I");
                //        break;
                //    case 36:
                //        builder.Append("J");
                //        break;
                //    case 37:
                //        builder.Append("K");
                //        break;
                //    case 38:
                //        builder.Append("L");
                //        break;
                //    case 39:
                //        builder.Append("M");
                //        break;
                //    case 40:
                //        builder.Append("N");
                //        break;
                //    case 41:
                //        builder.Append("O");
                //        break;
                //    case 42:
                //        builder.Append("P");
                //        break;
                //    case 43:
                //        builder.Append("Q");
                //        break;
                //    case 44:
                //        builder.Append("R");
                //        break;
                //    case 45:
                //        builder.Append("S");
                //        break;
                //    case 46:
                //        builder.Append("T");
                //        break;
                //    case 47:
                //        builder.Append("U");
                //        break;
                //    case 48:
                //        builder.Append("V");
                //        break;
                //    case 49:
                //        builder.Append("W");
                //        break;
                //    case 50:
                //        builder.Append("X");
                //        break;
                //    case 51:
                //        builder.Append("Y");
                //        break;
                //    case 52:
                //        builder.Append("Z");
                //        break;
                //    case 53:
                //        builder.Append(" ");
                //        break;
                //    case 54:
                //        builder.Append("0");
                //        break;
                //    case 55:
                //        builder.Append("1");
                //        break;
                //    case 56:
                //        builder.Append("2");
                //        break;
                //    case 57:
                //        builder.Append("3");
                //        break;
                //    case 58:
                //        builder.Append("4");
                //        break;
                //    case 59:
                //        builder.Append("5");
                //        break;
                //    case 60:
                //        builder.Append("6");
                //        break;
                //    case 61:
                //        builder.Append("7");
                //        break;
                //    case 62:
                //        builder.Append("8");
                //        break;
                //    case 63:
                //        builder.Append("9");
                //        break;
                //    default:
                //        break;
                //}
                #endregion turned off
            }
            text = Convert.ToString(builder);
        }

        private void convertToNumbers(string lastEntry)
        {
            char[] entry;
            entry = lastEntry.ToCharArray();
            for (int i = 0; i < entry.Length; i++)
            {
                listOfNumbers.Add(l.StringToNumber(entry[i]));
                //listOfNumbers.Add(index(entry[i]));
                
            }
            listOfNumbers.Reverse();
        }

        //private int index(char v)
        //{
        //    int temp = 0;
        //    #region //switch
        //    switch (v)
        //    {
        //        case 'a':
        //            temp = 1;
        //            break;
        //        case 'b':
        //            temp = 2;
        //            break;
        //        case 'c':
        //            temp =3;
        //            break;
        //        case 'd':
        //            temp =4;
        //            break;
        //        case 'e':
        //            temp =5;
        //            break;
        //        case 'f':
        //            temp =6;
        //            break;
        //        case 'g':
        //            temp =7;
        //            break;
        //        case 'h':
        //            temp =8;
        //            break;
        //        case 'i':
        //            temp = 9;
        //            break;
        //        case 'j':
        //            temp =10;
        //            break;
        //        case 'k':
        //            temp =11;
        //            break;
        //        case 'l':
        //            temp = 12;
        //            break;
        //        case 'm':
        //            temp = 13;
        //            break;
        //        case 'n':
        //            temp = 14;
        //            break;
        //        case 'o':
        //            temp = 15;
        //            break;
        //        case 'p':
        //            temp = 16;
        //            break;
        //        case 'q':
        //            temp = 17;
        //            break;
        //        case 'r':
        //            temp =18;
        //            break;
        //        case 's':
        //            temp = 19;
        //            break;
        //        case 't':
        //            temp =20;
        //            break;
        //        case 'u':
        //            temp =21;
        //            break;
        //        case 'v':
        //            temp =22;
        //            break;
        //        case 'w':
        //            temp =23;
        //            break;
        //        case 'x':
        //            temp =24;
        //            break;
        //        case 'y':
        //            temp =25;
        //            break;
        //        case 'z':
        //            temp =26;
        //            break;
        //        case 'A':
        //            temp =27;
        //            break;
        //        case 'B':
        //            temp =28;
        //            break;
        //        case 'C':
        //            temp =29;
        //            break;
        //        case 'D':
        //            temp =30;
        //            break;
        //        case 'E':
        //            temp =31;
        //            break;
        //        case 'F':
        //            temp =32;
        //            break;
        //        case 'G':
        //            temp =33;
        //            break;
        //        case 'H':
        //            temp =34;
        //            break;
        //        case 'I':
        //            temp =35;
        //            break;
        //        case 'J':
        //            temp =36;
        //            break;
        //        case 'K':
        //            temp =37;
        //            break;
        //        case 'L':
        //            temp =38;
        //            break;
        //        case 'M':
        //            temp =39;
        //            break;
        //        case 'N':
        //            temp =40;
        //            break;
        //        case 'O':
        //            temp =41;
        //            break;
        //        case 'P':
        //            temp =42;
        //            break;
        //        case 'Q':
        //            temp =43;
        //            break;
        //        case 'R':
        //            temp =44;
        //            break;
        //        case 'S':
        //            temp =45;
        //            break;
        //        case 'T':
        //            temp =46;
        //            break;
        //        case 'U':
        //            temp =47;
        //            break;
        //        case 'V':
        //            temp =48;
        //            break;
        //        case 'W':
        //            temp =49;
        //            break;
        //        case 'X':
        //            temp =50;
        //            break;
        //        case 'Y':
        //            temp =51;
        //            break;
        //        case 'Z':
        //            temp =52;
        //            break;
        //        case ' ':
        //            temp =53;
        //            break;
        //        case '0':
        //            temp =54;
        //            break;
        //        case '1':
        //            temp =55;
        //            break;
        //        case '2':
        //            temp =56;
        //            break;
        //        case '3':
        //            temp =57;
        //            break;
        //        case '4':
        //            temp =58;
        //            break;
        //        case '5':
        //            temp =59;
        //            break;
        //        case '6':
        //            temp =60;
        //            break;
        //        case '7':
        //            temp =61;
        //            break;
        //        case '8':
        //            temp =62;
        //            break;
        //        case '9':
        //            temp =63;
        //            break;
        //        default:
        //            temp = 1;
        //            break;
        //    }
        //    #endregion
        //    return temp;
        //}
    }

    public class wordXFromReference
    {
        private string referenceWord;
        private int amountOfJumps;
        linker l = new linker();

        public wordXFromReference(string referenceWord, int amountOfJumps)
        {
            this.referenceWord = referenceWord;
            this.amountOfJumps = amountOfJumps;
        }

        public string UpdateReferenceWord { get { return referenceWord; } set { referenceWord = value; } }
        public string DoJump()
        {
            string word = "";
            char[] charReferenceWord = referenceWord.ToCharArray();
            int[] intForm = new int[charReferenceWord.Length];
            int solidIntForm = 0;

            for (int i = 0; i < charReferenceWord.Length; i++)
            {
                intForm[i] = l.StringToNumber(charReferenceWord[i]);
            }

            solidIntForm = intArrayToSolidInt(intForm);
            solidIntForm += amountOfJumps;
            intForm = SolidToIntArray(solidIntForm);

            StringBuilder a = new StringBuilder();

            for (int i = 0; i < intForm.Length; i++)
            {
                a.Append(l.NumberToString(intForm[i]));
            }

            word = a.ToString();
            referenceWord = word;
            return word;
        }

        private int[] SolidToIntArray(int solid)
        {
            int[] value;
            int power = 0;
            int check;
            bool run = true;

            while (run)
            {
                check = solid - (int)(Math.Pow(l.IndexLenght, power));
                if (check > 0)
                {
                    power++;
                }
                if (check == 0)
                {
                    break;
                }
                if (check < 0)
                {
                    power--;
                    break;
                }
            }

            List<int> tempList = new List<int>();
            int loopCount = 0;

            while (true)
            {
                check = solid - (int)(Math.Pow(l.IndexLenght, power));
                if (check > 0)
                {
                    solid = check;
                    loopCount++;
                }
                if (check < 0)
                {
                    power--;
                    tempList.Add(loopCount);
                    loopCount = 0;
                }
                if (check == 0)
                {
                    tempList.Add(loopCount);
                    break;
                }
            }

            value = tempList.ToArray();
            return value;
        }
        private int intArrayToSolidInt(int[] array)
        {
            int lenght = array.Length;
            int indexLenght = l.IndexLenght;
            int solid = 0;

            for (int i = 0; i < array.Length; i++)
            {
                lenght--;
                solid += array[i] * (int)(Math.Pow(indexLenght, lenght));
            }

            return solid;
        }
    }

    class linker
    {
        private int indexLenght = 63;
        public int IndexLenght { get { return indexLenght; } }
        public string NumberToString(int i)
        {
            switch (i)
            {
                case 1:
                    return("a");

                case 2:
                    return("b");

                case 3:
                    return("c");

                case 4:
                    return("d");

                case 5:
                    return("e");

                case 6:
                    return("f");
                    
                case 7:
                    return("g");
                    
                case 8:
                    return("h");
                    
                case 9:
                    return("i");
                    
                case 10:
                    return("j");
                    
                case 11:
                    return("k");
                    
                case 12:
                    return("l");
                    
                case 13:
                    return("m");
                    
                case 14:
                    return("n");
                    
                case 15:
                    return("o");
                    
                case 16:
                    return("p");
                    
                case 17:
                    return("q");
                    
                case 18:
                    return("r");
                    
                case 19:
                    return("s");
                    
                case 20:
                    return("t");
                    
                case 21:
                    return("u");
                    
                case 22:
                    return("v");
                    
                case 23:
                    return("w");
                    
                case 24:
                    return("x");
                    
                case 25:
                    return("y");
                    
                case 26:
                    return("z");
                    
                case 27:
                    return("A");
                    
                case 28:
                    return("B");
                    
                case 29:
                    return("C");
                    
                case 30:
                    return("D");
                    
                case 31:
                    return("E");
                    
                case 32:
                    return("F");
                    
                case 33:
                    return("G");
                    
                case 34:
                    return("H");
                    
                case 35:
                    return("I");
                    
                case 36:
                    return("J");
                    
                case 37:
                    return("K");
                    
                case 38:
                    return("L");
                    
                case 39:
                    return("M");
                    
                case 40:
                    return("N");
                    
                case 41:
                    return("O");
                    
                case 42:
                    return("P");
                    
                case 43:
                    return("Q");
                    
                case 44:
                    return("R");
                    
                case 45:
                    return("S");
                    
                case 46:
                    return("T");
                    
                case 47:
                    return("U");
                    
                case 48:
                    return("V");
                    
                case 49:
                    return("W");
                    
                case 50:
                    return("X");
                    
                case 51:
                    return("Y");
                    
                case 52:
                    return("Z");
                    
                case 53:
                    return(" ");
                    
                case 54:
                    return("0");
                    
                case 55:
                    return("1");
                    
                case 56:
                    return("2");
                    
                case 57:
                    return("3");
                    
                case 58:
                    return("4");
                    
                case 59:
                    return("5");
                    
                case 60:
                    return("6");
                    
                case 61:
                    return("7");
                    
                case 62:
                    return("8");
                    
                case 63:
                    return("9");
                    
                default:
                    return ("");
            }
        }
		public int StringToNumber(char letter){

            int temp = 1;
			
			switch (letter)
            {
                case 'a':
                    temp = 1;
                    break;
                case 'b':
                    temp = 2;
                    break;
                case 'c':
                    temp =3;
                    break;
                case 'd':
                    temp =4;
                    break;
                case 'e':
                    temp =5;
                    break;
                case 'f':
                    temp =6;
                    break;
                case 'g':
                    temp =7;
                    break;
                case 'h':
                    temp =8;
                    break;
                case 'i':
                    temp = 9;
                    break;
                case 'j':
                    temp =10;
                    break;
                case 'k':
                    temp =11;
                    break;
                case 'l':
                    temp = 12;
                    break;
                case 'm':
                    temp = 13;
                    break;
                case 'n':
                    temp = 14;
                    break;
                case 'o':
                    temp = 15;
                    break;
                case 'p':
                    temp = 16;
                    break;
                case 'q':
                    temp = 17;
                    break;
                case 'r':
                    temp =18;
                    break;
                case 's':
                    temp = 19;
                    break;
                case 't':
                    temp =20;
                    break;
                case 'u':
                    temp =21;
                    break;
                case 'v':
                    temp =22;
                    break;
                case 'w':
                    temp =23;
                    break;
                case 'x':
                    temp =24;
                    break;
                case 'y':
                    temp =25;
                    break;
                case 'z':
                    temp =26;
                    break;
                case 'A':
                    temp =27;
                    break;
                case 'B':
                    temp =28;
                    break;
                case 'C':
                    temp =29;
                    break;
                case 'D':
                    temp =30;
                    break;
                case 'E':
                    temp =31;
                    break;
                case 'F':
                    temp =32;
                    break;
                case 'G':
                    temp =33;
                    break;
                case 'H':
                    temp =34;
                    break;
                case 'I':
                    temp =35;
                    break;
                case 'J':
                    temp =36;
                    break;
                case 'K':
                    temp =37;
                    break;
                case 'L':
                    temp =38;
                    break;
                case 'M':
                    temp =39;
                    break;
                case 'N':
                    temp =40;
                    break;
                case 'O':
                    temp =41;
                    break;
                case 'P':
                    temp =42;
                    break;
                case 'Q':
                    temp =43;
                    break;
                case 'R':
                    temp =44;
                    break;
                case 'S':
                    temp =45;
                    break;
                case 'T':
                    temp =46;
                    break;
                case 'U':
                    temp =47;
                    break;
                case 'V':
                    temp =48;
                    break;
                case 'W':
                    temp =49;
                    break;
                case 'X':
                    temp =50;
                    break;
                case 'Y':
                    temp =51;
                    break;
                case 'Z':
                    temp =52;
                    break;
                case ' ':
                    temp =53;
                    break;
                case '0':
                    temp =54;
                    break;
                case '1':
                    temp =55;
                    break;
                case '2':
                    temp =56;
                    break;
                case '3':
                    temp =57;
                    break;
                case '4':
                    temp =58;
                    break;
                case '5':
                    temp =59;
                    break;
                case '6':
                    temp =60;
                    break;
                case '7':
                    temp =61;
                    break;
                case '8':
                    temp =62;
                    break;
                case '9':
                    temp =63;
                    break;
                default:
                    temp = 1;
                    break;
            }
			return temp;
		}
    }
}
