using System.Runtime.Serialization;
using Microsoft.VisualStudio.LanguageServer.Protocol;

namespace GoPlsRunner;

public class InitializeParamsExt
{
    [DataMember(Name = "processId")]
    public int? ProcessId { get; set; }

    [DataMember(Name = "rootPath")]
    public string RootPath { get; set; }

    [DataMember(Name = "initializationOptions")]
    public object InitializationOptions { get; set; }

    [DataMember(Name = "capabilities")]
    public object Capabilities { get; set; }

    [DataMember(Name = "rootUri")]
    public string RootUri { get; set; }

    [DataMember(Name = "workspaceFolders")]
    public WorkspaceFolder[] WorkspaceFolders { get; set; }
}

public class WorkspaceFolder
{
    [DataMember(Name = "uri")]
    public string Uri { get; set; }

    [DataMember(Name = "name")]
    public string Name { get; set; }
}