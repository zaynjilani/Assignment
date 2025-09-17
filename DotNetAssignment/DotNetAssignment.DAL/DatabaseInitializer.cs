using DotNetAssignment.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetAssignment.DAL
{
    public class DatabaseInitializer
    {
        #region Fields
        private readonly DatabaseContext _context;
        #endregion

        #region Constructors
        public DatabaseInitializer(DatabaseContext context)
        {
            _context = context;
        }
        #endregion

        #region Methods
        public void Initialize()
        {
            _context.Database.EnsureCreated();

            // Check if data already exists
            if (_context.GlobalSettings.Any())
            {
                return; // Data already exists
            }

            // Seed data
            _context.GlobalSettings.Add(new GlobalSetting
            {
                Key = "PrivacyPolicy",
                Value = "This is our privacy policy"
            });

            _context.SaveChanges();
        }
        #endregion
    }
}
