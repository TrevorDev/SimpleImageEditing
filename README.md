# SimpleImageEditing
Edit SoftwareBitmap objects by pixel without operating on bytes or compiling with unsafe flag

### Installing Nuget Package and Dependencies
- This requires your project to be a Windows 10 Universal App project
- Right click project references -> Manage Nuget Packages
- Search and install SimpleImageEditing nuget package

### Supported image types
Currently only supports BGRA8 Software Bitmap objects.
To convert to supported format:
```
SoftwareBitmap supported = SoftwareBitmap.Convert(toConvert, BitmapPixelFormat.Bgra8, BitmapAlphaMode.Premultiplied);
```

### Basic Example (Turns image green)
```
using (SoftwareBitmapEditor editor = new SoftwareBitmapEditor(input))
{
    for (uint y = 0; y < editor.height; y++)
    {
        for (uint x = 0; x < editor.width; x++)
        {
            SoftwareBitmapPixel pixel = editor.getPixel(x, y);
            editor.setPixel(x, y, pixel.r, (byte)Math.Min(pixel.g + 100, 255), pixel.b);
        }
    }
}
```

### Useful links
- [MediaCapture How to's](https://msdn.microsoft.com/en-us/library/windows/apps/mt244352.aspx)
- [MediaCapture API](https://msdn.microsoft.com/library/windows/apps/br241124)
