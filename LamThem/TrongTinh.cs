using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
   public class TrongTinh : DuLich
    {
        public string DuaDon { get; set; }

        public TrongTinh(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string triHoan, string loaiTour, string duaDon)
            : base(maTour, diemDi, diemDen, ngayDi, ngayVe, gia, triHoan, loaiTour)
        {
           DuaDon = duaDon;
        }
        public TrongTinh()
        {

        }

        public static int AddTrongTinh(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string loaiTour,string duaDon)
        {
           

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();
               

                int baseResult = AddTourBase(maTour, diemDi, diemDen, ngayDi, ngayVe, gia, loaiTour);
                if(baseResult == 0)
                {
                    return 0;
                }
                var command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "INSERT INTO TrongTinh (MaTour, DuaDon) VALUES (@MaTour, @DuaDon)";
                command.Parameters.AddWithValue("@MaTour", maTour);
                command.Parameters.AddWithValue("@DuaDon", duaDon);

                var rows_affected = command.ExecuteNonQuery();
                return rows_affected;

            }
        }

       

    }


}
