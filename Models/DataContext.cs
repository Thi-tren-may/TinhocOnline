using Microsoft.EntityFrameworkCore;

namespace TinhocOnline.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        // DbSets
        public DbSet<User> Users { get; set; }
        public DbSet<Topic> Topics { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Exam> Exams { get; set; }
        public DbSet<ExamType> ExamTypes { get; set; }
        public DbSet<ExamQuestion> ExamQuestions { get; set; }
        public DbSet<ExamTopic> ExamTopics { get; set; }
        public DbSet<StudentExam> StudentExams { get; set; }
        public DbSet<StudentAnswer> StudentAnswers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User configurations
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Question configurations
            modelBuilder.Entity<Question>()
                .ToTable(t => t.HasCheckConstraint("CK_Questions_GradeLevel", 
                    "[GradeLevel] IS NULL OR [GradeLevel] IN ('10', '11', '12')"));

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Topic)
                .WithMany(t => t.Questions)
                .HasForeignKey(q => q.TopicId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Question>()
                .HasOne(q => q.Creator)
                .WithMany(u => u.Questions)
                .HasForeignKey(q => q.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Answer configurations
            modelBuilder.Entity<Answer>()
                .HasOne(a => a.Question)
                .WithMany(q => q.Answers)
                .HasForeignKey(a => a.QuestionId)
                .OnDelete(DeleteBehavior.Cascade);

            // Exam configurations
            modelBuilder.Entity<Exam>()
                .ToTable(t => t.HasCheckConstraint("CK_Exams_GradeLevel", 
                    "[GradeLevel] IS NULL OR [GradeLevel] IN ('10', '11', '12')"));

            modelBuilder.Entity<Exam>()
                .ToTable(t => t.HasCheckConstraint("CK_Exams_PassingScore", 
                    "[PassingScore] >= 0 AND [PassingScore] <= 10"));

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.ExamType)
                .WithMany(et => et.Exams)
                .HasForeignKey(e => e.ExamTypeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Exam>()
                .HasOne(e => e.Creator)
                .WithMany(u => u.Exams)
                .HasForeignKey(e => e.CreatedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // ExamType configurations
            modelBuilder.Entity<ExamType>()
                .HasIndex(et => et.TypeCode)
                .IsUnique();

            // ExamQuestion configurations
            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Exam)
                .WithMany(e => e.ExamQuestions)
                .HasForeignKey(eq => eq.ExamId)
                .OnDelete(DeleteBehavior.Cascade);  // Khi Exam bị xóa thì các ExamQuestion liên quan sẽ bị xóa theo

            modelBuilder.Entity<ExamQuestion>()
                .HasOne(eq => eq.Question)
                .WithMany(q => q.ExamQuestions)
                .HasForeignKey(eq => eq.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentExam configurations
            modelBuilder.Entity<StudentExam>()
                .HasOne(se => se.Exam)
                .WithMany(e => e.StudentExams)
                .HasForeignKey(se => se.ExamId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentExam>()
                .HasOne(se => se.Student)
                .WithMany(u => u.StudentExams)
                .HasForeignKey(se => se.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            // StudentAnswer configurations
            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.StudentExam)
                .WithMany(se => se.StudentAnswers)
                .HasForeignKey(sa => sa.StudentExamId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Question)
                .WithMany(q => q.StudentAnswers)
                .HasForeignKey(sa => sa.QuestionId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<StudentAnswer>()
                .HasOne(sa => sa.Answer)
                .WithMany()
                .HasForeignKey(sa => sa.AnswerId)
                .OnDelete(DeleteBehavior.Restrict);

            // ExamTopic configurations
            modelBuilder.Entity<ExamTopic>()
                .HasOne(et => et.Exam)
                .WithMany(e => e.ExamTopics)
                .HasForeignKey(et => et.ExamId)
                .OnDelete(DeleteBehavior.Cascade); // Khi Exam bị xóa thì các ExamTopic liên quan sẽ bị xóa theo

            modelBuilder.Entity<ExamTopic>()
                .HasOne(et => et.Topic)
                .WithMany(t => t.ExamTopics)
                .HasForeignKey(et => et.TopicId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}