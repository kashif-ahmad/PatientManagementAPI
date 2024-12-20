using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Models;
using PatientManagementApi.UnitOfWork;
using PatientManagementApi.Utils;

namespace PatientManagementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PatientsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Dependency Injection: Constructor Injection for the Unit of Work
        public PatientsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Patients
        /// <summary>
        /// Retrieves all patients.
        /// </summary>
        /// <returns>A list of patients</returns>
        [HttpGet]
        public IActionResult GetAllPatients()
        {
            // Repository Pattern: Accessing the repository via Unit of Work
            var patients = _unitOfWork.Patients.GetAllPatients();
            return Ok(patients);
        }

        // GET: api/Patients/{id}
        /// <summary>
        /// Retrieves a specific patient by ID.
        /// </summary>
        /// <param name="id">The patient ID</param>
        /// <returns>A patient or NotFound</returns>
        [HttpGet("{id}")]
        public IActionResult GetPatientById(int id)
        {
            var patient = _unitOfWork.Patients.GetPatientById(id);
            if (patient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }

            return Ok(patient);
        }

        // POST: api/Patients
        /// <summary>
        /// Adds a new patient.
        /// </summary>
        /// <param name="patient">The patient to add</param>
        /// <returns>Status 201 Created</returns>
        [HttpPost]
        public IActionResult AddPatient([FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Patients.AddPatient(patient);
            _unitOfWork.Commit(); // Unit of Work Pattern: Commit transaction
            return CreatedAtAction(nameof(GetPatientById), new { id = patient.PatientId }, patient);
        }

        // PUT: api/Patients/{id}
        /// <summary>
        /// Updates an existing patient.
        /// </summary>
        /// <param name="id">The patient ID</param>
        /// <param name="patient">The updated patient details</param>
        /// <returns>Status 200 OK or 404 Not Found</returns>
        [HttpPut("{id}")]
        public IActionResult UpdatePatient(int id, [FromBody] Patient patient)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingPatient = _unitOfWork.Patients.GetPatientById(id);
            if (existingPatient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }

            // Update logic
            patient.PatientId = id; // Ensure the ID matches
            _unitOfWork.Patients.UpdatePatient(patient);
            _unitOfWork.Commit();
            return Ok(new { Message = "Patient updated successfully" });
        }

        // DELETE: api/Patients/{id}
        /// <summary>
        /// Deletes a patient by ID.
        /// </summary>
        /// <param name="id">The patient ID</param>
        /// <returns>Status 200 OK or 404 Not Found</returns>
        [HttpDelete("{id}")]
        public IActionResult DeletePatient(int id)
        {
            var existingPatient = _unitOfWork.Patients.GetPatientById(id);
            if (existingPatient == null)
            {
                return NotFound(new { Message = "Patient not found" });
            }

            _unitOfWork.Patients.DeletePatient(id);
            _unitOfWork.Commit();
            return Ok(new { Message = "Patient deleted successfully" });
        }
    }
}
