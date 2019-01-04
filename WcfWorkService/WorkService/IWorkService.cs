using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WorkService
{
    [ServiceContract]
    public interface IWorkService
    {
        [OperationContract]
        bool create_work(string title, string author, string theme, string keywords, string barecode, string type, int reserved);

        [OperationContract]
        List<Work> get_all_works();

        [OperationContract]
        List<Work> get_works(string title, string author, string theme, string keywords, string barecode, string type);

        [OperationContract]
        Work get_work_by_barecode(string barecode);

        [OperationContract]
        bool update_work(int id_work, string title, string author, string theme, string keywords, string barecode, string type, int reserved);

        [OperationContract]
        bool delete_work(int id_work);

        [OperationContract]
        bool reserve_work(int id_work, string id_client);

        [OperationContract]
        bool free_work(int id_work);
    }
}
