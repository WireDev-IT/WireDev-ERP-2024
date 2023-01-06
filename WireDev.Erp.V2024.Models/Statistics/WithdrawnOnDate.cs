using System;
namespace WireDev.Erp.V1.Models.Statistics
{
	public class WithdrawnOnDate : EventOnDate
	{
        public WithdrawnOnDate(DateTime date, Guid productId) : base(date, productId)
        {
        }
    }
}

