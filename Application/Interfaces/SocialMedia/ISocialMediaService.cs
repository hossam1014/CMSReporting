using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.SocialMedia
{
   public interface ISocialMediaService
    {
        Task<bool> ShareToPlatform(string endpoint, string token, string caption, string tag, string mediaUrl);

    }
}
