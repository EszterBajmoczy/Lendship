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
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace Lendship.Backend.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class UserDetailsDto : IEquatable<UserDetailsDto>
    { 
        /// <summary>
        /// Gets or Sets Id
        /// </summary>

        [DataMember(Name="id")]
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or Sets Name
        /// </summary>
        [Required]
        
        [DataMember(Name="name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or Sets Email
        /// </summary>
        [Required]
        
        [DataMember(Name="email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or Sets EmailNotification
        /// </summary>
        [Required]
        
        [DataMember(Name="emailNotification")]
        public bool? EmailNotification { get; set; }

        /// <summary>
        /// Gets or Sets Credit
        /// </summary>

        [DataMember(Name="credit")]
        public int? Credit { get; set; }

        /// <summary>
        /// Gets or Sets Latitude
        /// </summary>
        [Required]
        
        [DataMember(Name="latitude")]
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or Sets Longitude
        /// </summary>
        [Required]
        
        [DataMember(Name="longitude")]
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Gets or Sets Location
        /// </summary>
        [Required]

        [DataMember(Name = "location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsAdvertiser
        /// </summary>

        [DataMember(Name = "evaluationAsAdvertiser")]
        public double? EvaluationAsAdvertiser { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsAdvertiserCount
        /// </summary>

        [DataMember(Name = "evaluationAsAdvertiserCount")]
        public int? EvaluationAsAdvertiserCount { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsLender
        /// </summary>

        [DataMember(Name = "evaluationAsLender")]
        public double? EvaluationAsLender { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsLenderCount
        /// </summary>

        [DataMember(Name = "evaluationAsLenderCount")]
        public int? EvaluationAsLenderCount { get; set; }

        /// <summary>
        /// Gets or Sets Registration
        /// </summary>
        [Required]
        
        [DataMember(Name="registration")]
        public DateTime? Registration { get; set; }

        /// <summary>
        /// Gets or Sets Image
        /// </summary>
        [Required]

        [DataMember(Name = "image")]
        public string Image { get; set; }

        /// <summary>
        /// Gets or Sets Evaluation
        /// </summary>

        [DataMember(Name = "evaluation")]
        public EvaluationComputedDto? Evaluation { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UserDetails {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  Email: ").Append(Email).Append("\n");
            sb.Append("  EmailNotification: ").Append(EmailNotification).Append("\n");
            sb.Append("  Credit: ").Append(Credit).Append("\n");
            sb.Append("  Latitude: ").Append(Latitude).Append("\n");
            sb.Append("  Longitude: ").Append(Longitude).Append("\n");
            sb.Append("  Location: ").Append(Location).Append("\n");
            sb.Append("  Registration: ").Append(Registration).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
            sb.Append("  Evaluation: ").Append(Evaluation).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserDetailsDto)obj);
        }

        /// <summary>
        /// Returns true if UserDetails instances are equal
        /// </summary>
        /// <param name="other">Instance of UserDetails to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserDetailsDto other)
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
                    Name == other.Name ||
                    Name != null &&
                    Name.Equals(other.Name)
                ) && 
                (
                    Email == other.Email ||
                    Email != null &&
                    Email.Equals(other.Email)
                ) && 
                (
                    EmailNotification == other.EmailNotification ||
                    EmailNotification != null &&
                    EmailNotification.Equals(other.EmailNotification)
                ) && 
                (
                    Credit == other.Credit ||
                    Credit != null &&
                    Credit.Equals(other.Credit)
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
                    Registration == other.Registration ||
                    Registration != null &&
                    Registration.Equals(other.Registration)
                ) &&
                (
                    Image == other.Image ||
                    Image != null &&
                    Image.Equals(other.Image)
                ) &&
                (
                    Evaluation == other.Evaluation ||
                    Evaluation != null &&
                    Evaluation.Equals(other.Evaluation)
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
                    if (Name != null)
                    hashCode = hashCode * 59 + Name.GetHashCode();
                    if (Email != null)
                    hashCode = hashCode * 59 + Email.GetHashCode();
                    if (EmailNotification != null)
                    hashCode = hashCode * 59 + EmailNotification.GetHashCode();
                    if (Credit != null)
                    hashCode = hashCode * 59 + Credit.GetHashCode();
                    if (Latitude != null)
                    hashCode = hashCode * 59 + Latitude.GetHashCode();
                    if (Longitude != null)
                    hashCode = hashCode * 59 + Longitude.GetHashCode();
                    if (Location != null)
                    hashCode = hashCode * 59 + Location.GetHashCode();
                    if (Registration != null)
                    hashCode = hashCode * 59 + Registration.GetHashCode();
                    if (Image != null)
                    hashCode = hashCode * 59 + Image.GetHashCode();
                    if (Evaluation != null)
                    hashCode = hashCode * 59 + Evaluation.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UserDetailsDto left, UserDetailsDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserDetailsDto left, UserDetailsDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
