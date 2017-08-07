using Microsoft.WindowsAzure.Storage.Table;

namespace MissKittin.Azure
{
    public class CatEntity : TableEntity
    {
        public CatEntity()
        {
        }

        public CatEntity(string id, string url, int likes)
        {
            RowKey = id;
            Url = url;
            Likes = likes;
            PartitionKey = "cat";
        }

        public string Id => RowKey;
        public string Url { get; set; }
        public int Likes { get; set; }        
    }
}