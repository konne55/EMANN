using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    public abstract class Neuron
    {

        public enum enmFireFunctions
        {
            sigmoid,
            sigmoidDerivative,
            identity,
            positivIdentity
        }


        static ulong globalID;
        private ulong id;
        private float value;
        private float derivative;
        private float deltaE;

        private bool isMashed;
        private bool isInputNeuron;
        private bool isOutputNeuron;
        private bool isBias;
        private enmFireFunctions fireFunc;
        private float inputValue;
        List<Synapse> senderSynapsen = new List<Synapse>();
        List<Synapse> recipientSynapsen = new List<Synapse>();


        public Neuron()
        {
            id = globalID++;
        }

        public float Value { get { return this.value; } set { this.value = value; } }
        public ulong ID { get { return this.id; } set { this.id = value; } }
        public float InputValue { get { return inputValue; } set { inputValue = value; } }
        public float DeltaE { get { return deltaE; } set { deltaE = value; } }
        public float Derivative { get { return derivative; } }
        public List<Synapse> SenderSynapsen { get { return senderSynapsen; } }
        public List<Synapse> RecipientSynapsen { get { return recipientSynapsen; } }

        public bool IsMashed { get { return isMashed; } set { this.isMashed = value; } }
        public bool IsInputNeuron { get { return isInputNeuron; } set { isInputNeuron = value; } }
        public bool IsOutputNeuron { get { return isOutputNeuron; } set { isOutputNeuron = value; } }
        public bool IsBias { get { return isBias; } set { isBias = value; } }

        public void addSenderSynapse(Synapse s)
        {
            senderSynapsen.Add(s);
        }
        public void addRecipientSynapse(Synapse s)
        {
            recipientSynapsen.Add(s);
        }

        public void Activate() //Wandelt ihren Ouput entsprechend von f um
        {
            enmFireFunctions f = FireFunc;
            if (f == enmFireFunctions.sigmoid)
            {
                Value = new FireFunc().Sigmoid(inputValue);
                derivative = value * (1 - value);
            }
            else if (f == enmFireFunctions.identity)
            {
                Value = new FireFunc().Identity(inputValue);
                derivative = 1;
            }
            else if (f == enmFireFunctions.positivIdentity)
            {
                Value = new FireFunc().positiveIdentity(inputValue);
                if (inputValue < 0)
                {
                    derivative = 0;
                }
                else
                {
                    derivative = 1;
                }

            }
        }

        public void SumInputs() //Zählt ihren Input
        {
            InputValue = 0;
            foreach (Synapse s in SenderSynapsen)
            {
                if (s.FarestNeuron == this && !s.NearestNeuron.IsBias)
                {
                    s.FarestNeuron = s.NearestNeuron;
                    s.NearestNeuron = this;
                    //InputValue += s.NearestNeuron.Value * s.Weight;
                }

                InputValue += s.FarestNeuron.Value * s.Weight;

            }
        }


        public void fire()
        {
            SumInputs();
            Activate();
        }

        public enmFireFunctions FireFunc
        {
            get { return fireFunc; }
            set { this.fireFunc = value; }
        }



    }
}
