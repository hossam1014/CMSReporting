using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Abstractions;

namespace Application.Errors.SocialMedia
{
   public class SocialMediaErrors
    {
        public static readonly Error ReportNotFound =
            new("SocialMedia.ReportNotFound", "The requested report was not found.");

        public static readonly Error UnsupportedPlatform =
            new("SocialMedia.UnsupportedPlatform", "The provided sharing platform is not supported.");

        public static readonly Error ShareFailed =
            new("SocialMedia.ShareFailed", "Failed to share the report on social media.");
    }
}
