using NITGEN.SDK.NBioBSP;
using System;
using System.Collections.Generic;
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
            NBioAPI.Type.INIT_INFO_0 initInfo0;
            uint ret = m_NBioAPI.GetInitInfo(out initInfo0);
            if (ret == NBioAPI.Error.NONE)
            {
                initInfo0.EnrollImageQuality = Convert.ToUInt32(50);
                initInfo0.VerifyImageQuality = Convert.ToUInt32(30);
                initInfo0.DefaultTimeout = Convert.ToUInt32(10000);
                initInfo0.SecurityLevel = (int)NBioAPI.Type.FIR_SECURITY_LEVEL.NORMAL - 1;
            }

            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);
            NBioAPI.Type.HFIR hCapturedFIR;
            NBioAPI.Type.FIR_TEXTENCODE texto;
            // Get FIR data
            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            m_NBioAPI.Capture(out hCapturedFIR);

            try
            {
                if (hCapturedFIR != null)
                {
                    m_NBioAPI.GetTextFIRFromHandle(hCapturedFIR, out texto, true);
                    return texto.TextFIR;
                }
                return null;
            }
            catch (Exception ex)
            {

                throw new Exception("ERRO:... " + ex.Message);
            }
        }

        public string Enroll()
        {

            NBioAPI m_NBioAPI = new NBioAPI();
            NBioAPI.Type.FIR_TEXTENCODE m_textFIR;
            NBioAPI.Type.HFIR NewFIR;
            NBioAPI.IndexSearch m_IndexSearch = new NBioAPI.IndexSearch(m_NBioAPI);


            NBioAPI.Type.WINDOW_OPTION m_WinOption = new NBioAPI.Type.WINDOW_OPTION();
            m_WinOption.WindowStyle = (uint)NBioAPI.Type.WINDOW_STYLE.NO_WELCOME;

            string Retorno = "";

            m_NBioAPI.OpenDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            uint ret = m_NBioAPI.Enroll(out NewFIR, null);

            //uint ret = m_NBioAPI.Enroll(null, out NewFIR, null, NBioAPI.Type.TIMEOUT.DEFAULT, null, m_WinOption);


            if (ret != NBioAPI.Error.NONE)
            {
                m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
            }

            if (NewFIR != null)
            {
                m_NBioAPI.GetTextFIRFromHandle(NewFIR, out m_textFIR, true);


                if (m_textFIR.TextFIR != null)
                {
                    m_NBioAPI.CloseDevice(NBioAPI.Type.DEVICE_ID.AUTO);
                    Retorno = m_textFIR.TextFIR.ToString();
                }
            }
            return Retorno;
        }
    }
}
