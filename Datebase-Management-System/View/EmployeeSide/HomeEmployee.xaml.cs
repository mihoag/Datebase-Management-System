using HospitalManagement.model;
using HospitalManagement.Utils;
using iTextSharp.text.pdf;
using iTextSharp.text;
using Microsoft.Data.SqlClient;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Reflection;
using System.Runtime.ConstrainedExecution;

namespace HospitalManagement.View.EmployeeSide
{
    /// <summary>
    /// Interaction logic for HomeEmployee.xaml
    /// </summary>
    public partial class HomeEmployee : Window
    {
        public static  string id_nv;
        List<Medicine> listMedicine = new List<Medicine>();
        int MedicineNumber = 0;
        public HomeEmployee(string id)
        {
            id_nv = id;
            InitializeComponent();
        }

        private void logout(object sender, RoutedEventArgs e)
        {
            MainWindow mainWD = new MainWindow();
            mainWD.Show();
            this.Close();
        }

        private void setSchedule(object sender, RoutedEventArgs e)
        {
            ListFreeTimeDentist listFreeTimeDentistWD = new ListFreeTimeDentist();
            listFreeTimeDentistWD.Show();   
            this.Close();
        }

     

        private void patientRecord(object sender, RoutedEventArgs e)
        {
            patientRecord patientRecord = new patientRecord();
            patientRecord.Show();   
            this.Close();
        }


        private void showMedicine(object sender, RoutedEventArgs e)
        {
            showMedicine showMedicineWD = new showMedicine();
            showMedicineWD.Show();
            this.Close();
        }

        private void deleteBill(object sender, RoutedEventArgs e)
        {
            deleteBill deleteBill = new deleteBill();
            deleteBill.Show();
            this.Close();
        }

        private void printError(object sender, RoutedEventArgs e)
        {
            LoadAllMedicine("sp_INDSTHUOC_ERROR");
            ExportPDF();
        }

        private void printFix(object sender, RoutedEventArgs e)
        {
            LoadAllMedicine("sp_INDSTHUOC_FIX");
            ExportPDF();
        }
        private void LoadAllMedicine(string proc)
        {
            listMedicine.Clear();
            try
            {
                using (SqlConnection connection = DB.Instance.Connection)
                {
                    using (SqlCommand cmd = new SqlCommand(proc, connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            // Check if the reader has rows
                            if (reader.HasRows)
                            {
                                reader.Read();
                                MedicineNumber = (int)reader["TONG_SO_THUOC"];
                                reader.NextResult();
                                while (reader.Read())
                                {
                                    // Access the columns in the result set
                                    string ID_Thuoc = reader["ID_THUOC"].ToString();
                                    string tenthuoc = reader["TENTHUOC"].ToString();
                                    string donvitinh = reader["DONVITINH"].ToString();
                                    string chidinh = reader["DONVITINH"].ToString();
                                    int tonkho = (int)reader["TONKHO"];
                                    string ngayhethan = reader["NGAYHETHAN"].ToString();
                                    int dongia = (int)reader["DONGIA"];
                                    string ID_QTV = reader["ID_QTV"].ToString();

                                    Medicine medicine = new Medicine()
                                    {
                                        ID_THUOC = ID_Thuoc,
                                        TEN_THUOC = tenthuoc,
                                        DONVITINH = donvitinh,
                                        CHI_DINH = chidinh,
                                        TONKHO = tonkho,
                                        NGAY_HH = ngayhethan,
                                        DONGIA = dongia,
                                        ID_QTV = ID_QTV
                                    };
                                    listMedicine.Add(medicine);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error: " + ex.Message);
            }
        }

        private void ExportPDF()
        {
            if (MedicineNumber > 0)
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "PDF (*.pdf)|*.pdf";
                sfd.FileName = "DSTHUOC.pdf";
                bool fileError = false;
                if (sfd.ShowDialog() == true)
                {
                    if (File.Exists(sfd.FileName))
                    {
                        try
                        {
                            File.Delete(sfd.FileName);
                        }
                        catch (IOException ex)
                        {
                            fileError = true;
                            MessageBox.Show("Không thể ghi dữ liệu tới ổ đĩa. Mô tả lỗi:" + ex.Message);
                        }
                    }
                    if (!fileError)
                    {
                        try
                        {
                            PdfPTable pdfTable = new PdfPTable(8);
                            pdfTable.DefaultCell.Padding = 3;
                            pdfTable.WidthPercentage = 100;
                            pdfTable.HorizontalAlignment = Element.ALIGN_LEFT;


                            PdfPCell cell = new PdfPCell(new Phrase("STT"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("ID_THUOC"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("TENTHUOC"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("DONVITINH"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("CHIDINH"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("TONKHO"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("NGAYHETHAN"));
                            pdfTable.AddCell(cell);
                            cell = new PdfPCell(new Phrase("DONGIA"));
                            pdfTable.AddCell(cell);


                            foreach (var (row, index) in listMedicine.Select((r, i) => (r, i)))
                            {
                                pdfTable.AddCell((index+1).ToString());
                                pdfTable.AddCell(row.ID_THUOC.ToString());
                                pdfTable.AddCell(row.TEN_THUOC.ToString());
                                pdfTable.AddCell(row.DONVITINH.ToString());
                                pdfTable.AddCell(row.CHI_DINH.ToString());
                                pdfTable.AddCell(row.TONKHO.ToString());
                                pdfTable.AddCell(row.NGAY_HH.ToString());
                                pdfTable.AddCell(row.DONGIA.ToString());
                            }

                            using (FileStream stream = new FileStream(sfd.FileName, FileMode.Create, FileAccess.Write, FileShare.None))
                            {
                                Document pdfDoc = new Document(PageSize.A4, 10f, 20f, 20f, 10f);
                                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                                pdfDoc.Open();
                                pdfDoc.Add(new iTextSharp.text.Paragraph("So luong loai thuoc: "+MedicineNumber.ToString()));
                                pdfDoc.Add(new iTextSharp.text.Paragraph("\n\n"));
                                pdfDoc.Add(pdfTable);
                                pdfDoc.Close();
                                stream.Close();
                            }

                            MessageBox.Show("Dữ liệu Export thành công!!!", "Info");
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Mô tả lỗi :" + ex.Message);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Không có bản ghi nào được Export!!!", "Info");
            }
        }
    }
}
