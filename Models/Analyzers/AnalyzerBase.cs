using System;

namespace SteganographyGraduate.Models.Analyzers
{
    public abstract class AnalyzerBase : IAnalyzer
    { 
        protected string _imagePath;
        protected readonly UnsafeBitmap UnsafeBitmap;

        protected readonly int Rows;
        protected readonly int Columns;

        public AnalyzerBase(string imagePath)
        {
            _imagePath = imagePath;
            UnsafeBitmap = new UnsafeBitmap(imagePath);
            Rows = UnsafeBitmap.Height;
            Columns = UnsafeBitmap.Width;
        }

        public unsafe abstract bool Analyze();

        public abstract string GetResult();

        #region Dispose

        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    UnsafeBitmap.Dispose();
                }

                _disposed = true;
            }
        }

        ~AnalyzerBase() => Dispose(false);

        #endregion
    }
}
