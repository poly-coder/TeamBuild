using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Logging;
using Microsoft.JSInterop;

namespace TeamBuild.Core.Blazor;
public interface IClipboardService
{
    Task CopyText(string text);
}

public class ClipboardService : IClipboardService
{
    private readonly IJSRuntime jsRuntime;

    public ClipboardService(IJSRuntime jsRuntime)
    {
        this.jsRuntime = jsRuntime;
    }

    public async Task CopyText(string text)
    {
        await jsRuntime.InvokeVoidAsync("navigator.clipboard.writeText", text);
    }
}

public enum ClipboardMessageType
{
    Success,
    Error
}

public static class ClipboardServiceExtensions
{
    public static async Task CopyText(
        this IClipboardService service,
        string? text,
        ILogger? logger = null,
        Action<string, ClipboardMessageType>? showMessage = null
    )
    {
        if (string.IsNullOrEmpty(text))
            return;

        try
        {
            await service.CopyText(text);

            // TODO: Use culture-specific message
            showMessage?.Invoke("Content copied to clipboard!", ClipboardMessageType.Success);
        }
        catch (Exception exception)
        {
            logger?.LogError(exception, "Failed to copy content to clipboard.");

            // TODO: Use culture-specific message
            showMessage?.Invoke("Failed to copy content to clipboard.", ClipboardMessageType.Error);
        }
    }

    public static EventCallback CopyTextCallback(
        this IClipboardService service,
        object receiver,
        string? text,
        ILogger? logger = null,
        Action<string, ClipboardMessageType>? showMessage = null
    )
    {
        return EventCallback.Factory.Create(
            receiver,
            () => service.CopyText(text, logger, showMessage)
        );
    }

    public static EventCallback<T> CopyTextCallback<T>(
        this IClipboardService service,
        object receiver,
        Func<string?> getText,
        ILogger? logger = null,
        Action<string, ClipboardMessageType>? showMessage = null
    )
    {
        return EventCallback.Factory.Create<T>(
            receiver,
            () => service.CopyText(getText(), logger, showMessage)
        );
    }
}
