//Corey Wunderlich WunderVision 2022
//Reading and writing the TimeCard charge codes
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace TimeKeeper.Models
{
    public class TimeCard
    {
        private string _chargeNumberFilePath;

        private ObservableCollection<ChargeCodeTimer> _chargeCodes = new ObservableCollection<ChargeCodeTimer>();
        public ObservableCollection<ChargeCodeTimer> ChargeCodes
        {
            get => _chargeCodes;
        }

        public TimeCard(string initial_load_path)
        {
            _chargeNumberFilePath = initial_load_path;
        }

        public void LoadRecentChargeNumbers()
        {
            if (File.Exists(_chargeNumberFilePath))
            {
                var chargeCodeFile = ChargeCodeFile.ReadFile(_chargeNumberFilePath);
                _chargeCodes.Clear();
                foreach (var chargeCode in chargeCodeFile.ChargeCode)
                {
                    _chargeCodes.Add(new ChargeCodeTimer(chargeCode.Code, chargeCode.Description));
                }
            }
        }


        public ChargeCodeTimer AddNewChargeCode()
        {
            var chargeCode = new ChargeCodeTimer("NEWCODE", "Description of work.");
            _chargeCodes.Add(chargeCode);
            return chargeCode;
        }
        public void RemoveChargeCode(ChargeCodeTimer chargeCode)
        {
            _chargeCodes.Remove(chargeCode);
        }

        public void Reset()
        {
            foreach (var chargeCode in _chargeCodes)
            {
                chargeCode.Time.Clear();
            }
        }


        public void Load(string path)
        {
            _chargeNumberFilePath = path;
            LoadRecentChargeNumbers();
        }

        public void Save(string path)
        {
            var chargeCodeFile = new ChargeCodeFile();
            foreach (var chargeCodeTimer in _chargeCodes)
            {
                chargeCodeFile.ChargeCode.Add(new ChargeCode()
                {
                    Code = chargeCodeTimer.Code,
                    Description = chargeCodeTimer.Description
                });
            }
            chargeCodeFile.WriteFile(path);
        }

        public void WriteCSV(string path)
        {
            var chargeCodeTimerDict = new Dictionary<string, ChargeCodeTimer>();
            foreach (var timer in _chargeCodes)
            {
                if (timer.Code != null)
                {
                    if (!chargeCodeTimerDict.ContainsKey(timer.Code))
                    {
                        chargeCodeTimerDict.Add(timer.Code, timer);
                    }
                }

            }
            DateTime currentTime = DateTime.Now;
            string todayColumn = currentTime.ToShortDateString();
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
            if (!table.ContainsHeader(todayColumn))
            {
                table.AddColumn(todayColumn);
            }

            List<string> chargeCodes = table.GetColumn("Charge Codes").ToList();
            foreach (string code in chargeCodeTimerDict.Keys)
            {
                int chargeCodeIdx = chargeCodes.IndexOf(code);
                //If the code is already in the list, set the time.
                if (chargeCodeIdx >= 0)
                {
                    table.SetValue(todayColumn, chargeCodeIdx, chargeCodeTimerDict[code].Time.ToTimeSpan().ToString());
                }
                else // We will have to add the code..
                {
                    int rowIdx = table.AddRow();
                    table.SetValue("Charge Codes", rowIdx, code);
                    table.SetValue(todayColumn, rowIdx, chargeCodeTimerDict[code].Time.ToTimeSpan().ToString());
                }
            }
            table.WriteToFile(path);
        }

    }
}
