namespace OrderService.Domain
{
    public static class DomainConstants
    {
        public static class Order
        {
            public const int OrderNumberMaxLength = 64;
            public const int CurrencyMaxLength = 3;
            public const int ProductNameMaxLength = 256;
            public const int CustomerIdMaxLength = 200;
            public const int ProductIdMaxLength = 200;
        }

        public static class Message
        {
            public const int EventTypeMaxLength = 200;
            public const int PayloadMaxLength = 4000;
            public const int ConsumerMaxLength = 200;
        }
    }
}
