using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuanLySinhVien.Models
{
    public class TeacherInfoModel
    {
        public string TeacherName { get; set; } = String.Empty;
        public string TeacherBirthDay { get; set; } = String.Empty;

        public string TeacherGender { get; set; } = String.Empty;

        public string TeacherAddress { get; set; } = String.Empty;

        public string TeacherEmail { get; set; } = String.Empty;
    }
}
