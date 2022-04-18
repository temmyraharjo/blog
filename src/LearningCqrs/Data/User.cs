﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using LearningCqrs.Contracts;

namespace LearningCqrs.Data;

public class User : AuditEntity, IUser
{
    [MaxLength(100)]
    public string Username { get; set; }
    [MaxLength(150)]
    public string Password { get; set; }
    [ForeignKey("TimeZone")]
    public Guid? TimeZoneId { get; set; }
    public TimeZoneInfo? TimeZone { get; set; }
}