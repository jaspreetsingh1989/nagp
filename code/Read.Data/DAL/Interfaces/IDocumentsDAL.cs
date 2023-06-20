using Read.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Read.Data.DAL
{

    public interface IDocumentsDAL
    {
        Task<List<UsersDto>> GetDocuments();
    }
}