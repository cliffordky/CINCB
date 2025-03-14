using BlazorBootstrap;
using Core.Models.Configuration;
using Flurl;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Microsoft.JSInterop;
using Radzen;
using Radzen.Blazor;

namespace Web.Components.Pages.AssetManagement
{
    public partial class Document : PageBase
    {
        [Inject] private IOptions<DocumentStorageSettings> DocumentStorageSettings { get; set; }

        [Parameter] public int AssetId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            users.AddRange((await _access.GetUsersAsync()).ToList());
        }

        protected override async Task OnParametersSetAsync()
        {
            var result = await _asset.GetAssetDetailsByIdAsync(AssetId);
            if (result.IsSuccess)
            {
                asset = result.Value;

                var assets = await _asset.GetAllAssetDocumentsByAssetIdAsync(AssetId);

                documentList.Clear();
                foreach (var asset in assets)
                {
                    var document = new AssetDocument()
                    {
                        Id = asset.Id,
                        Name = asset.Name,
                        FileName = asset.FileName,
                        CreatedDate = asset.CreatedDate,
                        ModifiedUser = await base.GetUserFullNameAsync(asset.ModifiedUser)
                    };

                    documentList.Add(document);
                }

                await documentGrid.RefreshDataAsync();
            }
        }

        private string GetFileTypeIcon(string name)
        {
            if (String.IsNullOrEmpty(name)) return "contract";
            if (name.ToLower().EndsWith(".pdf")) return "picture_as_pdf";
            if (name.ToLower().EndsWith(".csv")) return "csv";
            if (name.ToLower().EndsWith(".jpg")) return "image";
            if (name.ToLower().EndsWith(".png")) return "image";
            if (name.ToLower().EndsWith(".gif")) return "image";
            if (name.ToLower().EndsWith(".webp")) return "image";
            return "contract";
        }

        private async Task OnImageUpload(UploadChangeEventArgs args)
        {
            foreach (var file in args.Files)
            {
                try
                {
                    var directoryPath = Path.Combine(DocumentStorageSettings.Value.FileSystemBasePath, DocumentStorageSettings.Value.AssetSlug, asset.PublicKey.ToString());
                    if (!System.IO.Directory.Exists(directoryPath))
                    {
                        System.IO.Directory.CreateDirectory(directoryPath);
                    }

                    long maxFileSize = 10 * 1024 * 1024;
                    await using FileStream fs = new(Path.Combine(directoryPath, file.Name), FileMode.Create);
                    await file.OpenReadStream(maxFileSize).CopyToAsync(fs);

                    var uploadResult = await _asset.AddAssetDocumentAsync(new Core.Models.Data.AssetDocument()
                    {
                        AssetId = AssetId,
                        FileName = file.Name,
                        PublicKey = Guid.NewGuid(),
                        ModifiedUser = await base.GetUserId()
                    });

                    if (uploadResult.IsSuccess)
                    {
                        documentList.Add(new AssetDocument()
                        {
                            Id = uploadResult.Value.Id,
                            FileName = uploadResult.Value.FileName,
                            //Icon = GetFileTypeIcon(uploadResult.Value.FileName),

                            CreatedDate = uploadResult.Value.CreatedDate,
                            ModifiedUser = await base.GetUserFullNameAsync(uploadResult.Value.ModifiedUser)
                        });

                        await documentGrid.RefreshDataAsync();
                    }
                }
                catch (Exception ex)
                {
                }
            }
        }

        private async Task EditRow(AssetDocument assetDocument)
        {
            if (!documentGrid.IsValid) return;

            if (editMode == DataGridEditMode.Single)
            {
                Reset();
            }

            documentsToUpdate.Add(assetDocument);
            await documentGrid.EditRow(assetDocument);
        }

        private async void OnUpdateRow(AssetDocument assetDocument)
        {
            Reset(assetDocument);
            var itemResult = await _asset.GetAssetDocumentByIdAsync(assetDocument.Id);
            if (itemResult.IsSuccess)
            {
                itemResult.Value.Name = assetDocument.Name;
                itemResult.Value.ModifiedDate = DateTime.Now;
                itemResult.Value.ModifiedUser = await base.GetUserId();

                await _asset.UpdateAssetDocumentAsync(itemResult.Value);
            }
        }

        private async Task SaveRow(AssetDocument assetDocument)
        {
            await documentGrid.UpdateRow(assetDocument);
        }

        private void CancelEdit(AssetDocument assetDocument)
        {
            Reset(assetDocument);
            documentGrid.CancelEditRow(assetDocument);
        }

        private async Task DeleteRow(AssetDocument assetDocument)
        {
            Reset(assetDocument);

            if (documentList.Contains(assetDocument))
            {
                var result = await _asset.DeleteAssetDocumentAsync(assetDocument.Id);
                if (result.IsSuccess)
                {
                    var filePath = (Path.Combine(DocumentStorageSettings.Value.FileSystemBasePath, DocumentStorageSettings.Value.AssetSlug, asset.PublicKey.ToString(), assetDocument.FileName));
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }
                }

                documentList.Remove(assetDocument);

                await documentGrid.Reload();
            }
            else
            {
                documentGrid.CancelEditRow(assetDocument);
                await documentGrid.Reload();
            }
        }

        private void Reset()
        {
            documentsToUpdate.Clear();
        }

        private void Reset(AssetDocument assetAttribute)
        {
            documentsToUpdate.Remove(assetAttribute);
        }

        private async Task OpenFile(AssetDocument assetDocument)
        {
            var file = await File.ReadAllBytesAsync(Path.Combine(DocumentStorageSettings.Value.FileSystemBasePath, DocumentStorageSettings.Value.AssetSlug, asset.PublicKey.ToString(), assetDocument.FileName));
            using var fileStream = new MemoryStream(file);
            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JsRuntime.InvokeVoidAsync("downloadFileFromStream", assetDocument.FileName, streamRef);
        }

        private RadzenDataGrid<AssetDocument> documentGrid;
        private List<AssetDocument> documentList = new List<AssetDocument>();
        private List<Core.Models.Data.User> users = new List<Core.Models.Data.User>();
        private Core.Models.Data.Asset asset;
        private DataGridEditMode editMode = DataGridEditMode.Single;

        private List<AssetDocument> documentsToUpdate = new List<AssetDocument>();

        protected class AssetDocument
        {
            public int Id { get; set; }
            public string Name { get; set; }

            public string FileName { get; set; }
            public DateTime CreatedDate { get; set; }
            public string ModifiedUser { get; set; }
        }
    }
}