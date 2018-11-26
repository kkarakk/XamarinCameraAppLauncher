namespace CameraAppDemo
{
    using System;
    using System.IO;
    using System.Threading.Tasks;
    using Android.Graphics;
    using Java.IO;

    public static class BitmapHelpers
    {
        public static async System.Threading.Tasks.Task<Bitmap> LoadAndResizeBitmapAsync(this string fileName, int width, int height)
        {
            // First we get the the dimensions of the file on disk,injustdecodebounds is true 
            BitmapFactory.Options options = new BitmapFactory.Options { InJustDecodeBounds = true };
            await Task.Run(async () => { await BitmapFactory.DecodeFileAsync(fileName, options); });

            // Next we calculate the ratio that we need to resize the image by
            // in order to fit the requested dimensions.
            int outHeight = options.OutHeight;
            int outWidth = options.OutWidth;
            int inSampleSize = 1;
            if (outHeight > height || outWidth > width)
            {
                inSampleSize = outWidth > outHeight
                                   ? outHeight / height
                                   : outWidth / width;
            }

            // Now we will load the image and have BitmapFactory resize it for us.
            options.InSampleSize = inSampleSize;
            options.InJustDecodeBounds = false;
            options.InPurgeable = true;

            Bitmap resizedBitmap = BitmapFactory.DecodeFile(fileName, options);
            ByteArrayOutputStream bos = new ByteArrayOutputStream();
            //await resizedBitmap.CompressAsync(Bitmap.CompressFormat.Jpeg, 75, bos);
            byte[] bitmapdata = bos.ToByteArray();

            return resizedBitmap;
        }
    }
}
