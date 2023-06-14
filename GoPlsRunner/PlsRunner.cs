using System.Diagnostics;
using Microsoft.VisualBasic.FileIO;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using StreamJsonRpc;

namespace GoPlsRunner;

public class PlsRunner
{
    private static JsonRpc _rpc;

    public static async Task Run(string exePath, string repositoryRoot, string fileName, Position position)
    {
        var process = StartProcess("/Users/ariellevy/.nvm/versions/node/v16.17.0/bin/typescript-language-server");
        _rpc = new JsonRpc(process.StandardInput.BaseStream, process.StandardOutput.BaseStream);
        _rpc.StartListening();
        try
        {
            await DoInit(repositoryRoot, _rpc);
        }
        catch (Exception e)
        {
            Console.WriteLine("poop");
        }
        await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/catalog.js", _rpc);
        await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/index.js", _rpc);
        // await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/orders.js", _rpc);
        await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/referenceData.js", _rpc);
        await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/security.js", _rpc);
        await DoOpenFile("/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/superAdmin.js", _rpc);

        await DoOpenFile(fileName, _rpc);
        
        var positionParams = ParamsFactory.GetPositionParams(fileName, position);
        for (int i = 0; i < 2; i++)
        {
            try
            {
                var x = await Invoker.InvokeGotoDefinitionThroughCommandAsync(positionParams, _rpc);
                var y = await Invoker.GotoDefinitionAsync(positionParams, _rpc);
                var hover = await Invoker.InvokeHoverAsync(positionParams, _rpc);
                 var locations = await Invoker.GotoTypeDefinitionAsync(positionParams, _rpc);
                PrintLocations(x);
            }
            catch
            {
                // ignored
            }


        }

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
        var initParams = ParamsFactory.GetInitObject(rootPath);
        var res = await Invoker.InvokeInitAsync(initParams, rpc);
        try
        {
            await Invoker.InvokeInitializedAsync(rpc);
        }
        catch (RemoteInvocationException e) when (e.Message.Contains("initialized called while server in initialized state"))
        {
        }
    }
    
    private static async Task DoOpenFile(string filePath, JsonRpc rpc)
    {
        var fileContent = await File.ReadAllTextAsync(filePath);
        var didOpenParams = ParamsFactory.GetOpenFileObject(filePath, fileContent);
        try
        {
            await Invoker.OpenTextDocumentAsync(didOpenParams, _rpc);
        }
        catch (RemoteInvocationException e) when (e.Message.Contains("initialized called while server in initialized state"))
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
        proc.StartInfo.WorkingDirectory = "/Users/ariellevy/dev/express-api-example/";
        proc.StartInfo.Arguments = "--stdio";
        proc.Start();
        return proc;
    }
}