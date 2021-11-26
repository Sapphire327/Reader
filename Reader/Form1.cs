using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
namespace Reader
{
    public partial class Form1 : Form
    {
        string fileconfig = "config.sar";
        int page = 1;
        String FileName;
        int idbook=-1;
        public Form1()
        {
            InitializeComponent();
            if (File.Exists(fileconfig)) {
                using (Stream stream = File.Open(fileconfig, FileMode.Open))
                {
                    BinaryFormatter bf = new BinaryFormatter();
                    try
                    {
                        while (true)
                        {
                            LboxReadBefore.Items.Add((Book)bf.Deserialize(stream));
                        }
                    }
                    catch { }
                }
               
            }
        }
        void Read(int page)
        {
            Pbar.Maximum = ((Book)LboxReadBefore.Items[idbook]).PagesNumber;
            Pbar.Value = page;

            string remainderstring = "";
            int remaindercountstrok = 0;
            if (page % 2 == 0&&page>1)
            {
                page -= 1;
            }
            LLeftpage.Text = page.ToString();
            LRightPage.Text = (page + 1).ToString();
            ((Book)LboxReadBefore.Items[idbook]).PageNow = page;
            save();
            labelLeftPage.Text = "";
            labelRightPage.Text = "";
            Stream stream = File.Open(FileName, FileMode.Open);
            using (StreamReader sr = new StreamReader(stream, Encoding.Default))
            {
                for (int i = 1; i < page; i++)
                {
                    for (int j = remaindercountstrok; j < 38; j++)
                    {
                        remaindercountstrok = 0;
                        if (sr.EndOfStream)
                        {
                            return;
                        }
                        string temp = sr.ReadLine();
                        if (temp.Length > 60)
                        {
                            j = j + temp.Length / 60;
                            if (j > 38)
                            {
                                remainderstring = temp;
                                remaindercountstrok = temp.Length / 60;
                                break;
                            }
                        }
                    }
                }

                if (remaindercountstrok != 0)
                {
                    labelLeftPage.Text += remainderstring;
                }

                for (int i = remaindercountstrok; i < 38; i++)
                {
                    remaindercountstrok = 0;
                    if (sr.EndOfStream)
                    {
                        return;
                    }
                    string temp = sr.ReadLine();
                    if (temp.Length > 60)
                    {
                        i = i + temp.Length / 60;
                        if (i > 38)
                        {
                            remainderstring = temp;
                            remaindercountstrok = temp.Length / 60;
                            break;
                        }
                    }
                    if (temp == "")
                        labelLeftPage.Text += "\n";
                    else
                        labelLeftPage.Text += temp;
                }

                if (remaindercountstrok != 0)
                {
                    labelRightPage.Text += remainderstring;
                }

                for (int i = remaindercountstrok; i < 38; i++)
                {
                    if (sr.EndOfStream)
                    {
                        return;
                    }
                    string temp = sr.ReadLine();
                    if (temp.Length > 60)
                    {
                        i = i + temp.Length / 60;
                        if (i > 38)
                        {
                            break;
                        }
                    }
                    if (temp == "")
                        labelRightPage.Text += "\n";
                    else
                        labelRightPage.Text += temp;
                }
            }
            stream.Close();
        }

        int countpages()
        {
            int remaindercountstrok = 0;
            int count = 0;
            int strinttxt = 0;
            Stream stream = File.Open(FileName, FileMode.Open);
            using (StreamReader sr = new StreamReader(stream, Encoding.Default))
            {
                while (!sr.EndOfStream)
                {
                    for (int j = remaindercountstrok; j < 38; j++)
                    {
                        remaindercountstrok = 0;
                        if (sr.EndOfStream)
                        {
                            return count + 1;
                        }
                        string temp = sr.ReadLine();
                        strinttxt++;
                        if (temp.Length > 60)
                        {
                                if (temp.Length>2200) {
                                MessageBox.Show($"Строка {strinttxt} слишком длинная");
                                return -1;
                                }
                            j = j + temp.Length / 60;
                            if (j > 38)
                            {
                               
                                remaindercountstrok = temp.Length / 60;
                                break;
                            }
                        }
                    }
                    if (count == 300) {
                        Console.WriteLine("f");
                    }
                    count++;
                }
                return count;
            }
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();
            opf.Filter = "Текстовые файлы(*txt)|*txt";
            opf.Title = "Открытие книги";
            if (opf.ShowDialog()==DialogResult.OK) {
                FileName = opf.FileName;
                int i = 0;
                foreach (Book item in LboxReadBefore.Items)
                {
                    if (item.Path == opf.FileName) {
                        idbook = i;
                        refreshmark();
                        page = item.PageNow;
                        Read(page);
                            return;
                    }
                    i++;
                }
                Book book = new Book();
                int pages = countpages();
                if (pages == -1) {
                    return;
                }
                book.PagesNumber = pages;
                book.Path = FileName;
                book.Name = Path.GetFileNameWithoutExtension(opf.FileName);

                LboxReadBefore.Items.Add(book);
                save();

                idbook = LboxReadBefore.Items.Count-1;
                Read(0);
            }

        }
        void save() {
            using (Stream stream = File.Create(fileconfig))
            {
                BinaryFormatter bf = new BinaryFormatter();
                foreach (var item in LboxReadBefore.Items)
                {
                    bf.Serialize(stream, item);
                }
            }
        }
        private void btnPageLeft_Click(object sender, EventArgs e)
        {
            if (page > 2) {
                page -= 2;
            }
            Read(page);
        }

        private void btnPageRight_Click(object sender, EventArgs e)
        {
            if (page < ((Book)LboxReadBefore.Items[idbook]).PagesNumber)
            {
                page += 2;
            }
            Read(page);
        }

        private void LboxMark_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        private void btnOpen_MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).ForeColor = Color.FromArgb(246, 231, 210);
        }

        private void btnOpen_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.Silver;

        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (LboxReadBefore.SelectedIndex != -1) {
                FileName = ((Book)LboxReadBefore.SelectedItem).Path;
                LboxMark.Items.Clear();
                idbook = LboxReadBefore.SelectedIndex;
                page = ((Book)LboxReadBefore.SelectedItem).PageNow;
                Read(page);
                refreshmark();
            }
        }
        void refreshmark() {
            LboxMark.Items.Clear();

            foreach (Mark m in ((Book)LboxReadBefore.Items[idbook]).marks)
            {
                LboxMark.Items.Add(m);
            }
        }
        private void btnAddMark_Click(object sender, EventArgs e)
        {
            if (idbook != -1) {
                Mark m = new Mark();
                m.page = page;
                FormAddMark fam = new FormAddMark(m,((Book)LboxReadBefore.Items[idbook]).PagesNumber);
                if (fam.ShowDialog() == DialogResult.OK) {
                    ((Book)LboxReadBefore.Items[idbook]).marks.Add(m);
                }
                save();
                refreshmark();
            }
        }

        private void btnChangeMark_Click(object sender, EventArgs e)
        {
            if (LboxMark.SelectedIndex != -1)
            {
                Mark m = new Mark();
                m.name = ((Book)LboxReadBefore.Items[idbook]).marks[LboxMark.SelectedIndex].name;
                m.page = ((Book)LboxReadBefore.Items[idbook]).marks[LboxMark.SelectedIndex].page;
                FormAddMark fam = new FormAddMark(m, ((Book)LboxReadBefore.Items[idbook]).PagesNumber);
                if (fam.ShowDialog() == DialogResult.OK)
                {
                    ((Book)LboxReadBefore.Items[idbook]).marks[LboxMark.SelectedIndex ] = m;
                }
                save();
                refreshmark();
            }
        }

        private void btnDeleteMark_Click(object sender, EventArgs e)
        {
            if (LboxMark.SelectedIndex != -1)
            {
                ((Book)LboxReadBefore.Items[idbook]).marks.RemoveAt(LboxMark.SelectedIndex);
                refreshmark();
                save();
            }

        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (LboxMark.SelectedIndex != -1) {
                page = ((Book)LboxReadBefore.Items[idbook]).marks[LboxMark.SelectedIndex].page;
                Read(page);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (LboxReadBefore.SelectedIndex != -1) {
                if (LboxReadBefore.SelectedIndex == idbook) {
                    labelLeftPage.Text = "";
                    labelRightPage.Text = "";
                    LRightPage.Text = "";
                    LLeftpage.Text = "";
                    FileName = "";
                    idbook = -1;
                    LboxMark.Items.Clear();
                    Pbar.Value = 0;
                }
                LboxReadBefore.Items.RemoveAt(LboxReadBefore.SelectedIndex);
                save();
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            save();
            Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (Pbar.Visible)
            {
                Pbar.Visible = false;
                btnPbar.Text = "Включить ProgressBar";
            }
            else {
                Pbar.Visible = true;
                btnPbar.Text = "Убрать ProgressBar";
            }
        }
    }
}
