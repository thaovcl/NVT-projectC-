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
    public partial class frmKhoa : Form
    {
        public frmKhoa()
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

            SqlCommand cmd = new SqlCommand("Select * from Khoa", con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvKhoa.DataSource = dt;
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void frmKhoa_Load(object sender, EventArgs e)
        {
            loadDataGrid();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = KetNoiCSDL())
            {
                try
                {
                    string sql = "INSERT INTO Khoa VALUES (@MAKHOA, @TENKHOA)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@MAKHOA", txtmakhoa.Text);
                    cmd.Parameters.AddWithValue("@TENKHOA", txttenkhoa.Text);
                    cmd.ExecuteNonQuery();
                    loadDataGrid();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }
            }
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = KetNoiCSDL())
            {
                try
                {
                    string sql = "UPDATE Khoa SET TenKhoa = @tenkh WHERE MaKhoa = @makh";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@tenkh", txttenkhoa.Text);
                    cmd.Parameters.AddWithValue("@makh", txtmakhoa.Text);
                    cmd.ExecuteNonQuery();
                    loadDataGrid();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }
            }
            loadDataGrid();
        }
        private void HienThiDuLieuTuDataGridViewLenThanhNhap(DataGridViewRow row)
        {
            if (row != null)
            {
                txtmakhoa.Text = row.Cells["MaKhoa"].Value.ToString();
                txttenkhoa.Text = row.Cells["TenKhoa"].Value.ToString();
            }
            else
            {
                txtmakhoa.Clear();
                txttenkhoa.Clear();
            }
        }
        private void dgwKhoa_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKhoa.RowCount - 1)
            {
                DataGridViewRow selectedRow = dgvKhoa.Rows[e.RowIndex];
                HienThiDuLieuTuDataGridViewLenThanhNhap(selectedRow);
            }
        }
        private void DeleteKhoa(string maKhoa)
        {
            using (SqlConnection con = KetNoiCSDL())

            {
                try
                {
                    string sql = "DELETE FROM Khoa WHERE MaKhoa = @makh";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@makh", maKhoa);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }
            }
        }
        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKhoa.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string maMH = dgvKhoa.SelectedRows[0].Cells[0].Value.ToString();
                    DeleteKhoa(maMH);
                    loadDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnDatlai_Click(object sender, EventArgs e)
        {
            txtmakhoa.Clear();
            txttenkhoa.Clear();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn thoát khỏi ứng dụng không?", "Xác nhận thoát", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                Application.Exit();
            }
        }
    }
}
