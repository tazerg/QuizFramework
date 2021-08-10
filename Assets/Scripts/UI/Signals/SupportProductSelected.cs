namespace QuizFramework.UI.Signals
{
    public struct SupportProductSelected
    {
        public string ProductId { get; }
        public string ProductName { get; }

        public SupportProductSelected(string productId, string productName)
        {
            ProductId = productId;
            ProductName = productName;
        }
    }
}