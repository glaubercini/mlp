using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Multi_Layer_Perceptron
{
    public class MLP
    {
        private Random Rand = new Random(DateTime.Now.Millisecond);

        private int InputLayerSize;
        private int HiddenLayerSize;
        private int OutputLayerSize;

        private Neuron[] InputLayer;
        private Neuron[] HiddenLayer;
        private Neuron[] OutputLayer;

        private int Bias = 1;

        public List<double> Errors = new List<double>();
        
        public MLP(int inputLayerSize, int hiddenLayerSize, int outputLayerSize)
        {
            this.InputLayerSize = inputLayerSize;
            this.HiddenLayerSize = hiddenLayerSize;
            this.OutputLayerSize = outputLayerSize;

            this.Inicialize();
        }

        private void Inicialize()
        {
            this.InputLayer = new Neuron[this.InputLayerSize + 1];

            this.HiddenLayer = new Neuron[this.HiddenLayerSize + 1];

            this.OutputLayer = new Neuron[this.OutputLayerSize];

            //Cria camada de input
            Neuron bias_input_neuron = new Neuron();
            bias_input_neuron.InputValue = new double[1] { this.Bias };
            bias_input_neuron.OutputValue = this.Bias;
            this.InputLayer[0] = bias_input_neuron;

            for (int i = 1; i <= this.InputLayerSize; i++)
            {
                Neuron ni = new Neuron();
                ni.InputValue = new double[1];
                this.InputLayer[i] = ni;
            }

            //Cria camada oculta
            Neuron bias_hidden_neuron = new Neuron();
            bias_hidden_neuron.InputValue = new double[1] { this.Bias };
            bias_hidden_neuron.OutputValue = this.Bias;
            this.HiddenLayer[0] = bias_hidden_neuron;
            for (int j = 1; j <= this.HiddenLayerSize; j++)
            {
                Neuron nh = new Neuron();
                nh.InputValue = new double[this.InputLayer.Length];
                nh.DeltaWeight = new double[this.InputLayer.Length];
                this.HiddenLayer[j] = nh;
            }

            //Cria camada de saída
            for (int k = 0; k < this.OutputLayerSize; k++)
            {
                Neuron no = new Neuron();
                no.InputValue = new double[this.HiddenLayer.Length];
                no.DeltaWeight = new double[this.HiddenLayer.Length];
                this.OutputLayer[k] = no;
            }

            //Crio os pesos da camada de entrada para a camada oculta
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                for (int i = 0; i < this.InputLayer.Length; i++)
                {
                    nh.InputValue[i] = this.RandomNumber();
                    nh.DeltaWeight[i] = 0;
                }
            }

            for (int k = 1; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                for (int j = 0; j < this.HiddenLayer.Length; j++)
                {
                    no.InputValue[j] = this.RandomNumber();
                    no.DeltaWeight[j] = 0;
                }
            }
        }

        public void XORTest()
        {
            this.HiddenLayer[1].InputValue[0] = 0.2916;
            this.HiddenLayer[1].InputValue[1] = 0.1325;
            this.HiddenLayer[1].InputValue[2] = -0.1851;
                
            this.HiddenLayer[2].InputValue[0] = -0.0923;
            this.HiddenLayer[2].InputValue[1] = 0.1547;
            this.HiddenLayer[2].InputValue[2] = -0.1232;

            this.OutputLayer[0].InputValue[0] = -0.1923;
            this.OutputLayer[0].InputValue[1] = 0.1432;
            this.OutputLayer[0].InputValue[2] = 0.1762;
        }

        private void FeedForward(double[] inputValues)
        {
            //O valor de input e output é o mesmo para a camada de entrada
            for (int i = 1; i < this.InputLayer.Length; i++)
            {
                this.InputLayer[i].InputValue = new double[1] { inputValues[i-1] };
                this.InputLayer[i].OutputValue = inputValues[i-1];
            }

            //Calcular o net e output dos neuronios ocultos
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                double WeightedSum = 0;
                for (int i = 0; i < this.InputLayer.Length; i++)
                {
                    WeightedSum += nh.InputValue[i] * this.InputLayer[i].OutputValue;
                }
                nh.OutputValue = this.SigmoidFunction(WeightedSum);
            }

            //Calcular o net e output dos neurônios de saída
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                double WeightedSum = 0;
                for (int j = 0; j < this.HiddenLayer.Length; j++)
                {
                    WeightedSum += no.InputValue[j] * this.HiddenLayer[j].OutputValue;
                }
                no.OutputValue = this.SigmoidFunction(WeightedSum);
            }
        }

        private double ErrorCalculate(double[] desiredValues)
        {
            double error = 0;
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                double dv = desiredValues[k];
                //error += dv - this.OutputLayer[k].OutputValue;
                error += 0.5 * Math.Pow((dv - this.OutputLayer[k].OutputValue), 2);
            }

            return error;
        }

        [Obsolete("BackPropagation is deprecated, please use BackPropagationAlpha instead.")]
        private void BackPropagation(double[] desiredValues, double eta)
        {
            //Calcula o Delta da camada de saída
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                double dv = desiredValues[k];
                no.Error = (dv - no.OutputValue);
                no.DeltaO = no.Error * no.OutputValue * (1 - no.OutputValue);
            }

            /*
            // Método exercício professor
            //Soma ponderada do delta_o pelo peso
            double SumOH = 0;
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                for (int j = 0; j < this.HiddenLayer.Length; j++)
                {
                    SumOH += no.DeltaO * no.InputValue[j];
                }
            }

            //Calcula o delta_h
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                nh.DeltaH = nh.OutputValue * (1 - nh.OutputValue) * SumOH;
            }
            */

            double SumOH = 0;
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                for (int k = 0; k < this.OutputLayer.Length; k++)
                {
                    Neuron no = this.OutputLayer[k];
                    SumOH += no.DeltaO * no.InputValue[j];
                }

                nh.DeltaH = nh.OutputValue * (1 - nh.OutputValue) * SumOH;
            }

            //Atualizo os pesos da camada oculta -> camada de saída
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                for (int j = 0; j < this.HiddenLayer.Length; j++)
                {
                    Neuron nh = this.HiddenLayer[j];
                    no.InputValue[j] = no.InputValue[j] + (eta * no.DeltaO * nh.OutputValue);
                }
            }

            //Atualizo os pesos da camada de entrada -> camada oculta
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                for (int i = 0; i < this.InputLayer.Length; i++)
                {
                    Neuron ni = this.InputLayer[i];
                    nh.InputValue[i] = nh.InputValue[i] + (eta * nh.DeltaH * ni.OutputValue);
                }
            }
        }
        
        private void BackPropagationAlpha(double[] desiredValues, double eta, double alpha)
        {
            //Calcula o Delta da camada de saída
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                double dv = desiredValues[k];
                no.Error = (dv - no.OutputValue);
                no.DeltaO = no.Error * no.OutputValue * (1 - no.OutputValue);
            }

            double SumOH = 0;
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                for (int k = 0; k < this.OutputLayer.Length; k++)
                {
                    Neuron no = this.OutputLayer[k];
                    SumOH += no.DeltaO * no.InputValue[j];
                }

                nh.DeltaH = nh.OutputValue * (1 - nh.OutputValue) * SumOH;
            }

            //Atualizo os pesos da camada oculta -> camada de saída
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                Neuron no = this.OutputLayer[k];
                no.DeltaWeight[0] = (eta * no.DeltaO) + (alpha * no.DeltaWeight[0]);
                no.InputValue[0] += no.DeltaWeight[0];
                for (int j = 1; j < this.HiddenLayer.Length; j++)
                {
                    Neuron nh = this.HiddenLayer[j];
                    no.DeltaWeight[j] = (eta * no.DeltaO * nh.OutputValue) + (alpha * no.DeltaWeight[j]);
                    no.InputValue[j] += no.DeltaWeight[j];
                }
            }

            //Atualizo os pesos da camada de entrada -> camada oculta
            for (int j = 1; j < this.HiddenLayer.Length; j++)
            {
                Neuron nh = this.HiddenLayer[j];
                nh.DeltaWeight[0] = (eta * nh.DeltaH) + (alpha * nh.DeltaWeight[0]);
                nh.InputValue[0] += nh.DeltaWeight[0];
                for (int i = 1; i < this.InputLayer.Length; i++)
                {
                    Neuron ni = this.InputLayer[i];
                    nh.DeltaWeight[i] = (eta * nh.DeltaH * ni.OutputValue) + (alpha * nh.DeltaWeight[i]);
                    nh.InputValue[i] += nh.DeltaWeight[i];
                }
            }
        }

        private void DebugWeights(int epochs)
        {
            System.Diagnostics.Debug.WriteLine("Época: " + epochs);
            System.Diagnostics.Debug.WriteLine("Input Layer -> Hidden Layer:");
            for (int i = 0; i < this.InputLayer.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine(String.Join(", ", this.InputLayer[i].InputValue));
            }

            System.Diagnostics.Debug.WriteLine("Hidden Layer -> Output Layer:");
            for (int i = 0; i < this.HiddenLayer.Count(); i++)
            {
                System.Diagnostics.Debug.WriteLine(String.Join(", ", this.HiddenLayer[i].InputValue));
            }
        }

        //eta é a taxa de aprendizado
        public void Training(double[][] vetInputValues, double[][] vetDesiredValues, int epochs, double eta, double alpha, double limit_error)
        {
            for (int p = 0; p < epochs; p++)
            {
                double error_sum = 0;
                for (int v = 0; v < vetInputValues.Length; v++)
                {
                    this.FeedForward(vetInputValues[v]);
                    error_sum += this.ErrorCalculate(vetDesiredValues[v]);
                    //this.BackPropagation(vetDesiredValues[v], eta);
                    this.BackPropagationAlpha(vetDesiredValues[v], eta, alpha);
                }

                //this.DebugWeights(p);

                this.Errors.Add(error_sum);

                if (error_sum < limit_error)
                {
                    break;
                }
            }
        }

        public double[] Apply(double[] inputValues)
        {
            this.FeedForward(inputValues);
            double[] output = new double[this.OutputLayer.Length];
            for (int k = 0; k < this.OutputLayer.Length; k++)
            {
                output[k] = this.OutputLayer[k].OutputValue;
            }

            return output;
        }

        public double[] NormalizedApply(double[] inputValues)
        {
            this.FeedForward(inputValues);
            double[] output = new double[this.OutputLayer.Length];
            int big = 0;
            double temp = this.OutputLayer[0].OutputValue;
            for (int k = 1; k < this.OutputLayer.Length; k++)
            {
                if (this.OutputLayer[k].OutputValue > temp)
                {
                    big = k;
                    temp = this.OutputLayer[k].OutputValue;
                }
                output[k] = 0;
            }

            output[big] = 1;

            return output;
        }

        private double SigmoidFunction(double x)
        {
            double fx = 1d / (1d + Math.Exp(-x));

            return fx;
        }

        private double RandomNumber()
        {
            double min = 0.001;
            double max = 0.01;

            double rn = this.Rand.NextDouble() * (max - min) + min;

            return rn;
        }
    }
}
