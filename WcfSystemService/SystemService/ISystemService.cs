using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace SystemService
{
    [ServiceContract]
    public interface ISystemService
    {
       
        [OperationContract]
        void didnt_show();

        [OperationContract]
        void auto_deblock();

        [OperationContract]
        void reminder();
    }

}
