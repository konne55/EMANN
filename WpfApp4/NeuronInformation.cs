using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    class NeuronInformation
    {

        public string ID { get; set; }
        public string OutputValue { get; set; }
        public string InputValue { get; set; }
        public string fireFunc { get; set; }
        public List<Synapse> SynapsenOfNeuron { get; set; }
        public bool isFilled { get; set; }
    }
}
