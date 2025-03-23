namespace FileConverter
{
    public interface ILoadTest
    {
        void Perform(int maxParallelism);
    }
}
