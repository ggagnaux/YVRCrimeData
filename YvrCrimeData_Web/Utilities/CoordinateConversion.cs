///*
// * Author: Sami Salkosuo, sami.salkosuo@fi.ibm.com
// *
// * (c) Copyright IBM Corp. 2007
// */
//package com.ibm.util;

//import java.util.Hashtable;
//import java.util.Map;

//public class CoordinateConversion
//{


//}



using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace YvrCrimeData_Web.Utilities
{
    public class CoordinateConversion
    {
        private const double MinimumLatitude = -90.0D;
        private const double MaximumLatitude = 90.0D;
        private const double MinimumLongitude = -180.0D;
        private const double MaximumLongitude = 180.0D;


        public CoordinateConversion()
        {
        }

        public double[] UTM2LatLon(String UTM)
        {
            UTM2LatLon c = new UTM2LatLon();
            return c.convertUTMToLatLong(UTM);
        }

        public String LatLon2UTM(double latitude, double longitude)
        {
            LatLon2UTM c = new LatLon2UTM();
            return c.convertLatLonToUTM(latitude, longitude);

        }

        public static void Validate(double latitude, double longitude)
        {
            if (latitude < MinimumLatitude || latitude > MaximumLatitude || 
                longitude < MinimumLongitude || longitude >= MaximumLongitude)
            {
                throw new ArgumentException($"Latitude and/or Longitude out of range. " +
                                            $"Legal ranges: latitude [{MinimumLatitude},{MaximumLatitude}], " +
                                            $"longitude [{MinimumLongitude},{MaximumLongitude}].");
            }

        }

        public String latLon2MGRUTM(double latitude, double longitude)
        {
            LatLon2MGRUTM c = new LatLon2MGRUTM();
            return c.convertLatLonToMGRUTM(latitude, longitude);

        }

        public double[] mgrutm2LatLon(String MGRUTM)
        {
            MGRUTM2LatLon c = new MGRUTM2LatLon();
            return c.convertMGRUTMToLatLong(MGRUTM);
        }

        public static double DegreeToRadian(double degree)
        {
            return degree * Math.PI / 180;
        }

        public static double radianToDegree(double radian)
        {
            return radian * 180 / Math.PI;
        }

        public static double POW(double a, double b)
        {
            return Math.Pow(a, b);
        }

        public static double SIN(double value)
        {
            return Math.Sin(value);
        }

        public static double COS(double value)
        {
            return Math.Cos(value);
        }

        public static double TAN(double value)
        {
            return Math.Tan(value);
        }
    }

    public class LatLon2MGRUTM : LatLon2UTM
    {
        public String convertLatLonToMGRUTM(double latitude, double longitude)
        {
            CoordinateConversion.Validate(latitude, longitude);

            String mgrUTM = string.Empty;

            setVariables(latitude, longitude);

            String longZone = getLongZone(longitude);
            LatZones latZones = new LatZones();
            String latZone = latZones.getLatZone(latitude);

            double _easting = getEasting();
            double _northing = getNorthing(latitude);
            Digraphs digraphs = new Digraphs();
            String digraph1 = digraphs.getDigraph1(Convert.ToInt32(longZone), _easting);
            String digraph2 = digraphs.getDigraph2(Convert.ToInt32(longZone), _northing);

            String easting = Convert.ToString((int)_easting); //String.valueOf((int)_easting);
            if (easting.Length < 5)
            {
                easting = "00000" + easting;
            }

            easting = easting.Substring(easting.Length - 5);

            String northing;
            northing = Convert.ToString((int)_northing);
            if (northing.Length < 5)
            {
                northing = "0000" + northing;
            }
            northing = northing.Substring(northing.Length - 5);

            mgrUTM = longZone + latZone + digraph1 + digraph2 + easting + northing;
            return mgrUTM;
        }
    }

    public class LatLon2UTM
    {
        public String convertLatLonToUTM(double latitude, double longitude)
        {
            CoordinateConversion.Validate(latitude, longitude);
            String UTM = "";

            setVariables(latitude, longitude);

            String longZone = getLongZone(longitude);
            LatZones latZones = new LatZones();
            String latZone = latZones.getLatZone(latitude);

            double _easting = getEasting();
            double _northing = getNorthing(latitude);

            UTM = longZone + " " + latZone + " " + ((int)_easting) + " " + ((int)_northing);
            // UTM = longZone + " " + latZone + " " + decimalFormat.format(_easting) +
            // " "+ decimalFormat.format(_northing);

            return UTM;

        }

        protected void setVariables(double latitude, double longitude)
        {
            latitude = CoordinateConversion.DegreeToRadian(latitude);
            rho = equatorialRadius * (1 - e * e)
                / CoordinateConversion.POW(1 - CoordinateConversion.POW(e * CoordinateConversion.SIN(latitude), 2), 3 / 2.0);

            nu = equatorialRadius / CoordinateConversion.POW(1 - CoordinateConversion.POW(e * CoordinateConversion.SIN(latitude), 2), (1 / 2.0));

            double var1;
            if (longitude < 0.0)
            {
                var1 = ((int)((180 + longitude) / 6.0)) + 1;
            }
            else
            {
                var1 = ((int)(longitude / 6)) + 31;
            }
            double var2 = (6 * var1) - 183;
            double var3 = longitude - var2;
            p = var3 * 3600 / 10000;

            S = A0 * latitude - B0 * CoordinateConversion.SIN(2 * latitude) + C0 * CoordinateConversion.SIN(4 * latitude) - D0
                * CoordinateConversion.SIN(6 * latitude) + E0 * CoordinateConversion.SIN(8 * latitude);

            K1 = S * k0;
            K2 = nu * CoordinateConversion.SIN(latitude) * CoordinateConversion.COS(latitude) * CoordinateConversion.POW(sin1, 2) * k0 * (100000000)
                / 2;
            K3 = ((CoordinateConversion.POW(sin1, 4) * nu * CoordinateConversion.SIN(latitude) * Math.Pow(CoordinateConversion.COS(latitude), 3)) / 24)
                * (5 - CoordinateConversion.POW(CoordinateConversion.TAN(latitude), 2) + 9 * e1sq * CoordinateConversion.POW(CoordinateConversion.COS(latitude), 2) + 4
                    * CoordinateConversion.POW(e1sq, 2) * CoordinateConversion.POW(CoordinateConversion.COS(latitude), 4))
                * k0
                * (10000000000000000L);

            K4 = nu * CoordinateConversion.COS(latitude) * sin1 * k0 * 10000;

            K5 = CoordinateConversion.POW(sin1 * CoordinateConversion.COS(latitude), 3) * (nu / 6)
                * (1 - CoordinateConversion.POW(CoordinateConversion.TAN(latitude), 2) + e1sq * CoordinateConversion.POW(CoordinateConversion.COS(latitude), 2)) * k0
                * 1000000000000L;

            A6 = (CoordinateConversion.POW(p * sin1, 6) * nu * CoordinateConversion.SIN(latitude) * CoordinateConversion.POW(CoordinateConversion.COS(latitude), 5) / 720)
                * (61 - 58 * CoordinateConversion.POW(CoordinateConversion.TAN(latitude), 2) + CoordinateConversion.POW(CoordinateConversion.TAN(latitude), 4) + 270
                    * e1sq * CoordinateConversion.POW(CoordinateConversion.COS(latitude), 2) - 330 * e1sq
                    * CoordinateConversion.POW(CoordinateConversion.SIN(latitude), 2)) * k0 * (1E+24);

        }

        protected String getLongZone(double longitude)
        {
            double longZone = 0;
            if (longitude < 0.0)
            {
                longZone = ((180.0 + longitude) / 6) + 1;
            }
            else
            {
                longZone = (longitude / 6) + 31;
            }

            //String val = String.valueOf((int)longZone);
            string val = Convert.ToString((int)longZone);

            if (val.Length == 1)
            {
                val = "0" + val;
            }
            return val;
        }

        protected double getNorthing(double latitude)
        {
            double northing = K1 + K2 * p * p + K3 * CoordinateConversion.POW(p, 4);
            if (latitude < 0.0)
            {
                northing = 10000000 + northing;
            }
            return northing;
        }

        protected double getEasting()
        {
            return 500000 + (K4 * p + K5 * CoordinateConversion.POW(p, 3));
        }

        // Lat Lon to UTM variables

        // equatorial radius
        public const double equatorialRadius = 6378137;

        // polar radius
        public const double polarRadius = 6356752.314;

        // flattening
        double flattening = 0.00335281066474748;// (equatorialRadius-polarRadius)/equatorialRadius;

        // inverse flattening 1/flattening
        double inverseFlattening = 298.257223563;// 1/flattening;

        // Mean radius
        double rm = CoordinateConversion.POW(equatorialRadius * polarRadius, 1 / 2.0);

        // scale factor
        double k0 = 0.9996;

        // eccentricity
        public double e = Math.Sqrt(1 - CoordinateConversion.POW(polarRadius / equatorialRadius, 2));

        private double e1sq
        {
            get
            {
                return e * e / (1 - e * e);
            }
        }

        //double e1sq = e * e / (1 - e * e);

        double n = (equatorialRadius - polarRadius)
            / (equatorialRadius + polarRadius);

        // r curv 1
        double rho = 6368573.744;

        // r curv 2
        double nu = 6389236.914;

        // Calculate Meridional Arc Length
        // Meridional Arc
        double S = 5103266.421;

        double A0 = 6367449.146;

        double B0 = 16038.42955;

        double C0 = 16.83261333;

        double D0 = 0.021984404;

        double E0 = 0.000312705;

        // Calculation Constants
        // Delta Long
        double p = -0.483084;

        double sin1 = 4.84814E-06;

        // Coefficients for UTM Coordinates
        double K1 = 5101225.115;

        double K2 = 3750.291596;

        double K3 = 1.397608151;

        double K4 = 214839.3105;

        double K5 = -2.995382942;

        double A6 = -1.00541E-07;
    }

    public class MGRUTM2LatLon : UTM2LatLon
    {
        public double[] convertMGRUTMToLatLong(String mgrutm)
        {
            double[] latlon = { 0.0, 0.0 };
            // 02CNR0634657742
            int zone = Convert.ToInt32(mgrutm.Substring(0, 2)); //Integer.parseInt(mgrutm.substring(0, 2));
            String latZone = mgrutm.Substring(2, 3);

            String digraph1 = mgrutm.Substring(3, 4);
            String digraph2 = mgrutm.Substring(4, 5);
            easting = Convert.ToDouble(mgrutm.Substring(5, 10)); // Double.parseDouble(mgrutm.Substring(5, 10));
            northing = Convert.ToDouble(mgrutm.Substring(10, 15));// Double.parseDouble(mgrutm.Substring(10, 15));

            LatZones lz = new LatZones();
            double latZoneDegree = lz.getLatZoneDegree(latZone);

            double a1 = latZoneDegree * 40000000 / 360.0;
            double a2 = 2000000 * Math.Floor(a1 / 2000000.0);

            Digraphs digraphs = new Digraphs();

            double digraph2Index = digraphs.getDigraph2Index(digraph2);

            double startindexEquator = 1;
            if ((1 + zone % 2) == 1)
            {
                startindexEquator = 6;
            }

            double a3 = a2 + (digraph2Index - startindexEquator) * 100000;
            if (a3 <= 0)
            {
                a3 = 10000000 + a3;
            }
            northing = a3 + northing;

            zoneCM = -183 + 6 * zone;
            double digraph1Index = digraphs.getDigraph1Index(digraph1);
            int a5 = 1 + zone % 3;
            double[] a6 = { 16, 0, 8 };
            double a7 = 100000 * (digraph1Index - a6[a5 - 1]);
            easting = easting + a7;

            setVariables();

            double latitude = 0;
            latitude = 180 * (phi1 - fact1 * (fact2 + fact3 + fact4)) / Math.PI;

            if (latZoneDegree < 0)
            {
                latitude = 90 - latitude;
            }

            double d = _a2 * 180 / Math.PI;
            double longitude = zoneCM - d;

            if (getHemisphere(latZone).Equals("S"))
            {
                latitude = -latitude;
            }

            latlon[0] = latitude;
            latlon[1] = longitude;
            return latlon;
        }
    }

    public class UTM2LatLon
    {
        public double easting;

        public double northing;

        int zone;

        String southernHemisphere = "ACDEFGHJKLM";

        protected String getHemisphere(String latZone)
        {
            String hemisphere = "N";
            if (southernHemisphere.IndexOf(latZone) > -1)
            {
                hemisphere = "S";
            }
            return hemisphere;
        }

        public double[] convertUTMToLatLong(String UTM)
        {
            double[] latlon = { 0.0, 0.0 };
            String[] utm = UTM.Split(' ');
            zone = Convert.ToInt32(utm[0]);// Integer.parseInt(utm[0]);
            String latZone = utm[1];
            easting = Convert.ToDouble(utm[2]);// Double.parseDouble(utm[2]);
            northing = Convert.ToDouble(utm[3]);// Double.parseDouble(utm[3]);
            String hemisphere = getHemisphere(latZone);
            double latitude = 0.0;
            double longitude = 0.0;

            if (hemisphere.Equals("S"))
            {
                northing = 10000000 - northing;
            }
            setVariables();
            latitude = 180 * (phi1 - fact1 * (fact2 + fact3 + fact4)) / Math.PI;

            if (zone > 0)
            {
                zoneCM = 6 * zone - 183.0;
            }
            else
            {
                zoneCM = 3.0;

            }

            longitude = zoneCM - _a3;
            if (hemisphere.Equals("S"))
            {
                latitude = -latitude;
            }

            latlon[0] = latitude;
            latlon[1] = longitude;
            return latlon;

        }

        protected void setVariables()
        {
            arc = northing / k0;
            mu = arc
                / (a * (1 - CoordinateConversion.POW(e, 2) / 4.0 - 3 * CoordinateConversion.POW(e, 4) / 64.0 - 5 * CoordinateConversion.POW(e, 6) / 256.0));

            ei = (1 - CoordinateConversion.POW((1 - e * e), (1 / 2.0)))
                / (1 + CoordinateConversion.POW((1 - e * e), (1 / 2.0)));

            ca = 3 * ei / 2 - 27 * CoordinateConversion.POW(ei, 3) / 32.0;

            cb = 21 * CoordinateConversion.POW(ei, 2) / 16 - 55 * CoordinateConversion.POW(ei, 4) / 32;
            cc = 151 * CoordinateConversion.POW(ei, 3) / 96;
            cd = 1097 * CoordinateConversion.POW(ei, 4) / 512;
            phi1 = mu + ca * CoordinateConversion.SIN(2 * mu) + cb * CoordinateConversion.SIN(4 * mu) + cc * CoordinateConversion.SIN(6 * mu) + cd
                * CoordinateConversion.SIN(8 * mu);

            n0 = a / CoordinateConversion.POW((1 - CoordinateConversion.POW((e * CoordinateConversion.SIN(phi1)), 2)), (1 / 2.0));

            r0 = a * (1 - e * e) / CoordinateConversion.POW((1 - CoordinateConversion.POW((e * CoordinateConversion.SIN(phi1)), 2)), (3 / 2.0));
            fact1 = n0 * CoordinateConversion.TAN(phi1) / r0;

            _a1 = 500000 - easting;
            dd0 = _a1 / (n0 * k0);
            fact2 = dd0 * dd0 / 2;

            t0 = CoordinateConversion.POW(CoordinateConversion.TAN(phi1), 2);
            Q0 = e1sq * CoordinateConversion.POW(CoordinateConversion.COS(phi1), 2);
            fact3 = (5 + 3 * t0 + 10 * Q0 - 4 * Q0 * Q0 - 9 * e1sq) * CoordinateConversion.POW(dd0, 4)
                / 24;

            fact4 = (61 + 90 * t0 + 298 * Q0 + 45 * t0 * t0 - 252 * e1sq - 3 * Q0
                * Q0)
                * CoordinateConversion.POW(dd0, 6) / 720;

            //
            lof1 = _a1 / (n0 * k0);
            lof2 = (1 + 2 * t0 + Q0) * CoordinateConversion.POW(dd0, 3) / 6.0;
            lof3 = (5 - 2 * Q0 + 28 * t0 - 3 * CoordinateConversion.POW(Q0, 2) + 8 * e1sq + 24 * CoordinateConversion.POW(t0, 2))
                * CoordinateConversion.POW(dd0, 5) / 120;
            _a2 = (lof1 - lof2 + lof3) / CoordinateConversion.COS(phi1);
            _a3 = _a2 * 180 / Math.PI;

        }

        double arc;

        double mu;

        double ei;

        double ca;

        double cb;

        double cc;

        double cd;

        double n0;

        double r0;

        double _a1;

        double dd0;

        double t0;

        double Q0;

        double lof1;

        double lof2;

        double lof3;

        public double _a2;

        public double phi1;

        public double fact1;

        public double fact2;

        public double fact3;

        public double fact4;

        public double zoneCM;

        double _a3;

        double b = 6356752.314;

        double a = 6378137;

        double e = 0.081819191;

        double e1sq = 0.006739497;

        double k0 = 0.9996;
    }

    public class Digraphs
    {
        private Hashtable digraph1 = new Hashtable();

        private IDictionary digraph2 = new Hashtable();

        private String[] LetterArray = { "A", "B", "C", "D", "E", "F", "G", "H",
        "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z" };

        private String[] digraph1Array = { "A", "B", "C", "D", "E", "F", "G", "H",
        "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V", "W", "X",
        "Y", "Z" };

        private String[] digraph2Array = { "V", "A", "B", "C", "D", "E", "F", "G",
        "H", "J", "K", "L", "M", "N", "P", "Q", "R", "S", "T", "U", "V" };

        public Digraphs()
        {
            for (var x=1; x<=LetterArray.Length; x++)
            {
                digraph1.Add(x, LetterArray[x-1]);
            }
            //digraph1.Add(1, "A");
            //digraph1.Add(2, "B");
            //digraph1.Add(3, "C");
            //digraph1.Add(4, "D");
            //digraph1.Add(5, "E");
            //digraph1.Add(6, "F");
            //digraph1.Add(7, "G");
            //digraph1.Add(8, "H");
            //digraph1.Add(9, "J");
            //digraph1.Add(10, "K");
            //digraph1.Add(11, "L");
            //digraph1.Add(12, "M");
            //digraph1.Add(13, "N");
            //digraph1.Add(14, "P");
            //digraph1.Add(15, "Q");
            //digraph1.Add(16, "R");
            //digraph1.Add(17, "S");
            //digraph1.Add(18, "T");
            //digraph1.Add(19, "U");
            //digraph1.Add(20, "V");
            //digraph1.Add(21, "W");
            //digraph1.Add(22, "X");
            //digraph1.Add(23, "Y");
            //digraph1.Add(24, "Z");

            for (var x = 0; x < digraph2Array.Length; x++)
            {
                digraph2.Add(x, digraph2Array[x]);
            }

            //digraph2.put(new Integer(0), "V");
            //digraph2.put(new Integer(1), "A");
            //digraph2.put(new Integer(2), "B");
            //digraph2.put(new Integer(3), "C");
            //digraph2.put(new Integer(4), "D");
            //digraph2.put(new Integer(5), "E");
            //digraph2.put(new Integer(6), "F");
            //digraph2.put(new Integer(7), "G");
            //digraph2.put(new Integer(8), "H");
            //digraph2.put(new Integer(9), "J");
            //digraph2.put(new Integer(10), "K");
            //digraph2.put(new Integer(11), "L");
            //digraph2.put(new Integer(12), "M");
            //digraph2.put(new Integer(13), "N");
            //digraph2.put(new Integer(14), "P");
            //digraph2.put(new Integer(15), "Q");
            //digraph2.put(new Integer(16), "R");
            //digraph2.put(new Integer(17), "S");
            //digraph2.put(new Integer(18), "T");
            //digraph2.put(new Integer(19), "U");
            //digraph2.put(new Integer(20), "V");
        }

        public int getDigraph1Index(String letter)
        {
            for (int i = 0; i < digraph1Array.Length; i++)
            {
                if (digraph1Array[i].Equals(letter))
                {
                    return i + 1;
                }
            }

            return -1;
        }

        public int getDigraph2Index(String letter)
        {
            for (int i = 0; i < digraph2Array.Length; i++)
            {
                if (digraph2Array[i].Equals(letter))
                {
                    return i;
                }
            }

            return -1;
        }

        public String getDigraph1(int longZone, double easting)
        {
            int a1 = longZone;
            double a2 = 8 * ((a1 - 1) % 3) + 1;

            double a3 = easting;
            double a4 = a2 + ((int)(a3 / 100000)) - 1;
            //return (String)digraph1.get(new Integer((int)Math.Floor(a4)));
            return (String)digraph1[Math.Floor(a4)];

        }

        public String getDigraph2(int longZone, double northing)
        {
            int a1 = longZone;
            double a2 = 1 + 5 * ((a1 - 1) % 2);
            double a3 = northing;
            double a4 = (a2 + ((int)(a3 / 100000)));
            a4 = (a2 + ((int)(a3 / 100000.0))) % 20;
            a4 = Math.Floor(a4);
            if (a4 < 0)
            {
                a4 = a4 + 19;
            }

            //return (String)digraph2.get(new Integer((int)Math.Floor(a4)));
            return (String)digraph2[Math.Floor(a4)];
        }

    }

    public class LatZones
    {
        private char[] letters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K',
        'L', 'M', 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Z' };

        private int[] degrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24, -16,
        -8, 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private char[] negLetters = { 'A', 'C', 'D', 'E', 'F', 'G', 'H', 'J', 'K', 'L', 'M' };

        private int[] negDegrees = { -90, -84, -72, -64, -56, -48, -40, -32, -24, -16, -8 };

        private char[] posLetters = { 'N', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Z' };

        private int[] posDegrees = { 0, 8, 16, 24, 32, 40, 48, 56, 64, 72, 84 };

        private int arrayLength = 22;

        public LatZones()
        {
        }

        public int getLatZoneDegree(String letter)
        {
            //char ltr = letter.charAt(0);
            char ltr = letter.ToCharArray()[0];
            for (int i = 0; i < arrayLength; i++)
            {
                if (letters[i] == ltr)
                {
                    return degrees[i];
                }
            }
            return -100;
        }

        public String getLatZone(double latitude)
        {
            int latIndex = -2;
            int lat = (int)latitude;

            if (lat >= 0)
            {
                int len = posLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == posDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat > posDegrees[i])
                    {
                        continue;
                    }
                    else
                    {
                        latIndex = i - 1;
                        break;
                    }
                }
            }
            else
            {
                int len = negLetters.Length;
                for (int i = 0; i < len; i++)
                {
                    if (lat == negDegrees[i])
                    {
                        latIndex = i;
                        break;
                    }

                    if (lat < negDegrees[i])
                    {
                        latIndex = i - 1;
                        break;
                    }
                    else
                    {
                        continue;
                    }

                }

            }

            if (latIndex == -1)
            {
                latIndex = 0;
            }
            if (lat >= 0)
            {
                if (latIndex == -2)
                {
                    latIndex = posLetters.Length - 1;
                }

                //return String.valueOf(posLetters[latIndex]);
                return Convert.ToString(posLetters[latIndex]);
            }
            else
            {
                if (latIndex == -2)
                {
                    latIndex = negLetters.Length - 1;
                }
                //return String.valueOf(negLetters[latIndex]);
                return Convert.ToString(negLetters[latIndex]);

            }
        }
    }
}
