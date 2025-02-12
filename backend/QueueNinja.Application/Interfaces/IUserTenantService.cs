namespace QueueNinja.Application.Interfaces
{
    public interface IUserTenantService
    {
        Task AssignUserToTenantAsync(string userId, int tenantId);
    }
}
