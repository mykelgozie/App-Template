namespace App.Dtos.ResponseModel
{
    public class ModelResponse<T>
    {
        public bool Status { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
    }
}
