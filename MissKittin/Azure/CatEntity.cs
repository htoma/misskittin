using Microsoft.WindowsAzure.Storage.Table;

namespace MissKittin.Azure
{
    public class CatEntity : TableEntity
    {
        public CatEntity()
        {
        }

        public CatEntity(string id, int likes)
        {
            RowKey = id;
            Likes = likes;
            PartitionKey = "cat";
        }

        public string Id => RowKey;
        public int Likes { get; set; }
    }
}