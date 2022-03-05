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
        private List<string> _header_list = new List<string>();
        private int _row_count = 0;
        public int RowCount { get => _row_count;  }
        public int ColumnCount { get => _table.Keys.Count; }
        public bool HasHeader { get; private set; } = true;
        private char _separator = ',';
        public char Separator { get => _separator; }
        public Table(bool has_header = true, char separator = ',')
        {
            HasHeader = has_header;
            _separator = separator;
        }

        public void AddColumn(string header)
        {
            var column = new List<string>(_row_count);
            //Fill in each row of the new column with an empty string
            for(int r = 0; r < _row_count; r++) { column.Add(""); }
            _table.Add(header, column);
            _header_list.Add(header);
        }
        public int AddRow(int count = 1)
        {
            if (count > 0)
            {
                foreach (List<string> column in _table.Values)
                {
                    for (int c = 0; c < count; c++) { column.Add(""); }
                }
                _row_count += count;
                //return the row index added
                return _row_count - 1;
            }
            return -1;
        }



        public IEnumerable<string> Headers()
        {
            return _header_list.AsEnumerable();
        }
        public bool ContainsHeader(string header)
        {
            return _table.ContainsKey(header);
        }
        public List<string> GetColumn(string header)
        {
            if (_table.ContainsKey(header))
            {
                return _table[header];
            }
            return null;
        }
       
        public bool SetValue(int column_idx, int row_idx, string value)
        {
            if (_header_list.Count > column_idx)
            {
                var header = _header_list[column_idx];
                return SetValue(header, row_idx, value);
            }
            return false;

        }
        public bool SetValue(string header, int row_idx, string value)
        {
            if (_table[header].Count > row_idx)
            {
                _table[header][row_idx] = value;
                return true;
            }
            return false;
        }
        public string GetValue(string header, int row_idx)
        {
            if (_table.ContainsKey(header))
            {
                if (_table[header].Count > row_idx)
                {
                    return _table[header][row_idx];
                }
            }
            return null;
        }

        public void WriteToFile(string filename)
        {
            string output = "";
            if (HasHeader)
            {
                int h = 0;
                for (; h < _header_list.Count - 1; ++h)
                {
                    output += _header_list[h] + Separator;
                }
                output += _header_list[h] + "\n";
            }
            for (int r = 0; r < _row_count; ++r)
            {
                int h = 0;
                for (; h < _header_list.Count - 1; ++h)
                {
                    output += _table[_header_list[h]][r] + Separator;
                }
                output += _table[_header_list[h]][r] + "\n";
            }
            File.WriteAllText(filename, output);
        }

        public static Table ReadFromFile(string filename, bool has_header = true, char seperator = ',')
        {
            var table = new Table(has_header, seperator);
            string[] lines = File.ReadAllText(filename).Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            int line_index = 0;
            if (lines.Length > 0)
            {
                string[] header_line = lines[0].Split(seperator);
                if (table.HasHeader)
                {
                    //If we have a header, use the strings as the header text
                    foreach (string header in header_line)
                    {
                        table.AddColumn(header.Trim());
                    }
                    //Don't add this row to our data rows
                    line_index++;
                }
                else
                {
                    //else add generic numbers for the columns
                    for (int i = 0; i < header_line.Length; ++i)
                    {
                        table.AddColumn(i.ToString());
                    }
                }
                int column_count = table.ColumnCount;
                for (; line_index < lines.Length; ++line_index)
                {
                    string[] vals = lines[line_index].Split(seperator);
                    //Only add rows that have the right amount of columns
                    if (vals.Length == column_count)
                    {
                        int row_idx = table.AddRow();
                        for (int c = 0; c < column_count; ++c)
                        {
                            table.SetValue(c, row_idx, vals[c]);
                        }
                    }
                }
            }
            return table;
        }
    }
}
