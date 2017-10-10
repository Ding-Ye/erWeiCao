using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;



namespace AdaMgr
{
    class SmplPBForm
    {
        //public string text1 = System.IO.File.ReadAllText(@"C:\Users\admin\Desktop\text1.txt");
        public int returnval;
        public string st1;
        GetDataReturn returnval1;
        //Opens a Comport or IP Port depending on the option seleceted
        public string open()
        {
           
            returnval = 99;
            returnval = SimplePB.OpenPort(4, 9600);

           //string strData = text1;//存储来自cr1000的数据
           string strData = String.Empty;//存储来自cr1000的数据
          // returnval1 = 0;
           returnval1 = SimplePB.GetData(1, DeviceTypeCodes.CR1000, 6, -1, ref strData);
            if (returnval1 == 0)
            {
                st1 = strData;
                return st1;
                
            }
            else if (returnval1 > 0)
            {
                st1 = strData;
                // return st1;
                do
                {
                    returnval1 = SimplePB.GetData(1, DeviceTypeCodes.CR1000, 6, -1, ref strData);
                    if (returnval1 < 0)
                    {
                        break;
                    }
                    else
                        st1 = strData;
                   // return st1;
                } while (returnval1 > 0);

                return st1;
            }
    
            return st1;
        }
        
        public string[] test = null;
        /// <summary>
        /// 对字符进行切割
        /// </summary>
        /// <param name="st1"></param>
        /// <returns></returns>
        public string[] cut(string st1) 
        {
            string[] myWords;
            string myString = st1;
            if (st1 != "0")
            {


                myWords = myString.Split(new Char[] { ',', '\r', '\n' });
                //12-60是含水率的数值隔四个取一次(含水率-温度-基质势)
                string[] test = { myWords[17], myWords[25], myWords[33], myWords[41], myWords[49], myWords[57], myWords[65], myWords[73], myWords[81], myWords[89], myWords[97], myWords[105], myWords[113], 
                                   myWords[117], myWords[125], myWords[133], myWords[141], myWords[149], myWords[157], myWords[165], myWords[173], myWords[181], myWords[189], myWords[197], myWords[205], myWords[213]
                                    ,myWords[121], myWords[129], myWords[137], myWords[145], myWords[153], myWords[161], myWords[169], myWords[177], myWords[185], myWords[193], myWords[201], myWords[209], myWords[217]};
                for (int i = 0; i < test.Length; i++)
                {
                    if (test[i] == "\"NAN\"" || test[i] == "0")
                    {
                        test[i] = "00.00";
                    }
                }
                Console.ReadLine();
                return test;
            }
            else
            {
                return test;
            }
         
        }
    }
}


                    
    
