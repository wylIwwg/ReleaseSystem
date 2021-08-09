using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReleaseSystem.bean
{
    public class DeviceBean
    {
        public string name { get; set; }
        public string ip { get; set; }
        public string mac { get; set; }
        public string type { get; set; }
        public string online { get; set; }

        public bool IsGrouping { get; set; }
        public List<DeviceBean> children { get; set; }


        public DeviceBean()
        {
            children = new List<DeviceBean>();
        }
    }
}
