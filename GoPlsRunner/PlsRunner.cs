using System.Diagnostics;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace GoPlsRunner;

public class PlsRunner
{
    private static JsonRpc _rpc;

    public static async Task Run(string exePath, string repositoryRoot, string fileName, Position position)
    {
        var process = StartProcess(exePath);
        _rpc = new JsonRpc(process.StandardInput.BaseStream, process.StandardOutput.BaseStream);
        _rpc.StartListening();
        await DoInit(repositoryRoot, _rpc);
        
        var positionParams = ParamsFactory.GetPositionParams(fileName, position);
        //var locations = await Invoker.InvokeGotoDefinitionAsync(positionParams, _rpc);

        var referenceParams = ParamsFactory.GetReferenceParams(fileName, position);
        var locations = await Invoker.InvokeFindAllReferencesAsync(referenceParams, _rpc);
        
        PrintLocations(locations);

        await Invoker.ShutdownAsync(_rpc);
    }

    public static async Task Shutdown()
    {
        await Invoker.ShutdownAsync(_rpc);
    }

    private static void PrintLocations(IEnumerable<Location> locations)
    {
        if (locations == null || locations.Count() == 0)
        {
            Console.WriteLine("No results");
            return;
        }

        foreach (var location in locations)
        {
            Console.WriteLine($"{location.Uri} {location.Range.Start.Line}:{location.Range.Start.Character}");
        }
    }

    private static async Task DoInit(string rootPath, JsonRpc rpc)
    {
        InitializeParamsExt initParams = ParamsFactory.GetInitObject(rootPath);
        var res = await Invoker.InvokeInitAsync(initParams, rpc);
        try
        {
            await Invoker.InvokeInitializedAsync(rpc);
        }
        catch (RemoteInvocationException e) when (e.Message.Contains(
                                                      "initialized called while server in initialized state"))
        {
        }
    }

    private static Process StartProcess(string exePath)
    {
        Console.WriteLine($"starting {exePath}");
        var psi = new ProcessStartInfo(exePath);

        var proc = new Process();
        proc.StartInfo = psi;
        proc.StartInfo.RedirectStandardInput = true;
        proc.StartInfo.RedirectStandardOutput = true;
        proc.Start();
        return proc;
    }
}