using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TshirtCo_NN.Models
{
    public class Image
    {
        /// <summary>
        /// constructor to set the folder that the uploaded product images will be saved in
        /// </summary>
        public const string ProductImageFolder = @"images\products";
        /// <summary>
        /// constructor to set the folder that the default product image is in if an image doesn't get uploaded
        /// </summary>
        public const string DefaultProductImage = "default_image.jpg";

        /// <summary>
        /// constructor to set the folder that the uploaded design images will be saved in
        /// </summary>
        public const string DesignImageFolder = @"images\designs";
        /// <summary>
        /// constructor to set the folder that the default design image is in if an image doesn't get uploaded
        /// </summary>
        public const string DefaultDesignImage = "default_image.jpg";
    }
}
