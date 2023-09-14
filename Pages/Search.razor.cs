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

        private async Task SearchButton()
        {
            if (string.IsNullOrEmpty(searchText))
            {
                _=DialogService.Alert("Search field can not be empty...", "Warning", new AlertOptions() { OkButtonText = "Ok" });
            }
            else
            {
                if (Values.Contains(1))//DFM
                {

                }
                if (Values.Contains(2))//ICM
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://icmcdn.akamaized.net/imp/v3/incidents/omnisearch?searchString=" + HttpUtility.UrlEncode(searchText), "_blank");
                }
                if (Values.Contains(3))//DevOps Work Items
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=workitem&lp=custom-Collection&filters=&pageSize=25", "_blank");
                }
                if (Values.Contains(4))//DevOps Wiki
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?action=contents&text=" + HttpUtility.UrlEncode(searchText) + "&type=wiki&lp=custom-Collection&filters=&pageSize=25", "_blank");
                }
                if (Values.Contains(5))//DevOps Code
                {
                    await JSRuntime.InvokeVoidAsync("open", "https://supportability.visualstudio.com/_search?&text=" + HttpUtility.UrlEncode(searchText) + "&type=code&lp=custom-Collection&filters=&pageSize=25", "_blank");                
                }
                if (Values.Contains(6))//AVA
                {

                }
                if (Values.Contains(7))//OUTLOOK
                {

                }
            }
        }

        void OnSpeechCaptured(string speechValue, bool updateTextArea, string name)
        {
            ///console.Log($"Speech Captured from {name}: {speechValue}");

            if (updateTextArea)
            {
                searchText += speechValue;
            }
        }

        void OnTextAreaChange(string value, string name)
        {
            //console.Log($"{name} value changed to {value}");
        }
    }
}

//BlobServiceClient blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=ticketmtool;AccountKey=3fk/ac57fiU9Kf8DopFzM4MGLZpSTpum1ArNcoZz6VreQZOoF6TjmZ+Zh+EkZvGb/F+r8lbN1cuP+AStMHRV4A==;EndpointSuffix=core.windows.net");
//BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient("ticketmanagementtool");

//blobClient = containerClient.GetBlobClient(Security.User.Email.Replace("@", "_") + ".json");
//if (blobClient.Exists())