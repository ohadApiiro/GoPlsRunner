using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace GoPlsRunner;

public class Invoker
{
    public static async Task<InitializeResult> InvokeInitAsync(object initParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<InitializeResult>("initialize", rpc, initParams);
    }

    public static async Task InvokeInitializedAsync(JsonRpc rpc)
    {
        await rpc.NotifyAsync("initialized", new { });
    }

    public static async Task<object> InvokeHoverAsync(TextDocumentPositionParams hoverParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<object>("textDocument/hover", rpc, hoverParams);
    }
    
    public static async Task<object> InvokeImplementation(TextDocumentPositionParams hoverParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<object>("textDocument/implementation", rpc, hoverParams);
    }
    
    public static async Task OpenTextDocumentAsync(DidOpenTextDocumentParams openParams, JsonRpc rpc)
    {
        await rpc.NotifyWithParameterObjectAsync("textDocument/didOpen", openParams);
    }

    public static async Task ShutdownAsync(JsonRpc rpc)
    {
        await rpc.InvokeWithParameterObjectAsync("shutdown", new { });
    }


    public static async Task<Location[]> InvokeGotoDefinitionThroughCommandAsync(TextDocumentPositionParams positionParams,
        JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("workspace/executeCommand", rpc,
            new Dictionary<string, object>
            {
                { "command", "_typescript.goToSourceDefinition" },
                {
                    "arguments", new List<object> { positionParams.TextDocument.Uri, positionParams.Position }
                }
            });
    }

    public static async Task<Location[]> GotoDefinitionAsync(TextDocumentPositionParams positionParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("textDocument/definition", rpc, positionParams);
    }
    
    public static async Task<Location[]> GotoTypeDefinitionAsync(TextDocumentPositionParams positionParams, JsonRpc rpc)
    {
        return await InvokeWithParameterObjectAsync<Location[]>("textDocument/typeDefinition", rpc,
            positionParams);
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