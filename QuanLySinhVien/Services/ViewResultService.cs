using QuanLySinhVien.Models;
using System.Collections.Generic;

namespace QuanLySinhVien.Services
{
    public class ViewResultService
    {
        public IEnumerable<ResultModel> GetResults => new[]{
        new ResultModel { ResultID = "1", ResultName = "Result 1" },
        new ResultModel { ResultID = "2", ResultName = "Result 2" },
        new ResultModel { ResultID = "3", ResultName = "Result 3" },
        };
    }
}