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

namespace TP4_ModeDeconnecte
{
    public partial class Form1 : Form
    {
        static SqlConnection connection = new SqlConnection(@"Data Source=localhost;Initial Catalog=TP_01_Stagiaires;Integrated Security=True");
        SqlDataAdapter employeAd = new SqlDataAdapter("SELECT * FROM Employe", connection);
        SqlDataAdapter serviceAd = new SqlDataAdapter("SELECT * FROM Service", connection);
        DataSet data = new DataSet();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            employeAd.Fill(data, "Employe");
            serviceAd.Fill(data, "Service");
            comboBoxSerivce.DataSource = null;
            comboBoxSerivce.DataSource = data.Tables["Service"];
            comboBoxSerivce.DisplayMember = "nomServ";
            comboBoxSerivce.ValueMember = "IdService";
            remplirDataGridView();
        }
        
        private void remplirDataGridView()
        {
            dataGridViewEmpl.DataSource = null;
            dataGridViewEmpl.DataSource = data.Tables["Employe"];
        }

        private void btnChercher_Click(object sender, EventArgs e)
        {
            if (!Exist())
            {
                MessageBox.Show("L'employe n'existe pas");
                return;
            }
            foreach (DataRow row in data.Tables["Employe"].Rows)
            {
                if (textBoxId.Text == row["id"].ToString())
                {
                    textBoxNom.Text = row["nom"].ToString();
                    textBoxPrenom.Text = row["prenom"].ToString();
                    DateNaissance.Value = DateTime.Parse(row["dateNaissance"].ToString());
                    comboBoxSerivce.SelectedValue = Int32.Parse(row["Id_Service"].ToString());
                    break;
                }
            }
            
        }

        private void btnAjouter_Click(object sender, EventArgs e)
        {
            if (Exist())
            {
                MessageBox.Show("L'employe existe dèja");
            }
            else
            {
                DataRow row = data.Tables["Employe"].NewRow();
                row["id"] = Int32.Parse(textBoxId.Text);
                row["nom"] = textBoxNom.Text;
                row["prenom"] = textBoxPrenom.Text;
                row["dateNaissance"] = DateNaissance.Value.ToShortDateString();
                row["Id_Service"] = comboBoxSerivce.SelectedValue;
                data.Tables["Employe"].Rows.Add(row);
                SqlCommandBuilder builder = new SqlCommandBuilder(employeAd);
                builder.GetInsertCommand();
                employeAd.Update(data, "Employe");
                remplirDataGridView();
                MessageBox.Show("l'employe est enregistré");
            }
        }

        private bool Exist()
        {
            foreach (DataRow row in data.Tables["Employe"].Rows)
            {
                if (textBoxId.Text == row["id"].ToString())
                {
                    return true;
                }
            }
            return false;
        }

        private void btnModifier_Click(object sender, EventArgs e)
        {
            if (Exist())
            {
                foreach (DataRow row in data.Tables["Employe"].Rows)
                {
                    if (textBoxId.Text == row["id"].ToString())
                    {
                        row["nom"] = textBoxNom.Text;
                        row["prenom"] = textBoxPrenom.Text;
                        row["dateNaissance"] = DateNaissance.Value.ToShortDateString();
                        row["Id_Service"] = comboBoxSerivce.SelectedValue;
                        SqlCommandBuilder builder = new SqlCommandBuilder(employeAd);
                        builder.GetInsertCommand();
                        employeAd.Update(data, "Employe");
                        remplirDataGridView();
                        MessageBox.Show("l'employe est modifié");
                    }
                }
            }
            else
            {
                MessageBox.Show("l'employe n'existe pas");
            }
        }

        private void btnSupprimer_Click(object sender, EventArgs e)
        {
            if (Exist())
            {
                foreach (DataRow row in data.Tables["Employe"].Rows)
                {
                    if (textBoxId.Text == row["id"].ToString())
                    {
                        row.Delete();
                        SqlCommandBuilder builder = new SqlCommandBuilder(employeAd);
                        builder.GetInsertCommand();
                        employeAd.Update(data, "Employe");
                        remplirDataGridView();
                        break;
                    }
                }
            }
            else
            {
                MessageBox.Show("l'employe n'existe pas");
            }
        }
    }
}
