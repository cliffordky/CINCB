using BlazorBootstrap;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using System.Security.Claims;
using EnumsNET;
using static Core.Enumerations;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Forms;
using System.ComponentModel.DataAnnotations;
using Cron.Core;
using static Web.Utils.Enumerations;

namespace Web.Components.Pages
{
    public partial class Home
    {
        [SupplyParameterFromForm]
        private InputModel? Input { get; set; } = new();

        public string Description { get; set; }

        protected override async Task OnInitializedAsync()
        {
            var cron = new CronBuilder("0 7 * * 1,3,5");
            Description = cron.Description;
        }

        protected async Task Submit(EditContext formContext)
        {
            var cronResult = await openAI.GenerateCronExpression(Input.CronInstruction);
            if (cronResult.IsSuccess)
            {
                try
                {
                    //int startIndex = cronResult.Value.IndexOf("##");
                    //int endIndex = cronResult.Value.LastIndexOf("##");
                    //var expression = cronResult.Value.Substring(startIndex + 2, endIndex - startIndex - 2);

                    var cron = new CronBuilder(cronResult.Value);
                    Description = cron.Description;
                }
                catch (Exception Ex)
                {
                    Description = Ex.Message;
                }
            }
        }

        private sealed class InputModel
        {
            public string CronInstruction { get; set; } = null!;
        }
    }
}