using AudioPlayerWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioPlayerWebAPI.Infrastructure.EntityTypeConfigurations
{
    public class UserPlaylistsConfiguration : IEntityTypeConfiguration<UserPlaylists>
    {
        public void Configure(EntityTypeBuilder<UserPlaylists> builder)
        {
            builder.HasKey("UserId", "PlaylistId");
            builder.HasIndex("UserId", "PlaylistId");
        }
    }
}
