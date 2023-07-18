using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Collections.Generic;
using Newtonsoft.Json;


namespace WinFormsApp2
{


    public partial class App : Form
    {
        static HttpClient client = new HttpClient();
        public string basUrl = "http://127.0.0.1:8000";
        public int index;

        private byte[] blankImage = new byte[] {
        0x42, 0x4D, 0xC6, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x76,
        0x0, 0x0, 0x0, 0x28, 0x0, 0x0, 0x0, 0xB, 0x0, 0x0, 0x0, 0xA,
        0x0, 0x0, 0x0, 0x1, 0x0, 0x4, 0x0, 0x0, 0x0, 0x0, 0x0, 0x50,
        0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x10,
        0x0, 0x0, 0x0, 0x10, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0, 0x0,
        0x0, 0x80, 0x0, 0x0, 0x80, 0x0, 0x0, 0x0, 0x80, 0x80, 0x0,
        0x80, 0x0, 0x0, 0x0, 0x80, 0x0, 0x80, 0x0, 0x80, 0x80, 0x0,
        0x0, 0xC0, 0xC0, 0xC0, 0x0, 0x80, 0x80, 0x80, 0x0, 0x0, 0x0,
        0xFF, 0x0, 0x0, 0xFF, 0x0, 0x0, 0x0, 0xFF, 0xFF, 0x0, 0xFF,
        0x0, 0x0, 0x0, 0xFF, 0x0, 0xFF, 0x0, 0xFF, 0xFF, 0x0, 0x0,
        0xFF, 0xFF, 0xFF, 0x0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0,
        0x0, 0x0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xF0, 0x0, 0x0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0,
        0x0, 0x0, 0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0,
        0xFF, 0xFF, 0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0, 0xFF, 0xFF,
        0xFF, 0xFF, 0xFF, 0xF0, 0x0, 0x0, 0xFF, 0xFF, 0xFF, 0xFF,
        0xFF, 0xF0, 0x0, 0x0};
        public App()
        {
            InitializeComponent();

        }

        private async void App_Load(object sender, EventArgs e)
        {
            dataGridView1.CellClick += new
            DataGridViewCellEventHandler(dataGridView1_CellClick);
            dataGridView1.CellEndEdit += new
            DataGridViewCellEventHandler(dataGridView1_CellEndEdit);

            dataGridView1.CellFormatting += dataGridView1_CellFormatting;
            dataGridView1.Columns[0].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[4].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            dataGridView1.Columns[5].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;

            Bitmap blank = new Bitmap(new MemoryStream(this.blankImage));
            DataGridViewImageColumn dgvImage = new DataGridViewImageColumn();
            dgvImage.HeaderText = "Image";
            dgvImage.ImageLayout = DataGridViewImageCellLayout.Stretch;
            dgvImage.Image = blank;
            dataGridView1.Columns.Add(dgvImage);
            dataGridView1.RowTemplate.Height = 80;
            await getProductAsync("http://127.0.0.1:8000/api/admin/products/view-all");
        }
         async Task<Product> getProductAsync(String url)
        {
            Product product = null;
            HttpResponseMessage response = await client.GetAsync(url);
            string responseBody = await response.Content.ReadAsStringAsync();
          //  List<Product> myObjects = JsonConverter.DeserializeObject<List<Product>>(responseBody);

            List<Product> myObjects = JsonConvert.DeserializeObject<List<Product>>(responseBody);
            foreach (Product obj in myObjects)
            {
                Image newImage = Image.FromFile(@"C:\Users\ASUS\source\repos\WinFormsApp2\WinFormsApp2\icon.png");
                this.dataGridView1.Rows.Add(obj.id, obj.name, obj.sku, obj.name, DateTime.Now, DateTime.Now, newImage);

                Debug.WriteLine($"Name: {obj.name}, Sku: {obj.sku}, Id: {obj.id}");
            }
            return product;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
           // MessageBox.Show("cell cick");
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            MessageBox.Show("CELL END eDIT cick");
        }

        void dataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {

            if (e.ColumnIndex == 0)
            {
                if (dataGridView1.Rows[e.RowIndex].Cells[0].Value != null)
                {
                    var value = dataGridView1.Rows[e.RowIndex].Cells[0].Value;
                    if (value.ToString() == "False")
                    {
                      
                      //  dataGridView1.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = blank;
                        dataGridView1.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Gray;
                    }
                }

            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.index += 1;
            bool isBot = this.index % 2 == 0 ? true : false;
       
            Image newImage = Image.FromFile(@"C:\Users\ASUS\source\repos\WinFormsApp2\WinFormsApp2\icon.png");
            this.dataGridView1.Rows.Add(isBot, "DD", "AAA", "BBDD", DateTime.Now, DateTime.Now, newImage);

        }

        private void dataGridView1_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            // Check if the changed cell is in the column you want to base the color change on
            MessageBox.Show("nothing");

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }
   

        static void ShowProduct(Product product)
        {
            Console.WriteLine($"Name: {product.name}\tPrice: ") ;
        }
    }

    public class Product
    {
        public string id { get; set; }
        public string name { get; set; }
        public decimal quantity { get; set; }
        public string sku { get; set; }
    }
}
