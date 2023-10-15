using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LamThem
{
    public class DuLich
    {
        public string MaTour { get; set; }
        public string DiemDi { get; set; }
        public string DiemDen { get; set; }
        public DateTime NgayDi { get; set; }
        public DateTime NgayVe { get; set; }

        public decimal Gia { get; set; }

        public string TriHoan { get; set; }

        public string LoaiTour { get; set; }

        private List<DuLich> _duLich = new List<DuLich>();
        public DuLich() { }

        public DuLich(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string triHoan, string loaiTour)
        {
            maTour = MaTour;
            diemDi = DiemDi;
            diemDen = DiemDen;
            ngayDi = NgayDi;
            ngayVe = NgayVe;
            gia = Gia;
            triHoan = TriHoan;
            loaiTour = LoaiTour;
        }
        public static int AddTourBase(string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string loaiTour)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                var command = new SqlCommand();
                connection.Open();
                command.Connection = connection;
                string triHoan = "Không";

                command.CommandText = "INSERT INTO DuLich (MaTour, DiemDi, DiemDen, NgayDi, NgayVe, Gia, TriHoan, LoaiTour) VALUES (@MaTour, @DiemDi, @DiemDen, @NgayDi, @NgayVe, @Gia, @TriHoan, @LoaiTour)";
                command.Parameters.AddWithValue("@MaTour", maTour);
                command.Parameters.AddWithValue("@DiemDi", diemDi);
                command.Parameters.AddWithValue("@DiemDen", diemDen);
                command.Parameters.AddWithValue("@NgayDi", ngayDi);
                command.Parameters.AddWithValue("@NgayVe", ngayVe);
                command.Parameters.AddWithValue("@Gia", gia);
                command.Parameters.AddWithValue("@TriHoan", triHoan);
                command.Parameters.AddWithValue("@LoaiTour", loaiTour);
                var rows_affected = command.ExecuteNonQuery();
                return rows_affected;
            }
        }

        public static List<DuLich> GetTourInfo()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                List<DuLich> list = new List<DuLich>();
                string query = "SELECT * FROM DuLich ORDER BY NgayDi ASC , MaTour DESC ";
                connection.Open();
                SqlCommand command = new SqlCommand(query, connection);
                SqlDataReader reader = command.ExecuteReader();
                var table = new ConsoleTable("MaTour", "DiemDi", "DiemDen", "NgayDi", "NgayVe", "Gia", "TriHoan");
                while (reader.Read())
                {
                    DuLich item = new DuLich();
                    item.MaTour = reader["MaTour"].ToString();
                    item.DiemDi = reader["DiemDi"].ToString();
                    item.DiemDen = reader["DiemDen"].ToString();
                    item.NgayDi = DateTime.Parse(reader["NgayDi"].ToString());
                    item.NgayVe = DateTime.Parse(reader["NgayVe"].ToString());
                    item.Gia = Decimal.Parse(reader["Gia"].ToString());
                    item.TriHoan = reader["TriHoan"].ToString();
                    list.Add(item);
                }
                foreach (var item in list)
                {
                    table.AddRow(item.MaTour, item.DiemDi, item.DiemDen, item.NgayDi, item.NgayVe, item.Gia, item.TriHoan);
                }
                table.Write();
                return list;
            }
            Console.ReadLine();
        }

        public static void ThongKe()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();
                string thongKequery = "SELECT LoaiTour, COUNT(LoaiTour) AS \"SoLuong\" FROM DULICH WHERE TriHoan = 'Có' GROUP BY LoaiTour";
                SqlCommand command = new SqlCommand(thongKequery, connection);
                using (var reader = command.ExecuteReader())
                {
                    Console.WriteLine("Tong so tour du lich bi tri hoan");
                    while (reader.Read())
                    {
                        string loaitour = reader["LoaiTour"].ToString();
                        int soluong = int.Parse(reader["SoLuong"].ToString());
                        Console.WriteLine(loaitour + "\t" + soluong);
                    }
                }
            }
        }
        public static void UpdateDiemNong()
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();

                string updateDateQuery = "UPDATE dulich SET NgayDi = DATEADD(day, 14, NgayDi), NgayVe = DATEADD(day, 14, NgayVe) WHERE DiemDen IN ('HCM', 'Da Nang', 'Ha Noi', 'Nghe An')";

                using (SqlCommand cmd = new SqlCommand(updateDateQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }

                string updateTriHoanQuery = "UPDATE dulich SET TriHoan = 'Có' WHERE DiemDen IN  ('HCM', 'Da Nang', 'Ha Noi', 'Nghe An')";
                using (SqlCommand cmd = new SqlCommand(updateTriHoanQuery, connection))
                {
                    cmd.ExecuteNonQuery();
                }
                GetTourInfo();
                //string query = "SELECT * FROM DuLich ";
                //List<DuLich> list = new List<DuLich>();

                //using (SqlCommand cmd = new SqlCommand(query, connection))
                //{
                //    SqlDataReader reader = cmd.ExecuteReader();
                //    var table = new ConsoleTable("MaTour", "DiemDi", "DiemDen", "NgayDi", "NgayVe", "Gia", "TriHoan");
                //    while (reader.Read())
                //    {
                //        DuLich item = new DuLich();
                //        item.MaTour = reader["MaTour"].ToString();
                //        item.DiemDi = reader["DiemDi"].ToString();
                //        item.DiemDen = reader["DiemDen"].ToString();
                //        item.NgayDi = DateTime.Parse(reader["NgayDi"].ToString());
                //        item.NgayVe = DateTime.Parse(reader["NgayVe"].ToString());
                //        item.Gia = Decimal.Parse(reader["Gia"].ToString());
                //        item.TriHoan = reader["TriHoan"].ToString();
                //        list.Add(item);
                //    }
                //    foreach (var item in list)
                //    {
                //        table.AddRow(item.MaTour, item.DiemDi, item.DiemDen, item.NgayDi, item.NgayVe, item.Gia, item.TriHoan);
                //    }
                //    table.Write();
                //}

            }
        }
        public static void DeleteHaNoi()
        {
            bool recordsDeleted = false; // Biến để kiểm tra xem có bản ghi nào được xóa hay không

            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand("DELETE FROM TrongTinh WHERE MaTour IN (SELECT dl.MaTour FROM DuLich dl WHERE dl.DiemDi = 'Ha Noi' AND dl.NgayDi > GETDATE())", connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        recordsDeleted = true;
                    }
                }

                using (SqlCommand cmd = new SqlCommand("DELETE FROM NgoaiTinh WHERE MaTour IN (SELECT dl.MaTour FROM DuLich dl WHERE dl.DiemDi = 'Ha Noi' AND dl.NgayDi > GETDATE())", connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        recordsDeleted = true;
                    }
                }

                using (SqlCommand cmd = new SqlCommand("DELETE FROM NuocNgoai WHERE MaTour IN (SELECT dl.MaTour FROM DuLich dl WHERE dl.DiemDi = 'Ha Noi' AND dl.NgayDi > GETDATE())", connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        recordsDeleted = true;
                    }
                }

                using (SqlCommand cmd = new SqlCommand("DELETE FROM DuLich WHERE DiemDi = 'Ha Noi' AND NgayDi > GETDATE()", connection))
                {
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        recordsDeleted = true;
                    }
                }
            }

            if (recordsDeleted)
            {
                Console.WriteLine("Xoa du lieu thanh cong");
                GetTourInfo();
            }
            else
            {
                Console.WriteLine("Khong co tour nao den Ha Noi");
            }
        }

    }
}

