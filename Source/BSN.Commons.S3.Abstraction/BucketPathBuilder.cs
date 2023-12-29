using System.Text;

namespace BSN.Commons.S3.Abstraction
{
    public class BucketPathBuilder
    {
        public static string Build(params string[] buckets)
        {
            StringBuilder Builder = new StringBuilder();

            for (int i = 0; i < buckets.Length; i++)
            {
                if (buckets[i] != string.Empty)
                {
                    if (i == buckets.Length - 1)
                        Builder.Append(buckets[i]);
                    else
                    {
                        Builder.Append(buckets[i]);
                        Builder.Append("/");
                    }
                }
            }

            return Builder.ToString();
        }
    }
}