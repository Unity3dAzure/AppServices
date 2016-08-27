namespace Unity3dAzure.AppServices
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