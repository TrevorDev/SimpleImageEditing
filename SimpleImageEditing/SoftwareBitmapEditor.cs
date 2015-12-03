using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Graphics.Imaging;

namespace SimpleImageEditing
{
    public sealed class SoftwareBitmapPixel
    {
        public byte r { get; set; }
        public byte g { get; set; }
        public byte b { get; set; }
    }
    public sealed class SoftwareBitmapEditor : IDisposable
    {
        [ComImport]
        [Guid("5b0d3235-4dba-4d44-865e-8f1d0e4fd04d")]
        [InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        unsafe interface IMemoryBufferByteAccess
        {
            void GetBuffer(out byte* buffer, out uint capacity);
        }

        private unsafe byte* pixels;
        public int width { get; set; }
        public int height{ get; set; }
        private BitmapPlaneDescription desc;
        private const int BYTES_PER_PIXEL = 4;
        private BitmapBuffer inputBuffer;
        private Windows.Foundation.IMemoryBufferReference inputReference;
        public SoftwareBitmapEditor(SoftwareBitmap input)
        {
            unsafe
            {
                if(input.BitmapPixelFormat != BitmapPixelFormat.Bgra8)
                {
                    throw new System.ArgumentException("BitmapPixelFormat of softwarebitmap must be Bgra8. See SoftwareBitmap.Convert to convert.");
                }
                uint inputCapacity;
                inputBuffer = input.LockBuffer(BitmapBufferAccessMode.ReadWrite);
                inputReference = inputBuffer.CreateReference();
                ((IMemoryBufferByteAccess)inputReference).GetBuffer(out pixels, out inputCapacity);
                desc = inputBuffer.GetPlaneDescription(0);
                this.width = desc.Width;
                this.height = desc.Height;
            }
        }

        public SoftwareBitmapPixel getPixel(uint posX, uint posY)
        {
            var inputCurrPixel = desc.StartIndex + desc.Stride * posY + BYTES_PER_PIXEL * posX;
            SoftwareBitmapPixel ret = new SoftwareBitmapPixel();
            unsafe
            {
                ret.r = pixels[inputCurrPixel + 2];
                ret.b = pixels[inputCurrPixel + 1];
                ret.g = pixels[inputCurrPixel + 0];
            }
            return ret;
        }

        public void setPixel(uint posX, uint posY, Byte r, Byte g, Byte b)
        {
            unsafe
            {
                var inputCurrPixel = desc.StartIndex + desc.Stride * posY + BYTES_PER_PIXEL * posX;
                pixels[inputCurrPixel + 0] = b; // Blue
                pixels[inputCurrPixel + 1] = g; // Green
                pixels[inputCurrPixel + 2] = r; // Red
            }

        }

        public void Dispose()
        {
            inputBuffer.Dispose();
            inputReference.Dispose();
        }
    }
}
