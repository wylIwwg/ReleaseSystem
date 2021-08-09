using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReleaseSystem.bean
{
    public class BeanControls
    {
        public string control { get; set; }
        public double Y { get; set; }
        public double X { get; set; }

        public double Width { get; set; }

        public double Height { get; set; }

        public ControlAttribute attribute { get; set; }

    }
}
