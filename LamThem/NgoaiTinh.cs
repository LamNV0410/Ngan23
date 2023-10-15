using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
   public class NgoaiTinh :DuLich 
    {
        public string HangMayBay { get; set; }

        public decimal GiaVeMayBay { get; set; }
        public NgoaiTinh(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia,string triHoan, string loaiTour, string hangMayBay, decimal giaVeMayBay)
           : base(maTour, diemDi, diemDen, ngayDi, ngayVe, gia, triHoan, loaiTour)
        {
            HangMayBay = hangMayBay;
            GiaVeMayBay = giaVeMayBay;
        }
        public NgoaiTinh() {  }

        public static int AddNgoaiTinh(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia,string loaiTour, string hangMayBay, decimal giaVeMayBay)
        {


            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();


                int baseResult = AddTourBase(maTour, diemDi, diemDen, ngayDi, ngayVe, gia, loaiTour);
                if (baseResult == 0)
                {
                    return 0;
                }
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO NgoaiTinh (MaTour, HangMayBay, GiaVeMayBay) VALUES (@MaTour, @HangMayBay, @GiaVeMayBay)";
                command.Parameters.AddWithValue("@MaTour", maTour);
                command.Parameters.AddWithValue("@HangMayBay", hangMayBay);
                command.Parameters.AddWithValue("@GiaVeMayBay", giaVeMayBay);

                var rows_affected = command.ExecuteNonQuery();
                return rows_affected;

            }
        }
    }
}
