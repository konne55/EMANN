using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class InputNeuron : Neuron 
    {

        public InputNeuron()
        {
            Value = InputValue;
            IsInputNeuron = true;
        }
       

    }
}
