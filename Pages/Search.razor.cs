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

        IEnumerable<int> Values = new int[] { 1, 2, 3, 4, 5, 6 };
        public string searchText { get; set; }

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

                }
                if (Values.Contains(4))//DevOps Wiki
                {

                }
                if (Values.Contains(5))//AVA
                {

                }
                if (Values.Contains(6))//OUTLOOK
                {

                }
            }
        }
    }
}