namespace Bunker.Business.Interfaces.Responses
{
    public class TaskResponse
    {
        public int    Id          { get; set; }
        public string Name        { get; set; }
        public string Description { get; set; }
        public int    Score       { get; set; }
    }
}