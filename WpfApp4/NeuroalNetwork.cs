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
    public class NeuronalNetwork
    {
        Random random;
        List<InputNeuron> inputNeuronen;
        List<List<HiddenNeuron>> hiddenLayers;
        List<OutputNeuron> outputNeuronen;
        List<Synapse> allSynapses;
        List<Neuron> neurons;
        Neuron bias;
        LearningData learningData;
        float globalErrorPerIteration;
        float[] results;
        float alpha; //Fehlerkorrektur
        bool isInputSet;
        float globalErrorAveragePerSet;
        List<float> errors;

        // <ctor>
        public NeuronalNetwork()
        {
            inputNeuronen = new List<InputNeuron>();
            hiddenLayers = new List<List<HiddenNeuron>>();
            outputNeuronen = new List<OutputNeuron>();
            allSynapses = new List<Synapse>();
            neurons = new List<Neuron>();
            random = new Random();
            results = new float[0];
            learningData = new LearningData();
            errors = new List<float>();
        }
        // </ctor>

        // <Props>
        public List<InputNeuron> InputNeuronen { get { return inputNeuronen; } }
        public List<List<HiddenNeuron>> HiddenLayers { get { return hiddenLayers; } }
        public List<OutputNeuron> OutputNeuronen { get { return outputNeuronen; } }
        public List<Synapse> AllSynapses {  get { return allSynapses; } }
        public List<Neuron> Neuronen { get { return neurons; } }
        public Neuron Bias { get { return bias; } }
        public int GetAmountOutputNeurons { get { return outputNeuronen.Count; }  }
        public int GetAmountInputNeurons { get { return inputNeuronen.Count; } }
        public float GlobalErrorPerIteration { get { return globalErrorPerIteration; }  }
        public float GlobalErrorAveragePerSet { get { return globalErrorAveragePerSet; } }
        public float Alpha { get { return alpha; } set { alpha = value; } }
        public LearningData LearningData { get { return learningData; } set { this.learningData = value; } }
        public long Epoch { get; set; }
        // </props>

        // <create a Network Template>
        public void createNetTemplate(int AmountInputNeuronen, int AmountHiddenLayer, int AmountHiddenNeuronenPerLayer, int AmountOutputNeuronen)
        {
            for (int i = 0; i < AmountInputNeuronen; i++)
            {
                InputNeuron iN = new InputNeuron();
                inputNeuronen.Add(iN);
            }

            for (int i = 0; i < AmountHiddenLayer ; i++)
            {
                List<HiddenNeuron> HiddenNeuronen = new List<HiddenNeuron>();
                for (int j = 0; j < AmountHiddenNeuronenPerLayer ; j++)
                {
                    HiddenNeuron hN = new HiddenNeuron();
                    HiddenNeuronen.Add(hN);
                }
                hiddenLayers.Add(HiddenNeuronen);
            }

            for (int i = 0; i < AmountOutputNeuronen ; i++)
            {
                OutputNeuron oN = new OutputNeuron();
                outputNeuronen.Add(oN);
            }
        }
        // </create a network template>


        // ###############################################
        // ###############  ADD NEURONS ##################
        // ###############################################

        // <add  Input Neuron>
        public void addInputNeurons(int amountNeurons)
        {
            for (int i = 0; i < amountNeurons; i++)
            {
                InputNeuron iN = new InputNeuron();
                iN.FireFunc = Neuron.enmFireFunctions.identity;
                inputNeuronen.Add(iN);
                neurons.Add(iN);
            }
        }
        // </ add single Input Neuron>

        // <add single Output Neuron>
        public void addOutputNeurons(int amountNeurons)
        {
            for (int i = 0; i < amountNeurons ; i++)
            {
                OutputNeuron oN = new OutputNeuron();
                oN.FireFunc = Neuron.enmFireFunctions.sigmoid    ;
                outputNeuronen.Add(oN);
                neurons.Add(oN);
            }
            Array.Resize(ref results, outputNeuronen.Count);
        }
        // </ add single Output Neuron>

        // <add Hidden Layer>
        public void addHiddenLayer(int amountHiddenNeuronen, int position)
        {
            List<HiddenNeuron> HiddenNeurons = new List<HiddenNeuron>();
            for (int i = 0; i < amountHiddenNeuronen ; i++)
            {
                HiddenNeuron hN = new HiddenNeuron();
                hN.FireFunc = Neuron.enmFireFunctions.sigmoid;
                HiddenNeurons.Add(hN);
                neurons.Add(hN);
            }
            if (position > hiddenLayers.Count)
            {
                position = hiddenLayers.Count();
            }
            hiddenLayers.Insert(position, HiddenNeurons);
            
        }
        // </add Hidden Layer>

        // <add global Bias>
        public void enableGlobalBias(bool biasActivate) {
            if(biasActivate)
            {
                bias = new InputNeuron();
                bias.IsBias = true;
                bias.InputValue = 1;
                bias.Value = 1;
                neurons.Add(bias);

                foreach (Neuron n in neurons)
                {
                    if (n.IsInputNeuron != true) 
                    { 
                    Synapse s = new Synapse();
                    s.FarestNeuron = n;
                    s.NearestNeuron = bias;
                    n.addSenderSynapse(s);
                    bias.addRecipientSynapse(s);
                    allSynapses.Add(s);
                    }
                }

            }

        }
        // </add global Bias>

        
            
        // ###############################################
        // ############  MESHING THE NETWORK #############
        // ###############################################

        // <mesh the full Network - each Neuron to his direct neighbor>
        public void fullMeshNetwork()
        {
            fullMeshInputNeurons();
            fullMeshAllHiddenLayers();
            fullMeshOutputNeurons();
        }
        // </mesh full network>

        // <mesh InputNeurons to if existing HiddenLayers or else to outputlayer>
        public void fullMeshInputNeurons()
        {
            foreach (InputNeuron iN in inputNeuronen)
            {
                if (hiddenLayers.Count() > 0) //InputNeuronen mit der ersten HiddenSchicht vernetzten
                {
                    foreach (HiddenNeuron hN in hiddenLayers.First())
                    {
                        Synapse sI = new Synapse(iN, hN);
                        //Synapse sH = new Synapse(hN, iN);
                        iN.addRecipientSynapse(sI);
                        hN.addSenderSynapse(sI);
                        //hN.IsMashedLeftSide = true;

                        allSynapses.Add(sI);
                        //allSynapses.Add(sH);
                    }
                }
                else //InputNeuronen mit den Output Neuronen vernetzten
                {
                    foreach (OutputNeuron oN in outputNeuronen)
                    {
                        Synapse sI = new Synapse(iN, oN);
                        //Synapse sO = new Synapse(oN, iN);
                        iN.addRecipientSynapse(sI);
                        oN.addSenderSynapse(sI);
                        //oN.addSynapse(sO);
                        //oN.IsMashed = true;

                        allSynapses.Add(sI);
                        //allSynapses.Add(sO);
                    }
                }

                //iN.IsMashed = true;
            }
        }
        // </mesh InputLayer>

        // <full Mesh OutputNeurons>
        public void fullMeshOutputNeurons()
        { 
            foreach (OutputNeuron oN in outputNeuronen)
            {
                if (hiddenLayers.Count() > 0) // Mesh Output with Last HiddenLayer
                {
                    foreach (HiddenNeuron  hN in hiddenLayers.Last())
                    {
                        Synapse sO = new Synapse(oN,hN);
                        Synapse sH = new Synapse(hN, oN);
                        oN.addSenderSynapse(sO);
                        hN.addRecipientSynapse(sO);
                        //hN.addSynapse(sH);
                        //hN.IsMashedRightSide = true;

                        allSynapses.Add(sO);
                        //allSynapses.Add(sH);
                    }
                }
                else // Mesh Output with InputLayer
                {
                    if (!oN.IsMashed ) 
                    /* Sollte eigentlich nie true werden, 
                    *da die Neuronen wenn es keine HiddenLayer gibt schon bei den InputNeuronen vernetzt werden!
                    */
                    {
                        foreach (InputNeuron  iN in inputNeuronen )
                        {
                            Synapse sO = new Synapse(oN, iN);
                            //Synapse sI = new Synapse(iN, oN);
                            iN.addRecipientSynapse(sO);
                            oN.addSenderSynapse(sO);
                            //iN.addSynapse(sI);
                            //iN.IsMashed = true;

                            allSynapses.Add(sO);
                            //allSynapses.Add(sI);
                        }
                    }
                }
                oN.IsMashed = true;

            }

        }
        // </full Mesch OuputNeurons>

        // <full Mesh All HiddenNeurons>
        public void fullMeshAllHiddenLayers()
        {
            //First with Input happens in the InputMethod
            //Last with Output happens in the OutputMehtod
            //Here, we just mesh all Layers inbetween

            if (hiddenLayers.Count > 1) //there are some Layers to Mesh
            {
                for (int i = 0; i < hiddenLayers.Count -1; i++)
                {

                    foreach (HiddenNeuron hNl in hiddenLayers.ElementAt(i))
                    {
                        foreach (HiddenNeuron hNr in hiddenLayers.ElementAt (i+1))
                        {
                            Synapse sL = new Synapse(hNl, hNr);
                            //Synapse sR = new Synapse(hNr, hNl);
                            hNr.addSenderSynapse(sL);
                            hNl.addRecipientSynapse(sL);
                            //hNr.addSynapse(sR);
                            //hNr.IsMashedLeftSide  = true;

                            allSynapses.Add(sL);
                            //allSynapses.Add(sR);
                        }
                        //hNl.IsMashedRightSide = true;
                    }

                }

            }
        }
        // </full Mesh All HiddenNeurons>


        // ###############################################
        // ############  INITIALIZE NETWORK ##############
        // ###############################################

        // <randomize weights>
        public void setRandomizedWeights()
        {
            foreach (Synapse  s in allSynapses )
            {
                s.Weight = (float)(random.NextDouble() * 2) - 1;
            }

        }
        // </randomize all weights>

        // <set Input Values>
        public void setNewInputValuesFromLearningDataset()
        {
            float[] inputValues = learningData.GetNewInputDataset();
            if ((inputValues.Length  == inputNeuronen.Count))
            {
                for (int i = 0; i < inputValues.Length ; i++)
                {
                    inputNeuronen.ElementAt(i).InputValue  = inputValues[i];
                    inputNeuronen.ElementAt(i).Value = inputValues[i];
                }
                isInputSet = true;
            }
            else
            {
                isInputSet = false;
                // Inputarray stimmt nicht mit den InputNeuronen überein!!
            }
        }
        // </set Input Values>

        // ###############################################
        // ############## CALCULATE NETWORK ##############
        // ###############################################

        // <calculate the Network!>
        public float[] calculate()
        {
            int i = 0;
            if(isInputSet) { 
                foreach (List<HiddenNeuron> list in hiddenLayers ) //Beginn with the first Layer in HiddenNeurons
                {
                    foreach (HiddenNeuron hN in list)
                    {
                        hN.fire(); //Calculate all HiddenNeurons 
                    }
                }

                foreach (OutputNeuron oN in outputNeuronen ) //After HiddenNeurons Calculate the OutputNeurons
                {
                    oN.fire();
                    results[i] = oN.Value;
                    i++;
                }

                return results;
            }
            else
            {
                return null;
            }
        }

        // ###############################################
        // #############  TRAIN THE NETWORK ##############
        // ###############################################

        public void calculateGlobalOutputError()
        {
            if(learningData.IsLearningDataSet) { 
                float[] OutputDataSet = learningData.GetNewOutputDataset();
                globalErrorPerIteration = 0;
                if(OutputDataSet.Length == results.Length)
                {
                    for (int i = 0; i < results.Length ; i++)
                    {
                        globalErrorPerIteration += Math.Abs(OutputDataSet[i] - results[i]);
                    }
                    if(errors.Count > 100 * learningData.AmountLearningDataSets)
                    {
                        errors.RemoveAt(0);
                    }

                    errors.Add( globalErrorPerIteration);
                    globalErrorAveragePerSet = errors.Average();
                }
            }
        }

        public void backPropagate()
        {
            float[] OutputDataSet = learningData.GetNewOutputDataset();

            int i = 0;
            foreach (OutputNeuron oN in outputNeuronen )
            {
                // delta Errechnen
                oN.DeltaE = oN.Derivative * ( oN.Value - OutputDataSet[i++]);
                foreach (Synapse s in oN.SenderSynapsen )
                {
                    s.Delta = oN.DeltaE * s.FarestNeuron.Value * alpha;
                    s.adjustYourself();
                }

            }

            foreach (List<HiddenNeuron> lHN in hiddenLayers)
            {
                foreach (HiddenNeuron hN in lHN)
                {
                    float temp = 0;
                    foreach (Synapse sR in hN.RecipientSynapsen )
                    {
                        temp += sR.NearestNeuron.DeltaE * sR.Weight;
                    }
                    hN.DeltaE = hN.Derivative * temp ;

                    foreach (Synapse s in hN.SenderSynapsen)
                    {
                        s.Delta = hN.DeltaE * s.FarestNeuron.Value * alpha;
                        s.adjustYourself();
                    }
                }
            }


            //foreach (Neuron n in neurons )
            {

            }
        }

        public void nextDataSet()
        {
            learningData.DataSetMoveNext();
        }

        public void NextEpoch()
        {
            Epoch++;
        }

        // <Print the basic Networkstats for ex. the ConsoleApp>
        public string printNetworkStats()
        {
            int HLN;
            HLN = !(hiddenLayers.Count > 0) == true ?  0 :  hiddenLayers.First().Count;
            return string.Format("Das Netz besteht aus {0} Inputneuronen, {1} HiddenLayer à {2} HiddenNeuronen und {3} Outputneuronen. Es ist mit {4} Synapsen" +
                "verknüpft.", inputNeuronen.Count, hiddenLayers.Count, HLN, outputNeuronen.Count, allSynapses.Count);
        }
        // </Print the basic Networkstats>

        // ###############################################
        // ###############  SERIALIZATION ################
        // ###############################################


            // TUT NOCH NICHT!!!
        public void saveSynapses(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                var xml = new XmlSerializer(this.allSynapses.GetType ());
                {
                   xml.Serialize(stream, this.AllSynapses );
                }
            }
        }


        public void saveLearningData(string FileName)
        {
            using (FileStream stream = new FileStream(FileName, FileMode.Create))
            {
                var xml = new XmlSerializer(typeof(LearningData));
                {
                    xml.Serialize(stream, LearningData  );
                }
            }
        }

    }
}
