using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    class SerializeData
    {

        Stream stream = null;
        BinaryFormatter bformatter = null ;
        string txtFileName = "";
        public string DataPath { get { return txtFileName; } set { txtFileName = value; } }

        public SerializeData(string FileName)
        {
            txtFileName = FileName;
            bformatter = new BinaryFormatter();
            //stream = File.Open(txtFileName, FileMode.Create);
        }

        public SerializeData()
        {
            bformatter = new BinaryFormatter();
        }


        public void SerializeObject(Object objectToSerialize)
        {
            stream = File.Open(txtFileName, FileMode.Create);
            bformatter.Serialize(stream, objectToSerialize);
            stream.Close();
        }



        public Object DeserializeObject()
        {

            Object objectToDeserialize = null;
            stream = File.Open(txtFileName, FileMode.Open);

            try
            {
                while(stream.CanSeek)
                {
                    objectToDeserialize = (Object)bformatter.Deserialize(stream);

                    if (objectToDeserialize is NeuronalNetwork)
                    {
                        stream.Close();
                      return objectToDeserialize ;
                    }
                }
            }catch (SerializationException)
            {

            }
            stream.Close();
            return null;
        }


        public void CloseStream()
        {
            stream.Close();
        }
    }
}
