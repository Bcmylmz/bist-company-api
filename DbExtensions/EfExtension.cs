using Microsoft.EntityFrameworkCore;


namespace DbExtensions;

    public static class EfExtension
    {

        /*public static Company AddOrUpdate(this DataContext db, Company model)
        {
            try
            {
                if (db.Entry(model).IsKeySet)
                {
                    db.Companies.Add(model);
                }
                else
                {
                    db.Companies.Update(model);
                }
                db.SaveChanges();

                return model;
            }
            catch (Exception ex)
            {

                return new Company();
            }
        }
        public static List<Company> MultipleAddOrUpdate(this DataContext db, List<Company> models)
        {
            try
            {
                List<Company> modelsWithout = models.Where(x => !db.Companies.Contains(x)).ToList();
                List<Company> modelsWith = models.Where(x => db.Companies.Contains(x)).ToList();
                db.Companies.AddRange(modelsWithout);
                db.SaveChanges();
                db.Companies.UpdateRange(modelsWith);
                db.SaveChanges();

                return models;
            }
            catch (Exception ex)
            {

                return new List<Company>();
            }
        }*/
        public static List<T> MultipleAddOrUpdate<T>(this DbContext db, List<T> models) where T : class
        {
            try
            {
                var dbSet = db.Set<T>();

                
                List<T> modelsWithout = models.Where(x => !dbSet.Contains(x)).ToList();
                List<T> modelsWith = models.Where(x => dbSet.Contains(x)).ToList();

                dbSet.AddRange(modelsWithout);
                db.SaveChanges();

                dbSet.UpdateRange(modelsWith);
                db.SaveChanges();

                return models;
            }
            catch (Exception ex)
            {
                
                return new List<T>();
            }
        }

    }



