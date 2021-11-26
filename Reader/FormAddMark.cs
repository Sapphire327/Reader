using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Reader
{
    public partial class FormAddMark : Form
    {
        Mark mark;
        int maximum;
        public FormAddMark(Mark m,int max)
        {
            InitializeComponent();
            maximum = max;
            mark = m;
            Tboxpage.Text = m.page.ToString();
            TboxName.Text = m.name;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            int temp;
            if (!String.IsNullOrEmpty(TboxName.Text) && int.TryParse(Tboxpage.Text, out temp)) {
                mark.page = Convert.ToInt32(Tboxpage.Text);
                if (mark.page > 0 && mark.page < maximum)
                {
                    mark.name = TboxName.Text;
                    DialogResult = DialogResult.OK;
                }
            }
        }
        private void btn_MouseMove(object sender, MouseEventArgs e)
        {
            ((Button)sender).ForeColor = Color.FromArgb(246, 231, 210);
        }

        private void btn_MouseLeave(object sender, EventArgs e)
        {
            ((Button)sender).ForeColor = Color.Silver;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.No;
        }
    }
}
