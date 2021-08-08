namespace QuizFramework.UI.Signals
{
    public struct SupportProductSelected
    {
        public string ProductId { get; }

        public SupportProductSelected(string productId)
        {
            ProductId = productId;
        }
    }
}