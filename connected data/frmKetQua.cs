using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace connected_data
{
    public partial class frmKetQua : Form
    {
        public frmKetQua()
        {
            InitializeComponent();
        }

        private void KetQua_Load(object sender, EventArgs e)
        {
            loadDataGrid();
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

            SqlCommand cmd = new SqlCommand("Select * from KetQua", con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dgvKetQua.DataSource = dt;
        }
        
        void timvitriluoi(string ma)
        {
            int i = 0;
            while (i < dgvKetQua.Rows.Count)
            {
                if (ma.Trim().Equals(dgvKetQua.Rows[i].Cells[0].Value.ToString().Trim()))
                {
                    dgvKetQua.CurrentCell = dgvKetQua.Rows[i].Cells[0];
                    break;
                }
                i++;
            }
        }
        private void HienThiDuLieuTuDataGridViewLenThanhNhap(DataGridViewRow row)
        {
            if (row != null)
            {
                txtDiem.Text = row.Cells["Diem"].Value.ToString();
                txtLanThi.Text = row.Cells["LanThi"].Value.ToString();
                txtMaSV.Text = row.Cells["MaSV"].Value.ToString();
                txtMaMH.Text = row.Cells["MaMH"].Value.ToString();
            }
            else
            {
                txtDiem.Clear();
                txtLanThi.Clear();
                txtMaSV.Clear();
                txtMaMH.Clear();
            }
        }

        private void dgvKQ_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0 && e.RowIndex < dgvKetQua.RowCount - 1)
            {
                DataGridViewRow selectedRow = dgvKetQua.Rows[e.RowIndex];
                HienThiDuLieuTuDataGridViewLenThanhNhap(selectedRow);
            }
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = KetNoiCSDL())
            {
                try
                {
                    string sql = "INSERT INTO KetQua VALUES (@MaSV, @MaMH, @Lanthi,@Diem)";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@MaSV", txtMaSV.Text);
                    cmd.Parameters.AddWithValue("@MaMH", txtMaMH.Text);
                    cmd.Parameters.AddWithValue("@Lanthi", txtLanThi.Text);
                    cmd.Parameters.AddWithValue("@Diem", txtDiem.Text);
                    cmd.ExecuteNonQuery();
                    timvitriluoi(txtMaSV.Text);
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }

            }

            loadDataGrid();
        }

        private void btnSua_Click(object sender, EventArgs e)
        {
            using (SqlConnection con = KetNoiCSDL())
            {
                try
                {
                    string sql = "UPDATE KetQua SET MaMH = @mamh, Lanthi = @lanthi, Diem = @diem WHERE MaSV = @masv";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@mamh", txtMaMH.Text);
                    cmd.Parameters.AddWithValue("@lanthi", txtLanThi.Text);
                    cmd.Parameters.AddWithValue("@diem", txtDiem.Text);
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

        private void btnXoa_Click(object sender, EventArgs e)
        {
            if (dgvKetQua.SelectedRows.Count > 0)
            {
                DialogResult result = MessageBox.Show("Bạn có chắc chắn muốn xóa dòng này không?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.Yes)
                {
                    string maMH = dgvKetQua.SelectedRows[0].Cells[0].Value.ToString();
                    DeleteKQ(maMH);
                    loadDataGrid();
                }
            }
            else
            {
                MessageBox.Show("Vui lòng chọn dòng để xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        private void DeleteKQ(string maMH)
        {
            using (SqlConnection con = KetNoiCSDL())

            {
                try
                {
                    string sql = "DELETE FROM MonHoc WHERE MaMH = @mamh";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@mamh", maMH);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception er)
                {
                    MessageBox.Show("Lỗi" + er.Message);
                }
            }
        }

        private void btnDatlai_Click(object sender, EventArgs e)
        {
            txtDiem.Clear();
            txtLanThi.Clear();
            txtMaMH.Clear();
            txtMaSV.Clear();

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
