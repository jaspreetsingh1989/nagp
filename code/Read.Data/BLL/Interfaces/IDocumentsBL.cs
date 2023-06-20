using Read.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Read.Data.BLL
{
    public interface IDocumentsBL
    {
        Task<List<UsersDto>> GetDocuments();
    }
}