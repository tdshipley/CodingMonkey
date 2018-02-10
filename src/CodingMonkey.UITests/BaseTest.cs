namespace CodingMonkey.UITests
{
    using System;
    using PageObjects;
    using PageObjects.PageObjects;

    public class BaseTest : IDisposable
    {
        protected BasePageObject _basePageObject { get; }

        public BaseTest()
        {
            _basePageObject = new BasePageObject();
        }

        // *** IDisposable Logic ***
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if(_basePageObject != null)
                {
                    _basePageObject.QuitDriver();
                }

                disposedValue = true;
            }
        }

        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
        }
    }
}