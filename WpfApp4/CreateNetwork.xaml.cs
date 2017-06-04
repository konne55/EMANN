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
    /// Interaktionslogik für CreateNetwork.xaml
    /// </summary>
    public partial class NetGenerator : Window
    {
        int globalid = 0;
        ObservableCollection<HiddenLayer> hiddenLayers;
        public NetGenerator()
        {
            InitializeComponent();
            hiddenLayers = new ObservableCollection<HiddenLayer>();
            ListBoxHiddenLayers.ItemsSource = hiddenLayers;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            globalid++;
            HiddenLayer hl = new HiddenLayer();
            hl.id = globalid;
            hiddenLayers.Add(hl);
        }

        private class HiddenLayer
        {

            public HiddenLayer()
            {

            }

            public int id { get; set; }
            public int numOfNeurons { get; set; }
            public FireFunc  FireFunction { get; set; }
        }
    }
}
