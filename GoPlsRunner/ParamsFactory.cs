using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace GoPlsRunner;

public class ParamsFactory
{
    private static string _rootPath;
    private static string _cacheFolderPath = "/home/ohad/tmp";

    public static DidOpenTextDocumentParams GetDidOpenParams(string filePath) =>
        new()
        {
            TextDocument = new TextDocumentItem
            {
                Text = File.ReadAllText(filePath),
                Uri = new Uri(FilePathToUriString(filePath)),
                LanguageId = "go",
                Version = 1
            }
        };

    protected static string FilePathToUriString(string filePath)
        => new Uri(filePath).ToString();


    public static TextDocumentPositionParams GetPositionParams(string filePath, Position position)
        => new()
        {
            TextDocument = new() { Uri = new Uri(filePath) },
            Position = position,
        };

    public static TextDocumentPositionParams GetHoverParams(string filePath, Position position)
    {
        return new TextDocumentPositionParams
        {
            Position = position,
            TextDocument = new TextDocumentIdentifier
            {
                Uri = new Uri(filePath)
            }
        };
    }

    public static InitializeParamsExt GetInitObject(string rootPath)
        => new()
        {
            ProcessId = Environment.ProcessId,
            RootUri = rootPath,
            WorkspaceFolders = DetermineWorkspaceFolders(rootPath),
            Capabilities = new ClientCapabilities
            {
                TextDocument = new TextDocumentClientCapabilities
                {
                    Hover = new HoverSetting { ContentFormat = new[] { MarkupKind.PlainText } },
                    References = new DynamicRegistrationSetting()
                },
                Workspace = new WorkspaceClientCapabilities()
            },
            InitializationOptions = new
            {
                symbolStyle = "Full",
                hoverKind = "fullDocumentation"
            }
        };

    private static string FilePathToUri(string filePath)
        => new Uri(filePath).ToString();

    static string RelativizePath(string relativePath, string absolutePath) =>
        Path.GetRelativePath(relativePath, absolutePath);

    private static WorkspaceFolder[] DetermineWorkspaceFolders(string rootPath)
    {
        var moduleDirectories = Utils.GetFilePathsRecursive(rootPath, "go.mod")
            .Select(Path.GetDirectoryName).ToHashSet();
        return moduleDirectories.Count == 0
            ? null
            : moduleDirectories
                .Select(_ => new WorkspaceFolder { Name = RelativizePath(rootPath, _), Uri = FilePathToUri(_) })
                .ToArray();
    }
}