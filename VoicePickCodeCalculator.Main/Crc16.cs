using OperationResult;
using System;

namespace VoicePickCodeCalculator.Main
{
    internal static class Crc16
    {
        private static ushort[] table = new ushort[256];
        private const ushort polynominal = 0xA001;

        static Crc16()
        {
            ushort value;
            ushort temp;
            for (ushort i = 0; i < table.Length; i++)
            {
                value = 0;
                temp = i;
                for (byte j = 0; j < 8; ++j)
                {
                    if (0 != ((value ^ temp) & 0x0001))
                    {
                        value = (ushort)((value >> 1) ^ polynominal);
                    }
                    else
                    {
                        value >>= 1;
                    }
                    temp >>= 1;
                }
                table[i] = value;
            }
        }

        /// <summary>
        /// Method responsable to calculate the voice pick code
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Result<ushort> ComputeChecksum(byte[] bytes)
        {
            try
            {
                ushort crc = 0;
                for (int i = 0; i < bytes.Length; i++)
                {
                    byte index = (byte)(crc ^ bytes[i]);
                    crc = (ushort)((crc >> 8) ^ table[index]);

                }
                return Result.Success(crc);
            }
            catch (Exception exc)
            {
                return Result.Error<ushort>(exc);
            }
        }

    }
}
