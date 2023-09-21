

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
            repositoryRoot: "/Users/ariellevy/dev/adaptiveconsulting-reactivetradercloud",
            fileName:
            "/Users/ariellevy/dev/adaptiveconsulting-reactivetradercloud/src/new-client/src/services/executions/executions.ts",
            position: new Position(43, 8));// lines are 0 based
        
        Console.WriteLine("The end");
        
    }
    
}