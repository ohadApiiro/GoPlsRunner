

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
        Foo(Blas);
        var exePath = "/home/ohad/src/go/tools/gopls/gopls";
        await PlsRunner.Run(
            exePath,
            repositoryRoot: "/home/ohad/src/repros/cmp_main",
            fileName:
            "/home/ohad/src/repros/cmp_main/services/flexsave/aws/functions/aws-account-nuker/rebuy-mocks/mock_cloudformationiface/mock.go",
            position: new Position(4127, 28)); // lines are 0 based
        
        Console.WriteLine("The end");
        
    }

    public static void Foo(Func<int, string> func)
    {
        var s = func(1);
        Console.WriteLine(s);
        
    }
}