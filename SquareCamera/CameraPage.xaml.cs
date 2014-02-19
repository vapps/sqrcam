using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Devices;
using Microsoft.Xna.Framework.Media;
using System.Windows.Media.Imaging;
using System.IO;

namespace SquareCamera
{
    public partial class CameraPage : PhoneApplicationPage
    {
        private PhotoCamera _photoCamera;
        private MediaLibrary _mediaLibrary;
        public CameraPage()
        {
            InitializeComponent();
            Loaded += CameraPage_Loaded;
        }
        public void SetCamera()
        {
            _photoCamera = new PhotoCamera(CameraType.Primary);
            cameraViewFinder.SetSource(_photoCamera);
            double cameraRotation = _photoCamera.Orientation;
            previewTransform.Rotation = _photoCamera.Orientation + 0;
        }

        void CameraPage_Loaded(object sender, RoutedEventArgs e)
        {
            // 카메라 설정
            SetCamera();

            // 카메라 버튼 설정
            CameraButtons.ShutterKeyHalfPressed += CameraButtons_ShutterKeyHalfPressed;
            CameraButtons.ShutterKeyReleased += CameraButtons_ShutterKeyReleased;
            CameraButtons.ShutterKeyPressed += CameraButtons_ShutterKeyPressed;

            // 포커스
            ViewFinder.Tap += ViewFinder_Tap;

            // 저장용
            _mediaLibrary = new MediaLibrary();
        }

        void CameraButtons_ShutterKeyPressed(object sender, EventArgs e)
        {
            CaptureSquareImage();
        }

        private void CaptureSquareImage()
        {
            //_photoCamera.CaptureImage();
            WriteableBitmap bitmap = new WriteableBitmap(400, 400);
            Stream stream = new MemoryStream();

            bitmap.Render(ViewFinder, null);
            bitmap.Invalidate();

            bitmap.SaveJpeg(stream, 400, 400, 0, 100);
            stream.Seek(0, SeekOrigin.Begin);

            _mediaLibrary.SavePictureToCameraRoll("ShareCamera" + DateTime.Today.Date.ToString(), stream);
            MessageBox.Show("Saved to Camera Roll");
        }

        void CameraButtons_ShutterKeyReleased(object sender, EventArgs e)
        {
            _photoCamera.CancelFocus();
        }

        void CameraButtons_ShutterKeyHalfPressed(object sender, EventArgs e)
        {
            _photoCamera.Focus();
        }

        void ViewFinder_Tap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            _photoCamera.Focus();
        }

        void _photoCamera_AutoFocusCompleted(object sender, CameraOperationCompletedEventArgs e)
        {
        }

        void CaptureButton_Click(object sender, EventArgs e)
        {
            CaptureSquareImage();
        }   
    }
}