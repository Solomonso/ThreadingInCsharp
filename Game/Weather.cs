using System;
using System.Collections.Generic;
using System.Text;

namespace ThreadingInCsharp.Game
{
	public class Weather
	{
		int[] temperature;
		int[] humidity;
		int[] sunshine;
		int[] rain;
		int value;

		public Weather()
		{
			//random weather values to loop through
			temperature = new int[] { 5, 10, 15, 20, 25, 30, 35 };
			humidity = new int[] { 20, 30, 40, 50, 60, 70, 80 };
			sunshine = new int[] { 20, 25, 30, 35, 40, 50, 55 };
			rain = new int[]     { 15, 25, 50, 30, 75, 45, 100};
			value = 0;
		}

		public int randomTemp()
		{
			//generates temperature
			Random random = new Random();
			int index = random.Next(0, 7);

			return value + temperature[index];
		}

		public int randomHumidity()
		{
			//generates Humidity
			Random random = new Random();
			int index = random.Next(0, 7);
			return value + humidity[index];
		}

		public int randomSun()
		{
			//generates Sun
			Random random = new Random();
			int index = random.Next(0, 7);
			return value + sunshine[index];
		}

		public int randomRain()
		{
			//randomly generates rain value for percentage of when it rains
			Random random = new Random();
			int index = random.Next(0, 7);
			return value + rain[index];
		}
	}
}
