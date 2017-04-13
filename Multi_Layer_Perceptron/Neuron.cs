using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Layer_Perceptron
{
    public class Neuron
    {
        /* input neuron */
        public double[] InputValue; //X, Xp

        /* hidden neuron */
        public double DeltaH; //Erro

        /* input, hidden and output neuron */
        public double OutputValue; //InputValue na camada de entrada, activationvalue na oculta e de saída

        /* hidden and output neuron */
        public double[] DeltaWeight; //necessário quando implementado o momentum term (alpha)
        public double Error;

        /* output neuron */
        public double DeltaO; //Erro
    }
}
