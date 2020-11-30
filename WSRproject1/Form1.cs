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
using System.Diagnostics;

namespace WSRproject1
{
    public partial class Form1 : Form
    {
        static string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
        SqlConnection connection = new SqlConnection(connectionString);
        int sec=0;
        public Form1()
        {
            InitializeComponent();
           
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (Car.Location.Y > -50)
            {
                Car.Location = new Point(Car.Location.X, Car.Location.Y - 111);
                sec++;
            }
            else
            {
                timer1.Stop();
                try
                {
                    string connectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=usersdb;Integrated Security=True";
                    SqlConnection connection = new SqlConnection(connectionString);

                    // Открываем подключение
                    connection.Open();
                    Console.WriteLine("Подключение открыто");
                    
                    // Команда Insert.
                    string sql = "Insert into Table1 (Record) " + " values (@record) ";

                    SqlCommand cmd = connection.CreateCommand();
                    cmd.CommandText = sql;

                    SqlParameter highSalaryParam = cmd.Parameters.Add("@record", SqlDbType.Text);
                    highSalaryParam.Value = sec.ToString();

                    int rowCount = cmd.ExecuteNonQuery();

                    Console.WriteLine("Row Count affected = " + rowCount);
                }
                catch (SqlException ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    // закрываем подключение
                    connection.Close();
                    Console.WriteLine("Подключение закрыто...");
                }
            }
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            timer1.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            // 1) Create Process Info using System.Diagnostics;
            var psi = new ProcessStartInfo();
            psi.FileName = @"C:\Program Files (x86)\Microsoft Visual Studio\Shared\Python37_64\python.exe";

            // 2) Provide script and arguments
            var script = @"Simple.py";

            psi.Arguments = $"\"{script}\" \"{Car.Location.X}\" \"{Car.Location.Y}\"";

            // 3) Process configuration
            psi.UseShellExecute = false;
            psi.CreateNoWindow = true;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;

            // 4) Execute process and get output
            var errors = "";
            var results = "";

            using (var process = Process.Start(psi))
            {
                errors = process.StandardError.ReadToEnd();
                results = process.StandardOutput.ReadToEnd();
            }

            // 5) Display output
            Console.WriteLine("ERRORS:");
            Console.WriteLine(errors);
            Console.WriteLine();
            Console.WriteLine("Results:");
            Console.WriteLine(results);

            // 6) Change car location
            string[] s = results.Split();
            Car.Location = new Point(int.Parse(s[0]), int.Parse(s[1]));
        }
    }
}
