using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MessagingService
{
    public interface IMessageProcessor
    {
        Task SendMessageAsync();
        Task ReceiveMessageAsync();
    }
}
