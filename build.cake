#tool  "nuget:?package=Node.js.redist&Version=12.18.4"
#tool  "nuget:?package=Yarn.MSBuild&Version=1.22.0"


var nodeExe = GetNode("12.18.4");
var yarnCli  = MakeAbsolute(File($"./tools/yarn.msbuild.1.22.0/dist/bin/yarn.js"));

var target = Argument("target", "build");
var configuration = Argument("configuration", "Debug");


Task("Clean")
    .WithCriteria(c => HasArgument("rebuild"))
    .Does(() =>{
        CleanDirectory($"./src/server/eScratchLottery.Server/bin/{configuration}");
        CleanDirectory($"./src/server/eScratchLottery.Server.Test/bin/{configuration}");
});

Task("BuildWeb")
    .Does(()=>{
        Yarn(path: "./src/frontend", command: "install");
        Yarn(path: "./src/frontend", "build");
 });

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("BuildWeb")
    .Does(() =>{
         DotNetCoreBuild("./src/server/eScratchLottery.sln", new DotNetCoreBuildSettings{
            Configuration = configuration,
        });
});


Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
{
    DotNetCoreTest("./src/server/eScratchLottery.sln", new DotNetCoreTestSettings
    {
        Configuration = configuration,
        NoBuild = true,
    });
});

FilePath GetNode(string version)
{
    var osSpecificExe = string.Empty;

    if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows))
    {
        Information("Running on windows");

        osSpecificExe = "win-x64/node.exe";

    }
    else if (System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux))
    {
        Information("Running on linux");

        osSpecificExe = "linux-x64/node";
    }
    else
    {
        var osNameAndVersion = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        throw new InvalidOperationException($"OS: {osNameAndVersion} is not supported");
    }

    return MakeAbsolute(File($"./tools/Node.js.redist.{version}/tools/{osSpecificExe}"));
}


string Yarn(string path = "./src", string command = null)
{
    if (!FileExists(nodeExe)) {
        throw new Exception($"Can't find path to node exe ({nodeExe})");
    }
    if (!FileExists(yarnCli)) {
        throw new Exception($"Can't find path to yarn cli ({yarnCli})");
    }
    IEnumerable<string> redirectedStandardOutput;
    var rc = StartProcess(nodeExe, new ProcessSettings
    {
        Arguments = yarnCli + " " + command,
        WorkingDirectory = path
    }, out redirectedStandardOutput);

    if (rc != 0) {
        throw new Exception($"{path}>yarn {command} -- failed with {rc}");
    }

    if (redirectedStandardOutput != null)
    {
        var log = redirectedStandardOutput.ToArray();
        return log.FirstOrDefault();
    }

    return null;
}

RunTarget(target);
