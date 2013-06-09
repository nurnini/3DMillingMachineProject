using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MillingMachineCoreDll.Utilities
{
    public static class Utility
    {

        public static double FindMaxValue<T>(List<T> list, Converter<T, double> projection)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            double maxValue = double.MinValue;
            foreach (T item in list)
            {
                double value = projection(item);
                if (value > maxValue)
                {
                    maxValue = value;
                }
            }
            return maxValue;
        }

        public static double FindMinValue<T>(List<T> list, Converter<T, double> projection)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }
            double minValue = double.MaxValue;
            foreach (T item in list)
            {
                double value = projection(item);
                if (value < minValue)
                {
                    minValue = value;
                }
            }
            return minValue;
        }

        public static int GetDiscreteValue(double value, int step)
        {
            int exactDivision = Convert.ToInt32(Math.Floor(value / step));
            int lowerBoundary = exactDivision * step;
            int upperBoundary = (exactDivision + 1) * step;

            if (value <= lowerBoundary + (step / 2))
            {
                value = lowerBoundary;
            }
            else
            {
                value = upperBoundary;
            }

            return Convert.ToInt32(value);
        }

        /*
        public static void NormaliseValue<T>(List<T> list, double normalisedValue, Converter<T, double> projection)
        {
            if (list.Count == 0)
            {
                throw new InvalidOperationException("Empty list");
            }

             if (normalisedValue == 0)
            {
                throw new InvalidOperationException("Normalised value cannot be zero");
            }

            foreach (T item in list)
            {
                double value = projection(item);
                value = value / normalisedValue;
               
            }
           
        } */
    }
}
