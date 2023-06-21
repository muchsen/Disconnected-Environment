using Disconnected_Environment;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace tes_disconeted
{
    public partial class Form2 : Form
    {
        private string stringconnection = "data source=NDP4KOE1\\MUCHSEN" + "database=Disconnected; User ID=Sa; Password=Cencen14";
        private SqlConnection koneksi;
        private void refreshform()
        {
            nmp.Text = ""; nmp.Enabled = false;
            btnSave.Enabled = false; btnClear.Enabled = false;
        }
        public Form2()
        {
            InitializeComponent();
            koneksi = new SqlConnection(stringconnection); refreshform();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
        }
        private void dataGridView1_CellContentClick()
        {
            koneksi.Open(); string str = "select nama_prodi from dbo.prodi1";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi); DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0]; koneksi.Close();
        }
        private void Form2_Load_1(object sender, EventArgs e)
        {
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView1_CellContentClick();
            btnOpen.Enabled = false;
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {
            nmp.Enabled = true;
            btnSave.Enabled = true; btnClear.Enabled = true;
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            string nmprodi = nmp.Text;
            if (nmprodi == "")
            {
                MessageBox.Show("Masukkan Nama Prodi", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                koneksi.Open(); string str = "insert into dbo.prodi1 (nama_prodi)" + "values(@id)";
                SqlCommand cmd = new SqlCommand(str, koneksi); cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new SqlParameter("id", nmprodi)); cmd.ExecuteNonQuery();
                koneksi.Close();
                MessageBox.Show("Data Berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information); dataGridView1_CellContentClick();
                refreshform();
            }
        }
        private void FormDataProdi_FormClosed(object sender, FormClosedEventArgs e)
        {
            Form1 hu = new Form1();
            hu.Show(); this.Hide();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}