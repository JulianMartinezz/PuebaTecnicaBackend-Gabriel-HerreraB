using System;
using System.Collections.Generic;

namespace Backend.Models;

public partial class medical_record_type
{
    public int medical_record_type_id { get; set; }

    public string? name { get; set; }

    public string? description { get; set; }

    public virtual ICollection<t_medical_record> t_medical_records { get; set; } = new List<t_medical_record>();
}
