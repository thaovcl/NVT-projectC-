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
    public partial class frmMonHoc : Form
    {
        public frmMonHoc()
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

            SqlCommand cmd = new SqlCommand("Select * from Monhoc", con);
            SqlDataReader dr = cmd.ExecuteReader();
            DataTable dt = new DataTable();
            dt.Load(dr);
            dataGridView1.DataSource = dt;
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            loadDataGrid();
        }

        private void btnThem_Click(object sender, EventArgs e)
        {
            try
            {
                SqlConnection con = KetNoiCSDL();
                string sql = "insert into MonHoc values(@mamh,@tenmh,@sotiet)";
                SqlCommand cmd = new SqlCommand(sql, con);
                cmd.Parameters.AddWithValue("@mamh", txtMaMH.Text);
                cmd.Parameters.AddWithValue("@tenmh", txtTenMH.Text);
                cmd.Parameters.AddWithValue("@sotiet", txtSotiet.Text);
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
            try {
                using (SqlConnection con = KetNoiCSDL())
                {
                    string sql = "UPDATE MonHoc SET TenMH = @tenmh, Sotiet = @sotiet WHERE MaMH = @mamh";
                    SqlCommand cmd = new SqlCommand(sql, con);
                    cmd.Parameters.AddWithValue("@tenmh", txtTenMH.Text);
                    cmd.Parameters.AddWithValue("@sotiet", txtSotiet.Text);
                    cmd.Parameters.AddWithValue("@mamh", txtMaMH.Text);
                    cmd.ExecuteNonQuery();
                }
                loadDataGrid();
            }
            catch (Exception er)
            {
                MessageBox.Show("Lỗi"+ er.Message);
            }
        }

        private void dataGridView1_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            int rowIndex = e.RowIndex;
            if (rowIndex >= 0 && rowIndex < dataGridView1.RowCount - 1)
            {
                txtMaMH.Text = dataGridView1.Rows[rowIndex].Cells[0].Value.ToString();
                txtTenMH.Text = dataGridView1.Rows[rowIndex].Cells[1].Value.ToString();
                txtSotiet.Text = dataGridView1.Rows [rowIndex].Cells[2].Value.ToString();
                
            }
            else
            {
                txtMaMH.Clear();
                txtTenMH.Clear();
                txtSotiet.Clear();
            }
        }

        private void btnXoa_Click(object sender, EventArgs e)
        {
            SqlConnection con = KetNoiCSDL();
            string sql = "delete MonHoc where MaMH=@mamh";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@mamh", txtMaMH.Text);
            
            cmd.ExecuteNonQuery();
            loadDataGrid();
            con.Close();
            con = null;
        }
        private void ResetForm()
        {
            txtMaMH.Clear();
            txtTenMH.Clear();
            txtSotiet.Clear();
            // Có thể thêm các xử lý khác nếu cần reset trạng thái khác của form
        }
        private void btnDatlai_Click(object sender, EventArgs e)
        {
            ResetForm();
        }

        private void btnThoat_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show(
                "Bạn có chắc muốn thoát",
                "Thông báo",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
                Close();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
    }
}
