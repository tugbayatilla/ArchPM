# ArchPM

ArchPm QueryBuilder is helping programmers who want to write sql queries with typesafety. Create an instance of QueryBuilder and write you sql query with your classes and generate your sql query

## Code Samples

you can find how to use



### CREATE AN INSTANCE
```
ArchPM.QBuilder builder = new ArchPM.QBuilder();
OR
var builder = new QBuilder(new TSqlQueryGenerator());
OR
var builder = new QBuilder('YOUR CUSTOM CLASS IMPLENTING ISqlQueryGenerator');

```



### INSERT

```
  INSERT INTO Person (Name) VALUES ('my name');

  
  Person p = new Person();
  p.Name = "my name";
  String query = builder.Insert(p, x => x.Name).ToString();
```

### SELECT

```
   String query = builder.Select<Person>(p => p.Birth).Count<Person>(p => new { MY_COUNT = p.Id }).ToString();
   Assert.AreEqual("SELECT t0.Birth, COUNT(t0.Id) AS [MY_COUNT] FROM Person AS t0", query);
```

```
  String query = builder.Select<Person>(p=>p.Birth).Count<Person>(p => p.Id).ToString();
  Assert.AreEqual("SELECT t0.Birth, COUNT(t0.Id) AS [COUNT] FROM Person AS t0", query);
```

```
 String query = builder.Count<Person>().ToString();
 Assert.AreEqual("SELECT COUNT(*) AS [COUNT] FROM Person AS t0", query);
```

```
  String query = builder.Count<Person>(p => p.Id).ToString();
  Assert.AreEqual("SELECT COUNT(t0.Id) AS [COUNT] FROM Person AS t0", query);
```


```
  String query = builder.Select<Person>().ToString();
  Assert.AreEqual("SELECT t0.* FROM Person AS t0", query);
```


```
  String query = builder.Select<Person>(p => p.Id).ToString();
  Assert.AreEqual("SELECT t0.Id FROM Person AS t0", query);
```

```
  String query = builder.Select<Person>(p => new { p.Id, p.Id2 }).ToString();
  Assert.AreEqual("SELECT t0.Id, t0.Id2 FROM Person AS t0", query);
```

```
  String query = builder
                .Select<Person>(p => new { p.Id, p.Id2 })
                .ChangeTableName<Person>("NewTableName")
                .ToString();

  Assert.AreEqual("SELECT t0.Id, t0.Id2 FROM NewTableName AS t0", query);
```

```
  String query = builder.Select<Person>(p => new { p.Id }).ToString();
  Assert.AreEqual("SELECT t0.Id FROM Person AS t0", query);
```

```
  String query = builder.Select<Person>(p => null).ToString();
  Assert.AreEqual("SELECT t0.* FROM Person AS t0", query);
```

```
  String query = builder.Select<Person>(p => p.Id).Select<Address>(p => p.Id2).ToString();
  Assert.AreEqual("SELECT t0.Id, t1.Id2 FROM Person AS t0, Address AS t1", query);
```

```
  String query = builder.Select<Person>(p => new { p.Id, p.Id2 }).Select<Address>(p => new { p.Id, p.Id2 }).ToString();
  Assert.AreEqual("SELECT t0.Id, t0.Id2, t1.Id, t1.Id2 FROM Person AS t0, Address AS t1", query);
```

```
  String query = builder.Select<Person>(p => new { p.Id, p.Id2 })
                                  .ChangeTableName<Person>("NewTableName")
                                  .Select<Address>(p => new { p.Id, p.Id2 })
                                  .ChangeTableName<Address>("NewTableName2")
                                  .ToString();
  Assert.AreEqual("SELECT t0.Id, t0.Id2, t1.Id, t1.Id2 FROM NewTableName AS t0, NewTableName2 AS t1", query);
```

```
  String query = builder.Select<Person>(p => new { p.Id }).Select<Address>(p => new { AddressId = p.Id }).ToString();
  Assert.AreEqual("SELECT t0.Id, t1.Id AS [AddressId] FROM Person AS t0, Address AS t1", query);
```

```
  String query = builder.Select<Person>(p => new { MyAlias = p.Id }).ToString();
  Assert.AreEqual("SELECT t0.Id AS [MyAlias] FROM Person AS t0", query);
```


### WHERE

```
   var list = EnumManager<Genders>.GetList().Select(p => p.Value);
   String query = builder.Select<Person>().Where<Person>(p => list.Contains((Int32)p.Gender)).ToString();
   Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Gender IN (0,1,2)", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.IsFriendly && p.IsFriendly2.HasValue && (p.Id == 1 || p.Id2 == 2)).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1 AND t0.IsFriendly2 IS NOT NULL AND (t0.Id = 1 OR t0.Id2 = 2)", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.IsFriendly || p.IsFriendly2.HasValue).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly = 1 OR t0.IsFriendly2 IS NOT NULL", query);
```


```
    String query = builder.Select<Person>().Where<Person>(p => !p.IsFriendly && !p.IsFriendly2.HasValue).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.IsFriendly != 1 AND t0.IsFriendly2 IS NULL", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.Id != 1).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id != 1", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.Id2 != null).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NOT NULL", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.Id2 == null).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NULL", query);
```

```
    String query = builder.Select<Person>().Where<Person>(p => p.Id2.HasValue).ToString();
    Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NOT NULL", query);
```

```
   String query = builder.Select<Person>().Where<Person>(p => !p.Id2.HasValue).ToString();
   Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id2 IS NULL", query);
```

```
   String query = builder.Select<Person>().Where<Person>(p => p.Id == 1).ToString();
   Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1", query);
```


```
   String query = builder.Select<Person>().Where<Person>(p => p.Id == 1).ToString();
   Assert.AreEqual("SELECT t0.* FROM Person AS t0 WHERE t0.Id = 1", query);
```


### JOIN

```
   String query = builder.Select<Person>(p => p.Id).Join<Person, Address>((a, b) => a.Id == b.Id).ToString();
   Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id", query);
```

```
   String query = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .ToString();

   Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id LEFT JOIN Address AS t1 ON t0.Id = t1.Id", query);
```

```
   String query = builder
                .Select<Person>(p => p.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Where<Person>(p => p.Salary == 256)
                .ToString();

   Assert.AreEqual("SELECT t0.Id FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id LEFT JOIN Address AS t1 ON t0.Id = t1.Id WHERE t0.Salary = '256'", query);
```

```
   String query = builder
                .Select<Person>(p => p.Id)
                .Select<Address>(p => new { p.Id, p.Description } )
                .Join<Person, Address>((a, b) => a.Id == b.Id)
                .Where<Person>(p => p.Salary == 256)
                .ToString();

   Assert.AreEqual("SELECT t0.Id, t1.Id, t1.Description FROM Person AS t0 LEFT JOIN Address AS t1 ON t0.Id = t1.Id WHERE t0.Salary = '256'", query);
```


### UPDATE

```
   Person person = new Person();
   person.Id = 1;

   String query = builder.Update<Person>(person, p => p.Id, IncludeExclude.Include).Where<Person>(x => x.Id == 1).ToString();
   Assert.AreEqual("UPDATE t0 SET [Id] = 1 FROM Person AS t0 WHERE t0.Id = 1", query);
```

### PAGING

```
   String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, p => p.Id).ToString();

   Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.Id  FROM Person AS t0 WHERE t0.Id = 1) AS t99 WHERE t99.trow BETWEEN 50 AND 60 ORDER BY t99.Id ASC", query);     
```


```
    ParameterExpression pe = Expression.Parameter(typeof(Person), "p");
    var propertyInfo = Extend<Person>.GetPropertyInfoByPropertyName("id");
    Expression property = Expression.Property(pe, propertyInfo);
    Expression<Func<Person, Object>> predicate = p => property;

    String query = builder.Select<Person>(p => p.Id).Where<Person>(p => p.Id == 1).Paging<Person>(5, 10, predicate).ToString();
    Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.Id  FROM Person AS t0 WHERE t0.Id = 1) AS t99 WHERE t99.trow BETWEEN 50 AND 60 ORDER BY t99.Id ASC", query);        
```


```
    String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Id).ToString();

    Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Id ASC) AS trow, t0.* FROM Person AS t0) AS t99 WHERE t99.trow BETWEEN 200 AND 220 ORDER BY t99.Id ASC", query);
```

```
     String query = builder.Select<Person>().Paging<Person>(10, 20, x => x.Gender).ToString();

     Assert.AreEqual("SELECT * FROM (SELECT ROW_NUMBER() OVER(ORDER BY t0.Gender ASC) AS trow, t0.* FROM Person AS t0) AS t99 WHERE t99.trow BETWEEN 200 AND 220 ORDER BY t99.Gender ASC", query);
```

### SAMPLE CLASS'

```

    class SmallTable
    {
        public Int32 Id42 { get; set; }
        public Int32 Id2 { get; set; }
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public Decimal Salary { get; set; }
    }

    class SmallTableInherited : SmallTable
    {
        public Int32 Age { get; set; }
    }


    class Person
    {
        public Int32 Id { get; set; }
        public Int32? Id2 { get; set; }
        public Int64 Height { get; set; }
        public Int64? Height2 { get; set; }
        public Int16 Weight { get; set; }
        public Int16? Weight2 { get; set; }
        public String Name { get; set; }
        public String Name2 { get; set; }
        public Decimal Salary { get; set; }
        public Decimal? Salary2 { get; set; }
        public Genders Gender { get; set; }
        public Genders? Gender2 { get; set; }
        public Fears Fear { get; set; }
        public Fears? Fear2 { get; set; }
        public Boolean IsFriendly { get; set; }
        public Boolean? IsFriendly2 { get; set; }
        public IMyInterface MyInterface { get; set; }
        public IMyInterface MyInterface2 { get; set; }
        public MyClass Myclass { get; set; }
        public MyClass Myclass2 { get; set; }
        public DateTime Birth { get; set; }
        public DateTime? Birth2 { get; set; }
    }

    class Address
    {
        public Int32 Id { get; set; }
        public Int32? Id2 { get; set; }
        public Int32 PersonId { get; set; }
        public Int32? PersonId2 { get; set; }
        public Int64 Size { get; set; }
        public Int64? Size2 { get; set; }
        public String Name { get; set; }
        public String Name2 { get; set; }
        public String Description { get; set; }
        public String Description2 { get; set; }
        public DateTime MovingDate { get; set; }
        public DateTime? MovingDate2 { get; set; }
    }

    interface IMyInterface
    {
        Int32 Id { get; set; }
    } 

    class MyInterfaceClass : IMyInterface
    {
        public Int32 Id { get; set; }
    }

    class MyClass
    { }

    enum Genders
    {
        Male,
        Female,
        Other
    }

    enum Fears : byte
    {
        Dark = 0,
        Alone = 1,
        Cat = 2
    }


```



## License

This project is licensed - see the [LICENSE.md](LICENSE.md) file for details

## Acknowledgments

* waiting your comments and 
* Inspiration
* 
