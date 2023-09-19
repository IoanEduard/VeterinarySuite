using Core.Entities;
using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PatientsController : BaseApiController
    {
        private readonly IPatientRepository _patientRepository;
        public PatientsController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;
        }

        [HttpGet("allPatients")]
        public async Task<ActionResult<IReadOnlyList<Patient>>> GetPacients()
        {
            return Ok(await _patientRepository.GetPatients());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<IReadOnlyList<Patient>>> GetPacientById(Guid id)
        {
            return Ok(await _patientRepository.GetPatientByIdAsync(id));
        }
    }
}