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
    public partial class UserDto : IEquatable<UserDto>
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
        /// Gets or Sets EvaluationAsAdvertiser
        /// </summary>

        [DataMember(Name="evaluationAsAdvertiser")]
        public double? EvaluationAsAdvertiser { get; set; }


        /// <summary>
        /// Gets or Sets EvaluationAsAdvertiserCount
        /// </summary>

        [DataMember(Name = "evaluationAsAdvertiserCount")]
        public int? EvaluationAsAdvertiserCount { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsLender
        /// </summary>

        [DataMember(Name="evaluationAsLender")]
        public double? EvaluationAsLender { get; set; }

        /// <summary>
        /// Gets or Sets EvaluationAsLenderCount
        /// </summary>

        [DataMember(Name = "evaluationAsLenderCount")]
        public int? EvaluationAsLenderCount { get; set; }

        /// <summary>
        /// Gets or Sets Image
        /// </summary>

        [DataMember(Name = "image")]
        public string Image { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class User {\n");
            sb.Append("  Id: ").Append(Id).Append("\n");
            sb.Append("  Name: ").Append(Name).Append("\n");
            sb.Append("  EvaluationAsAdvertiser: ").Append(EvaluationAsAdvertiser).Append("\n");
            sb.Append("  EvaluationAsLender: ").Append(EvaluationAsLender).Append("\n");
            sb.Append("  Image: ").Append(Image).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UserDto)obj);
        }

        /// <summary>
        /// Returns true if User instances are equal
        /// </summary>
        /// <param name="other">Instance of User to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UserDto other)
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
                    EvaluationAsAdvertiser == other.EvaluationAsAdvertiser ||
                    EvaluationAsAdvertiser != null &&
                    EvaluationAsAdvertiser.Equals(other.EvaluationAsAdvertiser)
                ) && 
                (
                    EvaluationAsLender == other.EvaluationAsLender ||
                    EvaluationAsLender != null &&
                    EvaluationAsLender.Equals(other.EvaluationAsLender)
                ) &&
                (
                    Image == other.Image ||
                    Image != null &&
                    Image.Equals(other.Image)
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
                    if (EvaluationAsAdvertiser != null)
                    hashCode = hashCode * 59 + EvaluationAsAdvertiser.GetHashCode();
                    if (EvaluationAsLender != null)
                    hashCode = hashCode * 59 + EvaluationAsLender.GetHashCode();
                    if (Image != null)
                    hashCode = hashCode * 59 + Image.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UserDto left, UserDto right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UserDto left, UserDto right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
