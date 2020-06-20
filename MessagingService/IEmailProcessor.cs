using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService
{
    public interface IEmailProcessor
    {
        Task SendMessageAsync();
        Task ReceiveMessageAsync();
    }
}
