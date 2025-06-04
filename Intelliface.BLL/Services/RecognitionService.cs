using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.DAL.Repositories;
using Intelliface.Entities.Models;
using System.Drawing;
using System.IO;

public class RecognitionService : IRecognitionService
{
    private readonly IRepository<Employee> _employeeRepository;

    public RecognitionService(IRepository<Employee> employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public async Task<bool> RecognizeAsync(int employeeId, byte[] imageBytes)
    {
        var employee = await _employeeRepository.GetByIdAsync(employeeId);
        if (employee == null || employee.FaceRecognitionModel == null)
            throw new Exception("Employee or trained model not found.");

        var model = new EigenFaceRecognizer();
        string tempModelPath = Path.GetTempFileName();
        await File.WriteAllBytesAsync(tempModelPath, employee.FaceRecognitionModel);
        model.Read(tempModelPath);
        File.Delete(tempModelPath);

        using var ms = new MemoryStream(imageBytes);
        using var bitmap = new Bitmap(ms);
        var mat = BitmapToMat(bitmap);
        var image = mat.ToImage<Bgr, byte>();
        var grayImage = image.Convert<Gray, byte>();

        var cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Resources", "haarcascade_frontalface_default.xml");
        var faceCascade = new CascadeClassifier(cascadePath);
        var faces = faceCascade.DetectMultiScale(grayImage, 1.1, 5, new Size(50, 50));

        foreach (var faceRect in faces)
        {
            var faceImage = grayImage.Copy(faceRect)
                                      .Resize(100, 100, Emgu.CV.CvEnum.Inter.Linear);

            var result = model.Predict(faceImage.Mat);
            int predictedLabel = result.Label;
            double distance = result.Distance;

            if (predictedLabel == 0 && distance < 3000)  
                return true;
        }

        return false;
    }
   public Mat BitmapToMat(Bitmap bitmap)
    {
        var mat = new Mat();
        using (var stream = new MemoryStream())
        {
            bitmap.Save(stream, System.Drawing.Imaging.ImageFormat.Bmp);
            stream.Position = 0;
            CvInvoke.Imdecode(stream.ToArray(), Emgu.CV.CvEnum.ImreadModes.Color, mat);
        }
        return mat;
    }

}
