using Read.Data.DAL;
using Read.Data.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Read.Data.BLL
{
    public class DocumentsBL : IDocumentsBL
    {
        #region Read only attributes
        private readonly IDocumentsDAL _documentsDAL;
        #endregion

        public DocumentsBL(IDocumentsDAL documentsDAL)
        {
            this._documentsDAL = documentsDAL;
        }

        #region Public Methods
        public async Task<List<UsersDto>> GetDocuments()
        {
            return await this._documentsDAL.GetDocuments();
        }

        #endregion
    }
}
