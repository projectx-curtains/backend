﻿namespace Curtains.Domain.Models
{
    /// <summary>
    ///  This class describes fabric model.
    /// </summary>
    public class FabricModel : ExtendedBaseEntity
    {
        #region PropertiesRegion
        /// <summary>
        ///  This property represents situation, if fabric is dense or not.
        /// </summary>
        public bool Density { get; set; }
        /// <summary>
        ///  This property represents ColorGroupId column.
        /// </summary>
        public int ColorGroup { get; set; }
        /// <summary>
        ///  This property represents DecorationId column.
        /// </summary>
        public int DecorationsId { get; set; }
        /// <summary>
        ///  This property represents ManufacturerId column.
        /// </summary>
        public int ManufacturerId { get; set; }
        /// <summary>
        ///  This property represents price of fabric.
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        ///  This property represents length of fabric.
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        ///  This property represents wifth of fabric.
        /// </summary>
        public int Width { get; set; }
        /// <summary>
        ///  This property represents designId of fabric.
        /// </summary>
        public int DesignId { get; set; }
        /// <summary>
        ///  This property represents MaterialId of fabric.
        /// </summary>
        public int MaterialId { get; set; }
        #endregion

        #region NavigationProperties
        /// <summary>
        ///  This property represents color group of fabric.
        /// </summary>
        public virtual ColorGroupModel ColorGroupId { get; set; }
        /// <summary>
        ///  This property represents decoration of fabric.
        /// </summary>
        public virtual DecorationsModel Decorations { get; set; }
        /// <summary>
        ///  This property represents design of fabric.
        /// </summary>
        public virtual DesignModel Design { get; set; }
        /// <summary>
        ///  This property represents material of fabric.
        /// </summary>
        public virtual MaterialModel Material { get; set; }
        /// <summary>
        ///  This property represents manufacturer of fabric.
        /// </summary>
        public virtual ManufacturerModel Manufacturer { get; set; }
        /// <summary>
        ///  This property represents list of curtains with that fabric.
        /// </summary>
        public virtual ICollection<CurtainsModel> Curtains { get; set; }
        /// <summary>
        ///  This property represents list of bedspreads with that fabric.
        /// </summary>
        public virtual ICollection<BedspreadsModel> Bedspreads { get; set; }
        /// <summary>
        ///  This property represents list of pillows with that fabric.
        /// </summary>
        public virtual ICollection<PillowsModel> Pillows { get; set; }
        /// <summary>
        ///  This property represents product images of fabric.
        /// </summary>
        public virtual ICollection<ProductImageModel> ProductImages { get; }
        #endregion
    }
}
