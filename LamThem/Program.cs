using ConsoleTables;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;

namespace LamThem
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                int choose = 0;
                do
                {
                    Console.WriteLine("------Quan Li Du Lich--------");
                    Console.WriteLine("1. Them thong tin du lich");
                    Console.WriteLine("2. Hien thi thong tin cac loai du lich");
                    Console.WriteLine("3. Cap nhap thong tin cac loai du lich");
                    Console.WriteLine("4. Thong ke theo ngay di va ngay den");
                    Console.WriteLine("5. Xoa thong tin tour Ha Noi");
                    Console.WriteLine("6. Thoat");
                    choose = int.Parse(Console.ReadLine());
                }
                while (choose > 6 || choose < 1);

                switch (choose)
                {
                    case 1:
                        int chooseTourType = 0;
                        do
                        {
                            Console.WriteLine("Chon loai tour:");
                            Console.WriteLine("1. Tour trong tinh");
                            Console.WriteLine("2. Tour ngoai tinh");
                            Console.WriteLine("3. Tour nuoc ngoai");
                            chooseTourType = int.Parse(Console.ReadLine());
                        }
                        while (chooseTourType < 1 || chooseTourType > 3);
                        switch (chooseTourType)
                        {
                            case 1:
                                Console.WriteLine("Nhap thong tin tour trong tinh:");
                                var thongTinChung = NhapThongTinChung(chooseTourType);
                                Console.Write("Dua Don: ");
                                string duaDon = Console.ReadLine();
                                int result = TrongTinh.AddTrongTinh(thongTinChung.maTour, thongTinChung.diemDi, thongTinChung.diemDen, thongTinChung.ngayDi, thongTinChung.ngayVe, thongTinChung.gia, thongTinChung.loaiTour, duaDon);
                                if (result != 0)
                                {
                                    Console.WriteLine("Them thong tin tour trong tinh thanh công");
                                }
                                else
                                {
                                    Console.WriteLine("Them thong tin tour that bai");
                                }
                                break;

                            case 2:
                                Console.WriteLine("Nhap thong tin tour ngoai tinh:");
                                var thongTinChung2 = NhapThongTinChung(chooseTourType);

                                Console.WriteLine("Nhap Hang May Bay: ");
                                string hangMayBay = Console.ReadLine();

                                Console.WriteLine("Nhap Gia Ve : ");
                                decimal giaVe = decimal.Parse(Console.ReadLine());

                                int result2 = NgoaiTinh.AddNgoaiTinh(thongTinChung2.maTour, thongTinChung2.diemDi, thongTinChung2.diemDen, thongTinChung2.ngayDi, thongTinChung2.ngayVe, thongTinChung2.gia, thongTinChung2.loaiTour, hangMayBay, giaVe);
                                if (result2 != 0)
                                {
                                    Console.WriteLine("Them thong tin tour ngoai tinh thanh công");
                                }
                                else
                                {
                                    Console.WriteLine("Them thong tin tour that bai");
                                }
                                break;

                            case 3:
                                Console.WriteLine("Nhap thong tin tour nuoc ngoai:");
                                var thongTinChung3 = NhapThongTinChung(chooseTourType);
                                Console.WriteLine("Nhap Hang May Bay: ");
                                string hangMayBayNN = Console.ReadLine();

                                Console.WriteLine("Nhap Gia Ve : ");
                                decimal giaVeNN = decimal.Parse(Console.ReadLine());

                                Console.WriteLine("Nhap Le Phi Visa : ");
                                decimal lePhiVisa = decimal.Parse(Console.ReadLine());
                                int result3 = NuocNgoai.AddNuocNgoai(thongTinChung3.maTour, thongTinChung3.diemDi, thongTinChung3.diemDen, thongTinChung3.ngayDi, thongTinChung3.ngayVe, thongTinChung3.gia, thongTinChung3.loaiTour, lePhiVisa, hangMayBayNN, giaVeNN);

                                if (result3 != 0)
                                {
                                    Console.WriteLine("Them thong tin tour nuoc ngoai thanh công");
                                }
                                else
                                {
                                    Console.WriteLine("Them thong tin tour that bai");
                                }
                                break;
                        }
                        break;
                    case 2:
                        List<DuLich> list = new List<DuLich>();
                        var dulich = DuLich.GetTourInfo();
                        foreach (var item in dulich)
                        {
                            list.Add(item);
                        }
                        break;

                    case 3:
                        DuLich.UpdateDiemNong();
                        Console.WriteLine("Da cap nhat thong tin du lich tai cac diem nong covid");
                        break;

                    case 4:
                        DuLich.ThongKe();
                        break;

                    case 5:
                        DuLich.DeleteHaNoi();
                        break;
                }
            }

        }

        public static bool IsMaTourUnique(string maTour)
        {
            using (var connection = new SqlConnection(ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString))
            {
                connection.Open();
                using (var command = new SqlCommand("SELECT COUNT(*) FROM DuLich WHERE MaTour = @MaTour", connection))
                {
                    command.Parameters.AddWithValue("@MaTour", maTour);
                    int count = (int)command.ExecuteScalar();
                    return count == 0;
                }
            }
        }
       
        public static (string maTour, string diemDi, string diemDen, DateTime ngayDi, DateTime ngayVe, decimal gia, string loaiTour) NhapThongTinChung(int tourType)
        {
            Console.Write("Ma tour: ");
            string maTour = Console.ReadLine();

            if (!IsMaTourUnique(maTour))
            {
                throw new Exception("MaTour da ton tai, vui long nhap Ma Tour khac! ");
            }

            Console.Write("Diem di: ");
            string diemDi = Console.ReadLine();
            Console.Write("Diem den: ");
            string diemDen = Console.ReadLine();
            DateTime ngayDi, ngayVe;

            do
            {
                Console.Write("Ngay di (dd/MM/yyyy): ");
                string ngayDiInput = Console.ReadLine();
                var (ngayDiHopLe, ngayDiDateTime) = IsValidDateTime(ngayDiInput, "dd/MM/yyyy");

                if (!ngayDiHopLe)
                {
                    Console.WriteLine("Ngay di khong hop le. Nhap lai.");
                }
                else
                {
                    Console.Write("Ngay ve (dd/MM/yyyy): ");
                    string ngayVeInput = Console.ReadLine();
                    var (ngayVeHopLe, ngayVeDateTime) = IsValidDateTime(ngayVeInput, "dd/MM/yyyy");

                    if (!ngayVeHopLe)
                    {
                        Console.WriteLine("Ngay ve khong hop le. Nhap lai.");
                    }
                    else
                    {
                        try
                        {
                            CheckNgayDiDen(ngayDiDateTime, ngayVeDateTime);
                            ngayDi = ngayDiDateTime;
                            ngayVe = ngayVeDateTime;
                            break; // Ngày thời gian hợp lệ, thoát khỏi vòng lặp.
                        }
                        catch (TourDateException ex)
                        {
                            Console.WriteLine($"Co loi xay ra: {ex.Message}. Nhap lai ngay di va ngay ve.");
                        }
                    }
                }
            } while (true); // Vòng lặp lặp lại cho đến khi có ngày thời gian hợp lệ.


            // Gia Tour

            Console.Write("Gia tour: ");
            decimal giaTour = decimal.Parse(Console.ReadLine());

            if ((tourType == 1 && giaTour < 2000000) || (tourType == 2 && giaTour < 5000000) || (tourType == 3 && giaTour < 15000000))
            {
                throw new TourPriceException($"Gia tour phai lon hon hoac bang {(tourType == 1 ? "2.000.000" : (tourType == 2 ? "5.000.000" : "15.000.000"))} VND.");
            }


            //loaiTour

            string loaiTour = ""; 

            if (tourType == 1)
            {
                loaiTour = "TrongTinh";
            }
            else if (tourType == 2)
            {
                loaiTour = "NgoaiTinh";
            }
            else if (tourType == 3)
            {
                loaiTour = "NuocNgoai";
            }

            return (maTour, diemDi, diemDen, ngayDi, ngayVe, giaTour, loaiTour);

           
            
        } 

        //public static DateTime InputDatime(string option)
        //{
        //    DateTime date;
        //    bool check = true;
        //    Console.WriteLine($"Nhap {option}");
        //    // sua lai cai do while nhe. Anh nhap sai no     
        //    do
        //    {
        //        string input = Console.ReadLine();
        //        var (isDateTime, dateTime) = IsValidDateTime(input, "dd/MM/yyyy");
        //        if (!isDateTime)
        //            throw new TourDateException("Ngay khong hop le");
        //        else
        //        {
        //            date = dateTime;
        //            check = false;
        //        }

        //    } while (check);

        //    return date;
        //}

        private static (bool isDateTime, DateTime dateTime) IsValidDateTime(string dateTime, string format)
        {
            DateTime date;
            return (DateTime.TryParseExact(
                dateTime,
                format,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out date), date);
        }

        public static void CheckNgayDiDen(DateTime ngayDi, DateTime ngayDen)
        {
            if (DateTime.Compare(ngayDi, ngayDen) > 0 || DateTime.Compare(ngayDen, DateTime.Now.Date) <= 0)
            {
                throw new TourDateException("Ngay ve phai lon hon ngay di va lon hon ngay hien tai");
            }
        }


    }
}


