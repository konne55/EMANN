using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class LearningData
    {
        public float[] inputValues;
        public float[] outputValues;
        public Dictionary<float[], float[]> datasets;
        public int lastDataset = 0;
        public bool isLearningDataSet;

        public bool IsLearningDataSet { get { return isLearningDataSet; } }

        public LearningData()
        {
            datasets = new Dictionary<float[], float[]>();
        }

        public void addDataSet(float[] input, float[] output)
        {
            inputValues = input;
            outputValues = output;
            datasets.Add(inputValues, outputValues);
            isLearningDataSet = true;
        }

        public float[] GetNewInputDataset()
        {
            return datasets.ElementAt(lastDataset).Key;
        }

        public float[] GetNewOutputDataset()
        {
            return datasets.ElementAt(lastDataset).Value;   
        }

        public void DataSetMoveNext()
        {
            if (lastDataset == datasets.Count-1 )
            {
                lastDataset = 0;
            }
            else
            {
                lastDataset++;
            }

        }

        public int AmountLearningDataSets { get { return datasets.Count; } }
        public int LastDataset { get { return lastDataset; } set { lastDataset = value; } }

        

    }
}
