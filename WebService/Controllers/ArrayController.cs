using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;

namespace WebService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ArrayController : ControllerBase
    {

        Array array = Array.GetInstance();

        [HttpPost]
        public ActionResult<string> Post([FromBody]string greeting)
        {
            try
            {
                array.AddIntoArray(greeting);
                return greeting;
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }

        [HttpPut]
        public ActionResult<string> Put([FromQuery]int itemNumber, string greeting)
        {
            try
            {
                array.AddIntoArrayToNumber(itemNumber, greeting);
                return greeting;
            }
            catch (Exception e)
            {
                return BadRequest();
            }
        }

        ///Array
        [HttpGet]
        public ActionResult<string[]> Get()
        {
            return array.array.ToArray();
        }

        ///Array/1
        [HttpGet("{itemNumber}")]
        public ActionResult<string> GetByNumber(int itemNumber)
        {
            try
            {
                return array.GetElement(itemNumber);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
        }


        [HttpDelete]                
       public ActionResult<string> Delete([FromBody]int itemNumber)
       {
            try
            {
                return array.DeleteIntoArray(itemNumber);
            }
            catch(Exception e)
            {
                return BadRequest();
            }
       }
    }

    public class Array
    {
        private readonly string[] checkRusArray = new string[5] { "привет", "добрый день", "добрый вечер", "доброе утро", "здравствуйте" };
        private readonly string[] checkEngArray = new string[5] { "hi", "hello", "good evening", "good morning", "good afternoon" };

        public List<string> array { get; set; }

        private static Array instance;

        private Array()
        {
            array = new List<string>();
        }

        public static Array GetInstance()
        {
            if (instance == null)
                instance = new Array();
            return instance;
        }

        public void AddIntoArray(string greeting)
        {
            if (!CheckPermissibleValue(greeting))
            {
                throw new Exception("Недопустимое приветствие");
            }
            array.Add(greeting);

        }
        public void AddIntoArrayToNumber(int number, string greeting)
        {
            if (CheckOutOfRange(number))
            {
                throw new Exception($"Количество элементов в массиве меньше {number}");
            }

            if (CheckPermissibleValue(greeting))
            {
                throw new Exception("Недопустимое приветствие");
            }

            array[number - 1] = greeting;
        }

        public string DeleteIntoArray(int number)
        {
            if (!CheckOutOfRange(number))
            {
                var greeting = array[number - 1];
                array.Remove(greeting);
                return greeting;
            }

            throw new Exception($"Количество элементов в массиве меньше {number}");
        }

        public string GetElement(int number)
        {
            if (CheckOutOfRange(number))
            {
                throw new Exception($"Количество элементов в массиве меньше {number}");
            }

            return array[number - 1];
        }

        /// <summary>
        /// проверка, что элемент массива существует
        /// </summary>
        private bool CheckOutOfRange(int number)
        {
            return array.Count < number;
        }

        /// <summary>
        /// проверка, что мы добавлем только приветствие
        /// </summary>
        private bool CheckPermissibleValue(string greeting)
        {
            foreach (var word in checkRusArray)
            {
                if (String.Compare(greeting, word, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }

            foreach (var word in checkEngArray)
            {
                if (String.Compare(greeting, word, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
