using System;
using System.IO;

namespace ReleaseSystem.fileserver
{
    public static class Extensitions
    {
        #region Fields

        public static readonly string HtmlParentDir = "<tr><td><a href=\"../\">上一级</a></td><td class='tdsize'>--</td><td class='tdtime'>--</td><td></td><td></td></tr>";

        public static readonly string HtmlStyle = "<style type=\"text/css\">" +
                        "body { width:100%; } " +
                "table { width:100%; } " +
                "div { height:35px; font-family:'Microsoft YaHei', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif; } " +
                "tr{ height: 35px; } " +
                "th{ text-align:left; color:#6e6e6e; border-bottom: 1px #aac solid; } " +
                "td{ border-bottom: 1px #aaa5 solid; margin-left: 15px; margin-top: 5px; margin-right: 50px; } " +
                "a { text-decoration:none; } a:hover{  text-decoration:underline; } " +
                ".thname { width:50%; } " +
                ".thsize { width:10%; } " +
                ".thtime { width:10%; } " +
                ".thother { width:15%; } " +
                ".tdsize { color:#353535; } " +
                ".tdtime { color:#aaa; } " +
                "</style>";

        private static string[] sizeUnit = new string[] { "B", "KB", "MB", "GB", "TB", "PB", "EB" };

        #endregion Fields

        #region Methods

        public static string LengthToSize(long length)
        {
            if (length == 0)
                return "0B";
            var step = (int)Math.Truncate(Math.Log(length, 1024L));
            if (step > sizeUnit.Length)
                step = sizeUnit.Length - 1;
            var v = length / Math.Pow(1024L, step);
            v = Math.Round(v, 2);
            return v.ToString() + sizeUnit[step];
        }

        public static string ToHtmlText(this FileInfo finfo)
        {
            return $"<tr><td><a href=\"./{finfo.Name}\">{finfo.Name}</a></td><td class='tdsize'>{LengthToSize(finfo.Length)}</td><td class='tdtime'>{finfo.LastWriteTime}</td><td></td><td></td></tr>";
        }

        public static string ToHtmlText(this DirectoryInfo dir)
        {
            return $"<tr><td><a href=\"./{dir.Name}/\">{dir.Name}</a></td><td class='tdsize'>--</td><td class='tdtime'>{dir.LastWriteTime}</td><td></td><td></td></tr>";
        }

        #endregion Methods
    }
}