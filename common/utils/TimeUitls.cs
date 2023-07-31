using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUitls
{

    public static long getCurrentTimeSpan()
    {
        return getTimeSpan(DateTime.Now);
    }

    /// <summary>
    /// ��ȡ��ͣ����ʱ���
    /// </summary>
    /// <param name="dtime"></param>
    /// <returns></returns>
    public static long getTimeSpan(DateTime dtime)
    {
        //��������ʱ��
        //var timeSpan_Greenwich = dtime - new DateTime(1970, 1, 1, 0, 0, 0);
        //��׼����ʱ��
        var timeSpan = dtime - new DateTime(1970, 1, 1, 8, 0, 0);            //��������д������׼ʱ��
        var timeSpan_beijing = dtime - TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1, 0, 0, 0)); 
        return (long)timeSpan.TotalSeconds;
    }


    /// <summary>  
    /// ʱ���Timestampת��������  
    /// </summary>  
    /// <param name="timeStamp"></param>  
    /// <returns></returns>  
    private static DateTime GetDateTime(long timeStamp)
    {

        DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
        long lTime = ((long)timeStamp * 10000000);
        TimeSpan toNow = new TimeSpan(lTime);
        DateTime targetDt = dtStart.Add(toNow);

        if (string.IsNullOrEmpty(targetDt.ToString()))
        {
            targetDt = DateTime.Now;
        }

        return targetDt;
    }

    /// <summary>
    /// 13λʱ���ת ���ڸ�ʽ   1652338858000 -> 2022-05-12 03:00:58
    /// </summary>
    /// <param name="timestamp"></param>
    /// <returns></returns>
    public static DateTime GetDateTimeMilliseconds(long timestamp)
    {
        long begtime = timestamp * 10000000;
        DateTime dt_1970 = new DateTime(1970, 1, 1, 8, 0, 0);
        long tricks_1970 = dt_1970.Ticks;//1970��1��1�տ̶�
        long time_tricks = tricks_1970 + begtime;//��־���ڿ̶�
        DateTime dt = new DateTime(time_tricks);//ת��ΪDateTime

        return dt;
    }
}
