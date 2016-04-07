using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace ModelManagementServices
{
    // 注意: 您可以使用 [重構] 功能表上的 [重新命名] 命令同時變更程式碼和組態檔中的介面名稱 "IService1"。
    [ServiceContract]
    public interface IService1
    {
        [OperationContract]   // CurrentVersion
        string CurrentVersion();

        [OperationContract]
        string getCNCType(string cncNumber);

        [OperationContract]
        string uploadModel(ModelStorationInformation modelStorationInformation, string UserName);


        [OperationContract]
        string uploadTempModel(string UserName);

        [OperationContract]
        string getTempBlob(string UserName);

        [OperationContract]
        string deleteModel(string modelName);

        [OperationContract]
        string deleteTempBlobData(string dataID, string modelFileType);

        [OperationContract]
        List<ModelInformation> getModelInformationList(string ServiceBrokerID, string vMachineID, string cnc_number, string ProductID, DateTime creationStartDate, DateTime creationEndDate, string userName, string userCompany);

        [OperationContract]
        ServiceBrokerServices.ModelFull fanOutModelControl(string modelID, List<FanOutEquipmentInformation> EquipmentList, string user);

        [OperationContract]
        bool fanOutModel(List<Model_SendContent> ModelInfo);


        [OperationContract]
        ModelFilterParameters getModelFilterParameter(string UserCompany);

        [OperationContract]
        ServiceBrokerServices.Vmachine[] getVmachineInformationList();


        [OperationContract]
        List<ModelInformation> getAllModelInformation();

        [OperationContract]
        List<ModelManagementServices.ServiceReference1.EquipmentInformation> getEquipmentInformationList(string ServiceBrokerID, string company);

        [OperationContract]
        string getEquipmentInformationLists(string ServiceBrokerID, string company);



        //[OperationContract]
        //List<ProductBasicInformation> getProductBasicInfoList();

    }
}
