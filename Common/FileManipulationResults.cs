using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class FileManipulationResults : IDisposable
    {
        private bool disposedValue;

        public FileManipulationResults()
        {
            this.ResultType = ResultTypes.Success;
            MSs = new Dictionary<string, MemoryStream>();
        }

        [DataMember]
        public Dictionary<string, MemoryStream> MSs { get; set; }

        [DataMember]
        public string ResultMessage { get; set; }

        [DataMember]
        public ResultTypes ResultType { get; set; }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                foreach (KeyValuePair<string, MemoryStream> kvp in MSs)
                {
                    if (kvp.Value != null)
                    {
                        kvp.Value.Dispose();
                        kvp.Value.Close();
                    }
                }
                MSs.Clear();
                disposedValue = true;
            }
        }

        // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~FileManipulationResults()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
