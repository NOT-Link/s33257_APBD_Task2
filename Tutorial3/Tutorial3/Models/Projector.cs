namespace Tutorial3.Models;

public class Projector : Equipment
{
    public int LumensOutput { get; set; }
    public string Resolution { get; set; }

    public Projector(string name, int lumensOutput, string resolution) : base(name)
    {
        LumensOutput = lumensOutput;
        Resolution = resolution;
    }

    public override string GetDetails()
    {
        return $"Projector - Lumens: {LumensOutput}, Resolution: {Resolution}";
    }
}
