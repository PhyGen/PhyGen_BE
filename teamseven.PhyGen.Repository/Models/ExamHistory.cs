﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace teamseven.PhyGen.Repository.Models;

public partial class ExamHistory
{
    public int Id { get; set; }

    public int ExamId { get; set; }

    public int ActionByUserId { get; set; }

    public string Action { get; set; }

    public string Description { get; set; }

    public DateTime ActionDate { get; set; }

    public virtual User ActionByUser { get; set; }

    public virtual Exam Exam { get; set; }
}