using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReleaseSystem.bean
{
    public class Ratio
    {
        public int id { get; set; }
        public int width { get; set; }
        public int heigth { get; set; }
        public string value { get; set; }

        public Ratio(int id, int width, int heigth)
        {
            this.id = id;
            this.width = width;
            this.heigth = heigth;
            this.value = width + "*" + heigth;
        }

        public Ratio(string v)
        {
            this.value = v;
        }
    }
}
