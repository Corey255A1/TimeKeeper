using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeKeeper.Models
{
    public class TimeCard
    {
        private string _charge_number_file_path;
        private ObservableCollection<ChargeCodeTimer> _charge_codes = new ObservableCollection<ChargeCodeTimer>();
        public ObservableCollection<ChargeCodeTimer> ChargeCodes
        {
            get => _charge_codes;
        }

        public TimeCard(string initial_load_path)
        {
            _charge_number_file_path = initial_load_path;
            Initialize();
        }
        public void Initialize()
        {
            if (File.Exists(_charge_number_file_path))
            {
                var ccf = ChargeCodeFile.ReadFile(_charge_number_file_path);
                _charge_codes.Clear();
                foreach (var ccode in ccf.ChargeCode)
                {
                    _charge_codes.Add(new ChargeCodeTimer(ccode.Code, ccode.Description));
                }
            }
        }


        public void AddNewChargeCode()
        {
            _charge_codes.Add(new ChargeCodeTimer("NEWCODE", ""));
        }
        public void RemoveChargeCode(ChargeCodeTimer charge_code)
        {
            _charge_codes.Remove(charge_code);
        }


        public void Load(string path)
        {
            _charge_number_file_path = path;
            Initialize();
        }

        public void Save(string path)
        {
            var charge_code_file = new ChargeCodeFile();
            foreach (var timer in _charge_codes)
            {
                charge_code_file.ChargeCode.Add(new ChargeCode()
                {
                    Code = timer.Code,
                    Description = timer.Description
                });
            }
            charge_code_file.WriteFile(path);
        }

        public void WriteCSV(string path)
        {
            var time_dict = new Dictionary<string, ChargeCodeTimer>();
            foreach (var timer in _charge_codes)
            {
                if (timer.Code != null)
                {
                    if (!time_dict.ContainsKey(timer.Code))
                    {
                        time_dict.Add(timer.Code, timer);
                    }
                }

            }
            DateTime current_time = DateTime.Now;
            string todays_column = current_time.ToShortDateString();
            CSCSV.Table table = null;
            if (File.Exists(path))
            {
                table = CSCSV.Table.ReadFromFile(path);
            }
            else
            {
                table = new CSCSV.Table();
            }
            if (!table.ContainsHeader("Charge Codes"))
            {
                table.AddColumn("Charge Codes");
            }
            //If this table doesn't have a column for today, add one
            if (!table.ContainsHeader(todays_column))
            {
                table.AddColumn(todays_column);
            }
            List<string> chargecodes = table.GetColumn("Charge Codes").ToList();
            foreach (string code in time_dict.Keys)
            {
                int idx = chargecodes.IndexOf(code);
                //If the code is already in the list, set the time.
                if (idx >= 0)
                {
                    table.SetValue(todays_column, idx, time_dict[code].Time.ToTimeSpan().ToString());
                }
                else // We will have to add the code..
                {
                    int row_idx = table.AddRow();
                    table.SetValue("Charge Codes", row_idx, code);
                    table.SetValue(todays_column, row_idx, time_dict[code].Time.ToTimeSpan().ToString());
                }
            }
            table.WriteToFile(path);
        }


    }
}
