public class CreateWarrantyDTO
{

    public string? Name { get; set; } 

    public string? Description { get; set; }

    public int? PeriodMonths { get; set; }

    public int? IndexShow { get; set; }

    public bool? IsActive { get; set; } 

}

public class UpdateWarrantyDTO
{

    public string? Name { get; set; } 

    public string? Description { get; set; } 
    public int? PeriodMonths { get; set; }

    public int? IndexShow { get; set; }

    public bool? IsActive { get; set; } 

}