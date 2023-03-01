using DarkRift.Server;
using DarkRift.Server.Configuration;

namespace MmoConsole
{
	public class Worker : BackgroundService
	{
		private readonly ILogger<Worker> _logger;
		private readonly DarkRiftServer _server;

		public Worker(ILogger<Worker> logger, DarkRiftServer server)
		{
			_logger = logger;
			_server = server;
		}

		protected override async Task ExecuteAsync(CancellationToken stoppingToken)
		{
			new Thread(new ThreadStart(ConsoleLoop)).Start();

			while (!stoppingToken.IsCancellationRequested && !_server.Disposed)
			{
				_server.DispatcherWaitHandle.WaitOne();
				_server.ExecuteDispatcherTasks();
			}
		}
		/// <summary>
		///     Invoked from another thread to repeatedly execute commands from the console.
		/// </summary>
		private void ConsoleLoop()
		{
			while (!_server.Disposed)
			{
				string input = System.Console.ReadLine();

				if (input == null)
				{
					System.Console.WriteLine("Stopping input loop as we seem to be running without an input stream.");
					return;
				}

				_server.ExecuteCommand(input);
			}
		}
	}

}