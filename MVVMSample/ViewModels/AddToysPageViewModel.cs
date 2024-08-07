﻿using MVVMSample.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MVVMSample.ViewModels
{
    class AddToysPageViewModel : ViewModelBase
    {
        #region list of toys and toys Type will be moved to Service Folder
        List<Toy> toys = new();//רשימת הצעצועים
        public List<ToyTypes> ToyTypes
        { get; set;} = new List<ToyTypes>()
        {
            new ToyTypes()
            {
                Id = 1, Name = "פאזל" 
            },

            new ToyTypes()
            {
            Id = 2, Name = "משחקי חשיבה"
            },
            new ToyTypes()
            {
            Id = 3, Name = "בובה" 
            }
        };

        #endregion

        #region Fields
        Toy newToy;//צעצוע להוספה

        private string name;//שם צעצוע
        private double price;//מחיר צעצוע
        private string errorMessage;//הודעת שגיאה
        private ToyTypes selectedType;//סוג צעצוע נבחר

        #endregion

        #region Properties
      
        //שם צעצוע
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                if (name != value)
                {

                    name = value;
                    OnPropertyChanged(nameof(Name));
                    HandleError();//בדוק האם יש השם תקין
                }

            }
        }
        //האם השם של הצעצוע תקין
        public bool HasError
        {
            get
            {
                return string.IsNullOrEmpty(Name) || !ValidName();
            }

        }
        //הודעת שגיאה
        public string ErrorMessage
        {
            get
            {
                return errorMessage;
            }
            set
            {
                if (errorMessage != value)
                {
                    errorMessage = value;
                    OnPropertyChanged(nameof(ErrorMessage));
                }
            }
        }
 //מחיר
        public double Price
        {
            get
            {
                return price;
            }
            set
            {
                if (price != value)
                {
                    price = value;
                    OnPropertyChanged(nameof(Price));
                }
            }
        }
        //סוג צעצוע נבחר
        public ToyTypes SelectedType
        {
            get
            {
                return selectedType;
            }
            set
            {
                if (selectedType != value)
                {
                    selectedType = value;
                    OnPropertyChanged(nameof(SelectedType));
                }
            }
        }
        //האם ניתן לבצע את הפעולה של הוספת צעצוע
        public bool CanAddToy
        {
            get
            {//אין בעיה בשם וגם מחיר גדול מ0 וגם יש ערך בסוג צעצוע
                return !HasError && Price>0&&SelectedType!=null;
            }
        }
       
        #endregion

        #region Commands
        //PROPERTY to be Bound to the Button Command
        public ICommand AddToyCommand
        {
            get; private set;
        }
        #endregion

        #region Constructor
        public AddToysPageViewModel()
        {
            //connect the property with the action method to perform
            //Command([method Name]) --> for void methods
            //Command<type>([method name]--> for methods that has parameters

            AddToyCommand = new Command<string>(AddnewToy);

        }

        #endregion

        #region פעולות 
        //לוגיקת שגיאה
        private void HandleError()
        {
            if (!ValidName())
            {
                ErrorMessage = "שם קצר מידי או מכיל ערכים לא תקינים";
            }
            else
                ErrorMessage = string.Empty;
            OnPropertyChanged(nameof(HasError));
            OnPropertyChanged(nameof(CanAddToy));
        }
        
        #region Implement Valid Checks
        //1. name Should start Capital letter, at lease 4 letter word
        //Regex: ^[A-Z][a-zA-Z]{3,}
        //sample Prompt used in claude:
        /*
         * אני צריך REGEX expression עבור מחרוזת בעלת אילוצים הבאים: 
         * 1. חייבת להתחיל באות גדולה
         * 2. לפחות 4 אותיות
         * כתוב גם מקרי קצה שיש לבדוק שאכן תקין 3. 
         * 4. המלץ על אתרים בהם ניתן לבדוק אותו.
         * השימוש יעשה בשפת C# */
        private bool ValidName()
        {
            Regex reg = new Regex(@"^[A-Z][a-zA-Z]{3,}$");
            return reg.IsMatch(Name);
        }
        #endregion

        //הוספת צעצוע חדש
        private void AddnewToy(string name)
        {
            newToy = new Toy() { Id = 1, Name = this.Name, Price = this.Price, Type = SelectedType };
            if (name == "הוסף צעצוע")
                newToy.IsSecondHand = false;
            else
            {
                newToy.IsSecondHand = true;
                newToy.Price = newToy.Price * 0.9;
            }
            toys.Add(newToy);



        }
        #endregion
    }
}