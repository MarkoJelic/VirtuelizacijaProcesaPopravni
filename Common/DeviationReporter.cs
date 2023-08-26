using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DeviationReporter
    {
        public DeviationReporter(DeviationCalculator calculator)
        {
            calculator.DeviationCalculated += DeviationCalculatedEventHandler;
        }

        private void DeviationCalculatedEventHandler(object sender, Load load, double deviation, string deviationType)
        {
            // {Name = "PercentageDeviationCalculator" FullName = "Common.PercentageDeviationCalculator"}
            if (deviationType == "APD")
            {
                load.AbsolutePercentageDeviation = deviation;
            }
            else if (deviationType == "SD")
            {
                load.SquaredDeviation = deviation;
            }
        }
    }
}
