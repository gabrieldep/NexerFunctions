using System;

namespace FunctionWeather.Models
{
    public class SensorReading
    {
        public DateTime Date { get; set; }
        public double MeasuredValue { get; set; }

        internal static SensorReading FromStringData(string data)
        {
            SensorReading sensorReading = new();
            string[] obj = data.Split(';');

            sensorReading.Date = Convert.ToDateTime(obj[0]);

            if (obj[1][0] == ',')
                obj[1] = "0" + obj[1];
            sensorReading.MeasuredValue = Convert.ToDouble(obj[1]);
            return sensorReading;
        }
    }
}
