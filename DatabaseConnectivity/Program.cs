using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;

namespace DatabaseConnectivity;

class Program
{
    static string ConnectionString = "Data Source=DESKTOP-DH01RE2;Initial Catalog=db_hr_sibkm;Integrated Security=True;Connect Timeout=30;";

    static SqlConnection connection;
    static void Main(string[] args)
    {
        GetAllRegion();
        //InsertRegion("CahayaAsia");
        //GetAllRegion();
        //UpdateRegionById(6, "SouthEast Asia");
        //UpdateRegionById();
        //DeleteRegionById(5);
         
    }

    // GETALL : REGION (Command)
    public static void GetAllRegion()
    {
        connection = new SqlConnection(ConnectionString);

        //Membuat instance untuk command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM region";

        //Membuka koneksi
        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Console.WriteLine("Id: " + reader[0]);
                Console.WriteLine("Name: " + reader[1]);
                Console.WriteLine("====================");
            }
        }
        else
        {
            Console.WriteLine("Data not found!");
        }
        reader.Close();
        connection.Close();
    }

    // GETBYID : REGION (Command)
    public static void GetRegionById(int id)
    {
        //menginstance conection string ke dalam sql connection
        connection = new SqlConnection(ConnectionString);

        //Membuat instance untuk command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM region WHERE id = @id";

        //Membuka koneksi
        connection.Open();

        //membuat parameter dan memberikan value id
        SqlParameter pId = new SqlParameter();
        pId.ParameterName = "@id";
        pId.Value = id;
        pId.SqlDbType = SqlDbType.Int;

        //menambahkan value ke dalam command untuk variabel id
        command.Parameters.Add(pId);

        //mneggunakan sqldatareader utk mengeksekusi query
        using SqlDataReader reader = command.ExecuteReader();
        //menampilkan hasil query reader dari command
        if (reader.HasRows)
        {
            while (reader.Read())
            {
                Console.WriteLine("Id: " + reader[0]);
                Console.WriteLine("Name: " + reader[1]);
                Console.WriteLine("====================");
            }
        }
        else //jika data dengan id yang dcari tidak ada
        {
            Console.WriteLine("Data not found!");
        }
        //mengakhiri penggunaan sql data reader
        reader.Close();
        //menutup koneksi ke database
        connection.Close();
    }

    // INSERT : REGION (Transaction)
    public static void InsertRegion(string name)
    {
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "INSERT INTO region (name) VALUES (@name)";
            command.Transaction = transaction;

            //Membuat parameter
            SqlParameter pName = new SqlParameter();
            pName.ParameterName = "@name";
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;

            //Menambahkan parameter ke command
            command.Parameters.Add(pName);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            if (result > 0)
            {
                Console.WriteLine("Data berhasil ditambahkan!");
            }
            else
            {
                Console.WriteLine("Data gagal ditambahkan!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }

    // UPDATE : REGION (Transaction)
    public static void UpdateRegionById(int id, string name)
    {
        //menginstance conection string ke dalam sql connection
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "UPDATE region SET name=@name WHERE id=@id";
            command.Transaction = transaction;

            //Membuat parameter dan mnegisi value id
            SqlParameter pId = new SqlParameter();
            //mengambil nama parameter yang akan di isi
            pId.ParameterName = "@id";
            //mengisi parameter id dari parameter method update 
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;

            //Membuat parameter dan mnegisi value name
            SqlParameter pName = new SqlParameter();
            //mengambil nama parameter yang akan di isi
            pName.ParameterName = "@name";
            //mengisi parameter name dari parameter method update 
            pName.Value = name;
            pName.SqlDbType = SqlDbType.VarChar;


            //Menambahkan parameter ke command
            command.Parameters.Add(pId);
            command.Parameters.Add(pName);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            //jika data berhasil diperbarui dengan jumlah lebih dari 0 maka data dihapus dan sebaliknya
            if (result > 0)
            {
                Console.WriteLine("Data berhasil diperbarui!");
            }
            else
            {
                Console.WriteLine("Data gagal diperbarui!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }

    // DELETE : REGION (Transaction)
    public static void DeleteRegionById(int id)
    {
        //menginstance conection string ke dalam sql connection
        connection = new SqlConnection(ConnectionString);

        //Membuka koneksi
        connection.Open();

        // menggunakan transact sql untuk memulai query delete
        SqlTransaction transaction = connection.BeginTransaction();
        try
        {
            //Membuat instance untuk command
            SqlCommand command = new SqlCommand();
            command.Connection = connection;
            command.CommandText = "DELETE FROM region WHERE id = @id";
            command.Transaction = transaction;

            //Membuat parameter dan mnegisi value id
            SqlParameter pId = new SqlParameter();
            //mengambil nama parameter yang akan di isi
            pId.ParameterName = "@id";
            //mengisi parameter id dari parameter method delete 
            pId.Value = id;
            pId.SqlDbType = SqlDbType.Int;

            //Menambahkan parameter ke command
            command.Parameters.Add(pId);

            //Menjalankan command
            int result = command.ExecuteNonQuery();
            transaction.Commit();

            //jika data berhasil dihapus dengan jumlah lebih dari 0 maka data dihapus dan sebaliknya
            if (result > 0)
            {
                Console.WriteLine("Data berhasil dihapus!");
            }
            else
            {
                Console.WriteLine("Data gagal dihapus!");
            }

            //Menutup koneksi
            connection.Close();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
            try
            {
                transaction.Rollback();
            }
            catch (Exception rollback)
            {
                Console.WriteLine(rollback.Message);
            }
        }

    }
}