//Corey Wunderlich 2018
//Serialize and Deserialize the Charge Code XML File
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
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
            XmlSerializer serializer = new XmlSerializer(typeof(ChargeCodeFile));
            using (var reader = new StreamReader(filename))
            {
                var chargeCodeFile = (ChargeCodeFile)serializer.Deserialize(reader);

                return chargeCodeFile;
            }

        }
        public void WriteFile(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(ChargeCodeFile));
            using (var writer = new StreamWriter(filename))
            {
                serializer.Serialize(writer, this);
            }

        }
    }
}
