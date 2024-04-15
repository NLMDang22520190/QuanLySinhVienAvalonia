using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    public class StudentInfoModel
    {
        public string StudentName { get; set; }
        public string StudentBirthDay { get; set; }
        public string StudentGender { get; set; }
        public string StudentAddress { get; set; }
        public string StudentEmail { get; set; }

        public float StudentGPA1 { get; set; }

        public float StudentGPA2 { get; set; }
    }
}
