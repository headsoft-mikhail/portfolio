using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ZedGraph;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace UsbTest
{
    public partial class DN_View : Form
    {
        public DataSerialazer.MeasData measData;
        public List<MeasDataComplex> measDataComplexList = new List<MeasDataComplex>();
        public MeasDataComplex editedMeasDataComplex;
        public DataSerialazer.MeasData editedMeasData;
        List<string> fileName = new List<string>();
        int startPointNum = 0, stopPointNum = 0, baseFileIndex = 0, ray = 0;
        string[] legend = new string[3] { "P0", "P1", "NK" };
        Color[] colors = new Color[6] { Color.Red, Color.Green, Color.Blue, Color.Magenta, Color.Cyan, Color.Yellow };

        public DN_View()
        {
            InitializeComponent();
            GraphPane paneAmp = zedGraphControlAmpl.GraphPane;
            paneAmp.Title.IsVisible = false;
            paneAmp.Border.IsVisible = false;
            paneAmp.Fill.Color = SystemColors.Control;
            paneAmp.Title.Text = "Amplitude";
            paneAmp.XAxis.Title.Text = "Angle, deg";
            paneAmp.YAxis.Title.Text = "Amplitude, V";
        }

        private void LoadFileButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog1 = new OpenFileDialog();
            openFileDialog1.InitialDirectory = "";
            openFileDialog1.Filter = "Measurement results (*.sav)|*.sav";
            openFileDialog1.Title = "Load file";
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            fileName.Add(openFileDialog1.FileName);
            FileStream fstream = File.Open(fileName[fileName.Count - 1], FileMode.Open);
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            measData = (DataSerialazer.MeasData)binaryFormatter.Deserialize(fstream);
            fstream.Close();
            if (measDataComplexList.Count == 0)
            {
                trackBarStartPoint.Maximum = measData.corners.Length - 1;
                trackBarStopPoint.Maximum = trackBarStartPoint.Maximum;
                buttonAverageAngle.Enabled = true;
                buttonAverageFreq.Enabled = true;
                buttonReplace.Enabled = true;
                buttonSet.Enabled = true;
                buttonAverageSelected.Enabled = true;
                editedMeasDataComplex = new MeasDataComplex(measData);
            }
            if ((measDataComplexList.Count > 0) && (measData.freqs.Length != measDataComplexList[0].freqs.Length) && (measData.corners.Length != measDataComplexList[0].corners.Length))
            {
                MessageBox.Show("Measure formats are not similar. \n File not loaded.", "Error");
                fileName.RemoveAt(fileName.Count - 1);
                return;
            }
            measDataComplexList.Add(new MeasDataComplex(measData));
            trackBarSelectedFile.Maximum = measDataComplexList.Count - 1;
            Plot(measDataComplexList, 0, startPointNum, stopPointNum);
            radioButtonEditCh1.Enabled = true;
            radioButtonEditCh2.Enabled = true;
            radioButtonEditCh3.Enabled = true;
            FreqNumTextBox.Enabled = true;
            FrequencyTextBox.Enabled = true;
            FreqNumTextBox.Text = "0";
            FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[0] / 1000000);
        }

        public void Plot(List<MeasDataComplex> measDataComplex, int freqNum, int startPoint, int stopPoint)
        {
            GraphPane paneAmp = zedGraphControlAmpl.GraphPane;
            paneAmp.CurveList.Clear();
            paneAmp.YAxisList.Clear();
            LineItem line;
            if (radioButtonPlotAmplitude.Checked)
            {
                int axis1 = paneAmp.AddYAxis("Amplitude");
                line = paneAmp.AddCurve("Edited data", editedMeasDataComplex.corners, MeasDataParser.GetCornerArrayAmpl(editedMeasDataComplex, ray, freqNum), Color.Black, SymbolType.None);
                line.Line.Width = 5.5f;
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                line.YAxisIndex = axis1;
                for (int i = 0; i < measDataComplex.Count; i++)
                {
                    line = paneAmp.AddCurve(legend[ray], measDataComplex[i].corners, MeasDataParser.GetCornerArrayAmpl(measDataComplex[i], ray, freqNum), colors[i], SymbolType.None);
                    if (i == baseFileIndex)
                    {
                        line.Line.Width = 2.5f;
                    }
                    if (i == trackBarSelectedFile.Value)
                    {
                        line.Symbol.Type = SymbolType.Circle;
                        line.Symbol.Fill.Type = FillType.Solid;
                        line.Symbol.Size = 6;
                    }
                    line.YAxisIndex = axis1;
                }
                line = paneAmp.AddCurve("", new double[2] { measDataComplex[baseFileIndex].corners[startPoint], measDataComplex[baseFileIndex].corners[stopPoint] },
                                            new double[2] { measDataComplex[baseFileIndex].complex[ray, freqNum, startPoint].Abs, measDataComplex[baseFileIndex].complex[ray, freqNum, stopPoint].Abs },
                                            Color.Gray, SymbolType.Circle);
                line.YAxisIndex = axis1;
                line.Line.IsVisible = false;
                line.Symbol.Fill.Color = Color.Gray;
                line.Symbol.Fill.Type = FillType.Solid;
                line.Symbol.Size = 15;
            }
            else
            {
                int axis2 = paneAmp.AddYAxis("Phase");
                line = paneAmp.AddCurve("Edited data", editedMeasDataComplex.corners, MeasDataParser.GetCornerArrayPhase(editedMeasDataComplex, 2, ray, freqNum), Color.Black, SymbolType.None);
                line.Line.Width = 5.5f;
                line.Line.Style = System.Drawing.Drawing2D.DashStyle.Dot;
                line.YAxisIndex = axis2;
                for (int i = 0; i < measDataComplex.Count; i++)
                {
                    line = paneAmp.AddCurve(legend[ray], measDataComplex[i].corners, MeasDataParser.GetCornerArrayPhase(measDataComplex[i], 2, ray, freqNum), colors[i], SymbolType.None);
                    if (i == baseFileIndex)
                    {
                        line.Line.Width = 2.5f;
                    }
                    if (i == trackBarSelectedFile.Value)
                    {
                        line.Symbol.Type = SymbolType.Circle;
                        line.Symbol.Fill.Type = FillType.Solid;
                        line.Symbol.Size = 6;
                    }
                    line.YAxisIndex = axis2;
                }
                double phase1 = measDataComplex[baseFileIndex].complex[ray, freqNum, startPoint].Arg * 180 / Math.PI;
                double phase2 = measDataComplex[baseFileIndex].complex[ray, freqNum, stopPoint].Arg * 180 / Math.PI;
                while (phase1 < 0)
                    phase1 += 360;
                while (phase2 < 0)
                    phase2 += 360;
                line = paneAmp.AddCurve("", new double[2] { measDataComplex[baseFileIndex].corners[startPoint], measDataComplex[baseFileIndex].corners[stopPoint] },
                                       new double[2] { phase1, phase2 },
                                       Color.Gray, SymbolType.Circle);
                line.YAxisIndex = axis2;
                line.Line.IsVisible = false;
                line.Symbol.Fill.Color = Color.Gray;
                line.Symbol.Fill.Type = FillType.Solid;
                line.Symbol.Size = 15;
                paneAmp.YAxis.Scale.Min = 0;
                paneAmp.YAxis.Scale.Max = 360;
            }

            paneAmp.XAxis.Scale.Min = 0;
            paneAmp.XAxis.Scale.Max = 360;

            paneAmp.XAxis.MajorGrid.IsVisible = true;
            paneAmp.YAxis.MajorGrid.IsVisible = true;
            zedGraphControlAmpl.AxisChange();
            zedGraphControlAmpl.Invalidate();
        }

        private void RayRadioButton_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButtonEditCh1.Checked) ray = 0;
            if (radioButtonEditCh2.Checked) ray = 1;
            if (radioButtonEditCh3.Checked) ray = 2;
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }


        private void FrequencyTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (ContainKey(e.KeyData, Keys.Shift) && ContainKey(e.KeyData, Keys.Down))
            {
                if (Convert.ToInt16(FreqNumTextBox.Text) <= 10)
                    FreqNumTextBox.Text = "0";
                else
                    FreqNumTextBox.Text = (int.Parse(FreqNumTextBox.Text) - 10).ToString();
                Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
                FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[Convert.ToInt16(FreqNumTextBox.Text)] / 1000000);
            }
            if (ContainKey(e.KeyData, Keys.Shift) && ContainKey(e.KeyData, Keys.Up))
            {
                if (Convert.ToInt16(FreqNumTextBox.Text) >= measDataComplexList[0].freqs.Length - 11)
                    FreqNumTextBox.Text = Convert.ToString(measDataComplexList[0].freqs.Length - 1);
                else
                    FreqNumTextBox.Text = (int.Parse(FreqNumTextBox.Text) + 10).ToString();
                Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
                FrequencyTextBox.Text = Convert.ToString(measData.freqs[Convert.ToInt16(FreqNumTextBox.Text)] / 1000000);
            }

            if (e.KeyCode == Keys.Down)
            {
                if (!ContainKey(e.KeyData, Keys.Shift))
                {
                    if (Convert.ToInt16(FreqNumTextBox.Text) <= 1)
                        FreqNumTextBox.Text = "0";
                    else
                        FreqNumTextBox.Text = (int.Parse(FreqNumTextBox.Text) - 1).ToString();
                    Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
                    FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[Convert.ToInt16(FreqNumTextBox.Text)] / 1000000);
                }
            }
            if (e.KeyCode == Keys.Up)
            {
                if (!ContainKey(e.KeyData, Keys.Shift))
                {
                    if (Convert.ToInt16(FreqNumTextBox.Text) >= measDataComplexList[0].freqs.Length - 2)
                        FreqNumTextBox.Text = Convert.ToString(measDataComplexList[0].freqs.Length - 1);
                    else
                        FreqNumTextBox.Text = (int.Parse(FreqNumTextBox.Text) + 1).ToString();
                    Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
                    FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[Convert.ToInt16(FreqNumTextBox.Text)] / 1000000);
                }
            }


            if (e.KeyCode == Keys.Enter)
            {
                if (Convert.ToDouble(FrequencyTextBox.Text) > measDataComplexList[0].freqs[measData.freqs.Length - 1] / 1000000)
                {
                    FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[measData.freqs.Length - 1] / 1000000);
                    FreqNumTextBox.Text = Convert.ToString(measDataComplexList[0].freqs.Length - 1);
                }
                if (Convert.ToDouble(FrequencyTextBox.Text) < measDataComplexList[0].freqs[0] / 1000000)
                {
                    FreqNumTextBox.Text = "0";
                    FrequencyTextBox.Text = Convert.ToString(measData.freqs[0] / 1000000);
                }
                FreqNumTextBox.Text = Convert.ToString(Math.Round(MeasDataParser.FindFreqNumByFreq(measDataComplexList[0], 1000000 * Convert.ToDouble(FrequencyTextBox.Text))));
                FrequencyTextBox.Text = Convert.ToString(measDataComplexList[0].freqs[Convert.ToInt16(FreqNumTextBox.Text)] / 1000000);
                Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
            }
        }

        bool ContainKey(Keys source, Keys key)
        {
            return (source & key) == key;
        }

        private void buttonbuttonSelectBaseFile_Click(object sender, EventArgs e)
        {
            baseFileIndex = trackBarSelectedFile.Value;
            labelSelectedFile.Text = fileName[baseFileIndex];
            for (int k = 0; k < editedMeasDataComplex.corners.Length; k++)
                editedMeasDataComplex.complex[ray, Convert.ToInt16(FreqNumTextBox.Text), k] = measDataComplexList[trackBarSelectedFile.Value].complex[ray, Convert.ToInt16(FreqNumTextBox.Text), k];
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void buttonSelectBaseAtAll_Click(object sender, EventArgs e)
        {
            for (int rayNum = 0; rayNum < editedMeasDataComplex.rays.Length; rayNum++)
            {
                editedMeasDataComplex.rays[rayNum] = measDataComplexList[trackBarSelectedFile.Value].rays[rayNum];
                for (int freqNum = 0; freqNum < editedMeasDataComplex.freqs.Length; freqNum++)
                {
                    editedMeasDataComplex.freqs[freqNum] = measDataComplexList[trackBarSelectedFile.Value].freqs[freqNum];
                    for (int cornerNum = 0; cornerNum < editedMeasDataComplex.corners.Length; cornerNum++)
                    {
                        editedMeasDataComplex.corners[cornerNum] = measDataComplexList[trackBarSelectedFile.Value].corners[cornerNum];
                        editedMeasDataComplex.complex[rayNum, freqNum, cornerNum] = measDataComplexList[trackBarSelectedFile.Value].complex[rayNum, freqNum, cornerNum];
                    }
                }
            }
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            measDataComplexList.Clear();
            editedMeasData = new DataSerialazer.MeasData();
            editedMeasDataComplex = new MeasDataComplex();
            fileName.Clear();
            trackBarSelectedFile.Value = 0;
            trackBarSelectedFile.Maximum = 0;
            trackBarStartPoint.Value = 0;
            trackBarStartPoint.Maximum = 0;
            trackBarStopPoint.Value = 0;
            trackBarStopPoint.Maximum = 0;
            baseFileIndex = 0;
            startPointNum = 0;
            stopPointNum = 0;
            buttonAverageAngle.Enabled = true;
            buttonAverageFreq.Enabled = true;
            buttonReplace.Enabled = true;
            buttonSet.Enabled = true;
            buttonAverageSelected.Enabled = true;
            GraphPane paneAmp = zedGraphControlAmpl.GraphPane;
            paneAmp.CurveList.Clear();
            zedGraphControlAmpl.Invalidate();
        }



        private void buttonReplace_Click(object sender, EventArgs e)
        {
            int length = measDataComplexList[baseFileIndex].corners.Length;
            int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
            int stopPoint = stopPointNum;
            int startPoint = startPointNum;
            if (stopPoint < startPoint)
                stopPoint += length;
            for (int i = startPoint; i <= stopPoint; i++)
            {
                if (i <= length - 1)
                    editedMeasDataComplex.complex[ray, freqNum, i] = measDataComplexList[trackBarSelectedFile.Value].complex[ray, freqNum, i];
                else
                    editedMeasDataComplex.complex[ray, freqNum, i - length] = measDataComplexList[trackBarSelectedFile.Value].complex[ray, freqNum, i - length];
            }
            Plot(measDataComplexList, freqNum, startPointNum, stopPointNum);

        }

        private void buttonAverageSelected_Click(object sender, EventArgs e)
        {
            int length = measDataComplexList[baseFileIndex].corners.Length;
            int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
            int stopPoint = stopPointNum;
            int startPoint = startPointNum;
            if (stopPoint < startPoint)
                stopPoint += length;
            for (int i = startPoint; i <= stopPoint; i++)
            {
                if (i <= length - 1)
                    editedMeasDataComplex.complex[ray, freqNum, i] = 0.5 * (measDataComplexList[trackBarSelectedFile.Value].complex[ray, freqNum, i] + measDataComplexList[baseFileIndex].complex[ray, freqNum, i]);
                else
                    editedMeasDataComplex.complex[ray, freqNum, i - length] = 0.5 * (measDataComplexList[trackBarSelectedFile.Value].complex[ray, freqNum, i - length] + measDataComplexList[baseFileIndex].complex[ray, freqNum, i - length]);
            }
            Plot(measDataComplexList, freqNum, startPointNum, stopPointNum);

        }

        private void buttonAverageFreq_Click(object sender, EventArgs e)
        {
            int length = measDataComplexList[baseFileIndex].corners.Length;
            int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
            if ((freqNum == 0) || (freqNum == length - 1))
            {
                MessageBox.Show("Selected frequency is at the edge of  measured range. \n Averaging aborted.", "Error");
                return;
            }
            for (int i = 0; i < length; i++)
                editedMeasDataComplex.complex[ray, freqNum, i] = 0.5 * (measDataComplexList[baseFileIndex].complex[ray, freqNum - 1, i] + measDataComplexList[baseFileIndex].complex[ray, freqNum + 1, i]);
            Plot(measDataComplexList, freqNum, startPointNum, stopPointNum);
        }

        private void checkBoxSinglePoint_CheckedChanged(object sender, EventArgs e)
        {
            trackBarStopPoint.Value = trackBarStartPoint.Value;
            stopPointNum = trackBarStopPoint.Value;
            startPointNum = trackBarStartPoint.Value;
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void buttonAverageAngle_Click(object sender, EventArgs e)
        {
            int length = measDataComplexList[baseFileIndex].corners.Length;
            int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
            alglib.complex prevVal, nextVal;
            int stopPoint, startPoint;
            if (checkBoxSinglePoint.Checked)
            {
                stopPoint = stopPointNum + 1;
                startPoint = startPointNum - 1;
            }
            else
            {
                stopPoint = stopPointNum;
                startPoint = startPointNum;
            }

            if (startPoint == -1) prevVal = measDataComplexList[baseFileIndex].complex[ray, freqNum, length - 1];
            else prevVal = measDataComplexList[baseFileIndex].complex[ray, freqNum, startPoint];
            if (stopPoint == length) nextVal = measDataComplexList[baseFileIndex].complex[ray, freqNum, 0];
            else nextVal = measDataComplexList[baseFileIndex].complex[ray, freqNum, stopPoint];
            if (stopPoint < startPoint)
                stopPoint += length;
            int range = stopPoint - startPoint;

            if (checkBoxSinglePoint.Checked)
            {
                editedMeasDataComplex.complex[ray, freqNum, startPointNum] = (prevVal + nextVal) / 2;
            }
            else
            {
                for (int i = startPoint; i <= stopPoint; i++)
                {
                    if (i <= length - 1)
                        editedMeasDataComplex.complex[ray, freqNum, i] = prevVal + (nextVal - prevVal) * (i - startPoint) / range;
                    else
                        editedMeasDataComplex.complex[ray, freqNum, i - length] = prevVal + (nextVal - prevVal) * (i - startPoint) / range;
                }
            }


            Plot(measDataComplexList, freqNum, startPointNum, stopPointNum);
        }

        private void trackBarStopPoint_Scroll(object sender, EventArgs e)
        {
            if (checkBoxSinglePoint.Checked)
                trackBarStartPoint.Value = trackBarStopPoint.Value;
            startPointNum = trackBarStartPoint.Value;
            stopPointNum = trackBarStopPoint.Value;
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void trackBarStartPoint_Scroll(object sender, EventArgs e)
        {
            if (checkBoxSinglePoint.Checked)
                trackBarStopPoint.Value = trackBarStartPoint.Value;
            startPointNum = trackBarStartPoint.Value;
            stopPointNum = trackBarStopPoint.Value;
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            editedMeasData = new DataSerialazer.MeasData(editedMeasDataComplex.rays.Length, editedMeasDataComplex.freqs.Length, editedMeasDataComplex.corners.Length);
            editedMeasData.corners = editedMeasDataComplex.corners;
            editedMeasData.freqs = editedMeasDataComplex.freqs;
            editedMeasData.rays = editedMeasDataComplex.rays;
            editedMeasData.description = "edited";
            for (int rayNum = 0; rayNum < editedMeasData.rays.Length; rayNum++)
            {
                for (int freqNum = 0; freqNum < editedMeasData.freqs.Length; freqNum++)
                {
                    for (int cornerNum = 0; cornerNum < editedMeasData.corners.Length; cornerNum++)
                    {
                        editedMeasData.complexRe[rayNum, freqNum, cornerNum] = editedMeasDataComplex.complex[rayNum, freqNum, cornerNum].Re;
                        editedMeasData.complexIm[rayNum, freqNum, cornerNum] = editedMeasDataComplex.complex[rayNum, freqNum, cornerNum].Im;
                    }
                }
            }
            DataSerialazer.DataSerialazer.Save(editedMeasData, textBoxFilename.Text);
        }

        string zedGraph_PointEditEvent(ZedGraphControl sender, GraphPane pane, CurveItem curve, int iPt)
        {
            string str = curve[iPt].X + " " + curve[iPt].Y;
            if (curve.Color == Color.Black)
            {
                int angleNum = (int)MeasDataParser.FindAngleNumByAngle(editedMeasDataComplex, curve[iPt].X);
                int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
                if (radioButtonPlotAmplitude.Checked)
                    editedMeasDataComplex.complex[ray, freqNum, angleNum] = new alglib.complex(curve[iPt].Y, editedMeasDataComplex.complex[ray, freqNum, angleNum].Arg, true);
                else
                    editedMeasDataComplex.complex[ray, freqNum, angleNum] = new alglib.complex(editedMeasDataComplex.complex[ray, freqNum, angleNum].Abs, Math.PI * curve[iPt].Y / 180, true);
            }
            return str;
        }

        private void checkBoxManualAdjust_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxManualAdjust.Checked)
            {
                // при нажатой средней кнопке мыши
                zedGraphControlAmpl.EditButtons = MouseButtons.Middle;
                // ... и клавише Ctrl.
                zedGraphControlAmpl.EditModifierKeys = Keys.Control;
                // Точки можно перемещать
                zedGraphControlAmpl.IsEnableHEdit = false;
                zedGraphControlAmpl.IsEnableVEdit = true;
                // Подпишемся на событие, вызываемое после перемещения точки
                zedGraphControlAmpl.PointEditEvent += new ZedGraphControl.PointEditHandler(zedGraph_PointEditEvent);
            }
            else
            {
                zedGraphControlAmpl.IsEnableHEdit = false;
                zedGraphControlAmpl.IsEnableVEdit = false;
                zedGraphControlAmpl.PointEditEvent -= new ZedGraphControl.PointEditHandler(zedGraph_PointEditEvent);
            }
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void buttonSet_Click(object sender, EventArgs e)
        {
            int length = measDataComplexList[baseFileIndex].corners.Length;
            int freqNum = Convert.ToInt16(FreqNumTextBox.Text);
            int stopPoint = stopPointNum;
            int startPoint = startPointNum;
            if (stopPoint < startPoint)
                stopPoint += length;

            for (int i = startPoint; i <= stopPoint; i++)
            {
                if (i <= length - 1)
                    editedMeasDataComplex.complex[ray, freqNum, i] = new alglib.complex(double.Parse(textBoxAbs.Text), Math.PI * double.Parse(textBoxPhi.Text) / 180, true);
                else
                    editedMeasDataComplex.complex[ray, freqNum, i - length] = new alglib.complex(double.Parse(textBoxAbs.Text), Math.PI * double.Parse(textBoxPhi.Text) / 180, true);
            }
            Plot(measDataComplexList, freqNum, startPointNum, stopPointNum);
        }

        private void checkBoxPlotPhase_CheckedChanged(object sender, EventArgs e)
        {
            Plot(measDataComplexList, Convert.ToInt16(FreqNumTextBox.Text), startPointNum, stopPointNum);
        }

        private void textBoxNumericDot_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 46) e.KeyChar = ',';

            string vlCell = ((TextBox)sender).Text;

            if ((e.KeyChar == 44) && (vlCell.IndexOf(',') == -1))
            {
                return;
            }

            if (!Char.IsDigit(e.KeyChar) && (e.KeyChar != 8) && (e.KeyChar != 127))
                e.Handled = true;
        }
    }
}
