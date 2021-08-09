using System.Net;
using System.Web;

namespace ReleaseSystem.fileserver
{
    public class RequestModel : BindableBase
    {
        #region Fields

        private string _status;

        #endregion Fields

        #region Constructors

        public RequestModel(string url, EndPoint ep)
        {
            RequestUrl = url;
            EndPoint = ep;
        }

        #endregion Constructors

        #region Properties

        public EndPoint EndPoint
        {
            get;
        }

        public string RequestUrl
        {
            get;
        }

        public string Status
        {
            get => _status;
            set => SetProperty(ref _status, value);
        }

        #endregion Properties

        #region Methods

        public override string ToString()
        {
            return $"{EndPoint} {HttpUtility.UrlDecode(RequestUrl)}";
        }

        #endregion Methods
    }
}