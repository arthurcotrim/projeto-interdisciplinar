namespace Gerenciamento.Models
{
    public class BaseModel
    {
        public int? Id { get; set; }
        public DateTime DtCreation { get; set; }
        public DateTime DtLastModified { get; set; }
        public int? Status { get; set; }
    }
}
