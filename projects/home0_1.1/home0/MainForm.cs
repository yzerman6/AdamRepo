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
        int radioFlag;
        Image img2 = Image .FromFile(@"C:\Users\hanak\Desktop\home0\home0\electronic-cards-250x250.jpg");
        Image img3 = Image.FromFile(@"C:\Users\hanak\Desktop\home0\home0\CT+.jpg");
       
        CardDBDataSet set = new CardDBDataSet();

        /// <summary>
        ///  ____________________________NAGYON FONTOS KAPCSOLAT AZ SQL ADATBÁZISSAL__________________________________________
        /// </summary>
        SqlConnection connection;
        string connectionString; // a string that has information about where the database is how to connect to it...

        /// <summary>
        ///  ___________________________________________MAIN FORM______________________________________________________________
        /// </summary>
        public MainForm()
        {
            DataTable tbl = set.CardTable;
            
            InitializeComponent();
            Image img = Image.FromFile(@"C:\Users\hanak\Desktop\home0\home0\PackagingElectronicCards.jpg");
            pictureBox1.Image = img;
            pictureBox2.Image = img;
            pictureBox3.Image = img;

            connectionString = ConfigurationManager.ConnectionStrings["home0.Properties.Settings.CardDBConnectionString"].ConnectionString;

            
            
            //PopulateCardType();

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

            }

            
        }
       
            
        private void MainForm_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'cardDBDataSet.CardTable' table. You can move, or remove it, as needed.
           // this.cardTableTableAdapter.Fill(this.cardDBDataSet.CardTable);

           // PopulateCardType();                       
        }


       /* private void PopulateCardType()
        {
            string query;
             
            query = "SELECT * FROM CardTypeTable WHERE TypeName = 'CT+' ";       //WHERE TypeName = 'CT+'
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                // connection.Open(); megcsinálja ezt is

                // C# class -> get the query result
                DataTable TypeTable = new DataTable();                   // elvileg létrehoz egy újat
                adapter.Fill(TypeTable);

                LISTTYPES.DataSource = TypeTable;

                LISTTYPES.DisplayMember = "TypeName";                     //ezt látjuk
                LISTTYPES.ValueMember = "Id";                       // erre az értékre gondol ha kiválasztom




                // connection.Close();  nem kell mert ilyen using formában használjuk
            }
        }*/
       /* private void PopulateCards()
        {
            //                                                          query: each time we can pass whatever value we want
            string query = "SELECT a.CardName FROM CardTable a" +
                "INNER JOIN TypeCard b ON a.Id = b.CardsId" +
                "WHERE b.CardTypeId = @CardTypeId";
            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))// they support parameters like @CardType
            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                // Whatever CardType is selected in the listbox get the id of that and pass to the query(up) and return the cards of thy type
                command.Parameters.AddWithValue("@CardTypeId", lstType.SelectedValue);

                DataTable cardsTable = new DataTable();                   // elvileg létrehoz egy újat
                adapter.Fill(cardsTable);
                lstCards.DataSource = cardsTable;
                lstCards.DisplayMember = "CardName";                     //ezt látjuk
                lstCards.ValueMember = "Id";                       // erre az értékre gondol ha kiválasztom

            }

        }*/
        


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
                //enter code here
                // Save the screenshot to the specified path that the user has chosen
                bmpScreenshot.Save(saveImageDialog.FileName, ImageFormat.Png);
            }
        }

    //-------------------------------------------------------------- EDIT ---------------------------------------------------------
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

        /*private void lstType_SelectedIndexChanged(object sender, EventArgs e)
        {
                //PopulateCards();
        }*/
        /// <summary>
        /// SELECTED TYPE
        /// </summary>
        
        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            string query;
            int index = LISTTYPES.SelectedIndex;
            
            switch (index)
            {
                case 0: query = "SELECT * FROM CardTable WHERE CardType = 'CT+' "; break;
                case 1: query = "SELECT * FROM CardTable WHERE CardType = 'VT+' "; break;
                case 2: query = "SELECT * FROM CardTable WHERE CardType = 'PSPT+' "; break;
                case 3: query = "SELECT * FROM CardTable WHERE CardType = 'PS+' "; break;
                case 4: query = "SELECT * FROM CardTable WHERE CardType = 'CPU+' "; break;
                case 5: query = "SELECT * FROM CardTable WHERE CardType = 'O6R+' "; break;
                default: query = "SELECT * FROM CardTable WHERE CardType LIKE '%T%' "; break;
            }

            using (connection = new SqlConnection(connectionString))
            using (SqlCommand command = new SqlCommand(query, connection))
            using (SqlDataAdapter adapter = new SqlDataAdapter(command/*"SELECT * FROM CardTypeTable", connection*/))
            {
                
                DataTable cardTable = new DataTable();                   // elvileg létrehoz egy újat
                adapter.Fill(cardTable);

                listresult.DataSource = cardTable;

                listresult.DisplayMember = "CardName";                     //ezt látjuk
                listresult.ValueMember = "CardName";                       // erre az értékre gondol ha kiválasztom

            }
        }

        private void buttonADD_Click(object sender, EventArgs e)
        {
            
            if (radioB.Checked) radioFlag = 1;
            if (radioC.Checked) radioFlag = 2;
            if (radioD.Checked) radioFlag = 3;
            switch (radioFlag)
            {
                case 1: textBox2.Text = listresult.SelectedValue.ToString(); break;
                case 2: textBox3.Text = listresult.SelectedValue.ToString(); break;
                case 3: textBox4.Text = listresult.SelectedValue.ToString(); break;
                default: textBox4.Text = listresult.SelectedValue.ToString(); break;
            }
        }

        private void buttonFORM_Click(object sender, EventArgs e)
        {
            Conf c = new Conf();
            c.Show();
        }
    }

}
