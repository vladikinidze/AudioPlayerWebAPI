using AudioPlayerWebAPI.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AudioPlayerWebAPI.Infrastructure.EntityTypeConfigurations
{
    public class PlaylistTracksConfiguration : IEntityTypeConfiguration<PlaylistTracks>
    {
        public void Configure(EntityTypeBuilder<PlaylistTracks> builder)
        {
            builder.HasKey("PlaylistId", "TrackId");
            builder.HasIndex("PlaylistId", "TrackId");
        }
    }
}
