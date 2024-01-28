namespace Services.Inputs
{
    public interface IInputService
    {
        bool IsCurrentDevice();
        void OnControls();
        void OffControls();
    }
}