namespace GoPlsRunner;

public class Utils
{
    public static IEnumerable<string> GetFilePathsRecursive(string path, string searchPattern)
    {
        foreach (var filePath in GetFilePaths(path, searchPattern))
        {
            yield return filePath;
        }

        foreach (var subDirectoryPath in GetDirectoryPaths(path))
        {
            foreach (var filePath in GetFilePathsRecursive(
                         subDirectoryPath,
                         searchPattern
                     ))
            {
                yield return filePath;
            }
        }
    }

    public static IEnumerable<string> GetDirectoryPaths(string path, bool excludeNoiseDirectories = false,
        SearchOption searchOption = SearchOption.TopDirectoryOnly)
    {
        if (searchOption == SearchOption.AllDirectories)
        {
            return GetDirectoryPathsRecursive(path, excludeNoiseDirectories)
                .Except(new[] { path });
        }


        return Directory.GetDirectories(
            path,
            "*",
            searchOption
        );
    }

    private static IEnumerable<string> GetDirectoryPathsRecursive(string path, bool excludeNoiseDirectories = false)
    {
        yield return path;
        foreach (var currentPath in GetDirectoryPaths(path, excludeNoiseDirectories))
        {
            foreach (var subPath in GetDirectoryPathsRecursive(currentPath, excludeNoiseDirectories))
            {
                yield return subPath;
            }
        }
    }

    public static IEnumerable<string> GetFilePaths(string path, string searchPattern = "*") =>
        Directory.GetFiles(
            path,
            searchPattern,
            SearchOption.TopDirectoryOnly
        );
}