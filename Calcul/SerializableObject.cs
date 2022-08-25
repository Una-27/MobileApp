using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace ModelLibrary
{
    public abstract class SerializableObject
    {

    }

    public enum RecipeType
    {
        Primary = 0,
        Secondary = 1,
        Garnish = 2,
        ColdSnacks = 3,
        Drink = 4
    }

    public static class Tools 
    {
        public static string GetEnumName(this Enum enumValue)
        {
            return enumValue.GetType()
                            .GetMember(enumValue.ToString())
                            .First().Name;
        }
    }

    public class Recipe : SerializableObject
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public int Weight { get; set; }
        public int Colories { get; set; }
        public int Proteins { get; set; }
        public int Greases { get; set; }
        public int Carbohydrates { get; set; }
        public bool IsVegan { get; set; }
        public bool IsVegetarian { get; set; }
        public byte[] MainPicture { get; set; }
        public int PictureCount { get; set; }
        public RecipeType Type { get; set; }
    }

    public class DayMenu : SerializableObject
    { 
        public long Id { get; set; }
        public string Name { get; set; }
        public String Date { get; set; }
        public List<Recipe> BreakfastRecipes { get; set; } = new List<Recipe>();
        public List<Recipe> LaunchRecipes { get; set; } = new List<Recipe>();
        public List<Recipe> DinnerRecipes { get; set; } = new List<Recipe>();
    }

    public static class Serialization<T> where T : SerializableObject
    {
        public static List<T> ReadList(string bytes)
        {
            return JsonConvert.DeserializeObject<List<T>>(bytes);
        }

        public static string WriteList(List<T> recipes)
        {
            return JsonConvert.SerializeObject(recipes.ToArray());
        }

        public static T Read(string bytes)
        {
            return JsonConvert.DeserializeObject<T>(bytes);
        }

        public static string Write(T recipe)
        {
            return JsonConvert.SerializeObject(recipe);
        }
    }
}
