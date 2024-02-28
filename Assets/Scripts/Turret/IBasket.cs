namespace Turret
{
    public interface IBasket
    {
        bool IsEmpty { get; }
        void SpendBox();
    }
}