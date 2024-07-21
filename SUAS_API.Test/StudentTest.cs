using Microsoft.AspNetCore.Mvc;
using NUnit.Framework;
using SUAS_API.Commands;
using SUAS_API.Controllers;
using SUAS_API.Models;
using Shouldly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using SUAS_API.Data;
using Microsoft.AspNetCore.Components.Forms;
using Moq;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Queries;
using SUAS_API.Handlers;
using Moq.EntityFrameworkCore;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace SUAS_API.Test
{

    [TestFixture]
    public class StudentTest
    {
        private Mock<IMediator> _mediatorMock;
        private StudentsController _studentController;
        private AppDBContext _appDBContext;
        private Student Student1;
        private Student Student2;
        private Student UpdatedStudent1;
        public AppDBContext GetMemoryContext()
        {
            var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: "TestDB")
            .Options;
            return new AppDBContext(options);
        }
        [SetUp]
        public void SetUp()
        {
            string dateString = "1989-03-23T16:35:09.204Z";
            string format = "yyyy-MM-ddTHH:mm:ss.fffZ";

            Student1 = new Student
            {
                ID = 1,
                FirstName = "Josemari",
                LastName = "Acielo",
                DateOfBirth = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind)
            };
            Student2 = new Student
            {
                ID = 2,
                FirstName = "Josemari",
                LastName = "Acielo",
                DateOfBirth = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind)
            };
            UpdatedStudent1 = new Student
            {
                ID = 1,
                FirstName = "Jheyem",
                LastName = "Acielo",
                DateOfBirth = DateTime.ParseExact(dateString, format, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.RoundtripKind)
            };
        }
        [TearDown]
        public void TearDown()
        { 
            //var db = GetMemoryContext();
            //db.Database.EnsureDeleted();
        }
        [Test,Order(1)]
        public async Task TestAddStudent()
        {
            var db = GetMemoryContext();
            PostStudentRequest req1 = new PostStudentRequest(Student1);
            PostStudentRequest req2 = new PostStudentRequest(Student2);
            PostStudentHandler handler = new PostStudentHandler(db);
            var addStudent1 = await handler.Handle(req1, new System.Threading.CancellationToken());
            addStudent1.StudentInfo.ShouldBe(Student1);
            var addStudent2 = await handler.Handle(req2, new System.Threading.CancellationToken());
            addStudent2.StudentInfo.ShouldBe(Student2);
        }

        [Test,Order(2)]
        public async Task TestGetStudent()
        {
            var db = GetMemoryContext();
            GetStudentQuery studentQ1 = new GetStudentQuery(1);
            GetStudentQuery studentQ2 = new GetStudentQuery(2);
            GetStudentHandler handler = new GetStudentHandler(db);
            var getStudent1 = await handler.Handle(studentQ1,new System.Threading.CancellationToken());
            var getStudent2 = await handler.Handle(studentQ2, new System.Threading.CancellationToken());
            getStudent1.ID.ShouldBe(Student1.ID);
            getStudent2.ID.ShouldBe(Student2.ID);
        }
        [Test, Order(3)]
        public async Task UpdateStudent1()
        {
            var db = GetMemoryContext();
            UpdateStudentRequest updateStudent1 = new UpdateStudentRequest(UpdatedStudent1);
            UpdateStudentHandler handler = new UpdateStudentHandler(db);
            var update = await handler.Handle(updateStudent1, new System.Threading.CancellationToken());
            update.Success.ShouldBeTrue();
            update.UpdatedStudentInfo.FirstName.ShouldBe(UpdatedStudent1.FirstName);
        }
        [Test,Order(4)]
        public async Task DeleteStudent()
        {
            var db = GetMemoryContext();
            DeleteStudentRequest deleteStudent1 = new DeleteStudentRequest(1);
            DeleteStudentHandler handler = new DeleteStudentHandler(db);
            var deleteS1 = await handler.Handle(deleteStudent1,new System.Threading.CancellationToken());
            deleteS1.Success.ShouldBeTrue();
        }
        [Test, Order(5)]
        public async Task GetUpdatedCountofStudents()
        {
            var db = GetMemoryContext();
            GetAllStudentsQuery getAllStudents = new GetAllStudentsQuery();
            GetAllStudentsHandler handler = new GetAllStudentsHandler(db);
            var allStudents = await handler.Handle(getAllStudents, new System.Threading.CancellationToken());
            allStudents.Count().ShouldBe(1);
        }
    }
}
