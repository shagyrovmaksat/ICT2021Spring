using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Example2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            
            InitializeComponent();

            LoadContacts();
        }

        BLL bll = default(BLL);

        private void LoadContacts()
        {
            ContactDB contacts2 = new ContactDB();

            bll = new BLL(contacts2);

            bindingSource1.DataSource = bll.GetContacts();


            bindingNavigator1.BindingSource = bindingSource1;
            dataGridView1.DataSource = bindingSource1;

            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            toolStripStatusLabel1.Text = bll.GetContacts().Count.ToString();
            bindingSource1.DataSource = bll.GetContacts();
        }

        private void dataGridView1_UserAddedRow(object sender, DataGridViewRowEventArgs e)
        {
            //MessageBox.Show("ok");
           
        }

        private void dataGridView1_RowsAdded(object sender, DataGridViewRowsAddedEventArgs e)
        {
            // MessageBox.Show("dataGridView1_RowsAdded");
           
        }

        private void dataGridView1_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
           
        }

        private void dataGridView1_RowValidated(object sender, DataGridViewCellEventArgs e)
        {
        }

        CreateContactForm createContactForm = new CreateContactForm();
        EditContactForm editContactForm = new EditContactForm();
        DetailContackForm detailContactForm = new DetailContackForm();

        private void bindingNavigatorAddNewItem_Click(object sender, EventArgs e)
        {
           
        }

        private void button2_Click(object sender, EventArgs e)
        {
            createContactForm.nameTxtBx.Text = "";
            createContactForm.phoneTxtBx.Text = "";
            createContactForm.addressTxtBx.Text = "";
            if (createContactForm.ShowDialog() == DialogResult.OK)
            {
                CreateContactCommand command = new CreateContactCommand();
                command.Name = createContactForm.nameTxtBx.Text;
                command.Phone = createContactForm.phoneTxtBx.Text;
                command.Addr = createContactForm.addressTxtBx.Text;
                bll.CreateContact(command);
                bindingSource1.DataSource = bll.GetContacts();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                editContactForm.nameTxtBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                editContactForm.phoneTxtBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                editContactForm.addressTxtBox.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                if (editContactForm.ShowDialog() == DialogResult.OK)
                {
                    CreateContactCommand command = new CreateContactCommand();
                    command.Name = editContactForm.nameTxtBox.Text;
                    command.Phone = editContactForm.phoneTxtBox.Text;
                    command.Addr = editContactForm.addressTxtBox.Text;
                    bll.UpdateContectById(dataGridView1.SelectedRows[0].Cells[0].Value.ToString(), command);
                    bindingSource1.DataSource = bll.GetContacts();
                }
            }
            catch { }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            try
            {
                bll.DelecteContact(dataGridView1.SelectedRows[0].Cells[0].Value.ToString());
                bindingSource1.DataSource = bll.GetContacts();
            }
            catch { }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            try
            {
                detailContactForm.nameLabel.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                detailContactForm.phoneLabel.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                detailContactForm.addressLabel.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                if (detailContactForm.ShowDialog() == DialogResult.Cancel)
                {

                }
            }
            catch { }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if(textBox1.Text == "")
            {
                bindingSource1.DataSource = bll.GetContacts();
            }
            else
            {
                List<ContactDTO> data = bll.GetContactsByName(textBox1.Text);
                bindingSource1.DataSource = data;
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (textBox2.Text == "")
            {
                bindingSource1.DataSource = bll.GetContacts();
            }
            else
            {
                List<ContactDTO> data = bll.GetContactsByPhone(textBox2.Text);
                bindingSource1.DataSource = data;
            }
        }

        private void button7_Click(object sender, EventArgs e)
        {
            Button b = sender as Button;
            if(b.Text == "back")
            {
                if(pageNum.Text != "0")
                {
                    int num = int.Parse(pageNum.Text);
                    pageNum.Text = (--num).ToString();
                    List<ContactDTO> data = bll.GetContactsOnePage(num);
                    bindingSource1.DataSource = data;
                }
            }
            else
            {
                int num = int.Parse(pageNum.Text);
                pageNum.Text = (++num).ToString();
                List<ContactDTO> data = bll.GetContactsOnePage(num);
                bindingSource1.DataSource = data;
            }

            if (pageNum.Text == "0")
            {
                bindingSource1.DataSource = bll.GetContacts();
            }
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            bll.changeLimit(int.Parse(numericUpDown1.Value.ToString()));
            List<ContactDTO> data = bll.GetContactsOnePage(int.Parse(pageNum.Text));
            bindingSource1.DataSource = data;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            bindingSource1.DataSource = bll.GetSortedContacts();
        }
    }
}
