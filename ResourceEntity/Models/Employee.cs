using System;
using System.Collections.Generic;

namespace ResourceEntity.Models;

public partial class Employee
{
    public int EmployeeId { get; set; }

    public string? EmployeeName { get; set; }

    public string? EmployeeAddress { get; set; }

    public string? EmployeeEmail { get; set; }

    public string? EmployeeCv { get; set; }

    public string? EmployeeAvatar { get; set; }

    public string? EmployeeDepartment { get; set; }

    public string? EmployeePhone { get; set; }

    public string? EmployeeDelete { get; set; }

    public string? EmployeeGender { get; set; }
}
