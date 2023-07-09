using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Layer3Objects;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace ShopBase
{
    public class Picture
    {
        public int Id { get; set; }
        public string Filename { get; set; }

        [JsonIgnore] // For not display in Json
        public byte[] Data { get; set; }
        public Article Article { get; set; }

        /// Returns img as String for HTML


        public Picture()
        {

        }

        public Picture(string path)
        {
            string[] a = path.Split('\\');
            Filename = a[a.Length - 1];
            Data = File.ReadAllBytes(path);

            Resize();
        }

        public Picture(int id, string filename, byte[] ba)
        {
            Id = id;
            Filename = filename;
            Data = ba;
        }

        public Picture(string filename, byte[] ba, Article a = null)
        {
            Filename = filename;
            Data = ba;
            Article = a;
        }


        public string GetAsString()
        {
            if (Data == null)
                return "";

            string imageBase64Data = Convert.ToBase64String(Data);
            return $"data:image/jpg;base64,{imageBase64Data}";
        }
        public void SaveToFile(string path)
        {
            File.WriteAllBytes(path, Data);
        }
        public void Resize()
        {
            // Tests mit ImageSharp (SixLabor) zum Skalieren des Bildes

            if (Data.Length > 11000) //Rest is for Overhead
            {
                double factor = 1.0 / (Data.Length / 11000.0);

                MemoryStream ms = new MemoryStream();
                ms.Write(Data, 0, Data.Length);
                ms.Seek(0, SeekOrigin.Begin);
                using (Image image = Image.Load(ms))
                {
                    int width = (int)(image.Width * factor);
                    int height = (int)(image.Height * factor);

                    image.Mutate(x => x.Resize(width, height, KnownResamplers.Lanczos3));


                    MemoryStream outStream = new MemoryStream();
                    //image.Save(outStream, new PngEncoder());    //  Png encoder
                    image.Save(outStream, new JpegEncoder()); // Jpeg encoder
                    Data = outStream.ToArray();
                }
            }
        }

        public void Insert() => DBObjects.Insert(this);
        public void Change() => DBObjects.Change<Picture>(this);
        public static Picture? Get(int id)
        {
            try
            {
                return DBObjects.ReadAll<Picture>(id)[0];
            }
            catch (Exception)
            {
                return null;
            }
        }
        public static List<Picture> GetAll() => DBObjects.ReadAll<Picture>();
        public static Picture? GetFromArticle(Article a) => DBObjects.GetFromArticle(a);
    }
}
