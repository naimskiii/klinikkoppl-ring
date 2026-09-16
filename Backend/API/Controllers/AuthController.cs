using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Infrastructure;
using System.Security.Cryptography;

namespace API.Controllers;

public class AuthController : BaseController
{
    private readonly StoreContext _context;

    public AuthController(StoreContext context)
    {
        _context = context;
    }

    [HttpPost("create-student")]
    public async Task<ActionResult> CreateStudent(CreateStudentRequest request)
    {
        var student = new Student
        {
            Name = request.Name,
            AccessCode = await GenerateUniqueAccessCode()
        };

        _context.Students.Add(student);
        await _context.SaveChangesAsync();

        return Ok(new { student.Name, student.AccessCode });
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginRequest request)
    {
        var student = await _context.Students
            .FirstOrDefaultAsync(s => s.AccessCode == request.AccessCode);

        if (student == null)
        {
            return Unauthorized(new { message = "Feil kode." });
        }

        return Ok(new { message = "Innlogget!", name = student.Name });
    }

    // Tegnsettet utelater forvirrende tegn (0/O, 1/I/L) slik at studenter
    // ikke mistolker koden når de skriver den av fra papir eller SMS.
    private const string SafeChars = "ABCDEFGHJKMNPQRSTUVWXYZ23456789";

    private static string GenerateAccessCode()
    {
        var buffer = new char[10];
        for (int i = 0; i < buffer.Length; i++)
        {
            buffer[i] = SafeChars[RandomNumberGenerator.GetInt32(SafeChars.Length)];
        }
        return new string(buffer);
    }

    // Sjekker (i det usannsynlige tilfellet) at koden faktisk er unik i databasen
    // før den tas i bruk, i stedet for å bare stole blindt på tilfeldigheten.
    private async Task<string> GenerateUniqueAccessCode()
    {
        string code;
        bool exists;
        do
        {
            code = GenerateAccessCode();
            exists = await _context.Students.AnyAsync(s => s.AccessCode == code);
        } while (exists);

        return code;
    }
}

public class CreateStudentRequest
{
    public string Name { get; set; }
}

public class LoginRequest
{
    public string AccessCode { get; set; }
}