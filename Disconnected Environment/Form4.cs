using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Disconnected_Environment
{

    public partial class Form4 : Form
    {
        private string stringconnection = "data source=NDP4KOE1\\MUCHSEN" + "database=Disconnected; User ID=Sa; Password=Cencen14";
        private SqlConnection koneksi;
        private string kstr;
        public Form4()
        {
            InitializeComponent();
            koneksi = new SqlConnection(kstr);
            refreshform();
        }
        private void refreshform()
        {
            cbNama.Enabled = false;
            cbstatusmahasiswa.Enabled = false;
            cbTahunMasuk.Enabled = false;
            cbNama.SelectedIndex = -1;
            cbstatusmahasiswa.SelectedIndex = -1;
            cbTahunMasuk.SelectedIndex = -1;
            txtNim.Visible = false;
            btnSave.Enabled = false;
            btnClear.Enabled = false;
            btnAdd.Enabled = true;
        }
        private void dataGridView()
        {
            koneksi.Open();
            string str = "select * from dbo.status_mahasiswa";
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            dataGridView1.DataSource = ds.Tables[0];
            koneksi.Close();
        }
        private void nama()
        {
            koneksi.Open();
            string str = "select nama_mahasiswa from dbo.Mahasiswa where " +
                "not EXISTS(select id_status from dbo.status_mahasiswa where)" +
                "status_mahasiswa.nim = mahasiswa.nim";
            SqlCommand cmd = new SqlCommand(str, koneksi);
            SqlDataAdapter da = new SqlDataAdapter(str, koneksi);
            DataSet ds = new DataSet();
            da.Fill(ds);
            cmd.ExecuteReader();
            koneksi.Close();

            cbNama.DisplayMember = "nama_mahasiswa";
            cbNama.ValueMember = "NIM";
            cbNama.DataSource = ds.Tables[0];
        }
        private void TahunMasuk()
        {
            int y = DateTime.Now.Year - 2010;
            string[] type = new string[y];
            int i = 0;
            for (i = 0; i - 1 < type.Length; i++)
            {
                if (i == 0)
                {
                    cbTahunMasuk.Items.Add("2010");
                }
                else
                {
                    int l = 2010 + i;
                    cbTahunMasuk.Items.Add(l.ToString());
                }
            }
        }
        private void cbxNama_SelectedIndexChanged(object sender, EventArgs e)
        {
            koneksi.Open();
            string nim = "";
            string strs = "select NIM from dbo.Mahasiswa where nama_mahasiswa = @nm";
            SqlCommand cm = new SqlCommand(strs, koneksi);
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(new SqlParameter("@nm", cbNama.Text));
            SqlDataReader dr = cm.ExecuteReader();
            while (dr.Read())
            {
                nim = dr["NIM"].ToString();
            }
            dr.Close();
            koneksi.Close();
            txtNim.Text = nim;
        }
        private void Form4_Load(object sender, EventArgs e)
        {

        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            dataGridView();
            btnOpen.Enabled = false;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            cbTahunMasuk.Enabled = true;
            cbNama.Enabled = true;
            cbstatusmahasiswa.Enabled = true;
            txtNim.Visible = true;
            TahunMasuk();
            nama();
            btnClear.Enabled = true;
            btnSave.Enabled = true;
            btnAdd.Enabled = false;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            refreshform();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            string statusMahasiswa = cbstatusmahasiswa.Text;
            string tahunMasuk = cbTahunMasuk.Text;
            int count = 0;
            string tempKodeStatus = "";
            string kodeStatus = "";
            koneksi.Open();

            string str = "select count (*) from dbo.status_mahasiswa";
            SqlCommand cm = new SqlCommand(str, koneksi);
            count = (int)cm.ExecuteScalar();
            if (count == 0)
            {
                kodeStatus = "1";
            }
            else
            {
                string querystring = "select Max(id_status) from dbo.status_mahasiswa";
                SqlCommand cmStatusMahasiswaSum = new SqlCommand(str, koneksi);
                int totalStatusMahasiswa = (int)cmStatusMahasiswaSum.ExecuteScalar();
                int finalKodeStatusInt = totalStatusMahasiswa + 1;
                kodeStatus = Convert.ToString(finalKodeStatusInt);
                {
                    string queryString = "insert into dbo.status_mahasiswa(id_status, nim, status_mahasiswa, tahun_masuk)" +
                "values(@NIM, @sm, @tm)";
                    SqlCommand cmd = new SqlCommand(queryString, koneksi);
                    cmd.CommandType = CommandType.Text;

                    cmd.Parameters.Add(new SqlParameter("ids", kodeStatus));
                    cmd.Parameters.Add(new SqlParameter("sm", statusMahasiswa));
                    cmd.Parameters.Add(new SqlParameter("tm", tahunMasuk));
                    cmd.ExecuteNonQuery();
                    koneksi.Close();

                    MessageBox.Show("Data berhasil Disimpan", "Sukses", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    refreshform();
                    dataGridView();
                }
            }

        }
    }
}
