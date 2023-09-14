using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.JSInterop;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Radzen;
using Radzen.Blazor;
using System.Web;
using Chaos.Shared;
using Chaos.Components;
using Microsoft.Extensions.Configuration;
using Azure.Storage.Blobs;
using Chaos.Models;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;


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

        //CheckBox
        IEnumerable<int> Values = new int[] { 1, 2, 3, 4, 5, 6 ,7};

        //Search
        public string searchText { get; set; }

        //SpeachtoText
        string valueSpeech;
        //EventConsole console;

        public DateOnly date;

        public string test;

        private BlobClient blobClient;

        public List<SearchItem> items = new();

        IEnumerable<SearchItem> itemsX;

        [Inject]
        private IConfiguration configuration { get; set; }//then you can get your connectionStringvar connection=  configuration["connectionString"];

        bool allowVirtualization;

        protected async override Task OnInitializedAsync()
        {
            //OnInitialize          
            Console.WriteLine(Security.User.Email);

            BlobServiceClient blobServiceClient = new BlobServiceClient(configuration["SASKey"]);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(configuration["BlobName"]);
            blobClient = containerClient.GetBlobClient(Security.User.Email.Replace("@", "_") + ".json");

            if (blobClient.Exists())
            {
                var response = blobClient.Download();
                using (StreamReader r = new StreamReader(response.Value.Content))
                {
                    string json = r.ReadToEnd();
                    items = Newtonsoft.Json.JsonConvert.DeserializeObject<List<SearchItem>>(json);
                    itemsX = items.AsQueryable();  
                }
            }
        }

        private async Task SearchButton()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _=DialogService.Alert("Search field can not be empty...", "Warning", new AlertOptions() { OkButtonText = "Ok" });
            }
            else
            {
                
                if (Values.Contains(1))//ICM
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://icmcdn.akamaized.net/imp/v3/incidents/omnisearch?searchString=" + HttpUtility.UrlEncode(searchText), "_blank");
                }
                if (Values.Contains(2))//DevOps Work Items
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=workitem&lp=custom-Collection&filters=&pageSize=25", "_blank");
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

            items.Add(new SearchItem() { SearchText=searchText, SearchDateTime= DateOnly.FromDateTime(DateTime.UtcNow)});
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

        void OnTextAreaChange(string value, string name)
        {
            //console.Log($"{name} value changed to {value}");
        }

        private void PopupWarning()
        {
            DialogService.Open<PopUpWarning>("PopUp Requirements", null, new DialogOptions() { Width = "1000px", Height = "600px", Resizable = true, Draggable = true });
        }

        void stg()
        {

            //blobClient = containerClient.GetBlobClient(Security.User.Email.Replace("@", "_") + ".json");
            //if (blobClient.Exists())
        }
    }
}
