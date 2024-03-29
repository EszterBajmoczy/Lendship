/*
 * Simple Inventory API
 *
 * This is a simple API
 *
 * OpenAPI spec version: 1.0.0
 * Contact: you@your-company.com
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using System;
using System.Linq;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;
using Lendship.Backend.Models;
using static Lendship.Backend.DTO.ReservationDetailDto;

namespace Lendship.Backend.DTO
{ 
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class AdvertisementDetailsDto : IEquatable<AdvertisementDetailsDto>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public int? Id { get; set; }

        /// <summary>
        /// Gets or Sets IsService
        /// </summary>
        [Required]

        [DataMember(Name = "isService")]
        public bool IsService { get; set; }

        /// <summary>
        /// Gets or Sets User
        /// </summary>        
        [DataMember(Name="user")]
        public UserDto User { get; set; }

        /// <summary>
        /// Gets or Sets Title
        /// </summary>
        [Required]
        
        [DataMember(Name="title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or Sets Description
        /// </summary>
        [Required]
        
        [DataMember(Name="description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or Sets InstructionManual
        /// </summary>

        [DataMember(Name="instructionManual")]
        public string InstructionManual { get; set; }

        /// <summary>
        /// Gets or Sets Price
        /// </summary>

        [DataMember(Name="price")]
        public int? Price { get; set; }

        /// <summary>
        /// Gets or Sets Credit
        /// </summary>

        [DataMember(Name="credit")]
        public int? Credit { get; set; }

        /// <summary>
        /// Gets or Sets Deposit
        /// </summary>

        [DataMember(Name="deposit")]
        public int? Deposit { get; set; }

        /// <summary>
        /// Gets or Sets Latitude
        /// </summary>
        [Required]
        
        [DataMember(Name="latitude")]
        public decimal Latitude { get; set; }

        /// <summary>
        /// Gets or Sets Longitude
        /// </summary>
        [Required]
        
        [DataMember(Name="longitude")]
        public decimal Longitude { get; set; }

        /// <summary>
        /// Gets or Sets Location
        /// </summary>
        [Required]

        [DataMember(Name = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or Sets IsPublic
        /// </summary>
        [Required]
        
        [DataMember(Name="isPublic")]
        public bool? IsPublic { get; set; }

        /// <summary>
        /// Gets or Sets Category
        /// </summary>
        [Required]
        
        [DataMember(Name="category")]
        public CategoryDto Category { get; set; }

        /// <summary>
        /// Gets or Sets Availabilities
        /// </summary>

        [DataMember(Name="availabilities")]
        public List<AvailabilityDto> Availabilities { get; set; }

        /// <summary>
        /// Gets or Sets ImageLocations
        /// </summary>

        [DataMember(Name = "imageLocations")]
        public List<string> ImageLocations { get; set; }

        /// <summary>
        /// Gets or Sets PrivateUsers
        /// </summary>

        [DataMember(Name = "privateUsers")]
        public List<UserDto> PrivateUsers { get; set; }

        /// <summary>
        /// Gets or Sets Creation
        /// </summary>

        [DataMember(Name="creation")]
        public DateTime? Creation { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Advertisement {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  IsService: ").Append(IsService).Append("\n");
            sb.Append("  User: ").Append(User).Append("\n");
            sb.Append("  Title: ").Append(Title).Append("\n");
            sb.Append("  Description: ").Append(Description).Append("\n");
            sb.Append("  InstructionManual: ").Append(InstructionManual).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");
            sb.Append("  Credit: ").Append(Credit).Append("\n");
            sb.Append("  Deposit: ").Append(Deposit).Append("\n");
            sb.Append("  Latitude: ").Append(Latitude).Append("\n");
            sb.Append("  Longitude: ").Append(Longitude).Append("\n");
            sb.Append("  Location: ").Append(Location).Append("\n");
            sb.Append("  IsPublic: ").Append(IsPublic).Append("\n");
            sb.Append("  Category: ").Append(Category).Append("\n");
            sb.Append("  Availabilities: ").Append(Availabilities).Append("\n");
            sb.Append("  Creation: ").Append(Creation).Append("\n");
            sb.Append("  ImageLocations: ").Append(ImageLocations).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }
        
        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((AdvertisementDetailsDto)obj);
        }

        /// <summary>
        /// Returns true if Advertisement instances are equal
        /// </summary>
        /// <param name="other">Instance of Advertisement to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(AdvertisementDetailsDto other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Id == other.Id ||
                    Id != null &&
                    Id.Equals(other.Id)
                ) &&
                (
                    IsService == other.IsService ||
                    IsService != null &&
                    IsService.Equals(other.IsService)
                ) &&
                (
                    User == other.User ||
                    User != null &&
                    User.Equals(other.User)
                ) &&
                (
                    Title == other.Title ||
                    Title != null &&
                    Title.Equals(other.Title)
                ) && 
                (
                    Description == other.Description ||
                    Description != null &&
                    Description.Equals(other.Description)
                ) && 
                (
                    InstructionManual == other.InstructionManual ||
                    InstructionManual != null &&
                    InstructionManual.Equals(other.InstructionManual)
                ) && 
                (
                    Price == other.Price ||
                    Price != null &&
                    Price.Equals(other.Price)
                ) && 
                (
                    Credit == other.Credit ||
                    Credit != null &&
                    Credit.Equals(other.Credit)
                ) && 
                (
                    Deposit == other.Deposit ||
                    Deposit != null &&
                    Deposit.Equals(other.Deposit)
                ) && 
                (
                    Latitude == other.Latitude ||
                    Latitude != null &&
                    Latitude.Equals(other.Latitude)
                ) && 
                (
                    Longitude == other.Longitude ||
                    Longitude != null &&
                    Longitude.Equals(other.Longitude)
                ) &&
                (
                    Location == other.Location ||
                    Location != null &&
                    Location.Equals(other.Location)
                ) &&
                (
                    IsPublic == other.IsPublic ||
                    IsPublic != null &&
                    IsPublic.Equals(other.IsPublic)
                ) && 
                (
                    Category == other.Category ||
                    Category != null &&
                    Category.Equals(other.Category)
                ) && 
                (
                    Availabilities == other.Availabilities ||
                    Availabilities != null &&
                    Availabilities.SequenceEqual(other.Availabilities)
                ) &&
                (
                    ImageLocations == other.ImageLocations ||
                    ImageLocations != null &&
                    ImageLocations.SequenceEqual(other.ImageLocations)
                ) &&
                (
                    Creation == other.Creation ||
                    Creation != null &&
                    Creation.Equals(other.Creation)
                );
        }
        

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (Id != null)
                    hashCode = hashCode * 59 + Id.GetHashCode();
                    hashCode = hashCode * 59 + IsService.GetHashCode();
                    if (User != null)
                    hashCode = hashCode * 59 + User.GetHashCode();
                    if (Title != null)
                    hashCode = hashCode * 59 + Title.GetHashCode();
                    if (Description != null)
                    hashCode = hashCode * 59 + Description.GetHashCode();
                    if (InstructionManual != null)
                    hashCode = hashCode * 59 + InstructionManual.GetHashCode();
                    if (Price != null)
                    hashCode = hashCode * 59 + Price.GetHashCode();
                    if (Credit != null)
                    hashCode = hashCode * 59 + Credit.GetHashCode();
                    if (Deposit != null)
                    hashCode = hashCode * 59 + Deposit.GetHashCode();
                    if (Latitude != null)
                    hashCode = hashCode * 59 + Latitude.GetHashCode();
                    if (Longitude != null)
                    hashCode = hashCode * 59 + Longitude.GetHashCode();
                    if (Location != null)
                    hashCode = hashCode * 59 + Location.GetHashCode();
                    if (IsPublic != null)
                    hashCode = hashCode * 59 + IsPublic.GetHashCode();
                    if (Category != null)
                    hashCode = hashCode * 59 + Category.GetHashCode();
                    if (Availabilities != null)
                    hashCode = hashCode * 59 + Availabilities.GetHashCode();
                    if (ImageLocations != null)
                    hashCode = hashCode * 59 + ImageLocations.GetHashCode();
                    if (Creation != null)
                    hashCode = hashCode * 59 + Creation.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(AdvertisementDetailsDto left, AdvertisementDetailsDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(AdvertisementDetailsDto left, AdvertisementDetailsDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
