using System;
namespace WireDev.Erp.V1.Models.Statistics
{
	public class CanceledOnDate : EventOnDate
	{
        public CanceledOnDate(DateTime date, Guid productId) : base(date, productId)
        {
        }
    }
}

