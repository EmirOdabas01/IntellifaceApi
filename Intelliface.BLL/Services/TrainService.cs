using Emgu.CV;
using Emgu.CV.Face;
using Emgu.CV.Structure;
using Intelliface.BLL.Interfaces;
using Intelliface.DAL.Interfaces;
using Intelliface.DAL.Repositories;
using Intelliface.Entities.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Intelliface.BLL.Services
{
    public class TrainService : ITrainService
    {
        private readonly IRepository<EmployeeTrainImage> _trainImageRepository;
        private readonly IRepository<Employee> _employeeRepository;

        public TrainService(IRepository<EmployeeTrainImage> trainImageRepository, IRepository<Employee> employeeRepository)
        {
            _trainImageRepository = trainImageRepository;
            _employeeRepository = employeeRepository;
        }

        public async Task TrainModelAsync(int employeeId)
        {
            var employee = await _employeeRepository.GetByIdAsync(employeeId);
            if (employee == null)
                throw new Exception("Employee not found");

            var images = await _trainImageRepository.GetAllAsync(emp => emp.EmployeeId == employeeId);
            if (!images.Any())
                throw new Exception("No training images found");

            var trainingImages = new List<Mat>();
            var labels = new List<int>();

            var cascadePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "haarcascade_frontalface_default.xml");
            if (!File.Exists(cascadePath))
                throw new Exception("Haar cascade file not found.");

            var faceCascade = new CascadeClassifier(cascadePath);

            foreach (var image in images)
            {
                using var ms = new MemoryStream(image.ImageData);
                using var bitmap = new Bitmap(ms);
                using var mat = BitmapToMat(bitmap);
                var bgrImage = mat.ToImage<Bgr, byte>();
                var grayImage = bgrImage.Convert<Gray, byte>();
                 

                var faces = faceCascade.DetectMultiScale(
                    grayImage,
                    scaleFactor: 1.1,
                    minNeighbors: 5,
                    minSize: new Size(50, 50));

                foreach (var faceRect in faces)
                {
                    var face = grayImage.Copy(faceRect)
                                         .Resize(100, 100, Emgu.CV.CvEnum.Inter.Linear);

                    trainingImages.Add(face.Mat);
                    labels.Add(0);  
                }
            }

            if (!trainingImages.Any())
                throw new Exception("No faces detected in any image.");

            var recognizer = new EigenFaceRecognizer();
            recognizer.Train(trainingImages.ToArray(), labels.ToArray());

            string tempPath = Path.GetTempFileName();
            recognizer.Write(tempPath);
            byte[] modelData = await File.ReadAllBytesAsync(tempPath);
            File.Delete(tempPath);

            employee.FaceRecognitionModel = modelData;
            _employeeRepository.Update(employee);
            await _employeeRepository.SaveAsync();
        }
        private static Mat BitmapToMat(Bitmap bitmap)
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
}
