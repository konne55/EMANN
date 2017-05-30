using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class Synapse
    {
        float weight = 0.5F;
        float delta = 0F;

        private  Neuron nearestNeuron;
        private  Neuron farestNeuron;

        public float Weight { get {return weight; } set { weight = value; } }
        public Neuron FarestNeuron { get { return this.farestNeuron; } set { this.farestNeuron = value; } }
        public Neuron NearestNeuron { get { return this.nearestNeuron; } set { this.nearestNeuron = value;} }
        public float Delta { get { return delta; } set { delta = value; } }

        public Synapse()
        {
        }

        public Synapse(Neuron nN, Neuron fN)
        {
            this.nearestNeuron = nN;
            this.farestNeuron = fN;
        }

        public void adjustYourself()
        {
            weight -= delta;
        }
        
    }
}
