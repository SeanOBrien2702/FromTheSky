using UnityEngine;

namespace FTS.Characters
{
    public static class RandomNameGen
    {
        static string[] firstNames = new string[] {"Ed", "Roxann"  ,"Apolonia"  ,"Fanny"
                                            ,"Bessie","Nakita","Forrest" ,"Shanika"
                                            ,"Aracelis","Mildred","Rosalind","Nelson","Eve","Emile"
                                            ,"Blanch","Chia","Dominic","Troy","Deloise","Meghan"
                                            ,"Marva","Stacey","Tania","Dwight","Sonja","Xuan","Ferdinand"
                                            ,"Francene","Lynna","Mayme","Jenee","Carli","Lenard"
                                            ,"Thu","Susannah","Danica","Bette","Vern","Camellia"
                                            ,"Coletta","Lauren","Vernon","Miquel","Ethelyn","Janette"
                                            ,"Fredericka"  ,"Emery","Jenise","Briana","Lekisha"};



        static string[] lastNames = new string[] {"Drake","Walters","Doty","Mccall","Mcdonald"
                                            ,"Hurley","Edmonds","Mccauley","Crocker ","Vega"
                                            ,"Leal","Mcclendon","Katz","Holman","Dotson"
                                            ,"Schaefer","Muller","Boswell","Blount","Kurtz"
                                            ,"Xiong","Corley","Hutton","Murdock","Mcwilliams"
                                            ,"Corbin","Darden","Gorman","Hagan ","Becker","Quintero"
                                            ,"Melton","Carpenter ","Rosen","Beck","Rucker"
                                            ,"Rollins","Cooper","Pruitt","Addison","Downey"
                                            ,"Koehler","Blevins","Arnold","Coon","Salazar"
                                            ,"Gipson","Compton","Pelletier","Steward"};

        public static string GenerateName()
        {
            string name = "No Name";

            name = firstNames[Random.Range(0, 50)];

            name += " " + lastNames[Random.Range(0, 50)];

            return name;
        }
    }
}
