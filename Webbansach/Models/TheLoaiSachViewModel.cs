using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Webbansach.Models
{

    public class TheLoaiSachViewModel
    {

        public List<TheLoai> TheLoais { get; set; }
        public List<Sach> Sachs { get; set; }
    }
}