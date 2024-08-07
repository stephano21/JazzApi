﻿using JazzApi.DTOs.Reto;
using JazzApi.Entities.Auditory;
using JazzApi.Entities.Auth;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace JazzApi.Entities.Reto
{
    [Table("TaskNotes", Schema = "CAT")]
    public class TaskNotes: Audit
    {
        [Key]
        public long Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        [ForeignKey("Profile")]
        public string UserId { get; set; }
        //[JsonIgnore]
        public virtual Profile Profile { get; set; }

        public void UpdateTask(TaskDTO data )
        {
            Title = data.Title;
            Description = data.Description; 
        }
    }
}
