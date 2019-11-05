using NITGEN.SDK.NBioBSP;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TsService
{
    class utilsNitgen
    {


        public string Capturar()
        {
            NBioAPI m_NBioAPI = new NBioAPI();

            NBioAPI.Type.HFIR hCapturedFIR = new NBioAPI.Type.HFIR();
            NBioAPI.Type.FIR_TEXTENCODE texto = new NBioAPI.Type.FIR_TEXTENCODE();
            // Get FIR data
          
               m_NBioAPI.OpenDevice(255);          

               m_NBioAPI.Capture(out hCapturedFIR);
               m_NBioAPI.GetTextFIRFromHandle(hCapturedFIR, out texto, true);


            return texto.TextFIR;
         
             
            
          
        }

       
    }
}
