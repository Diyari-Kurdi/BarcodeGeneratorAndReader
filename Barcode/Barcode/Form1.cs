using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using Zen.Barcode;

namespace Barcode
{
    public partial class Form1 : Form
    {
        MySqlConnection connection = new MySqlConnection();
        ListBox barcodes = new ListBox();


        internal static string conString = "server=sql2.freemysqlhosting.net;user=sql2362964;port=3306;password=iB4%yR1%;";
        public Form1()
        {
            InitializeComponent();
        }

        private void load() 
        {
            barcodes.Items.Clear();
            connection.ConnectionString = conString;
            connection.Open();
            MySqlCommand cmd = connection.CreateCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT `barcode` FROM `sql2362964`.`table1`;";
            cmd.ExecuteNonQuery();
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            foreach (DataRow dr in dt.Rows)
            {
                barcodes.Items.Add(dr["barcode"].ToString());
            }

            connection.Close();
            dt.Clear();
            connection.Open();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT * FROM `sql2362964`.`table1`;";
            cmd.ExecuteNonQuery();
            da.Fill(dt);
            dataGridView1.DataSource = dt;

            connection.Close();
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            load();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            connection.ConnectionString = conString;
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandType = CommandType.Text;
            command.CommandText = "INSERT INTO `sql2362964`.`table1` (`name`,`price`,`barcode`) VALUES('"+txtName.Text+ "','" + txtPrice.Text+ "','" + txtBarcode.Text+ "'); ";
            command.ExecuteNonQuery();
            connection.Close();
            load();
        }
        int rndNum;
        Random rnd = new Random();
        Code128BarcodeDraw barcode = BarcodeDrawFactory.Code128WithChecksum;

        private void button1_Click(object sender, EventArgs e)
        {
            rndNum = rnd.Next(000000000, 999999999);
            txtBarcode.Text = rndNum.ToString();
            if (!barcodes.Items.Contains(rndNum.ToString()))
            {

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
                    graphics.DrawString(txtBarcode.Text + Environment.NewLine + txtName.Text, font, brush, resultImage.Width / 2, resultImage.Height, format);
                }

                pictureBox1.Image = resultImage;

            }
            else
            {
                while (barcodes.Items.Contains(rndNum.ToString()))
                {
                    rndNum = rnd.Next(000000000, 999999999);
                    txtBarcode.Text = rndNum.ToString();
                }
                if (!barcodes.Items.Contains(rndNum.ToString()))
                {
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
                        graphics.DrawString(txtBarcode.Text + Environment.NewLine + txtName.Text, font, brush, resultImage.Width / 2, resultImage.Height, format);
                    }

                    pictureBox1.Image = resultImage;
                }
            }
        }
    }
}
