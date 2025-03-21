using System.Text.Json;
using Microsoft.JSInterop;
using System.Threading.Tasks;
using System.Collections.Generic;

public class IndexedDbService
{
    private readonly IJSRuntime _jsRuntime;
    private const string DbName = "GameAssetTrackerDB";
    private const string StoreName = "Assets";

    public IndexedDbService(IJSRuntime jsRuntime)
    {
        _jsRuntime = jsRuntime;
    }

    public async Task InitializeDatabaseAsync()
    {
        await _jsRuntime.InvokeVoidAsync("indexedDBHelper.initDb", DbName, StoreName);
    }

    public async Task AddAssetAsync(GameAsset asset)
    {
        string assetJson = JsonSerializer.Serialize(asset);
        await _jsRuntime.InvokeVoidAsync("indexedDBHelper.addItem", DbName, StoreName, assetJson);
    }

    public async Task<List<GameAsset>> GetAssetsAsync()
    {
        var json = await _jsRuntime.InvokeAsync<string>("indexedDBHelper.getAllItems", DbName, StoreName);
        return string.IsNullOrEmpty(json) ? new List<GameAsset>() : JsonSerializer.Deserialize<List<GameAsset>>(json);
    }

    public async Task DeleteAssetAsync(int id)
    {
        await _jsRuntime.InvokeVoidAsync("indexedDBHelper.deleteItem", DbName, StoreName, id);
    }
}
