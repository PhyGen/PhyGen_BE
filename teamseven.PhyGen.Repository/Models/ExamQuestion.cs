﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace teamseven.PhyGen.Repository.Models;

public partial class ExamQuestion
{
    public int Id { get; set; } 
    public int ExamId { get; set; }
    public int QuestionId { get; set; }
    public int Order { get; set; }
    public DateTime CreatedAt { get; set; }
    public virtual Exam Exam { get; set; }
    public virtual Question Question { get; set; }
}