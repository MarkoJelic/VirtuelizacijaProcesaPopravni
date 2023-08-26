using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class GetFilesHandler : IGetFilesHandler
    {
        private readonly IQuery getFilesQuery;

        public GetFilesHandler(IQuery getFilesQuery)
        {
            this.getFilesQuery = getFilesQuery;
        }

        public FileManipulationResults GetFiles()
        {
            FileManipulationResults results;
            try
            {
                results = getFilesQuery.GetResults();
                if (results.MSs == null || results.MSs.Count == 0)
                {
                    results.ResultType = ResultTypes.Warning;
                    results.ResultMessage = "Upit nije vratio nijednu datoteku.";
                }
                else
                {
                    results.ResultMessage = $"Upit je vratio {results.MSs.Count} datoteka.";
                }
            }
            catch (Exception ex)
            {
                results = new FileManipulationResults
                {
                    ResultType = ResultTypes.Failed,
                    ResultMessage = ex.Message
                };
            }

            return results;
        }
    }
}
