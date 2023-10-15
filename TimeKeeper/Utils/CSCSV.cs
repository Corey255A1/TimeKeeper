//Corey Wunderlich 2018
//A Class for reading and writing out CSV files.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
namespace CSCSV
{
    public class Table
    {
        //Key off header
        private Dictionary<string, List<string>> _table = new Dictionary<string, List<string>>();
        public int RowCount
        {
            get
            {
                if (_table.Count == 0)
                {
                    return 0;
                }

                return _table.Values.Take(1).Count();
            }
        }

        public int ColumnCount { get => _table.Keys.Count; }

        public IEnumerable<string> Headers => _table.Keys;
        public bool HasHeader { get; private set; } = true;

        private char _separator = ',';
        public char Separator { get => _separator; }

        public Table(bool hasHeader = true, char separator = ',')
        {
            HasHeader = hasHeader;
            _separator = separator;
        }

        public void AddColumn(string header)
        {
            int rowCount = RowCount;
            var column = new List<string>(rowCount);
            //Fill in each row of the new column with an empty string
            for (int rowIdx = 0; rowIdx < rowCount; rowIdx++)
            {
                column.Add("");
            }
            _table.Add(header, column);
        }
        public int AddRow(int numberOfRowsToAdd = 1)
        {
            if (numberOfRowsToAdd < 0)
            {
                return -1;
            }

            foreach (List<string> column in _table.Values)
            {
                for (int rowAddIdx = 0; rowAddIdx < numberOfRowsToAdd; rowAddIdx++)
                {
                    column.Add("");
                }
            }
            return RowCount;
        }

        public bool ContainsHeader(string header)
        {
            return _table.ContainsKey(header);
        }
        public List<string> GetColumn(string header)
        {
            if (!_table.ContainsKey(header))
            {
                return null;
            }
            return _table[header];
        }

        public bool SetValue(int columnIdx, int rowIdx, string value)
        {
            if (columnIdx < 0 || columnIdx >= ColumnCount)
            {
                return false;
            }

            var header = Headers.ElementAt(columnIdx);
            return SetValue(header, rowIdx, value);
        }
        public bool SetValue(string header, int rowIdx, string value)
        {
            if (rowIdx >= _table[header].Count)
            {
                return false;
            }

            _table[header][rowIdx] = value;
            return true;
        }
        public string GetValue(string header, int rowIdx)
        {
            if (!_table.ContainsKey(header))
            {
                return null;
            }
            if (rowIdx >= _table[header].Count)
            {
                return null;
            }

            return _table[header][rowIdx];
        }

        public void WriteToFile(string filename)
        {
            string output = "";
            var headerList = Headers.ToList();
            if (HasHeader)
            {
                int headerIdx = 0;
                for (; headerIdx < headerList.Count - 1; ++headerIdx)
                {
                    output += headerList[headerIdx] + Separator;
                }
                output += headerList[headerIdx] + "\n";
            }
            for (int rowIdx = 0; rowIdx < RowCount; ++rowIdx)
            {
                int headerIdx = 0;
                for (; headerIdx < headerList.Count - 1; ++headerIdx)
                {
                    output += _table[headerList[headerIdx]][rowIdx] + Separator;
                }
                output += _table[headerList[headerIdx]][rowIdx] + "\n";
            }
            File.WriteAllText(filename, output);
        }

        public static Table ReadFromFile(string filename, bool hasHeader = true, char valueSeparator = ',')
        {
            var table = new Table(hasHeader, valueSeparator);
            string[] lines = File.ReadAllText(filename).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int lineIdx = 0;
            if (lines.Length <= 0)
            {
                return table;
            }

            string[] headerLine = lines[0].Split(valueSeparator);
            if (table.HasHeader)
            {
                //If we have a header, use the strings as the header text
                foreach (string header in headerLine)
                {
                    table.AddColumn(header.Trim());
                }
                //Don't add this row to our data rows
                lineIdx++;
            }
            else
            {
                //else add generic numbers for the columns
                for (int i = 0; i < headerLine.Length; ++i)
                {
                    table.AddColumn(i.ToString());
                }
            }
            int columnCount = table.ColumnCount;
            for (; lineIdx < lines.Length; ++lineIdx)
            {
                string[] separatedValues = lines[lineIdx].Split(valueSeparator);
                //Only add rows that have the right amount of columns
                if (separatedValues.Length == columnCount)
                {
                    int rowIdx = table.AddRow();
                    for (int c = 0; c < columnCount; ++c)
                    {
                        table.SetValue(c, rowIdx, separatedValues[c]);
                    }
                }
            }

            return table;
        }
    }
}
