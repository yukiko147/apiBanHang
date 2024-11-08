namespace BackendWebBanLinhKien.Hepler
{
    public class UploadFile
    {
        public string Upload(IFormFile file)
        {
            try
            {
                List<string> filesExtention = new List<string>() { ".jpg", ".png", ".gif" };
                string ex = Path.GetExtension(file.FileName);
                if (!filesExtention.Contains(ex))
                {
                    return $"Extention is not valid({string.Join(',', filesExtention)})";
                }

                long size = file.Length;
                if (size > (5 * 1024 * 1024))
                {
                    return "Maximun size can be 5mb";
                }

                string filename = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), "Img");
                FileStream stream = new FileStream(Path.Combine(path, filename), FileMode.Create);
                file.CopyTo(stream);
                stream.Dispose();
                stream.Close();
                return filename;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public string getFileBase64(string fileName)
        {
            try
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(), "Img", fileName);
                if (!System.IO.File.Exists(path))
                    return "";

                var imageBytes = System.IO.File.ReadAllBytes(path);
                var base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
