using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace teht2
{
    public partial class Form1 : Form
    {
        private readonly string _connectionString;
        private string firstName = "";
        private string lastName = "";
        private int selectedGroupId = -1; // To store the selected group ID

        public Form1(string con_str)
        {
            Debug.WriteLine(con_str);
            _connectionString = con_str;
            InitializeComponent();
        }

        private async void Form1_Load(object sender, EventArgs e)
        {
            await LoadDataGridViewAsync();
            await LoadComboBoxAsync();
        }

        //datagrid
        private async Task LoadDataGridViewAsync()
        {
            string query = "SELECT * FROM opiskelijat";

            using SqlConnection myConnection = new SqlConnection(_connectionString);
            try
            {
                await myConnection.OpenAsync();
                using SqlCommand command = new(query, myConnection);
                using SqlDataAdapter adapter = new(command);

                DataTable studentsTable = new();
                await Task.Run(() => adapter.Fill(studentsTable)); // Run Fill on a background thread

                dataGridView1.DataSource = studentsTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //group loading
        private async Task LoadComboBoxAsync()
        {
            string query = "SELECT * FROM opiskelijaryhmat";
            using SqlConnection myConnection = new SqlConnection(_connectionString);
            try
            {
                await myConnection.OpenAsync();
                using SqlCommand command = new(query, myConnection);
                using SqlDataReader reader = await command.ExecuteReaderAsync();
                while (await reader.ReadAsync())
                {
                    comboBox1.Items.Add(new { Id = (int)reader["id"], Name = reader["nimi"].ToString() });
                }

                comboBox1.DisplayMember = "Name"; // Display group names
                comboBox1.ValueMember = "Id";    // Use group IDs for operations
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //firstname
        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            firstName = textBox1.Text ?? "";
        }
        //surname
        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            lastName = textBox2.Text ?? "";
        }

        //add button
        private async void button1_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(firstName) || string.IsNullOrWhiteSpace(lastName) || selectedGroupId == -1)
            {
                MessageBox.Show("Please fill in all fields and select a group.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string query = "INSERT INTO opiskelijat (etunimi, sukunimi, ryhma_id) VALUES (@etunimi, @sukunimi, @ryhma_id)";

            using SqlConnection myConnection = new SqlConnection(_connectionString);
            try
            {
                await myConnection.OpenAsync();
                using SqlCommand command = new(query, myConnection);
                command.Parameters.AddWithValue("@etunimi", firstName);
                command.Parameters.AddWithValue("@sukunimi", lastName);
                command.Parameters.AddWithValue("@ryhma_id", selectedGroupId);

                await command.ExecuteNonQueryAsync();
                MessageBox.Show("Student added successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataGridViewAsync(); // Refresh the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        //combobox with groups
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                dynamic selectedItem = comboBox1.SelectedItem;
                selectedGroupId = selectedItem.Id; // Set the selected group ID
            }
        }

        //delete button
        private async void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Please select a student to delete.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int selectedId = (int)dataGridView1.SelectedRows[0].Cells["id"].Value;

            string query = "DELETE FROM opiskelijat WHERE id = @id";

            using SqlConnection myConnection = new SqlConnection(_connectionString);
            try
            {
                await myConnection.OpenAsync();
                using SqlCommand command = new(query, myConnection);
                command.Parameters.AddWithValue("@id", selectedId);

                await command.ExecuteNonQueryAsync();
                MessageBox.Show("Student deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                await LoadDataGridViewAsync(); // Refresh the data grid
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
