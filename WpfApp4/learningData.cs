using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class LearningData
    {
        float[] inputValues;
        float[] outputValues;
        Dictionary<float[], float[]> datasets;
        int lastDataset = 0;
        bool isLearningDataSet;

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
