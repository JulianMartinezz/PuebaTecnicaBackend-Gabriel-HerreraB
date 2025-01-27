using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Backend.Models;

public partial class t_medical_record
{
    public int medical_record_id { get; set; }

    public string? audiometry { get; set; }

    public string? position_change { get; set; }

    public string? mother_data { get; set; }

    public string? diagnosis { get; set; }

    public string? other_family_data { get; set; }

    public string? father_data { get; set; }

    public string? execute_micros { get; set; }

    public string? execute_extra { get; set; }

    public string? voice_evaluation { get; set; }
    public DateOnly? deletion_date { get; set; }
    public DateOnly? creation_date { get; set; }
    public DateOnly? modification_date { get; set; }
    public DateOnly? start_date { get; set; }
    public DateOnly? end_date { get; set; }


    public int? status_id { get; set; }

    public int? medical_record_type_id { get; set; }

    public string? disability { get; set; }

    public string? medical_board { get; set; }

    public string? deletion_reason { get; set; }

    public string? observations { get; set; }

    public decimal? disability_percentage { get; set; }

    public string? deleted_by { get; set; }

    public string? created_by { get; set; }

    public string? modified_by { get; set; }

    public string? area_change { get; set; }
   
    [ForeignKey("medical_record_type_id")]
    public virtual medical_record_type? medical_record_type { get; set; }
    [ForeignKey("status_id")]
    public virtual status? status { get; set; }
}
