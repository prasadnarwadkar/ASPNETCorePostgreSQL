using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UnnaturalDeathsMicroservice.ServiceBus
{
    public interface IDeathDetailsSender
    {
        void SendDeathDetails(Unnaturaldeaths death);
    }
}
