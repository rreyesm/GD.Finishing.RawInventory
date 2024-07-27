using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock_Finishing.Models
{
    public class ResultModel<T>
    {
        string message = string.Empty;
        public ResultModel()
        {
            Message = string.Empty;
        }
        public ResultModel(T data)
        {
            Data = data;
        }

        public bool IsSuccess { get; set; }
        public string Message { get => message; set { IsSuccess = string.IsNullOrWhiteSpace(value) ? true : false; message = value; } }
        public T Data { get; set; }
        public int ResultType { get; set; }
    }
}
