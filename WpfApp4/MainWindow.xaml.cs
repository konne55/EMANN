using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EMANeuralNetwerk
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        NeuronalNetwork nn;
        System.Timers.Timer updateTick;
        bool isNetworkInitialized;
        volatile bool isLearning;
        bool isSlowLearning;
        bool DrawNetworkIsEnabled =true;
        bool PrintStatsIsEnabled = true ;
        string learnDataPath;
        string status;
        float[] result;
        Thread cNNThread;
        SerializeData sdNetwork;
        GraphXY errorGraph;

        long start;
        long diff;

        LearningData learningData = new LearningData();
        DrawNN drawNN;
        NeuronInformation neuronInformation = new NeuronInformation();

        public MainWindow()
        {
            InitializeComponent();

            //Berechnet das NN
            updateTick = new System.Timers.Timer(0.01);
            updateTick.Elapsed += UpdateTick_Elapsed;

            //Updated die GUI
            DispatcherTimer dpTimer = new DispatcherTimer();
            dpTimer.Interval = TimeSpan.FromSeconds  (0.015);
            dpTimer.Tick += DpTimer_Tick;
            dpTimer.Start ();

            sdNetwork = new SerializeData();
            result = new float[0];
            nn = new NeuronalNetwork();
            drawNN = new DrawNN(drawArea);
            errorGraph = new GraphXY(GraphError, "Fehlerentwicklung", " Anzahl Epochs", "Fehler");
        }

        // <activationFunctions for Timer>
        private void UpdateTick_Elapsed(object sender, ElapsedEventArgs e)
        {
            start = DateTime.Now.ToFileTime();
            calculateOneStepOfNN();
            diff = DateTime.Now.ToFileTime() - start;
        }
        private void DpTimer_Tick(object sender, EventArgs e)
        {
            updateResult();
        }
        // </activationFunctions for Timer>


        public void updateResult()
        {
            if (isNetworkInitialized)
            {
                // < print the OutputNeurons_Value in the Listbox>
                if (PrintStatsIsEnabled) { 
                    resultViewBox.Items.Clear();
                    
                    for (int i = 0; i < result.Length; i++)
                    {
                        ListBoxItem lbi = new ListBoxItem();
                        lbi.Content = String.Format("Neuron {0}: {1}", i + 1, result[i]);
                        resultViewBox.Items.Add(lbi);
                    }
                    txtblckglobalOutputError.Text =  nn.GlobalErrorPerIteration.ToString() + String.Format("({0})",nn.GlobalErrorAveragePerSet) ;
                }
                // </print OutputNeurons_value>
                // <print basic Network Stats into StatusBar>
                StatusBarNetwork.Text = status;
                    // </print basic Network Stats into Statusbar>
                
                // <draw the Network on Canvas>
                if (DrawNetworkIsEnabled) { drawNN.drawNetwork(nn); }
                // </draw Network on Canvas>

                // <get and print detailed Information of hovered or clicked Neuron>
                if (PrintStatsIsEnabled)
                {
                neuronInformation = drawNN.printNeuronInformation();
                if(neuronInformation.isFilled)
                { 
                    txtBlckFireFunc.Text  = "FireFunc: " + neuronInformation.fireFunc;
                    txtBlckNeuronId.Text = "NeuronID: " + neuronInformation.ID ;
                    txtBlckNeuronInputValue.Text = "InputValue: " + neuronInformation.InputValue ;
                        txtBlckOutputValue.Text = "OutputValue: " + neuronInformation.OutputValue;
                    synapseViewBox.Items.Clear();
                    foreach (Synapse s in neuronInformation.SynapsenOfNeuron )
                    {
                        ListBoxItem lbi = new ListBoxItem();
                            lbi.Content = string.Format("({0}){1}*{2}={3}", s.FarestNeuron.ID.ToString(), s.FarestNeuron.Value.ToString(), s.Weight.ToString(),
                                (s.FarestNeuron.Value * s.Weight).ToString () );
                        lbi.Tag = s;
                        synapseViewBox.Items.Add(lbi);
                    }
                }
                }
                // </detaild info of neuron>

                // <print alpha>
                txtblckAlpha.Text  = nn.Alpha.ToString();
                sliderAlpha.Value = nn.Alpha;
                // </print alpha>

                // <print calculations-Info>
                calcDuration.Text = diff.ToString();
                amountCalcs.Text = nn.Epoch.ToString();
                // </print calculation-Info>


            }

        }
        public void calculateOneStepOfNN()
        {
            
            nn.setNewInputValuesFromLearningDataset();
            result = nn.calculate();
            status = nn.printNetworkStats();
            nn.backPropagate();
            nn.calculateGlobalOutputError();
            nn.nextDataSet();
            nn.NextEpoch();
            
        }
        public void trainNN()
        {
            while (isLearning )
            {
                calculateOneStepOfNN();
            }
        }
        

        // <Initialize Network>
        private void btnInitNetwork_Click(object sender, RoutedEventArgs e)
        {
            nn = new NeuronalNetwork();
            // <Net-Dimensions>
            nn.addInputNeurons(int.Parse (txtbxAmountInputNeurons.Text ));
            nn.addOutputNeurons(int.Parse (txtbxAmountOutputNeurons.Text ), Neuron.enmFireFunctions.sigmoid);
            
            for (int i = 0; i < int.Parse (txtbxAmountHiddenLayer.Text); i++)
            {
                nn.addHiddenLayer(int.Parse(txtbxAmountHiddenNeurons.Text), i);
            }
            nn.enableGlobalBias((bool)chckBxEnableGlobalBias.IsChecked);
            // </Net-Dimensions>

            nn.Alpha = 0.25F;

            nn.LearningData = learningData;
            nn.fullMeshNetwork();
            nn.setRandomizedWeights();
            Array.Resize(ref result, nn.GetAmountOutputNeurons);

            btnCalcOneStep.IsEnabled = true;
            btnStartLearning.IsEnabled = true;
            btnSlowLearning.IsEnabled  = true;
            isNetworkInitialized = true;

        }
        // </Initialize Network>

        // <OneStep Calculation>
        private void btnCalcOneStep_Click(object sender, RoutedEventArgs e)
        {
            if(!(isLearning || isSlowLearning)) // wer will schon beim lernen gestört werden....
            { 
            calculateOneStepOfNN();
            }
        }
        // </OneStep Calculation>

        // <LearningProgress>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!isSlowLearning)
            {
                if (isLearning)
                {
                    isLearning = false;
                    while (!cNNThread.IsAlive) ;
                    Thread.Sleep(1);
                    cNNThread.Join();
                    btnStartLearning.Content = "Starte Lernvorgang";
                }
                else
                {
                    isLearning = true;
                    cNNThread = new Thread(trainNN);
                    cNNThread.Start();
                    btnStartLearning.Content = "Pausiere Lernvorgang";
                }
            }

        }
        // </LearningProgress>

        private void btnSlowLearning_Click(object sender, RoutedEventArgs e)
        {
            if (!isLearning)
            {
                if (isSlowLearning)
                {
                    isSlowLearning = false;
                    updateTick.Enabled = false;
                    btnSlowLearning.Content = "Starte Lernvorgang";
                }
                else
                {
                    isSlowLearning = true;
                    updateTick.Enabled = true;
                    btnSlowLearning.Content = "Pausiere Lernvorgang";
                }
            }
        }

        // <load learning data>
        private void loadLearningData_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "CSV-Tabellen (*.csv)|*.csv";
            if (ofd.ShowDialog() == true)
            {
                string[] temp;
                learnDataPath = ofd.FileName;
                temp = File.ReadAllLines(learnDataPath, Encoding.UTF8);
                int InputFields = Int32.Parse(temp[0]);
                int OutputFields = Int32.Parse(temp[1]);

                txtbxAmountInputNeurons.Text = InputFields.ToString();
                txtbxAmountOutputNeurons.Text = OutputFields.ToString();

                for (int i = 2; i < temp.Length ; i++)
                {
                    string[] data = temp[i].Split('\t');
                    float[] inputData = new float[InputFields];
                    float[] outputData = new float[OutputFields];
                    for (int j = 0; j < data.Length; j++)
                    {
                        if(j<InputFields )
                        {
                            inputData[j] = float.Parse ( data[j]);
                        }
                        else
                        {
                            outputData[j - InputFields] = float.Parse(data[j]);
                        }
                    }
                    learningData.addDataSet(inputData, outputData);
                    nn.LearningData = learningData ;
                }

            }
        }
        // </load learning data>

        // <adjust alpha LearnRatio>
        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (isNetworkInitialized ) { 
            nn.Alpha = (float)e.NewValue ;
            }
        }
        // </adjust alpha learnRatio>

        // <manage GUI, Graphs and Stats>
        private void chckBxEnableNetworkStats_Click(object sender, RoutedEventArgs e)
        {
            PrintStatsIsEnabled = (bool)chckBxEnableNetworkStats.IsChecked;
        }

        private void chckBxEnableNetworkGraph_Click(object sender, RoutedEventArgs e)
        {
            DrawNetworkIsEnabled = (bool)chckBxEnableNetworkGraph.IsChecked;
        }
        // </manage GUI, Graphs and Stats>

        // <save Network>
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SaveFileDialog  sfd = new SaveFileDialog();
            if (sfd.ShowDialog() == true )
            {
                sdNetwork.DataPath = sfd.FileName.ToString();
            }
            sdNetwork.SerializeObject(nn);
        }
        // </save Network>

        // <load Network>
        private void LoadNetworkBtn_Click(object sender, RoutedEventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();

            if (ofd.ShowDialog() == true)
            {
                sdNetwork.DataPath = ofd.FileName;
            nn = (NeuronalNetwork)sdNetwork.DeserializeObject();

            btnCalcOneStep.IsEnabled = true;
            btnStartLearning.IsEnabled = true;
                btnSlowLearning.IsEnabled = true;
            isNetworkInitialized = true;
            Array.Resize(ref result, nn.GetAmountOutputNeurons);
            }
        }

        private void btnShowNetGenerator(object sender, RoutedEventArgs e)
        {
            NetGenerator ng = new NetGenerator();
            ng.Show();


        }
        // </load Network>


    }
}
