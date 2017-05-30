using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class OutputNeuron : Neuron
    {


        //<ctor>
        public OutputNeuron()
        {
            IsOutputNeuron = true;
        }
        //</ctor>



    }
}
