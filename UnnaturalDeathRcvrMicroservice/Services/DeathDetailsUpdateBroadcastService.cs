using System;
using Common.Models;
using MediatR;

namespace UnnaturalDeathsRcvrMicroservice.Services
{
    public class DeathDetailsUpdateBroadcastService : IDeathDetailsUpdateBroadcastService
    {
        private readonly IMediator _mediator;

        public DeathDetailsUpdateBroadcastService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public void UpdateDeathDetails(Unnaturaldeaths death)
        {
            // Data is received here from RabbitMQ queue as a listener. 
            // Todo: do anything here. Like: respond to the message
            // by sending another message in the same queue by correlating 
            // the received message so that the sender reads it back and gets to know
            // that this microservice has received the message sent by sender in 
            // the first place.
        }
    }
}