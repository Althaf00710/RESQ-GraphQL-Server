namespace Core.DTO
{
    public class EmergencyCategoryCreateInput
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }

    public class EmergencyCategoryUpdateInput
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public string? Icon { get; set; }
    }
}
