using System;

namespace Storage.API.Models
{
    public class StorageItem
    {

        public int id { get; set;}
        public int quantity { get; set; }
        public string name { get; set; }
        public string location { get; set; }

        public StorageItem() 
		{
            this.id = -1;
            this.quantity = 0;
            this.name = "";
            this.location = "";
		}

        public StorageItem(int id, int quantity, string name, string location)
        {
            this.id = id;
            this.quantity = quantity;
            this.name = name;
            this.location = location;
		}

        public override string ToString()
        {
            return string.Format("id: {0}, name: {1}, quantity: {2}, location: {3}", id, name, quantity, location);
		}
    }
}
