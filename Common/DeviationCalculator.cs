using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class DeviationCalculator
    {
        public delegate void DeviationCalculatedEventHandler(object sender, Load load, double deviation, string deviationType);

        public event DeviationCalculatedEventHandler DeviationCalculated;

        public void CalculateDeviation(List<Load> loads, string deviationType)
        {
            foreach (var obj in loads)
            {
                double deviation = 0;
                if (deviationType == "APD")
                {
                    deviation = ((obj.MeasuredValue - obj.ForecastValue) / obj.MeasuredValue) * 100;
                }
                else if (deviationType == "SD")
                {
                    deviation = Math.Pow(((obj.MeasuredValue - obj.ForecastValue) / obj.MeasuredValue), 2);
                }

                OnDeviationCalculated(obj, deviation, deviationType);
            }
        }

        protected virtual void OnDeviationCalculated(Load load, double deviation, string deviationType)
        {
            if (DeviationCalculated != null)
            {
                DeviationCalculated(this, load, deviation, deviationType);
            }
        }
    }
}
