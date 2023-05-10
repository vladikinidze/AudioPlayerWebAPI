namespace AudioPlayerWebAPI.Models
{
    public class AudioPlayerDbContext : DbContext
    {
        public AudioPlayerDbContext(DbContextOptions<AudioPlayerDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; } = null!;
        public DbSet<Playlist> Playlists { get; set; } = null!;
        public DbSet<Track> Tracks { get; set; } = null!;
        public DbSet<UserRefreshToken> RefreshTokens { get; set; } = null!;
         
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.RefreshToken)
                .WithOne(rt => rt.User)
                .HasForeignKey<UserRefreshToken>(rt => rt.UserId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Playlists)
                .WithMany(p => p.Users)
                .UsingEntity(x => x.ToTable("UserPlaylists"));

            modelBuilder.Entity<Playlist>()
                .HasMany(p => p.Tracks)
                .WithMany(t => t.Playlists)
                .UsingEntity(x => x.ToTable("PlaylistTracks"));

            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Playlist>().ToTable("Playlist");
            modelBuilder.Entity<Track>().ToTable("Track");
            modelBuilder.Entity<UserRefreshToken>().ToTable("RefreshToken");
            modelBuilder.ApplyConfiguration(new PlaylistConfiguration());
            modelBuilder.ApplyConfiguration(new TrackConfiguration());
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            base.OnModelCreating(modelBuilder);
        }
    }
}
