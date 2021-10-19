
using UnityEngine;


    public class NFC_WriteDebug : NFC_Interface
    {
      
        
        public override void Proceed(string [] data)
        {
            //example
            string nftText = data[1].Substring(3);
            Debug.Log(nftText);
           
        }
    }
