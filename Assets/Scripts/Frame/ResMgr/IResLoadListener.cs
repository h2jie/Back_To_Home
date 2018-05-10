public interface IResLoadListener
{
    void Finish(object asset);

    void Failure();
}