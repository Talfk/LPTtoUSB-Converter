using System;
using System.IO;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*A class to manqage The Xml file that saves the Folder and COM Port
settings for the next time the program opens
*/
namespace LPTtoUSB_Converter
{
    public class XmlManager
    {
        //XmlDataWriter writes objects of any type to the Xml file  
        public static void XmlDataWriter(object obj, string filename)
        {
            XmlSerializer sr = new XmlSerializer(obj.GetType());
            TextWriter writer = new StreamWriter(filename);
            sr.Serialize(writer, obj);
            writer.Close();
        }


        //XmlDataReader reads the values from the Xml file
        public static Data XmlDataReader(string filename)
        {
            Data obj = new Data();
            XmlSerializer xs = new XmlSerializer(typeof(Data));
            FileStream reader = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
            obj = (Data)xs.Deserialize(reader);
            reader.Close();
            return obj;
        }
    }
}
