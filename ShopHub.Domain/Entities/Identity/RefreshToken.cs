using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShopHub.Domain.Entities.Identity
{
    public class RefreshToken
    {
        // Primary key of the refresh token record in the database.
        public int Id { get; set; }

        // The refresh token value sent to the client and used
        // to request a new access token.
        public string Token { get; set; }

        // The date and time when the refresh token was created.
        // UtcNow is used to keep time consistent across different time zones.
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        // The date and time when the refresh token will expire.
        // After this time, the refresh token cannot be used.
        public DateTime ExpiresOn { get; set; }

        // The date and time when the refresh token was revoked.
        // Null means the token has not been revoked.
        public DateTime? RevokedOn { get; set; }

        // Returns true when the current time has reached or passed
        // the token's expiration time.
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

        // Returns true only when the token has not been revoked
        // and has not expired.
        public bool IsActive => RevokedOn == null && !IsExpired;

        // The user who owns this refresh token
        public string UserId { get; set; } = null!;

        // Navigation property
        public AppUser User { get; set; } = null!;
    }
}
