using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;
using Newtonsoft.Json;

namespace GoPlsRunner;

public class ParamsFactory
{
    private static string _rootPath;
    private static string _cacheFolderPath = "/home/ohad/tmp";

    public static DidOpenTextDocumentParams GetDidOpenParams(string filePath)
    {
        return new DidOpenTextDocumentParams
        {
            TextDocument = new TextDocumentItem
            {
                Text = File.ReadAllText(filePath),
                Uri = new Uri(FilePathToUriString(filePath)),
                LanguageId = "go",
                Version = 1
            }
        };
    }

    protected static string FilePathToUriString(string filePath)
    {
        return new Uri(filePath).ToString();
    }


    public static TextDocumentPositionParams GetPositionParams(string filePath, Position position)
    {
        return new TextDocumentPositionParams
        {
            TextDocument = new TextDocumentIdentifier { Uri = new Uri(filePath) },
            Position = position
        };
    }

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

    public static object GetInitObject(string rootPath)
    {
        return new Dictionary<string, object>
        {
            { "processId", Environment.ProcessId },
            { "clientInfo", new Dictionary<string, string> { { "name", "" }, { "version", "" } } },
            { "locale", "" },
            { "rootUri", new Uri(FilePathToUri(rootPath)).ToString() },
            { "rootPath", new Uri(FilePathToUri(rootPath)).ToString() },
            { 
                "capabilities", new Dictionary<object, object>
                {
                    { "workspace", null },
                    {
                        "textDocument", new Dictionary<string, object>
                        {
                            {
                                "definition", new Dictionary<string, object>{
                                {
                                    "linkSupport", true
                                }}
                            }
                        }
                    },
                    { "notebookDocument", null },
                    { "window", null },
                    { "general", null },
                    { "experimental", null }
                }
            },
            {
                "initializationOptions", new Dictionary<object, object>
                {
                    { "preferences", "test" },
                //     {
                //     "tsserver", new Dictionary<string,object>
                //     {
                //         {"useSyntaxServer", "auto"}
                //     }
                // }
                }
            },
            { "trace", null },

        };
    }

    public static DidOpenTextDocumentParams GetOpenFileObject(string rootPath, string content)
    {
        return new DidOpenTextDocumentParams()
        {
            TextDocument = new TextDocumentItem()
            {
                Uri = new Uri(rootPath),
                LanguageId = "typescript",
                Version = 0,
                Text = content
            }
        };
    }
    [DataContract]
    public class MyDidOpenTextDocumentParams
    {
        /// <summary>
        /// Gets or sets the <see cref="T:Microsoft.VisualStudio.LanguageServer.Protocol.TextDocumentItem" /> which represents the text document that was opened.
        /// </summary>
        [DataMember(Name = "textDocument")]
        public MyTextDocumentItem TextDocument { get; set; }
    }
    public class MyTextDocumentItem
    {
        /// <summary>Gets or sets the document URI.</summary>
        [DataMember(Name = "uri")]
        [JsonConverter(typeof (DocumentUriConverter))]
        public string Uri { get; set; }

        /// <summary>Gets or sets the document language identifier.</summary>
        [DataMember(Name = "languageId")]
        public string LanguageId { get; set; }

        /// <summary>Gets or sets the document version.</summary>
        [DataMember(Name = "version")]
        public int Version { get; set; }

        /// <summary>Gets or sets the content of the opened text document.</summary>
        [DataMember(Name = "text")]
        public string Text { get; set; }
    }

    private static string FilePathToUri(string filePath)
    {
        return new Uri(filePath).ToString();
    }

    private static string RelativizePath(string relativePath, string absolutePath)
    {
        return Path.GetRelativePath(relativePath, absolutePath);
    }

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