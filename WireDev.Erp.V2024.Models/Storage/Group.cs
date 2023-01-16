using System;
using System.ComponentModel.DataAnnotations;

namespace WireDev.Erp.V1.Models.Storage
{
	public class Group
	{
		public Group()
		{

		}

		public Group(int groupNumber)
		{
			Uuid = groupNumber;
		}

		[Key]
		public int Uuid { get; }
		[Required(AllowEmptyStrings = false, ErrorMessage = "Group needs a name!")]
		public string? Name { get; set; }
		public string? Description { get; set; }
		public bool Used { get; private set; } = false;
		public bool Use() => Used = true;
	}
}