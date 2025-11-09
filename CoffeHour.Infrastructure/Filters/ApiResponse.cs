namespace CoffeHour.Infrastructure.Filters
{
    internal class ApiResponse<T>
    {
        private string message;
        private bool v;

        public ApiResponse(string message, bool v)
        {
            this.message = message;
            this.v = v;
        }
    }
}