using Azure.Storage.Blobs;
using Chaos.Components;
using Chaos.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Newtonsoft.Json;
using Radzen;
using System.Text;
using System.Web;


namespace Chaos.Pages
{
    public partial class Search
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

        //CheckBox
        IEnumerable<int> Values = new int[] {1,2,3,4};

        //Search
        public string searchText { get; set; }

        public DateOnly date;

        public string test;

        private BlobClient blobClient;
        private BlobClient blobClientPref;

        public LinkedList<SearchItem> items = new();

        bool allowVirtualization = true;

        public string DropDownValue;
        IEnumerable<string> DropDownTeams;

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

            DropDownValue = "All";
            DropDownTeams = (IEnumerable<string>)new[] {"All", "AppServices"};
        }

        private async Task SearchButton()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _=DialogService.Alert("Search field can not be empty...", "Warning", new AlertOptions() { OkButtonText = "Ok" });
            }
            else
            {
                if (DropDownValue == "All")
                {
                    if (Values.Contains(1))//ICM
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://icmcdn.akamaized.net/imp/v3/incidents/omnisearch?searchString=" + HttpUtility.UrlEncode(searchText), "_blank");
                    }
                    if (Values.Contains(2))//DevOps Work Items
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://msazure.visualstudio.com/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=workitem&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(3))//DevOps Wiki
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=wiki&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(4))//DevOps Code
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?&text=" + HttpUtility.UrlEncode(searchText) + "&type=code&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(5))//DFM
                    {

                    }
                    if (Values.Contains(6))//AVA
                    {

                    }
                    if (Values.Contains(7))//OUTLOOK
                    {

                    }
                }
                else if (DropDownValue == "AppServices")
                {
                    if (Values.Contains(1))//ICM
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://icmcdn.akamaized.net/imp/v3/incidents/omnisearch?searchString=" + HttpUtility.UrlEncode(searchText) + " s:\"app service\"", "_blank");
                    }
                    if (Values.Contains(2))//DevOps Work Items
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://msazure.visualstudio.com/Antares/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=workitem&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(3))//DevOps Wiki
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/AzureAppService/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=wiki&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(4))//DevOps Code
                    {
                        await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/AzureAppService/_search?&text=" + HttpUtility.UrlEncode(searchText) + "&type=code&lp=custom-Collection&filters=&pageSize=25", "_blank");
                    }
                    if (Values.Contains(5))//DFM
                    {

                    }
                    if (Values.Contains(6))//AVA
                    {

                    }
                    if (Values.Contains(7))//OUTLOOK
                    {

                    }
                }


                items.AddFirst(new SearchItem() { SearchText = searchText, SearchDateTime = DateOnly.FromDateTime(DateTime.UtcNow) });
                var itemsJson = JsonConvert.SerializeObject(items);

                try
                {
                    using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(itemsJson)))
                    {
                        blobClient.Upload(ms, overwrite: true);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Exception details: " + ex.ToString());
                }
            }


          
        }

        void OnSpeechCaptured(string speechValue, bool updateTextArea, string name)
        {
            ///console.Log($"Speech Captured from {name}: {speechValue}");

            if (updateTextArea)
            {
                searchText = new string((from c in speechValue
                                         where char.IsWhiteSpace(c) || char.IsLetterOrDigit(c)
                                         select c).ToArray());
            }
        }

        private void PopupWarning()
        {
            DialogService.Open<PopUpWarning>("PopUp Requirements", null, new DialogOptions() { Width = "1000px", Height = "600px", Resizable = true, Draggable = true });
        }

        private async void HistorySearch(string value)
        {
            searchText = value;
            await @SearchButton();
        }

        private void EditSearch(string value)
        {
            searchText = value;
          
        }
    }
}
