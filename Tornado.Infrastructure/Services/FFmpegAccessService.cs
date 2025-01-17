using Microsoft.Extensions.Options;
using System.Diagnostics;
using Tornado.Infrastructure.Services.Interfaces;
using Tornado.Infrastructure.Services.Settings;

namespace Tornado.Infrastructure.Services
{
	public class FFmpegAccessService : IFFmpegAccessService
	{
		private FFmpegSettings _settings;

		public FFmpegAccessService(IOptions<FFmpegSettings> options) { 
			_settings = options.Value;
		}
		public async Task ExecuteWithArguments(string args)
		{
			var processInfo = new ProcessStartInfo
			{
				CreateNoWindow = false,
				UseShellExecute = false,
				FileName = _settings.ExePath,
				WindowStyle = ProcessWindowStyle.Normal,
				Arguments = args
			};

			try
			{
				using (var exeProcess = Process.Start(processInfo))
				{
					await exeProcess!.WaitForExitAsync();
				}
			}
			catch (Exception ex)
			{
				{
					throw new Exception("FFmpeg execution error:" + ex.Message);
				}
			}
		}
	}
}
