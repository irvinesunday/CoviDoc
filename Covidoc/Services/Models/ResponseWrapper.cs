using System.Net;

namespace CoviDoc.Services.Models
{
    public class ResponseWrapper
    {
        public ResponseWrapper(AtResponse atResponse)
        {
            AtResponse = atResponse;
        }
        public ResponseWrapper(string errorMessage, HttpStatusCode httpStatus, string resultReasonPhrase)
        {
            ErrorMessage = errorMessage;
            HttpStatus = httpStatus;
        }
        public AtResponse AtResponse { get; }
        public string ErrorMessage { get; }
        public HttpStatusCode HttpStatus { get; }
    }
}