namespace Domain.ProductContext
{
    public class ImageRef : ValueObject<ImageRef>
    {
        public ImageRef(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}