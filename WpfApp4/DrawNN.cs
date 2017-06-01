using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace EMANeuralNetwerk
{
    class DrawNN
    {
        NeuronalNetwork nn;
        Canvas canvas;
        int amountLayers;
        int spaceRow;



        public DrawNN( Canvas Canvas)
        {
            nn = new NeuronalNetwork();
            this.canvas = Canvas;


        }

        public void drawNetwork(NeuronalNetwork nn)
        {
            this.nn = nn;
            amountLayers = 2 + nn.HiddenLayers.Count;
            spaceRow = (int)(canvas.ActualWidth / (amountLayers + 1));

            canvas.Children.Clear();
            drawInputNeurons();
            drawHiddenNeurons();
            drawOutputNeurons();
            drawBias();
            drawSynapses();
        }


        public void drawInputNeurons()
        {
            int top = 0;
            int diff = (int)(canvas.ActualHeight / (nn.InputNeuronen.Count + 1));
            foreach (InputNeuron iN in nn.InputNeuronen )
            {
                
                top += diff;

                TextBlock tb = new TextBlock();
                tb.Text = String.Format("In: {0}\nOut:{1}", iN.InputValue, iN.Value);
                tb.FontSize = 12.0;
                tb.Width = 60;
                tb.Height = 30;

                Ellipse elli = new Ellipse();
                if(iN.Value < 0.5)
                {
                    elli.Fill = Brushes.PaleVioletRed;
                }
                else
                { 
                elli.Fill = Brushes.CornflowerBlue;
                }

                double radius = 40 + 10 * iN.Value; // Geht nut anständig wenn sigmoid - sollte ich noch abfangen
                elli.Height = radius ;
                elli.Width = radius ;
                elli.Tag = iN;
                elli.MouseDown += Elli_MouseDown;
                elli.MouseEnter += Elli_MouseEnter;
                elli.ToolTip = "Wert: "+iN.Value.ToString () ;
                Canvas.SetLeft(elli, spaceRow - elli.Width  / 2);
                Canvas.SetTop(elli, top - elli.Height /2);
                Canvas.SetLeft(tb, spaceRow - 80   );
                Canvas.SetTop(tb, top - tb.Height / 2);

                if (!canvas.Children.Contains(elli))
                {
                    canvas.Children.Add(elli);
                }
                if (!canvas.Children.Contains (tb))
                {
                    canvas.Children.Add(tb);
                }
               
            }
        }

        public void drawHiddenNeurons()
        {
            if (nn.HiddenLayers.Count > 0)
            {
                int i=1;
                foreach (List<HiddenNeuron>  lHN in nn.HiddenLayers )
                {
                    i++;
                    int top = 0;
                    int diff = (int)(canvas.ActualHeight / (lHN.Count + 1));
                    foreach (HiddenNeuron  hN in lHN)
                    {
                        
                        top += diff;
                        Ellipse elli = new Ellipse();
                        if (hN.Value < 0)
                        {
                            elli.Fill = Brushes.LightGray ;
                        }
                        else
                        {
                            elli.Fill = Brushes.LightBlue ;
                        }

                        double radius = 40 + 20 * hN.Value;
                        elli.Height = radius;
                        elli.Width = radius;
                        elli.Tag = hN;
                        elli.MouseDown += Elli_MouseDown;
                        elli.MouseEnter += Elli_MouseEnter;
                        elli.ToolTip = "Wert: " + hN.Value.ToString ();
                        Canvas.SetLeft(elli, spaceRow * i - elli.Width/2 );
                        Canvas.SetTop(elli, top - elli.Height /2);

                        if (!canvas.Children.Contains(elli))
                        {
                            canvas.Children.Add(elli);
                        }

                    }
                }
                
            }

        }

        public void drawOutputNeurons()
        {
            int top = 0;
            int diff = (int)(canvas.ActualHeight / (nn.OutputNeuronen.Count + 1));
            foreach (OutputNeuron  oN in nn.OutputNeuronen  )
            {
                top += diff;

                TextBlock tb = new TextBlock();
                tb.Text = String.Format("In: {0}\nOut:{1}", oN.InputValue, oN.Value);
                tb.FontSize = 12.0;
                tb.Width = 60;
                tb.Height = 30;

                Ellipse elli = new Ellipse();
                if (oN.Value < 0)
                {
                    elli.Fill = Brushes.RosyBrown;
                }else if(oN.Value > 0 && oN.Value < 0.5)
                {
                    elli.Fill = Brushes.MistyRose;
                }
                else {
                    elli.Fill = Brushes.Aquamarine;
                }




                double radius = 40 + 20 * oN.Value; // Geht nut anständig wenn sigmoid - sollte ich noch abfangen
                elli.Height = radius;
                elli.Width = radius;
                elli.Tag = oN;
                elli.MouseDown += Elli_MouseDown;
                elli.MouseEnter += Elli_MouseEnter;
                elli.ToolTip = "Wert: "+ oN.Value.ToString();
                Canvas.SetLeft(elli, spaceRow * amountLayers - elli.Width /2 );
                Canvas.SetTop(elli, top - elli.Height / 2);

                Canvas.SetLeft(tb, spaceRow * amountLayers + 40);
                Canvas.SetTop(tb, top - tb.Height / 2);

                if (!canvas.Children.Contains(elli))
                {
                    canvas.Children.Add(elli);
                }
                if (!canvas.Children.Contains(tb))
                {
                    canvas.Children.Add(tb);
                }
            }
        }

        public void drawBias()
        {
            if(nn.Bias != null) //Draw Bias
            {
                //int top = 0;
                int diff = (int)(canvas.ActualHeight / (nn.InputNeuronen.Count + 1));
                Ellipse elli = new Ellipse();
                elli.Fill = Brushes.ForestGreen ;
                elli.Opacity = 0.2;
                double radius = 30;
                elli.Height = radius;
                elli.Width = radius;
                elli.Tag = nn.Bias;
                elli.MouseDown += Elli_MouseDown;
                elli.MouseEnter += Elli_MouseEnter;
                elli.ToolTip = "Wert: " + nn.Bias.Value.ToString();
                Canvas.SetLeft(elli, spaceRow);
                Canvas.SetTop(elli, diff -50 );
                if (!canvas.Children.Contains(elli))
                {
                    canvas.Children.Add(elli);
                }
            }
        }

        public void drawSynapses()
        {
            foreach (Synapse s in nn.AllSynapses )
            {
                Line line = new Line();
                line.StrokeThickness = 1 + Math.Abs ( s.Weight )* 2;
                line.Opacity = 0.5;
                line.ToolTip = "Gewicht: "+ s.Weight.ToString();

                if (s.Weight < 0)
                {
                line.Stroke = Brushes.Red;
                line.Fill = Brushes.Red;
                }
                else
                {
                line.Stroke = Brushes.Blue;
                line.Fill = Brushes.Blue;
                }

                if(s.NearestNeuron == nn.Bias)
                {
                    line.Opacity = 0.15;
                    line.StrokeThickness = line.StrokeThickness * 0.5;
                }

                foreach (Ellipse item in canvas.Children.OfType<Ellipse>())
                {
                    if(item.Tag == s.FarestNeuron)
                    {
                        // line.X1 = item
                        line.Y1 = Canvas.GetTop(item) + (item.Height  / 2 );
                        line.X1 = Canvas.GetLeft(item) + (item.Width / 2 );

                    }
                    else if(item.Tag == s.NearestNeuron )
                    {
                        line.Y2 = Canvas.GetTop(item) + (item.Height / 2); ;
                        line.X2 = Canvas.GetLeft(item) + (item.Width / 2); ;
                    }
                }
                canvas.Children.Add(line);
            }
        }

        private void Elli_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Ellipse elli = (Ellipse)sender;
            SetNeuronInformation((Neuron)elli.Tag);
        }

        private void Elli_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Ellipse elli = (Ellipse)sender;
            SetNeuronInformation((Neuron)elli.Tag);
        }

        private NeuronInformation neuronInformation = new NeuronInformation();

        public void SetNeuronInformation(Neuron n)
        {
            neuronInformation.isFilled = true;
            neuronInformation.ID = n.ID.ToString();
            neuronInformation.OutputValue = n.Value.ToString();
            neuronInformation.InputValue = n.InputValue.ToString();
            neuronInformation.fireFunc = n.FireFunc.ToString();
            neuronInformation.SynapsenOfNeuron = n.SenderSynapsen;

        }
        public NeuronInformation printNeuronInformation()
        {
            return neuronInformation;
        }




    }
}
