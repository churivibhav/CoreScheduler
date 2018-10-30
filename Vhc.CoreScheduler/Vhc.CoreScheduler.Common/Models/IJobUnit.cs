namespace Vhc.CoreScheduler.Common.Models
{
    public interface IJobUnit
    {
        string Content { get; set; }
        int Id { get; set; }
        string Name { get; set; }
        UnitType Type { get; set; }
    }
}