using Xunit;
using Moq;
using FluentAssertions;
using PatientManagementApi.Controllers;
using PatientManagementApi.Models;
using PatientManagementApi.UnitOfWork; // Update based on your project structure
using Microsoft.AspNetCore.Mvc;
using PatientManagementApi.Repositories.Interfaces;

public class PatientsControllerTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly PatientsController _controller;

    public PatientsControllerTests()
    {
        // Mock the UnitOfWork
        _mockUnitOfWork = new Mock<IUnitOfWork>();

        // Inject the mocked UnitOfWork into the PatientsController
        _controller = new PatientsController(_mockUnitOfWork.Object);
    }

    [Fact]
    public void AddPatient_ShouldReturnBadRequest_WhenInvalidData()
    {
        // Arrange
        var invalidPatient = new Patient
        {
            Name = "", // Invalid Name
            Age = -1   // Invalid Age
        };

        _controller.ModelState.AddModelError("Name", "Name is required.");
        _controller.ModelState.AddModelError("Age", "Age must be a positive number.");

        // Mock the PatientRepository
        var mockPatientRepo = new Mock<IPatientRepository>();

        // No need to return; just simulate AddPatient behavior
        mockPatientRepo.Setup(repo => repo.AddPatient(It.IsAny<Patient>()))
                       .Callback((Patient patient) =>
                       {
                           // Simulate logic, if needed
                       })
                       .Throws(new Exception("Database error"));

        // Set up the mock UnitOfWork to return the mocked repository
        _mockUnitOfWork.Setup(uow => uow.Patients).Returns(mockPatientRepo.Object);

        // Act
        var result = _controller.AddPatient(invalidPatient);

        // Assert
        result.Should().BeOfType<BadRequestObjectResult>();
    }





    [Fact]
    public void AddPatient_ShouldReturnCreated_WhenValidData()
    {
        // Arrange
        var validPatient = new Patient
        {
            Name = "John Doe",
            Age = 30
        };

        // Mock the repository
        var mockPatientRepo = new Mock<IPatientRepository>();
        mockPatientRepo
            .Setup(repo => repo.AddPatient(It.IsAny<Patient>()))
            .Callback<Patient>(p => p.PatientId = 1); // Simulate setting the PatientId after insertion

        _mockUnitOfWork
            .Setup(uow => uow.Patients)
            .Returns(mockPatientRepo.Object);

        _mockUnitOfWork
            .Setup(uow => uow.Commit())
            .Verifiable(); // Ensure Commit is called

        // Act
        var result = _controller.AddPatient(validPatient) as CreatedAtActionResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(201); // Created status code
        result.Value.Should().BeEquivalentTo(validPatient, options => options.Excluding(p => p.PatientId));
        mockPatientRepo.Verify(repo => repo.AddPatient(It.IsAny<Patient>()), Times.Once);
        _mockUnitOfWork.Verify(uow => uow.Commit(), Times.Once); // Ensure Commit is called once
    }

    [Fact]
    public void GetPatients_ShouldReturnPatientsList()
    {
        // Arrange
        var patientsList = new List<Patient>
        {
            new Patient { PatientId = 1, Name = "John Doe", Age = 30 },
            new Patient { PatientId = 2, Name = "Jane Smith", Age = 25 }
        };

        var mockPatientRepo = new Mock<IPatientRepository>();
        mockPatientRepo
            .Setup(repo => repo.GetAllPatients())
            .Returns(patientsList); // Simulate data retrieval

        _mockUnitOfWork
            .Setup(uow => uow.Patients) // Access the correct Patients property
            .Returns(mockPatientRepo.Object);

        // Act
        var result = _controller.GetAllPatients() as OkObjectResult;

        // Assert
        result.Should().NotBeNull();
        result.StatusCode.Should().Be(200);
        result.Value.Should().BeEquivalentTo(patientsList);
    }
}

