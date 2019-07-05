namespace Domain.ProductContext
{
    public class ImageRef : ValueObject<ImageRef>
    {
        public ImageRef(string relativeUrl, string relativeThumbUrl)
        {
            RelativeUrl = relativeUrl;
            RelativeThumbUrl = relativeThumbUrl;
        }

        public string RelativeUrl { get; }
        public string RelativeThumbUrl { get; }
    }
}