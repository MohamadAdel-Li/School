using Clinics.Core;
using Clinics.Core.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Primitives;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

public class NotificationHub : Hub

{
    private readonly IUnitOfWork _unitOfWork;
    public static readonly ConcurrentDictionary<string, string> userConnectionMap = new ConcurrentDictionary<string, string>();

    private string userId;
    public NotificationHub(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
       
    }

    public async Task SetUserId(string userId)
    {
        this.userId = userId;
        var connectionId = Context.ConnectionId;
        userConnectionMap.AddOrUpdate(userId, connectionId, (key, value) => connectionId);

        foreach (var kvp in userConnectionMap)
        {
            Console.WriteLine($"UserId: {kvp.Key}, ConnectionId: {kvp.Value}");
        }


        Console.WriteLine($"User connected: {userId}");

        // Perform any additional logic with the userId if needed

        // Send acknowledgment back to the client if desired
        await Clients.Caller.SendAsync("UserIdReceived", userId);
    }
    public IDictionary<string, string> GetUserConnectionMap()
    {
        return userConnectionMap;
    }
    //ths method isn't being used at the moment

    public async Task AddAssignment(Assignment assignment)
    {
        // Save the assignment in the database

        int courseId = assignment.CourseID;

        // Retrieve the list of student IDs registered for the course
        var studentIds = await _unitOfWork.StudentCourse.GetStudentbycourse(courseId);


        // Send the notification to each connected student
        foreach (var studentId in studentIds)
        {
            if (userConnectionMap.TryGetValue(studentId, out var connectionId))
            {
                await Clients.Client(connectionId).SendAsync("NewAssignmentAdded", assignment);
            }
        }
    }


}
