using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;

namespace Web.Components.Pages.AssetManagement
{
    public partial class AccessControl
    {
        [Inject] private Core.IAccess _access { get; set; }

        [Parameter] public int AssetId { get; set; }

        [Parameter] public List<int> SelectedUserIds { get; set; }
        [Parameter] public EventCallback<int> OnUserSelected { get; set; }
        [Parameter] public EventCallback<int> OnUserDeselected { get; set; }

        protected override async Task OnInitializedAsync()
        {
            selectedEmployees = new List<Employee>();
            users.AddRange((await _access.GetUsersAsync()).ToList());

            foreach (var user in users)
            {
                employees.Add(new Employee()
                {
                    Id = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Photo = "https://images.generated.photos/VEFNRr8_f-ZbZSGTxh5Cf8VLGOb1b2ts9UoF-Rlzeh4/rs:fit:256:256/czM6Ly9pY29uczgu/Z3Bob3Rvcy1wcm9k/LnBob3Rvcy92M18w/OTI0MDk2LmpwZw.jpg", //user.Photo
                    Roles = "Guest, Administrator, Supervisor, Auditor"
                });

                if (SelectedUserIds.Any(x => x == user.Id))
                {
                   await employeeGrid.SelectRow(employees.Last());
                    //selectedEmployees.Add(employees.Last());
                }
            }

            await employeeGrid.RefreshDataAsync();
        }

        private List<Core.Models.Data.User> users = new List<Core.Models.Data.User>();

        protected class Employee
        {
            public int Id { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Photo { get; set; }
            public string Roles { get; set; }
        }

        private bool allowRowSelectOnRowClick = true;
        private IList<Employee> selectedEmployees;
        private List<Employee> employees = new List<Employee>();
        private RadzenDataGrid<Employee> employeeGrid;

        private async Task HandleRowSelectEvent(Employee args)
        {
            await OnUserSelected.InvokeAsync(args.Id);
        }

        private async Task HandleRowDeselectEvent(Employee args)
        {
            await OnUserDeselected.InvokeAsync(args.Id);
        }
    }
}