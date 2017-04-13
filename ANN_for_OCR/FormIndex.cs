using Multi_Layer_Perceptron;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace ANN_for_OCR
{
    public partial class FormIndex : Form
    {
        private List<CheckBox> CbxList;

        private MLP _MLP;

        public FormIndex()
        {
            InitializeComponent();

            this.CbxList = new List<CheckBox>();

            this.CbxList.Add(this.cbxIn11);
            this.CbxList.Add(this.cbxIn12);
            this.CbxList.Add(this.cbxIn13);
            this.CbxList.Add(this.cbxIn14);
            this.CbxList.Add(this.cbxIn15);

            this.CbxList.Add(this.cbxIn21);
            this.CbxList.Add(this.cbxIn22);
            this.CbxList.Add(this.cbxIn23);
            this.CbxList.Add(this.cbxIn24);
            this.CbxList.Add(this.cbxIn25);

            this.CbxList.Add(this.cbxIn31);
            this.CbxList.Add(this.cbxIn32);
            this.CbxList.Add(this.cbxIn33);
            this.CbxList.Add(this.cbxIn34);
            this.CbxList.Add(this.cbxIn35);

            this.CbxList.Add(this.cbxIn41);
            this.CbxList.Add(this.cbxIn42);
            this.CbxList.Add(this.cbxIn43);
            this.CbxList.Add(this.cbxIn44);
            this.CbxList.Add(this.cbxIn45);

            this.CbxList.Add(this.cbxIn51);
            this.CbxList.Add(this.cbxIn52);
            this.CbxList.Add(this.cbxIn53);
            this.CbxList.Add(this.cbxIn54);
            this.CbxList.Add(this.cbxIn55);

            this.CbxList.Add(this.cbxIn61);
            this.CbxList.Add(this.cbxIn62);
            this.CbxList.Add(this.cbxIn63);
            this.CbxList.Add(this.cbxIn64);
            this.CbxList.Add(this.cbxIn65);

            this.btnA.Click += btnChar_Click;
            this.btnB.Click += btnChar_Click;
            this.btnC.Click += btnChar_Click;
            this.btnD.Click += btnChar_Click;
            this.btnE.Click += btnChar_Click;
            this.btnF.Click += btnChar_Click;
            this.btnG.Click += btnChar_Click;
            this.btnH.Click += btnChar_Click;
            this.btnI.Click += btnChar_Click;
            this.btnJ.Click += btnChar_Click;

            this.txtHiddenNeurons.Text = "40";

            this.txtEpochs.Text = "10000";
            this.txtError.Text = "0.001";
            this.txtEta.Text = "0.29";
            this.txtAlpha.Text = "0.8";
            this.btnTrain.Enabled = false;
            this.btnTest.Enabled = false;
            this.btnCopyDebug.Enabled = false;

            this.errorChart.Series.Clear();
            this.errorChart.Series.Add("Erro");
            this.errorChart.Series["Erro"].ChartType = SeriesChartType.Line;
        }

        private void btnChar_Click(object sender, EventArgs e)
        {
            Button cbx = sender as Button;

            OCRChar obj = new OCRChar(cbx.Text.ToCharArray()[0]);
            byte[] drawVet = obj.toDraw();
            this.SetCheckboxes(drawVet);
        }

        private void SetCheckboxes(byte[] _cbxCheckedFlags)
        {
            for (int i = 0; i < this.CbxList.Count(); i++)
            {
                this.CbxList[i].Checked = (_cbxCheckedFlags[i] == 1) ? true : false;
            }
        }

        private double[] GetCheckboxes()
        {
            double[] cbxs = new double[this.CbxList.Count()];
            for (int i = 0; i < this.CbxList.Count(); i++)
            {
                if (this.CbxList[i].Checked)
                {
                    cbxs[i] = 1;
                }
                else
                {
                    cbxs[i] = 0;
                }
            }

            return cbxs;
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                int hiddenLayerSize = int.Parse(this.txtHiddenNeurons.Text, System.Globalization.CultureInfo.InvariantCulture);
                this._MLP = new MLP(30, hiddenLayerSize, 10);
                this.errorChart.Series["Erro"].Points.Clear();
                this.btnTrain.Enabled = true;
                this.lblTotEpochs.Text = "0";
            }
            catch (Exception exc)
            {
                MessageBox.Show(String.Format("Invalid number format. [{0}]", exc.Message));
            }
        }

        private void btnTrain_Click(object sender, EventArgs e)
        {
            //Padrões de Entrada para o Treinamento
            byte[][] DrawMap = OCRChar.DrawMap;
            double[][] DrawMapDouble = new double[DrawMap.Length][];
            for (int i = 0; i < DrawMap.Length; i++)
            {
                DrawMapDouble[i] = new double[DrawMap[i].Length];
                for (int j = 0; j < DrawMap[i].Length; j++)
                {
                    DrawMapDouble[i][j] = DrawMap[i][j];
                }
            }

            //Resultados esperados
            byte[][] BitMap = OCRChar.BitMap;
            double[][] BitMapDouble = new double[BitMap.Length][];
            for (int i = 0; i < BitMap.Length; i++)
            {
                BitMapDouble[i] = new double[BitMap[i].Length];
                for (int j = 0; j < BitMap[i].Length; j++)
                {
                    BitMapDouble[i][j] = BitMap[i][j];
                }
            }

            try
            {
                int epochs = int.Parse(this.txtEpochs.Text, System.Globalization.CultureInfo.InvariantCulture);
                double error = double.Parse(this.txtError.Text, System.Globalization.CultureInfo.InvariantCulture);
                double eta = double.Parse(this.txtEta.Text, System.Globalization.CultureInfo.InvariantCulture);
                double alpha = double.Parse(this.txtAlpha.Text, System.Globalization.CultureInfo.InvariantCulture);

                this._MLP.Training(DrawMapDouble, BitMapDouble, epochs, eta, alpha, error);

                this.errorChart.Series["Erro"].Points.Clear();
                //this.errorChart.ChartAreas[0].AxisY.Maximum = 3;
                this.errorChart.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
                this.errorChart.ChartAreas[0].AxisY.ScaleView.Zoomable = true;
                this.errorChart.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
                
                for (int i = 0; i < this._MLP.Errors.Count(); i++)
                {
                    this.errorChart.Series["Erro"].Points.Add(new DataPoint(i, this._MLP.Errors[i]));
                }

                this.lblTotEpochs.Text = this._MLP.Errors.Count().ToString();
                this.btnTest.Enabled = true;
                this.btnCopyDebug.Enabled = true;
            }
            catch (Exception exc)
            {
                MessageBox.Show(String.Format("Invalid number format. [{0}]", exc.Message));
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            this.txtResult.Text = "";

            double[] applyin = this.GetCheckboxes();

            double[] returnData;

            returnData = this._MLP.NormalizedApply(applyin);

            OCRChar oc = new OCRChar(returnData);
            this.txtResult.Text += "Corresponding letter: " + oc.toChar();
            this.txtResult.Text += Environment.NewLine + Environment.NewLine;

            this.txtResult.Text += "Output vector:" + Environment.NewLine + "[" + String.Join(", ", returnData) + "]";
            this.txtResult.Text += Environment.NewLine + Environment.NewLine;

            returnData = this._MLP.Apply(applyin);
            this.txtResult.Text += "Output values:" + Environment.NewLine + String.Join(Environment.NewLine, returnData);

            this.TesteComposto();
        }

        private void TesteComposto()
        {
            this.txtResult.Text += Environment.NewLine + Environment.NewLine + Environment.NewLine;

            this.txtResult.Text += "Test: " + Environment.NewLine;

            char[] letras = OCRChar.CharMap;
            for (int i = 0; i < OCRChar.CharMap.Length; i++)
            {
                OCRChar oc = new OCRChar(OCRChar.CharMap[i]);
                double[] retorno = this._MLP.Apply(oc.toDrawDoubleMap());

                this.txtResult.Text += "Letter: " + oc.toChar() + Environment.NewLine;
                this.txtResult.Text += "Output values:" + Environment.NewLine + String.Join(Environment.NewLine, retorno);
                this.txtResult.Text += Environment.NewLine + Environment.NewLine;
            }
        }

        private void btnCopyDebug_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(String.Join(Environment.NewLine, this._MLP.Errors));
        }

        private void txtResult_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control & e.KeyCode == Keys.A)
                this.txtResult.SelectAll();
            return;
        }
    }
}
