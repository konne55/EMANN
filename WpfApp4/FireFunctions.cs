using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMANeuralNetwerk
{
    [Serializable]
    class FireFunc
    {

    
        public float Sigmoid(float v)
        {
            return 1 / (1 + (float)Math.Exp(-v));
        }
        public float SigmoidDerivative(float v)
        {
            return v * (1 - v);
        }

        public float Identity(float v)
        {
            return v;
        }

        public float positiveIdentity(float v)
        {
            if (v > 0)
            {
                return v;
            }
            else
            {
                return 0;
            }
        }


    }
}
