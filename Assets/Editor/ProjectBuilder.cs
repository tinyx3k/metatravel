
using UnityEditor;
using System.Diagnostics;
public class ProjectBuilder {
    [UnityEditor.MenuItem("CustomScripts/Build Windows")]
    public static void BuildAndroid()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[]
        {
            "Assets/Scenes/Main.unity",
            "Assets/Scenes/Structures/MangLang.unity",
            "Assets/Scenes/Structures/NghinhPhong.unity",
            "Assets/Scenes/Structures/Nhan.unity",
            "Assets/Scenes/Structures/ThanhLuong.unity"
        };
        buildPlayerOptions.locationPathName = "builds/x64";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.None;
        BuildPipeline.BuildPlayer(buildPlayerOptions);
    }
}
 