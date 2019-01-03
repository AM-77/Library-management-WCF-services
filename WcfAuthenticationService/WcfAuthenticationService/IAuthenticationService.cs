using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfAuthenticationService
{
    [ServiceContract]
    public interface IAuthenticationService
    {
        [OperationContract]
        bool subscribtion_admin(string first_name, string second_name, string email, string password);

        [OperationContract]
        bool subscribtion_student(string id, string first_name, string second_name, string specaility, string level, string email, string password);

        [OperationContract]
        bool subscribtion_teacher(string id, string first_name, string second_name, string grade, string email, string password);

        [OperationContract]
        string login(string email, string password);

        [OperationContract]
        bool is_teacher(string email);

        [OperationContract]
        bool is_student(string email);

        [OperationContract]
        bool is_teacher_id(string id);

        [OperationContract]
        bool is_student_id(string id);

        [OperationContract]
        bool is_admin(string email);

        [OperationContract]
        Teacher get_teacher_by_id(string id);

        [OperationContract]
        Student get_student_by_id(string id);

        [OperationContract]
        Teacher get_teacher(string email);

        [OperationContract]
        Student get_student(string email);

        [OperationContract]
        bool is_blocked_student(string id);

        [OperationContract]
        bool is_blocked_teacher(string id);

    }

}
