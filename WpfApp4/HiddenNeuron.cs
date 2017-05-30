using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace EMANeuralNetwerk
{
    [Serializable]
    public class HiddenNeuron : Neuron 
    {
        [XmlIgnore]
        private bool isMashedLeftSide;
        [XmlIgnore]
        private bool isMashedRightSide;
        [XmlIgnore]
        public bool IsMashedLeftSide{ get { return isMashedLeftSide; } set { this.isMashedLeftSide = value; } }
        [XmlIgnore]
        public bool  IsMashedRightSide { get { return isMashedRightSide; } set { this.isMashedRightSide = value; } }

    }
}
