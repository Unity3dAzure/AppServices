namespace Unity3dAzure.MobileServices
{
    public class AzureDataModel : IAzureDataModel
    {
        public string id { get; set; }

        public string GetId()
        {
            return id;
        }
    }
}