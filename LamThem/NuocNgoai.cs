using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
    public class NuocNgoai : DuLich
    {
        public decimal LePhiVisa { get; set; }
        public string HangMayBay { get; set; }
        public decimal GiaVeMayBay { get; set; }

        public NuocNgoai(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string triHoan, string loaiTour, decimal lePhiVisa, string hangMayBay, decimal giaVeMayBay)
            : base(maTour, diemDi, diemDen, ngayDi, ngayVe, gia, triHoan, loaiTour)
        {
            LePhiVisa = lePhiVisa;
            HangMayBay = hangMayBay;
            GiaVeMayBay = giaVeMayBay;
        }

        public NuocNgoai()
        {

        }

        public static int AddNuocNgoai(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string loaiTour, decimal lePhiVisa, string hangMayBay, decimal giaVeMayBay)
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
                command.CommandText = "INSERT INTO TrongTinh (MaTour, LePhiVisa, HangMayBay , GiaVeMayBay ) VALUES (@MaTour, @LePhiVisa, @HangMayBay , @GiaVeMayBay)";
                command.Parameters.AddWithValue("@MaTour", maTour);
                command.Parameters.AddWithValue("@LePhiVisa", lePhiVisa);
                command.Parameters.AddWithValue("@HangMayBay", hangMayBay);
                command.Parameters.AddWithValue("@GiaVeMayBay", giaVeMayBay);

                var rows_affected = command.ExecuteNonQuery();
                return rows_affected;

            }
        }

    }
}
