using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singletone : MonoBehaviour
{
   public static bool canMove = true;

   //надеюсь это дерьмо никто не увидит
   public static int[] calculateColumnsRovs(int x)
   {
      switch (x)
      {
         case 0:
            return new int[] {1, 1};
         case 1:
            return new int[] {1, 2};
         case 2:
            return new int[] {1, 3};
         case 3:
            return new int[] {2, 1};
         case 4:
            return new int[] {2, 2};
         case 5:
            return new int[] {2, 3};
         case 6:
            return new int[] {3, 1};
         case 7:
            return new int[] {3, 2};
         case 8:
            return new int[] {3, 3};
      }

      return new int[] {0, 0};
   }

   public static int calculatePosittion(int[] mass)
   {
      if (mass == new int[] {1, 1})
      {
         return 0;
      }
      else if(mass == new int[] {1, 2})
      {
         return 1;
      }
      else if(mass == new int[] {1, 3})
      {
         return 2;
      }
      else if(mass == new int[] {2, 1})
      {
         return 3;
      }
      else if(mass == new int[] {2, 2})
      {
         return 4;
      }
      else if(mass == new int[] {2, 3})
      {
         return 5;
      }
      else if(mass == new int[] {3, 1})
      {
         return 6;
      }
      else if(mass == new int[] {3, 2})
      {
         return 7;
      }
      else if(mass == new int[] {3, 3})
      {
         return 8;
      }

      return 0;

   }

}
