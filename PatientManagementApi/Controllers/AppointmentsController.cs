using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Models;
using PatientManagementApi.UnitOfWork;

namespace PatientManagementApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        // Dependency Injection: Constructor Injection for Unit of Work
        public AppointmentsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // GET: api/Appointments
        [HttpGet]
        public IActionResult GetAllAppointments()
        {
            var appointments = _unitOfWork.Appointments.GetAllAppointments();
            return Ok(appointments);
        }

        // GET: api/Appointments/{id}
        [HttpGet("{id}")]
        public IActionResult GetAppointmentById(int id)
        {
            var appointment = _unitOfWork.Appointments.GetAppointmentById(id);
            if (appointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }
            return Ok(appointment);
        }

        // POST: api/Appointments
        [HttpPost]
        public IActionResult AddAppointment([FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _unitOfWork.Appointments.AddAppointment(appointment);
            _unitOfWork.Commit();
            return CreatedAtAction(nameof(GetAppointmentById), new { id = appointment.AppointmentId }, appointment);
        }

        // PUT: api/Appointments/{id}
        [HttpPut("{id}")]
        public IActionResult UpdateAppointment(int id, [FromBody] Appointment appointment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var existingAppointment = _unitOfWork.Appointments.GetAppointmentById(id);
            if (existingAppointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            appointment.AppointmentId = id;
            _unitOfWork.Appointments.UpdateAppointment(appointment);
            _unitOfWork.Commit();
            return Ok(new { Message = "Appointment updated successfully" });
        }

        // DELETE: api/Appointments/{id}
        [HttpDelete("{id}")]
        public IActionResult DeleteAppointment(int id)
        {
            var existingAppointment = _unitOfWork.Appointments.GetAppointmentById(id);
            if (existingAppointment == null)
            {
                return NotFound(new { Message = "Appointment not found" });
            }

            _unitOfWork.Appointments.DeleteAppointment(id);
            _unitOfWork.Commit();
            return Ok(new { Message = "Appointment deleted successfully" });
        }
    }
}
