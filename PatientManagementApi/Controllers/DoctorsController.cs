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
    public class DoctorsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Dependency Injection: Constructor Injection for Unit of Work
        public DoctorsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Doctors
        [HttpGet]
        public IActionResult GetAllDoctors()
        {
            var doctors = _unitOfWork.Doctors.GetAllDoctors();
            return Ok(doctors);
        }

        // GET: api/Doctors/{id}
        [HttpGet("{id}")]
        public IActionResult GetDoctorById(int id)
        {
            var doctor = _unitOfWork.Doctors.GetDoctorById(id);
            if (doctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }
            return Ok(doctor);
        }

        // POST: api/Doctors
        [HttpPost]
        public IActionResult AddDoctor([FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Doctors.AddDoctor(doctor);
            _unitOfWork.Commit();
            return CreatedAtAction(nameof(GetDoctorById), new { id = doctor.DoctorId }, doctor);
        }

        // PUT: api/Doctors/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateDoctor(int id, [FromBody] Doctor doctor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingDoctor = _unitOfWork.Doctors.GetDoctorById(id);
            if (existingDoctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }

            doctor.DoctorId = id;
            _unitOfWork.Doctors.UpdateDoctor(doctor);
            _unitOfWork.Commit();
            return Ok(new { Message = "Doctor updated successfully" });
        }

        // DELETE: api/Doctors/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteDoctor(int id)
        {
            var existingDoctor = _unitOfWork.Doctors.GetDoctorById(id);
            if (existingDoctor == null)
            {
                return NotFound(new { Message = "Doctor not found" });
            }

            _unitOfWork.Doctors.DeleteDoctor(id);
            _unitOfWork.Commit();
            return Ok(new { Message = "Doctor deleted successfully" });
        }
    }
}
