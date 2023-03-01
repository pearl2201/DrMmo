using DarkRift.Server;
using System.Collections.Specialized;
using System.Collections;

using DarkRift.Server.Configuration;

using MmoConsole;
string[] rawArguments = CommandEngine.ParseArguments(string.Join(" ", args));
string[] arguments = CommandEngine.GetArguments(rawArguments);
NameValueCollection variables = CommandEngine.GetFlags(rawArguments);

foreach (DictionaryEntry environmentVariable in Environment.GetEnvironmentVariables())
	variables.Add((string)environmentVariable.Key, (string)environmentVariable.Value);

string serverConfigFile;
string clusterConfigFile;
if (arguments.Length < 1)
{
	serverConfigFile = "Server.config";
	clusterConfigFile = "Cluster.config";
}
else if (arguments.Length == 1)
{
	serverConfigFile = arguments[0];
	clusterConfigFile = "Cluster.config";
}
else if (arguments.Length == 2)
{
	serverConfigFile = arguments[0];
	clusterConfigFile = arguments[1];
}
else
{
	System.Console.Error.WriteLine("Unexpected number of comand line arguments passed. Expected 0-2 but found " + arguments.Length + ".");
	System.Console.WriteLine("Press any key to exit...");
	System.Console.ReadKey();
	return;
}
DarkRiftServerConfigurationBuilder serverConfigurationBuilder;


try
{
	serverConfigurationBuilder = DarkRiftServerConfigurationBuilder.CreateFromXml(serverConfigFile, variables);
}
catch (IOException e)
{
	System.Console.Error.WriteLine("Could not load the server config file needed to start (" + e.Message + "). Are you sure it's present and accessible?");
	System.Console.WriteLine("Press any key to exit...");
	System.Console.ReadKey();
	return;
}
catch (XmlConfigurationException e)
{
	System.Console.Error.WriteLine($"Failed to load '{serverConfigFile}': {e.Message}");
	System.Console.Error.WriteLine();
	System.Console.Error.WriteLine(e.DocumentationLink != null ? $"See {e.DocumentationLink} for more information." : "No additional documentation available.");
	System.Console.Error.WriteLine();
	System.Console.Error.WriteLine(e.LineInfo != null && e.LineInfo.HasLineInfo() ? $"Line {e.LineInfo.LineNumber} Col: {e.LineInfo.LinePosition}" : "(Unknown location)");
	System.Console.Error.WriteLine();
	System.Console.WriteLine("Press any key to exit...");
	System.Console.ReadKey();
	return;
}

// Set this thread as the one executing dispatcher tasks
serverConfigurationBuilder.WithDispatcherExecutorThreadID(Thread.CurrentThread.ManagedThreadId);
DarkRiftServer server;
if (File.Exists(clusterConfigFile))
{
	DarkRiftClusterConfigurationBuilder clusterConfigurationBuilder;
	try
	{
		clusterConfigurationBuilder = DarkRiftClusterConfigurationBuilder.CreateFromXml(clusterConfigFile, variables);
	}
	catch (IOException e)
	{
		System.Console.Error.WriteLine("Could not load the cluster config file needed to start (" + e.Message + "). Are you sure it's present and accessible?");
		System.Console.WriteLine("Press any key to exit...");
		System.Console.ReadKey();
		return;
	}
	catch (XmlConfigurationException e)
	{
		System.Console.Error.WriteLine($"Failed to load '{clusterConfigFile}': {e.Message}");
		System.Console.Error.WriteLine();
		System.Console.Error.WriteLine(e.DocumentationLink != null ? $"See {e.DocumentationLink} for more information." : "No additional documentation available.");
		System.Console.Error.WriteLine();
		System.Console.Error.WriteLine(e.LineInfo != null && e.LineInfo.HasLineInfo() ? $"Line {e.LineInfo.LineNumber} Col: {e.LineInfo.LinePosition}" : "(Unknown location)");
		System.Console.Error.WriteLine();
		System.Console.WriteLine("Press any key to exit...");
		System.Console.ReadKey();
		return;
	}

	server = new DarkRiftServer(serverConfigurationBuilder.ServerSpawnData, clusterConfigurationBuilder.ClusterSpawnData);
}
else
{
	server = new DarkRiftServer(serverConfigurationBuilder.ServerSpawnData);
}




IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
		services.AddScoped<DarkRiftServer>((services) => server);
		services.AddHostedService<Worker>();
    })
    .Build();

await host.RunAsync();
