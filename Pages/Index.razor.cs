using Azure.Storage.Blobs;
using Chaos.Models;
using Chaos.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Chaos.Pages
{
    public partial class Index
    {
        [Inject]
        protected IJSRuntime JSRuntime { get; set; }

        [Inject]
        protected NavigationManager NavigationManager { get; set; }

        [Inject]
        protected DialogService DialogService { get; set; }

        [Inject]
        protected TooltipService TooltipService { get; set; }

        [Inject]
        protected ContextMenuService ContextMenuService { get; set; }

        [Inject]
        protected NotificationService NotificationService { get; set; }

        [Inject]
        protected SecurityService Security { get; set; }

        [Inject]
        private IConfiguration configuration { get; set; }//then you can get your connectionStringvar connection=  configuration["connectionString"];

        bool showDataLabels = false;
        private BlobClient blobClient;
        private LinkedList<SearchItem> items;

        class DataItem
        {
            public DateOnly Date { get; set; }
            public int search { get; set; }
        }

        DataItem[] searchs;
        List<DataItem> data = new List<DataItem>();

        protected async override Task OnInitializedAsync()
        {
            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration["SASKey"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobName"]);
            blobClient = containerClient.GetBlobClient(Security.User.Email.Replace("@", "_") + ".json");

            if (blobClient.Exists())
            {
                var response = blobClient.Download();
                using (StreamReader r = new StreamReader(response.Value.Content))
                {
                    string json = r.ReadToEnd();
                    items = JsonConvert.DeserializeObject<LinkedList<SearchItem>>(json);
                }
            }

            foreach(var item in items)
            {
                var x = data.FirstOrDefault(x => x.Date == item.SearchDateTime);
                if(x != null)
                    data.First(x => x.Date == item.SearchDateTime).search++;
                else
                    data.Add(new DataItem() { Date = item.SearchDateTime, search = 1 });
                
                
            }
            searchs = data.ToArray();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {

        }

    }
}