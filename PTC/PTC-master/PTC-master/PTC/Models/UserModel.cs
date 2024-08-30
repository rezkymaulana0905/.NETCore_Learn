namespace PTC.Models;

public class User
{
    public int Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}

public class PasswordDump
{
    public int Id { get; set; }
    public required int UserId { get; set; }
    public required string Password { get; set; }
    public DateTime CreateDate { get; set; }
}

public class SsoUser
{
    public required string EmpId { get; set; }
    public required string Username { get; set;}
    public required int RoleId { get; set; }
}

public class UserRecord
{
    public int Id { get; set; }
    public required string Username { get; set;}
    public required string Address { get; set;}
    public required DateTime LoginTime { get; set; }
}

public class UserRole
{
    public int Id { get; set; }
    public required string RoleName { get; set; }
}

public class UserAuth
{
    public required string UserName { get; set; }
    public required int UserId { get; set; }
    public  int LoginId { get; set; }
}

public class UserLock
{
    public int UserId { get; set; }
    public DateTime CreateDate { get; set; }
}

public class UserPasswordAttempt
{
    public int UserId { get; set; }
    public int Attempt { get; set; }
}