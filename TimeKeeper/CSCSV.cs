//Corey Wunderlich 2018
//A Class for reading and writing out CSV files.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
namespace CSCSV
{
    public class Table
    {
        //Key off header
        Dictionary<string, string[]> theTable = new Dictionary<string, string[]>();
        List<string> theHeaderList = new List<string>();
        int rowCount = 2;
        bool HasHeader = true;
        char Seperator = ',';
        public Table(bool h=true, char sep=',')
        {
            HasHeader = h;
            Seperator = sep;
        }
        public int RowCount
        {
            get { return rowCount; }
            set {
                if (value < 0) return;
                if (theHeaderList.Count > 0)
                {
                    if (value > rowCount)
                    {
                        foreach (string h in theHeaderList)
                        {
                            string[] sa = theTable[h];
                            Array.Resize(ref sa, value + 10);
                            theTable[h] = sa;
                        }
                    }
                    else if (value < rowCount)
                    {
                        foreach (string h in theHeaderList)
                        {
                            string[] sa = theTable[h];
                            Array.Resize(ref sa, value);
                            theTable[h] = sa;
                        }
                    }
                }
                rowCount = value;

            }
        }
        public void AddColumn(string header)
        {
            theTable.Add(header, new string[rowCount]);
            theHeaderList.Add(header);
        }
        public IEnumerable<string> Headers()
        {
            return theHeaderList.AsEnumerable();
        }
        public bool ContainsHeader(string h)
        {
            return theTable.ContainsKey(h);
        }
        public string[] GetColumn(string h)
        {
            if(theTable.ContainsKey(h))
            {
                return theTable[h];
            }
            return null;
        }
        public bool SetValue(int c, int r, string value)
        {
            if(theHeaderList.Count>c)
            {
                var header = theHeaderList[c];
                return SetValue(header, r, value);
            }
            return false;
            
        }
        public bool SetValue(string header, int r, string value)
        {
            if (theTable[header].Length > r)
            {
                theTable[header][r] = value;
                return true;
            }
            return false;
        }
        public string GetValue(string header, int row)
        {
            if(theTable.ContainsKey(header))
            {
                if(theTable[header].Length>row)
                {
                    return theTable[header][row];
                }
            }
            return null;            
        }

        public void WriteToFile(string filename)
        {
            string output = "";
            if(HasHeader)
            {
                int h = 0;
                for (; h < theHeaderList.Count - 1; ++h)
                {
                    output += theHeaderList[h] + Seperator;
                }
                output += theHeaderList[h] + "\n";
            }
            int rows = RowCount;
            for(int r =0; r<rows;++r)
            {
                int h = 0;
                for (; h < theHeaderList.Count - 1; ++h)
                {
                    output += theTable[theHeaderList[h]][r] + Seperator;
                }
                output += theTable[theHeaderList[h]][r] + "\n";
            }
            File.WriteAllText(filename, output);
        }


        public static Table LoadFromFile(string filename, bool header = true, char seperator = ',')
        {
            var table = new Table(header,seperator);
            string[] lines = File.ReadAllText(filename).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            table.RowCount = lines.Length;
            if (header) table.RowCount -= 1;
            string[] firstLine = lines[0].Split(seperator);
            int columnCount = firstLine.Length;
            int l = 0;
            if (header)
            {
                foreach(string h in firstLine)
                {
                    table.AddColumn(h.Trim());
                }
                l = 1;
            }
            else
            {
                for(int i=0; i<columnCount; ++i)
                {
                    table.AddColumn(i.ToString());
                }
            }
            int r = 0;
            for(;l<lines.Length;++l)
            {
                string[] vals = lines[l].Split(seperator);
                if (vals.Length == columnCount)
                {
                    for (int c = 0; c < columnCount; ++c)
                    {
                        table.SetValue(c, r, vals[c]);
                    }
                    ++r;
                }
            }

            return table;
            

        }
    }
}
