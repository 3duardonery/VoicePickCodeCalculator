using OperationResult;
using System;
using System.Text;

namespace VoicePickCodeCalculator.Main
{
    public static class Calculator
    {
        public static Result<string> GetVoicePickCode(string gtin, string lote, string dataEmbalagem = null)
        {
            if (string.IsNullOrEmpty(gtin) || string.IsNullOrEmpty(lote))
                return Result.Error<string>(new Exception("Properties cannot be null or empty"));

            var crc = string.IsNullOrEmpty(dataEmbalagem) ?
                Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(
                     string.Format("{0}{1}", gtin, lote))) :
                Crc16.ComputeChecksum(Encoding.ASCII.GetBytes(
                     string.Format("{0}{1}{2}", gtin, lote, dataEmbalagem)));

            if (!crc.IsSuccess)
                return Result.Error<string>(crc.Exception);

            var resultVoicePick = string.Format("{0:0000}", crc.Value % 10000);

            return Result.Success(resultVoicePick);
        }
    }
}
