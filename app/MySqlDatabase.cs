using MySql.Data.MySqlClient;
using System;
using System.Security.Cryptography;
using System.Text;
 using System.Collections.Generic;
 using Newtonsoft.Json;

namespace MySqlDatabase
{
    public class MySqlDatabase
    {
        private readonly string _connectionString;

        public MySqlDatabase(string host, int port, string username, string password, string databaseName)
        {
            _connectionString = $"Server={host};Port={port};Database={databaseName};User={username};Password={password};";
        }

    

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }

        public bool IsValidUser(string username, string providedPassword)
        {
            using MySqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                string sqlQuery = "SELECT password FROM Users WHERE Username = @username";
                using (MySqlCommand cmd = new MySqlCommand(sqlQuery, connection))
                {
                    cmd.Parameters.AddWithValue("@username", username);
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader.GetString("password");

                            // Hash the provided password using the same method as stored
                            string hashedProvidedPassword = HashPassword(providedPassword);

                            // Compare the hashed provided password with the stored hash
                            return hashedProvidedPassword == storedHash;
                        }
                        else
                        {
                            // User not found
                            return false;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred while connecting to the SQL database: " + e.Message);
                return false;
            }
        }

        private string HashPassword(string password)
        {
            // Implement the same hashing method used for storing passwords in the database here
            // Example: SHA256 hashing
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }


 public string? GetLicenseDataAsJson()
        {
            using MySqlConnection connection = GetConnection();
            try
            {
                connection.Open();
                string sqlQuery = "SELECT * FROM licenses";
                using (MySqlCommand cmd = new MySqlCommand(sqlQuery, connection))
                {
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        List<License> licenses = new List<License>();

                        while (reader.Read())
                        {
                     License license = new License
                            {
                                ID = reader.GetInt32("ID"),
                                NAME = reader.GetString("NAME"),
                                SURNAME = reader.GetString("SURNAME"),
                                EMAIL = reader.GetString("EMAIL"),
                                COMPANY = reader.GetString("COMPANY"),
                                LICENSE_NAME = reader.GetString("LICENSE_NAME"),
                                LICENSE_TYPE = reader.GetString("LICENSE_TYPE"),
                                LICENSE_ID = reader.GetInt32("LICENSE_ID"),
                                OLD_LICENSE_ID = reader.GetInt32("OLD_LICENSE_ID"),
                                QUANTITY = reader.GetInt32("QUANTITY"),
                                LICENSE_EXP_DATE = reader.GetDateTime("LICENSE_EXP_DATE"),
                                ASP_INVOICE_DATE = reader.GetDateTime("ASP_INVOICE_DATE"),
                                ASP_PAY_DUE_DATE = reader.GetDateTime("ASP_PAY_DUE_DATE"),
                                YUNUS_TERM_DAYS = reader.GetInt32("YUNUS_TERM_DAYS"),
                                YUNUS_PAY_AMOUNT = reader.GetFloat("YUNUS_PAY_AMOUNT"),
                                YUNUS_PAY_STATUS = reader.GetString("YUNUS_PAY_STATUS"),
                                PO_NUM = reader.GetString("PO_NUM"),
                                YUNUS_INVO_NUM = reader.GetString("YUNUS_INVO_NUM"),
                                SECTOR = reader.GetString("SECTOR"),
                                SI_END_USER = reader.GetString("SI_END_USER"),
                                ASP_INVOICE = reader.GetString("ASP_INVOICE"),
                                ASP_PAY_TERM_DAYS = reader.GetInt32("ASP_PAY_TERM_DAYS"),
                                COMP_MADE_PAY_STATUS = reader.GetString("COMP_MADE_PAY_STATUS"),
                                ENT_IN_FINSYS_STATUS = reader.GetString("ENT_IN_FINSYS_STATUS"),
                                FOREIGN_INST_DATE = reader.GetDateTime("FOREIGN_INST_DATE"),
                                NOTES = reader.GetString("NOTES"),
                                SQL_ID = reader.GetInt32("ID")

                            };

                            licenses.Add(license);
                        }

                        string json = JsonConvert.SerializeObject(licenses, Formatting.Indented);
                        return json;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Oh no! SQL error: " + e);
                return null; // Handle the error as needed
            }
        }

public class License
{
    public int ID { get; set; }
    public required string NAME { get; set; }
    public required string SURNAME { get; set; }
    public required string EMAIL { get; set; }
    public required string COMPANY { get; set; }
    public required string LICENSE_NAME { get; set; }
    public required string LICENSE_TYPE { get; set; }
    public int LICENSE_ID { get; set; }
    public int OLD_LICENSE_ID { get; set; }
    public int QUANTITY { get; set; }
    public DateTime LICENSE_EXP_DATE { get; set; }
    public DateTime ASP_INVOICE_DATE { get; set; }
    public DateTime ASP_PAY_DUE_DATE { get; set; }
    public int YUNUS_TERM_DAYS { get; set; }
    public float YUNUS_PAY_AMOUNT { get; set; }
    public required string YUNUS_PAY_STATUS { get; set; }
    public required string PO_NUM { get; set; }
    public required string YUNUS_INVO_NUM { get; set; }
    public required string SECTOR { get; set; }
    public required string SI_END_USER { get; set; }
    public required string ASP_INVOICE { get; set; }
    public int ASP_PAY_TERM_DAYS { get; set; }
    public required string COMP_MADE_PAY_STATUS { get; set; }
    public required string ENT_IN_FINSYS_STATUS { get; set; }
    public DateTime FOREIGN_INST_DATE { get; set; }
    public required string NOTES { get; set; }
    public required int SQL_ID { get; set; }

}


/////
///
public int SaveData(string?[] fields)
{        Console.WriteLine(" function starting....");

    using MySqlConnection connection = GetConnection();
    try
    {
        connection.Open();
        string sqlQuery = "";

        if (fields[0] != null)
        {//////////////////
//////////////////
//////////////////

            if (fields[0].Equals("add"))
            {
                sqlQuery = "INSERT INTO licenses(name, surname, email, company, license_name, license_type, license_id, old_license_id, quantity, license_exp_date, asp_invoice_date, asp_pay_due_date, YUNUS_term_days, YUNUS_pay_amount, YUNUS_pay_status, po_num, YUNUS_invo_num, sector, si_end_user, asp_invoice, asp_pay_term_days, comp_made_pay_status, ent_in_finsys_status, foreign_inst_date, notes)" +
                    " VALUES(@Name, @Surname, @Email, @Company, @LicenseName, @LicenseType, @LicenseID, @OldLicenseID, @Quantity, @LicenseExpDate, @AspInvoiceDate, @AspPayDueDate, @PtcTermDays, @PtcPayAmount, @PtcPayStatus, @PoNum, @PtcInvoNum, @Sector, @SiEndUser, @AspInvoice, @AspPayTermDays, @CompMadePayStatus, @EntInFinsysStatus, @ForeignInstDate, @Notes)";

                using MySqlCommand cmd = new(sqlQuery, connection);
                // parameters for the INSERT query here using cmd.Parameters.AddWithValue
cmd.Parameters.AddWithValue("@Name", fields[1]);
cmd.Parameters.AddWithValue("@Surname", fields[2]);
cmd.Parameters.AddWithValue("@Email", fields[3]);
cmd.Parameters.AddWithValue("@Company", fields[4]);
cmd.Parameters.AddWithValue("@LicenseName", fields[5]);
cmd.Parameters.AddWithValue("@LicenseType", fields[6]);
cmd.Parameters.AddWithValue("@LicenseID", fields[7]);
cmd.Parameters.AddWithValue("@OldLicenseID", fields[8]);
cmd.Parameters.AddWithValue("@Quantity", fields[9]);
cmd.Parameters.AddWithValue("@LicenseExpDate", fields[10]);
cmd.Parameters.AddWithValue("@AspInvoiceDate", fields[11]);
cmd.Parameters.AddWithValue("@AspPayDueDate", fields[12]);
cmd.Parameters.AddWithValue("@PtcTermDays", fields[13]);
cmd.Parameters.AddWithValue("@PtcPayAmount", fields[14]);
cmd.Parameters.AddWithValue("@PtcPayStatus", fields[15]);
cmd.Parameters.AddWithValue("@PoNum", fields[16]);
cmd.Parameters.AddWithValue("@PtcInvoNum", fields[17]);
cmd.Parameters.AddWithValue("@Sector", fields[18]);
cmd.Parameters.AddWithValue("@SiEndUser", fields[19]);
cmd.Parameters.AddWithValue("@AspInvoice", fields[20]);
cmd.Parameters.AddWithValue("@AspPayTermDays", fields[21]);
cmd.Parameters.AddWithValue("@CompMadePayStatus", fields[22]);
cmd.Parameters.AddWithValue("@EntInFinsysStatus", fields[23]);
cmd.Parameters.AddWithValue("@ForeignInstDate", fields[24]);
cmd.Parameters.AddWithValue("@Notes", fields[25]);


                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected > 0)
                {
                    return 1; // Successful insertion
                }
                else
                {
                    return 0; // Insertion failed
                }
            }
            //////////////////
//////////////////
//////////////////

            else if (fields[0].Equals("update"))
            {

  sqlQuery = "UPDATE licenses SET " +
    "name = CASE WHEN @Name <> 'NONE' THEN @Name ELSE name END, " +
    "surname = CASE WHEN @Surname <> 'NONE' THEN @Surname ELSE surname END, " +
    "email = CASE WHEN @Email <> 'NONE' THEN @Email ELSE email END, " +
    "company = CASE WHEN @Company <> 'NONE' THEN @Company ELSE company END, " +
    "license_name = CASE WHEN @LicenseName <> 'NONE' THEN @LicenseName ELSE license_name END, " +
    "license_type = CASE WHEN @LicenseType <> 'NONE' THEN @LicenseType ELSE license_type END, " +
    "license_id = CASE WHEN @LicenseID <> 'NONE' THEN @LicenseID ELSE license_id END, " +
    "old_license_id = CASE WHEN @OldLicenseID <> 'NONE' THEN @OldLicenseID ELSE old_license_id END, " +
    "quantity = CASE WHEN @Quantity <> 'NONE' THEN @Quantity ELSE quantity END, " +
    "license_exp_date = CASE WHEN @LicenseExpDate <> 'NONE' THEN @LicenseExpDate ELSE license_exp_date END, " +
    "asp_invoice_date = CASE WHEN @AspInvoiceDate <> 'NONE' THEN @AspInvoiceDate ELSE asp_invoice_date END, " +
    "asp_pay_due_date = CASE WHEN @AspPayDueDate <> 'NONE' THEN @AspPayDueDate ELSE asp_pay_due_date END, " +
    "YUNUS_term_days = CASE WHEN @PtcTermDays <> 'NONE' THEN @PtcTermDays ELSE YUNUS_term_days END, " +
    "YUNUS_pay_amount = CASE WHEN @PtcPayAmount <> 'NONE' THEN @PtcPayAmount ELSE YUNUS_pay_amount END, " +
    "YUNUS_pay_status = CASE WHEN @PtcPayStatus <> 'NONE' THEN @PtcPayStatus ELSE YUNUS_pay_status END, " +
    "po_num = CASE WHEN @PoNum <> 'NONE' THEN @PoNum ELSE po_num END, " +
    "YUNUS_invo_num = CASE WHEN @PtcInvoNum <> 'NONE' THEN @PtcInvoNum ELSE YUNUS_invo_num END, " +
    "sector = CASE WHEN @Sector <> 'NONE' THEN @Sector ELSE sector END, " +
    "si_end_user = CASE WHEN @SiEndUser <> 'NONE' THEN @SiEndUser ELSE si_end_user END, " +
    "asp_invoice = CASE WHEN @AspInvoice <> 'NONE' THEN @AspInvoice ELSE asp_invoice END, " +
    "asp_pay_term_days = CASE WHEN @AspPayTermDays <> 'NONE' THEN @AspPayTermDays ELSE asp_pay_term_days END, " +
    "comp_made_pay_status = CASE WHEN @CompMadePayStatus <> 'NONE' THEN @CompMadePayStatus ELSE comp_made_pay_status END, " +
    "ent_in_finsys_status = CASE WHEN @EntInFinsysStatus <> 'NONE' THEN @EntInFinsysStatus ELSE ent_in_finsys_status END, " +
    "foreign_inst_date = CASE WHEN @ForeignInstDate <> 'NONE' THEN @ForeignInstDate ELSE foreign_inst_date END, " +
    "notes = CASE WHEN @Notes <> 'NONE' THEN @Notes ELSE notes END " +
    "WHERE ID = @ID";

    using MySqlCommand cmd = new(sqlQuery, connection);

    // Include ID parameter to specify which row to update
    cmd.Parameters.AddWithValue("@ID", fields[26]);

    // Set parameters for the UPDATE query using cmd.Parameters.AddWithValue
cmd.Parameters.AddWithValue("@Name", fields[1]);
cmd.Parameters.AddWithValue("@Surname", fields[2]);
cmd.Parameters.AddWithValue("@Email", fields[3]);
cmd.Parameters.AddWithValue("@Company", fields[4]);
cmd.Parameters.AddWithValue("@LicenseName", fields[5]);
cmd.Parameters.AddWithValue("@LicenseType", fields[6]);
cmd.Parameters.AddWithValue("@LicenseID", fields[7]);
cmd.Parameters.AddWithValue("@OldLicenseID", fields[8]);
cmd.Parameters.AddWithValue("@Quantity", fields[9]);
cmd.Parameters.AddWithValue("@LicenseExpDate", fields[10]);
cmd.Parameters.AddWithValue("@AspInvoiceDate", fields[11]);
cmd.Parameters.AddWithValue("@AspPayDueDate", fields[12]);
cmd.Parameters.AddWithValue("@PtcTermDays", fields[13]);
cmd.Parameters.AddWithValue("@PtcPayAmount", fields[14]);
cmd.Parameters.AddWithValue("@PtcPayStatus", fields[15]);
cmd.Parameters.AddWithValue("@PoNum", fields[16]);
cmd.Parameters.AddWithValue("@PtcInvoNum", fields[17]);
cmd.Parameters.AddWithValue("@Sector", fields[18]);
cmd.Parameters.AddWithValue("@SiEndUser", fields[19]);
cmd.Parameters.AddWithValue("@AspInvoice", fields[20]);
cmd.Parameters.AddWithValue("@AspPayTermDays", fields[21]);
cmd.Parameters.AddWithValue("@CompMadePayStatus", fields[22]);
cmd.Parameters.AddWithValue("@EntInFinsysStatus", fields[23]);
cmd.Parameters.AddWithValue("@ForeignInstDate", fields[24]);
cmd.Parameters.AddWithValue("@Notes", fields[25]);
 
    int rowsAffected = cmd.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        return 1; // Successful update
    }
    else
    {
        return 0; // Update failed
    }

            }
            //////////////////
//////////////////
//////////////////

            else if (fields[0].Equals("delete"))
            {
                 
  sqlQuery = "DELETE FROM licenses " +

    "WHERE ID = @ID";

    using MySqlCommand cmd = new(sqlQuery, connection);

    // Include ID parameter to specify which row to update
    cmd.Parameters.AddWithValue("@ID", fields[26]);

     
 
    int rowsAffected = cmd.ExecuteNonQuery();

    if (rowsAffected > 0)
    {
        return 1; // Successful update
    }
    else
    {
        return 0; // Update failed
    }
 
            }


//////////////////
//////////////////
//////////////////



        }

        return 0; // If none of the conditions match
    }
    catch (Exception e)
    {
        Console.WriteLine("Oh no! SQL error: " + e);
        return 0;
    }
}

 

    }


    }