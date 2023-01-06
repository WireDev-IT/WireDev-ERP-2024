using System;
namespace WireDev.Erp.V1.Models.Statistics
{
	public class DisposedOnDate : EventOnDate
	{
        public DisposedOnDate(DateTime date, Guid productId) : base(date, productId)
        {
        }
    }
}

