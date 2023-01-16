using System;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models
{
	public class Settings
	{
		public Settings()
		{
			Uuid = Guid.NewGuid();
		}

		[Key]
		public Guid Uuid { get; }

        public uint NextProductNumber { get; private set; } = 10000;
        public int NextGroupNumber { get; private set; } = 100;
        public uint ContinueProductNumber() => NextProductNumber++;
        public int ContinueGroupNumber() => NextGroupNumber++;
    }
}