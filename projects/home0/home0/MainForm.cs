using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Drawing.Imaging;

namespace home0
{
    public partial class MainForm : Form
    {
        Image img2 = Image .FromFile(@"C:\Users\hanak\Downloads\hom-20160907T153911Z\hom\home0\home0\11-MICRESP8.jpg");
        Image img3 = Image.FromFile(@"C:\Users\hanak\Downloads\hom-20160907T153911Z\hom\home0\home0\images.jpg");
    
        CardDBDataSet set = new CardDBDataSet();

        /// <summary>
        ///  ____________________________ KAPCSOLAT AZ SQL ADATBÁZISSAL__________________________________________
        /// </summary>
        SqlConnection connection;
        string connectionString; // a string that has information about where the database is how, to connect to it...

        /// <summary>
        ///  ___________________________________________MAIN FORM______________________________________________________________
        /// </summary>
        public MainForm()
        {
            DataTable tbl = set.CardTable;
            
            InitializeComponent();
            Image img = Image.FromFile(@"C:\Users\hanak\Downloads\hom-20160907T153911Z\hom\home0\home0\am1.jpg");
            pictureBox1.Image = img;
            pictureBox2.Image = img;
            pictureBox3.Image = img;

            connectionString = ConfigurationManager.ConnectionStrings["home0.Properties.Settings.CardDBConnectionString"].ConnectionString;
          
           // PopulateCardType();
            
            using (connection = new SqlConnection(connectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM CardTable", connection))
            {
                // connection.Open(); megcsinálja ezt is

                // C# class -> get the query result
                DataTable cardTable = new DataTable();      // elvileg létrehoz egy újat
                adapter.Fill(cardTable);

                listBox1.DataSource = cardTable;

                listBox1.DisplayMember = "CardName";            //ezt látjuk
                listBox1.ValueMember = "CardName";                    // erre az értékre gondol ha kiválasztom


                dataGridView2.DataSource = cardTable;
                dataGridView2.Rows[1].Cells["Size"].Value = 2;
                dataGridView2.Rows[0].Cells["Image"].Value = img2;
                dataGridView2.Rows[1].Cells["Image"].Value = img2;
                dataGridView2.Rows[2].Cells["Image"].Value = img2;
                dataGridView2.Rows[3].Cells["Image"].Value = img2;
                dataGridView2.Rows[4].Cells["Image"].Value = img3;
                dataGridView2.Rows[5].Cells["Image"].Value = img3;
                dataGridView2.Rows[6].Cells["Image"].Value = img3;
                dataGridView2.Rows[7].Cells["Image"].Value = img3;
                // dataGridView2.Columns["Image"].Visible = false;

                //______________________________________________ ADATBÁZISBÓL KÉP___________________________
                pictureBox6.Image = byteArrayToImage((Byte[])dataGridView2.Rows[5].Cells["Image"].Value);

                // connection.Close();  nem kell mert using formában használjuk
            }

        }
       

        // __________________________________________________________________________________________!!!!!!!______________________
        /*private void cardTableBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.cardTableBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.cardDBDataSet);
        }*/
        //
        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cardDBDataSet.CardTable' table. You can move, or remove it, as needed.
           // this.cardTableTableAdapter.Fill(this.cardDBDataSet.CardTable);

            PopulateCardType();                       
        }



        /// <summary>
        ///  ______________________________________________Válogatás kártya típusok közt_____________________________________________________
        /// </summary>
        private void PopulateCardType()
        {
            string query = "SELECT * FROM CardTypeTable";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command/*"SELECT * FROM CardTypeTable", connection*/))
            {
                // connection.Open(); megcsinálja ezt is

                // C# class -> get the query result
                DataTable TypeTable = new DataTable();   
                adapter.Fill(TypeTable);

                lstType.DataSource = TypeTable;

                lstType.DisplayMember = "TypeName"; 
                lstType.ValueMember = "Id";           




                // connection.Close();  nem kell mert ilyen using formában használjuk
            }
        }
        private void PopulateCards()
        {
            //   query: each time we can pass whatever value we want
            string query = "SELECT a.CardName FROM CardTable a " +
                " INNER JOIN TypeCard b ON a.Id = b.CardsId " +
                "WHERE b.CardTypeId = @CardTypeId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))// they support parameters like @CardType
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                // Whatever CardType is selected in the listbox get the id of that and pass to the query(up) and return the cards of thy type
                command.Parameters.AddWithValue("@CardTypeId", lstType.SelectedValue);

                DataTable cardsTable = new DataTable(); 
                adapter.Fill(cardsTable);
                lstCards.DataSource = cardsTable;
                lstCards.DisplayMember = "CardName";                //ezt látjuk
                lstCards.ValueMember = "Id";                       // erre az értékre gondol ha kiválasztom --> Image type is not allowed

            }

        }
        

        /// <summary>
        ///  ___________________________________________SAVE AS IMAGE________________________________________________
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            Bitmap bmpScreenshot = new Bitmap(this.Bounds.Width, this.Bounds.Height, PixelFormat.Format32bppArgb);
            Graphics gfxScreenshot = Graphics.FromImage(bmpScreenshot);
            Point p = this.PointToScreen(new Point(groupBox1.Bounds.X, groupBox1.Bounds.Y));
            gfxScreenshot.CopyFromScreen(p.X, p.Y, 0, 0, groupBox1.Bounds.Size, CopyPixelOperation.SourceCopy);

            SaveFileDialog saveImageDialog = new SaveFileDialog();
            saveImageDialog.Title = "Select output file:";
            saveImageDialog.Filter = "JPeg Image|*.jpg|Bitmap Image|*.bmp|Gif Image|*.gif";
            //saveImageDialog.FileName = printFileName;
            if (saveImageDialog.ShowDialog() == DialogResult.OK)
            {
                
                // Save the screenshot to the specified path that the user has chosen
                bmpScreenshot.Save(saveImageDialog.FileName, ImageFormat.Png);
            }
        }

        /// <summary>
        ///   PUT Selected item to the slot (where radio button checked)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            if(radioButton1.Checked )
            textBox1.Text  = listBox1.SelectedValue.ToString();
            // ide egy lekérdezés kéne, hogy ahhoz az id-hez melyik kép tartozik
            if (textBox1.Text == "CT+ 0101")
            {
                pictureBox4.Image = byteArrayToImage((Byte[])dataGridView2.Rows[5].Cells["Image"].Value);
                pictureBox5.Image = null;
                pictureBox5.Invalidate();
                //pictureBox5.Dispose();
                //pictureBox4.Image = img2;   // fekete
            }
            else
            {                
                pictureBox4.Image = null;
                pictureBox4.Invalidate();
               // pictureBox4.Dispose();
                pictureBox5.Image = img3; // szines
            }
        }


        public Image byteArrayToImage(byte[] bytesArr)
        {
            MemoryStream memstr = new MemoryStream(bytesArr);
            Image img = Image.FromStream(memstr);
            return img;
        }

        private void lstType_SelectedIndexChanged(object sender, EventArgs e)
        {
                //PopulateCards(); 
        }
       
    }

}
