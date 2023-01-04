using System;
namespace WireDev.Erp.V1.Models.Statistics
{
	public class SoldOnDate : EventOnDate
	{
        public SoldOnDate(DateTime date, Guid productId) : base(date, productId)
        {
        }
    }
}

