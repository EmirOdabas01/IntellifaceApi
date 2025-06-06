﻿using Emgu.CV;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Intelliface.BLL.Interfaces
{
    public interface IRecognitionService
    {
        Task<bool> RecognizeAsync(int employeeId, byte[] imageBytes);
        Mat BitmapToMat(Bitmap bitmap);
    }
}
