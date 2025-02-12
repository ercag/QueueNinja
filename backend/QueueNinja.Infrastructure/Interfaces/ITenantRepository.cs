using QueueNinja.Domain.Entities;
using System.Threading.Tasks;

namespace QueueNinja.Application.Interfaces
{
    public interface ITenantRepository
    {
        Task<Tenant?> GetTenantByIdAsync(int tenantId);
    }
}
