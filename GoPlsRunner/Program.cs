

using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace GoPlsRunner;

class Program
{
    private static string Blas(int x)
    {
        Console.WriteLine(x);
        return "bla";
    }
    
    static async Task Main(string[] args)
    {
        
        var exePath = "/Users/ariellevy/.nvm/versions/node/v16.17.0/bin/typescript-language-server";
        await PlsRunner.Run(
            exePath,
            repositoryRoot: "/Users/ariellevy/dev/viveckh-veniqa",
            fileName:
            "/Users/ariellevy/dev/viveckh-veniqa/management-server/routes/orders.js",
            position: new Position(17, 79));// lines are 0 based
        
        Console.WriteLine("The end");
        
    }
    
}