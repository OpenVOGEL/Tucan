#region Copyright © 2009, De Santiago-Castillo JA. All rights reserved.

//Copyright © 2009 Jose Antonio De Santiago-Castillo 
//E-mail:JAntonioDeSantiago@gmail.com
//Web: www.DotNumerics.com
//
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using DotNumerics.FortranLibrary;

namespace DotNumerics.LinearAlgebra
{
    /// <summary>
    /// Displays and edit a matrix in a grid.
    /// </summary>
    [ToolboxItem(true)]
    [DesignTimeVisible(true)]
    public partial class MatrixView : UserControl
    {
        #region Fields
        private BaseMatrix _RealMatrix;
        private ComplexMatrix _ComplexMatrix;
        private bool _IsRealMatrix = true;
        private bool _ReadOnly = false;
        private string _Format = "0.000";
        private IFormatProvider MeFormatProvider = System.Globalization.CultureInfo.CurrentUICulture;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the MatrixView class.
        /// </summary>
        public MatrixView()
        {
            InitializeComponent();

            this.dataGridView1.ReadOnly = this._ReadOnly;
            this.dataGridView1.DefaultCellStyle.Format = this._Format;
            this.dataGridView1.DefaultCellStyle.FormatProvider = this.MeFormatProvider;

            this.dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;

            this.dataGridView1.CellEndEdit += new DataGridViewCellEventHandler(dataGridView1_CellEndEdit);
            this.dataGridView1.CellParsing += new DataGridViewCellParsingEventHandler(dataGridView1_CellParsing);
            //this.dataGridView1.RowsAdded += new DataGridViewRowsAddedEventHandler(dataGridView1_RowsAdded);
        }



        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the user can edit the cells of the control.
        /// </summary>
        public bool ReadOnly
        {
            get { return _ReadOnly; }
            set 
            { 
                _ReadOnly = value;
                this.dataGridView1.ReadOnly = this._ReadOnly;
            }
        }

        /// <summary>
        /// Gets or sets the object used to provide culture-specific formatting of System.Windows.Forms.DataGridView cell values.
        /// </summary>
        public IFormatProvider FormatProvider
        {
            get { return MeFormatProvider; }
            set 
            { 
                MeFormatProvider = value;
                this.dataGridView1.DefaultCellStyle.FormatProvider = this.MeFormatProvider;
            }
        }

        /// <summary>
        /// Gets or sets the format string applied to the textual content of a cell.
        /// </summary>
        public string Format
        {
            get { return _Format; }
            set 
            { 
                _Format = value;
                this.dataGridView1.DefaultCellStyle.Format = this._Format;
            }
        }

        #endregion 

        #region Events


        //void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        //{
        //    dataGridView1.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();

        //    //m_Grid.Rows[e.RowIndex].HeaderCell.Value = e.RowIndex.ToString();

        //}

        void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (this._IsRealMatrix)
                {
                    this._RealMatrix[e.RowIndex, e.ColumnIndex] = (double)this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                }
                else
                {
                    this._ComplexMatrix[e.RowIndex, e.ColumnIndex] = (Complex)this.dataGridView1[e.ColumnIndex, e.RowIndex].Value;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        void dataGridView1_CellParsing(object sender, DataGridViewCellParsingEventArgs e)
        {
            if (e != null && e.Value != null)
            {
                if (this._IsRealMatrix)
                {
                    double newVal = 0f;
                    if (double.TryParse(e.Value.ToString(), out newVal) == true)
                    {
                        e.ParsingApplied = true;
                        e.Value = newVal;
                    }
                    else
                    {
                        MessageBox.Show(this, "The value must be a number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Value = 1d;
                        e.ParsingApplied = true;
                    }
                }
                else
                {
                    Complex newVal;
                    if (this.TryParseComplex(e.Value.ToString(), out newVal) == true)
                    {
                        e.ParsingApplied = true;
                        e.Value = newVal;
                    }
                    else
                    {
                        MessageBox.Show(this, "The value must be a complex number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        e.Value = new Complex(1, 0);
                        e.ParsingApplied = true;
                    }
                }
            }
        }


        #endregion


        #region Methods

        /// <summary>
        /// Sets the matrix to show.
        /// </summary>
        /// <param name="matrix">A real matrix.</param>
        public void Matrix(BaseMatrix matrix)
        {
            this._RealMatrix = matrix;
            this._IsRealMatrix = true;

            this._ComplexMatrix = new ComplexMatrix(1);

            this.ShowMatrix();

        }
        /// <summary>
        /// Sets the matrix to show.
        /// </summary>
        /// <param name="matrix">A complex matrix.</param>
        public void Matrix(ComplexMatrix matrix)
        {
            this._ComplexMatrix = matrix;
            this._IsRealMatrix = false;

            this._RealMatrix = new Matrix(1);

            this.ShowMatrix();
        }

        /// <summary>
        /// Remove the matrix.
        /// </summary>
        public void RemoveMatrix()
        {
            this._ComplexMatrix = new ComplexMatrix(1);
            this._ComplexMatrix = new ComplexMatrix(1);
            this.dataGridView1.DataSource = null;
        }

        /// <summary>
        /// Refreshes the value of the matrix.
        /// </summary>
        private void RefreshMatrix()
        {
            this.ShowMatrix();
        }

        private void ShowMatrix()
        {
            this.dataGridView1.DataSource = null;

            DataTable table = new DataTable();

            if (this._IsRealMatrix == true && this._RealMatrix != null)
            {
                for (int i = 0; i < this._RealMatrix.ColumnCount; i++)
                {
                    DataColumn col = table.Columns.Add(i.ToString());
                    col.DataType = typeof(System.Double);
                }

                for (int i = 0; i < this._RealMatrix.RowCount; i++)
                {
                    DataRow row = table.NewRow();
                    for (int j = 0; j < this._RealMatrix.ColumnCount; j++)
                    {
                        row[j] = this._RealMatrix[i, j];
                    }
                    table.Rows.Add(row);
                }
            }
            else if (this._ComplexMatrix != null)
            {
                for (int i = 0; i < this._ComplexMatrix.ColumnCount; i++)
                {
                    DataColumn col = table.Columns.Add(i.ToString());
                    col.DataType = typeof(Complex);
                }

                for (int i = 0; i < this._ComplexMatrix.RowCount; i++)
                {
                    DataRow row = table.NewRow();
                    for (int j = 0; j < this._ComplexMatrix.ColumnCount; j++)
                    {
                        row[j] = this._ComplexMatrix[i, j];
                    }
                    table.Rows.Add(row);
                }

            }

            DataSet data = new DataSet();
            data.Tables.Add(table);

            this.dataGridView1.DataSource = table;

            //for (int i = 0; i < dataGridView1.Rows.Count; i++)
            //{
            //    dataGridView1.Rows[i].HeaderCell.Value = i.ToString();
            //}

            foreach (DataGridViewColumn column in this.dataGridView1.Columns)
            {
                column.SortMode = DataGridViewColumnSortMode.NotSortable;
            }

        }


        private Complex ParseComplex(string s)
        {
            Complex c;

            TryParseComplex(s, out c);

            return c;
        }


        private bool TryParseComplex(string s, out Complex result)
        {

            bool isComplex = false;
            result = new Complex(1, 0);
            //+45.3434-233.4i, 45.3434-233.4i, -45.3434-233.4i
            //45, +45, 45i, -45i, i.+1,-i

            if (s == null || s.Length == 0) return false;

            s = s.ToLower();  // Para que sea valido para 3+5I I enlugar de i

            if (s[s.Length - 1] == 'i')
            {
                isComplex = true;
            }
            else
            {
                isComplex = false;
            }

            if (isComplex == false) // En este caso no se tiene parte imaginaria (s = 45, +45)
            {
                double real;
                if (double.TryParse(s, out real) == true)
                {
                    result = new Complex(real, 0);
                    return true;
                }
                else return false;
            }
            else  // Si tiene la parte imaginaria se revisa que tenga la parte real
            {
                if (s.Contains("+") == false && s.Contains("-") == false)  // En este caso solo tiene la parte imaginaria y es positiva (s= 45i, i)
                {
                    if (s == "i") // Si es i
                    {
                        result = new Complex(0, 1);
                        return true;
                    }
                    else
                    {
                        s = s.Remove(s.Length - 1);  // Se elimina la i
                        double imag;
                        if (double.TryParse(s, out imag) == true)
                        {
                            result = new Complex(0, imag);
                            return true;
                        }
                        else return false;
                    }
                }
                else
                {
                    s = s.Remove(s.Length - 1);  // Se elimina la i

                    string sign = "+";
                    //if (s.Contains("+") == true && s.Contains("-") == false) // 5+6i , +6+4i
                    //{
                    //    sign = "+";
                    //}
                    //else if (s.Contains("+") == false && s.Contains("-") == true) // 5-6i , -6-4i
                    //{
                    //    sign = "-";
                    //}
                    //else if (s.Contains("+") == true && s.Contains("-") == true) // +5-i, -5+i Se debe determinar cual signo divede la parte real y la parte imaginaria
                    //{

                    //Se debe determinar cual signo divede la parte real y la parte imaginaria
                    int minosIndex = s.LastIndexOf("-");
                    int plusIndex = s.LastIndexOf("+");

                    if (plusIndex > minosIndex) sign = "+";
                    else sign = "-";
                    //}

                    int signoIndex = s.LastIndexOf(sign);

                    if (signoIndex == 0) //En este caso solo tiene la parte imaginaria (s= +45i, -i, +i)
                    {
                        if (s.Length == 1) s = s + "1";  // solo se tiene "+" o "-" asi que se pone "+1" o "-1"

                        double imag;
                        if (double.TryParse(s, out imag) == true)
                        {
                            result = new Complex(0, imag);
                            return true;
                        }
                        else return false;
                    }
                    else
                    {
                        string sReal = s.Substring(0, signoIndex);
                        string sIm = s.Substring(signoIndex);

                        if (sIm.Length == 1) sIm = sIm + "1"; // solo se tiene "+" o "-" asi que se pone "+1" o "-1"

                        double imag;
                        double real;
                        if (double.TryParse(sIm, out imag) == true && double.TryParse(sReal, out real) == true)
                        {
                            result = new Complex(real, imag);
                            return true;
                        }
                        else return false;
                    }
                }

            }

            //return isComplex;
        }


        #endregion


    }
}
