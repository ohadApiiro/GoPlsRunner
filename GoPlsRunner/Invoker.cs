using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace GoPlsRunner;

public class Invoker
{
    public static async Task<InitializeResult> InvokeInitAsync(InitializeParamsExt initParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<InitializeResult>("initialize", rpc, initParams);
    }

    public static async Task InvokeInitializedAsync(JsonRpc rpc)
    {
        await rpc.InvokeWithParameterObjectAsync("initialized", new { });
    }

    public static async Task<Hover> InvokeHoverAsync(TextDocumentPositionParams hoverParams, JsonRpc rpc)
        => await InvokeWithParameterObjectAsync<Hover>("textDocument/hover", rpc, hoverParams);

    public static async Task OpenTextDocumentAsync(DidOpenTextDocumentParams openParams, JsonRpc rpc)
        => await rpc.InvokeWithParameterObjectAsync("textDocument/didOpen", openParams);

    public static async Task ShutdownAsync(JsonRpc rpc)
        => await rpc.InvokeWithParameterObjectAsync("shutdown", new { });


    public static async Task<Location[]> InvokeGotoDefinitionAsync(TextDocumentPositionParams positionParams,
        JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("textDocument/definition", rpc, positionParams);
    }

    public static async Task<Location[]> InvokeFindAllReferencesAsync(TextDocumentPositionParams positionParams,
        JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("textDocument/references", rpc, positionParams);
    }

    public static async Task<Location[]> InvokeGotoDeclarationAsync(TextDocumentPositionParams positionParams,
        JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("textDocument/declaration", rpc, positionParams);
    }

    private static async Task InvokeWithParameterObjectAsync(string targetName, JsonRpc rpc, object argument = null)
    {
        await rpc.InvokeWithParameterObjectAsync(targetName, argument ?? new { });
    }

    private static async Task<TResult> InvokeWithParameterObjectAsync<TResult>(string targetName, JsonRpc rpc,
        object argument = null)
    {
        var result = await rpc.InvokeWithParameterObjectAsync<TResult>(targetName, argument ?? new { });
        return result;
    }
}