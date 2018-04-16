using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace TimeKeeper
{

    public class ChargeCode
    {
        [XmlAttribute]
        public string Code;

        [XmlText]
        public string Description;
    }

    [Serializable]
    [XmlRoot("ChargeCodes")]
    public class ChargeCodeFile
    {
        [XmlElement("ChargeCode")]
        public List<ChargeCode> ChargeCode;

        public ChargeCodeFile()
        {
            ChargeCode = new List<ChargeCode>();
        }

        public static ChargeCodeFile ReadFile(string filename)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ChargeCodeFile));
            using (var reader = new StreamReader(filename))
            {
                var ccf = (ChargeCodeFile)xs.Deserialize(reader);

                return ccf;
            }

        }
        public void WriteFile(string filename)
        {
            XmlSerializer xs = new XmlSerializer(typeof(ChargeCodeFile));
            using (var writer = new StreamWriter(filename))
            {
                xs.Serialize(writer, this);
            }

        }
    }
}
