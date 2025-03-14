namespace Api.Models.v1
{
    public class ContributionRequest
    {
        public Guid VendorKey { get; set; }
        public string ConsumerIdentifier { get; set; }
        public AccountPayment MonthlyAccountPayment { get; set; }


        public class AccountPayment
        {
            public DateTime AccountOpenedDate { get; set; }
            public string SubscriberName { get; set; }
            public string CreditLimitAmt { get; set; }
            public string CurrentBalanceAmt { get; set; }
            public string MonthlyInstalmentAmt { get; set; }
            public string ArrearsAmt { get; set; }
            public string ArrearsTypeInd { get; set; }
            public string AccountType { get; set; }
            public string AccountNo { get; set; }
        }


    }
}
