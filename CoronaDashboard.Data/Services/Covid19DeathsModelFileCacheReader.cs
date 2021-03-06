﻿using System;
using System.IO;

namespace CoronaDashboard.Data
{
	public class Covid19DeathsModelFileCacheReader : ICovid19DeathsModelCacheReader
	{
		public const string CACHE_FILE = "time_series_19-covid-Deaths.csv";
		public const int EXPIRES_HOURS = 6;

		ICovid19DeathsModelReader _reader = new Covid19DeathsModelDowloader();

		public Covid19DeathsModelFileCacheReader(ICovid19DeathsModelReader reader)
		{
			_reader = reader;
		}

		protected override void CacheWrite(string data)
		{
			File.WriteAllText(CACHE_FILE, data);
		}

		protected override bool CacheExits()
		{
			if (File.Exists(CACHE_FILE))
			{
				TimeSpan age = DateTime.Now - File.GetLastWriteTime(CACHE_FILE);
				if (age.TotalHours < EXPIRES_HOURS)
					return true;
			}
			return false;
		}

		public override void CacheInvalidate()
		{
			File.Delete(CACHE_FILE);
		}

		protected override string ReadData()
		{
			return _reader.GetCovid19Deaths();
		}

		protected override string CacheRead()
		{
			return File.ReadAllText(CACHE_FILE);
		}
	}
}
