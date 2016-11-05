using System;
using System.Linq;
using UnityEditor;

public class BuildScript
{
    public static void BuildAndroid()
    {
        Build(BuildTarget.Android);
    }

    public static void BuildOSX()
    {
        Build(BuildTarget.StandaloneOSXIntel64);
    }

    public static void BuildWindows()
    {
        Build(BuildTarget.StandaloneWindows);
    }

    public static void Build(BuildTarget target)
    {
        var scenes = EditorBuildSettings.scenes.Select(x => x.path).ToArray();

        BuildPipeline.BuildPlayer(scenes,
            Environment.GetCommandLineArgs().Last(),
            target,
            BuildOptions.None);
    }
}