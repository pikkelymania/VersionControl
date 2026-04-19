using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using UserMaintenance.Entities;
using System.IO;

namespace UserMaintenance
{
    public partial class Form1 : Form
    {
        // A lista, ami az adatokat tárolja
        BindingList<User> users = new BindingList<User>();

        public Form1()
        {
            InitializeComponent();

            // Feliratok beállítása a Resource fájlból
            label1.Text = Resource1.FullName;
            button1.Text = Resource1.Add;
            button2.Text = Resource1.Save;
            button3.Text = Resource1.Delete;

            // ListBox összekötése a listával
            listBox1.DataSource = users;
            listBox1.ValueMember = "ID";
            listBox1.DisplayMember = "FullName";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var u = new User()
            {
                FullName = textBox1.Text,
            };
            users.Add(u);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "CSV fájlok (*.csv)|*.csv|Szöveges fájlok (*.txt)|*.txt";

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                // Fájlba írás a StreamWriter segítségével
                using (StreamWriter sw = new StreamWriter(sfd.FileName))
                {
                    foreach (var user in users)
                    {
                        // Kiírjuk az ID-t és a FullName-et pontosvesszővel elválasztva
                        sw.WriteLine($"{user.ID};{user.FullName}");
                    }
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedItem != null)
            {
                users.Remove((User)listBox1.SelectedItem);
            }
        }
    }
}
