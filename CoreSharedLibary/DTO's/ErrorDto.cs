using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreSharedLibary.DTO_s
{
    public class ErrorDto
    {
        public List<String> Errors { get; private set; }

        public bool IsShow { get; set; }

        public ErrorDto()
        {
            Errors = new List<String>();
        }

        public ErrorDto(string error,bool isShow)
        {
            Errors.Add(error);
            isShow = true;
        }

        public ErrorDto(List<string> errors,bool isShow)
        {
            Errors = Errors;
            IsShow = isShow;
        }
    }
}
