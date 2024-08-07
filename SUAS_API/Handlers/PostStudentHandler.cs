﻿using MediatR;
using Microsoft.EntityFrameworkCore;
using SUAS_API.Commands;
using SUAS_API.Data;
using SUAS_API.Helpers;
using SUAS_API.Models;
using SUAS_API.Responses;

namespace SUAS_API.Handlers
{
    public class PostStudentHandler:IRequestHandler<PostStudentRequest,PostStudentResponse>
    {
        private readonly AppDBContext _dbContext;

        public PostStudentHandler(AppDBContext StudentDbContext)
        {
            _dbContext = StudentDbContext;
        }

        public async Task<PostStudentResponse> Handle(PostStudentRequest request, CancellationToken cancellationToken)
        {
            PostStudentResponse response = new PostStudentResponse();
            try
            {
                response.StudentInfo = request.Student;
                response.Success = false;
                var existingStudentRecord = await _dbContext.Students.FindAsync(request.Student.ID);
                if (existingStudentRecord == null)
                {
                    _dbContext.Students.Add(request.Student);
                    await _dbContext.SaveChangesAsync();
                    response.StudentInfo = request.Student;
                    response.Success = true;
                    response.Message = "Record Saved.";
                }
                response.Message = "Student ID is already existing";
                return response;
            }
            catch (Exception ex)
            {
                var ErrorReferenceNumber = Utility.LogTheError(ex);
                response.Success = false;
                response.Message = $"Unable to save the record. Error Reference Number: {ErrorReferenceNumber}";
                return response;
            }
        }
    }
}
