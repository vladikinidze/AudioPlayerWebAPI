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
