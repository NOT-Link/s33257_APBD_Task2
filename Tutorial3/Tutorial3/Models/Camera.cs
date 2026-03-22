namespace Tutorial3.Models;

public class Camera : Equipment
{
    public double MegaPixels { get; set; }
    public bool HasVideoRecording { get; set; }

    public Camera(string name, double megaPixels, bool hasVideoRecording) : base(name)
    {
        MegaPixels = megaPixels;
        HasVideoRecording = hasVideoRecording;
    }

    public override string GetDetails()
    {
        return $"Camera - {MegaPixels}MP, Video: {(HasVideoRecording ? "Yes" : "No")}";
    }
}
