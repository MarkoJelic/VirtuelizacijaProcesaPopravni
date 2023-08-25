using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application
{
    public class InsertFileHandler : IInsertFileHandler
    {
        private readonly ICommand insertFileCommand;

        public InsertFileHandler(ICommand insertFileCommand)
        {
            this.insertFileCommand = insertFileCommand;
        }

        // Upisuje podatke iz MS u fs pomocu metode execute (pa insertFile u FileSysteminsertFileCommand klasi), ukoliko nije moguce vraca tip i poruku resultata
        public FileManipulationResults InsertFile()
        {
            FileManipulationResults results = new FileManipulationResults();
            try
            {
                insertFileCommand.Execute();
            }
            catch (Exception ex)
            {
                results.ResultType = ResultTypes.Failed;
                results.ResultMessage = ex.Message;
            }
            return results;
        }
    }
}
