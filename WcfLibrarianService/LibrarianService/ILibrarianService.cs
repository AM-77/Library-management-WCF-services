using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace LibrarianService
{
    [ServiceContract]
    public interface ILibrarianService
    {
        [OperationContract]
        bool deblock(string id_client);

        [OperationContract]
        bool confirm_reservation(string id_client, int id_work);

    }

}
