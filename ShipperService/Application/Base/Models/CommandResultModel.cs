namespace Base.Application.Models
{
    public class CommandResultModel
    {
        public bool Succeeded { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
    }
}
