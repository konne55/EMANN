using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace EMANeuralNetwerk
{
    /// <summary>
    /// Interaktionslogik für Main.xaml
    /// </summary>
    public partial class Main : Window
    {
        ObservableCollection<NeuronalNetwork> neuronalNetworks;

        public Main()
        {
            InitializeComponent();

            neuronalNetworks = new ObservableCollection<NeuronalNetwork>();

            //for (int i = 0; i < 10; i++)
            //{
            //    NeuronalNetwork nn = new NeuronalNetwork();
            //    nn.Name = String.Format ( "{0}. Netzwerk",i);
            //    neuronalNetworks.Add(nn);
            //}

            DataBinding();
        }


        // <Binding ListData onto UI-Elements
        private void DataBinding()
        {
            listBoxNetworks.ItemsSource = neuronalNetworks;
            MainTabControl.ItemsSource = neuronalNetworks;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NetGenerator createNetworkWindow = new NetGenerator();
            createNetworkWindow.Show();

            //NeuronalNetwork nn = new NeuronalNetwork();
            //nn.Name = "Neues netz";
            //nn.Epoch = 1010;
            //neuronalNetworks.Add(nn);
            //listBoxNetworks.ItemsSource = neuronalNetworks;
        }
    }
}
