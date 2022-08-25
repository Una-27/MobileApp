using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using ModelLibrary;

namespace Calcul
{
    public enum Food_
    {
        [Description("Нет")]
        No,
        [Description("Веган")]
        Vegan,
        [Description("Вегетарианец")]
        Vegetarian
        
    }
    class PersonalData : SerializableObject
    {
        public string name;
        public int age;
        public int height;
        public int current_weight;
        public int target_weight;
        public Food_ food;			// food: {нет, веган, вегетарианец}
        public int purpose;			// purpose: {сбросить вес, поддержать вес, набрать вес}
        public int activity;		// activity: {низкий, средний, высокий, очень высокий}
        public bool gender;		    // gender: true - Ж, false - М
        public byte[] image;
        public int calories;

        private string path = System.IO.Path.Combine(Xamarin.Essentials.FileSystem.AppDataDirectory, "userfile.txt");
        public PersonalData()
        {
            name = "";
            age = 0;
            height = 0;
            current_weight = 0;
            target_weight = 0;
            food = 0;
            purpose = 1;
            activity = 0;
            gender = true;
            image = null;
            calories = 0;
        }
        public Bitmap Convert_image_to(byte[] im)
        {
            return BitmapFactory.DecodeByteArray(im, 0, im.Length);
        }
        public byte[] Convert_image_from(Bitmap im)
        {
            using (var st = new MemoryStream())
            {
                im.Compress(Bitmap.CompressFormat.Jpeg, 0, st);
                return st.ToArray();
            }
        }
        public string Convert_purpose()
        {
            switch (purpose)
            {
                case 0: return "Сбросить вес";
                case 1: return "Поддержать вес";
                case 2: return "Набрать вес";
            }
            return "";
        }
        public string Convert_gender()
        {
            switch (gender)
            {
                case true: return "Женский";
                case false: return "Мужской";
            }
            return "";
        }
        public string Convert_activity()
        {
            switch (activity)
            {
                case 0: return "Низкий";
                case 1: return "Средний";
                case 2: return "Высокий";
                case 3: return "Очень высокий";
            }
            return "";
        }
        public void FileWR(PersonalData us)
        {
            calories = (int)Calculation_colories();
            FileInfo flw = new FileInfo(path);
            flw.Delete();
            using (FileStream filew = new FileStream(path, FileMode.OpenOrCreate))
            {
                string textin = Serialization<PersonalData>.Write(us);
                byte[] array = System.Text.Encoding.Default.GetBytes(textin);
                filew.Write(array, 0, array.Length);
                filew.Close();
            }
        }
        public PersonalData FileRD()
        {
            using (FileStream filer = File.OpenRead(path))
            {
                byte[] array = new byte[filer.Length];
                filer.Read(array, 0, array.Length);
                string textout = System.Text.Encoding.Default.GetString(array);
                filer.Close();
                return Serialization<PersonalData>.Read(textout);
            }
        }
        public double Calculation_colories()
        {
            double[] activ_k = new double[4] { 1.2, 1.46, 1.64, 1.9 };
            double BMR;
            if (gender == true) 
                BMR = (((9.99 * current_weight) + (6.25 * height) - (4.92 * age) - 161) * activ_k[activity]) * ((double)target_weight / (double)current_weight);
            else
                BMR = (((9.99 * current_weight) + (6.25 * height) - (4.92 * age)  + 5) * activ_k[activity]) * ((double)target_weight / (double)current_weight);
            return BMR;
        }
       /* ~PersonalData()
        {
            FileInfo ff = new FileInfo(path);
            if (ff.Exists)
            {
                FileWR(this);
            }
        }*/
    }
}
