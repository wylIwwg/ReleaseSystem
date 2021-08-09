using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReleaseSystem.bean
{
    public class ControlAttribute
    {
        public String text { get; set; }//文本内容
        public String textColor { get; set; }//#2232323
        public float textSize { get; set; }//文字大小
        public String backgroundImage { get; set; }//背景图片 连接形式
        public String backgroundColor { get; set; }//背景颜色  #12312312
        public int gravity { get; set; }//文字方向 1 居中


    }
}
