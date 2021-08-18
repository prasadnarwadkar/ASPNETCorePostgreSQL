using Common.Models;

namespace UnnaturalDeathsRcvrMicroservice.Services
{
    public interface IDeathDetailsUpdateBroadcastService
    {
        void UpdateDeathDetails(UnnaturalDeaths death);
    }
}