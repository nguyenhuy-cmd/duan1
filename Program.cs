using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;

class Phim
{
    public string TenPhim { get; set; } = string.Empty;
    public string DaoDien { get; set; } = string.Empty;
    public int NamSanXuat { get; set; }
    public string TheLoai { get; set; } = string.Empty;
    public string DienVien { get; set; } = string.Empty;
    public string QuocGia { get; set; } = string.Empty;
    public string ThoiLuong { get; set; } = string.Empty;
}

class Program
{
    static void DrawColoredBox(int x, int y, int width, int height, ConsoleColor color)
    {
        ConsoleColor oldBack = Console.BackgroundColor;
        Console.BackgroundColor = color;

        int maxWidth = Math.Max(0, Math.Min(width, Console.WindowWidth - x));
        int maxHeight = Math.Max(0, Math.Min(height, Console.WindowHeight - y));

        for (int row = 0; row < maxHeight; row++)
        {
            for (int col = 0; col < maxWidth; col++)
            {
                Console.SetCursorPosition(x + col, y + row);
                Console.Write(" ");
            }
        }

        Console.BackgroundColor = oldBack;
    }
    static void WriteTextInBox(int x, int y, string text, ConsoleColor bgColor, ConsoleColor fgColor = ConsoleColor.White)
    {
        ConsoleColor oldBg = Console.BackgroundColor;
        ConsoleColor oldFg = Console.ForegroundColor;

        Console.BackgroundColor = bgColor;
        Console.ForegroundColor = fgColor;

        for (int i = 0; i < text.Length; i++)
        {
            Console.SetCursorPosition(x + i, y);
            Console.Write(text[i]);
        }

        Console.BackgroundColor = oldBg;
        Console.ForegroundColor = oldFg;
    }
    static void ShowMenuArt()
    {
        string[] menuArt = {
            @" __  __  _____  _   _  _   _      ____   _    _  _  __  __",
            @"|  \/  || ____|| \ | || | | |    |  _ \ | |  | || ||  \/  |",
            @"| |\/| ||  _|  |  \| || | | |    | |_) || |__| || || |\/| |",
            @"| |  | || |___ | |\  || |_| |    |  __/ | |__| || || |  | |",
            @"|_|  |_||_____||_| \_||_____|    |_|    |_|  |_||_||_|  |_|",
        };
        int startY = 1;
        int startX = (Console.WindowWidth - menuArt[0].Length) / 2;
        ConsoleColor oldBg = Console.BackgroundColor;
        ConsoleColor oldFg = Console.ForegroundColor;
        for (int i = 0; i < menuArt.Length; i++)
        {
            Console.SetCursorPosition(startX, startY + i);
            for (int j = 0; j < menuArt[i].Length; j++)
            {
                Console.Write(menuArt[i][j]);
            }
        }
        Console.BackgroundColor = oldBg;
        Console.ForegroundColor = oldFg;
    }

    static void ShowMovieList(List<Phim> danhSachPhim, int chon)
    {
        int startY = 1;
        int menuArtHeight = 5;
        string tieuDe = "DANH SÁCH PHIM";
        int tieuDeX = 2;
        int tieuDeY = startY + menuArtHeight + 1;
        Console.ForegroundColor = ConsoleColor.DarkMagenta;
        Console.SetCursorPosition(tieuDeX, tieuDeY);
        Console.WriteLine(tieuDe);
        Console.ResetColor();
        int maxRows = Console.WindowHeight - 10;
        int phimToShow = Math.Min(danhSachPhim.Count, maxRows);
        for (int i = 0; i < phimToShow; i++)
        {
            int x = 2;
            int y = tieuDeY + 2 + i;
            if (y >= Console.WindowHeight - 1) break;
            Console.SetCursorPosition(x, y);
            if (i + 1 == chon)
            {
                Console.Write($"> {i + 1}. {danhSachPhim[i].TenPhim}   ");
            }
            else
            {
                Console.Write($"  {i + 1}. {danhSachPhim[i].TenPhim}   ");
            }
        }
    }

    static void ShowMovieDetail(Phim phim, bool datVeSelected)
    {
        Console.Clear();
        Console.WriteLine("--- Thông tin chi tiết ---");
        Console.WriteLine($"Tên phim: {phim.TenPhim}");
        Console.WriteLine($"Thể loại: {phim.TheLoai}");
        Console.WriteLine($"Diễn viên: {phim.DienVien}");
        Console.WriteLine($"Đạo diễn: {phim.DaoDien}");
        Console.WriteLine($"Quốc gia: {phim.QuocGia}");
        Console.WriteLine($"Thời lượng: {phim.ThoiLuong}");
        Console.Write("\n");
        if (!datVeSelected)
            Console.Write("[Quay lại]    Đặt vé ");
        else
            Console.Write(" Quay lại   [Đặt vé]");
    }

    static void HandleMenuInput(ref int chon, int phimCount, ref bool inMenu)
    {
        ConsoleKeyInfo keyInfo = Console.ReadKey(true);
        if (inMenu && (keyInfo.Key == ConsoleKey.UpArrow || keyInfo.Key == ConsoleKey.LeftArrow))
        {
            chon--;
            if (chon < 1) chon = phimCount;
        }
        else if (inMenu && (keyInfo.Key == ConsoleKey.DownArrow || keyInfo.Key == ConsoleKey.RightArrow))
        {
            chon++;
            if (chon > phimCount) chon = 1;
        }
        else if (inMenu && keyInfo.Key == ConsoleKey.Enter)
        {
            inMenu = false;
        }
        else if (!inMenu && keyInfo.Key == ConsoleKey.Escape)
        {
            inMenu = true;
        }
        else if (inMenu && keyInfo.Key == ConsoleKey.Escape)
        {
            Console.WriteLine("\nKết thúc chương trình.");  
            Environment.Exit(0);
        }
    }

    static void HandleDetailInput(ref bool datVeSelected, ref bool inMenu)
    {
        var detailKey = Console.ReadKey(true);
        if (detailKey.Key == ConsoleKey.LeftArrow || detailKey.Key == ConsoleKey.RightArrow)
        {
            datVeSelected = !datVeSelected;
        }
        else if (detailKey.Key == ConsoleKey.Enter)
        {
            if (datVeSelected)
            {
                // Có thể thêm chức năng đặt vé ở đây nếu muốn
            }
            inMenu = true;
        }
        else if (detailKey.Key == ConsoleKey.Escape)
        {
            inMenu = true;
        }
    }

    static void Main(string[] args)
    {
        Console.Clear();
        KiemTraKetNoiMySQL();
        List<Phim> danhSachPhim = LayDanhSachPhimTuMySQL();
        int chon = 1;
        bool inMenu = true;
        while (true)
        {
            if (inMenu)
            {
                ShowMenuArt();
                ShowMovieList(danhSachPhim, chon);
            }
            HandleMenuInput(ref chon, danhSachPhim.Count, ref inMenu);
            if (!inMenu)
            {
                bool datVeSelected = false;
                while (!inMenu)
                {
                    ShowMovieDetail(danhSachPhim[chon - 1], datVeSelected);
                    HandleDetailInput(ref datVeSelected, ref inMenu);
                }
            }
        }
    }
    static void KiemTraKetNoiMySQL()
    {
        string connectionString = "server=localhost;user=root;password=140923;database=phimdb;";
        using (var connection = new MySqlConnection(connectionString))
        {
            try
            {
                connection.Open();
                Console.WriteLine("Kết nối MySQL thành công!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Kết nối thất bại: " + ex.Message);
            }
        }
    }
    static List<Phim> LayDanhSachPhimTuMySQL()
    {
        var ds = new List<Phim>();
        string connectionString = "server=localhost;user=root;password=140923;database=phimdb;";
        using (var connection = new MySqlConnection(connectionString))
        {
            connection.Open();
            var cmd = new MySqlCommand("SELECT tenphim, daodien, namsanxuat, theloai, dienvien, quocgia, thoiluong FROM phim", connection);
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    ds.Add(new Phim
                    {
                        TenPhim = reader.GetString(0),
                        DaoDien = reader.GetString(1),
                        NamSanXuat = reader.GetInt32(2),
                        TheLoai = reader.GetString(3),
                        DienVien = reader.GetString(4),
                        QuocGia = reader.GetString(5),
                        ThoiLuong = reader.GetString(6)
                    });
                }
            }
        }
        return ds;
    }
}