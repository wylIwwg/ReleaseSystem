using System.Collections.ObjectModel;
using System.IO;

namespace ReleaseSystem.fileserver
{
    public class ViewModel : BindableBase
    {
        #region Fields

        private int _port = 80;
        private string _workDir;

        #endregion Fields

        #region Constructors

        public ViewModel()
        {
            RequestModels = new ObservableCollection<RequestModel>();
            WorkDir = Path.GetDirectoryName(GetType().Assembly.Location);
        }

        #endregion Constructors

        #region Properties

        public int ListenPort
        {
            get => _port;
            set
            {
                if (0 < value && value < 65536)
                    SetProperty(ref _port, value);
            }
        }

        public ObservableCollection<RequestModel> RequestModels { get; }

        public string WorkDir
        {
            get => _workDir;
            set
            {
                if (Directory.Exists(value))
                    SetProperty(ref _workDir, value);
            }
        }

        #endregion Properties
    }
}