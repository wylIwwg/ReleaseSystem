using System;
using System.Collections.Concurrent;
using System.IO;
using System.Text;

namespace ReleaseSystem.fileserver
{
    public class HttpFileServer
    {
        #region Fields

        private readonly string curexefile;

        private readonly ConcurrentDictionary<string, string> DirHtmlDic;
        private readonly string htmlAbout;
        private readonly string htmltbHead;
        private readonly ConcurrentDictionary<string, string> PathETagDic;
        private FileSystemWatcher _fileWatcher;

        #endregion Fields

        #region Constructors

        public HttpFileServer()
        {
            DirHtmlDic = new ConcurrentDictionary<string, string>();
            PathETagDic = new ConcurrentDictionary<string, string>();

            var assm = GetType().Assembly;
            curexefile = assm.Location;

            htmltbHead = "<tr>" +
                "<th class='thname'>文件名</th><th class='thsize'>大小</th><th class='thtime'>修改时间</th><th class='thother'></th><th class='thother'></th>" +
                "</tr>";

            htmlAbout = $"<p style=\"text-align:center;\"><a href=\"/{{HttpFileServer}}\">HttpFileServer</a> --V{assm.GetName().Version} \t </p>";
        }

        #endregion Constructors

        #region Methods

        public string GetDirContentHtmlText(string dstpath, bool hidetitle = false)
        {
            dstpath = dstpath.TrimEnd('\\');
            if (!DirHtmlDic.TryGetValue(dstpath, out string html))
            {
                html = CreateDirContentHtmlText(dstpath, hidetitle);
            }
            return html;
        }

        public string GetPathETag(string path)
        {
            if (PathETagDic.TryGetValue(path, out var tag))
            {
                return tag;
            }

            if (File.Exists(path))
            {
                var finfo = new FileInfo(path);
                tag = finfo.LastWriteTime.ToFileTime().ToString();
                PathETagDic.TryAdd(path, tag);
                return tag;
            }

            if (Directory.Exists(path))
            {
                CreateDirContentHtmlText(path);
                PathETagDic.TryGetValue(path, out tag);
                return tag;
            }

            return null;
        }

        public void Start(string dirPath)
        {
            if (_fileWatcher == null)
            {
                _fileWatcher = new FileSystemWatcher(dirPath)
                {
                    IncludeSubdirectories = true,
                    Filter = "*.*"
                };
                _fileWatcher.Changed += OnFileWatcher_Changed;
                _fileWatcher.Created += OnFileWatcher_Changed;
                _fileWatcher.Deleted += OnFileWatcher_Changed;

                _fileWatcher.Renamed += OnFileWatcher_Renamed;

                _fileWatcher.NotifyFilter = NotifyFilters.DirectoryName | NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.Size;
            }
            else
            {
                _fileWatcher.Path = dirPath;
            }
            _fileWatcher.EnableRaisingEvents = true;
        }

        public void Stop()
        {
            if (_fileWatcher != null)
                _fileWatcher.EnableRaisingEvents = false;
            PathETagDic.Clear();
            DirHtmlDic.Clear();
        }

        private string CreateDirContentHtmlText(string dstpath, bool hidetitle = false)
        {
            if (!Directory.Exists(dstpath))
                return null;

            var dirs = Directory.GetDirectories(dstpath);
            var files = Directory.GetFiles(dstpath);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<!DOCTYPE html>");
            sb.AppendLine("<html lang=\"zh-CN\">");
            sb.AppendLine("<head>");
            sb.AppendLine("<meta charset=\"UTF-8\">");
            if (hidetitle)
            {
                sb.AppendLine("<title>/</title>");
            }
            else
            {
                sb.AppendLine($"<title>{new DirectoryInfo(dstpath).Name}</title>");
            }
            sb.AppendLine(Extensitions.HtmlStyle);
            sb.AppendLine("</head>");
            sb.AppendLine("<body>");
            sb.AppendLine("<div>");
            sb.AppendLine("<table>");
            sb.AppendLine(htmltbHead);
            sb.AppendLine(Extensitions.HtmlParentDir);
            foreach (var dir in dirs)
            {
                var drinfo = new DirectoryInfo(dir);
                sb.AppendLine(drinfo.ToHtmlText());
            }
            foreach (var file in files)
            {
                if (file.Equals(curexefile))
                    continue;
                var finfo = new FileInfo(file);
                PathETagDic.TryRemove(file, out _);
                PathETagDic.TryAdd(file, finfo.LastWriteTime.ToFileTime().ToString());

                sb.AppendLine(finfo.ToHtmlText());
            }

            var dtcache = DateTime.Now;

            sb.AppendLine("</table>");
            sb.AppendLine("<div style=\"margin-top:50px;\">");
            sb.AppendLine($"<p style=\"text-align:center;color:#aac;\">缓存时间: {dtcache}</p>");
            sb.AppendLine(htmlAbout);
            sb.AppendLine("</div>");
            sb.AppendLine("</div>");
            sb.AppendLine("</body>");
            sb.AppendLine("</html>");

            var content = sb.ToString();

            PathETagDic.TryRemove(dstpath, out _);
            PathETagDic.TryAdd(dstpath, dtcache.ToFileTime().ToString());

            DirHtmlDic.TryAdd(dstpath, content);

            return content;
        }

        private void OnFileWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            var dstpath = e.FullPath.TrimEnd('\\');

            PathETagDic.TryRemove(dstpath, out _);

            var dirpath = Path.GetDirectoryName(dstpath);
            PathETagDic.TryRemove(dirpath, out _);
            DirHtmlDic.TryRemove(dirpath, out _);
        }

        private void OnFileWatcher_Renamed(object sender, RenamedEventArgs e)
        {
            var dstpath = e.OldFullPath.TrimEnd('\\');

            PathETagDic.TryRemove(dstpath, out _);

            if (File.Exists(e.FullPath))
            {//文件变化
                dstpath = Path.GetDirectoryName(dstpath);
            }

            PathETagDic.TryRemove(dstpath, out _);
            DirHtmlDic.TryRemove(dstpath, out _);

            dstpath = e.FullPath;
            if (File.Exists(e.FullPath))
            {//文件变化
                dstpath = Path.GetDirectoryName(dstpath);
            }

            PathETagDic.TryRemove(dstpath, out _);
            DirHtmlDic.TryRemove(dstpath, out _);
        }

        #endregion Methods
    }
}