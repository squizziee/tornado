using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tornado.Infrastructure.Services.Settings
{
    public record VideoUploadDirectorySettings
    {
        public required string Root { get; set; }
        public required string Source { get; set; }
        public required string Q144p { get; set; }
        public required string Q240p { get; set; }
        public required string Q360p { get; set; }
        public required string Q480p { get; set; }
        public required string Q720p { get; set; }
        public required string Q1080p { get; set; }
        public required string Q1440p { get; set; }
        public required string Q2160p { get; set; }
    }
}
