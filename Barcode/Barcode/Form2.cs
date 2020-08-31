using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Zen.Barcode;

namespace Barcode
{
    public partial class Form2 : Form
    {
        MySqlConnection connection = new MySqlConnection();
        string name;
        internal static string conString = "server=sql2.freemysqlhosting.net;user=sql2362964;port=3306;password=iB4%yR1%;";
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            connection.ConnectionString = conString;
            MySqlCommand command = connection.CreateCommand();
            MySqlDataAdapter da = new MySqlDataAdapter(command);
            DataTable dt = new DataTable();
            connection.Open();
            command.CommandType = CommandType.Text;
            command.CommandText = "SELECT * FROM `sql2362964`.`table1`;";
            command.ExecuteNonQuery();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            connection.Close();
        }
        Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;
        private void Search() 
        {
            try
            {
                connection.Open();
                DataTable dt = new DataTable();

                dt.Rows.Clear();
                MySqlDataAdapter ODA = new MySqlDataAdapter("SELECT * FROM `sql2362964`.`table1` WHERE CONCAT(`id`,`name`,`price`,`barcode`) LIKE '%" + txtBarcode.Text + "%';", connection);
                ODA.Fill(dt);
                dataGridView1.DataSource = dt;
                name = dataGridView1.Rows[dataGridView1.CurrentRow.Index].Cells[1].Value.ToString();
                var barcodeImage = barcode.Draw(txtBarcode.Text, 50);

                var resultImage = new Bitmap(barcodeImage.Width, barcodeImage.Height + 44);

                using (var graphics = Graphics.FromImage(resultImage))
                using (var font = new Font("Microsoft Sans Serif", 9))
                using (var brush = new SolidBrush(Color.Black))
                using (var format = new StringFormat()
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Far
                })
                {
                    graphics.Clear(Color.White);
                    graphics.DrawImage(barcodeImage, 0, 0);
                    graphics.DrawString(Environment.NewLine+name, font, brush, resultImage.Width / 2, resultImage.Height, format);
                }

                pictureBox1.Image = resultImage;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "هەڵە", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Search();
        }

        private void txtBarcode_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter) 
            {
                Search();
            }
        }
    }
}
