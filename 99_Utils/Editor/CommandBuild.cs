using System.IO;
using System.Linq;
using UnityEditor;

public class CommandBuild
{
    public const string buildDir = "Build";
    public const string buildName = "TanTanRanger";

    [MenuItem("Tools/StartBuild")]
    public static void StartBuild()
    {
        var sceneNames = EditorBuildSettings.scenes.Select(v => v.path).ToArray();

        var pathDir = Path.GetFullPath(buildDir);
        var pathFile = Path.Combine(pathDir, $"{buildName}.apk");

        if (Directory.Exists(pathDir) == false)
        {
            Directory.CreateDirectory(pathDir);
        }

        BuildPipeline.BuildPlayer(sceneNames, pathFile, BuildTarget.Android, BuildOptions.None);
    }
}
