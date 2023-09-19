using Core.Entities;

namespace Core.Interfaces
{
    public interface IPatientRepository
    {
        Task<Patient> GetPatientByIdAsync(Guid id);
        Task<IReadOnlyList<Patient>> GetPatients();
    }
}