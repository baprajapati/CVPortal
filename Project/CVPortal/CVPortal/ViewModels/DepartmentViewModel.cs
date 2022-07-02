using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace CVPortal.ViewModels
{
    public class DepartmentViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Seg_ID { get; set; }

        [Required]
        public string Dept_Code { get; set; }

        [Required]
        public string Dept_Desc { get; set; }
    }
}