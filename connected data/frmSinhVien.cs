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

namespace connected_data
{
    public partial class frmSinhVien : Form
    {
        public frmSinhVien()
        {
            InitializeComponent();
        }
        SqlConnection KetNoiCSDL()
        {
            string strCon = @"Data source=PC\ANHYEUEM; Initial Catalog =QLDiem; Integrated Security=true";
            SqlConnection con = new SqlConnection(strCon);
            con.Open();
            return con;
        }
        void loadDataGrid()
        {
            SqlConnection con = KetNoiCSDL();

            SqlCommand cmd = new SqlCommand("Select * from SinhVien", con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvSinhVien.DataSource = dt;
        }
        void loadDataCombo()
        {
            SqlConnection con = KetNoiCSDL();

            SqlCommand cmd = new SqlCommand("Select * from SinhVien", con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            cmbKhoa.DataSource = dt;
            cmbKhoa.DisplayMember = "TenKhoa";
            cmbKhoa.ValueMember = "MaKhoa";
            cmbKhoa.SelectedIndex = -1;
        }
        void timvitri(string MaKhoa)
        {
            int i = 0;
            cmbKhoa.SelectedIndex = 0;
            while (!cmbKhoa.SelectedValue.ToString().Equals(MaKhoa))
            {
                i++;
                cmbKhoa.SelectedIndex = i;
            }
        }
        private void frmKhoa_Load(object sender, EventArgs e)
        {
            loadDataCombo();
            loadDataGrid();
            
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = KetNoiCSDL();
                string sql = "insert into SinhVien values(@masv,@hosv,@tensv,@phai,@ngaysinh,@makhoa)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@masv", txtMaSV.Text);
                cmd.Parameters.AddWithValue("@hosv", txtHoSV.Text);
                cmd.Parameters.AddWithValue("@tensv", txtTenSV.Text);
                cmd.Parameters.AddWithValue("@phai", radNam.Checked ? "Nam" : "Nữ");
                cmd.Parameters.AddWithValue("@ngaysinh", dpkNgaySinh.Value);
                cmd.Parameters.AddWithValue("@makhoa", cmbKhoa.SelectedValue);
                cmd.ExecuteNonQuery();
                loadDataGrid();
            }
            catch (Exception er)
            {
                MessageBox.Show("Lỗi" + er.Message);
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = KetNoiCSDL();
                string sql = "update SinhVien set MaSV=@masv, ";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@masv", txtMaSV.Text);
                cmd.Parameters.AddWithValue("@hosv", txtHoSV.Text);
                cmd.Parameters.AddWithValue("@tensv", txtTenSV.Text);
                cmd.Parameters.AddWithValue("@phai", radNam.Checked ? "Nam" : "Nữ");
                cmd.Parameters.AddWithValue("@ngaysinh", dpkNgaySinh.Value);
                cmd.Parameters.AddWithValue("@makhoa", cmbKhoa.SelectedValue);
                cmd.ExecuteNonQuery();
                loadDataGrid();
            }
            catch (Exception er)
            {
                MessageBox.Show("Lỗi" + er.Message);
            }
        }
        void datlai()
        {
            txtMaSV.Clear();
            txtHoSV.Clear();
            txtTenSV.Clear();
            radNam.Checked = false;
            radNu.Checked = false;
            cmbKhoa.SelectedIndex = -1;
        }

        private void dgvSinhVien_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int i = e.RowIndex;
            if (i < dgvSinhVien.RowCount - 1)
            {
                txtMaSV.Text = dgvSinhVien.Rows[i].Cells[0].Value.ToString();
                txtHoSV.Text = dgvSinhVien.Rows[i].Cells[1].Value.ToString();
                txtTenSV.Text = dgvSinhVien.Rows[i].Cells[2].Value.ToString();

                string phai = dgvSinhVien.Rows[i].Cells[3].Value.ToString().Trim();
                if (phai.ToLower().Equals("nam"))//truyền chữ thường
                    radNam.Checked = true;
                else
                    radNu.Checked=true;
                dpkNgaySinh.Value = DateTime.Parse(dgvSinhVien.Rows[i].Cells[4].Value.ToString());
                timvitri(dgvSinhVien.Rows[i].Cells[5].Value.ToString());
            }

        }
        void timvitriluoi(string ma)
        {
            int i = 0;
            while (i < dgvSinhVien.Rows.Count)
            {
                if (ma.Trim().Equals(dgvSinhVien.Rows[i].Cells[0].Value.ToString().Trim()))
                {
                    dgvSinhVien.CurrentCell = dgvSinhVien.Rows[i].Cells[0];
                    break;
                }
                i++;
            }
        }
            private void btnXoa_Click(object sender, EventArgs e)
        {

            using (SqlConnection con = KetNoiCSDL())
            {
                try
                {
                    string sql = "delete from Sinhvien where MaSV = @MaSV";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@MASV", txtMaSV.Text);
                    cmd.ExecuteNonQuery();
                    loadDataGrid();
                    timvitriluoi(txtMaSV.Text);
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }
            }


        }

        private void btnDatlai_Click(object sender, EventArgs e)
        {
            datlai();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            {
                DialogResult result = MessageBox.Show(
                    "Bạn có chắc muốn thoát",
                    "Thông báo",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                    Close();
            }
        }
    }
}
