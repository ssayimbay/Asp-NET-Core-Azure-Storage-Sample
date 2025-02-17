﻿using Azure.Storage.Queues;
using Azure.Storage.Queues.Models;
using AzureStorageLibrary.Abstract;
using AzureStorageLibrary.Constant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureStorageLibrary.Concrete
{
    public class AzQueue : IQueue

    {
        private readonly QueueClient _queueClient;

        public AzQueue()
        {
            _queueClient = new QueueClient(AzureStorageConstant.AzureStorageConnectionString, AzureStorageConstant.QueueName);
            _queueClient.CreateIfNotExists();
        }

        public async Task DeleteMessage(string messageId, string popReceipt)
        {
            await _queueClient.DeleteMessageAsync(messageId, popReceipt);
        }

        public async Task<QueueMessage> RetrieveNextMessageAsync()
        {
            QueueProperties properties = await _queueClient.GetPropertiesAsync();
            if(properties.ApproximateMessagesCount>0)
            {
                QueueMessage[] queueMessages = await _queueClient.ReceiveMessagesAsync(1, TimeSpan.FromMinutes(1));

                if(queueMessages.Any())
                {
                    return queueMessages[0];
                }
            }
            return null;
        }

        public async Task SendMessageAsync(string message)
        {
            await _queueClient.SendMessageAsync(message);
        }
    }
}
