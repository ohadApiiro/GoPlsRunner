

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace GoPlsRunner;

class Program
{
    static async Task Main(string[] args)
    {
        var exePath = "/home/ohad/src/go/tools/gopls/gopls";
        await PlsRunner.Run(
            exePath,
            repositoryRoot: "/home/ohad/src/repros/dum",
            fileName:
            "/home/ohad/src/repros/dum/abusiveexperiencereport/v1/abusiveexperiencereport-gen.go",
            position: new Position(80, 7)); // lines are 0 based
        
        Console.WriteLine("The end");
    }
    
}