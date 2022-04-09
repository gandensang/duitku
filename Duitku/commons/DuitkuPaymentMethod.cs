using System;
using System.Collections.Generic;
using System.Text;

namespace Duitku
{
    public enum DuitkuPaymentMethod
    {
        VC, // Credit Card
        BC, // BCA Virtual Account
        M2, // Mandiri Virtual Account
        VA, // Maybank Virtual Account
        I1, // BNI Virtual Account
        B1, // BNI Virtual Account
        BT, // Permata Bank Virtual Account
        A1, // ATM Bersama
        AG, // Bank Artha Graha
        NC, // Bank Neo Commerce/BNC
        BR, // BRIVA
        S1, // Bank Sahabat Sampoerna
        FT, // Pegadaian/ALFA/Pos
        IR, // Indomaret
        OV, // OVO
        SA, // Shopee Pay Apps
        LF, // LinkAja Apps fixed fee
        LA, // LinkAja Apps (Percentage Fee)
        DA, // DANA
        SP, // Shopee Pay
        LQ, // LinkAja
        NQ, // Nobu
        DN  // Indodana
    }
}
